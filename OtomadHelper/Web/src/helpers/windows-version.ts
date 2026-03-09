/**
 * Get current used Windows NT OS version.
 * @returns Windows NT enum.
 */
export async function getCurrentWindowsVersion() {
	const ua = navigator.userAgent;

	if (navigator.userAgentData?.platform !== "Windows") return WindowsNT.Unknown;
	try {
		const highEntropyValues = await navigator.userAgentData.getHighEntropyValues(["platformVersion"]);
		const { platformVersion } = highEntropyValues; // The format is commonly: "15.0.0"
		const major = parseInt(platformVersion.split(".")[0], 10);

		// Mapping table: mapping sub versions according to platformVersion (API Contract).
		// See: https://learn.microsoft.com/microsoft-edge/web-platform/how-to-detect-win11#detecting-specific-windows-versions
		if (major > 0)
			return (
				major >= 20 ? WindowsNT.Next :
				major >= 19 ? WindowsNT.Windows11_24H2 :
				major >= 15 ? WindowsNT.Windows11_22H2 :
				major >= 13 ? WindowsNT.Windows11 :
				major >= 10 ? WindowsNT.Windows10_2004 :
				major >= 8 ? WindowsNT.Windows10_1903 :
				major >= 7 ? WindowsNT.Windows10_1809 :
				major >= 6 ? WindowsNT.Windows10_1803 :
				major >= 5 ? WindowsNT.Windows10_1709 :
				major >= 4 ? WindowsNT.Windows10_1703 :
				major >= 3 ? WindowsNT.Windows10_1607 :
				major >= 2 ? WindowsNT.Windows10_1511 :
				WindowsNT.Windows10
			);
	} catch {
		// Client Hints acquisition failed, degraded parsing UA string.
	}

	// Processing Windows 8.1 and earlier legacy systems (resolving the User Agent)
	const legacyMap = [
		{ regexp: /Windows NT 6.3/, system: WindowsNT.Windows8_1 },
		{ regexp: /Windows NT 6.2/, system: WindowsNT.Windows8 },
		{ regexp: /Windows NT 6.1/, system: WindowsNT.Windows7 },
		{ regexp: /Windows NT 6.0/, system: WindowsNT.WindowsVista },
		{ regexp: /Windows NT 5.1/, system: WindowsNT.WindowsXP },
		{ regexp: /Windows NT 5.0/, system: WindowsNT.Windows2000 },
	];

	for (const item of legacyMap)
		if (item.regexp.test(ua))
			return item.system;

	return WindowsNT.Unknown;
}

/**
 * Windows NT Consumer Versions.
 *
 * @remarks
 * The value is the build number.
 */
export const enum WindowsNT {
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
	Windows10_TP = 9841,
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
	Windows11_Dev = 21996,
	Windows11 = 22000,
	Windows11_22H2 = 22621,
	Windows11_23H2 = 22631,
	Windows11_24H2 = 26100,
	Windows11_25H2 = 26200,
	Next = 65535,
}
