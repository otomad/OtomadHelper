export { };

namespace AnimatedIconStateNS {
	export type Tuple = [marker?: string, loop?: boolean, speed?: number];
	export type Object = { marker?: string; loop?: boolean; speed?: number };
}

declare global {
	/** Check box selection status. */
	type CheckState = "unchecked" | "indeterminate" | "checked";

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
	type CurveType = "linear" | "fast" | "slow" | "smooth" | "sharp" | "none";

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

	/** The type of oscillator to use. Must be one of the following: "sine", "square", "sawtooth", "triangle". */
	type OscillatorCommonType = Exclude<OscillatorType, "custom">;
}
