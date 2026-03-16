const opticalSizings = {
	small: 5,
	text: 10.5,
	display: 36,
} as const;

export const weights = {
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
} as const;

function typography(fontSize: number, lineHeight?: number, weight: keyof typeof weights | number = "regular", opticalSizing: keyof typeof opticalSizings | number = "text") {
	weight = typeof weight === "string" ? weights[weight] : weight;
	opticalSizing = typeof opticalSizing === "string" ? opticalSizings[opticalSizing] : opticalSizing;
	return css`
		font-size: ${fontSize}px;
		font-weight: ${weight};
		font-variation-settings: "opsz" ${opticalSizing};
		${lineHeight !== undefined ? css`line-height: ${lineHeight}px;` : ""}
	`;
}

const text = {
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 12px | 16px | regular | small
	 */
	caption: typography(12, 16, "regular", "small"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 14px | 20px | regular | text
	 */
	body: typography(14, 20, "regular", "text"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 14px | 20px | semibold | text
	 */
	bodyStrong: typography(14, 20, "semibold", "text"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 18px | 24px | regular | text
	 */
	bodyLarge: typography(18, 24, "regular", "text"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 20px | 28px | semibold | display
	 */
	subtitle: typography(20, 28, "semibold", "display"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 28px | 36px | semibold | display
	 */
	title: typography(28, 36, "semibold", "display"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 40px | 52px | semibold | display
	 */
	titleLarge: typography(40, 52, "semibold", "display"),
	/**
	 * font-size | line-height | font-weight | optical-sizing
	 * --- | --- | --- | ---
	 * 68px | 92px | semibold | display
	 */
	display: typography(68, 92, "semibold", "display"),
	/**
	 * font-size |
	 * --- |
	 * 20px |
	 */
	icon: typography(20),
	/**
	 * font-size |
	 * --- |
	 * 15px |
	 */
	iconSmall: typography(15),
	custom: typography,
};

export default text;
