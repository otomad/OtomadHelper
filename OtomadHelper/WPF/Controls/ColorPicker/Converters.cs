using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(ColorPickerModelAxis), typeof(bool))]
public class ColorPickerModelAxisToCheckedConverter : ValueConverter<ColorPickerModelAxis, bool, string> {
	public override bool Convert(ColorPickerModelAxis modelAxis, Type targetType, string parameter, CultureInfo culture) =>
		modelAxis == ColorPickerModelAxis.FromName(parameter);

	public override ColorPickerModelAxis ConvertBack(bool value, Type targetType, string parameter, CultureInfo culture) =>
		ColorPickerModelAxis.FromName(parameter);
}

[ValueConversion(typeof(Dictionary<ColorPickerModelAxis, double>), typeof(string))]
public class ColorPickerValuesToTextConverter : ValueConverter<Dictionary<ColorPickerModelAxis, double>, string, string> {
	public override string Convert(Dictionary<ColorPickerModelAxis, double> dictionary, Type targetType, string parameter, CultureInfo culture) {
		return dictionary.TryGetValue(ColorPickerModelAxis.FromName(parameter), out double result) ? Math.Round(result).ToString() : string.Empty;
	}
}

[ValueConversion(typeof(TextChangedEventArgs), typeof(ValueTuple<string, string>))]
public class ColorPickerTextChangedEventArgsToTextAndNameConverter : ValueConverter<TextChangedEventArgs, (string text, string name)> {
	public override (string text, string name) Convert(TextChangedEventArgs e, Type targetType, object parameter, CultureInfo culture) {
		TextBox textBox = (TextBox)e.Source;
		return (textBox.Text, (string)textBox.Tag);
	}
}

[ValueConversion(typeof(Unicolour), typeof(Color))]
public class UnicolourToMediaColorConverter : ValueConverter<Unicolour, Color> {
	public override Color Convert(Unicolour unicolour, Type targetType, object parameter, CultureInfo culture) {
		Color color = unicolour.ToMediaColor();
		if (parameter is Color Color && Color == Colors.Transparent) color.A = 0;
		return color;
	}

	public override Unicolour ConvertBack(Color color, Type targetType, object parameter, CultureInfo culture) =>
		color.ToUnicolour();
}

public class TrackThumbInnerBaseMultiplySizeConverter : MultiValueConverter<double[], double> {
	public override double Convert(double[] values, Type targetType, object parameter, CultureInfo culture) =>
		values.ToArray().Aggregate(1d, (a, b) => a * b);
}

[ValueConversion(typeof(string), typeof(Range))]
public class TextBoxNameToRangeConverter : ValueConverter<string, Range> {
	public override Range Convert(string name, Type targetType, object parameter, CultureInfo culture) {
		(ColourSpace model, int axis) = ColorPickerModelAxis.FromName(name);
		(Range X, Range Y, Range Z) ranges = ColorPickerViewModel.GetInputRange(model);
		return axis switch {
			0 => ranges.X,
			1 => ranges.Y,
			2 => ranges.Z,
			_ => throw new ArgumentOutOfRangeException(name),
		};
	}
}
