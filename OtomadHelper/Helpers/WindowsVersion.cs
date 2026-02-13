namespace OtomadHelper.Helpers;

/// <summary>
/// A helper class to get current used Windows NT OS version.
/// </summary>
/// <remarks>
/// It is recommended to use API detection instead of version detection.
/// </remarks>
public static class WindowsVersion {
	/// <summary>
	/// Get current used Windows NT OS version.
	/// </summary>
	/// <remarks>
	/// It is recommended to use API detection instead of version detection.
	/// </remarks>
	public static WindowsNT Current { get; } = Get(Environment.OSVersion.Version);

	/// <summary>
	/// Get a specific Windows NT OS version.
	/// </summary>
	public static WindowsNT Get(Version version) {
		int build = version.Build;
		// Return directly if the current build is a exactly known build that declared in the enum.
		if (Enum.IsDefined(typeof(WindowsNT), build)) return (WindowsNT)build;
		// Iterate all known build from small to large in the enum, and find a build that is slightly smaller than it but does not greater than it.
		WindowsNT[] OSes = Enum.GetValues<WindowsNT>();
		WindowsNT current = (WindowsNT)build;
		for (int i = 1; i < OSes.Length; i++) {
			WindowsNT previous = OSes[i - 1], next = OSes[i];
			if (previous <= current && next > current) return previous;
		}
		return build <= 0 ? WindowsNT.Unknown : WindowsNT.Next;
	}

	#region Old implement
	/*public static WindowsNT Get(Version version) => version switch {
		{ Major: >= 10, Minor: > 0 } => WindowsNT.Next, // Future versions of Windows.
		{ Major: 10, Minor: 0, Build: >= 26200 } => WindowsNT.Windows11_25H2,
		{ Major: 10, Minor: 0, Build: >= 26100 } => WindowsNT.Windows11_24H2,
		{ Major: 10, Minor: 0, Build: >= 22631 } => WindowsNT.Windows11_23H2,
		{ Major: 10, Minor: 0, Build: >= 22621 } => WindowsNT.Windows11_22H2,
		{ Major: 10, Minor: 0, Build: >= 22000 } => WindowsNT.Windows11,
		{ Major: 10, Minor: 0, Build: >= 21996 } => WindowsNT.Windows11_Dev, // 10.0.21996 is the first Windows 11 version, not 10.0.22000 (RTM)!
		{ Major: 10, Minor: 0, Build: >= 19045 } => WindowsNT.Windows10_22H2, // Windows 10 version is from 10.0.10240 (RTM) through 10.0.19045 (22H2).
		{ Major: 10, Minor: 0, Build: >= 19044 } => WindowsNT.Windows10_21H2,
		{ Major: 10, Minor: 0, Build: >= 19043 } => WindowsNT.Windows10_21H1,
		{ Major: 10, Minor: 0, Build: >= 19042 } => WindowsNT.Windows10_20H2,
		{ Major: 10, Minor: 0, Build: >= 19041 } => WindowsNT.Windows10_2004,
		{ Major: 10, Minor: 0, Build: >= 18363 } => WindowsNT.Windows10_1909,
		{ Major: 10, Minor: 0, Build: >= 18362 } => WindowsNT.Windows10_1903,
		{ Major: 10, Minor: 0, Build: >= 17763 } => WindowsNT.Windows10_1809,
		{ Major: 10, Minor: 0, Build: >= 17134 } => WindowsNT.Windows10_1803,
		{ Major: 10, Minor: 0, Build: >= 16299 } => WindowsNT.Windows10_1709,
		{ Major: 10, Minor: 0, Build: >= 15063 } => WindowsNT.Windows10_1703,
		{ Major: 10, Minor: 0, Build: >= 14393 } => WindowsNT.Windows10_1607,
		{ Major: 10, Minor: 0, Build: >= 10586 } => WindowsNT.Windows10_1511,
		{ Major: 10, Minor: 0 } => WindowsNT.Windows10,
		{ Major: 6, Minor: 4 } => WindowsNT.Windows10_TP, // 6.4 is Windows 10 Technical Preview.
		{ Major: 6, Minor: 3 } => WindowsNT.Windows8_1,
		{ Major: 6, Minor: 2 } => WindowsNT.Windows8,
		{ Major: 6, Minor: 1, Build: 7601 } => WindowsNT.Windows7_SP1,
		{ Major: 6, Minor: 1 } => WindowsNT.Windows7,
		{ Major: 6, Minor: 0, Build: 6003 } => WindowsNT.WindowsVista_SP2_Update,
		{ Major: 6, Minor: 0, Build: 6002 } => WindowsNT.WindowsVista_SP2,
		{ Major: 6, Minor: 0, Build: 6001 } => WindowsNT.WindowsVista_SP1,
		{ Major: 6, Minor: 0 } => WindowsNT.WindowsVista,
		{ Major: 5, Minor: 2 } => WindowsNT.WindowsXP_x64, // 5.1 is Windows XP 32bit, 5.2 is Windows XP 64bit or Windows Server 2003.
		{ Major: 5, Minor: 1 } => WindowsNT.WindowsXP,
		{ Major: 5, Minor: 0 } => WindowsNT.Windows2000,
		{ Major: 4 } => WindowsNT.WindowsNT4,
		{ Major: 3, Minor: 51 } => WindowsNT.WindowsNT3_51,
		{ Major: 3, Minor: 5 } => WindowsNT.WindowsNT3_5,
		{ Major: 3, Minor: 1 } => WindowsNT.WindowsNT3_1,
		_ => WindowsNT.Unknown,
	};*/
	#endregion
}

/// <summary>
/// Windows NT Consumer Versions.
/// </summary>
/// <remarks>
/// The value is the build number.
/// </remarks>
public enum WindowsNT {
	Unknown = 0,
	WindowsNT3_1 = 528,
	WindowsNT3_5 = 807,
	WindowsNT3_51 = 1057,
	WindowsNT4 = 1381,
	Windows2000 = 2195,
	WindowsXP = 2600,
	WindowsXP_x64 = 3790,
	WindowsVista = 6000,
	WindowsVista_SP1 = 6001,
	WindowsVista_SP2 = 6002,
	WindowsVista_SP2_Update = 6003,
	Windows7 = 7600,
	Windows7_SP1 = 7601,
	Windows8 = 9200,
	Windows8_1 = 9600,
	Windows10_TP = 9841, // Windows 10 Technical Preview
	Windows10 = 10240,
	Windows10_1511 = 10586,
	Windows10_1607 = 14393,
	Windows10_1703 = 15063,
	Windows10_1709 = 16299,
	Windows10_1803 = 17134,
	Windows10_1809 = 17763,
	Windows10_1903 = 18362,
	Windows10_1909 = 18363,
	Windows10_2004 = 19041,
	Windows10_20H2 = 19042,
	Windows10_21H1 = 19043,
	Windows10_21H2 = 19044,
	Windows10_22H2 = 19045,
	Windows11_Dev = 21996, // 10.0.21996 is the first Windows 11 version, not 10.0.22000 (RTM)!
	Windows11 = 22000,
	Windows11_22H2 = 22621,
	Windows11_23H2 = 22631,
	Windows11_24H2 = 26100,
	Windows11_25H2 = 26200,
	Next = 65535, // Future versions of Windows.
}
