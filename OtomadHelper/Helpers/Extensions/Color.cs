using Wacton.Unicolour;

using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	extension(MediaColor color) {
		/// <summary>
		/// Converts a <see cref="MediaColor" /> to a <see cref="DrawingColor" />.
		/// </summary>
		public DrawingColor ToDrawingColor() =>
			DrawingColor.FromArgb(color.A, color.R, color.G, color.B);

		/// <summary>
		/// Converts a <see cref="MediaColor"/> to a 32-bit ABGR value.
		/// </summary>
		/// <param name="includeAlpha">Should include the alpha channel value of the color or use 0?</param>
		/// <returns>A 32-bit ABGR value representing the color.</returns>
		public uint ToAbgr(bool includeAlpha = true) =>
			(!includeAlpha ? 0 :(uint)color.A << 8 * 3) |
			(uint)color.B << 8 * 2 |
			(uint)color.G << 8 * 1 |
			(uint)color.R << 8 * 0;

		/// <summary>
		/// Converts a <see cref="MediaColor" /> to a <see cref="Unicolour" />.
		/// </summary>
		public Unicolour ToUnicolour() =>
			new(ColourSpace.Rgb255, color.R, color.G, color.B, color.A);

		/// <summary>
		/// Converts a <see cref="MediaColor" /> to a HEX color value (#RRGGBB[AA]).
		/// </summary>
		public string ToHex() {
			string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
			if (color.A != 255)
				hex += color.A.ToString("X2");
			return hex;
		}

		/// <summary>
		/// This function performs a quick calculation of the <i>perceived brightness</i> of a color,
		/// and takes into consideration ways that different channels in an RGB color value contribute
		/// to how bright it looks to the human eye. It uses all-integer math for speed on typical CPUs.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is not a model for real analysis of color brightness. It is good for quick calculations
		/// that require you to determine if a color can be classified as <i>light</i> or <i>dark</i>.
		/// Theme colors can often be light but not pure white, or dark but not pure black.
		/// </para>
		/// <para>
		/// Now that you have a function to check whether a color is light,
		/// you can use that function to detect if Dark mode is enabled.
		/// </para>
		/// <para>
		/// Dark mode is defined as a dark background with a contrasting light foreground.
		/// Since <c>IsColorLight</c> checks if a color is considered light, you can use that function
		/// to see if the foreground is light. If the foreground is light, then Dark mode is enabled.
		/// </para>
		/// <see href="https://learn.microsoft.com/zh-cn/windows/apps/desktop/modernize/ui/apply-windows-themes#know-when-dark-mode-is-enabled">
		/// Know when Dark mode is enabled</see>
		/// </remarks>
		public bool IsColorLight =>
			5 * color.G + 2 * color.R + color.B > 8 * 128;

		/// <summary>
		/// Converts a 32-bit ABGR value to a <see cref="MediaColor"/> object.
		/// </summary>
		/// <param name="value">The 32-bit ABGR value representing the color.</param>
		/// <param name="includeAlpha">
		/// Indicates whether the alpha channel should be included.
		/// If <c>true</c>, the alpha channel is extracted from the ABGR value;
		/// otherwise, the alpha channel is set to 255 (fully opaque).
		/// </param>
		/// <returns>A <see cref="MediaColor"/> object representing the color.</returns>
		public static MediaColor FromAbgr(uint value, bool includeAlpha = true) => MediaColor.FromArgb(
			(byte)(includeAlpha ? (value >> 8 * 3) : 0xff),
			(byte)(value >> 8 * 0),
			(byte)(value >> 8 * 1),
			(byte)(value >> 8 * 2)
		);

		/// <inheritdoc cref="FromAbgr(uint, bool)" />
		/// <remarks>
		/// Due to the <see cref="RegistryKey.GetValue(string)" /> method converting a <see langword="DWORD" /> value to an <see cref="int" /> type,
		/// this is usually not what we want. This overload method forces the conversion of <see cref="int" /> to <see cref="uint" /> for ease of use.
		/// </remarks>
		public static MediaColor FromAbgr(int value, bool includeAlpha = true) => FromAbgr(unchecked((uint)value), includeAlpha);
	}

	extension(DrawingColor color) {
		/// <summary>
		/// Converts a <see cref="DrawingColor" /> to a <see cref="MediaColor" />.
		/// </summary>
		public MediaColor ToMediaColor() =>
			MediaColor.FromArgb(color.A, color.R, color.G, color.B);

		/// <summary>
		/// Converts a <see cref="DrawingColor"/> to a 32-bit ABGR value.
		/// </summary>
		/// <param name="includeAlpha">Should include the alpha channel value of the color or use 0?</param>
		/// <returns>A 32-bit ABGR value representing the color.</returns>
		public uint ToAbgr(bool includeAlpha = true) =>
			(!includeAlpha ? 0 : (uint)color.A << 8 * 3) |
			(uint)color.B << 8 * 2 |
			(uint)color.G << 8 * 1 |
			(uint)color.R << 8 * 0;

		/// <summary>
		/// Convert a HEX color value (#RRGGBB[AA]) or HTML entity color to a <see cref="DrawingColor" />.
		/// </summary>
		/// <exception cref="Exception">The input <paramref name="hex" /> is invalid or cannot be recognized by C#.</exception>
		public static DrawingColor FromHex(string hex) {
			if (hex.StartsWith("#")) {
				// Transform from #RRGGBBAA to #AARRGGBB.
				if (hex.Length == 9) hex = "#" + hex.Substring(7, 2) + hex.Substring(1, 6);
				else if (hex.Length == 5) hex = "#" + hex[4].Repeat(2) + hex[1].Repeat(2) + hex[2].Repeat(2) + hex[3].Repeat(2);
			}
			return System.Drawing.ColorTranslator.FromHtml(hex);
		}
	}

	extension(Unicolour color) {
		/// <summary>
		/// Converts a <see cref="Unicolour" /> to a <see cref="MediaColor" />.
		/// </summary>
		public MediaColor ToMediaColor() {
			Rgb255 rgb = color.Rgb.Byte255;
			return MediaColor.FromArgb((byte)color.Alpha.A255, (byte)rgb.ConstrainedR, (byte)rgb.ConstrainedG, (byte)rgb.ConstrainedB);
		}
	}
}

// C# 14 static methods in extension member with same name will conflict with the static methods in the same class.
// So I have to declare them in a separate partial class to avoid the conflict.
// see: https://github.com/dotnet/csharplang/discussions/9537
public static partial class Extensions_Conflict_2 {
	extension(MediaColor color) {
		/// <summary>
		/// Convert a HEX color value (#RRGGBB[AA]) or HTML entity color to a <see cref="MediaColor" />.
		/// </summary>
		/// <exception cref="Exception">The input <paramref name="hex" /> is invalid or cannot be recognized by C#.</exception>
		public static MediaColor FromHex(string hex) =>
			DrawingColor.FromHex(hex).ToMediaColor();
	}
}
