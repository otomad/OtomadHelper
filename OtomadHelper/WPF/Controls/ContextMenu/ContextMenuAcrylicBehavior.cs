using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Attach acrylic material system backdrop (by Windows 10 Composition API) to <see cref="ContextMenu" /> and <see cref="ToolTip" />.
/// </summary>
[AttachedDependencyProperty<bool, ContextMenu>("FixCanExecute", DefaultValue = false)]
[AttachedDependencyProperty<bool, ContextMenu>("AutoIcon", DefaultValue = true)]
[AttachedDependencyProperty<CornerRadius>("CornerRadius")]
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

		bool isDarkTheme = BackdropWindow.ShouldAppsUseDarkMode();
		bool supportComposition = EnableAcrylicBlurBehind(Handle, !isDarkTheme ? 0x69fcfcfcu : 0x663a3a3au);
		if (supportComposition) {
			SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, isDarkTheme ? 1u : 0u);
			SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, 0xfffffffe);
			if (SetWindowAttribute(Handle, DwmWindowAttribute.WindowCornerPreference, (uint)(roundSmaller ? WindowCornerPreference.RoundSmall : WindowCornerPreference.Round)) == HResult.InvalidArg)
				SetCornerRadius(element, new(0));
		} else {
			SolidColorBrush background = (
				isDarkTheme ? BackdropWindow.SolidDarkThemeBackgroundBrush : // SystemColors doesn't support system dark theme colors.
				element switch {
					ContextMenu => SystemColors.MenuBarBrush,
					ToolTip => SystemColors.InfoBrush,
					_ => SystemColors.MenuBarBrush,
				}
			).Clone();
			background.Opacity = 0.75;
			(element as Control)?.Background = background;
			const double shadowDepth = 2;
			element.Margin = new(shadowDepth);
			element.Effect = new DropShadowEffect() {
				ShadowDepth = shadowDepth,
				Color = Colors.Black,
				BlurRadius = shadowDepth,
				Opacity = 0.3,
			};
		}
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
