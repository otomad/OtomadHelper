export { };

namespace AnimatedIconStateNS {
	export type Tuple = [marker?: string, loop?: boolean, speed?: number];
	export type Object = { marker?: string; loop?: boolean; speed?: number };
}

declare global {
	/** 复选框选中状态。 */
	type CheckState = "unchecked" | "indeterminate" | "checked";

	type AnimatedIconState = AnimatedIconStateNS.Tuple | AnimatedIconStateNS.Object;

	type Status = "neutual" | "accent" | "info" | "asterisk" | "warning" | "success" | "error";

	/** 工具提示、浮窗等的放置位置方向。 */
	type Placement = "top" | "right" | "bottom" | "left" | "x" | "y";
}
