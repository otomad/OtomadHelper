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
 */
export function endListener(): (node: HTMLElement, done: () => void) => void;
/**
 * 用于获取元素动画在何时结束，以助于自动获取动画时间。
 * @param nodeRef - 获取的子元素引用对象。
 */
export function endListener(nodeRef: RefObject<HTMLElement | undefined>): (done: () => void) => void;
export function endListener(nodeRef?: RefObject<HTMLElement | undefined>) {
	return !nodeRef ?
		(node: HTMLElement, done: () => void) => node.addEventListener("transitionend", done, false) :
		(done: () => void) => nodeRef.current?.addEventListener("transitionend", done, false);
}

type StyleProperties = string & keyof FilterValueType<CSSStyleDeclaration, string>;
type Keyframe = Partial<Override<Record<StyleProperties, Numberish>, { offset: number }>>;
type Keyframes = Keyframe[];
type DimensionAxis = "height" | "width" | "both";
type MaybePromise<T> = T | Promise<T>;

type AnimateSizeOptions = Partial<{
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
	/** 不要 `overflow: hidden;`？ */
	noCropping: boolean;
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
		noCropping = false,
	}: AnimateSizeOptions = {},
): AsyncGenerator<void, Animation | void, boolean> {
	element = toValue(element);
	if (!element) return;
	// if (isPrefersReducedMotion()) duration = 0;
	startHeight ??= element.clientHeight;
	startWidth ??= element.clientWidth;
	const _hasChangeFunc = yield;
	// if (hasChangeFunc && awaitNextTick) await nextTick();
	endHeight ??= element.clientHeight;
	endWidth ??= element.clientWidth;
	// if (getSize)
	// 	if (getSize instanceof Array) [getSize[0], getSize[1]] = [endWidth, endHeight];
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
	const setXPadding = withoutAdjustPadding === undefined || withoutAdjustPadding === "height",
		setYPadding = withoutAdjustPadding === undefined || withoutAdjustPadding === "width";
	if (setXPadding && isHeightChanged && setYPaddingIndex !== undefined)
		Object.assign(keyframes[setYPaddingIndex], { paddingTop: 0, paddingBottom: 0, marginTop: 0, marginBottom: 0 });
	if (setYPadding && isWidthChanged && setXPaddingIndex !== undefined)
		Object.assign(keyframes[setXPaddingIndex], { paddingLeft: 0, paddingRight: 0, marginLeft: 0, marginRight: 0 });
	const setTranslate = (pxes: number[]) => pxes.map(i => i + "px").join(" ");
	if (startReverseSlideIn)
		keyframes[0].translate = setTranslate([isWidthChanged ? endWidth : 0, isHeightChanged ? endHeight : 0]);
	if (endReverseSlideIn)
		keyframes[1].translate = setTranslate([isWidthChanged ? startWidth : 0, isHeightChanged ? startHeight : 0]);
	Object.assign(keyframes[0], startStyle);
	Object.assign(keyframes[1], endStyle);
	const animationOptions: KeyframeAnimationOptions = { duration, easing };
	const htmlElement = element as HTMLElement;
	if (!noCropping) htmlElement.style.overflow = "hidden";
	const result = element.animate(keyframes, animationOptions);
	if (!noCropping) result.addEventListener("finish", () => htmlElement.style.removeProperty("overflow"));
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
	return result.finished;
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

type SameOrDifferent<T> = T | undefined | [T | undefined, T | undefined];

/**
 * `animateSize` 函数的简化版，适用于更为简单的动画。
 * @param specified - 显式指定需要动画的是哪个方向。默认为高度动画。
 * @param duration - 指定动画时间。
 * @param easing - 指定动画缓动曲线。
 * @returns 返回 `onEnter` 和 `onExit` 两个函数。
 */
export function simpleAnimateSize(nodeRef: RefObject<HTMLElement>, specified: "width" | "height" = "height", duration?: SameOrDifferent<number>, easing?: SameOrDifferent<string>) {
	type Options = Parameters<typeof animateSize>[2];
	let enter: Options, exit: Options;
	if (specified === "width") {
		enter = { startWidth: 0 };
		exit = { endWidth: 0 };
	} else {
		enter = { startHeight: 0 };
		exit = { endHeight: 0 };
	}
	duration = duration instanceof Array ? duration : [duration, duration];
	easing = easing instanceof Array ? easing : [easing, easing];
	enter.duration = duration[0];
	exit.duration = duration[1];
	enter.easing = easing[0];
	exit.easing = easing[1];

	const onEnter = async () => {
		const el = nodeRef.current;
		if (!el) return;
		await animateSize(el, null, enter);
		el.dispatchEvent(new Event("transitionend"));
	};

	const onExit = async () => {
		const el = nodeRef.current;
		if (!el) return;
		await animateSize(el, null, exit);
		el.dispatchEvent(new Event("transitionend"));
		el.hidden = true;
	};

	return [onEnter, onExit];
}
