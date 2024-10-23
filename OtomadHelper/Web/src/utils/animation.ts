import { flushSync as reactDomFlushSync } from "react-dom";
import type * as Styled from "styled-components";
import "utils/array";

/**
 * Remove all animations in progress from the specified DOM element.
 * @param elements - HTML DOM element.
 * @returns Have any animations been removed?
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
 * Wait for the next animation frame to update refresh.
 * @returns Empty promise.
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
 * flushSync lets you force React to flush any updates inside the provided callback synchronously.
 * This ensures that the DOM is updated immediately.
 * @param callback - A function. React will immediately call this callback and flush any updates it
 * contains synchronously. It may also flush any pending updates, or Effects, or updates inside of
 * Effects. If an update suspends as a result of this flushSync call, the fallbacks may be re-shown.
 */
export function flushSync<R>(callback: () => R): R;
/**
 * flushSync lets you force React to flush any updates inside the provided callback synchronously.
 * This ensures that the DOM is updated immediately.
 * @returns Empty promise.
 */
export function flushSync(): Promise<void>;
export function flushSync(callback?: () => unknown) {
	if (callback) return reactDomFlushSync(callback);
	else return new Promise<void>(resolve => flushSync(resolve));
}

/**
 * **Temporarily** disable transition animations when styling an element.
 * @param element - HTML DOM element or its CSS style declaration.
 * @param style - CSS style.
 */
export async function setStyleWithoutTransition(element: HTMLElement | CSSStyleDeclaration, style: CSSProperties = {}) {
	const styles = element instanceof CSSStyleDeclaration ? element : element.style;
	Object.assign(styles, style);
	styles.transition = "none";
	await nextAnimationTick();
	styles.transition = null!;
}

/**
 * Replay CSS animations.
 * @param element - HTML DOM element.
 * @param className - A CSS class name that contains animations.
 */
export async function replayAnimation(element: Element, ...className: string[]) {
	element.classList.remove(...className);
	await nextAnimationTick();
	element.classList.add(...className);
}

/**
 * ### Reflow
 * Reset CSS animations.
 * @see https://stackoverflow.com/a/45036752/19553213
 * @see https://stackoverflow.com/questions/27637184
 * @param element - HTML DOM element.
 */
export async function resetAnimation(element: HTMLElement) {
	element.style.animation = "none";
	// element.offsetHeight;
	await nextAnimationTick();
	element.style.animation = null!;
}

/**
 * Did the user request to reduce the dynamic effect?
 * @returns User requested to reduce dynamic effects.
 */
export const isPrefersReducedMotion = () => window.matchMedia("(prefers-reduced-motion: reduce)").matches;

type StyleProperties = string & keyof FilterValueType<CSSStyleDeclaration, string>;
type Keyframe = Partial<Override<Record<StyleProperties, Numberish>, { offset: number }>>;
type Keyframes = Keyframe[];
type DimensionAxis = "height" | "width" | "both";

export type AnimateSizeOptions = Partial<{
	/** Explicitly specify the start height (optional). */
	startHeight: number;
	/** Explicitly specify the end height (optional). */
	endHeight: number;
	/** Explicitly specify the start width (optional). */
	startWidth: number;
	/** Explicitly specify the end width (optional). */
	endWidth: number;
	/** Animation duration. */
	duration: number;
	/** Animation easing curves. Defaults to `ease-out-smooth`. */
	easing: string;
	/** Explicitly specify which direction needs to be animated. Defaults to both width and height animation. */
	specified: DimensionAxis;
	/** Specify the padding/margin values in which directions **not** need to be animated. */
	withoutAdjustPadding: DimensionAxis;
	/** Automatically wait for the next tick after changing the callback function. */
	// nextTick: boolean;
	/** Get the final element size. */
	// getSize: TwoD | Ref<TwoD | undefined>;
	/** Get the final element rectangle. */
	// getRect: Ref<DOMRect | undefined>;
	/** Explicitly specify the start style (optional). */
	startStyle: Keyframe;
	/** Explicitly specify the end style (optional). */
	endStyle: Keyframe;
	/** Slide into the UI from the reverse direction at the start. */
	startReverseSlideIn: boolean;
	/** Slide into the UI from the reverse direction at the end. */
	endReverseSlideIn: boolean;
	/** The start translation of the **only** child element of the element. */
	startChildTranslate: Numberish;
	/** The end translation of the **only** child element of the element. */
	endChildTranslate: Numberish;
	/** Remove the first frame of the animation to resolve possible animation glitches? Only takes effect when there are child elements in it. */
	removeGlitchFrame: boolean;
	/** Attach other animations while the animation is playing, using the same duration and easing values. */
	attachAnimations: [Element, Keyframes][] | false;
	/** No `overflow: clip;`? */
	noClipping: boolean;
	/** Keep `overflow: clip;` after the animation ends? Only available if `noClipping` is set to false. */
	keepClippingAtEnd: boolean;
	/** Perform pixel offset adjustment on the **obtained** element width and height values. */
	clientAdjustment: Partial<{
		startHeight: number;
		endHeight: number;
		startWidth: number;
		endWidth: number;
	}>;
	/** Remove existing animations on this element and its children before animation? To solve the style exception caused when the user clicks twice quickly to trigger this animation twice in a row. */
	removePreviousAnimations: boolean;
	/** Keep the style after the animation is complete? */
	fillForward: boolean;
	/** Use Scheduler API to optimize long task, used in few cases only, animation will be abnormal in other cases. */
	// optimizeTask: boolean;
}>;

/**
 * Advanced hook generator function for animated width/height when width/height value is set to auto.
 * @param element - HTML DOM element.
 * @returns A generator function that returns the animation async promise.
 * @deprecated
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
		// nextTick: waitNextTick = true,
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
	if (isPrefersReducedMotion()) duration = 0;
	let isHeightChanged = specified === "height" || specified === "both",
		isWidthChanged = specified === "width" || specified === "both";
	// element.classList.add("calc-size");
	// await scheduler.postTask(() => {
	startHeight ??= !isHeightChanged ? 0 : element.clientHeight + (clientAdjustment.startHeight ?? 0);
	startWidth ??= !isWidthChanged ? 0 : element.clientWidth + (clientAdjustment.startWidth ?? 0);
	// });
	const _hasChangeFunc = yield;
	// await scheduler.postTask(() => {
	// if (hasChangeFunc && waitNextTick) await nextTick();
	if (removePreviousAnimations) removeExistAnimations(element, element.children[0]);
	endHeight ??= !isHeightChanged ? 0 : element.clientHeight + (clientAdjustment.endHeight ?? 0);
	endWidth ??= !isWidthChanged ? 0 : element.clientWidth + (clientAdjustment.endWidth ?? 0);
	// });
	// element.classList.remove("calc-size");
	// if (getSize)
	// 	if (Array.isArray(getSize)) [getSize[0], getSize[1]] = [endWidth, endHeight];
	// 	else getSize.value = [endWidth, endHeight];
	// if (getRect)
	// 	getRect.value = element.getBoundingClientRect();
	if (startHeight === endHeight) isHeightChanged = false; // No need to change.
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
		const onlyChild = element.children[0]; // Take only one child element.
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
 * Animate width/height when width/height value is set to auto.
 * @param element - HTML DOM element.
 * @param changeFunc - A callback function that will change the width/height.
 * @param options - Configuration options.
 * @returns Animation async promise.
 * @deprecated
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
 * A lite version of `animateSize` function, suitable for simpler animations.
 * @param specified - Explicitly specify which direction needs to be animated. Defaults to height animation.
 * @param duration - Specify the animation duration.
 * @param easing - Specify the animation easing curve.
 * @param enterOptions - Specify other parameters when entering animation.
 * @param exitOptions - Specify other parameters when exiting animation.
 * @returns Returns 3 functions `onEnter`, `onExit`, `endListener`.
 * @deprecated
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
	exit.removePreviousAnimations = true;
	exit.fillForward = true;
	enter.specified = exit.specified = specified;

	// Here we use a custom event to prevent the native CSS transition animation from interfering with the operation when it ends.
	const ANIMATE_SIZE_END_EVENT = "animatesizeend";
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
 * Add color-dependent view transition animations to the entire page.
 * @param changeFunc - A callback function that will change the page.
 * @param keyframes - Animation keyframes.
 * @param options - Animation options.
 * @returns The destructor can be executed after the animation is completed.
 */
export async function startColorViewTransition(changeFunc: () => MaybePromise<void | unknown>, animations: [keyframes: Keyframe[] | PropertyIndexedKeyframes, options?: KeyframeAnimationOptions][]) {
	if (!document.startViewTransition) {
		await changeFunc();
		return;
	}

	const style = document.createElement("style");
	style.id = STOP_TRANSITION_ID;
	style.textContent = String(css`
		*,
		*::before,
		*::after,
		*::-webkit-progress-value {
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

	const transition = document.startViewTransition(changeFunc);
	await transition.ready;

	await animations.asyncMap(async ([keyframes, options]) => {
		options ??= {};
		options.duration ??= 300;
		options.easing ??= eases.easeInOutSmooth;
		options.pseudoElement ??= "::view-transition-new(root)";

		return await document.documentElement.animate(keyframes, options).finished;
	});

	document.head.removeChild(style);
}

export function forthBack_keyframes<Props extends object = object>(strings: TemplateStringsArray, ...interpolations: Array<Styled.Interpolation<Props>>) {
	const forth = keyframes(strings, ...interpolations);

	const backStrings = [...strings];
	function reverseOffset(rule: string) {
		const reg = (word: string) => new RegExp(`(?<=\\s|^)${word}(?=\\s*\\{)`, "gu");
		return rule
			.replaceAll(reg("from"), "t\0o")
			.replaceAll(reg("to"), "fr\0om")
			.replaceAll(reg("([\\d\\.]+)%"), (_, percent) => 100 - parseFloat(percent) + "%")
			.replaceAll("\0", "");
	}
	backStrings.mapImmer(rule => reverseOffset(rule));
	interpolations.mapImmer(rule => typeof rule === "string" ? reverseOffset(rule) : rule);

	const back = keyframes(backStrings as unknown as TemplateStringsArray, ...interpolations);

	return [forth, back] as const;
}

const setStyleTemporarilyQueue: Promise<void>[] = [];
/**
 * Set the style, then do something others, after that, restore the style back.
 * @param element - HTML DOM element.
 * @param style - The style to be set.
 * @param action - After set the style, do something others.
 */
export async function setStyleTemporarily(element: HTMLElement, style: CSSProperties, action: () => MaybePromise<void>) {
	if (setStyleTemporarilyQueue[0] !== undefined) await setStyleTemporarilyQueue[0];
	const { promise, resolve } = Promise.withResolvers<void>();
	setStyleTemporarilyQueue.push(promise);

	style = objectReplaceKeys(style, camel => camel.startsWith("--") ? camel : new VariableName(camel).cssProperty);
	// Convert property names to kebab case or Element.style.setProperty won't recognize them.
	if (!("transition" in style)) style.transition = "none";

	const originalStyle: Record<string, [value: Numberish, priority: StylePriority]> = {};
	for (const property of Object.keys(style)) // Copy original style.
		originalStyle[property] = [element.style.getPropertyValue(property), element.style.getPropertyPriority(property) as StylePriority];

	for (const [property, value] of Object.entries(style))
		element.style.setProperty(property, value, "important");

	await action();

	for (const [property, [value, priority]] of Object.entries(originalStyle))
		element.style.setProperty(property, value as string, priority);

	resolve();
	setStyleTemporarilyQueue.removeItem(promise);
}
