using System.Collections.ObjectModel;

using DrWPF.Windows.Data;

using Wacton.Unicolour;

using static Wacton.Unicolour.Cam;

namespace OtomadHelper.WPF.Controls;

public partial class ColorPickerViewModel : ObservableObject<ColorPicker> {
	[ObservableProperty]
	private ColorPickerModelAxis modelAxis = new(ColourSpace.Hsl, 2);

	[ObservableProperty]
	private Unicolour color = new(ColourSpace.Rgb255, 0, 0, 0);

	private static readonly ColourSpace[] KnownModels = [ColourSpace.Hsl, ColourSpace.Rgb255];
	partial void OnColorChanged(Unicolour color) {
		//Values["Alpha"] = color.Alpha.A255;
		foreach (ColourSpace model in KnownModels) {
			ColourTriplet triplet = ToTriplet(model);
			Values[new(model, 0)] = triplet.First;
			Values[new(model, 1)] = triplet.Second;
			Values[new(model, 2)] = triplet.Third;
		}
		OnPropertyChanged(nameof(Values));
	}

	private ColourTriplet ToTriplet(ColourSpace model) {
		ColourRepresentation representation = model switch {
			ColourSpace.Rgb255 => Color.Rgb.Byte255,
			ColourSpace.Hsl => Color.Hsl,
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
