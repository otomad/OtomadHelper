import { flushSync as reactDomFlushSync } from "react-dom";

/**
 * 移除指定 DOM 元素正在进行的所有动画。
 * @param elements - HTML DOM 元素。
 * @returns 是否有移除动画。
 */
export function removeExistAnimations(...elements: Element[]) {
	let hasExistAnimations = false;
	for (const element of elements) {
		if (!element) continue;
		const existAnimations = element.getAnimations();
		if (existAnimations.length !== 0) {
			hasExistAnimations = true;
			existAnimations.forEach(animation => animation.cancel());
		}
	}
	return hasExistAnimations;
}

/**
 * 等待下一时刻 CSS 动画更新刷新。
 * @returns 空承诺。
 */
export function nextAnimationTick() {
	return new Promise<void>(resolve => {
		window.requestAnimationFrame(() => {
			window.requestAnimationFrame(() => {
				resolve();
			});
		});
	});
}

export function flushSync<R>(fn: () => R): R;
/**
 * flushSync 允许您强制 React 同步刷新所提供回调中的任何更新。这样可以确保 DOM 立即更新。
 * @returns 空承诺。
 */
export function flushSync(): Promise<void>;
export function flushSync(fn?: () => unknown) {
	if (fn) return reactDomFlushSync(fn);
	else return new Promise<void>(resolve => flushSync(resolve));
}

/**
 * 为元素设定样式时**暂时**禁用过渡动画。
 * @param element - HTML DOM 元素。
 * @param style - CSS 样式。
 */
export async function setStyleWithoutTransition(element: HTMLElement, style: CSSProperties) {
	Object.assign(element.style, style);
	element.style.transition = "none";
	await nextAnimationTick();
	element.style.removeProperty("transition");
}

/**
 * 没错，就是大名鼎鼎的延迟函数。
 * 叫 sleep 也行。
 * @param ms - 毫秒值。
 * @returns 空返回值。
 */
export function delay(ms: number): Promise<void> {
	return new Promise(resolve => setTimeout(resolve, ms));
}

/**
 * 用于获取元素动画在何时结束，以助于自动获取动画时间。
 * @deprecated
 */
export function endListener(): (node: HTMLElement, done: () => void) => void;
/**
 * 用于获取元素动画在何时结束，以助于自动获取动画时间。
 * @deprecated
 * @param nodeRef - 获取的子元素引用对象。
 */
export function endListener(nodeRef: RefObject<HTMLElement | undefined>): (done: () => void) => void;
export function endListener(nodeRef?: RefObject<HTMLElement | undefined>) { // DELETE: 引入新版 react transition group 之后可删除。
	const getListener = (node: HTMLElement | null | undefined, done: () => void) => {
		node?.addEventListener("transitionend", e => {
			if (e.target !== e.currentTarget) return;
			done();
		}, false);
	};
	return !nodeRef ?
		(node: HTMLElement, done: () => void) => getListener(node, done) :
		(done: () => void) => getListener(nodeRef.current, done);
}

type StyleProperties = string & keyof FilterValueType<CSSStyleDeclaration, string>;
type Keyframe = Partial<Override<Record<StyleProperties, Numberish>, { offset: number }>>;
type Keyframes = Keyframe[];
type DimensionAxis = "height" | "width" | "both";
type MaybePromise<T> = T | Promise<T>;

export type AnimateSizeOptions = Partial<{
	/** 显式指定初始高度（可选）。 */
	startHeight: number;
	/** 显式指定结束高度（可选）。 */
	endHeight: number;
	/** 显式指定初始宽度（可选）。 */
	startWidth: number;
	/** 显式指定结束宽度（可选）。 */
	endWidth: number;
	/** 动画时间。 */
	duration: number;
	/** 动画运动曲线。默认为：平滑缓出。 */
	easing: string;
	/** 显式指定需要动画的是哪个方向。 */
	specified: DimensionAxis;
	/** 指定**不**需要动画调整哪个方向的内/外边距值。 */
	withoutAdjustPadding: DimensionAxis;
	/** 在改变回调函数后自动增加等待下一帧。 */
	// nextTick: boolean;
	/** 获取最终的元素尺寸。 */
	// getSize: TwoD | Ref<TwoD | undefined>;
	/** 获取最终的元素矩形。 */
	// getRect: Ref<DOMRect | undefined>;
	/** 显式指定初始样式（可选）。 */
	startStyle: Keyframe;
	/** 显式指定结束样式（可选）。 */
	endStyle: Keyframe;
	/** 初始从反向滑入界面。 */
	startReverseSlideIn: boolean;
	/** 结束从反向滑入界面。 */
	endReverseSlideIn: boolean;
	/** 元素的**唯一**子元素初始位移。 */
	startChildTranslate: Numberish;
	/** 元素的**唯一**子元素结束位移。 */
	endChildTranslate: Numberish;
	/** 是否抽掉动画的第一帧以解决可能存在的动画故障？仅在有子元素时生效。 */
	removeGlitchFrame: boolean;
	/** 动画播放的同时附加其它动画，并使用与之相同的时长与缓动值。 */
	attachAnimations: [Element, Keyframes][] | false;
	/** 不要 `overflow: clip;`？ */
	noClipping: boolean;
	/** 在动画结束后保持 `overflow: clip;`？仅在 `noClipping` 为 false 时有效。 */
	keepClippingAtEnd: boolean;
	/** 对**获取的**元素宽高值进行像素偏移调整。 */
	clientAdjustment: Partial<{
		startHeight: number;
		endHeight: number;
		startWidth: number;
		endWidth: number;
	}>;
	/** 在动画之前移出该元素及其子元素现有的动画？以解决当用户快速点击两次连续触发两次本动画从而引发的样式异常。 */
	removePreviousAnimations: boolean;
	/** 动画完成后保持样式。 */
	fillForward: boolean;
}>;

/**
 * 当宽/高度值设为 auto 时的动画宽/高度的高级钩子生成器函数。
 * @param element - HTML DOM 元素。
 * @returns 最终返回动画异步承诺的生成器函数。
 */
export async function* animateSizeGenerator(
	element: MaybeRef<Element | undefined>,
	{
		startHeight,
		endHeight,
		startWidth,
		endWidth,
		duration = 250,
		easing = eases.easeOutSmooth,
		specified = "both",
		withoutAdjustPadding,
		// nextTick: awaitNextTick = true,
		// getSize,
		// getRect,
		startStyle = {},
		endStyle = {},
		startReverseSlideIn,
		endReverseSlideIn,
		startChildTranslate,
		endChildTranslate,
		removeGlitchFrame,
		attachAnimations,
		noClipping = false,
		keepClippingAtEnd = false,
		clientAdjustment = {},
		removePreviousAnimations = false,
		fillForward = false,
	}: AnimateSizeOptions = {},
): AsyncGenerator<void, Animation | void, boolean> {
	element = toValue(element);
	if (!element) return;
	// if (isPrefersReducedMotion()) duration = 0;
	startHeight ??= element.clientHeight + (clientAdjustment.startHeight ?? 0);
	startWidth ??= element.clientWidth + (clientAdjustment.startWidth ?? 0);
	const _hasChangeFunc = yield;
	// if (hasChangeFunc && awaitNextTick) await nextTick();
	if (removePreviousAnimations) removeExistAnimations(element, element.children[0]);
	endHeight ??= element.clientHeight + (clientAdjustment.endHeight ?? 0);
	endWidth ??= element.clientWidth + (clientAdjustment.endWidth ?? 0);
	// if (getSize)
	// 	if (Array.isArray(getSize)) [getSize[0], getSize[1]] = [endWidth, endHeight];
	// 	else getSize.value = [endWidth, endHeight];
	// if (getRect)
	// 	getRect.value = element.getBoundingClientRect();
	let isHeightChanged = specified === "height" || specified === "both",
		isWidthChanged = specified === "width" || specified === "both";
	if (startHeight === endHeight) isHeightChanged = false; // 不用动了。
	if (startWidth === endWidth) isWidthChanged = false;
	if (!isHeightChanged && !isWidthChanged) return;
	const keyframes = [{}, {}] as Keyframes;
	if (isHeightChanged) [keyframes[0].height, keyframes[1].height] = [startHeight + "px", endHeight + "px"];
	if (isWidthChanged) [keyframes[0].width, keyframes[1].width] = [startWidth + "px", endWidth + "px"];
	let setYPaddingIndex: number | undefined, setXPaddingIndex: number | undefined;
	if (startHeight === 0) setYPaddingIndex = 0;
	if (endHeight === 0) setYPaddingIndex = 1;
	if (startWidth === 0) setXPaddingIndex = 0;
	if (endWidth === 0) setXPaddingIndex = 1;
	type CSSStyleDeclarationNumberish = Partial<Record<StyleProperties, Numberish>>;
	const setXPadding = withoutAdjustPadding === undefined || withoutAdjustPadding === "height",
		setYPadding = withoutAdjustPadding === undefined || withoutAdjustPadding === "width";
	if (setXPadding && isHeightChanged && setYPaddingIndex !== undefined)
		Object.assign(keyframes[setYPaddingIndex], { paddingTop: 0, paddingBottom: 0, marginTop: 0, marginBottom: 0, borderTopWidth: 0, borderBottomWidth: 0 } satisfies CSSStyleDeclarationNumberish);
	if (setYPadding && isWidthChanged && setXPaddingIndex !== undefined)
		Object.assign(keyframes[setXPaddingIndex], { paddingLeft: 0, paddingRight: 0, marginLeft: 0, marginRight: 0, borderLeftWidth: 0, borderRightWidth: 0 } satisfies CSSStyleDeclarationNumberish);
	const setTranslate = (pxes: number[]) => pxes.map(i => i + "px").join(" ");
	if (startReverseSlideIn)
		keyframes[0].translate = setTranslate([isWidthChanged ? endWidth : 0, isHeightChanged ? endHeight : 0]);
	if (endReverseSlideIn)
		keyframes[1].translate = setTranslate([isWidthChanged ? startWidth : 0, isHeightChanged ? startHeight : 0]);
	Object.assign(keyframes[0], startStyle);
	Object.assign(keyframes[1], endStyle);
	const animationOptions: KeyframeAnimationOptions = { duration, easing, fill: fillForward ? "forwards" : undefined, composite: "replace" };
	const htmlElement = element as HTMLElement;
	if (!noClipping) htmlElement.style.overflow = "clip";
	const result = element.animate(keyframes, animationOptions);
	if (!noClipping && !keepClippingAtEnd) result.addEventListener("finish", () => htmlElement.style.removeProperty("overflow"));
	if (startChildTranslate || endChildTranslate || attachAnimations) {
		const onlyChild = element.children[0]; // 只取唯一一个子元素。
		if (onlyChild && element instanceof HTMLElement && removeGlitchFrame) {
			element.hidden = true;
			await nextAnimationTick();
			element.hidden = false;
		}
		if (onlyChild && (startChildTranslate || endChildTranslate)) onlyChild.animate([
			startChildTranslate ? { translate: startChildTranslate } : {},
			endChildTranslate ? { translate: endChildTranslate } : {},
		], animationOptions);
		if (attachAnimations) attachAnimations.forEach(group => group[0]?.animate(group[1], animationOptions));
	}
	return result.finished.catch(noop);
}

/**
 * 当宽/高度值设为 auto 时的动画宽/高度。
 * @param element - HTML DOM 元素。
 * @param changeFunc - 使宽/高度将会改变的回调函数。
 * @param options - 配置选项。
 * @returns 动画异步承诺。
 */
export async function animateSize(
	element: MaybeRef<Element | undefined>,
	changeFunc: (() => MaybePromise<void | unknown>) | undefined | null,
	options: AnimateSizeOptions = {},
): Promise<Animation | void> {
	const gen = animateSizeGenerator(element, options);
	gen.next();
	if (changeFunc) await changeFunc();
	const animation = await gen.next(!!changeFunc);
	return animation.value;
}

export type SameOrDifferent<T> = T | undefined | [T | undefined, T | undefined];

/**
 * `animateSize` 函数的简化版，适用于更为简单的动画。
 * @param specified - 显式指定需要动画的是哪个方向。默认为高度动画。
 * @param duration - 指定动画时间。
 * @param easing - 指定动画缓动曲线。
 * @param enterOptions - 在进入动画时指定其它参数。
 * @param exitOptions - 在退出动画时指定其它参数。
 * @returns 返回 `onEnter` 和 `onExit` 两个函数。
 */
export function simpleAnimateSize(specified: "width" | "height" = "height", duration?: SameOrDifferent<number>, easing?: SameOrDifferent<string>, enterOptions: AnimateSizeOptions = {}, exitOptions: AnimateSizeOptions = {}) {
	const enter = enterOptions, exit = exitOptions;
	if (specified === "width") {
		enter.startWidth = 0;
		exit.endWidth = 0;
	} else {
		enter.startHeight = 0;
		exit.endHeight = 0;
	}
	duration = Array.isArray(duration) ? duration : [duration, duration];
	easing = Array.isArray(easing) ? easing : [easing, easing];
	enter.duration = duration[0];
	exit.duration = duration[1];
	enter.easing = easing[0];
	exit.easing = easing[1];
	enter.removePreviousAnimations = true;
	exit.fillForward = true;

	const ANIMATE_SIZE_END_EVENT = "animatesizeend"; // 这里我们使用一个自定义的事件，以防原生 CSS 过渡动画结束时干扰运行。
	const currentAnimationThread = useRef<symbol>();

	const onEnter = async (el: HTMLElement) => {
		// const el = nodeRef.current;
		if (!el) return;
		const thisThread = Symbol("enter");
		currentAnimationThread.current = thisThread;
		await animateSize(el, null, enter);
		if (currentAnimationThread.current === thisThread)
			el.dispatchEvent(new CustomEvent(ANIMATE_SIZE_END_EVENT));
	};

	const onExit = async (el: HTMLElement) => {
		// const el = nodeRef.current;
		if (!el) return;
		const thisThread = Symbol("exit");
		currentAnimationThread.current = thisThread;
		await animateSize(el, null, exit);
		if (currentAnimationThread.current === thisThread)
			el.dispatchEvent(new CustomEvent(ANIMATE_SIZE_END_EVENT));
		// el.hidden = true;
	};

	const endListener = (node: HTMLElement, done: () => void) => {
		const listener = () => {
			node?.removeEventListener(ANIMATE_SIZE_END_EVENT, listener, false);
			done();
		};
		node?.addEventListener(ANIMATE_SIZE_END_EVENT, listener, false);
	};

	return [onEnter, onExit, endListener] as const;
}

export const STOP_TRANSITION_ID = "stop-transition";

/**
 * 为整个页面添加与颜色有关的视图过渡动画。
 * @param changeFunc - 使页面变化的回调函数。
 * @param keyframes - 动画关键帧。
 * @param options - 动画选项。
 * @returns 在动画播放完成之后可执行析构函数。
 */
export async function startColorViewTransition(changeFunc: () => MaybePromise<void | unknown>, keyframes: Keyframe[] | PropertyIndexedKeyframes, options: KeyframeAnimationOptions = {}) {
	if (!document.startViewTransition) {
		await changeFunc();
		return;
	}

	const style = document.createElement("style");
	style.id = STOP_TRANSITION_ID;
	style.textContent = String(css`
		*,
		*::before,
		*::after {
			-webkit-transition: none !important;
			-moz-transition: none !important;
			-ms-transition: none !important;
			-o-transition: none !important;
			transition: none !important;
		}

		::view-transition-old(root),
		::view-transition-new(root) {
			mix-blend-mode: normal;
			transition: none !important;
			animation: none !important;
		}

		::view-transition-old(*),
		::view-transition-new(*),
		::view-transition-old(*::before),
		::view-transition-new(*::after) {
			transition: none !important;
		}
	`);
	document.head.appendChild(style);

	options.duration ??= 300;
	options.easing ??= eases.easeInOutSmooth;
	options.pseudoElement ??= "::view-transition-new(root)";

	const transition = document.startViewTransition(changeFunc);
	await transition.ready;

	const animation = document.documentElement.animate(keyframes, options);
	await animation.finished;
	document.head.removeChild(style);
}
