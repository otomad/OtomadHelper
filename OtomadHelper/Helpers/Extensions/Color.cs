using Wacton.Unicolour;

using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	public static DrawingColor ToDrawingColor(this MediaColor color) =>
		DrawingColor.FromArgb(color.A, color.R, color.G, color.B);

	public static MediaColor ToMediaColor(this DrawingColor color) =>
		MediaColor.FromArgb(color.A, color.R, color.G, color.B);

	public static uint ToAbgr(this MediaColor color, bool includeAlpha = true) =>
		(!includeAlpha ? 0 :(uint)color.A << 8 * 3) |
		(uint)color.B << 8 * 2 |
		(uint)color.G << 8 * 1 |
		(uint)color.R << 8 * 0;

	public static uint ToAbgr(this DrawingColor color, bool includeAlpha = true) =>
		(!includeAlpha ? 0 : (uint)color.A << 8 * 3) |
		(uint)color.B << 8 * 2 |
		(uint)color.G << 8 * 1 |
		(uint)color.R << 8 * 0;

	public static Unicolour ToUnicolour(this MediaColor color) =>
		new(ColourSpace.Rgb255, color.R, color.G, color.B, color.A);

	public static MediaColor ToMediaColor(this Unicolour color) {
		Rgb255 rgb = color.Rgb.Byte255;
		return MediaColor.FromArgb((byte)color.Alpha.A255, (byte)rgb.ConstrainedR, (byte)rgb.ConstrainedG, (byte)rgb.ConstrainedB);
	}

	public static string ToHex(this MediaColor color) {
		string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		if (color.A != 255) hex += color.A.ToString("X2");
		return hex;
	}
}
