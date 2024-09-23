using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Makes your Corner Radius shape in an oval pill, instead of a weird ellipse.
/// </summary>
[AttachedDependencyProperty<bool, UIElement>("Enabled", DefaultValue = false)]
public static partial class OvalCornerRadius {
	static partial void OnEnabledChanged(UIElement element, bool enabled) {
		switch (element) {
			case Border border:
				if (enabled) border.SetBinding(Border.CornerRadiusProperty, CornerRadiusBinding);
				else border.ClearBinding(Border.CornerRadiusProperty);
				break;
			default: // I haven't written about other element types yet.
				break;
		}
	}

	/// <remarks>
	/// If use the feature without this helper and manually declare in the XAML:
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <m:OvalCornerRadiusConverter x:Key="Oval" />
	///
	/// <Border.CornerRadius>
	/// 	<MultiBinding Converter="{StaticResource Oval}">
	/// 		<Binding Path="ActualWidth" RelativeSource="{RelativeSource Self}" />
	/// 		<Binding Path="ActualHeight" RelativeSource="{RelativeSource Self}" />
	/// 	</MultiBinding>
	/// </Border.CornerRadius>
	/// ]]>
	/// </code>
	/// </example>
	/// </remarks>
	private static BindingBase CornerRadiusBinding {
		get {
			MultiBinding multiBinding = new() {
				Converter = new OvalCornerRadiusConverter()
			};
			RelativeSource relativeSource = new() { Mode = RelativeSourceMode.Self };

			multiBinding.Bindings.AddRange([
				new Binding("ActualWidth") { RelativeSource = relativeSource },
				new Binding("ActualHeight") { RelativeSource = relativeSource },
			]);

			return multiBinding;
		}
	}
}
