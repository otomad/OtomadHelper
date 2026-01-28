using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	private enum ColorKeys : uint {
		White = 5114,
		LightGray,
		Gray,
		DarkGray,
		Black,
		Coral,
		Rose,
		LightOrange,
		Tan,
		LightYellow,
		LightGreen,
		Lime,
		Aqua,
		SkyBlue,
		LightTurquoise,
		PaleBlue,
		LightBlue,
		IceBlue,
		Periwinkle,
		Lavender,
		Pink,
		Red,
		Orange,
		Brown,
		Gold,
		Yellow,
		OliveGreen,
		Green,
		BrightGreen,
		Teal,
		Turquoise,
		Blue,
		BlueGray,
		Indigo,
		Purple,
		DarkRed,
		DarkYellow,
		DarkGreen,
		DarkTeal,
		DarkBlue,
		DarkPurple,
		Plum,
	}

	private static Dictionary<ColorKeys, string> ColorStrings { get; set; } = [];

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
		foreach (ColorKeys resourceId in Enum.GetValues<ColorKeys>()) {
			int length = LoadString(handle, (uint)resourceId, buffer, buffer.Capacity);
			ColorStrings[resourceId] = buffer.ToString(0, length);
		}
		FreeLibrary(handle);
	}

	public static string ToDisplayName() {
		if (!IsAvailable) return string.Empty;
		return ColorStrings[ColorKeys.White];
	}
}
