using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace OtomadHelper.WPF.Controls;

public partial class ContextMenuResourceDictionary : CustomControlResourceDictionary {
	public void ContextMenu_IsVisibleChanged(object sender, RoutedPropertyChangedEventArgs<bool> e) {
		ContextMenu contextMenu = (ContextMenu)sender;
		if (!contextMenu.IsVisible) return;
		InitializeComponent(contextMenu);
	}

	internal static void InitializeComponent(ContextMenu contextMenu) {
		IntPtr? handle = (PresentationSource.FromVisual(contextMenu) as HwndSource)?.Handle;
		if (handle is not IntPtr Handle) return;

		bool isDarkTheme = BackdropWindow.ShouldAppsUseDarkMode();
		SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, isDarkTheme ? 1u : 0);
		SetWindowAttribute(Handle, DwmWindowAttribute.WindowCornerPreference, (int)WindowCornerPreference.Round);
		EnableAcrylicBlurBehind(Handle, isDarkTheme ? 0x663a3a3au : 0x69fcfcfcu);
		SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, 0xfffffffe);
	}
}
