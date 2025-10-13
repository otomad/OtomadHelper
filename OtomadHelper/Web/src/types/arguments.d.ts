export { };

namespace AnimatedIconStateNS {
	export type Tuple = [marker?: string, loop?: boolean, speed?: number];
	export type Object = { marker?: string; loop?: boolean; speed?: number };
}

declare global {
	/** Check box selection status. */
	type CheckState = "unchecked" | "indeterminate" | "checked";

	/** Positions from all sides. */
	type Position =
		/* eslint-disable @stylistic/no-multi-spaces */
		/* eslint-disable @stylistic/operator-linebreak */
		|    "top left"   |    "top center" |    "top right"
		| "center left"   | "center center" | "center right"
		| "bottom left"   | "bottom center" | "bottom right"

		|   "left top"    | "center top"    |  "right top"
		|   "left center" | "center center" |  "right center"
		|   "left bottom" | "center bottom" |  "right bottom"

		|                        "top"
		|          "left" |     "center"    | "right"
		|                       "bottom"

		|  "start start"  |  "start center" |  "start end"
		| "center start"  | "center center" | "center end"
		|    "end start"  |    "end center" |    "end end";
		/* eslint-enable  @stylistic/no-multi-spaces */
		/* eslint-enable  @stylistic/operator-linebreak */

	type AnimatedIconState = AnimatedIconStateNS.Tuple | AnimatedIconStateNS.Object;

	/** The status of info bar, badge, etc. */
	type Status = "neutual" | "accent" | "info" | "asterisk" | "warning" | "success" | "error";

	/** The placement of tooltips, flyouts, etc. */
	type Placement = "top" | "right" | "bottom" | "left" | "x" | "y";

	/** Item view mode. */
	type ItemView = "list" | "tile" | "grid" | "grid-list";

	/**
	 * Curve Type, Video Keyframe Type, or OFX Interpolation Type.
	 */
	type CurveType = "linear" | "fast" | "slow" | "smooth" | "sharp" | "hold";

	/**
	 * A string that represents the priority (e.g. `"important"`) if one exists.
	 * If none exists, returns the empty string.
	 */
	type StylePriority = "important" | "";

	type StatePropertiedObject<TState> = {
		[property in keyof TState]: StatePropertyNonNull<TState[property]>;
	};

	/** Three stage switch type. */
	type TrueFalseAuto = "true" | "false" | "auto";

	/** The type of oscillator to use. Must be one of the following: "sinusoid", "square", "sawtooth", "triangle". */
	type OscillatorCommonType = "sinusoid" | "triangle" | "square" | "sawtooth";

	/** The values that badge can be accepted. */
	type BadgeValue = string | number | boolean | undefined;

	/** Badge value and status. */
	type BadgeArgs = [badge?: BadgeValue, status?: Status, hidden?: boolean];

	/**
	 * A type which includes the numeric value and its unit enum type.
	 * @template TUnit - The unit name (string). You can narrow it.
	 */
	type Unit<TUnit extends string> = [numeric: number, unit: TUnit];

	/**
	 * A type which includes a range of two numeric values and their unit enum type.
	 * @template TUnit - The unit name (string). You can narrow it.
	 */
	type RangeUnit<TUnit extends string> = [start: number, end: number, unit: TUnit];

	/**
	 * The `aria-checked` attribute indicates the current "checked" state of checkboxes, radio buttons, and other widgets.
	 *
	 * The `aria-checked` attribute indicates whether the element is checked (`true`), unchecked (`false`), or if the checked
	 * status is indeterminate (`mixed`), meaning it is neither checked nor unchecked. The mixed value is supported by the
	 * tri-state input roles of `checkbox` and `menuitemcheckbox`.
	 *
	 * The mixed value is not supported on radio, menuitemradio, or switch and elements that inherits from these. The value
	 * will be false if mixed is set when not supported.
	 *
	 * [MDN Reference](https://developer.mozilla.org/docs/Web/Accessibility/ARIA/Attributes/aria-checked)
	 */
	type AriaChecked = React.AriaAttributes["aria-checked"];

	/**
	 * The `role` read-only property of the `ElementInternals` interface returns the
	 * [WAI-ARIA role](https://developer.mozilla.org/docs/Web/Accessibility/ARIA/Roles) for the element. For example,
	 * a checkbox might have `role="checkbox"`. It reflects the `role` attribute; it does not return the element's implicit ARIA
	 * role, if any, unless explicitly set.
	 *
	 * A string which contains an ARIA role. A full list of ARIA roles can be found on the
	 * [ARIA techniques page](https://developer.mozilla.org/docs/Web/Accessibility/ARIA/ARIA_Techniques).
	 *
	 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/ElementInternals/role)
	 */
	type AriaRole = React.AriaRole;

	/**
	 * The props any HTML DOM element accepting refs can receive.
	 * @example
	 * ```typescript
	 * if (!React.isValidElement<RefAttributes>(child)) return child;
	 * React.cloneElement(child, { ref })); // Now child has `ref` prop.
	 * ```
	 */
	type RefAttributes = { ref?: React.Ref<Element | null> };

	/** Audio or Visual. */
	type StreamKind = "audio" | "visual";

	/** The type reference to an element, node, referenced element, event target, event, or nullish. */
	type TargetType = Node | Element | RefObject<Element | null | undefined> | Event | EventTarget | null | undefined;

	/** The type reference to an element, node, referenced element, event target, event, nullish, or a CSS selector query string. */
	type DetectInPathType = Node | Element | RefObject<Element | null | undefined> | Event | EventTarget | string | null | undefined;

	/** Text writing mode with direction. */
	type FlowDirection = `${"lr" | "rl"}-${"tb" | "bt"}` | `${"tb" | "bt"}-${"lr" | "rl"}`;

	/** Is the orientation of the icon changed based on the writing direction? */
	type DirBasedIcon = boolean | FlowDirection | [from: FlowDirection, to: FlowDirection];

	/** Rough time unit which can be switched by combobox. */
	type RoughTimeUnit = "millisecond" | "second" | "minute" | "hour";

	// Add custom events
	interface GlobalEventHandlersEventMap {
		transitionExitCapture: CustomEvent<{
			target: HTMLElement;
		}>;
	}
}
