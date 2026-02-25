// TODO: "PInvoke.cs" rename to "PInvoke_Win32.cs" and "PInvoke_DotNet.cs".

using System.Drawing;

namespace OtomadHelper.Interop;

public static class PInvoke {
	/// <summary>
	/// Flags for specifying the system-drawn backdrop material of a window, including behind the non-client area.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/api/dwmapi/ne-dwmapi-dwm_systembackdrop_type"><c>DWM_SYSTEMBACKDROP_TYPE</c> enumeration (dwmapi.h)</a>
	/// </remarks>
	public enum SystemBackdropType {
		/// <remarks>
		/// The default. Let the Desktop Window Manager (DWM) automatically decide the system-drawn backdrop material for this window.
		/// This applies the backdrop material just behind the default Win32 title bar. This behavior attempts to preserve maximum backwards compatibility.
		/// For this reason, the DWM might also decide to draw no backdrop material at all based on internal heuristics.<br/>
		/// If drawing the backdrop material behind the entire window is required, choose one of the other more specific values of this enum as appropriate.
		/// </remarks>
		Auto,
		/// <summary>
		/// Don't draw any system backdrop.
		/// </summary>
		None,
		/// <summary>Mica</summary>
		/// <remarks>
		/// Draw the backdrop material effect corresponding to a long-lived window behind the entire window bounds.<br/>
		/// For Windows 11, this corresponds to Mica in its default variant. The material effect might change with future Windows releases.
		/// For more info about Mica, see <a href="https://learn.microsoft.com/windows/apps/design/style/mica">Mica</a>.
		/// </remarks>
		MainWindow,
		/// <summary>Acrylic</summary>
		/// <remarks>
		/// Draw the backdrop material effect corresponding to a transient window behind the entire window bounds.<br/>
		/// For Windows 11, this corresponds to Desktop Acrylic, also known as Background Acrylic, in its brightest variant.
		/// The material effect might change with future Windows releases. For more info about Desktop Acrylic, see
		/// <a href="https://learn.microsoft.com/windows/apps/design/style/acrylic">Acrylic</a>.
		/// <para>Redirect to <see cref="AccentState.EnableAcrylicBlurBehind" /> in Windows 10.</para>
		/// </remarks>
		TransientWindow,
		/// <summary>MicaAlt</summary>
		/// <remarks>
		/// Draw the backdrop material effect corresponding to a window with a tabbed title bar behind the entire window bounds.<br/>
		/// For Windows 11, this corresponds to Mica in its alternate variant (Mica Alt). The material might change with future releases of Windows.
		/// For more info about Mica Alt, see <a href="https://learn.microsoft.com/windows/apps/design/style/mica#app-layering-with-mica-alt">Layering with Mica Alt</a>.
		/// </remarks>
		TabbedWindow,
		///// <summary>Blur</summary>
		///// <remarks>
		///// Simulates blur effects of the Start Menu and Action Center in earlier versions of Windows 10. Acrylic was not proposed at that time.
		///// <para>Redirect to <see cref="AccentState.EnableBlurBehind" /> in Windows 10.</para>
		///// </remarks>
		//EarlyTransientWindow = -3,
	}

	/// <summary>
	/// Options used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute"><c>DWMWINDOWATTRIBUTE enumeration (dwmapi.h)</c></a><br/>
	/// </remarks>
	public enum DwmWindowAttribute {
		NCRenderingEnabled,
		NCRenderingPolicy,
		TransitionsForceDisabled,
		AllowNCPaint,
		CaptionButtonBounds,
		NonClientRtlLayout,
		ForceIconicRepresentation,
		Flip3dPolicy,
		ExtendedFrameBounds,
		HasIconicBitmap,
		DisallowPeek,
		ExcludedFromPeek,
		Cloak,
		Cloaked,
		FreezeRepresentation,
		PassiveUpdateMode,
		UseHostBackdropBrush,
		/// <summary>
		/// Use with DwmSetWindowAttribute. Allows the window frame for this window to be drawn in dark mode colors when the dark mode system setting is enabled.
		/// For compatibility reasons, all windows default to light mode regardless of the system setting. The pvAttribute parameter points to a value of type <b>BOOL</b>.
		/// <b>TRUE</b> to honor dark mode for the window, <b>FALSE</b> to always use light mode.
		/// </summary>
		UseImmersiveDarkMode = 20,
		WindowCornerPreference = 33,
		BorderColor,
		CaptionColor,
		TextColor,
		VisibleFrameBorderThickness,
		/// <summary>
		/// Use with DwmGetWindowAttribute or DwmSetWindowAttribute. Retrieves or specifies the system-drawn backdrop material of a window,
		/// including behind the non-client area. The <i>pvAttribute</i> parameter points to a value of type <see cref="PInvoke.SystemBackdropType"/>.
		/// </summary>
		SystemBackdropType,
		Last,
	}

	/// <summary>
	/// Ways you can round windows.
	/// </summary>
	public enum WindowCornerPreference {
		/// <summary>
		/// Determined by system or application preference.
		/// </summary>
		Default,
		/// <summary>
		/// Do not round the corners.
		/// </summary>
		DoNotRound,
		/// <summary>
		/// Round the corners.
		/// </summary>
		Round,
		/// <summary>
		/// Round the corners slightly.
		/// </summary>
		RoundSmall,
	}

	/// <summary>
	/// Extended Window Styles.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/winmsg/extended-window-styles">Extended Window Styles</a><br/>
	/// </remarks>
	[Flags]
	public enum ExtendedWindowStyles : long {
		AcceptFiles = 0x00000010L,
		AppWindow = 0x00040000L,
		ClientEdge = 0x00000200L,
		Composited = 0x02000000L,
		ContextHelp = 0x00000400L,
		ControlParent = 0x00010000L,
		DlgModalFrame = 0x00000001L,
		Layered = 0x00080000L,
		LayoutRtl = 0x00400000L,
		Left = 0x00000000L,
		LeftScrollBar = 0x00004000L,
		LtrReading = 0x00000000L,
		MdiChild = 0x00000040L,
		/// <remarks>
		/// A top-level window created with this style does not become the foreground window when the user clicks
		/// it. The system does not bring this window to the foreground when the user minimizes or closes the
		/// foreground window.<br />
		/// The window should not be activated through programmatic access or via keyboard navigation by accessible
		/// technology, such as Narrator.<br />
		/// To activate the window, use the SetActiveWindow or SetForegroundWindow function.<br />
		/// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use
		/// the <b>WS_EX_APPWINDOW</b> style.
		/// </remarks>
		NoActivate = 0x08000000L,
		NoInheritLayout = 0x00100000L,
		NoParentNotify = 0x00000004L,
		NoRedirectionBitmap = 0x00200000L,
		OverlappedWindow = WindowEdge | ClientEdge,
		PaletteWindow = WindowEdge | ToolWindow | TopMost,
		Right = 0x00001000L,
		RightScrollBar = 0x00000000L,
		RtlReading = 0x00002000L,
		StaticEdge = 0x00020000L,
		/// <remarks>
		/// The window is intended to be used as a floating toolbar. A tool window has a title bar that is
		/// shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window
		/// does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a
		/// tool window has a system menu, its icon is not displayed on the title bar. However, you can display
		/// the system menu by right-clicking or by typing ALT+SPACE.
		/// </remarks>
		ToolWindow = 0x00000080L,
		TopMost = 0x00000008L,
		Transparent = 0x00000020L,
		WindowEdge = 0x00000100L,
	}
	/// <summary>
	/// Window Styles.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/winmsg/window-styles">Window Styles</a><br/>
	/// </remarks>
	[Flags]
	public enum WindowStyles : long {
		Border = 0x00800000L,
		Caption = 0x00C00000L,
		Child = 0x40000000L,
		ChildWindow = 0x40000000L,
		ClipChildren = 0x02000000L,
		ClipSiblings = 0x04000000L,
		Disabled = 0x08000000L,
		DlgFrame = 0x00400000L,
		Group = 0x00020000L,
		HScroll = 0x00100000L,
		Iconic = 0x20000000L,
		Maximize = 0x01000000L,
		MaximizeBox = 0x00010000L,
		Minimize = 0x20000000L,
		MinimizeBox = 0x00020000L,
		Overlapped = 0x00000000L,
		OverlappedWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,
		Popup = 0x80000000L,
		PopupWindow = Popup | Border | SysMenu,
		SizeBox = 0x00040000L,
		SysMenu = 0x00080000L,
		TabStop = 0x00010000L,
		ThickFrame = 0x00040000L,
		Tiled = 0x00000000L,
		TiledWindow = Overlapped | Caption | SysMenu | ThickFrame | MinimizeBox | MaximizeBox,
		Visible = 0x10000000L,
		Vscroll = 0x00200000L,
	}

	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-setwindowlongw#parameters">Window Long Flags</a>
	/// </remarks>
	public enum WindowLongFlags {
		/// <summary>
		/// The extended window style.
		/// </summary>
		ExStyle = -20,
		HInstance = -6,
		ID = -12,
		/// <summary>
		/// The window style.
		/// </summary>
		Style = -16,
		UserData = -21,
		WndProc = -4,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Margins(int left, int top, int right, int bottom) {
		/// <summary>
		/// width of left border that retains its size
		/// </summary>
		public int cxLeftWidth = left;
		/// <summary>
		/// width of right border that retains its size
		/// </summary>
		public int cxRightWidth = right;
		/// <summary>
		/// height of top border that retains its size
		/// </summary>
		public int cyTopHeight = top;
		/// <summary>
		/// height of bottom border that retains its size
		/// </summary>
		public int cyBottomHeight = bottom;

		public Margins(int size) : this(size, size, size, size) { }
	};

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(nint hWnd, ref Margins pMarInset);

	[DllImport("dwmapi.dll")]
	public static extern HResult DwmGetWindowAttribute(nint hWnd, DwmWindowAttribute dwAttribute, out uint pvAttribute, int cbAttribute);

	[DllImport("dwmapi.dll")]
	public static extern HResult DwmSetWindowAttribute(nint hWnd, DwmWindowAttribute dwAttribute, ref uint pvAttribute, int cbAttribute);

	[DllImport("user32.dll")]
	public static extern long GetWindowLongPtr(nint hWnd, WindowLongFlags nIndex);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern nint SetWindowLongPtr(nint hWnd, WindowLongFlags nIndex, nint dwNewLong);
	[DllImport("user32.dll", SetLastError = true)]
	public static extern nint SetWindowLongPtr(nint hWnd, WindowLongFlags nIndex, long dwNewLong);
	[DllImport("user32.dll")]
	public static extern nint CallWindowProc(nint lpPrevWndFunc, nint hWnd, uint msg, nint wParam, nint lParam);
	[DllImport("user32.dll")]
	public static extern nint CallWindowProc(long lpPrevWndFunc, nint hWnd, uint msg, nint wParam, nint lParam);

	[DllImport("user32.dll")]
	public static extern nint GetActiveWindow();

	[DllImport("user32.dll")]
	public static extern nint SetActiveWindow(nint hWnd);

	[DllImport("user32.dll")]
	public static extern bool SetLayeredWindowAttributes(nint hWnd, uint crKey, byte bAlpha, uint dwFlags);

	public static int ExtendFrame(nint hWnd, Margins margins) =>
		DwmExtendFrameIntoClientArea(hWnd, ref margins);

	public static HResult SetWindowAttribute(nint hWnd, DwmWindowAttribute attribute, uint parameter) =>
		DwmSetWindowAttribute(hWnd, attribute, ref parameter, Marshal.SizeOf<uint>());

	private static bool CheckSupportSystemBackdropType() {
		HResult error = DwmGetWindowAttribute(0, DwmWindowAttribute.SystemBackdropType, out _, Marshal.SizeOf<uint>());
		return error != HResult.InvalidArg;
	}

	public static SupportSystemBackdropTypeLevel SupportSystemBackdropType { get; } =
		CheckSupportSystemBackdropType() ? SupportSystemBackdropTypeLevel.AcrylicMicaMicaAlt : // Windows 11 22H2 Build 22621
		WindowsVersion.Current switch {
			>= WindowsNT.Windows10_1803 => SupportSystemBackdropTypeLevel.AcrylicBlur,
			>= WindowsNT.Windows10 => SupportSystemBackdropTypeLevel.Blur,
			>= WindowsNT.Windows8 => SupportSystemBackdropTypeLevel.Colorization,
			>= WindowsNT.WindowsVista => SupportSystemBackdropTypeLevel.Aero,
			_ => SupportSystemBackdropTypeLevel.None,
		};

	public enum SupportSystemBackdropTypeLevel {
		None,
		Aero, // Windows Vista
		Colorization, // Windows 8
		Blur, // Windows 10 RTM
		AcrylicBlur, // Windows 10 1803
		AcrylicMicaMicaAlt, // Windows 11 22H2
	}

	/// <param name="hWnd">Window handle.</param>
	public static void AddExtendedWindowStyles(nint hWnd, params ExtendedWindowStyles[] styles) {
		long exStyle = GetWindowLongPtr(hWnd, WindowLongFlags.ExStyle);
		foreach (ExtendedWindowStyles style in styles)
			exStyle |= (long)style;
		SetWindowLongPtr(hWnd, WindowLongFlags.ExStyle, exStyle);
	}

	[DllImport("Ole32.dll")]
	public static extern int RevokeDragDrop(nint hWnd);

	[DllImport("Ole32.dll")]
	public static extern int RegisterDragDrop(nint hWnd, IOleDropTarget pDropTarget);

	[DllImport("User32.dll")]
	public static extern bool EnumChildWindows(nint hWndParent, EnumChildCallback lpEnumFunc, nint lParam);

	[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	public static extern int GetClassName(nint hWnd, StringBuilder lpClassname, int nMaxCount);

	public delegate bool EnumChildCallback(nint hWnd, nint lParam);

	private static bool EnumWindow(nint hWnd, nint lParam) {
		GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);
		if (gcChildhandlesList == null || gcChildhandlesList.Target == null) return false;
		StringBuilder buffer = new(128);
		GetClassName(hWnd, buffer, 128);
		if (buffer.ToString() == Chrome_WidgetWin) {
			List<nint>? childHandles = gcChildhandlesList.Target as List<nint>;
			childHandles?.Add(hWnd);
		}
		return true;
	}

	private static nint GetChildHandle(nint hWnd) {
		List<nint> childHandles = [];
		GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
		nint pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);
		try {
			EnumChildCallback childProc = new(EnumWindow);
			EnumChildWindows(hWnd, childProc, pointerChildHandlesList);
		} finally {
			gcChildhandlesList.Free();
		}
		return childHandles.FirstOrDefault();
	}

	public static void RevokeWebView2DragDropSwallow(System.Windows.Forms.Control owner) {
		nint chrome = GetChildHandle(owner.Handle);
		if (chrome == 0) return;
		DropTarget target = new(owner);
		RegisterDragDrop(chrome, target);
	}

	private const string Chrome_WidgetWin = "Chrome_RenderWidgetHostHWND";

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern nint GetSystemMenu(nint hWnd, bool bRevert);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool DeleteMenu(nint menu, uint uPosition, uint uFlags);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool EnableMenuItem(nint hMenu, uint uIDEnableItem, uint uEnable);

	/// <summary>
	/// Removes the specified menu items from the system menu. Such as restore, move, resize, minimize, maximize, close.
	/// </summary>
	/// <param name="hWnd">Handle of a window.</param>
	/// <param name="items">System window menu item.</param>
	public static void DeleteSystemMenuItems(nint hWnd, SystemMenuItemType items) {
		nint menu = GetSystemMenu(hWnd, false);
		foreach (KeyValuePair<SystemMenuItemType, uint> item in SystemMenuItemTag.Map)
			if ((items & item.Key) != 0)
				DeleteMenu(menu, item.Value, (uint)EnableMenuItemType.ByCommand);
	}

	/// <summary>
	/// Preserves the specified menu items from the system menu. That is the opposite of the
	/// <see cref="DeleteSystemMenuItems(nint, SystemMenuItemType)"/> method.
	/// </summary>
	/// <param name="hWnd">Handle of a window.</param>
	/// <param name="items">System window menu item.</param>
	public static void ReserveSystemMenuItems(nint hWnd, SystemMenuItemType items) =>
		DeleteSystemMenuItems(hWnd, ~items);

	/// <summary>
	/// Disable or enable the specified menu items from the system menu. Such as restore, move, resize, minimize, maximize, close.
	/// </summary>
	/// <param name="hWnd">Handle of a window.</param>
	/// <param name="items">System window menu item.</param>
	public static void DisableOrEnableSystemMenuItems(nint hWnd, SystemMenuItemType items, bool enabled) {
		nint menu = GetSystemMenu(hWnd, false);
		foreach (KeyValuePair<SystemMenuItemType, uint> item in SystemMenuItemTag.Map)
			if ((items & item.Key) != 0)
				EnableMenuItem(menu, item.Value, (uint)(EnableMenuItemType.ByCommand | (enabled ? EnableMenuItemType.Enabled : EnableMenuItemType.Grayed)));
	}

	private static class SystemMenuItemTag {
		public const uint RESTORE = 0xF120;
		public const uint MOVE = 0xF010;
		public const uint SIZE = 0xF000;
		public const uint MINIMIZE = 0xF020;
		public const uint MAXIMIZE = 0xF030;
		public const uint CLOSE = 0xF060;

		public static readonly Dictionary<SystemMenuItemType, uint> Map = new() {
			[SystemMenuItemType.Restore] = RESTORE,
			[SystemMenuItemType.Move] = MOVE,
			[SystemMenuItemType.Size] = SIZE,
			[SystemMenuItemType.Minimize] = MINIMIZE,
			[SystemMenuItemType.Maximize] = MAXIMIZE,
			[SystemMenuItemType.Close] = CLOSE,
		};
	}

	[Flags]
	public enum SystemMenuItemType {
		Restore = 1 << 0,
		Move = 1 << 1,
		Size = 1 << 2,
		Minimize = 1 << 3,
		Maximize = 1 << 4,
		Close = 1 << 5,
	}

	/// <summary>
	/// Controls the interpretation of the uIDEnableItem parameter and indicate whether the menu item is enabled, disabled, or grayed.
	/// This parameter must be a combination of the following values.
	/// </summary>
	/// <remarks><see href="https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-enablemenuitem#parameters" /></remarks>
	[Flags]
	private enum EnableMenuItemType : uint {
		/// <summary>
		/// Indicates that uIDEnableItem gives the identifier of the menu item.
		/// If neither the <see cref="ByCommand" /> nor <see cref="ByPosition" /> flag is specified, the <see cref="ByCommand" /> flag is the default flag.
		/// </summary>
		ByCommand = 0x00000000,
		/// <summary>
		/// Indicates that uIDEnableItem gives the zero-based relative position of the menu item.
		/// </summary>
		ByPosition = 0x00000400,
		/// <summary>
		/// Indicates that the menu item is disabled, but not grayed, so it cannot be selected.
		/// </summary>
		Disabled = 0x00000002,
		/// <summary>
		/// Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
		/// </summary>
		Enabled = 0x00000000,
		/// <summary>
		/// Indicates that the menu item is disabled and grayed so that it cannot be selected.
		/// </summary>
		Grayed = 0x00000001,
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AccentPolicy {
		public AccentState AccentState;
		public AccentFlags AccentFlags;
		/// <remarks>0xAABBGGRR</remarks>
		public uint GradientColor;
		public int AnimationId;
	}
	/// <summary>
	/// Set transparent backdrop by composition API, using <see cref="SetWindowCompositionAttribute" /> (Windows undocument API).
	/// </summary>
	/// <remarks>
	/// <para>Windows 10~11 only. For Windows 7, all values will have the same effect.</para>
	/// <para><a href="https://blog.walterlv.com/post/set-window-composition-attribute.html">Introduction</a></para>
	/// </remarks>
	public enum AccentState {
		/// <summary>
		/// No effect.
		/// </summary>
		/// <remarks>
		/// Black background, plain white border.
		/// </remarks>
		Disabled,
		/// <summary>
		/// Solid color gradient.
		/// </summary>
		/// <remarks>
		/// Gradient color background, dark border in inactive window.
		/// </remarks>
		EnableGradient,
		/// <summary>
		/// Transparent gradient.
		/// </summary>
		/// <remarks>
		/// Theme color background, dark border in inactive window.
		/// </remarks>
		EnableTransparentGradient,
		/// <summary>
		/// Older style blur (Windows 7-like).
		/// <para>Simulates blur effects of the Start Menu and Action Center in earlier versions of Windows 10. Acrylic was not proposed at that time.</para>
		/// </summary>
		/// <remarks>
		/// Blur effect background, gray border in inactive window.
		/// </remarks>
		EnableBlurBehind,
		/// <summary>
		/// Fluent Acrylic.
		/// </summary>
		/// <remarks>
		/// Acrylic effect that overlay with gradient color background.
		/// </remarks>
		EnableAcrylicBlurBehind,
		/// <summary>
		/// Uses system backdrop.
		/// <para>Old name: InvalidState</para>
		/// </summary>
		/// <remarks>
		/// Same as <see cref="Disabled"/>.
		/// </remarks>
		EnableHostBackdrop,
		/// <summary>
		/// Blur with region support.
		/// </summary>
		EnableBlurBehindWithBlurRegion,
		/// <summary>
		/// Acrylic but darker.
		/// </summary>
		EnableAcrylicBlurBehindBlack,
	}
	[Flags]
	public enum AccentFlags {
		None = 0,
		/// <summary>
		/// Enable this flag will cause the window size extending to the screen size.
		/// </summary>
		ExtendSize = 0x4,
		/// <summary>
		/// Enable the left border of the window.
		/// </summary>
		/// <remarks>
		/// Can be seen when <see cref="System.Windows.Window.WindowStyle" /> is <see cref="System.Windows.WindowStyle.None" />.
		/// </remarks>
		LeftBorder = 0x20,
		/// <summary>
		/// Enable the top border of the window.
		/// </summary>
		/// <inheritdoc cref="LeftBorder" />
		TopBorder = 0x40,
		/// <summary>
		/// Enable the right border of the window.
		/// </summary>
		/// <inheritdoc cref="LeftBorder" />
		RightBorder = 0x80,
		/// <summary>
		/// Enable the bottom border of the window.
		/// </summary>
		BottomBorder = 0x100,
		/// <summary>
		/// Merges them, enable all borders of the window.
		/// </summary>
		AllBorder = LeftBorder | TopBorder | RightBorder | BottomBorder,
	}
	/// <summary>
	/// Describes a key/value pair that specifies a window composition attribute and its value. This structure is used with the <see cref="GetWindowCompositionAttribute" />
	/// and <see cref="SetWindowCompositionAttribute" /> functions.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/en-us/windows/win32/dwm/windowcompositionattribdata">WINDOWCOMPOSITIONATTRIBDATA structure</a>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct WindowCompositionAttributeData {
		/// <summary>
		/// A flag describing which value to get or set, specified as a value of the <see cref="WindowCompositionAttribute" /> enumeration.
		/// This parameter specifies which attribute to get or set, and the <see cref="Data" /> member points to an object containing the attribute value.
		/// </summary>
		public WindowCompositionAttribute Attribute;
		/// <summary>
		/// When used with the <see cref="GetWindowCompositionAttribute" /> function, this member contains a pointer to a variable that will hold
		/// the value of the requested attribute when the function returns. When used with the <see cref="SetWindowCompositionAttribute" /> function,
		/// it points an object containing the attribute value to set. The type of the value set depends on the value of the Attrib member.
		/// For information about what type of value you should pass a pointer to in the pvData member, see <see cref="WindowCompositionAttribute" />.
		/// </summary>
		public nint Data;
		/// <summary>
		/// The size of the object pointed to by the <see cref="Data" /> member, in bytes.
		/// </summary>
		public int SizeOfData;
	}
	/// <summary>
	/// Specifies options used by the <see cref="WindowCompositionAttributeData" /> structure.
	/// </summary>
	/// <remarks>
	///	<para>Windows undocumented API enumeration.</para>
	///	<para><a href="https://learn.microsoft.com/windows/win32/dwm/windowcompositionattrib">WINDOWCOMPOSITIONATTRIB enumeration (incomplete API document)</a></para>
	/// </remarks>
	public enum WindowCompositionAttribute {
		Undefined,
		NCRenderingEnabled,
		NCRenderingPolicy,
		TransitionsForceDisabled,
		AllowNCPaint,
		CaptionButtonBounds,
		NonClientRtlLayout,
		ForceIconicRepresentation,
		ExtendedFrameBounds,
		HasIconicBitmap,
		ThemeAttributes,
		NCRenderingExiled,
		NCAdornmentInfo,
		ExcludedFromLivePreview,
		VideoOverlayActive,
		ForceActiveWindowAppearance,
		DisallowPeek,
		Cloak,
		Cloaked,
		AccentPolicy,
		FreezeRepresentation,
		EverUncloaked,
		VisualOwner,
		Last,
		/// <summary>
		/// Prevents a window from being captured by the Desktop Duplication API. The <c>pvData</c> member of the <see cref="WindowCompositionAttributeData" /> structure
		/// points to a value of type <see cref="bool" />. If the value is <see langword="true" />, the window is not captured. Otherwise, the window exhibits normal behavior.
		/// </summary>
		ExcludedFromDDA,
	}
	[DllImport("user32.dll")]
	public static extern bool GetWindowCompositionAttribute(nint hWnd, ref WindowCompositionAttributeData pAttrData);
	[DllImport("user32.dll")]
	public static extern bool SetWindowCompositionAttribute(nint hWnd, ref WindowCompositionAttributeData pAttrData);
	/// <summary>
	/// Using composition API to enable acrylic backdrop for a window, instead of <see cref="SystemBackdropType.TransientWindow" /> which takes no effect when the window
	/// is inactive.
	/// </summary>
	/// <remarks>
	/// Acrylic effect available since Windows 10 1803. However the API available since Windows 7.
	/// </remarks>
	/// <param name="hWnd">Handle of the window.</param>
	/// <param name="gradientColor">The tint color of the acrylic.</param>
	/// <returns>The current system supports to enable the acrylic backdrop using composition API?</returns>
	public static bool EnableAcrylicBlurBehind(nint hWnd, uint gradientColor = 0, AccentState backdrop = AccentState.EnableAcrylicBlurBehind) {
		AccentPolicy accent = new() {
			AccentState = backdrop,
			AccentFlags = AccentFlags.None,
			AnimationId = 0,
			GradientColor = gradientColor,
		};
		int accentStructSize = Marshal.SizeOf(accent);
		nint accentPtr = Marshal.AllocHGlobal(accentStructSize);
		Marshal.StructureToPtr(accent, accentPtr, false);
		WindowCompositionAttributeData data = new() {
			Attribute = WindowCompositionAttribute.AccentPolicy,
			Data = accentPtr,
			SizeOfData = accentStructSize,
		};
		bool ok = SetWindowCompositionAttribute(hWnd, ref data);
		Marshal.FreeHGlobal(accentPtr);
		return ok;
	}

	[DllImport("dwmapi.dll")]
	private static extern HResult DwmEnableBlurBehindWindow(IntPtr hWnd, ref DwmBlurBehind pBlurBehind);

	/// <summary>
	/// Specifies Desktop Window Manager (DWM) blur-behind properties. Used by the <see cref="DwmEnableBlurBehindWindow" /> function.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	private struct DwmBlurBehind {
		/// <summary>
		/// A bitwise combination of <a href="https://learn.microsoft.com/windows/desktop/dwm/dwm-bb-constants">DWM Blur Behind</a>
		/// constant values that indicates which of the members of this structure have been set.
		/// </summary>
		public DwmBlurBehindFlags Flags;
		/// <summary>
		/// <see langword="true" /> to register the window handle to DWM blur behind; <see langword="false" /> to unregister the window handle from DWM blur behind.
		/// </summary>
		public bool Enable;
		/// <summary>
		/// The region within the client area where the blur behind will be applied. An <see cref="IntPtr.Zero" /> value will apply the blur behind the entire client area.
		/// </summary>
		public IntPtr RgnBlur;
		/// <summary>
		/// <see langword="true" /> if the window's colorization should transition to match the maximized windows; otherwise, <see langword="false" />.
		/// </summary>
		public bool TransitionOnMaximized;
	}

	/// <summary>
	/// Flags used by the <see cref="DwmBlurBehind" /> structure to indicate which of its members contain valid information.
	/// </summary>
	[Flags]
	private enum DwmBlurBehindFlags : uint {
		/// <summary>
		/// Indicates a value for <see cref="DwmBlurBehind.Enable" /> has been specified.
		/// </summary>
		Enable = 0x00000001,
		/// <summary>
		/// Indicates a value for <see cref="DwmBlurBehind.RgnBlur" /> has been specified.
		/// </summary>
		BlurRegion = 0x00000002,
		/// <summary>
		/// Indicates a value for <see cref="DwmBlurBehind.TransitionOnMaximized" /> has been specified.
		/// </summary>
		TransitionOnMaximized = 0x00000004,
	}

	/// <summary>
	/// Enable Aero glass blur behind for a window.
	/// </summary>
	/// <remarks>
	/// Available for Windows Vista and Windows 7 only. For Windows 8.x, it only has a colored background.
	/// </remarks>
	/// <param name="hWnd">Handle of the window.</param>
	/// <returns>Enable blur behind successfully?</returns>
	public static bool EnableAeroBlurBehind(nint hWnd) {
		DwmBlurBehind blurBehindParameters = new() {
			Flags = DwmBlurBehindFlags.Enable,
			Enable = true,
			RgnBlur = IntPtr.Zero,
		};
		return DwmEnableBlurBehindWindow(hWnd, ref blurBehindParameters) == HResult.OK;
	}

	private enum PreferredAppMode {
		Default,
		AllowDark,
		ForceDark,
		ForceLight,
		Max,
	}

	[DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern int SetPreferredAppMode(PreferredAppMode preferredAppMode);

	[DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern void FlushMenuThemes();

	public static void EnableDarkSystemMenu(bool isDarkTheme) {
		SetPreferredAppMode(isDarkTheme ? PreferredAppMode.ForceDark : PreferredAppMode.ForceLight);
		FlushMenuThemes();
	}

	// https://blog.getpaint.net/2017/08/12/win32-how-to-get-the-refresh-rate-for-a-window/
	// https://github.com/rickbrew/RefreshRateWpf/blob/master/RefreshRateWpfApp/MainWindow.xaml.cs
	[DllImport("user32.dll", SetLastError = false)]
	private static extern nint MonitorFromWindow(nint hWnd, uint dwFlags);

	[StructLayout(LayoutKind.Sequential)]
	private struct Rect {
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private unsafe struct MonitorInfoExW {
		public uint cbSize;
		public Rect rcMonitor;
		public Rect rcWork;
		public uint dwFlags;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string szDevice;
	}

	[DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetMonitorInfoW(nint hMonitor, ref MonitorInfoExW lpmi);

	[DllImport("user32.dll", SetLastError = false, CharSet = CharSet.Unicode)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool EnumDisplaySettingsW([MarshalAs(UnmanagedType.LPWStr)] string lpszDeviceName, uint iModeNum, ref DevModeW lpDevMode);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct DevModeW {
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmDeviceName;

		public ushort dmSpecVersion;
		public ushort dmDriverVersion;
		public ushort dmSize;
		public ushort dmDriverExtra;
		public uint dmFields;

		// These next 4 int fields are a union with the above 8 shorts, but we don't need them right now
		public int dmPositionX;
		public int dmPositionY;
		public uint dmDisplayOrientation;
		public uint dmDisplayFixedOutput;

		public short dmColor;
		public short dmDuplex;
		public short dmYResolution;
		public short dmTTOption;
		public short dmCollate;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmFormName;

		public short dmLogPixels;
		public uint dmBitsPerPel;
		public uint dmPelsWidth;
		public uint dmPelsHeight;

		public uint dmNupOrDisplayFlags;
		public uint dmDisplayFrequency;

		public uint dmICMMethod;
		public uint dmICMIntent;
		public uint dmMediaType;
		public uint dmDitherType;
		public uint dmReserved1;
		public uint dmReserved2;
		public uint dmPanningWidth;
		public uint dmPanningHeight;
	}

	public readonly struct MonitorInfo {
		public uint Width { get; init; }
		public uint Height { get; init; }
		public uint Frequency { get; init; }

		public override string ToString() =>
			$"{Width} × {Height} @ {Frequency}Hz";
	}

	public static MonitorInfo? GetMonitorInfo(nint hWnd) {
		const uint MONITOR_DEFAULTTONEAREST = 2;
		const uint ENUM_CURRENT_SETTINGS = ~0u;

		nint hMonitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
		if (hMonitor == 0) return null;

		MonitorInfoExW monitorInfo = new() { cbSize = (uint)Marshal.SizeOf<MonitorInfoExW>() };
		if (!GetMonitorInfoW(hMonitor, ref monitorInfo)) return null;

		DevModeW devMode = new() { dmSize = (ushort)Marshal.SizeOf<DevModeW>() };
		if (!EnumDisplaySettingsW(monitorInfo.szDevice, ENUM_CURRENT_SETTINGS, ref devMode)) return null;

		return new MonitorInfo {
			Width = devMode.dmPelsWidth,
			Height = devMode.dmPelsHeight,
			Frequency = devMode.dmDisplayFrequency,
		};
	}

	/// <summary>
	/// Get the screen real bounds (size and position) without DPI scale.
	/// </summary>
	public static Rectangle GetPhysicalBounds(this Screen screen) {
		DevModeW devMode = new() { dmSize = (ushort)Marshal.SizeOf(typeof(DevModeW)) };
		EnumDisplaySettingsW(screen.DeviceName, ~0u, ref devMode);
		return new(devMode.dmPositionX, devMode.dmPositionY, (int)devMode.dmPelsWidth, (int)devMode.dmPelsHeight);
	}

	/// <summary>
	/// The following HRESULT values are the most common. More values are contained in the header file Winerror.h.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/windows/win32/seccrypto/common-hresult-values">Common HRESULT Values</a>
	/// </remarks>
	public enum HResult : long {
		/// <remarks>
		/// Operation successful.
		/// </remarks>
		OK = 0x00000000L,
		/// <remarks>
		/// Not implemented.
		/// </remarks>
		NotImpl = 0x80004001L,
		/// <remarks>
		/// No such interface supported.
		/// </remarks>
		NoInterface = 0x80004002L,
		/// <remarks>
		/// Pointer that is not valid.
		/// </remarks>
		Pointer = 0x80004003L,
		/// <remarks>
		/// Operation aborted.
		/// </remarks>
		Abort = 0x80004004L,
		/// <remarks>
		/// Unspecified failure.
		/// </remarks>
		Fail = 0x80004005L,
		/// <remarks>
		/// Unexpected failure.
		/// </remarks>
		Unexpected = 0x8000FFFFL,
		/// <remarks>
		/// General access denied error.
		/// </remarks>
		AccessDenied = 0x80070005L,
		/// <remarks>
		/// Handle that is not valid.
		/// </remarks>
		Handle = 0x80070006L,
		/// <remarks>
		/// Failed to allocate necessary memory.
		/// </remarks>
		OutOfMemory = 0x8007000EL,
		/// <remarks>
		/// One or more arguments are not valid.
		/// </remarks>
		InvalidArg = 0x80070057L,
	}

	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern bool RegisterShellHookWindow(nint hWnd);
	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern uint RegisterWindowMessage(string Message);
	[DllImport("User32.dll", CharSet = CharSet.Auto)]
	public static extern bool DeregisterShellHookWindow(nint hHandle);
	public enum ShellEvents {
		WindowCreated = 1,
		WindowDestroyed = 2,
		ActivateShellWindow = 3,
		WindowActivated = 4,
		GetMinRect = 5,
		Redraw = 6,
		TaskMan = 7,
		Language = 8,
		SysMenu = 9,
		EndTask = 10,
		AccessibilityState = 11,
		AppCommand = 12,
		WindowReplaced = 13,
		WindowReplacing = 14,
		HighBit = 0x8000,
		Flash = Redraw | HighBit,
		RudeappActivated = WindowActivated | HighBit,
	}

	[DllImport("User32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool IsWindowVisible(nint hWnd);

	/// <summary>
	/// Check if window is minimized.
	/// </summary>
	[DllImport("user32.dll")]
	public static extern bool IsIconic(nint hWnd);

	//[DllImport("user32.dll")]
	//private static extern nint GetForegroundWindow();

	[DllImport("user32.dll")]
	private static extern int GetWindowText(nint hWnd, StringBuilder text, int count);

	public static string? GetActiveWindowTitle() {
		const int nChars = 256;
		StringBuilder Buff = new(nChars);
		nint handle = GetActiveWindow();

		return GetWindowText(handle, Buff, nChars) > 0 ? Buff.ToString() : null;
	}

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern bool SystemParametersInfo(
		uint uiAction,
		uint uiParam,
		out uint pvParam,
		uint fWinIni
	);

	public enum MouseWheelScrollDirection {
		Vertical,
		Horizontal,
	}

	/// <summary>
	/// <para>Gets the number of characters a window scrolls horizontally when the mouse wheel is tilted.</para>
	/// <para>Or gets the number of lines a window scrolls vertically when the mouse wheel is rotated.</para>
	/// </summary>
	/// <remarks>
	/// For vertical scroll we can also use <see cref="System.Windows.Forms.SystemInformation.MouseWheelScrollLines" />,
	/// However for horizontal scroll it cannot help you.
	/// </remarks>
	/// <param name="direction">Mouse wheel scrolls horizontally or vertically?</param>
	/// <returns>
	/// <para>The number of characters / lines to scroll.</para>
	/// <para>Return -1 while <paramref name="direction" /> is vertical means the setting is window scrolls a page once the mouse wheel rotated.</para>
	/// </returns>
	public static int GetMouseWheelScrollSize(MouseWheelScrollDirection direction) {
		const uint GetWheelScrollChars = 0x006C; // For horizontal scroll chars.
		const uint GetWheelScrollLines = 0x0068; // For vertical scroll lines.

		// Call the SystemParametersInfo function with SPI_GETWHEELSCROLLCHARS / SPI_GETWHEELSCROLLCHARS.
		bool success = SystemParametersInfo(direction == MouseWheelScrollDirection.Horizontal ? GetWheelScrollChars : GetWheelScrollLines, 0, out uint scrollSize, 0);

		if (!success) {
			// Handle error or use a default value. ~~(e.g., 1, as suggested by older docs for XP/2000.)
			// The function will return 0 on failure, so default might be fine.~~
			// The SystemInformation.MouseWheelScrollLines property can be used as an alternative in System.Windows.Forms applications.
			// Use 3 as the fallback value because it is the default setting of Windows.
			Console.WriteLine("Failed to get scroll size setting via SystemParametersInfo. Defaulting to 3.");
			return 3;
		}

		return (int)scrollSize;
	}

	[DllImport("user32.dll")]
	public static extern unsafe bool InvalidateRect(nint hWnd, System.Windows.Int32Rect* lpRect, bool bErase);

	[DllImport("uxtheme.dll")]
	private static extern HResult SetWindowThemeAttribute(IntPtr hWnd, WindowThemeAttributeType wType, ref WindowThemeAttributeOptions attributes, uint size);

	/// <summary>
	/// Specifies the type of visual style attribute to set on a window.
	/// </summary>
	private enum WindowThemeAttributeType : uint {
		/// <summary>
		/// Non-client area window attributes will be set.
		/// </summary>
		NonClient = 1,
	}

	/// <summary>
	/// Defines options that are used to set window visual style attributes.
	/// </summary>
	private struct WindowThemeAttributeOptions {
		/// <summary>
		/// A combination of flags that modify window visual style attributes. Can be a combination of the <see cref="WindowThemeNonClientAttributes" /> constants.
		/// </summary>
		public WindowThemeNonClientAttributes Flags;
		/// <summary>
		/// A bitmask that describes how the values specified in <see cref="Flags" /> should be applied.
		/// If the bit corresponding to a value in <see cref="Flags" /> is 0, that flag will be removed. If the bit is 1, the flag will be added.
		/// </summary>
		public WindowThemeNonClientAttributes Mask;
	}

	/// <summary>
	/// Specifies flags that modify window visual style attributes. Use one, or a bitwise combination of the following values.
	/// </summary>
	[Flags]
	private enum WindowThemeNonClientAttributes : uint {
		/// <summary>
		/// Prevents the window caption from being drawn.
		/// </summary>
		NoDrawCaption = 1,
		/// <summary>
		/// Prevents the system icon from being drawn.
		/// </summary>
		NoDrawIcon = 2,
		/// <summary>
		/// Prevents the system icon menu from appearing.
		/// </summary>
		NoSysMenu = 4,
		/// <summary>
		/// Prevents mirroring of the question mark, even in right-to-left (RTL) layout.
		/// </summary>
		NoMirrorHelp = 8,
		/// <summary>
		/// A mask that contains all the valid bits.
		/// </summary>
		ValidBits = NoDrawCaption | NoDrawIcon | NoSysMenu | NoMirrorHelp,
	}

	/// <summary>
	/// Hide title bar caption and icon from a window, but keep caption and icon shown in the taskbar.
	/// </summary>
	/// <remarks>
	/// In Windows Vista and Windows 7, it only works in Windows Aero theme and Windows Basic theme, it doesn't work in Windows Classic theme and High contrast theme.
	/// </remarks>
	/// <param name="hWnd">Handle of the window.</param>
	/// <returns>
	/// Does this function succeed?
	/// <para><i>Returns <see langword="false" /> only if it is Windows Classic theme (includes high contrast theme) or Windows XP and before.</i></para>
	/// </returns>
	public static bool HideTitleBarCaptionAndIcon(IntPtr hWnd) {
		WindowThemeAttributeOptions options = new() {
			Flags = WindowThemeNonClientAttributes.NoDrawCaption | WindowThemeNonClientAttributes.NoDrawIcon | WindowThemeNonClientAttributes.NoSysMenu,
			Mask = WindowThemeNonClientAttributes.ValidBits,
		};
		return SetWindowThemeAttribute(hWnd, WindowThemeAttributeType.NonClient, ref options, (uint)Marshal.SizeOf(typeof(WindowThemeAttributeOptions))) == HResult.OK;
	}
}
