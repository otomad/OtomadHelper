using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Drawing;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).Handle;

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

	public static (double x, double y) GetDpi(this Form form) {
		const double DIVISOR = 96.0;
		Graphics graphics = form.CreateGraphics();
		try {
			return (graphics.DpiX / DIVISOR, graphics.DpiY / DIVISOR);
		} catch {
			return (1, 1);
		} finally {
			graphics.Dispose();
		}
	}
}
