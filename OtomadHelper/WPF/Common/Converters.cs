using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Common;

#region BooleanConverter
public class BooleanConverter<T>(T trueValue, T falseValue) : IValueConverter
	where T : notnull {

	public T True { get; set; } = trueValue;
	public T False { get; set; } = falseValue;

	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		value is bool boolean && boolean ? True : False;

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		value is T t && EqualityComparer<T>.Default.Equals(t, True);
}

/// <summary>
/// Convert a <see cref="bool"/> value to a <see cref="Visibility"/> value.<br />
/// Compared to build-in <see cref="System.Windows.Controls.BooleanToVisibilityConverter"/>,
/// you can manually specify the correspondence from true or false to <see cref="Visibility"/>.
/// </summary>
/// <remarks>
/// <example>
/// <para>
/// <c>true => Visible; false => Collapsed</c>
/// (Default, same as <see cref="System.Windows.Controls.BooleanToVisibilityConverter"/>)
/// <code>
/// <![CDATA[<m:BooleanToVisibilityConverter x:Key="BoolToVis" />]]>
/// </code>
/// </para>
/// <para>
/// <c>true => Visible; false => Hidden</c>
/// <code>
/// <![CDATA[<m:BooleanToVisibilityConverter x:Key="BoolToVis" True="Visible" False="Hidden" />]]>
/// </code>
/// </para>
/// <para>
/// <c>true => Collapsed; false => Visible</c>
/// (Invert the <see cref="bool"/> value)
/// <code>
/// <![CDATA[<m:BooleanToVisibilityConverter x:Key="BoolToVis" True="Collapsed" False="Visible" />]]>
/// </code>
/// </para>
/// </example>
/// </remarks>
[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility> {
	public BooleanToVisibilityConverter() :
		base(Visibility.Visible, Visibility.Collapsed) { }
}

[ValueConversion(typeof(bool), typeof(bool))]
public sealed class BooleanInversionConverter : BooleanConverter<bool> {
	public BooleanInversionConverter() :
		base(false, true) { }
}
#endregion

[ValueConversion(typeof(double), typeof(Thickness))]
public class DoubleToThicknessConverter : ValueConverter<double, Thickness, string> {
	public override Thickness Convert(double uniformLength, Type targetType, string parameter, CultureInfo culture) => new(uniformLength);

	public override double ConvertBack(Thickness thickness, Type targetType, string parameter, CultureInfo culture) {
		string side = (parameter ?? "left").ToString().ToLowerInvariant();
		return side switch {
			"top" => thickness.Top,
			"right" => thickness.Right,
			"bottom" => thickness.Bottom,
			_ => thickness.Left,
		};
	}
}

[ValueConversion(typeof(double), typeof(CornerRadius))]
public class DoubleToCornerRadiusConverter : ValueConverter<double, CornerRadius, string> {
	public override CornerRadius Convert(double uniformRadius, Type targetType, string parameter, CultureInfo culture) => new(uniformRadius);

	public override double ConvertBack(CornerRadius cornerRadius, Type targetType, string parameter, CultureInfo culture) {
		string side = (parameter ?? "top left").ToString().ToLowerInvariant();
		return side switch {
			string s when s.Contains("top") && s.Contains("right") => cornerRadius.TopRight,
			string s when s.Contains("bottom") && s.Contains("right") => cornerRadius.BottomRight,
			string s when s.Contains("bottom") && s.Contains("left") => cornerRadius.BottomLeft,
			_ => cornerRadius.TopLeft,
		};
	}
}

public class ActualSizeToRectConverter : MultiValueConverter<Tuple<double, double>, Rect> {
	public override Rect Convert(Tuple<double, double> values, Type targetType, object parameter, CultureInfo culture) {
		(double actualWidth, double actualHeight) = values;
		return new(0, 0, actualWidth, actualHeight);
	}
}

/// <summary>
/// MultiTrigger / MultiDataTrigger with OR instead of AND.
/// </summary>
/// <remarks>
/// <example>
/// <code>
/// <![CDATA[
/// <Style.Triggers>
///   <DataTrigger Value="True">
///     <DataTrigger.Binding>
///       <MultiBinding Converter="{StaticResource BoolOr}">
///         <Binding Path="PermissionFlag" />
///         <Binding Path="CCTVPath" />
///       </MultiBinding>
///     </DataTrigger.Binding>
///     <Setter Property="Visibility" Value="Hidden" />
///   </DataTrigger>
/// </Style.Triggers>
/// ]]>
/// </code>
/// </example>
/// </remarks>
public class BooleanOrConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
		ResolveTargetType(values.Any(IsTruthy), targetType);

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();

	/// <summary>
	/// Determines whether the specified <paramref name="value"/> can be recognized as a <see langword="true"/> value.
	/// </summary>
	public static bool IsTruthy(object value) {
		if (value == DependencyProperty.UnsetValue) return false; // Unfortunately, it cannot be placed in the switch.
		return value switch {
			bool val => val,
			string val => !string.IsNullOrEmpty(val),
			Visibility val => val == Visibility.Visible,
			null => false,
			_ => true,
		};
	}

	public static object ResolveTargetType(bool isTruthy, Type targetType) {
		if (targetType == typeof(Visibility))
			return isTruthy ? Visibility.Visible : Visibility.Collapsed;
		else
			return isTruthy;
	}
}

public class BooleanAndConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
		BooleanOrConverter.ResolveTargetType(values.All(BooleanOrConverter.IsTruthy), targetType);

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

public class BooleanNotConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		BooleanOrConverter.ResolveTargetType(!BooleanOrConverter.IsTruthy(value), targetType);

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		Convert(value, targetType, parameter, culture);
}

public class BooleanTruthyConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		BooleanOrConverter.ResolveTargetType(BooleanOrConverter.IsTruthy(value), targetType);

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		Convert(value, targetType, parameter, culture);
}

[ValueConversion(typeof(Rect), typeof(DrawingBrush))]
public class RectToDrawingBrushConverter : ValueConverter<Rect?, DrawingBrush> {
	public override DrawingBrush Convert(Rect? rect, Type targetType, object parameter, CultureInfo culture) {
		DrawingBrush brush = new();
		DrawingGroup group = new();
		group.Children.Add(new GeometryDrawing {
			Brush = new SolidColorBrush(Colors.Transparent),
			Geometry = new RectangleGeometry(new(0, 0, 1, 1)),
		});
		group.Children.Add(new GeometryDrawing {
			Brush = new SolidColorBrush(Colors.Black),
			Geometry = new RectangleGeometry(rect!.Value),
		});
		brush.Drawing = group;
		return brush;
	}

	public override Rect? ConvertBack(DrawingBrush brush, Type targetType, object parameter, CultureInfo culture) =>
		((brush.Drawing as DrawingGroup)?.Children.LastOrDefault() as GeometryDrawing)?.Geometry.Bounds;
}

public class RelativeToAbsoluteRectConverter : MultiValueConverter<Tuple<Rect, double, double>, Rect> {
	public override Rect Convert(Tuple<Rect, double, double> values, Type targetType, object parameter, CultureInfo culture) {
		(Rect relativeRect, double actualWidth, double actualHeight) = values;

		return new Rect(
			relativeRect.X * actualWidth,
			relativeRect.Y * actualHeight,
			relativeRect.Width * actualWidth,
			relativeRect.Height * actualHeight
		);
	}
}

[ValueConversion(typeof(string), typeof(string))]
public class AccessKeyAmpersandToUnderscoreConverter : ValueConverter<string, string> {
	public override string Convert(string value, Type targetType, object parameter, CultureInfo culture) =>
		(value ?? "").Replace('&', '_');

	public override string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture) =>
		(value ?? "").Replace('_', '&');
}

public class OvalCornerRadiusConverter : MultiValueConverter<double[], CornerRadius> {
	private static double IfNaN(double value, double replacement) =>
		value is double.NaN or double.PositiveInfinity or double.NegativeInfinity ? replacement : value;

	public override CornerRadius Convert(double[] values, Type targetType, object parameter, CultureInfo culture) {
		double radius = values.Select(value => IfNaN(value, double.PositiveInfinity)).Min() / 2;
		if (radius == double.PositiveInfinity) radius = 0;
		return new(radius);
	}
}

public class DebuggerConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		// Set breakpoint here
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		// Set breakpoint here
		return value;
	}
}

[ValueConversion(typeof(object), typeof(Visibility))]
public class ObjectToVisibilityConverter : ValueConverter<object, Visibility> {
	public override Visibility Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		bool shown = BooleanOrConverter.IsTruthy(value);
		return shown ? Visibility.Visible : Visibility.Collapsed;
	}
}

public class CommandParameters : MultiBinding {
	public CommandParameters() : base() {
		Converter = new MultiValueToArrayConverter();
	}
}

internal class MultiValueToArrayConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => values;

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(Color), typeof(Color))]
[ValueConversion(typeof(SolidColorBrush), typeof(SolidColorBrush))]
public class BackgroundToForegroundColorConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Color color = value is Color c ? c : value is SolidColorBrush b ? b.Color : throw new ArgumentException($"Unknown source value {value}");
		Wacton.Unicolour.Unicolour unicolour = color.ToUnicolour();
		bool tooWhite = unicolour.Oklch.L >= 0.65;
		Color foregroundColor = tooWhite ? Colors.Black : Colors.White;
		return targetType.Extends(typeof(Color)) ? foregroundColor : targetType.Extends(typeof(Brush)) ? new SolidColorBrush(foregroundColor) :
			throw new NotImplementedException($"Unknown target type {targetType}");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(object), typeof(string))]
public class ObjectToTypeNameConverter : ValueConverter<object, string> {
	public override string Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		value.GetType().Name;
}

[ValueConversion(typeof(Color), typeof(SolidColorBrush))]
[ValueConversion(typeof(SolidColorBrush), typeof(Color))]
public class SolidColorBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Color color = value is Color c ? c : value is SolidColorBrush b ? b.Color : throw new ArgumentException($"Unknown source value {value}");
		return targetType.Extends(typeof(Color)) ? color : targetType.Extends(typeof(Brush)) ? new SolidColorBrush(color) :
			throw new NotImplementedException($"Unknown target type {targetType}");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		Convert(value, targetType, parameter, culture);
}
