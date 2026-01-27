using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Data;

using Control = System.Windows.Forms.Control;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	extension(Window window) {
		/// <summary>
		/// Quickly get the handle to a WPF <see cref="Window"/>.
		/// </summary>
		/// <param name="window">A WPF <see cref="Window"/>.</param>
		/// <returns>The handle to the <see cref="Window"/>.</returns>
		public IntPtr Handle => new WindowInteropHelper(window).Handle;

		/// <summary>
		/// Force close window.
		/// </summary>
		/// <remarks>
		/// When run <see cref="Window.Close"/> in the <see cref="Window.Closing"/> event of <see cref="Window"/>,
		/// the following error will be reported:
		/// <code>Cannot set Visibility to Visible or call Show, ShowDialog, Close, or WindowInteropHelper.EnsureHandle while a Window is closing.</code>
		/// Use this method to resolve the issue and close the window successfully.
		/// </remarks>
		public async Task Vanish() =>
			await Dispatcher.CurrentDispatcher.InvokeAsync(window.Close, DispatcherPriority.Normal);
	}

	extension(Popup popup) {
		/// <summary>
		/// Quickly get the handle to a WPF <see cref="Popup"/>.
		/// </summary>
		/// <param name="popup">A WPF <see cref="Popup"/>.</param>
		/// <returns>The handle to the <see cref="Popup"/>.</returns>
		public IntPtr Handle => (PresentationSource.FromVisual(popup.Child) as HwndSource)?.Handle ?? IntPtr.Zero;
	}

	private const double DPI_DIVISOR = 96d;

	extension(Control form) {
		/// <summary>
		/// Get the DPI of the screen where the WinForm <see cref="Form"/> is located.
		/// </summary>
		/// <param name="form">A WinForm <see cref="Form"/>.</param>
		/// <returns>The screen DPI in two dimension.</returns>
		public (double DpiX, double DpiY) Dpi {
			get {
				Graphics graphics = form.CreateGraphics();
				try {
					return (graphics.DpiX / DPI_DIVISOR, graphics.DpiY / DPI_DIVISOR);
				} catch (Exception) {
					return (1, 1);
				} finally {
					graphics.Dispose();
				}
			}
		}
	}

	extension(Visual window) {
		/// <summary>
		/// Get the DPI of the screen where the WPF <see cref="Window"/> is located.
		/// </summary>
		/// <param name="window">A WPF <see cref="Window"/>.</param>
		/// <returns>The screen DPI in two dimension.</returns>
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public (double DpiX, double DpiY) Dpi {
			get {
				PresentationSource? source = PresentationSource.FromVisual(window);
				try {
					return (source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22);
				} catch (Exception) {
					return (1, 1);
				}
			}
		}
	}

	extension(DependencyObject? parent) {
		/// <summary>
		/// Find a child of a given type in the visual tree of a <see cref="DependencyObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the child to find.</typeparam>
		/// <param name="parent">The <see cref="DependencyObject"/> to start the search from.</param>
		/// <returns>The first child of type <typeparamref name="T"/> found in the visual tree,
		/// or <see langword="null"/> if no such child is found.</returns>
		public T? GetChildOfType<T>() where T : DependencyObject {
			if (parent is null)
				return null;
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
				DependencyObject? child = VisualTreeHelper.GetChild(parent, i);
				T? result = (child as T) ?? GetChildOfType<T>(child);
				if (result != null)
					return result;
			}
			return null;
		}

		/// <summary>
		/// Find all children of a given type in the visual tree of a <see cref="DependencyObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the children to find.</typeparam>
		/// <param name="parent">The <see cref="DependencyObject"/> to start the search from.</param>
		/// <returns>A list of all children of type <typeparamref name="T"/> found in the visual tree.
		/// If no such children are found, an empty list is returned.</returns>
		public List<T> GetChildrenOfType<T>() where T : DependencyObject {
			List<T> children = [];
			if (parent is null)
				return children;
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++) {
				DependencyObject? child = VisualTreeHelper.GetChild(parent, i);
				if (child is T typedChild)
					children.Add(typedChild);
				if (VisualTreeHelper.GetChildrenCount(child) != 0)
					children.AddRange(GetChildrenOfType<T>(child));
			}
			return children;
		}
	}

	extension(Control? parent) {
		/// <summary>
		/// Find all children of a given type <see cref="Control"/>.
		/// </summary>
		/// <typeparam name="T">The type of the children to find.</typeparam>
		/// <param name="parent">The <see cref="Control"/> to start the search from.</param>
		/// <param name="includeParent">Also includes the parent control itself?</param>
		/// <returns>A list of all children of type <typeparamref name="T"/> found.
		/// If no such children are found, an empty list is returned.</returns>
		public List<T> GetChildrenOfType<T>(bool includeParent = false) where T : Control {
			List<T> children = [];
			if (parent is null)
				return children;
			if (includeParent && parent is T expectedParent)
				children.Add(expectedParent);
			foreach (Control control in parent.Controls) {
				if (control is T expectedControl)
					children.Add(expectedControl);
				if (control.HasChildren)
					children.AddRange(control.GetChildrenOfType<T>());
			}
			return children;
		}
	}

	extension(DependencyObject? child) {
		/// <summary>
		/// Find the parent of a given <see cref="DependencyObject" /> in the visual tree.
		/// </summary>
		/// <param name="child">The <see cref="DependencyObject" /> to find the parent of.</param>
		/// <returns>The parent of the given <see cref="DependencyObject" />, or <see langword="null"/> if no parent is found.</returns>
		public DependencyObject? GetParent() {
			switch (child) {
				case null:
					return null;
				case ContentElement contentElement: {
					DependencyObject parent = ContentOperations.GetParent(contentElement);
					return parent ?? (contentElement is FrameworkContentElement fce ? fce.Parent : null);
				}
				case FrameworkElement frameworkElement: {
					DependencyObject parent = frameworkElement.Parent;
					if (parent is { })
						return parent;
					goto default;
				}
				default:
					return VisualTreeHelper.GetParent(child);
			}
		}
	}

	extension(DependencyObject child) {
		/// <summary>
		/// Find the parent of a given <typeparamref name="TElement" /> in the visual tree.
		/// </summary>
		/// <typeparam name="TElement">Type of parent.</typeparam>
		/// <param name="child">The <typeparamref name="TElement" /> to find the parent of.</param>
		/// <returns>The parent of the given <typeparamref name="TElement" />,
		/// or <see langword="null"/> if there is no parent of type <typeparamref name="TElement" />.</returns>
		public TElement? GetParent<TElement>() where TElement : DependencyObject {
			DependencyObject? parent;
			do
				parent = child.GetParent();
			while (parent is not (TElement or null));
			return parent as TElement;
		}
	}

	extension(FrameworkElement target) {
		/// <inheritdoc cref="BindingOperations.ClearBinding(DependencyObject, DependencyProperty)"/>
		public void ClearBinding(DependencyProperty dp) => BindingOperations.ClearBinding(target, dp);

		/// <inheritdoc cref="BindingOperations.ClearAllBindings(DependencyObject)"/>
		public void ClearBinding() => BindingOperations.ClearAllBindings(target);

		/// <summary>
		/// Get the index of the child in its parent.
		/// </summary>
		public int Index {
			get {
				if (target.Parent is not System.Windows.Controls.Panel parent) return -1;
				System.Windows.Controls.UIElementCollection children = parent.Children;
				int currentIndex = children.IndexOf(target);
				return currentIndex;
			}
		}
	}

	extension(System.Windows.Controls.TextBox textBox) {
		/// <summary>
		/// Check if a WPF <see cref="System.Windows.Controls.TextBox"/> is editable.
		/// </summary>
		/// <returns>The <see cref="System.Windows.Controls.TextBox"/> is editable?</returns>
		public bool IsEditable =>
			textBox is { IsEnabled: true, IsReadOnly: false, IsHitTestVisible: true };
	}
}
