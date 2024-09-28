using System.Diagnostics.CodeAnalysis;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color mediaColor) =>
		System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);

	public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color drawingColor) =>
		System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);

	public static uint ToAbgr(this System.Windows.Media.Color mediaColor, bool includeAlpha = true) =>
		(!includeAlpha ? 0 :(uint)mediaColor.A << 8 * 3) |
		(uint)mediaColor.B << 8 * 2 |
		(uint)mediaColor.G << 8 * 1 |
		(uint)mediaColor.R << 8 * 0;
}
