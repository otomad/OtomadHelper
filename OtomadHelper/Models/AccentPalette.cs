using System.Windows.Media;

namespace OtomadHelper.Models;

public class AccentPalette() : BaseWebMessageEvent {
	public Color Colorization { get; set; }
	public Color LightAccentColor { get; set; }
	public Color DarkAccentColor { get; set; }
}
