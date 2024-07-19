import segoeUiVF from "assets/fonts/SegoeUI-VF.ttf";
import openHuninn from "assets/fonts/jf-openhuninn-2.0.woff2";

export const fonts = [
	// #region Segoe UI Variable
	new FontFace("Segoe UI Variable", `url("${segoeUiVF}") format("truetype-variations")`, { weight: "300 700", style: "normal" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Black")', { weight: "900", style: "normal" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Light Italic")', { weight: "300", style: "italic" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Semilight Italic")', { weight: "350", style: "italic" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Italic")', { weight: "400", style: "italic" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Semibold Italic")', { weight: "600", style: "italic" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Bold Italic")', { weight: "700", style: "italic" }),
	new FontFace("Segoe UI Variable", 'local("Segoe UI Black Italic")', { weight: "900", style: "italic" }),
	// #endregion

	new FontFace("Open Huninn", `url("${openHuninn}") format("woff2")`),
];

{ // Init fonts
	for (const font of fonts) {
		document.fonts.add(font);
		font.load();
	}
	await document.fonts.ready;
}
