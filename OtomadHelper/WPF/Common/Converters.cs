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
public class DoubleToThicknessConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		double uniformLength = (double)value;
		return new Thickness(uniformLength);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		string side = (parameter ?? "left").ToString().ToLowerInvariant();
		Thickness thickness = (Thickness)value;
		return side switch {
			"top" => thickness.Top,
			"right" => thickness.Right,
			"bottom" => thickness.Bottom,
			_ => thickness.Left,
		};
	}
}

[ValueConversion(typeof(double), typeof(CornerRadius))]
public class DoubleToCornerRadiusConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		double uniformRadius = (double)value;
		return new CornerRadius(uniformRadius);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		string side = (parameter ?? "top left").ToString().ToLowerInvariant();
		CornerRadius cornerRadius = (CornerRadius)value;
		return side switch {
			string s when s.Contains("top") && s.Contains("right") => cornerRadius.TopRight,
			string s when s.Contains("bottom") && s.Contains("right") => cornerRadius.BottomRight,
			string s when s.Contains("bottom") && s.Contains("left") => cornerRadius.BottomLeft,
			_ => cornerRadius.TopLeft,
		};
	}
}

[ValueConversion(typeof(FrameworkElement), typeof(Rect))]
public class ActualSizeToRectConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		FrameworkElement element = (FrameworkElement)value;
		return new Rect(0, 0, element.ActualWidth, element.ActualHeight);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
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

[ValueConversion(typeof(Rect), typeof(DrawingBrush))]
public class RectToDrawingBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Rect rect = (Rect)value;
		DrawingBrush brush = new();
		DrawingGroup group = new();
		group.Children.Add(new GeometryDrawing {
			Brush = new SolidColorBrush(Colors.Transparent),
			Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1)),
		});
		group.Children.Add(new GeometryDrawing {
			Brush = new SolidColorBrush(Colors.Black),
			Geometry = new RectangleGeometry(rect),
		});
		brush.Drawing = group;
		return brush;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		DrawingBrush brush = (DrawingBrush)value;
		return ((brush.Drawing as DrawingGroup)?.Children.LastOrDefault() as GeometryDrawing)?.Geometry.Bounds;
	}
}

public class RelativeToAbsoluteRectConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		if (values.Any(value => value == DependencyProperty.UnsetValue))
			return DependencyProperty.UnsetValue;

		(Rect relativeRect, double actualWidth, double actualHeight) = values.ToTuple<Tuple<Rect, double, double>>();

		return new Rect(
			relativeRect.X * actualWidth,
			relativeRect.Y * actualHeight,
			relativeRect.Width * actualWidth,
			relativeRect.Height * actualHeight
		);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(string), typeof(string))]
public class AccessKeyAmpersandToUnderscoreConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		((string)value).Replace('&', '_');

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		((string)value).Replace('_', '&');
}

//[ValueConversion(typeof(FrameworkElement), typeof(CornerRadius))]
//public class OvalCornerRadiusConverter : IValueConverter {
//	private static double IfNaN(double value, double replacement) =>
//		value is double.NaN or double.PositiveInfinity or double.NegativeInfinity ? replacement : value;

//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
//		FrameworkElement element = (FrameworkElement)value;
//		double radius = Math.Min(IfNaN(element.Width, double.PositiveInfinity), IfNaN(element.Height, double.PositiveInfinity)) / 2;
//		if (radius == double.PositiveInfinity) radius = 0;
//		return new CornerRadius(radius);
//	}

//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
//		throw new NotImplementedException();
//}

public class OvalCornerRadiusConverter : IMultiValueConverter {
	private static double IfNaN(double value, double replacement) =>
		value is double.NaN or double.PositiveInfinity or double.NegativeInfinity ? replacement : value;

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		double radius = values.Cast<double>().Select(value => IfNaN(value, double.PositiveInfinity)).Min() / 2;
		if (radius == double.PositiveInfinity) radius = 0;
		return new CornerRadius(radius);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
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
public class ObjectToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		bool shown = BooleanOrConverter.IsTruthy(value);
		return shown ? Visibility.Visible : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
