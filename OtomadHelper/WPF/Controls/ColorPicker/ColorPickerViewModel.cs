using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Wacton.Unicolour;

using ThreeD = (double X, double Y, double Z);
using ThreeDRange = ((double Min, double Max) X, (double Min, double Max) Y, (double Min, double Max) Z);

namespace OtomadHelper.WPF.Controls;

public partial class ColorPickerViewModel : ObservableObject<ColorPicker> {
	[ObservableProperty]
	private ColorPickerModelAxis modelAxis = new(ColourSpace.Hsl, 2);

	[ObservableProperty]
	private Unicolour color = new(ColourSpace.Rgb255, 0, 0, 0);

	private static readonly ColourSpace[] KnownModels =
		[ColourSpace.Hsl, ColourSpace.Rgb255, ColourSpace.Hsb, ColourSpace.Hwb, ColourSpace.Oklab, ColourSpace.Oklch];
	private bool isColorChanging = false;
	internal void InitialColor() => OnColorChanged(null, Color);
	partial void OnColorChanged(Unicolour? prevColor, Unicolour color) {
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
			Alpha = color.Alpha.A255;
			IList<string> hexes = ToHex(color);
			if (!hexes.Contains(Hex)) Hex = hexes[0];
			UpdateSourcesBehavior behavior = UpdateSourcesBehavior.UpdateBoth;
			if (prevColor is not null && isInit) {
				(ColourSpace model, int axis) = ModelAxis;
				int z = GetPointXyz(2);
				double[] prevTriplet = ToTriplet(prevColor, model).ToArray<double>(), triplet = ToTriplet(color, model).ToArray<double>();
				if (prevTriplet[axis] == triplet[axis]) behavior &= ~UpdateSourcesBehavior.UpdatePrimary;
				if (prevTriplet.Keys().All(i => i == axis ? true : prevTriplet[i] == triplet[i])) behavior &= ~UpdateSourcesBehavior.UpdateSecondary;
			}
			UpdateSources(behavior);
			View?.SetAccentColor(Color.ToMediaColor());
			isInit = true;
			isColorChanging = false;
		}
	}
	private bool isInit = false;

	partial void OnModelAxisChanged(ColorPickerModelAxis value) {
		UpdateSources();
		UpdateThumbsBinding();
	}

	internal static ThreeD ToTriplet(Unicolour color, ColourSpace model) {
		ColourRepresentation representation = model switch {
			ColourSpace.Rgb255 => color.Rgb.Byte255,
			ColourSpace.Hsl => color.Hsl,
			ColourSpace.Hsb => color.Hsb,
			ColourSpace.Hwb => color.Hwb,
			ColourSpace.Lab => color.Lab,
			ColourSpace.Lchab => color.Lchab,
			ColourSpace.Oklab => color.Oklab,
			ColourSpace.Oklch => color.Oklch,
			_ => throw new NotImplementedException(),
		};
		ThreeD triplet = representation.ConstrainedTriplet.Tuple;
		return triplet;
	}

	private ThreeD ToTriplet(ColourSpace model) =>
		ToTriplet(Color, model);

	[RelayCommand]
	private void CheckModelAxis(string name) =>
		ModelAxis = ColorPickerModelAxis.FromName(name);

	[RelayCommand]
	private void GetColorFromScreen(Color color) =>
		Color = color.ToUnicolour();

	[ObservableProperty]
	private Dictionary<ColorPickerModelAxis, double> values = [];
	[ObservableProperty]
	private int alpha = 255;
	[ObservableProperty]
	private string hex = "000000";

	private bool isTextChanging = false;
	[RelayCommand]
	private void TextChanged((string Text, string Name) e) {
		lock (this) {
			if (isTextChanging) return;
			isTextChanging = true;
			if (e.Name is "A255" or "A100" or "HEX") {
				if (e.Name == "HEX") {
					Unicolour? color = FromHex(e.Text);
					if (color is not null) {
						Color = color;
						Hex = e.Text;
					}
				} else if (double.TryParse(e.Text, out double alpha)) {
					alpha = alpha / (e.Name == "A255" ? 255 : 100);
					ColourSpace model = ModelAxis.Model;
					Color = new(model, ToTriplet(model), alpha);
				}
			} else {
				double alpha = Color.Alpha.A;
				(ColourSpace model, int axis) = ColorPickerModelAxis.FromName(e.Name);
				if (double.TryParse(e.Text, out double value)) {
					double[] triplet = ToTriplet(model).ToArray<double>();
					Range inputRange = GetInputRange(model).Get<Range>(axis), outputRange = GetOutputRange(model).Get<Range>(axis);
					triplet[axis] = MathEx.Map(value, inputRange.Min, inputRange.Max, outputRange.Min, outputRange.Max);
					Color = new(model, triplet[0], triplet[1], triplet[2], alpha);
				}
			}
			isTextChanging = false;
		}
	}

	internal void UpdateThumbsBinding() {
		if (View is null) return;
		ThreeDRange range = GetInputRange(ModelAxis.Model);

		TextBox? GetTextBox(int xyzIndex) => View?.FindForm<TextBox>(new ColorPickerModelAxis(ModelAxis.Model, GetPointXyz(xyzIndex)).ToString());

		View.PointXy.XRange = range.Get<Range>(GetPointXyz(0));
		View.PointXy.YRange = range.Get<Range>(GetPointXyz(1));
		View.PointZ.YRange = range.Get<Range>(GetPointXyz(2));
		View.PointA.YRange = (0, 255);
		BindingOperations.SetBinding(View.PointXy, ColorTrackThumb.XProperty, new Binding("Text") { Source = GetTextBox(0), Mode = BindingMode.OneWay });
		BindingOperations.SetBinding(View.PointXy, ColorTrackThumb.YProperty, new Binding("Text") { Source = GetTextBox(1), Mode = BindingMode.OneWay });
		BindingOperations.SetBinding(View.PointZ, ColorTrackThumb.YProperty, new Binding("Text") { Source = GetTextBox(2), Mode = BindingMode.OneWay });
		BindingOperations.SetBinding(View.PointA, ColorTrackThumb.YProperty, new Binding("Text") { Source = View?.FindForm<TextBox>("A255"), Mode = BindingMode.OneWay });
		// BindingMode.TwoWay will break binding, so we use TextBox.SetCurrentValue instead.
	}

	[RelayCommand]
	public void ThumbDragged(ColorTrackThumbDraggingRoutedEventArgs e) {
		ColorTrackThumb thumb = (ColorTrackThumb)e.Source;
		TextBox? x = BindingOperations.GetBinding(thumb, ColorTrackThumb.XProperty)?.Source as TextBox;
		TextBox? y = BindingOperations.GetBinding(thumb, ColorTrackThumb.YProperty)?.Source as TextBox;
		x?.SetCurrentValue(TextBox.TextProperty, Math.Round(e.X).ToString());
		y?.SetCurrentValue(TextBox.TextProperty, Math.Round(e.Y).ToString());
	}

	private const int SOURCE_RESOLUTION = 32;

	[ObservableProperty]
	private WriteableBitmap primarySource = new(SOURCE_RESOLUTION, SOURCE_RESOLUTION, 96, 96, PixelFormats.Rgb24, null);
	[ObservableProperty]
	private WriteableBitmap secondarySource = new(1, SOURCE_RESOLUTION, 96, 96, PixelFormats.Rgb24, null);

	[Flags]
	private enum UpdateSourcesBehavior {
		DoNotUpdate = 0,
		UpdatePrimary = 1,
		UpdateSecondary = 2,
		UpdateBoth = UpdatePrimary | UpdateSecondary,
	}

	private void UpdateSources(UpdateSourcesBehavior behavior = UpdateSourcesBehavior.UpdateBoth) { // FIXME: So stuck.
		const int CHANNEL = 3;
		Tuple<double, double, double> tuple = ToTriplet(ModelAxis.Model).ToTuple();
		ThreeDRange range = GetOutputRange(ModelAxis.Model);
		if ((behavior & UpdateSourcesBehavior.UpdatePrimary) != 0) {
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
		if ((behavior & UpdateSourcesBehavior.UpdateSecondary) != 0) {
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

	internal static ThreeDRange GetOutputRange(ColourSpace model) {
		return model switch {
			ColourSpace.Rgb255 => ((0, 255), (0, 255), (0, 255)),
			ColourSpace.Hsl => ((0, 359), (0, 1), (0, 1)),
			ColourSpace.Hsb => ((0, 359), (0, 1), (0, 1)),
			ColourSpace.Hwb => ((0, 359), (0, 1), (0, 1)),
			ColourSpace.Oklab => ((0, 1), (-0.5, 0.5), (-0.5, 0.5)),
			ColourSpace.Oklch => ((0, 1), (0, 0.5), (0, 359)),
			_ => throw new NotImplementedException(),
		};
	}

	internal static ThreeDRange GetInputRange(ColourSpace model) {
		return model switch {
			ColourSpace.Rgb255 => ((0, 255), (0, 255), (0, 255)),
			ColourSpace.Hsl => ((0, 359), (0, 100), (0, 100)),
			ColourSpace.Hsb => ((0, 359), (0, 100), (0, 100)),
			ColourSpace.Hwb => ((0, 359), (0, 100), (0, 100)),
			ColourSpace.Oklab => ((0, 100), (-128, 128), (-128, 128)),
			ColourSpace.Oklch => ((0, 100), (0, 230), (0, 359)),
			_ => throw new NotImplementedException(),
		};
	}

	public static int GetPointXyz(int xyzIndex, int axis) {
		List<int> xyzMap = [0, 1, 2];
		xyzMap.Remove(axis);
		xyzMap.Add(axis);
		return xyzMap[xyzIndex];
	}
	public int GetPointXyz(int xyzIndex) => GetPointXyz(xyzIndex, ModelAxis.Axis);

	public static IList<string> ToHex(Unicolour color) {
		// color.Hex missing alpha
		byte r = (byte)color.Rgb.Byte255.ConstrainedR, g = (byte)color.Rgb.Byte255.ConstrainedG,
			b = (byte)color.Rgb.Byte255.ConstrainedB, a = (byte)color.Alpha.A255;
		string xr = r.ToString("X2"), xg = g.ToString("X2"), xb = b.ToString("X2"), xa = a.ToString("X2");
		List<string> results = new(4);
		if (a == 255) results.Add(xr + xg + xb);
		results.Add(xr + xg + xb + xa);
		if (r % 0x11 == 0 && g % 0x11 == 0 && b % 0x11 == 0) {
			if (a == 255) results.Add(new([xr[0], xg[0], xb[0]]));
			if (a % 0x11 == 0) results.Add(new([xr[0], xg[0], xb[0], xa[0]]));
		}
		return results;
	}

	public static Unicolour? FromHex(string hex) {
		hex = hex.TrimStart('#');
		if (hex.Length is not (3 or 6 or 4 or 8)) return null;
		if (hex.IsMatch(new(@"[^0-9A-F]", RegexOptions.IgnoreCase))) return null;

		static string RepeatTwice(string source) {
			StringBuilder sb = new();
			foreach (char c in source) {
				sb.Append(c);
				sb.Append(c);
			}
			return sb.ToString();
		}

		if (hex.Length is 3 or 4) hex = RepeatTwice(hex);
		return new(hex);
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

	internal static readonly Dictionary<string, ColourSpace> NameModelMap = new() {
		["RGB"] = ColourSpace.Rgb255,
		["HSL"] = ColourSpace.Hsl,
		["HSB"] = ColourSpace.Hsb,
		["HWB"] = ColourSpace.Hwb,
		["LAB"] = ColourSpace.Lab,
		["LCH"] = ColourSpace.Lchab,
		["OKLAB"] = ColourSpace.Oklab,
		["OKLCH"] = ColourSpace.Oklch,
	};

	public static ColorPickerModelAxis FromName(string name) {
		string[] splitted = name.Split('_', '.');
		return new(
			NameModelMap[splitted[0].ToUpperInvariant()],
			int.Parse(splitted[1])
		);
	}

	public override string ToString() => $"{NameModelMap.GetKeyByValue(Model)}.{Axis}";

	internal void Deconstruct(out ColourSpace model, out int axis) {
		model = Model;
		axis = Axis;
	}
}
