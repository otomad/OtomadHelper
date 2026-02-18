using System.Drawing;
using System.Windows.Forms;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	private const double DPI_DIVISOR = 96d;

	extension(Control form) {
		/// <summary>
		/// Get the DPI of the screen where the WinForm <see cref="Form"/> is located.
		/// </summary>
		/// <remarks>
		/// Defaults to <c>(1, 1)</c> (Unit: dppx. Equivalents to 100% scale or 96dpi.)
		/// </remarks>
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

	extension(Control? parent) {
		/// <summary>
		/// Find all children of a given type <see cref="Control"/>.
		/// </summary>
		/// <typeparam name="T">The type of the children to find.</typeparam>
		/// <param name="parent">The <see cref="Control"/> to start the search from.</param>
		/// <param name="includeParent">Also includes the parent control itself?</param>
		/// <returns>A enumerable of all children of type <typeparamref name="T"/> found.
		/// If no such children are found, an empty enumerable is returned.</returns>
		public IEnumerable<T> GetChildrenOfType<T>(bool includeParent = false) where T : Control {
			if (parent is null) yield break;
			if (includeParent && parent is T expectedParent)
				yield return expectedParent;
			foreach (Control control in parent.Controls) {
				if (control is T expectedControl)
					yield return expectedControl;
				if (control.HasChildren)
					foreach (T child in control.GetChildrenOfType<T>())
						yield return child;
			}
		}
	}
}
