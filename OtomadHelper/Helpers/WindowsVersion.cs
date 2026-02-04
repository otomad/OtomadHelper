namespace OtomadHelper.Helpers;

/// <summary>
/// A helper class to get current used Windows NT OS version.
/// </summary>
public static class WindowsVersion {
	/// <summary>
	/// Get current used Windows NT OS version.
	/// </summary>
	public static WindowsNT Current { get; } = Get(Environment.OSVersion.Version);

	/// <summary>
	/// Get a specific Windows NT OS version.
	/// </summary>
	public static WindowsNT Get(Version version) => version switch {
		{ Major: >= 10, Minor: > 0 } => WindowsNT.Next, // Future versions of Windows.
		{ Major: 10, Minor: 0, Build: >= 26200 } => WindowsNT.Windows11_25H2,
		{ Major: 10, Minor: 0, Build: >= 26100 } => WindowsNT.Windows11_24H2,
		{ Major: 10, Minor: 0, Build: >= 22631 } => WindowsNT.Windows11_23H2,
		{ Major: 10, Minor: 0, Build: >= 22621 } => WindowsNT.Windows11_22H2,
		{ Major: 10, Minor: 0, Build: >= 21996 } => WindowsNT.Windows11, // 10.0.21996 is the first Windows 11 version, not 10.0.22000 (RTM)!
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
		{ Major: 6, Minor: 4 } => WindowsNT.Windows10, // 6.4 is Windows 10 too.
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
	};
}

/// <summary>
/// Windows NT Consumer Versions.
/// </summary>
public enum WindowsNT : ushort {
	Unknown,
	WindowsNT3_1,
	WindowsNT3_5,
	WindowsNT3_51,
	WindowsNT4,
	Windows2000,
	WindowsXP,
	WindowsXP_x64,
	WindowsVista,
	WindowsVista_SP1,
	WindowsVista_SP2,
	WindowsVista_SP2_Update,
	Windows7,
	Windows7_SP1,
	Windows8,
	Windows8_1,
	Windows10,
	Windows10_1511,
	Windows10_1607,
	Windows10_1703,
	Windows10_1709,
	Windows10_1803,
	Windows10_1809,
	Windows10_1903,
	Windows10_1909,
	Windows10_2004,
	Windows10_20H2,
	Windows10_21H1,
	Windows10_21H2,
	Windows10_22H2,
	Windows11,
	Windows11_22H2,
	Windows11_23H2,
	Windows11_24H2,
	Windows11_25H2,
	Next = 65535,
}
