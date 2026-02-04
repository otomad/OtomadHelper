using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Attach acrylic material system backdrop (by Windows 10 Composition API) to <see cref="ContextMenu" /> and <see cref="ToolTip" />.
/// </summary>
[AttachedDependencyProperty<bool, ContextMenu>("FixCanExecute", DefaultValue = false)]
[AttachedDependencyProperty<bool, ContextMenu>("AutoIcon", DefaultValue = true)]
public partial class ContextMenuAcrylicBehavior : Behavior<FrameworkElement> {
	protected override void OnAttached() {
		AssociatedObject.IsVisibleChanged += ContextMenu_IsVisibleChanged;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.IsVisibleChanged -= ContextMenu_IsVisibleChanged;
	}

	private void ContextMenu_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
		FrameworkElement element = AssociatedObject;
		InitKnownIcons(Window.GetWindow(element));

		if (!element.IsVisible) return;
		InitializeComponent(element, element is ToolTip);

		if (element is ContextMenu contextMenu) {
			// Fix a confusing issue where the IsEnabled of MenuItems was not updated in time.
			if (GetFixCanExecute(contextMenu))
				foreach (object? anyItem in contextMenu.Items)
					if (anyItem is MenuItem item)
						if (item.Command is ICommand command) {
							item.Command = null;
							item.IsEnabled = command.CanExecute(item.CommandParameter);
							item.Command = command;
						}

			if (GetAutoIcon(contextMenu)) {
				Orientation? orientation = contextMenu.PlacementTarget is ScrollBar scrollBar ? scrollBar.Orientation : null;
				foreach (object? anyItem in contextMenu.Items)
					if (anyItem is MenuItem item)
						if (GetKnownIcon(item.Command, orientation) is Icon newIcon)
								item.Icon = newIcon;
			}
		}
	}

	internal static void InitializeComponent(FrameworkElement element, bool roundSmaller = false) {
		IntPtr? handle = (PresentationSource.FromVisual(element) as HwndSource)?.Handle;
		if (handle is not IntPtr Handle) return;

		if (WindowsVersion.Current >= WindowsNT.Windows10_1803) {
			bool isDarkTheme = BackdropWindow.ShouldAppsUseDarkMode();
			SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, isDarkTheme ? 1u : 0);
			SetWindowAttribute(Handle, DwmWindowAttribute.WindowCornerPreference, (uint)(roundSmaller ? WindowCornerPreference.RoundSmall : WindowCornerPreference.Round));
			EnableAcrylicBlurBehind(Handle, isDarkTheme ? 0x663a3a3au : 0x69fcfcfcu);
			SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, 0xfffffffe);
		} else
			(element as Control)?.Background = SystemColors.MenuBarBrush;
	}

	private static readonly Dictionary<ICommand, Icon> knownIcons = [];
	private static Icon? verticalScrollHereIcon;
	private static Icon? horizontalScrollHereIcon;
	private static void InitKnownIcons(FrameworkElement window) {
		if (knownIcons.Count != 0 || window is null) return;
		using BamlAssemblyResource baml = new();
		ResourceDictionary xaml = (ResourceDictionary)baml.GetXaml("WPF/Themes/Menus");
		IEnumerable<string> keys = xaml.Keys.Cast<string>();
		foreach (string key in keys) {
			object? value = window.Resources[key];
			if (value is ContextMenu contextMenu)
				foreach (object anyItem in contextMenu.Items)
					if (anyItem is MenuItem item && item.Command is not null && item.Icon is Icon icon) {
						if (item.Command == ScrollBar.ScrollHereCommand) {
							if (key == "VerticalScrollBarContextMenu") verticalScrollHereIcon = icon;
							else if (key == "HorizontalScrollBarContextMenu") horizontalScrollHereIcon = icon;
						} else
							knownIcons.Add(item.Command, icon);
					}
		}
	}
	protected internal static Icon? GetKnownIcon(ICommand command, Orientation? orientation = null) {
		if (command is null) return null;
		orientation ??= Orientation.Vertical;
		if (command == ScrollBar.ScrollHereCommand)
			return orientation == Orientation.Horizontal ? horizontalScrollHereIcon : verticalScrollHereIcon;
		return knownIcons.TryGetValue(command, out Icon value) ? value : null;
	}
}
