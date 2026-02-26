using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<ScrollBarLineButtonArrowPoint>("ArrowPoint", DefaultValue = ScrollBarLineButtonArrowPoint.Up)]
[DependencyProperty<double>("ArrowRotation", DefaultValue = 0, IsReadOnly = true)]
[DependencyProperty<double>("ArrowScale", DefaultValue = 1)]
public partial class ScrollBarLineButton : RepeatButton {
	partial void OnArrowPointChanged(ScrollBarLineButtonArrowPoint arrowPoint) => ArrowRotation = (double)arrowPoint;

	public ScrollBarLineButton() : base() {
		// Set is enabled property binding.
		MultiBinding binding = new();
		RelativeSource scrollBarRelativeSource = new(RelativeSourceMode.FindAncestor, typeof(ScrollBar), 1);
		binding.AddBinding([
			new(nameof(ArrowPoint)) { RelativeSource = new(RelativeSourceMode.Self) },
			new(nameof(ScrollBar.Value)) { RelativeSource = scrollBarRelativeSource },
			new(nameof(ScrollBar.Minimum)) { RelativeSource = scrollBarRelativeSource },
			new(nameof(ScrollBar.Maximum)) { RelativeSource = scrollBarRelativeSource },
		]);
		binding.Converter = new ScrollBarValueToEnabledConverter();
		SetBinding(IsEnabledProperty, binding);
	}

	private class ScrollBarValueToEnabledConverter : MultiValueConverter<Tuple<ScrollBarLineButtonArrowPoint, double, double, double>, bool> {
		public override bool Convert(Tuple<ScrollBarLineButtonArrowPoint, double, double, double> args, Type targetType, object parameter, CultureInfo culture) {
			(ScrollBarLineButtonArrowPoint point, double value, double min, double max) = args;
			return point is ScrollBarLineButtonArrowPoint.Up or ScrollBarLineButtonArrowPoint.Left ? value > min : value < max;
		}
	}

}

public enum ScrollBarLineButtonArrowPoint {
	Up = 0,
	Right = 90,
	Down = 180,
	Left = 270,
}

[DependencyProperty<double>("BaseWidth", TypeConverter = typeof(LengthConverter))]
[DependencyProperty<double>("BaseHeight", TypeConverter = typeof(LengthConverter))]
[DependencyProperty<Thickness>("BaseMargin")]
public partial class ScrollBarThumb : Thumb { }

[AttachedDependencyProperty<bool, ScrollBar>("IsMouseOverOrContextMenuOpened", DefaultValue = false)]
public static partial class ScrollBarAttached { }
