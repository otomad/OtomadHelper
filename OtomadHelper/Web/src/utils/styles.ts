/*
 * 转发 styles 目录中的模块。
 */

import type { ColorNames } from "styles/colors";
import eases from "styles/eases";
import effects from "styles/effects";
import { STATUS_PREFIX, type AvailableLottieStatus } from "styles/fake-animations";
import mixins from "styles/mixins";

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
		`rgb(from var(--${cssVarName}) r g b / ${alpha}%)`;
}

export const ifColorScheme = {
	light: '[data-scheme="light"]',
	dark: '[data-scheme="dark"]',
};

/**
 * 根据字符串或数字获取对应的 CSS 样式值。
 * @param value - 如果传入的值为数字，则返回其对应像素值；如果传入的值为字符串，则保留原字符串值。
 * @returns 对应的 CSS 样式值。
 */
function toValue(value: string | number) {
	return typeof value === "number" ? value + "px" : value;
}

export { eases };

export const styles = {
	mixins,
	effects,
	toValue,
};

export const useLottieStatus = {
	name: (status: AvailableLottieStatus) => css`animation-name: ${STATUS_PREFIX}${status};`,
	animation: (status: AvailableLottieStatus) => css`animation: ${STATUS_PREFIX}${status} 1s infinite;`,
};

enum TransitionGroupState {
	appear = 1 << 0,
	enter = 1 << 1,
	exit = 1 << 2,

	enterExit = 1 << 1 | 1 << 2,
	all = 1 << 0 | 1 << 1 | 1 << 2,
}

/**
 * 为 React Transition Group 离谱的理念而生成的状态选择器规则。
 * @param states - 过渡组的状态，包含出现、进入、退出各自的按位与值。
 * @param name - 可选的过渡组动画名称，留空表示不包含。
 * @returns 生成的状态选择器。
 */
export function tgs(states: TransitionGroupState = TransitionGroupState.all, name: string = "") {
	if (name) name += "-";
	const selectors: string[] = [];
	if (states & TransitionGroupState.appear)
		selectors.push(`&.${name}appear:not(.${name}appear-active)`);
	if (states & TransitionGroupState.enter)
		selectors.push(`&.${name}enter:not(.${name}enter-active)`);
	if (states & TransitionGroupState.exit)
		selectors.push(`&:is(.${name}exit-active, .${name}exit-done)`);
	return selectors.join(", ");
}
tgs.appear = TransitionGroupState.appear;
tgs.enter = TransitionGroupState.enter;
tgs.exit = TransitionGroupState.exit;
tgs.enterExit = TransitionGroupState.enterExit;
tgs.all = TransitionGroupState.all;
