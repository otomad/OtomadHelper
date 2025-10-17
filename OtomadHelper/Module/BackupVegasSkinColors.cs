using System.Drawing;

using ScriptPortal.MediaSoftware.Skins;

using VegasSkinColors = ScriptPortal.MediaSoftware.Skins.SkinColors;

namespace OtomadHelper.Module;

public static class BackupVegasSkinColors {
	/// <summary>
	/// Earlier VEGAS Pro skin colors, which used in VEGAS Pro 13 ~ 14.
	/// </summary>
	public static SkinColors VP13 = new(136, 0, 122, 171, 105, Color.FromArgb(254, 181, 123));

	/// <summary>
	/// Early VEGAS Pro skin colors, which used in VEGAS Pro 15 ~ 18.
	/// </summary>
	public static Interfaces VP15 = new(Color.FromArgb(25, 140, 254),
		new(45, 220, 55, 70, 35),
		new(94, 220, 114, 130, 85),
		new(146, 29, 154, 167, 139),
		new(210, 35, 226, 242, 197)
	);

	/// <summary>
	/// Middle VEGAS Pro skin colors, which used in VEGAS Pro 19 ~ 22.
	/// </summary>
	public static Interfaces VP19 = new(Color.FromArgb(25, 140, 254),
		new(34, 220, 51, 68, 17),
		new(68, 255, 85, 102, 51),
		new(187, 17, 153, 136, 170),
		new(238, 51, 204, 187, 221)
	);

	/// <summary>
	/// Modern VEGAS Pro skin colors, which used in VEGAS Pro 23 ~.
	/// </summary>
	public static Interfaces VP23 = new(Color.FromArgb(60, 140, 220),
		new(40, 240, 60, 80, 20),
		new(80, 240, 100, 120, 60),
		new(160, 20, 180, 200, 140),
		new(200, 20, 220, 240, 180)
	);

	public record Interfaces(Color Highlight, SkinColors Dark, SkinColors Medium, SkinColors Light, SkinColors White) {
		public SkinColors Dark { get; } = Dark with { Highlight = Highlight };
		public SkinColors Medium { get; } = Medium with { Highlight = Highlight };
		public SkinColors Light { get; } = Light with { Highlight = Highlight };
		public SkinColors White { get; } = White with { Highlight = Highlight };
	}
}

/// <summary>
/// Represent to VEGAS Pro skin colors.
/// </summary>
/// <param name="Background">UI background color (<see cref="VegasSkinColors.ButtonFace" />).</param>
/// <param name="Foreground">UI text color (<see cref="VegasSkinColors.ButtonText" />).</param>
/// <param name="ButtonNormal">Button normal state color (<see cref="VegasSkinColors.ControlLight" />).</param>
/// <param name="ButtonHover">Button hover state color (<see cref="VegasSkinColors.ControlLightLight" />).</param>
/// <param name="ButtonPressed">Button pressed state color (<see cref="VegasSkinColors.ControlDarkDark" />).</param>
/// <param name="Highlight">Selected item background color (<see cref="VegasSkinColors.Highlight" />).</param>
public record SkinColors(Color Background, Color Foreground, Color ButtonNormal, Color ButtonHover, Color ButtonPressed, Color Highlight) {
	/// <inheritdoc cref="SkinColors" />
	public SkinColors(byte Background, byte Foreground, byte ButtonNormal, byte ButtonHover, byte ButtonPressed, Color? Highlight = null) : this(Gray(Background), Gray(Foreground), Gray(ButtonNormal), Gray(ButtonHover), Gray(ButtonPressed), Highlight ?? Color.Transparent) { }

	/// <summary>
	/// Create a gray color from a single lightness value.
	/// </summary>
	/// <param name="lightness">The lightness of the gray color.</param>
	/// <returns>Gray color.</returns>
	private static Color Gray(byte lightness) => Color.FromArgb(lightness, lightness, lightness);

	/// <summary>
	/// Get skin colors of the current VEGAS Pro version, or get the latest version if it is not in the VEGAS environment.
	/// </summary>
	public static SkinColors Current {
		get {
#if !VEGAS_ENV
			return BackupVegasSkinColors.VP23.Dark;
#else
			VegasSkinColors c = Skins.Colors;
			return new(c.ButtonFace, c.ButtonText, c.ControlLight, c.ControlLightLight, c.ControlDarkDark, c.Highlight);
#endif
		}
	}

}
