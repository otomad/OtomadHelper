using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

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
		/// Find all children of a given type in the visual tree of a <see cref="DependencyObject"/>.
		/// </summary>
		/// <typeparam name="T">The type of the children to find.</typeparam>
		/// <param name="parent">The <see cref="DependencyObject"/> to start the search from.</param>
		/// <returns>A enumerable of all children of type <typeparamref name="T"/> found in the visual tree.
		/// If no such children are found, an empty enumerable is returned.</returns>
		public IEnumerable<T> GetChildrenOfType<T>() where T : DependencyObject {
			if (parent is null) yield break;
			foreach (DependencyObject child in parent.Children) {
				if (child is T typedChild)
					yield return typedChild;
				if (parent.Children.Count != 0)
					foreach (T grandchild in GetChildrenOfType<T>(child))
						yield return grandchild;
			}
		}
	}

	extension(DependencyObject parent) {
		/// <inheritdoc cref="VisualTreeHelper.GetChild(DependencyObject, int)" />
		public VisualTreeChildren Children => new(parent);
	}

	public class VisualTreeChildren(DependencyObject parent) : IReadOnlyList<DependencyObject> {
		public int Count => VisualTreeHelper.GetChildrenCount(parent);
		public DependencyObject this[int index] => VisualTreeHelper.GetChild(parent, index);
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<DependencyObject> GetEnumerator() {
			for (int i = 0; i < Count; i++)
				yield return this[i];
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
				if (target.Parent is not Panel parent) return -1;
				UIElementCollection children = parent.Children;
				int currentIndex = children.IndexOf(target);
				return currentIndex;
			}
		}
	}

	extension(TextBox textBox) {
		/// <summary>
		/// Check if a WPF <see cref="TextBox"/> is editable.
		/// </summary>
		/// <returns>The <see cref="TextBox"/> is editable?</returns>
		public bool IsEditable =>
			textBox is { IsEnabled: true, IsReadOnly: false, IsHitTestVisible: true };
	}

	extension(Grid grid) {
		/// <summary>
		/// Get column and row index simultaneously of <see cref="UIElement" /> in a <see cref="Grid" />.
		/// </summary>
		/// <param name="element">A <see cref="UIElement" /> that in a <see cref="Grid" />.</param>
		/// <returns>A <see cref="ValueTuple{T1, T2}" /> that contains column and row.</returns>
		public (int column, int row) GetCellPosition(UIElement element) => (Grid.GetColumn(element), Grid.GetRow(element));

		/// <summary>
		/// Find children in a <see cref="Grid" /> by column and row index.
		/// </summary>
		/// <param name="cell">Column and row.</param>
		/// <returns>The children that found out, or <see langword="null" /> if not found.</returns>
		public IEnumerable<UIElement> FindChildrenByCellPosition((int column, int row) cell) {
			foreach (UIElement child in grid.Children)
				if (grid.GetCellPosition(child) == cell)
					yield return child;
		}

		/// <summary>
		/// Find children in a <see cref="Grid" /> by column and row index.
		/// </summary>
		/// <param name="cell">Column and row.</param>
		/// <returns>The children that found out, or <see langword="null" /> if not found.</returns>
		public IEnumerable<UIElement> FindChildrenByCellPosition(int column, int row) => grid.FindChildrenByCellPosition((column, row));
	}
}
