using System.Windows;
using System.Windows.Data;

using OtomadHelper.WPF.Common;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(bool), typeof(TitleBarType))]
public sealed class ComboBoxFlyoutIsContentToTitleBarTypeConverter : BooleanConverter<TitleBarType> {
	public ComboBoxFlyoutIsContentToTitleBarTypeConverter() :
		base(TitleBarType.Borderless, TitleBarType.WindowChromeNoTitleBar) { }
}

[ValueConversion(typeof(bool), typeof(SystemBackdropType))]
public sealed class ComboBoxFlyoutIsContentToSystemBackdropConverter : BooleanConverter<SystemBackdropType> {
	public ComboBoxFlyoutIsContentToSystemBackdropConverter() :
		base(SystemBackdropType.None, SystemBackdropType.TransientWindow) { }
}

[ValueConversion(typeof(bool), typeof(Thickness))]
public sealed class ComboBoxFlyoutIsContentToBorderThicknessConverter : BooleanConverter<Thickness> {
	public ComboBoxFlyoutIsContentToBorderThicknessConverter() :
		base(new(0), new(1)) { }
}
