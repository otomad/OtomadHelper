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
	public static (double dpiX, double dpiY) GetDpi(this Control form) {
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
	/// <param name="window">A WPF <see cref="Window"/>.</param>
	/// <returns>The screen DPI in two dimension.</returns>
	public static (double dpiX, double dpiY) GetDpi(this Visual window) {
		PresentationSource source = PresentationSource.FromVisual(window);
		try {
			return (source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22);
		} catch {
			return (1, 1);
		}
	}

	/// <summary>
	/// Find a child of a given type in the visual tree of a <see cref="DependencyObject"/>.
	/// </summary>
	/// <typeparam name="T">The type of the child to find.</typeparam>
	/// <param name="sender">The <see cref="DependencyObject"/> to start the search from.</param>
	/// <returns>The first child of type <typeparamref name="T"/> found in the visual tree,
	/// or <see langword="null"/> if no such child is found.</returns>
	public static T? GetChildOfType<T>(this DependencyObject sender) where T : DependencyObject {
		if (sender is null) return null;
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sender); i++) {
			DependencyObject? child = VisualTreeHelper.GetChild(sender, i);
			T? result = (child as T) ?? GetChildOfType<T>(child);
			if (result != null) return result;
		}
		return null;
	}

	/// <summary>
	/// Find all children of a given type in the visual tree of a <see cref="DependencyObject"/>.
	/// </summary>
	/// <typeparam name="T">The type of the children to find.</typeparam>
	/// <param name="sender">The <see cref="DependencyObject"/> to start the search from.</param>
	/// <returns>A list of all children of type <typeparamref name="T"/> found in the visual tree.
	/// If no such children are found, an empty list is returned.</returns>
	public static List<T> GetChildrenOfType<T>(this DependencyObject sender) where T : DependencyObject {
		List<T> children = new();
		if (sender is null) return children;
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(sender); i++) {
			DependencyObject? child = VisualTreeHelper.GetChild(sender, i);
			if (child is T typedChild) children.Add(typedChild);
			if (VisualTreeHelper.GetChildrenCount(child) != 0)
				children.AddRange(GetChildrenOfType<T>(child));
		}
		return children;
	}

	/// <summary>
	/// Find the parent of a given <see cref="DependencyObject"/> in the visual tree.
	/// </summary>
	/// <param name="child">The <see cref="DependencyObject"/> to find the parent of.</param>
	/// <returns>The parent of the given <see cref="DependencyObject"/>, or <see langword="null"/> if no parent is found.</returns>
	public static DependencyObject? GetParentObject(this DependencyObject child) {
		if (child is null)
			return null;

		if (child is ContentElement contentElement) {
			DependencyObject parent = ContentOperations.GetParent(contentElement);
			return parent is not null ? parent :
				contentElement is FrameworkContentElement fce ? fce.Parent : null;
		}

		if (child is FrameworkElement frameworkElement) {
			DependencyObject parent = frameworkElement.Parent;
			if (parent is not null)
				return parent;
		}

		return VisualTreeHelper.GetParent(child);
	}
}
