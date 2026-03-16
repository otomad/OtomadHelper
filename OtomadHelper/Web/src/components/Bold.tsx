import { weights } from "styles/typographies";

export default function Bold({ bold = true, style, ...htmlAttrs }: FCP<{
	/**
	 * Activate the bold or provide the font weight value (number or enum).
	 * Enum | Number
	 * ---: | :---
	 * `false` | 500
	 * `true` | 700
	 * `"thin"` | 100
	 * `"hairline"` | 100
	 * `"extralight"` | 200
	 * `"ultralight"` | 200
	 * `"light"` | 300
	 * `"normal"` | 400
	 * `"regular"` | 400
	 * `"medium"` | 500
	 * `"semibold"` | 600
	 * `"demibold"` | 600
	 * `"bold"` | 700
	 * `"extrabold"` | 800
	 * `"ultrabold"` | 800
	 * `"black"` | 900
	 * `"heavy"` | 900
	 * @default true
	 */
	bold?: boolean | number | keyof typeof weights;
}, "strong">) {
	const fontWeight = typeof bold === "boolean" ? bold ? "bold" : "normal" :
		typeof bold === "string" ? weights[bold] : bold;
	return <strong style={{ ...style, fontWeight }} {...htmlAttrs} />;
}
