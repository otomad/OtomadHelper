import segoeUiVF from "assets/fonts/SegoeUI-VF.ttf";

export const fonts = [
	new FontFace("Segoe UI Variable", `url("${segoeUiVF}") format("truetype-variations")`),
];

export async function initFonts() {
	for (const font of fonts) {
		document.fonts.add(font);
		font.load();
	}
	await document.fonts.ready;
}