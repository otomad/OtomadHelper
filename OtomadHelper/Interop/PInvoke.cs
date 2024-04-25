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

	[Flags]
	public enum ExtendedWindowStyles {
		// ...
		/// <remarks>
		/// The window is intended to be used as a floating toolbar. A tool window has a title bar that is
		/// shorter than a normal title bar, and the window title is drawn using a smaller font. A tool window
		/// does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB. If a
		/// tool window has a system menu, its icon is not displayed on the title bar. However, you can display
		/// the system menu by right-clicking or by typing ALT+SPACE.
		/// </remarks>
		ToolWindow = 0x00000080,
		// ...
	}

	public enum GetWindowLongFields {
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
	internal static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

	[DllImport("dwmapi.dll")]
	internal static extern int DwmSetWindowAttribute(IntPtr hWnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, GetWindowLongFields nIndex);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, GetWindowLongFields nIndex, IntPtr dwNewLong);

	public static int ExtendFrame(IntPtr hWnd, MARGINS margins) =>
		DwmExtendFrameIntoClientArea(hWnd, ref margins);

	public static int SetWindowAttribute(IntPtr hWnd, DwmWindowAttribute attribute, int parameter) =>
		DwmSetWindowAttribute(hWnd, attribute, ref parameter, Marshal.SizeOf<int>());

	/// <summary>
	/// Set the window as tool window window style, to remove the window from Alt + Tab.
	/// </summary>
	/// <param name="hWnd">Window handle.</param>
	public static void SetAsToolWindowMode(IntPtr hWnd) {
		int exStyle = (int)GetWindowLongPtr(hWnd, GetWindowLongFields.ExStyle);
		exStyle |= (int)ExtendedWindowStyles.ToolWindow;
		SetWindowLongPtr(hWnd, GetWindowLongFields.ExStyle, (IntPtr)exStyle);
	}
}
