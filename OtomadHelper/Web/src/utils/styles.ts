/*
 * 转发 styles 目录中的模块。
 */

import eases from "../styles/eases";
import effects from "../styles/effects";
import mixins from "../styles/mixins";

export { eases };

export const styles = {
	mixins,
	effects,
};

/**
 * 调用主题色。
 * @param cssVarName - 颜色的 CSS 属性名称，不必加前面的“--”。也可以是 white 或 black。
 * @param alpha - Alpha 值，注意是百分比值而不是零到一之间的小数，如果留空表示不透明色。
 * @returns 返回由 var 调用的自定义属性纯色，或 rgba 封装的透明色。
 */
export function c(cssVarName: string, alpha?: number) {
	if (alpha !== undefined && (alpha < 0 || alpha > 100))
		throw RangeError("The alpha parameter should be in range [0, 100]");
	if (cssVarName === "white" || cssVarName === "black")
		return alpha === undefined ? cssVarName :
			"#" + (cssVarName === "white" ? "f" : "0").repeat(6) + Math.round(alpha / 100 * 255).toString(16).padStart(2, "0");
	return alpha === undefined ? `var(--${cssVarName})` :
		`rgba(var(--${cssVarName}-rgb), ${alpha}%)`;
}
