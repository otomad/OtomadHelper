using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// Quickly get the handle to a WPF <see cref="Window"/>.
	/// </summary>
	/// <param name="window">A WPF <see cref="Window"/>.</param>
	/// <returns>The handle to the <see cref="Window"/>.</returns>
	public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).Handle;

	/// <summary>
	/// Quickly get the handle to a WPF <see cref="Popup"/>.
	/// </summary>
	/// <param name="window">A WPF <see cref="Popup"/>.</param>
	/// <returns>The handle to the <see cref="Popup"/>.</returns>
	public static IntPtr GetHandle(this Popup popup) => ((HwndSource)PresentationSource.FromVisual(popup.Child)).Handle;

	/// <summary>
	/// Force close window.
	/// </summary>
	/// <remarks>
	/// When run <see cref="Window.Close"/> in the <see cref="Window.Closing"/> event of <see cref="Window"/>,
	/// the following error will be reported:
	/// <code>Cannot set Visibility to Visible or call Show, ShowDialog, Close, or WindowInteropHelper.EnsureHandle while a Window is closing.</code>
	/// Use this method to resolve the issue and close the window successfully.
	/// </remarks>
	public static async void Vanish(this Window window) =>
		await Dispatcher.CurrentDispatcher.InvokeAsync(window.Close, DispatcherPriority.Normal);

	private const double DPI_DIVISOR = 96.0;

	/// <summary>
	/// Get the DPI of the screen where the WinForm <see cref="Form"/> is located.
	/// </summary>
	/// <param name="form">A WinForm <see cref="Form"/>.</param>
	/// <returns>The screen DPI in two dimension.</returns>
	public static (double dpiX, double dpiY) GetDpi(this Form form) {
		Graphics graphics = form.CreateGraphics();
		try {
			return (graphics.DpiX / DPI_DIVISOR, graphics.DpiY / DPI_DIVISOR);
		} catch {
			return (1, 1);
		} finally {
			graphics.Dispose();
		}
	}

	/// <summary>
	/// Get the DPI of the screen where the WPF <see cref="Window"/> is located.
	/// </summary>
	/// <param name="visual">A WPF <see cref="Window"/>.</param>
	/// <returns>The screen DPI in two dimension.</returns>
	public static (double dpiX, double dpiY) GetDpi(this Visual visual) {
		PresentationSource source = PresentationSource.FromVisual(visual);
		try {
			return (source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22);
		} catch {
			return (1, 1);
		}
	}
}
