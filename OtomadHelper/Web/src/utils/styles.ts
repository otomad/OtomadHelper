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
 * Gets the positioning value of the tooltip based on the given tooltip placement and offset value.
 * @param rect - Target element size rectangle.
 * @param placement - The placement in which the flyout appears.
 * @param offset - Offset from the target element.
 * @param flyoutRect - Flyout element size rectangle.
 * @param adjustBySize - If false, only the position of the point will be returned (by default);
 * if true, it will be adjusted to the coordinates of its upper left corner according to the size of the element.
 * @returns The style attribute value that represents the tooltip position.
 */
export function getPosition(rect: MaybeRef<DOMRect | Element>, placement?: Placement, offset: number = 10, flyoutRect?: MaybeRef<DOMRect | Element | TwoD | undefined>) {
	rect = toValue(rect);
	flyoutRect = toValue(flyoutRect);
	if (rect instanceof Element) rect = rect.getBoundingClientRect();
	if (isRtl())
		if (placement === "left") placement = "right";
		else if (placement === "right") placement = "left";
	if (!placement || placement === "x" || placement === "y") { // The default tooltip placement looks for the direction furthest from the edge of the page.
		const toPageDistance = [rect.top, window.innerHeight - rect.bottom, window.innerWidth - rect.right, rect.left];
		if (placement === "x") toPageDistance[0] = toPageDistance[1] = -Infinity;
		else if (placement === "y") toPageDistance[2] = toPageDistance[3] = -Infinity;
		const placements = ["top", "bottom", "right", "left"] as const; // Priority order: top, bottom, right, left.
		placement = placements[toPageDistance.indexOf(Math.max(...toPageDistance))];
	}
	let position: TwoD;
	if (placement === "top")
		position = [rect.left + rect.width / 2, rect.top - offset];
	else if (placement === "bottom")
		position = [rect.left + rect.width / 2, rect.bottom + offset];
	else if (placement === "left")
		position = [rect.left - offset, rect.top + rect.height / 2];
	else
		position = [rect.right + offset, rect.top + rect.height / 2];
	if (flyoutRect) {
		if (flyoutRect instanceof Element) flyoutRect = flyoutRect.getBoundingClientRect();
		else if (Array.isArray(flyoutRect)) flyoutRect = { width: flyoutRect[0], height: flyoutRect[1] } as DOMRect;
		if (placement === "top")
			position = [position[0] - flyoutRect.width / 2, position[1] - flyoutRect.height];
		else if (placement === "bottom")
			position = [position[0] - flyoutRect.width / 2, position[1]];
		else if (placement === "left")
			position = [position[0] - flyoutRect.width, position[1] - flyoutRect.height / 2];
		else
			position = [position[0], position[1] - flyoutRect.height / 2];
	}
	return {
		position,
		style: { left: position[0] + "px", top: position[1] + "px" } as CSSProperties,
		placement,
		offset,
	};
}

/**
 * Detect element overflow. If the element exceeds the scope of the page, move it within the page.
 * @private
 * @param location - The coordinates of the element (only supported on tuple types).
 * @param size - The dimensions of the element (only supported for tuple types).
 * @param outline - The outline border width to reduce.
 * @returns The new coordinates after moving into the page.
 */
function moveIntoPage_tuple(location: TwoD, size: TwoD, outline: number = 0) {
	const result = [...location] as typeof location;
	const windowSize = [window.innerWidth, window.innerHeight];
	for (let i = 0; i < 2; i++) {
		if (result[i] + size[i] > windowSize[i] - outline)
			result[i] = windowSize[i] - outline - size[i];
		if (result[i] < outline)
			result[i] = outline;
	}
	return result;
}

/**
 * Detect element overflow. If the element exceeds the scope of the page, move it within the page.
 * @param location - Tuple type coordinates of the element.
 * @param size - Tuple type dimensions of the element.
 * @param outline - The outline border width to reduce.
 * @returns The new coordinates after moving into the page.
 */
export function moveIntoPage(location: MaybeRef<TwoD>, size?: MaybeRef<TwoD | DOMRect | undefined>, outline?: number): TwoD;
/**
 * Detect element overflow. If the element exceeds the scope of the page, move it within the page.
 * @param element - HTML DOM element.
 * @returns The new coordinate style declaration after moving into the page.
 */
export function moveIntoPage(element: MaybeRef<HTMLElement>): { top: string; left: string };
/**
 * Detect element overflow. If the element exceeds the scope of the page, move it within the page.
 * @param measureElement - The HTML DOM element to measure.
 * @param adjustElement - The HTML DOM element to reposition.
 * @param outline - The outline border width to reduce.
 * @returns The new coordinate style declaration after moving into the page.
 */
export function moveIntoPage(measureElement: MaybeRef<HTMLElement>, adjustElement?: MaybeRef<HTMLElement | undefined>, outline?: number): { top: string; left: string };
/**
 * Detect element overflow. If the element exceeds the scope of the page, move it within the page.
 * @param location - The coordinates of the element.
 * @param size - The dimensions of the element.
 * @param outline - The outline border width to reduce.
 * @returns The new coordinates after moving into the page.
 */
export function moveIntoPage(location: MaybeRef<TwoD | HTMLElement>, size?: MaybeRef<TwoD | DOMRect | HTMLElement | undefined>, outline: number = 0) {
	location = toValue(location);
	size = toValue(size);
	const adjustElementStyle = size instanceof Element && size.style;
	let returnAsStyle = false;
	if (location instanceof Element) {
		returnAsStyle = true;
		const rect = location.getBoundingClientRect();
		location = [rect.x, rect.y];
		size = [rect.width, rect.height];
	}
	if (size instanceof DOMRect)
		size = [size.width, size.height];
	const result = moveIntoPage_tuple(location, size as TwoD, outline);
	if (adjustElementStyle) {
		const adjustment = (["left", "top"] as const).map(pos => parseFloat(adjustElementStyle[pos])) as TwoD;
		location.forEach((original, i) => result[i] += adjustment[i] - original);
	}
	if (!returnAsStyle) return result;
	return getLocationStyle(result);
}

/**
 * Converts 2D coordinates to the location style value.
 * @param location - 2D coordinates.
 * @returns Location style value.
 */
export function getLocationStyle(location: MaybeRef<TwoD>): CSSProperties {
	location = toValue(location);
	return location[0] !== 0 || location[1] !== 0 ? { left: location[0] + "px", top: location[1] + "px" } : {};
}

/**
 * Retrieves bounding client rectangle of the given HTML DOM element.
 *
 * @deprecated
 *
 * @param element - The HTML DOM element (or even EventTarget or null) and get the bounding client rectangle from.
 * @returns A tuple containing the x, y, width, and height of the bounding client rectangle.
 *
 * @remarks
 * This function is useful when you need to get the position and dimensions of an HTML DOM element.
 * It first converts the given EventTarget or null to an HTMLElement using the `toValue` function.
 * Then, it retrieves the bounding client rectangle of the HTMLElement using the `getBoundingClientRect` method.
 * Finally, it returns a tuple containing the x, y, width, and height of the bounding client rectangle.
 *
 * @note In RTL mode, the `x` still indicates the left position instead of the right position.
 *
 * @example
 * ```typescript
 * const element = document.getElementById("myElement");
 * const [x, y, width, height] = getBoundingClientRectTuple(element);
 * console.log(`Element position: x=${x}, y=${y}, width=${width}, height=${height}`);
 * ```
 */
export function getBoundingClientRect(element: MaybeRef<EventTarget | null>): DOMRect {
	const el = toValue(element) as HTMLElement;
	let rect = el.getBoundingClientRect();
	const zoom = getUiScale1();
	rect = zoomDomRect(rect, zoom);
	return rect;
}

/**
 * Zooms the given DOMRect by a specified zoom factor.
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
 * Use pure CSS to calculate the high contrast text color (black or white) by the oklab model from
 * the specified background color.
 * @param colorVar - Background color CSS custom property name, the initial two dashes can be omitted.
 * @param alpha - The alpha value of the color, defaults to 1.
 * @returns A contrastive text color.
 * @remarks When `color-contrast()` available, this function will be deprecated.
 */
export function getContrastiveColor(colorVar: string, alpha: number = 1) {
	if (!colorVar.startsWith("--")) colorVar = "--" + colorVar;
	return `oklch(from var(${colorVar}) calc(1 - round(to-zero, L / 0.65)) 0 0 / ${alpha})`;
	// Cannot use `infinity`.
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
export const progressFinishedPart: typeof css<object> = (...style) => {
	return [
		"&::-webkit-progress-value",
		"&::-moz-progress-bar",
	].map(finishedPart => css`
		${finishedPart} {
			${css(...style)}
		}
	`);
};

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
