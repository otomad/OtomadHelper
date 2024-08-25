// TODO: "PInvoke.cs" rename to "PInvoke_Win32.cs" and "PInvoke_DotNet.cs".
namespace OtomadHelper.Interop;

public static class PInvoke {
	/// <summary>
	/// Flags for specifying the system-drawn backdrop material of a window, including behind the non-client area.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwm_systembackdrop_type"><c>DWM_SYSTEMBACKDROP_TYPE enumeration (dwmapi.h)</c></a><br/>
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
		/// For more info about Mica, see <a href="https://learn.microsoft.com/en-us/windows/apps/design/style/mica">Mica</a>.
		/// </remarks>
		MainWindow,
		/// <summary>Acrylic</summary>
		/// <remarks>
		/// Draw the backdrop material effect corresponding to a transient window behind the entire window bounds.<br/>
		/// For Windows 11, this corresponds to Desktop Acrylic, also known as Background Acrylic, in its brightest variant.
		/// The material effect might change with future Windows releases. For more info about Desktop Acrylic, see
		/// <a href="https://learn.microsoft.com/en-us/windows/apps/design/style/acrylic">Acrylic</a>.
		/// </remarks>
		TransientWindow,
		/// <summary>MicaAlt</summary>
		/// <remarks>
		/// Draw the backdrop material effect corresponding to a window with a tabbed title bar behind the entire window bounds.<br/>
		/// For Windows 11, this corresponds to Mica in its alternate variant (Mica Alt). The material might change with future releases of Windows.
		/// For more info about Mica Alt, see <a href="https://learn.microsoft.com/en-us/windows/apps/design/style/mica#app-layering-with-mica-alt">Layering with Mica Alt</a>.
		/// </remarks>
		TabbedWindow,
	}

	/// <summary>
	/// Options used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute"><c>DWMWINDOWATTRIBUTE enumeration (dwmapi.h)</c></a><br/>
	/// </remarks>
	public enum DwmWindowAttribute {
		NCRenderingEnabled,
		NCRenderingPolicy,
		TransitionsForceDisabled,
		AllowNCPaint,
		CaptionButtonBounds,
		NonclientRtlLayout,
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
		UseHostBackdropbrush,
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
	/// Extended Window Styles.
	/// </summary>
	/// <remarks>
	/// <a href="https://learn.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles">Extended Window Styles</a><br/>
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

	public enum WindowLongFlags {
		// ...
		/// <summary>
		/// Retrieves the extended window styles.
		/// </summary>
		ExStyle = -20,
		// ...
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS {
		/// <summary>
		/// width of left border that retains its size
		/// </summary>
		public int cxLeftWidth;
		/// <summary>
		/// width of right border that retains its size
		/// </summary>
		public int cxRightWidth;
		/// <summary>
		/// height of top border that retains its size
		/// </summary>
		public int cyTopHeight;
		/// <summary>
		/// height of bottom border that retains its size
		/// </summary>
		public int cyBottomHeight;
	};

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(IntPtr hWnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);

	[DllImport("user32.dll")]
	public static extern long GetWindowLongPtr(IntPtr hWnd, WindowLongFlags nIndex);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongFlags nIndex, IntPtr dwNewLong);
	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongFlags nIndex, long dwNewLong);

	[DllImport("user32.dll")]
	public static extern IntPtr GetActiveWindow();

	[DllImport("user32.dll")]
	public static extern IntPtr SetActiveWindow(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

	public static int ExtendFrame(IntPtr hWnd, MARGINS margins) =>
		DwmExtendFrameIntoClientArea(hWnd, ref margins);

	public static int SetWindowAttribute(IntPtr hWnd, DwmWindowAttribute attribute, int parameter) =>
		DwmSetWindowAttribute(hWnd, attribute, ref parameter, Marshal.SizeOf<int>());

	/// <param name="hWnd">Window handle.</param>
	public static void AddExtendedWindowStyles(IntPtr hWnd, params ExtendedWindowStyles[] styles) {
		long exStyle = GetWindowLongPtr(hWnd, WindowLongFlags.ExStyle);
		foreach (ExtendedWindowStyles style in styles)
			exStyle |= (long)style;
		SetWindowLongPtr(hWnd, WindowLongFlags.ExStyle, exStyle);
	}

	[DllImport("Ole32.dll")]
	public static extern int RevokeDragDrop(IntPtr hWnd);

	[DllImport("Ole32.dll")]
	public static extern int RegisterDragDrop(IntPtr hWnd, IOleDropTarget pDropTarget);

	[DllImport("User32.dll")]
	public static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildCallback lpEnumFunc, IntPtr lParam);

	[DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassname, int nMaxCount);

	public delegate bool EnumChildCallback(IntPtr hWnd, IntPtr lParam);

	private static bool EnumWindow(IntPtr hWnd, IntPtr lParam) {
		GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);
		if (gcChildhandlesList == null || gcChildhandlesList.Target == null) return false;
		StringBuilder buf = new(128);
		GetClassName(hWnd, buf, 128);
		if (buf.ToString() == Chrome_WidgetWin) {
			List<IntPtr>? childHandles = gcChildhandlesList.Target as List<IntPtr>;
			childHandles?.Add(hWnd);
		}
		return true;
	}

	private static IntPtr GetChildHandle(IntPtr hWnd) {
		List<IntPtr> childHandles = new();
		GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
		IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);
		try {
			EnumChildCallback childProc = new(EnumWindow);
			EnumChildWindows(hWnd, childProc, pointerChildHandlesList);
		} finally {
			gcChildhandlesList.Free();
		}
		return childHandles.FirstOrDefault();
	}

	public static void RevokeWebView2DragDropSwallow(System.Windows.Forms.Control owner) {
		IntPtr chrome = GetChildHandle(owner.Handle);
		if (chrome == IntPtr.Zero) return;
		DropTarget target = new(owner);
		RegisterDragDrop(chrome, target);
	}

	private const string Chrome_WidgetWin = "Chrome_RenderWidgetHostHWND";

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern bool DeleteMenu(IntPtr menu, uint uPosition, uint uFlags);

	/// <summary>
	/// Removes the specified menu items from the system menu. Such as restore, move, resize, minimize, maximize, close.
	/// </summary>
	/// <param name="hWnd">Handle of a window.</param>
	/// <param name="items">System window menu item.</param>
	public static void DeleteSystemMenuItems(IntPtr hWnd, SystemMenuItemType items) {
		IntPtr menu = GetSystemMenu(hWnd, false);
		const uint MF_BYCOMMAND = 0x00000000;
		foreach (KeyValuePair<SystemMenuItemType, uint> item in SystemMenuItemTag.Map)
			if ((items & item.Key) != 0)
				DeleteMenu(menu, item.Value, MF_BYCOMMAND);
	}

	/// <summary>
	/// Preserves the specified menu items from the system menu. That is the opposite of the
	/// <see cref="DeleteSystemMenuItems(IntPtr, SystemMenuItemType)"/> method.
	/// </summary>
	/// <param name="hWnd">Handle of a window.</param>
	/// <param name="items">System window menu item.</param>
	public static void ReserveSystemMenuItems(IntPtr hWnd, SystemMenuItemType items) =>
		DeleteSystemMenuItems(hWnd, ~items);

	public static class SystemMenuItemTag {
		public const uint RESTORE = 0xF120;
		public const uint MOVE = 0xF010;
		public const uint SIZE = 0xF000;
		public const uint MINIMIZE = 0xF020;
		public const uint MAXIMIZE = 0xF030;
		public const uint CLOSE = 0xF060;

		public static readonly Dictionary<SystemMenuItemType, uint> Map = new() {
			{ SystemMenuItemType.RESTORE, RESTORE },
			{ SystemMenuItemType.MOVE, MOVE },
			{ SystemMenuItemType.SIZE, SIZE },
			{ SystemMenuItemType.MINIMIZE, MINIMIZE },
			{ SystemMenuItemType.MAXIMIZE, MAXIMIZE },
			{ SystemMenuItemType.CLOSE, CLOSE },
		};
	}

	[Flags]
	public enum SystemMenuItemType {
		RESTORE = 1 << 0,
		MOVE = 1 << 1,
		SIZE = 1 << 2,
		MINIMIZE = 1 << 3,
		MAXIMIZE = 1 << 4,
		CLOSE = 1 << 5,
	}
}
