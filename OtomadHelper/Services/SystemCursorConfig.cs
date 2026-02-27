using System.Windows.Media;

using OtomadHelper.Interop;
using OtomadHelper.Models;

namespace OtomadHelper.Services;

public struct SystemCursorConfig {
	internal const int DEFAULT_SIZE = 32;
	internal static readonly Color DEFAULT_COLOR = Colors.White;

	public int Size { get; init; } = DEFAULT_SIZE;
	public SystemCursorType Type { get; init; } = SystemCursorType.WhiteBmp;
	public Color Color { get; init; } = DEFAULT_COLOR;

	public SystemCursorConfig() {
		using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors")) {
			if (key?.GetValue("CursorBaseSize") is int size)
				Size = size;
		}
		using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Accessibility")) {
			if (key?.GetValue("CursorType") is int type)
				Type = (SystemCursorType)type;
			if (key?.GetValue("CursorColor") is int color && Type == SystemCursorType.ColoredSvg)
				Color = Color.FromAbgr(color, false);
			if (Type is SystemCursorType.BlackBmp or SystemCursorType.BlackSvg)
				Color = Colors.Black;
		}
	}

	public static void SetSystemConfig(SystemCursorConfigModel config) {
		SystemCursorConfig cursorConfig = new();
		config.CursorSize = cursorConfig.Size;
		config.CursorFill = cursorConfig.Color;
	}

	public static event EventHandler? CursorChanged;
	static SystemCursorConfig() {
		RegistryMonitor monitor = new(RegistryHive.CurrentUser, @"Software\Microsoft\Accessibility") {
			RegChangeNotifyFilter = RegistryMonitor.RegChangeNotifyFilters.Value,
		};
		monitor.RegChanged += (sender, e) => CursorChanged?.Invoke(sender, e);
		monitor.Start();
	}
}

public enum SystemCursorType {
	WhiteBmp,
	BlackBmp,
	InvertBmp,
	WhiteSvg,
	BlackSvg,
	InvertSvg,
	ColoredSvg,
}
