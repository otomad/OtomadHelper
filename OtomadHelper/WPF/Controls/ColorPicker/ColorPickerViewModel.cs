using System.Windows.Media;

using Wacton.Unicolour;

using ThreeD = (double X, double Y, double Z);
using ThreeDRange = ((double Min, double Max) X, (double Min, double Max) Y, (double Min, double Max) Z);
using Range = (double Min, double Max);

namespace OtomadHelper.WPF.Controls;

public partial class ColorPickerViewModel : ObservableObject<ColorPicker> {
	[ObservableProperty]
	private ColorPickerModelAxis modelAxis = new(ColourSpace.Hsl, 2);

	[ObservableProperty]
	private Unicolour color = new(ColourSpace.Rgb255, 0, 0, 0);

	private static readonly ColourSpace[] KnownModels =
		[ColourSpace.Hsl, ColourSpace.Rgb255, ColourSpace.Hsb, ColourSpace.Hwb, ColourSpace.Oklab, ColourSpace.Oklch];
	private bool isColorChanging = false;
	partial void OnColorChanged(Unicolour color) {
		lock (this) {
			if (isColorChanging) return;
			isColorChanging = true;
			//Values["Alpha"] = color.Alpha.A255;
			foreach (ColourSpace model in KnownModels) {
				ThreeD triplet = ToTriplet(model);
				ThreeDRange inputRange = GetInputRange(model), outputRange = GetOutputRange(model);
				Values[new(model, 0)] = MathEx.ClampMap(triplet.X, outputRange.X.Min, outputRange.X.Max, inputRange.X.Min, inputRange.X.Max);
				Values[new(model, 1)] = MathEx.ClampMap(triplet.Y, outputRange.Y.Min, outputRange.Y.Max, inputRange.Y.Min, inputRange.Y.Max);
				Values[new(model, 2)] = MathEx.ClampMap(triplet.Z, outputRange.Z.Min, outputRange.Z.Max, inputRange.Z.Min, inputRange.Z.Max);
			}
			OnPropertyChanged(nameof(Values));
			UpdateSources();
			isColorChanging = false;
		}
	}

	partial void OnModelAxisChanged(ColorPickerModelAxis value) {
		UpdateSources();
	}

	private ThreeD ToTriplet(ColourSpace model) {
		ColourRepresentation representation = model switch {
			ColourSpace.Rgb255 => Color.Rgb.Byte255,
			ColourSpace.Hsl => Color.Hsl,
			ColourSpace.Hsb => Color.Hsb,
			ColourSpace.Hwb => Color.Hwb,
			ColourSpace.Lab => Color.Lab,
			ColourSpace.Lchab => Color.Lchab,
			ColourSpace.Oklab => Color.Oklab,
			ColourSpace.Oklch => Color.Oklch,
			_ => throw new NotImplementedException(),
		};
		ThreeD triplet = representation.Triplet.Tuple;
		return triplet;
	}

	[RelayCommand]
	public void CheckModelAxis(string name) =>
		ModelAxis = ColorPickerModelAxis.FromName(name);

	[ObservableProperty]
	private Dictionary<ColorPickerModelAxis, double> values = [];

	private bool isTextChanging = false;
	[RelayCommand]
	public void TextChanged((string text, string name) tuple) {
		lock (this) {
			if (isTextChanging) return;
			isTextChanging = true;
			double alpha = Color.Alpha.A;
			(ColourSpace model, int axis) = ColorPickerModelAxis.FromName(tuple.name);
			//ColourSpace model = modelAxis.Model;
			//int axis = modelAxis
			if (!double.TryParse(tuple.text, out double value)) return;
			double[] triplet = ToTriplet(model).ToArray<double>();
			Range inputRange = GetInputRange(model).Get<Range>(axis), outputRange = GetOutputRange(model).Get<Range>(axis);
			triplet[axis] = MathEx.Map(value, inputRange.Min, inputRange.Max, outputRange.Min, outputRange.Max);
			Color = new(model, triplet[0], triplet[1], triplet[2], alpha);
			isTextChanging = false;
		}
	}

	private const int SOURCE_RESOLUTION = 32;

	[ObservableProperty]
	private WriteableBitmap primarySource = new(SOURCE_RESOLUTION, SOURCE_RESOLUTION, 96, 96, PixelFormats.Rgb24, null);
	[ObservableProperty]
	private WriteableBitmap secondarySource = new(1, SOURCE_RESOLUTION, 96, 96, PixelFormats.Rgb24, null);

	private void UpdateSources() { // FIXME: So stuck.
		const int CHANNEL = 3;
		Tuple<double, double, double> tuple = ToTriplet(ModelAxis.Model).ToTuple();
		ThreeDRange range = GetOutputRange(ModelAxis.Model);
		{
			int width = PrimarySource.PixelWidth, height = PrimarySource.PixelHeight, stride = PrimarySource.BackBufferStride;
			unsafe {
				byte* pixels = (byte*)PrimarySource.BackBuffer.ToPointer();
				PrimarySource.Lock();
				//byte[] pixels = new byte[height * width * CHANNEL];
				for (int row = 0; row < height; row++) {
					for (int col = 0; col < width; col++) {
						int i = row * stride + col * CHANNEL;
						ThreeD triplet = tuple.ToValueTuple();
						switch (ModelAxis.Axis) {
							case 0:
								triplet.Y = MathEx.Map(col, 0, width - 1, range.Y.Min, range.Y.Max);
								triplet.Z = MathEx.Map(row, 0, height - 1, range.Z.Max, range.Z.Min);
								break;
							case 1:
								triplet.X = MathEx.Map(col, 0, width - 1, range.X.Min, range.X.Max);
								triplet.Z = MathEx.Map(row, 0, height - 1, range.Z.Max, range.Z.Min);
								break;
							case 2:
								triplet.X = MathEx.Map(col, 0, width - 1, range.X.Min, range.X.Max);
								triplet.Y = MathEx.Map(row, 0, height - 1, range.Y.Max, range.Y.Min);
								break;
							default:
								continue;
						}
						Unicolour color = new(ModelAxis.Model, triplet);
						Rgb255 rgb = color.Rgb.Byte255;
						pixels[i] = (byte)rgb.ConstrainedR;
						pixels[i + 1] = (byte)rgb.ConstrainedG;
						pixels[i + 2] = (byte)rgb.ConstrainedB;
					}
				}
				//PrimarySource.WritePixels(new(0, 0, width, height), pixels, PrimarySource.BackBufferStride, 0);
				PrimarySource.AddDirtyRect(new(0, 0, width, height));
				PrimarySource.Unlock();
			}
		}
		{
			int width = SecondarySource.PixelWidth, height = SecondarySource.PixelHeight, stride = SecondarySource.BackBufferStride;
			unsafe {
				byte* pixels = (byte*)SecondarySource.BackBuffer;
				SecondarySource.Lock();
				//byte[] pixels = new byte[height * 1 * CHANNEL];
				for (int row = 0; row < height; row++) {
					int i = row * stride;
					ThreeD triplet = tuple.ToValueTuple();
					switch (ModelAxis.Axis) {
						case 0:
							triplet.X = MathEx.Map(row, 0, height - 1, range.X.Max, range.X.Min);
							break;
						case 1:
							triplet.Y = MathEx.Map(row, 0, height - 1, range.Y.Max, range.Y.Min);
							break;
						case 2:
							triplet.Z = MathEx.Map(row, 0, height - 1, range.Z.Max, range.Z.Min);
							break;
						default:
							continue;
					}
					Unicolour color = new(ModelAxis.Model, triplet);
					Rgb255 rgb = color.Rgb.Byte255;
					pixels[i] = (byte)rgb.ConstrainedR;
					pixels[i + 1] = (byte)rgb.ConstrainedG;
					pixels[i + 2] = (byte)rgb.ConstrainedB;
				}
				//SecondarySource.WritePixels(new(0, 0, 1, height), pixels, CHANNEL, 0);
				SecondarySource.AddDirtyRect(new(0, 0, width, height));
				SecondarySource.Unlock();
			}
		}
	}

	private static ThreeDRange GetOutputRange(ColourSpace model) {
		return model switch {
			ColourSpace.Rgb255 => ((0, 255), (0, 255), (0, 255)),
			ColourSpace.Hsl => ((0, 360), (0, 1), (0, 1)),
			ColourSpace.Hsb => ((0, 360), (0, 1), (0, 1)),
			ColourSpace.Hwb => ((0, 360), (0, 1), (0, 1)),
			ColourSpace.Oklab => ((0, 1), (-0.5, 0.5), (-0.5, 0.5)),
			ColourSpace.Oklch => ((0, 1), (0, 0.5), (0, 360)),
			_ => throw new NotImplementedException(),
		};
	}

	private static ThreeDRange GetInputRange(ColourSpace model) {
		return model switch {
			ColourSpace.Rgb255 => ((0, 255), (0, 255), (0, 255)),
			ColourSpace.Hsl => ((0, 360), (0, 100), (0, 100)),
			ColourSpace.Hsb => ((0, 360), (0, 100), (0, 100)),
			ColourSpace.Hwb => ((0, 360), (0, 100), (0, 100)),
			ColourSpace.Oklab => ((0, 100), (-128, 128), (-128, 128)),
			ColourSpace.Oklch => ((0, 100), (0, 230), (0, 360)),
			_ => throw new NotImplementedException(),
		};
	}
}

public struct ColorPickerModelAxis(ColourSpace model, int axis) {
	public ColourSpace Model { get; set; } = model;
	public int Axis { get; set; } = axis;

	public static bool operator ==(ColorPickerModelAxis item1, ColorPickerModelAxis item2) =>
		item1.Model == item2.Model && item1.Axis == item2.Axis;
	public static bool operator !=(ColorPickerModelAxis item1, ColorPickerModelAxis item2) => !(item1 == item2);
	public override bool Equals(object obj) => obj is ColorPickerModelAxis item && this == item;
	public override int GetHashCode() => Model.GetHashCode() ^ Axis.GetHashCode();

	public static ColorPickerModelAxis FromName(string name) {
		string[] splitted = name.Split('_', '.');
		return new(
			splitted[0].ToUpperInvariant() switch {
				"RGB" => ColourSpace.Rgb255,
				"HSL" => ColourSpace.Hsl,
				"HSB" => ColourSpace.Hsb,
				"HWB" => ColourSpace.Hwb,
				"LAB" => ColourSpace.Lab,
				"LCH" => ColourSpace.Lchab,
				"OKLAB" => ColourSpace.Oklab,
				"OKLCH" => ColourSpace.Oklch,
				_ => throw new NotImplementedException(),
			},
			int.Parse(splitted[1])
		);
	}

	internal void Deconstruct(out ColourSpace model, out int axis) {
		model = Model;
		axis = Axis;
	}
}
