/*
 * 转发 styles 目录中的模块。
 */

import type { ColorNames } from "styles/colors";
import eases from "styles/eases";
import effects from "styles/effects";
import mixins from "styles/mixins";

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
export function c(cssVarName: string & {} | "white" | "black" | ColorNames, alpha?: number) {
	if (alpha !== undefined && (alpha < 0 || alpha > 100))
		throw RangeError("The alpha parameter should be in range [0, 100]");
	if (cssVarName === "white" || cssVarName === "black" || cssVarName.startsWith("#")) {
		let rgb = (cssVarName === "white" ? "f" : "0").repeat(6);
		if (cssVarName.startsWith("#")) {
			cssVarName = cssVarName.slice(1);
			if (cssVarName.length === 6) rgb = cssVarName;
			else if (cssVarName.length === 3) rgb = Array.from("001122", i => cssVarName[+i]).join("");
			else throw new RangeError("The color hex string length must be 3 or 6");
		}
		return alpha === undefined ? cssVarName :
			"#" + rgb + Math.round(alpha / 100 * 255).toString(16).padStart(2, "0");
	}
	return alpha === undefined ? `var(--${cssVarName})` :
		`rgba(var(--${cssVarName}-rgb), ${alpha}%)`;
}

export const ifColorScheme = {
	light: '[data-scheme="light"]',
	dark: '[data-scheme="dark"]',
};
