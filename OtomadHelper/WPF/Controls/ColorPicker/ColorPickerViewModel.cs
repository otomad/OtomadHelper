using System.Windows;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

public partial class ColorPickerViewModel : ObservableObject<ColorPicker> {
	[ObservableProperty]
	private ColorPickerModelAxis modelAxis = new(ColourSpace.Hsl, 2);

	[ObservableProperty]
	private Unicolour color = new(ColourSpace.Rgb255, 0, 0, 0);

	private static readonly ColourSpace[] KnownModels =
		[ColourSpace.Hsl, ColourSpace.Rgb255, ColourSpace.Hsb, ColourSpace.Hwb, ColourSpace.Oklab, ColourSpace.Oklch];
	partial void OnColorChanged(Unicolour color) {
		//Values["Alpha"] = color.Alpha.A255;
		foreach (ColourSpace model in KnownModels) {
			ColourTriplet triplet = ToTriplet(model);
			Values[new(model, 0)] = triplet.First;
			Values[new(model, 1)] = triplet.Second;
			Values[new(model, 2)] = triplet.Third;
		}
		OnPropertyChanged(nameof(Values));
		if (timer.IsStart) timer.Stop();
		timer.SingleShot();
	}

	partial void OnModelAxisChanged(ColorPickerModelAxis value) {
		if (timer.IsStart) timer.Stop();
		timer.SingleShot();
	}

	private ColourTriplet ToTriplet(ColourSpace model) {
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
		ColourTriplet triplet = representation.Triplet;
		return triplet;
	}

	[RelayCommand]
	public void CheckModelAxis(string name) =>
		ModelAxis = ColorPickerModelAxis.FromName(name);

	[ObservableProperty]
	private Dictionary<ColorPickerModelAxis, double> values = [];

	[RelayCommand]
	public void TextChanged((string text, string name) tuple) {
		double alpha = Color.Alpha.A;
		ColorPickerModelAxis modelAxis = ColorPickerModelAxis.FromName(tuple.name);
		if (!double.TryParse(tuple.text, out double value)) return;
		double[] triplet = ToTriplet(modelAxis.Model).ToArray();
		triplet[modelAxis.Axis] = value;
		Color = new(modelAxis.Model, triplet[0], triplet[1], triplet[2], alpha);
	}

	[ObservableProperty]
	private WriteableBitmap primarySource = new(128, 128, 96, 96, PixelFormats.Rgb24, null);
	[ObservableProperty]
	private WriteableBitmap secondarySource = new(1, 128, 96, 96, PixelFormats.Rgb24, null);

	private readonly ITimer timer;
	public ColorPickerViewModel() : base() {
		timer = new ITimer.WinForm(UpdateSources, 10);
	}

	private void UpdateSources() { // FIXME: So stuck.
		const int CHANNEL = 3;
		ColourTriplet triplet = ToTriplet(ModelAxis.Model);
		var range = GetRange(ModelAxis.Model);
		{
			int width = PrimarySource.PixelWidth, height = PrimarySource.PixelHeight;
			unsafe {
				byte* pixels = (byte*)PrimarySource.BackBuffer.ToPointer();
				PrimarySource.Lock();
				//byte[] pixels = new byte[height * width * CHANNEL];
				for (int row = 0; row < height; row++) {
					for (int col = 0; col < width; col++) {
						int i = row * width * CHANNEL + col * CHANNEL;
						(double, double, double) tuple = triplet.Tuple;
						switch (ModelAxis.Axis) {
							case 0:
								tuple.Item2 = MathEx.Map(col, 0, width - 1, range.Item2.min, range.Item2.max);
								tuple.Item3 = MathEx.Map(row, 0, height - 1, range.Item3.max, range.Item3.min);
								break;
							case 1:
								tuple.Item1 = MathEx.Map(col, 0, width - 1, range.Item1.min, range.Item1.max);
								tuple.Item3 = MathEx.Map(row, 0, height - 1, range.Item3.max, range.Item3.min);
								break;
							case 2:
								tuple.Item1 = MathEx.Map(col, 0, width - 1, range.Item1.min, range.Item1.max);
								tuple.Item2 = MathEx.Map(row, 0, height - 1, range.Item2.max, range.Item2.min);
								break;
							default:
								continue;
						}
						Unicolour color = new(ModelAxis.Model, tuple);
						pixels[i] = (byte)color.Rgb.Byte255.R;
						pixels[i + 1] = (byte)color.Rgb.Byte255.G;
						pixels[i + 2] = (byte)color.Rgb.Byte255.B;
					}
				}
				//PrimarySource.WritePixels(new Int32Rect(0, 0, width, height), pixels, PrimarySource.BackBufferStride, 0);
				PrimarySource.AddDirtyRect(new Int32Rect(0, 0, width, height));
				PrimarySource.Unlock();
			}
		}
		{
			int height = SecondarySource.PixelHeight;
			unsafe {
				//byte* pixels = (byte*)SecondarySource.BackBuffer.ToPointer();
				//SecondarySource.Lock();
				byte[] pixels = new byte[height * 1 * CHANNEL];
				for (int row = 0; row < height; row++) {
					int i = row * CHANNEL;
					(double, double, double) tuple = triplet.Tuple;
					switch (ModelAxis.Axis) {
						case 0:
							tuple.Item1 = MathEx.Map(row, 0, height - 1, range.Item1.max, range.Item1.min);
							break;
						case 1:
							tuple.Item2 = MathEx.Map(row, 0, height - 1, range.Item2.max, range.Item2.min);
							break;
						case 2:
							tuple.Item3 = MathEx.Map(row, 0, height - 1, range.Item3.max, range.Item3.min);
							break;
						default:
							continue;
					}
					Unicolour color = new(ModelAxis.Model, tuple);
					pixels[i] = (byte)color.Rgb.Byte255.R;
					pixels[i + 1] = (byte)color.Rgb.Byte255.G;
					pixels[i + 2] = (byte)color.Rgb.Byte255.B;
				}
				SecondarySource.WritePixels(new Int32Rect(0, 0, 1, height), pixels, CHANNEL, 0);
				//SecondarySource.AddDirtyRect(new Int32Rect(0, 0, 1, height));
				//SecondarySource.Unlock();
			}
		}
	}

	private static ((double min, double max), (double min, double max), (double min, double max)) GetRange(ColourSpace model) {
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
}
