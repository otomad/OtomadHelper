using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Common;

[AttachedDependencyProperty<double, UIElement>("CornerRadius")]
public static partial class CornerRadiusClip {
	static partial void OnCornerRadiusChanged(UIElement element, double cornerRadius) {
		RectangleGeometry rect = new() {
			RadiusX = cornerRadius,
			RadiusY = cornerRadius,
		};
		MultiBinding multiBinding = new() {
			Converter = new ActualSizeToRectConverter(),
		};
		RelativeSource relativeSource = new() {
			Mode = RelativeSourceMode.FindAncestor,
			AncestorType = element.GetType(),
		};
		multiBinding.Bindings.AddRange([
			new Binding("ActualWidth") { RelativeSource = relativeSource },
			new Binding("ActualHeight") { RelativeSource = relativeSource },
		]);
		BindingOperations.SetBinding(rect, RectangleGeometry.RectProperty, multiBinding);
		element.Clip = rect;
	}
}
