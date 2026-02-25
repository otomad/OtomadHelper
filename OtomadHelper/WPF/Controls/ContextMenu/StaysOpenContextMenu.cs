using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Inherit from <see cref="ContextMenu" /> into a new class that would coerce the <see cref="ContextMenu.IsOpen" /> property to
/// <see langword="true" /> when <see cref="ContextMenu.StaysOpen" /> is set to <see langword="true" />.
/// </summary>
/// <remarks>
/// <see href="https://siderite.dev/blog/forcing-open-wpf-contextmenu.html" />
/// </remarks>
public class StaysOpenContextMenu : ContextMenu {
	static StaysOpenContextMenu() => FixContextMenuStaysOpen();

	private static void FixContextMenuStaysOpen() {
		IsOpenProperty.OverrideMetadata(
			typeof(StaysOpenContextMenu),
			new FrameworkPropertyMetadata(false, null, CoerceIsOpen));
		StaysOpenProperty.OverrideMetadata(
			typeof(StaysOpenContextMenu),
			new FrameworkPropertyMetadata(false, null, CoerceStaysOpen));
	}

	private static object CoerceStaysOpen(DependencyObject dp, object baseValue) {
		dp.CoerceValue(IsOpenProperty);
		return baseValue;
	}

	private static object CoerceIsOpen(DependencyObject dp, object baseValue) {
		ContextMenu menu = (ContextMenu)dp;
		return menu.StaysOpen ? true : baseValue;
	}
}
