using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(ColorPickerModelAxis), typeof(bool))]
public class ColorPickerModelAxisToCheckedConverter : ValueConverter<ColorPickerModelAxis, bool, ColorPickerModelAxis> {
	public override bool Convert(ColorPickerModelAxis modelAxis, Type targetType, ColorPickerModelAxis parameter, CultureInfo culture) =>
		modelAxis == parameter;

	public override ColorPickerModelAxis ConvertBack(bool value, Type targetType, ColorPickerModelAxis parameter, CultureInfo culture) => parameter;
}

[ValueConversion(typeof(Dictionary<ColorPickerModelAxis, double>), typeof(ColorPickerModelAxis))]
public class ColorPickerValuesToTextConverter : ValueConverter<Dictionary<ColorPickerModelAxis, double>, string, ColorPickerModelAxis> {
	public override string Convert(Dictionary<ColorPickerModelAxis, double> dictionary, Type targetType, ColorPickerModelAxis parameter, CultureInfo culture) {
		try {
			return dictionary.TryGetValue(parameter, out double result) ? Math.Round(result).ToString() : string.Empty;
		} catch (Exception) {
			return string.Empty;
		}
	}
}

[ValueConversion(typeof(TextChangedEventArgs), typeof(ValueTuple<string, ColorPickerModelAxis>?))]
public partial class ColorPickerTextChangedEventArgsToTextAndModelAxisConverter : ValueConverter<TextChangedEventArgs, (string text, ColorPickerModelAxis modelAxis)?> {
	public override (string text, ColorPickerModelAxis modelAxis)? Convert(TextChangedEventArgs e, Type targetType, object parameter, CultureInfo culture) {
		TextBox textBox = (TextBox)e.Source;
		return (textBox.Text, ColorPicker.GetModelAxis(textBox)!);
	}
}

[ValueConversion(typeof(Unicolour), typeof(Color))]
public class UnicolourToMediaColorConverter : ValueConverter<Unicolour, Color, double?> {
	public override Color Convert(Unicolour unicolour, Type targetType, double? alpha, CultureInfo culture) {
		Color color = unicolour.ToMediaColor();
		if (alpha is { } a) color.A = (byte)(a * 255);
		return color;
	}

	public override Unicolour ConvertBack(Color color, Type targetType, double? parameter, CultureInfo culture) =>
		color.ToUnicolour();
}

public class TrackThumbInnerBaseMultiplySizeConverter : MultiValueConverter<double[], double> {
	public override double Convert(double[] values, Type targetType, object parameter, CultureInfo culture) =>
		values.ToArray().Aggregate(1d, (a, b) => a * b);
}

[ValueConversion(typeof(ColorPickerModelAxis), typeof(Range?))]
public class TextBoxModelAxisToRangeConverter : ValueConverter<ColorPickerModelAxis, Range?> {
	public override Range? Convert(ColorPickerModelAxis modelAxis, Type targetType, object parameter, CultureInfo culture) {
		try {
			(ColourSpace model, int axis) = modelAxis;
			(Range X, Range Y, Range Z) ranges = ColorPickerViewModel.GetInputRange(model);
			return axis switch {
				0 => ranges.X,
				1 => ranges.Y,
				2 => ranges.Z,
				_ => null,
			};
		} catch (Exception) {
			return null;
		}
	}
}

[ValueConversion(typeof(int), typeof(int))]
public class Alpha255ToAlpha100Converter : ValueConverter<int, int> {
	public override int Convert(int value, Type targetType, object parameter, CultureInfo culture) =>
		(int)Math.Round((double)value / 255 * 100);
}

[ValueConversion(typeof(ColorPickerModelAxis), typeof(string))]
public class ColorPickerModelAxisToTranslationConverter : ValueConverter<ColorPickerModelAxis, string, bool> {
	public override string Convert(ColorPickerModelAxis value, Type targetType, bool useLongName, CultureInfo culture) =>
		ColorPickerModelAxis.GetAxisDisplayName(value.GetAxisI18nKey(), useLongName);
}
