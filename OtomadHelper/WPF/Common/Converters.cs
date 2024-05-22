using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Common;

#region BooleanConverter
public class BooleanConverter<T> : IValueConverter
	where T : notnull {
	public BooleanConverter(T trueValue, T falseValue) {
		True = trueValue;
		False = falseValue;
	}

	public T True { get; set; }
	public T False { get; set; }

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
///       <MultiBinding Converter="{StaticResource OrConverter}">
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
public class MultiTriggerOrConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
		values.Any(value => {
			if (value == DependencyProperty.UnsetValue) return false; // Unfortunately, it cannot be placed in the switch.
			return value switch {
				bool val => val,
				string val => string.IsNullOrEmpty(val),
				null => false,
				_ => true,
			};
		});

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(Rect), typeof(DrawingBrush))]
public class RectToDrawingBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Rect rect = (Rect)value;
		DrawingBrush brush = new();
		DrawingGroup group = new();
		group.Children.Add(new GeometryDrawing() {
			Brush = new SolidColorBrush(Colors.Transparent),
			Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1)),
		});
		group.Children.Add(new GeometryDrawing() {
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
