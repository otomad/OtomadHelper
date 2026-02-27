using System.Windows.Media;

using OtomadHelper.Services;

namespace OtomadHelper.Models;

public class AccentPalette() {
	public Color? Colorization { get; set; }
	public Color? LightAccentColor { get; set; }
	public Color? DarkAccentColor { get; set; }
}

public class SystemCursorConfigModel() {
	public int CursorSize { get; set; } = SystemCursorConfig.DEFAULT_SIZE;
	public Color CursorFill { get; set; } = SystemCursorConfig.DEFAULT_COLOR;
}

public class SystemConfig() : BaseWebMessageEvent {
	public AccentPalette AccentPalette { get; set; } = new();
	public SystemCursorConfigModel SystemCursorConfig { get; set; } = new();
	public Color PanelBackgroundColor { get; set; }
}
