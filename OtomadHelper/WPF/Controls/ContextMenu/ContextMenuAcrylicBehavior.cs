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
[AttachedDependencyProperty<bool, ContextMenu>("IsHighContrast", DefaultValue = false)]
[AttachedDependencyProperty<CornerRadius>("CornerRadius")]
[AttachedDependencyProperty<bool, Control>("IsGlassEnabled", DefaultValue = false, IsReadOnly = true)]
[AttachedDependencyProperty<bool, Control>("IsCornerRadiusCustomizable", DefaultValue = false, IsReadOnly = true)]
[AttachedDependencyProperty<bool, Control>("IsSetAeroBlurBehindSizeChangedHookAdded", DefaultValue = false, IsReadOnly = true)]
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
		nint? handle = (PresentationSource.FromVisual(element) as HwndSource)?.Handle;
		if (handle is not nint Handle || element is not Control control) return;

		bool isDarkTheme = BackdropWindow.ShouldAppsUseDarkMode();
		bool isHighContrast = SystemParameters.HighContrast;
		BackdropWindow.BindHighContrastToProperty(control, IsHighContrastProperty);
		AccentState backdrop = isHighContrast ? AccentState.Disabled : WindowsVersion.Current switch {
			>= WindowsNT.Windows10_1803 => AccentState.EnableAcrylicBlurBehind,
			>= WindowsNT.Windows10 => AccentState.EnableBlurBehind,
			>= WindowsNT.Windows8 => AccentState.EnableTransparentGradient,
			_ => AccentState.EnableHostBackdrop,
		};
		if (backdrop == AccentState.EnableTransparentGradient) { // Windows 8/8.1.
			ApplySolidBackground();
			control.Effect = null;
		} else {
			bool supportComposition = BackdropWindow.SetAcrylicByComposition(Handle, control, backdrop);
			if (supportComposition && !isHighContrast) { // Windows Vista/7 Aero Theme, Windows 10/11.
				SetIsGlassEnabled(control, true);
				SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, isDarkTheme ? 1u : 0u); // Windows 10/11.
				SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, 0xfffffffe); // Windows 11.
				SetIsCornerRadiusCustomizable(control,
					WindowsVersion.Current is >= WindowsNT.WindowsVista and < WindowsNT.Windows8 || // Windows Vista/7.
					SetWindowAttribute(Handle, DwmWindowAttribute.WindowCornerPreference, (uint)(roundSmaller ? WindowCornerPreference.RoundSmall : WindowCornerPreference.Round)) != HResult.InvalidArg); // Windows 11.
			} else { // Windows Vista/7 Basic Theme or Classic Theme, Windows XP, etc.
				ApplySolidBackground();
				SetIsCornerRadiusCustomizable(control, true);
				SetIsGlassEnabled(control, false);
			}
		}
		if (WindowsVersion.Current is >= WindowsNT.Windows8 and < WindowsNT.Windows11_Dev) // Windows 8/8.1/10.
			SetIsCornerRadiusCustomizable(control, false);

		void ApplySolidBackground() =>
			control.Background = (
				isDarkTheme && !isHighContrast ? BackdropWindow.DarkThemeBackgroundBrush : // SystemColors doesn't support system dark theme colors.
				control switch {
					ContextMenu => SystemColors.MenuBarBrush,
					ToolTip => SystemColors.InfoBrush,
					_ => SystemColors.MenuBarBrush,
				}
			).Clone();
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
