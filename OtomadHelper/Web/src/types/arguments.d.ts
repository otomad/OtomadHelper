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
}
