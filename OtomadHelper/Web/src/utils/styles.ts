/*
 * 转发 styles 目录中的模块。
 */

import type { ColorNames } from "styles/colors";
import eases from "styles/eases";
import effects from "styles/effects";
import { STATUS_PREFIX, type AvailableLottieStatus } from "styles/fake-animations";
import mixins from "styles/mixins";

export const fallbackTransitions = `all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} 100ms, fill ${eases.easeOutMax} 100ms`;

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
function toValue_css(value: string | number) {
	return typeof value === "number" ? value + "px" : value;
}

export { eases };

export const styles = {
	mixins,
	effects,
	toValue: toValue_css,
};

export const useLottieStatus = {
	name: (status: AvailableLottieStatus) => css`animation-name: ${STATUS_PREFIX}${status};`,
	animation: (status: AvailableLottieStatus) => css`animation: ${STATUS_PREFIX}${status} 1s infinite;`,
};

enum TransitionGroupState {
	appear = 0b001,
	enter = 0b010,
	exit = 0b100,

	enterExit = 0b110,
	all = 0b111,
}

/**
 * 为 React Transition Group 离谱的选择器而生成的状态选择器规则。
 *
 * 不过尝试经魔改源码，额外增加一个 enter-from 状态后，发现并不好使，不知道具体原因。
 *
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

/**
 * 根据给定的工具提示方向和偏移值，获取工具提示的定位值。
 * @param rect - 目标元素尺寸矩形。
 * @param placement - 浮窗出现方向。
 * @param offset - 与目标元素距离偏移。
 * @param flyoutRect - 浮窗元素尺寸矩形。
 * @param adjustBySize - 如否则只返回该点位置（默认）；是则根据元素尺寸调整到其左上角的坐标。
 * @returns 表示工具提示位置的样式属性值。
 */
export function getPosition(rect: MaybeRef<DOMRect | Element>, placement?: Placement, offset: number = 10, flyoutRect?: MaybeRef<DOMRect | Element | TwoD | undefined>) {
	rect = toValue(rect);
	flyoutRect = toValue(flyoutRect);
	if (rect instanceof Element) rect = rect.getBoundingClientRect();
	if (isRtl())
		if (placement === "left") placement = "right";
		else if (placement === "right") placement = "left";
	if (!placement || placement === "x" || placement === "y") { // 如果缺省工具提示放置位置，则会寻找离页边最远的方向。
		const toPageDistance = [rect.top, window.innerHeight - rect.bottom, window.innerWidth - rect.right, rect.left];
		if (placement === "x") toPageDistance[0] = toPageDistance[1] = -Infinity;
		else if (placement === "y") toPageDistance[2] = toPageDistance[3] = -Infinity;
		const placements = ["top", "bottom", "right", "left"] as const; // 优先顺序：上、下、右、左。
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
		else if (flyoutRect instanceof Array) flyoutRect = { width: flyoutRect[0], height: flyoutRect[1] } as DOMRect;
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
 * 探测元素溢出。如果元素超出了页面范围，则将其移动到页面内。
 * @private
 * @param location - 元素的坐标（仅支持元组类型）。
 * @param size - 元素的尺寸（仅支持元组类型）。
 * @returns 返回移动入页面后的新坐标。
 */
function moveIntoPage_tuple(location: TwoD, size: TwoD) {
	const result = [...location] as typeof location;
	const windowSize = [window.innerWidth, window.innerHeight];
	for (let i = 0; i < 2; i++) {
		if (result[i] + size[i] > windowSize[i])
			result[i] = windowSize[i] - size[i];
		if (result[i] < 0)
			result[i] = 0;
	}
	return result;
}

/**
 * 探测元素溢出。如果元素超出了页面范围，则将其移动到页面内。
 * @param location - 元素的元组类型坐标。
 * @param size - 元素的元组类型尺寸。
 * @returns 返回移动入页面后的新坐标。
 */
export function moveIntoPage(location: MaybeRef<TwoD>, size?: MaybeRef<TwoD | DOMRect | undefined>): TwoD;
/**
 * 探测元素溢出。如果元素超出了页面范围，则将其移动到页面内。
 * @param element - HTML DOM 元素。
 * @returns 返回移动入页面后的新坐标样式声明。
 */
export function moveIntoPage(element: MaybeRef<HTMLElement>): { top: string; left: string };
/**
 * 探测元素溢出。如果元素超出了页面范围，则将其移动到页面内。
 * @param measureElement - 要测量的 HTML DOM 元素。
 * @param adjustElement - 要调整位置的 HTML DOM 元素。
 * @returns 返回移动入页面后的新坐标样式声明。
 */
export function moveIntoPage(measureElement: MaybeRef<HTMLElement>, adjustElement?: MaybeRef<HTMLElement | undefined>): { top: string; left: string };
/**
 * 探测元素溢出。如果元素超出了页面范围，则将其移动到页面内。
 * @param location - 元素的坐标。
 * @param size - 元素的尺寸。
 * @returns 返回移动入页面后的新坐标。
 */
export function moveIntoPage(location: MaybeRef<TwoD | HTMLElement>, size?: MaybeRef<TwoD | DOMRect | HTMLElement | undefined>) {
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
	const result = moveIntoPage_tuple(location, size as TwoD);
	if (adjustElementStyle) {
		const adjustment = (["left", "top"] as const).map(pos => parseFloat(adjustElementStyle[pos])) as TwoD;
		location.forEach((original, i) => result[i] += adjustment[i] - original);
	}
	if (!returnAsStyle) return result;
	return getLocationStyle(result);
}

/**
 * 将二维坐标转换为位置的样式值。
 * @param location - 二维坐标。
 * @returns 位置的样式值。
 */
export function getLocationStyle(location: MaybeRef<TwoD>): CSSProperties {
	location = toValue(location);
	return location[0] !== 0 || location[1] !== 0 ? { left: location[0] + "px", top: location[1] + "px" } : {};
}
