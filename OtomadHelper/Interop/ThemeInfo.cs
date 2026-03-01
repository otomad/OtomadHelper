namespace OtomadHelper.Interop;

/// <summary>
/// Get information about current Windows theme.
/// </summary>
/// <remarks>
/// <see href="https://gist.github.com/thomaslevesque/3653097" />
/// </remarks>
public class ThemeInfo {
	private ThemeInfo(string name, string fileName, string color, string size) {
		ThemeName = name;
		NormalColor = color;
		ThemeSize = size;
		ThemeFileName = fileName;
	}

	private ThemeInfo(string name, StringBuilder fileName, StringBuilder color, StringBuilder size) :
		this(name, fileName.ToString(), color.ToString(), size.ToString()) { }

	public string ThemeName { get; }
	public string NormalColor { get; }
	public string ThemeSize { get; }
	public string ThemeFileName { get; }

	/// <summary>
	/// Get information about current Windows theme.
	/// </summary>
	public static ThemeInfo Current {
		get {
			StringBuilder fileName = NewBuffer(), color = NewBuffer(), size = NewBuffer();
			int hresult = GetCurrentThemeName(fileName, fileName.Capacity, color, color.Capacity, size, size.Capacity);
			if (hresult < 0) throw Marshal.GetExceptionForHR(hresult);
			string themeName = new Path(fileName).FileRoot;
			return new(themeName, fileName, color, size);

			static StringBuilder NewBuffer() => new(260);
		}
	}

	[DllImport("uxtheme.dll", CharSet = CharSet.Auto)]
	private static extern int GetCurrentThemeName(
		StringBuilder pszThemeFileName,
		int dwMaxNameChars,
		StringBuilder pszColorBuff,
		int cchMaxColorChars,
		StringBuilder pszSizeBuff,
		int cchMaxSizeChars
	);
}
