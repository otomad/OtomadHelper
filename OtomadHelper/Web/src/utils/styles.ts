/*
 * Forward modules from the `styles` directory.
 */

import type { ColorNames, SystemColors } from "styles/colors";
import eases from "styles/eases";
import effects from "styles/effects";
import { type AvailableLottieStatus, STATUS_PREFIX } from "styles/fake-animations";
import mixins from "styles/mixins";

export { ifColorScheme } from "styles/colors";

export /* @internal */ const FALLBACK_TRANSITIONS = `all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} 100ms, visibility 0s, font-feature-settings 0s` as const;
export const fallbackTransitions = "var(--fallback-transitions)" as typeof FALLBACK_TRANSITIONS;

/**
 * Apply the theme color.
 * @remarks "c" stands for "color".
 * @param cssVarName - The CSS property name of the color. Does not need to add "--" before it. It can also be `white` or `black`.
 * @param alpha - Alpha value, note that it is a percentage value rather than a decimal value between 0 and 1. If left blank, it indicates an opaque color.
 * @returns The custom property solid color called by `var()`, or the translucent color encapsulated by relative color function `rgba(from ...)`.
 * @throws {RangeError} The `alpha` parameter value out of range [0 ~ 100].
 */
export function c(cssVarName: string & {} | "white" | "black" | ColorNames, alpha?: number | string) {
	if (typeof alpha === "number" && (Number.isNaN(alpha) || alpha < 0 || alpha > 100) || alpha === "")
		throw new RangeError("The alpha parameter should be in range [0 ~ 100]");
	if (cssVarName === "white" || cssVarName === "black")
		return alpha === undefined ? cssVarName :
			typeof alpha === "number" ?
				"#" + (cssVarName === "white" ? "f" : "0").repeat(6) + Math.round(alpha / 100 * 255).toString(16).padStart(2, "0") :
				`rgb(${cssVarName === "white" ? "255 255 255" : "0 0 0"} / ${alpha})`;
	return alpha === undefined ? `var(--${cssVarName})` :
		`rgb(from var(--${cssVarName}) r g b / calc(alpha * ${typeof alpha === "number" ? alpha + "%" : alpha}))`;
	// `--t(var(--${cssVarName}), ${typeof alpha === "number" ? alpha + "%" : alpha})`;
	// WARN: Bug in Chromium. See: https://issues.chromium.org/issues/473120371
}

/**
 * Apply the system color which used in high contrast color theme.
 * @remarks "cc" stands for "contrast color".
 * @param systemColor - CSS system color name.
 * @returns System color.
 */
export const cc = (systemColor: SystemColors) => systemColor;

/**
 * Make your selector higher in priority.
 * @param priority - The priority, the bigger the number, the higher it is. Less than or equal to zero has no effect.
 * Defaults to 1.
 * @returns
 * ```less
 * // before
 * .your-selector${important(5)} { }
 * // after
 * .your-selector:not[#\#]:not[#\#]:not[#\#]:not[#\#]:not[#\#] { }
 * ```
 */
export function important(priority: number = 1) {
	return ":not(#\\#)".repeat(priority);
}

function toValue_css(value: string | number | undefined) {
	return typeof value === "number" ? value + "px" : value;
}

export { eases };

export const styles = {
	mixins,
	effects,
	/**
	 * Get the corresponding CSS style value based on a string or number.
	 *
	 * Add "px" unit if the value is a number, or return the value itself if it already has a unit.
	 *
	 * This function takes a value as input and returns a CSS value. If the input value is a number,
	 * it is converted to a CSS length value with the unit "px". Otherwise, the input value is returned
	 * as is. This function is used to convert the calculated position values to CSS values.
	 *
	 * @remarks A function name that given by GPT is **`pxify`**.
	 *
	 * @param value
	 * - If the value passed in is a number, its corresponding pixel value is returned;
	 * if the value passed in is a string, the original string value is returned;
	 * if the value passed in is an undefined, the undefined is returned.
	 * - The CSS value, a number (treat as px unit) or a string.
	 *
	 * @returns The CSS value of the given value.
	 *
	 * @example
	 * ```javascript
	 * console.log(styles.toValue(8)); // "8px"
	 * console.log(styles.toValue("2rem")); // "2rem"
	 * ```
	 */
	toValue: toValue_css,
};

export const getLottieStatusName = (status: AvailableLottieStatus) => `${STATUS_PREFIX}${status}`;

export const useLottieStatus = {
	name: (status: AvailableLottieStatus) => css`animation-name: ${getLottieStatusName(status)};`,
	animation: (status: AvailableLottieStatus) => css`animation: ${getLottieStatusName(status)} 1s infinite;`,
};

/**
 * Zooms the given DOMRect by a specified zoom factor.
 *
 * @deprecated
 * // DELETE: I don't know if this function is still using.
 *
 * @param rect - The DOMRect to be zoomed.
 * @param zoom - The zoom factor to apply to the DOMRect.
 *
 * @returns A new DOMRect with the same position and dimensions as the original, but scaled by the specified zoom factor.
 *
 * @remarks
 * This function returns a new DOMRect by multiplying each numeric property by the given zoom factor.
 * It is useful for scaling UI elements or adjusting coordinates based on the UI scale.
 *
 * @example
 * ```typescript
 * const originalRect = new DOMRect(10, 20, 30, 40);
 * const zoomFactor = 2;
 * const newRect = zoomDomRect(originalRect, zoomFactor);
 * console.log(newRect); // Output: DOMRect {x: 20, y: 40, width: 60, height: 80,...}
 * ```
 */
export function zoomDomRect(rect: DOMRect, zoom: number) {
	const { x, y, width, height } = rect;
	return new DOMRect(x * zoom, y * zoom, width * zoom, height * zoom);
}

/**
 * Useful when you want to convert
 * ```typescript
 * element.style.backgroundColor = "black";
 * ```
 * to
 * ```typescript
 * element.style.setProperty("background-color", "black");
 * ```
 *
 * @example
 * · backgroundColor ⇒ background-color
 * · webkitTextStroke ⇒ -webkit-text-stroke
 * · --custom-property ⇒ --custom-property
 *
 * @param camel - Camel case CSS style property name, except CSS custom property.
 * @returns Kebab case CSS style property name.
 */
export function convertCamelStylePropertyToKebab(camel: string) {
	return camel.startsWith("--") ? camel : new VariableName(camel).cssProp;
}

/**
 * Define styles for the filled-in portion of the bar of a `<progress>` element.
 * The bar represents the amount of progress that has been made.
 *
 * It represents `::-webkit-progress-value` pseudo-element for webkit and
 * `::-moz-progress-bar` pseudo-element for mozilla.
 *
 * @param style - Styles for the filled-in portion of the bar of a `<progress>` element.
 * @returns CSS declaration string.
 *
 * @example
 * ```typescript
 * progressFinishedPart`
 *     background-color: currentColor;
 * `
 * ```
 * Equivalent to (in theory, but not work)
 * ```css
 * &::-webkit-progress-value,
 * &::-moz-progress-bar {
 *     background-color: currentColor;
 * }
 * ```
 *
 * @remarks
 * Why use this function? In fact, this theoretically equivalent CSS code cannot work properly on Chromium,
 * but it can work properly on Firefox. This is because Chromium is **very arrogant** and unwilling to write
 * `webkit` and `moz` together separated by commas. If you write them together, Chromium will not recognize
 * them. So you have to write theme separately.
 *
 * Equivalent code that can actually work
 * ```css
 * &::-webkit-progress-value {
 *     background-color: currentColor;
 * }
 *
 * &::-moz-progress-bar {
 *     background-color: currentColor;
 * }
 * ```
 */
export const progressFinishedPart = (style: RuleSet) => [
	"&::-webkit-progress-value",
	"&::-moz-progress-bar",
].map(finishedPart => css`
	${finishedPart} {
		${style}
	}
`);

/**
 * Generates CSS rules for elements with the `[hidden]` attribute and for the `@starting-style` pseudo-class,
 * applying the provided style to both selectors.
 *
 * @param style - The CSS rules to apply to the selectors.
 * @returns An array of CSS-in-JS style blocks, each targeting either `[hidden]` or `@starting-style`.
 */
export const hiddenAndStartingStyle = (style: RuleSet) => [
	"&[hidden]",
	"@starting-style",
].map(selector => css`
	${selector} {
		${style}
	}
`);

/* eslint-disable jsdoc/check-param-names */
/**
 * Returns the CSS transform string needed to convert an element's flow direction
 * from one orientation to another.
 *
 * @param from - The source flow direction. Defaults to `"lr-tb"` (left-to-right, top-to-bottom).
 * @param to - The target flow direction. Must be one of the supported `FlowDirection` values.
 * @returns The CSS transform string (e.g., `"scaleX(-1)"`, `"rotate(90deg)"`, etc.) required to visually convert
 * from the `from` direction to the `to` direction. Returns an empty string if no transformation is needed.
 *
 * @example
 * ```typescript
 * // Convert from left-to-right, top-to-bottom to right-to-left, top-to-bottom
 * const transform = transformFlowDirection("rl-tb"); // "scaleX(-1)"
 * const transform = transformFlowDirection("lr-tb", "rl-tb"); // "scaleX(-1)"
 * ```
 */
export function transformFlowDirection(to: FlowDirection): string;
/* eslint-enable jsdoc/check-param-names */
/**
 * Returns the CSS transform string needed to convert an element's flow direction
 * from one orientation to another.
 *
 * @param from - The source flow direction. Defaults to `"lr-tb"` (left-to-right, top-to-bottom).
 * @param to - The target flow direction. Must be one of the supported `FlowDirection` values.
 * @returns The CSS transform string (e.g., `"scaleX(-1)"`, `"rotate(90deg)"`, etc.) required to visually convert
 * from the `from` direction to the `to` direction. Returns an empty string if no transformation is needed.
 *
 * @example
 * ```typescript
 * // Convert from left-to-right, top-to-bottom to right-to-left, top-to-bottom
 * const transform = transformFlowDirection("rl-tb"); // "scaleX(-1)"
 * const transform = transformFlowDirection("lr-tb", "rl-tb"); // "scaleX(-1)"
 * ```
 */
export function transformFlowDirection(from: FlowDirection, to: FlowDirection): string;
export function transformFlowDirection(from: FlowDirection, to?: FlowDirection) {
	if (!to) {
		to = from;
		from = "lr-tb";
	}
	// #region Enumerated result, correct but lengthy.
	/* return {
		"lr-tb": {
			"lr-tb": "",
			"lr-bt": "scaleY(-1)",
			"rl-tb": "scaleX(-1)",
			"rl-bt": "rotate(180deg)",
			"tb-lr": "rotate(90deg) scaleY(-1)",
			"bt-lr": "rotate(-90deg)",
			"tb-rl": "rotate(90deg)",
			"bt-rl": "rotate(90deg) scaleX(-1)",
		},
		"lr-bt": {
			"lr-tb": "scaleY(-1)",
			"lr-bt": "",
			"rl-tb": "rotate(180deg)",
			"rl-bt": "scaleX(-1)",
			"tb-lr": "rotate(90deg)",
			"bt-lr": "rotate(90deg) scaleX(-1)",
			"tb-rl": "rotate(90deg) scaleY(-1)",
			"bt-rl": "rotate(-90deg)",
		},
		"rl-tb": {
			"lr-tb": "scaleX(-1)",
			"lr-bt": "rotate(180deg)",
			"rl-tb": "",
			"rl-bt": "scaleY(-1)",
			"tb-lr": "rotate(-90deg)",
			"bt-lr": "rotate(90deg) scaleY(-1)",
			"tb-rl": "rotate(90deg) scaleX(-1)",
			"bt-rl": "rotate(90deg)",
		},
		"rl-bt": {
			"lr-tb": "rotate(180deg)",
			"lr-bt": "scaleX(-1)",
			"rl-tb": "scaleY(-1)",
			"rl-bt": "",
			"tb-lr": "rotate(90deg) scaleX(-1)",
			"bt-lr": "rotate(90deg)",
			"tb-rl": "rotate(-90deg)",
			"bt-rl": "rotate(90deg) scaleY(-1)",
		},
		"tb-lr": {
			"lr-tb": "rotate(90deg) scaleY(-1)",
			"lr-bt": "rotate(-90deg)",
			"rl-tb": "rotate(90deg)",
			"rl-bt": "rotate(90deg) scaleX(-1)",
			"tb-lr": "",
			"bt-lr": "scaleY(-1)",
			"tb-rl": "scaleX(-1)",
			"bt-rl": "rotate(180deg)",
		},
		"bt-lr": {
			"lr-tb": "rotate(90deg)",
			"lr-bt": "rotate(90deg) scaleX(-1)",
			"rl-tb": "rotate(90deg) scaleY(-1)",
			"rl-bt": "rotate(-90deg)",
			"tb-lr": "scaleY(-1)",
			"bt-lr": "",
			"tb-rl": "rotate(180deg)",
			"bt-rl": "scaleX(-1)",
		},
		"tb-rl": {
			"lr-tb": "rotate(-90deg)",
			"lr-bt": "rotate(90deg) scaleY(-1)",
			"rl-tb": "rotate(90deg) scaleX(-1)",
			"rl-bt": "rotate(90deg)",
			"tb-lr": "scaleX(-1)",
			"bt-lr": "rotate(180deg)",
			"tb-rl": "",
			"bt-rl": "scaleY(-1)",
		},
		"bt-rl": {
			"lr-tb": "rotate(90deg) scaleX(-1)",
			"lr-bt": "rotate(90deg)",
			"rl-tb": "rotate(-90deg)",
			"rl-bt": "rotate(90deg) scaleY(-1)",
			"tb-lr": "rotate(180deg)",
			"bt-lr": "scaleX(-1)",
			"tb-rl": "scaleY(-1)",
			"bt-rl": "",
		},
	}[from][to]; */
	// #endregion
	// Map the direction to a 3 bit binary number: [Rotate 90°, V Flip, H Flip].
	const dirBits: Record<FlowDirection, number> = {
		"lr-tb": 0b000, "lr-bt": 0b010, "rl-tb": 0b001, "rl-bt": 0b011,
		"tb-lr": 0b100, "bt-lr": 0b110, "tb-rl": 0b101, "bt-rl": 0b111,
	};
	// Calculate transformation opcode: f⊕t (XOR).
	let op = dirBits[from] ^ dirBits[to];
	// Special adjustment rule, because it is not a simple XOR operation.
	if (op & 0b100) op ^= (dirBits[to] >> 1 ^ dirBits[to]) & 0b1 ? 0b1 : 0b10;
	// Generate transform string based on the opcode.
	return [
		"", // 000: No Transformation
		"scaleX(-1)", // 001: H Flip
		"scaleY(-1)", // 010: V Flip
		"rotate(180deg)", // 011: Rotate 180°
		"rotate(90deg)", // 100: Rotate 90°
		"rotate(90deg) scaleX(-1)", // 101: Rotate 90° + H Flip
		"rotate(90deg) scaleY(-1)", // 110: Rotate 90° + V Flip
		"rotate(-90deg)", // 111: Rotate -90°
	][op];
}

/**
 * React hook that returns a numeric measurement for a DOM element using ResizeObserver.
 *
 * Observes the provided element and updates a numeric value whenever ResizeObserver reports
 * a change. The returned value is initialized to 0 and will update on the first observation.
 *
 * @param el - A RefObject pointing to the DOM element to observe (may be null or undefined).
 * @param type - Which measurement to return. Accepted values:
 * - `"width" | "height"`: Reads from `ResizeObserverEntry.contentRect.width/height`;
 * - `"top" | "left" | "right" | "bottom" | "x" | "y"`: Reads the corresponding property from `contentRect`;
 * - `"borderBoxInlineSize"`: Reads `borderBoxSize[0].inlineSize` from the entry;
 * - `"borderBoxBlockSize"`: Reads `borderBoxSize[0].blockSize` from the entry;
 * - `"contentBoxInlineSize"`: Reads `contentBoxSize[0].inlineSize` from the entry;
 * - `"contentBoxBlockSize"`: Reads `contentBoxSize[0].blockSize` from the entry.
 *
 * @returns The current numeric measurement (pixels or logical units as reported by the browser).
 *
 * @remarks
 * - Internally registers a ResizeObserver on `el.current` and updates the returned state
 * whenever the observer callback fires.
 * - The observer is disconnected in the effect cleanup to avoid leaks.
 * - The hook initializes the returned size to 0 until the observer reports a value.
 * - Note: the effect that attaches the observer is tied to the `type` parameter. If the
 * ref object or its current element changes without changing `type`, the observer may
 * not be re-attached to the new element. Ensure `type` is updated or remount the hook
 * if you need to observe a different element reference.
 *
 * @example
 * ```typescript
 * const ref = useRef<HTMLElement | null>(null);
 * const width = useElementSize(ref, "width");
 * ```
 */
export function useElementSize(el: RefObject<Element | undefined | null>, type: "borderBoxInlineSize" | "borderBoxBlockSize" | "contentBoxInlineSize" | "contentBoxBlockSize" | Exclude<keyof DOMRect, "toJSON">) {
	const [size, setSize] = useState(0);

	useEffect(() => {
		if (!el.current) return;
		const observer = new ResizeObserver(([e]) => {
			setSize(
				type === "borderBoxInlineSize" ? e.borderBoxSize[0].inlineSize :
				type === "borderBoxBlockSize" ? e.borderBoxSize[0].blockSize :
				type === "contentBoxInlineSize" ? e.contentBoxSize[0].inlineSize :
				type === "contentBoxBlockSize" ? e.contentBoxSize[0].blockSize :
				e.contentRect[type],
			);
		});
		observer.observe(el.current);
		return () => observer.disconnect();
	}, [type]);

	return size;
}
