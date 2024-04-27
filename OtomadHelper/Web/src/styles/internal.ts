export type BorderRadiusPosition = "top-left" | "left-top" | "top-right" | "right-top" | "bottom-right" | "right-bottom" | "bottom-left" | "left-bottom" | "top" | "right" | "bottom" | "left" | "sharp-top-left" | "sharp-left-top" | "sharp-top-right" | "sharp-right-top" | "sharp-bottom-right" | "sharp-right-bottom" | "sharp-bottom-left" | "sharp-left-bottom" | "nwse" | "nesw" | "full";

export function setBorderRadius(radius: string, position: BorderRadiusPosition) {
	const positions: Record<BorderRadiusPosition, string> = {
		/* eslint-disable @stylistic/quote-props */
		"top-left": `${radius} 0 0 0`,
		"left-top": `${radius} 0 0 0`,
		"top-right": `0 ${radius} 0 0`,
		"right-top": `0 ${radius} 0 0`,
		"bottom-right": `0 0 ${radius} 0`,
		"right-bottom": `0 0 ${radius} 0`,
		"bottom-left": `0 0 0 ${radius}`,
		"left-bottom": `0 0 0 ${radius}`,
		"top": `${radius} ${radius} 0 0`,
		"right": `0 ${radius} ${radius} 0`,
		"bottom": `0 0 ${radius} ${radius}`,
		"left": `${radius} 0 0 ${radius}`,
		"sharp-top-left": `0 ${radius} ${radius} ${radius}`,
		"sharp-left-top": `0 ${radius} ${radius} ${radius}`,
		"sharp-top-right": `${radius} 0 ${radius} ${radius}`,
		"sharp-right-top": `${radius} 0 ${radius} ${radius}`,
		"sharp-bottom-right": `${radius} ${radius} 0 ${radius}`,
		"sharp-right-bottom": `${radius} ${radius} 0 ${radius}`,
		"sharp-bottom-left": `${radius} ${radius} ${radius} 0`,
		"sharp-left-bottom": `${radius} ${radius} ${radius} 0`,
		"nwse": `${radius} 0`, // I really don't know what word to use to refer to this situation, use this instead.
		"nesw": `0 ${radius} 0`, // I really don't know what word to use to refer to this situation, use this instead.
		"full": `${radius}`,
		/* eslint-enable @stylistic/quote-props */
	};
	return css`
		border-radius: ${positions[position]};
	`;
}
