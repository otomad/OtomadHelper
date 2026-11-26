using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Otomad.VegasScripts.OtomadHelper.V4 {

	/// <summary>
	/// Base class for windows forms with extended functionality.
	/// </summary>
	/// <remarks>
	/// <para>
	///     In order to use this class, create a new Form in Visual Studio, then modify the base
	///     class in the .cs (or in Visual Basic, the .Designer.vb) file for the form to this class.
	/// </para>
	/// <para>
	///     This class provides two main pieces of functionality: the ability to use the system font
	///     and the ability to use client area glass in Windows Vista.
	/// </para>
	/// <para>
	///     Windows Forms applications will normally always use MS Sans Serif 8pt, even when the
	///     system font is something else. For example, on Windows XP (English), the system font
	///     is Tahoma 8pt. On Windows XP Japanese, the system font is MS UI Gothic 8pt. On Windows
	///     Vista it is Segoe UI 9pt. By setting the <see cref="UseSystemFont"/> property to <see langword="true" /> for a form,
	///     that form will automatically use the proper system font.
	/// </para>
	/// <para>
	///     Because the font metrics of these different fonts can be vary greatly
	///     (especially on Vista which uses a larger font), please make sure the
	///     <see cref="ContainerControl.AutoScaleMode" /> property is set to
	///     <see cref="AutoScaleMode.Font"/>, and make special provisions for resizing
	///     graphics and other elements.
	///     Please note that some system fonts (such as MS UI Gothic) can also be smaller than MS Sans Serif
	///     which means the form will be scaled down, not up.
	/// </para>
	/// <note>
	///     If you set <see cref="UseSystemFont"/> to <see langword="true" />, it is strongly recommended you
	///     test your application with various different font and DPI settings.
	/// </note>
	/// <para>
	///     To use glass with your form, use the <see cref="GlassMargin"/> property. You can also
	///     specify whether it's possible to drag the form by its glass areas using the <see cref="AllowGlassDragging"/>
	///     property. Glass requires Windows Vista with the Desktop Window Manager enabled. The glass related
	///     properties have no effect on other operating systems.
	/// </para>
	/// <note>
	///     Many Windows Forms controls will not display correctly when placed over a glass area. Use the
	///     <see cref="Glass.DrawCompositedText"/> method to display text on a glass area.
	/// </note>
	/// <para>
	///     This class will automatically handle changes in the system font or DWM settings (including enabling/disabling
	///     of the DWM) while the application is running.
	/// </para>
	/// </remarks>
	/// <threadsafety instance="false" static="true" />
	public class ExtendedForm : Form {
		private Padding _glassMargin;
		private bool _allowGlassDragging = true;

		/// <summary>
		/// Raised when Desktop Window Manager (DWM) composition has been enabled or disabled.
		/// </summary>
		/// <remarks>
		/// <note>
		///   This event is only raised on Windows Vista or later.
		/// </note>
		/// <para>
		///   Use the <see cref="Glass.IsDwmCompositionEnabled"/> property to determine the
		///   current composition state.
		/// </para>
		/// </remarks>
		public event EventHandler DwmCompositionChanged;

		/// <summary>
		/// Creates a new instance of the <see cref="ExtendedForm" /> class.
		/// </summary>
		public ExtendedForm() {
		}

		/// <summary>
		/// Gets or sets a value that indicates whether or not the form automatically uses the system default font.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when the form's font is automatically adjusted to the system font;
		/// otherwise, <see langword="false" />. The default value is <see langword="false" />.
		/// </value>
		/// <remarks>
		/// <para>
		///   This property has no effect at design time; the font settings will only be applied at
		///   run time.
		/// </para>
		/// <para>
		///   When this property is set to <see langword="true" />, the form will use the configured
		///   system font and font size automatically, e.g. it will use Segoe UI on Windows Vista and
		///   Tahoma on Windows XP. Please make sure the <see cref="ContainerControl.AutoScaleMode" />
		///   property is set to <see cref="AutoScaleMode.Font"/>
		///   to account for the different metrics of the various fonts, and make special provisions
		///   to scale graphics and other element. Note that the system font can also be smaller than
		///   the font you used at design time, causing the form to be scaled down. For example the font
		///   MS Gothic UI, used on Japanese versions of Windows (pre-Vista), has smaller metrics than
		///   MS Sans Serif.
		/// </para>
		/// <note>
		///   It is strongly recommended to test your application with different font and DPI settings if you set this property
		///   to <see langword="true" />.
		/// </note>
		/// </remarks>
		[Category("Appearance"), DefaultValue(false), Description("Indicates whether or not the form automatically uses the system default font.")]
		public bool UseSystemFont { get; set; }

		/// <summary>
		/// Gets or sets the glass margins of the form.
		/// </summary>
		/// <value>
		/// A <see cref="Padding"/> that indicates the glass margins of the form. The default value is
		/// <see cref="Padding.Empty"/>.
		/// </value>
		/// <remarks>
		/// <note>
		///   Client-area glass requires Windows Vista or later with the Desktop Window Manager enabled. If the Desktop
		///   Window Manager is not enabled, or an older version of Windows is used, this property is ignored.
		/// </note>
		/// <para>
		///   Client-area glass extends the glass frame used by the Windows Vista Aero user interface into the client
		///   area of your window.
		/// </para>
		/// <para>
		///   Use negative margins to create the "sheet of glass" effect where the client area is rendered
		///   as a completely glass surface.
		/// </para>
		/// <para>
		///   Text rendered with the <see cref="TextRenderer" /> will not display correctly on the glass area (this includes
		///   text drawn by most controls). To draw text on the glass area, use <see cref="Glass.DrawCompositedText"/>.
		/// </para>
		/// <para>
		///   If the form is scaled, which can happen for instance if <see cref="UseSystemFont"/> is <see langword="true" />,
		///   the glass margin will also be scaled.
		/// </para>
		/// <para>
		///   At design time, the glass area will be indicated by a pattern drawn onto the form. This pattern will not
		///   be visible at runtime regardless of whether the glass is enabled or not.
		/// </para>
		/// </remarks>
		[Category("Appearance"), Description("The glass margins of the form.")]
		public Padding GlassMargin {
			get { return _glassMargin; }
			set {
				if (_glassMargin != value) {
					_glassMargin = value;
					EnableGlass();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the form can be dragged by the glass areas inside the client area.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when the form can be dragged using the client area glass; otherwise, <see langword="false" />.
		/// The default value is <see langword="false" />.
		/// </value>
		/// <remarks>
		/// This property has no effect on operating systems older than Windows Vista, when the Desktop Window Manager is disabled
		/// or when the <see cref="GlassMargin" /> property is set to zero.
		/// </remarks>
		[Category("Behavior"), Description("Indicates whether the form can be dragged by the glass areas inside the client area."), DefaultValue(true)]
		public bool AllowGlassDragging {
			get { return _allowGlassDragging; }
			set { _allowGlassDragging = value; }
		}

		/// <summary>
		/// Raises the <see cref="DwmCompositionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> containing the event data.</param>
		protected virtual void OnDwmCompositionChanged(EventArgs e) {
			EventHandler handler = DwmCompositionChanged;
			if (handler != null)
				handler(this, e);
		}

		/// <summary>
		/// Overrides the <see cref="Form.OnLoad"/> method.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e) {
			if (!DesignMode && UseSystemFont) {
				Font = System.Drawing.SystemFonts.IconTitleFont;
				Microsoft.Win32.SystemEvents.UserPreferenceChanged += new Microsoft.Win32.UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
			}
			base.OnLoad(e);
		}

		/// <summary>
		/// Overrides the <see cref="Form.OnFormClosed"/> method.
		/// </summary>
		/// <param name="e">A <see cref="FormClosedEventArgs"/> that contains the event data.</param>
		protected override void OnFormClosed(FormClosedEventArgs e) {
			base.OnFormClosed(e);
			Microsoft.Win32.SystemEvents.UserPreferenceChanged -= new Microsoft.Win32.UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="pevent">A <see cref="PaintEventArgs" /> that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent) {
			if (DesignMode || Glass.IsDwmCompositionEnabled) {
				if (DesignMode) {
					using (HatchBrush brush = new HatchBrush(HatchStyle.OutlinedDiamond, Color.SkyBlue, BackColor)) {
						PaintGlassArea(pevent, brush);
					}
				} else
					PaintGlassArea(pevent, Brushes.Black);
			} else
				base.OnPaintBackground(pevent);
		}

		/// <summary>
		/// This member overrides <see cref="Form.OnResize" />.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			if (_glassMargin.All != 0)
				Invalidate();
		}

		/// <summary>
		/// This member overrides <see cref="Form.OnHandleCreated" />.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e) {
			EnableGlass();
			base.OnHandleCreated(e);
		}

		/// <summary>
		/// This member overrides <see cref="Form.WndProc" />.
		/// </summary>
		/// <param name="m">The Windows <see cref="Message" /> to process.</param>
		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.LinkDemand, Flags = System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m) {
			base.WndProc(ref m);
			const int WM_NCHITTEST = 0x0084;
			const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
			switch (m.Msg) {
				case WM_NCHITTEST:
					if (_allowGlassDragging && m.Result == new IntPtr((int)HitTestResult.Client) && Glass.IsDwmCompositionEnabled) {
						if (_glassMargin.Left == -1 && _glassMargin.Top == -1 && _glassMargin.Right == -1 && _glassMargin.Bottom == -1) {
							m.Result = new IntPtr((int)HitTestResult.Caption);
							return;
						} else {
							Point p = new Point((int)m.LParam & 0xffff, (int)m.LParam >> 16);
							p = PointToClient(p);
							if (p.X < _glassMargin.Left || p.X > ClientSize.Width - _glassMargin.Right || p.Y < _glassMargin.Top || p.Y > ClientSize.Height - _glassMargin.Bottom) {
								m.Result = new IntPtr((int)HitTestResult.Caption);
								return;
							}
						}
					}
					break;
				case WM_DWMCOMPOSITIONCHANGED:
					// If the DWM has been disabled we don't need to do anything; if it was enabled we need to set the glass margins.
					if (_glassMargin.All != 0)
						EnableGlass();
					OnDwmCompositionChanged(EventArgs.Empty);
					m.Result = IntPtr.Zero;
					break;
			}
		}

		/// <summary>
		/// Overrides <see cref="Form.ScaleControl" />.
		/// </summary>
		/// <param name="factor">The height and width of the control's bounds.</param>
		/// <param name="specified">A <see cref="BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified) {
			float width = factor.Width;
			Padding newMargin = this.GlassMargin;
			if (width != 1f) {
				if (newMargin.Left > 0)
					newMargin.Left = (int)Math.Round(newMargin.Left * width);
				if (newMargin.Right > 0)
					newMargin.Right = (int)Math.Round(newMargin.Right * width);
			}
			float height = factor.Height;
			if (height != 1f) {
				if (newMargin.Top > 0)
					newMargin.Top = (int)Math.Round(newMargin.Top * height);
				if (newMargin.Bottom > 0)
					newMargin.Bottom = (int)Math.Round(newMargin.Bottom * height);
			}
			GlassMargin = newMargin;
			base.ScaleControl(factor, specified);
		}

		private void EnableGlass() {
			if (!DesignMode && Glass.IsDwmCompositionEnabled) {
				Glass.ExtendFrameIntoClientArea(this, GlassMargin);
				Invalidate();
			}
		}

		private void PaintGlassArea(PaintEventArgs pevent, Brush brush) {
			if (_glassMargin.Left == -1 && _glassMargin.Top == -1 && _glassMargin.Right == -1 && _glassMargin.Bottom == -1)
				pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
			else {
				Rectangle rect = new Rectangle(_glassMargin.Left, _glassMargin.Top, ClientSize.Width - _glassMargin.Right, ClientSize.Height - _glassMargin.Bottom);
				pevent.Graphics.FillRectangle(new System.Drawing.SolidBrush(BackColor), rect);

				if (_glassMargin.Left != 0)
					pevent.Graphics.FillRectangle(brush, new System.Drawing.Rectangle(0, 0, _glassMargin.Left, ClientSize.Height));
				if (_glassMargin.Right != 0)
					pevent.Graphics.FillRectangle(brush, new System.Drawing.Rectangle(ClientSize.Width - _glassMargin.Right, 0, ClientSize.Width, ClientSize.Height));
				if (_glassMargin.Top != 0)
					pevent.Graphics.FillRectangle(brush, new System.Drawing.Rectangle(0, 0, ClientSize.Width, _glassMargin.Top));
				if (_glassMargin.Bottom != 0)
					pevent.Graphics.FillRectangle(brush, new System.Drawing.Rectangle(0, ClientSize.Height - _glassMargin.Bottom, ClientSize.Width, ClientSize.Height));
			}
		}

		private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e) {
			if (e.Category == Microsoft.Win32.UserPreferenceCategory.Window && UseSystemFont)
				this.Font = System.Drawing.SystemFonts.IconTitleFont;
		}
	}

	/// <summary>
	/// Provides data for the <see cref="InputDialog.OkButtonClicked"/> event.
	/// </summary>
	/// <threadsafety instance="false" static="true" />
	public class OkButtonClickedEventArgs : CancelEventArgs {

		/// <summary>
		/// Initializes a new instance of the <see cref="OkButtonClickedEventArgs"/> class with the specified input text
		/// and input box window.
		/// </summary>
		/// <param name="input">The current value of the input field on the dialog.</param>
		/// <param name="inputBoxWindow">The input box window.</param>
		public OkButtonClickedEventArgs(string input, System.Windows.Forms.IWin32Window inputBoxWindow) {
			Input = input;
			InputBoxWindow = inputBoxWindow;
		}

		/// <summary>
		/// Gets the current value of the input field on the dialog.
		/// </summary>
		/// <value>
		/// The current value of the input field on the dialog.
		/// </value>
		/// <remarks>
		/// The <see cref="InputDialog.Input"/> property will not be updated until the dialog has been closed,
		/// so this property can be used to determine the value entered by the user when this event is raised.
		/// </remarks>
		public string Input { get; private set; }

		/// <summary>
		/// Gets the input box window.
		/// </summary>
		/// <value>
		/// The <see cref="System.Windows.Forms.IWin32Window"/> for the input box window.
		/// </value>
		/// <remarks>
		/// You can use this property if you need to display a modal dialog (for example to alert the user if the current input value
		/// is invalid) and you want the input box to be the parent of that dialog.
		/// </remarks>
		public System.Windows.Forms.IWin32Window InputBoxWindow { get; private set; }
	}

	static class DialogHelper {
		public static bool IsTaskDialogThemeSupported {
			get {
				return IsWindowsVistaOrLater &&
					VisualStyleRenderer.IsSupported &&
					Application.RenderWithVisualStyles;
			}
		}

		public static bool IsWindowsVistaOrLater {
			get {
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(6, 0, 6000);
			}
		}

		public static int GetTextHeight(IDeviceContext dc, string mainInstruction, string content, Font mainInstructionFallbackFont, Font contentFallbackFont, int width) {
			// compute the height the text needs at the current dialog width.
			Point location = Point.Empty;
			DrawText(dc, mainInstruction, content, ref location, mainInstructionFallbackFont, contentFallbackFont, true, width);
			return location.Y;
		}

		public static Size SizeDialog(IDeviceContext dc, string mainInstruction, string content, Screen screen, Font mainInstructionFallbackFont, Font contentFallbackFont, int horizontalSpacing, int verticalSpacing, int minimumWidth, int textMinimumHeight) {
			int width = minimumWidth - horizontalSpacing;
			int height = GetTextHeight(dc, mainInstruction, content, mainInstructionFallbackFont, contentFallbackFont, width);

			while (height > width) {
				int area = height * width;
				width = (int)(Math.Sqrt(area) * 1.1);
				height = GetTextHeight(dc, mainInstruction, content, mainInstructionFallbackFont, contentFallbackFont, width);
			}

			if (height < textMinimumHeight)
				height = textMinimumHeight;

			int newWidth = width + horizontalSpacing;
			int newHeight = height + verticalSpacing;

			Rectangle workingArea = screen.WorkingArea;
			if (newHeight > 0.9 * workingArea.Height) {
				int area = height * width;
				newHeight = (int)(0.9 * workingArea.Height);
				height = newHeight - verticalSpacing;
				width = area / height;
				newWidth = width + horizontalSpacing;
			}

			// If this happens the text won't display correctly, but even at 800x600 you need
			// to put so much text in the input box for this to happen that I don't care.
			if (newWidth > 0.9 * workingArea.Width)
				newWidth = (int)(0.9 * workingArea.Width);

			return new Size(newWidth, newHeight);
		}

		public static void DrawText(IDeviceContext dc, string text, VisualStyleElement element, Font fallbackFont, ref Point location, bool measureOnly, int width) {
			// For Windows 2000, using Int32.MaxValue for the height doesn't seem to work, so we'll just pick another arbitrary large value
			// that does work.
			Rectangle textRect = new Rectangle(location.X, location.Y, width, IsWindowsXPOrLater ? int.MaxValue : 100000);
			TextFormatFlags flags = TextFormatFlags.WordBreak;
			if (IsTaskDialogThemeSupported) {
				VisualStyleRenderer renderer = new VisualStyleRenderer(element);
				Rectangle textSize = renderer.GetTextExtent(dc, textRect, text, flags);
				location += new Size(0, textSize.Height);
				if (!measureOnly)
					renderer.DrawText(dc, textSize, text, false, flags);
			} else {
				if (!measureOnly)
					TextRenderer.DrawText(dc, text, fallbackFont, textRect, SystemColors.WindowText, flags);
				Size textSize = TextRenderer.MeasureText(dc, text, fallbackFont, new Size(textRect.Width, textRect.Height), flags);
				location += new Size(0, textSize.Height);
			}
		}

		public static bool IsWindowsXPOrLater {
			get {
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(5, 1, 2600);
			}
		}

		public static void DrawText(IDeviceContext dc, string mainInstruction, string content, ref Point location, Font mainInstructionFallbackFont, Font contentFallbackFont, bool measureOnly, int width) {
			if (!string.IsNullOrEmpty(mainInstruction))
				DrawText(dc, mainInstruction, AdditionalVisualStyleElements.TextStyle.MainInstruction, mainInstructionFallbackFont, ref location, measureOnly, width);

			if (!string.IsNullOrEmpty(content)) {
				if (!string.IsNullOrEmpty(mainInstruction))
					content = Environment.NewLine + content;

				DrawText(dc, content, AdditionalVisualStyleElements.TextStyle.BodyText, contentFallbackFont, ref location, measureOnly, width);
			}
		}
	}

	/// <summary>
	/// Contains classes that provide additional visual style elements not available through the regular
	/// <see cref="System.Windows.Forms.VisualStyles.VisualStyleElement"/> class.
	/// </summary>
	/// <threadsafety instance="false" static="false" />
	public static class AdditionalVisualStyleElements {
		/// <summary>
		/// Provides <see cref="System.Windows.Forms.VisualStyles.VisualStyleElement"/> objects for
		/// text styles.
		/// </summary>
		/// <remarks>
		/// <note>
		///   Use of these styles requires Windows Vista or a newer version of Windows.
		/// </note>
		/// </remarks>
		/// <threadsafety instance="false" static="false" />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public static class TextStyle {
			private const string _className = "TEXTSTYLE";

			private static VisualStyleElement _mainInstruction;
			private static VisualStyleElement _bodyText;

			/// <summary>
			/// Gets a visual style element that represents the main instruction in a dialog box.
			/// </summary>
			/// <value>
			/// A <see cref="VisualStyleElement"/> representing the style for the main instruction.
			/// </value>
			public static VisualStyleElement MainInstruction {
				get { return _mainInstruction ?? (_mainInstruction = VisualStyleElement.CreateElement(_className, 1, 0)); }
			}

			/// <summary>
			/// Gets a visual style element that represents the body text in a dialog box.
			/// </summary>
			/// <value>
			/// A <see cref="VisualStyleElement"/> representing the style for the body text.
			/// </value>
			public static VisualStyleElement BodyText {
				get { return _bodyText ?? (_bodyText = VisualStyleElement.CreateElement(_className, 4, 0)); }
			}
		}

		/// <summary>
		/// Provides <see cref="System.Windows.Forms.VisualStyles.VisualStyleElement"/> objects
		/// for task dialog related elements.
		/// </summary>
		/// <remarks>
		/// <note>
		///   Use of these styles requires Windows Vista or a newer version of Windows.
		/// </note>
		/// </remarks>
		/// <threadsafety instance="false" static="false" />
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
		public static class TaskDialog {
			private const string _className = "TASKDIALOG";

			private static VisualStyleElement _primaryPanel;
			private static VisualStyleElement _secondaryPanel;

			/// <summary>
			/// Gets a visual style element that represents the primary panel of a task dialog.
			/// </summary>
			/// <value>
			/// A <see cref="VisualStyleElement"/> representing the style for the primary panel.
			/// </value>
			public static VisualStyleElement PrimaryPanel {
				get { return _primaryPanel ?? (_primaryPanel = VisualStyleElement.CreateElement(_className, 1, 0)); }
			}

			/// <summary>
			/// Gets a visual style element that represents the secondary panel of a task dialog.
			/// </summary>
			/// <value>
			/// A <see cref="VisualStyleElement"/> representing the style for the secondary panel.
			/// </value>
			public static VisualStyleElement SecondaryPanel {
				get { return _secondaryPanel ?? (_secondaryPanel = VisualStyleElement.CreateElement(_className, 8, 0)); }
			}
		}
	}

	/// <summary>
	/// Provides functionality to use Aero Glass.
	/// </summary>
	/// <remarks>
	/// <para>
	///   This class provides functionality to extend the Windows Vista Aero glass window frame into a window's
	///   client area, and to draw text on the glass area.
	/// </para>
	/// <para>
	///   The <see cref="ExtendedForm"/> class provides more comprehensive support for client area glass than
	///   using <see cref="ExtendFrameIntoClientArea" /> by using its <see cref="ExtendedForm.GlassMargin"/> property.
	///   In addition to the basic client area glass support, <see cref="ExtendedForm"/> will also
	///   respond to changes in the Desktop Window Manager state and enable/disable client area glass as necessary. It also
	///   allows the window to be dragged using the client area glass areas using the <see cref="ExtendedForm.AllowGlassDragging"/>
	///   property.
	/// </para>
	/// <note>
	///   Use of glass requires Windows Vista or later with the Desktop Window Manager enabled.
	/// </note>
	/// </remarks>
	/// <threadsafety instance="false" static="true" />
	public static class Glass {
		/// <summary>
		/// Gets a value that indicates whether desktop composition is enabled.
		/// </summary>
		/// <value>
		/// <see langword="true" /> when desktop composition is supported and enabled; otherwise, <see langword="false" />.
		/// </value>
		public static bool IsDwmCompositionEnabled {
			get {
				return DialogHelper.IsWindowsVistaOrLater && DwmIsCompositionEnabled();
			}
		}

		[DllImport("dwmapi.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DwmIsCompositionEnabled();

		/// <summary>
		/// Extends the glass window frame into the client area of the specified window.
		/// </summary>
		/// <param name="window">The <see cref="IWin32Window"/> on which to enable client area glass.</param>
		/// <param name="glassMargin">The the margins to use when extending the frame into the client area.</param>
		/// <remarks>
		/// <para>
		///   Use negative margins to create the "sheet of glass" effect where the client area is rendered
		///   as a completely glass surface.
		/// </para>
		/// <para>
		///   The glass area must be filled with either a black brush or the <see cref="Form.TransparencyKey" /> color
		///   in order to display correctly. If the <see cref="Form.TransparencyKey"/> method is used, clicks in the
		///   glass area will "fall through" the window to the window below it. If the black brush method is used, text
		///   rendered with the <see cref="TextRenderer" /> will not display correctly on the glass area (this includes
		///   text drawn by most controls). To draw text on the glass area, use <see cref="DrawCompositedText"/>.
		/// </para>
		/// <para>
		///   This method needs to be called again if the state of the Desktop Window Manager is toggled.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="window"/> is <see langword="null" />.</exception>
		/// <exception cref="NotSupportedException">The current operating system does not support glass, or the Desktop Window Manager is not enabled.</exception>
		public static void ExtendFrameIntoClientArea(this System.Windows.Forms.IWin32Window window, Padding glassMargin) {
			if (!IsDwmCompositionEnabled)
				throw new NotSupportedException("The current operating system does not support glass, or the Desktop Window Manager is not enabled.");

			if (window == null)
				throw new ArgumentNullException("window");

			MARGINS margins = new MARGINS(glassMargin);
			DwmExtendFrameIntoClientArea(window.Handle, ref margins);
		}

		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, [In] ref MARGINS pMarInset);

		public struct MARGINS {
			public MARGINS(Padding value) {
				Left = value.Left;
				Right = value.Right;
				Top = value.Top;
				Bottom = value.Bottom;
			}
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}
	}

	public enum HitTestResult {
		Error = -2,
		Transparent = -1,
		Nowhere = 0,
		Client = 1,
		Caption = 2,
		SysMenu = 3,
		GrowBox = 4,
		Size = GrowBox,
		Menu = 5,
		HScroll = 6,
		VScroll = 7,
		MinButton = 8,
		MaxButton = 9,
		Left = 10,
		Right = 11,
		Top = 12,
		TopLeft = 13,
		TopRight = 14,
		Bottom = 15,
		BottomLeft = 16,
		BottomRight = 17,
		Border = 18,
		Reduce = MinButton,
		Zoom = MaxButton,
		SizeFirst = Left,
		SizeLast = BottomRight,
		Object = 19,
		Close = 20,
		Help = 21
	}
}
