/* eslint-disable import/order */
// import segoeUiVF from "assets/fonts/Segoe UI/SegoeUI-VF.ttf";
import inter from "assets/fonts/Inter/Inter-VF.woff2";
import interItalic from "assets/fonts/Inter/Inter-Italic-VF.woff2";
// import leteSansMath from "assets/fonts/Lete Sans Math/LeteSansMath.woff2";
// import leteSansMathBold from "assets/fonts/Lete Sans Math/LeteSansMath-Bold.woff2";
import openHuninn from "assets/fonts/Open Huninn/jf-openhuninn-2.0.woff2";
import yozoraSans from "assets/fonts/Yozora Sans/YozoraSans-VF.woff2";

export const fonts = [
	// Segoe UI Variable is too expensive.
	// #region Segoe UI Variable
	// new FontFace("Segoe UI Variable", `url("${segoeUiVF}") format("truetype-variations")`, { weight: "300 700", style: "normal" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Black")', { weight: "900", style: "normal" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Light Italic")', { weight: "300", style: "italic" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Semilight Italic")', { weight: "350", style: "italic" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Italic")', { weight: "400", style: "italic" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Semibold Italic")', { weight: "600", style: "italic" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Bold Italic")', { weight: "700", style: "italic" }),
	// new FontFace("Segoe UI Variable", 'local("Segoe UI Black Italic")', { weight: "900", style: "italic" }),
	// #endregion

	new FontFace("Inter", `url("${inter}") format("woff2")`, { weight: "100 900", style: "normal", sizeAdjust: "95%" }),
	new FontFace("Inter", `url("${interItalic}") format("woff2")`, { weight: "100 900", style: "italic", sizeAdjust: "95%" }),

	// new FontFace("Lete Sans Math", `url("${leteSansMath}") format("woff2")`, { weight: "normal" }),
	// new FontFace("Lete Sans Math", `url("${leteSansMathBold}") format("woff2")`, { weight: "bold" }),

	new FontFace("Open Huninn", `url("${openHuninn}") format("woff2")`),

	new FontFace("Yozora Sans", `url("${yozoraSans}") format("woff2")`, { weight: "250 900" }),
];

export const DEFAULT_FONT_FAMILY_NAME = "Inter";

{ // Initialize fonts
	for (const font of fonts) {
		document.fonts.add(font);
		font.load();
	}
	await document.fonts.ready;
}
