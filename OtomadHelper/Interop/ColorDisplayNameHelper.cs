using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OtomadHelper.Interop;

public static class ColorDisplayNameHelper {
	static ColorDisplayNameHelper() {
		UpdateColorStrings(Culture);
		CultureChanged += UpdateColorStrings;
	}

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern IntPtr LoadLibrary(string lpFileName);

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
	private static extern int LoadString(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool FreeLibrary(IntPtr hModule);

	private enum KnownColors : uint {
		White = 5114, LightGray, Gray, DarkGray, Black, Coral, Rose, LightOrange, Tan, LightYellow, LightGreen, Lime, Aqua, SkyBlue, LightTurquoise, PaleBlue, LightBlue, IceBlue, Periwinkle, Lavender, Pink, Red, Orange, Brown, Gold, Yellow, OliveGreen, Green, BrightGreen, Teal, Turquoise, Blue, BlueGray, Indigo, Purple, DarkRed, DarkYellow, DarkGreen, DarkTeal, DarkBlue, DarkPurple, Plum,
	}

	private static Dictionary<KnownColors, string> ColorStrings { get; set; } = [];

	public static bool IsAvailable => ColorStrings.Count > 0;

	private static void UpdateColorStrings(CultureInfo culture) {
		ColorStrings = [];
		string systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
		IntPtr handle = LoadLibrary($@"{systemRoot}\System32\{culture}\Windows.UI.Xaml.dll.mui");
		if (handle == IntPtr.Zero)
			handle = LoadLibrary($@"{systemRoot}\System32\Windows.UI.Xaml.dll");
		if (handle == IntPtr.Zero)
			return; // Windows 7 or earlier.
		StringBuilder buffer = new(1024);
		foreach (KnownColors resourceId in Enum.GetValues<KnownColors>()) {
			int length = LoadString(handle, (uint)resourceId, buffer, buffer.Capacity);
			ColorStrings[resourceId] = buffer.ToString(0, length);
		}
		FreeLibrary(handle);
	}

	public static string ToDisplayName(Color color) {
		if (!IsAvailable) return string.Empty;
		uint id = GetColorNameResourceId(color);
		return ColorStrings[(KnownColors)id];
	}

	#region Resource IDs Lookup Tables
	private static readonly int[] HueLimitsForSatLevel4 = [0, 11, 26, 0, 0, 38, 45, 0, 0, 56, 100, 121, 129, 0, 140, 0, 180, 0, 0, 224, 241, 0, 256];
	private static readonly int[] HueLimitsForSatLevel5 = [0, 13, 27, 0, 0, 36, 45, 0, 0, 59, 118, 0, 127, 136, 142, 0, 185, 0, 0, 216, 239, 0, 256];
	private static readonly int[] HueLimitsForSatLevel3 = [0, 8, 0, 0, 39, 46, 0, 0, 0, 71, 120, 0, 131, 144, 0, 0, 163, 0, 177, 211, 249, 0, 256];
	private static readonly int[] HueLimitsForSatLevel2 = [0, 10, 0, 32, 46, 0, 0, 0, 61, 0, 106, 0, 136, 144, 0, 0, 0, 158, 166, 241, 0, 0, 256];
	private static readonly int[] HueLimitsForSatLevel1 = [8, 0, 0, 44, 0, 0, 0, 63, 0, 0, 122, 0, 134, 0, 0, 0, 0, 166, 176, 241, 0, 256, 0];
	private static readonly int[] LumLimitsForHueIndexHigh = [170, 170, 170, 155, 170, 170, 170, 170, 170, 115, 170, 170, 170, 170, 170, 170, 170, 170, 150, 150, 170, 140, 165];
	private static readonly int[] LumLimitsForHueIndexLow = [130, 100, 115, 100, 100, 100, 110, 75, 100, 90, 100, 100, 100, 100, 80, 100, 100, 100, 100, 100, 100, 100, 100];
	private static readonly uint[] ColorNamesMid = [5119, 5135, 5136, 5137, 5122, 5138, 5139, 5140, 5140, 5141, 5141, 5142, 5143, 5126, 5144, 5129, 5145, 5146, 5147, 5148, 5134, 5137, 5135];
	private static readonly uint[] ColorNamesDark = [5137, 5149, 5137, 5137, 5137, 5150, 5150, 5137, 5151, 5151, 5151, 5151, 5152, 5152, 5152, 5153, 5153, 5146, 5147, 5154, 5155, 5137, 5149];
	private static readonly uint[] ColorNamesLight = [5119, 5120, 5121, 5122, 5122, 5123, 5123, 5122, 5124, 5125, 5124, 5124, 5126, 5127, 5128, 5129, 5130, 5131, 5132, 5133, 5134, 5122, 5120];
	#endregion

	private static (double h, double s, double l) ToHsl(Color color) {
		double r = color.R / 255d, g = color.G / 255d, b = color.B / 255d;
		double max = Enumerable.Max([r, g, b]), min = Enumerable.Min([r, g, b]);

		double h = 0, s = 0, l = (max + min) / 2, delta = max - min;

		if (Math.Abs(delta) > double.Epsilon) {
			s = delta / (l > 0.5 ? (2 - max - min) : (max + min));

			double deltaR = ((max - r) / 6 + delta / 2) / delta,
				deltaG = ((max - g) / 6 + delta / 2) / delta,
				deltaB = ((max - b) / 6 + delta / 2) / delta;

			h = Math.Abs(r - max) < double.Epsilon ? deltaB - deltaG :
				Math.Abs(g - max) < double.Epsilon ? 1 / 3d + deltaR - deltaB : 2 / 3d + deltaG - deltaR;

			if (h < 0) h += 1;
			if (h > 1) h -= 1;
		}

		return new(h, s, l);
	}

	private static uint GetColorNameResourceId(Color color) {
		(double h, double s, double l) = ToHsl(color);
		h *= 255; s *= 255; l *= 255;

		if (l > 240) return 5114;
		if (l < 20) return 5118;

		if (s > 20) {
			int[] hueLimits = s switch {
				> 240 => HueLimitsForSatLevel5,
				> 150 => HueLimitsForSatLevel4,
				> 115 => HueLimitsForSatLevel3,
				> 75 => HueLimitsForSatLevel2,
				_ => HueLimitsForSatLevel1,
			};

			int hueIndex = 0;
			while (hueIndex < 23 && hueLimits[hueIndex] <= h)
				hueIndex++;

			return l > LumLimitsForHueIndexHigh[hueIndex] ? ColorNamesLight[hueIndex] :
				l >= LumLimitsForHueIndexLow[hueIndex] ? ColorNamesMid[hueIndex] : ColorNamesDark[hueIndex];
		} else
			return l <= 170.0 ? l <= 100.0 ? (uint)5117 : 5116 : 5115;
	}
}
