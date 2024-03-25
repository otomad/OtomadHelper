const opszs = {
	small: 1,
	text: 10.5,
	display: 36,
};
const weights = {
	thin: 100,
	hairline: 100,
	extralight: 200,
	ultralight: 200,
	light: 300,
	normal: 400,
	regular: 400,
	medium: 500,
	semibold: 600,
	demibold: 600,
	bold: 700,
	extrabold: 800,
	ultrabold: 800,
	black: 900,
	heavy: 900,
};
// declare const css: (styles: TemplateStringsArray, ...interpolations: Object[]) => string;
function useTypography(fontSize: number, lineHeight?: number, weight: keyof typeof weights | number = "regular", opsz: keyof typeof opszs | number = "text") {
	weight = typeof weight === "string" ? weights[weight] : weight;
	opsz = typeof opsz === "string" ? opszs[opsz] : opsz;
	return css`
		font-size: ${fontSize}px;
		font-weight: ${weight};
		font-variation-settings: "opsz" ${opsz};
		${lineHeight !== undefined ? css`line-height: ${lineHeight}px;` : ""}
	`;
}

export default {
	focus: (valueOnly: boolean = false) => {
		const value = `0 0 0 2px ${c("stroke-color-focus-stroke-inner")}, 0 0 0 4px ${c("stroke-color-focus-stroke-outer")}`;
		return valueOnly ? value : css`
			box-shadow: ${value};
			transition: ${fallbackTransitions}, box-shadow cubic-bezier(0.8, 2.15, 0.67, 1) 300ms; // TODO: 变量化，非硬编码。
		`;
	},
	/**
	 * 指定 Segoe UI Variable 字体的字号、字重、行高和光学尺寸轴 (Optical Size axis) 参数。
	 */
	text: {
		caption: useTypography(12, 16, "regular", "small"),
		body: useTypography(14, 20, "regular", "text"),
		bodyStrong: useTypography(14, 20, "semibold", "text"),
		bodyLarge: useTypography(18, 24, "regular", "text"),
		subtitle: useTypography(20, 28, "semibold", "display"),
		title: useTypography(28, 36, "semibold", "display"),
		titleLarge: useTypography(40, 52, "semibold", "display"),
		display: useTypography(68, 92, "semibold", "display"),
		icon: useTypography(20),
		iconSmall: useTypography(15),
		custom: useTypography,
	},
};
