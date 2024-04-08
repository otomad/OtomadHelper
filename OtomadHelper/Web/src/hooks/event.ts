import type { EffectCallback } from "react";
type EffectCallbackWithAsync = EffectCallback | (() => (() => Promise<void>) | void);

/**
 * Hook to run an effect when the component mounts.
 * @param effect - The effect to run when the component mounts.
 */
export function useMountEffect(effect: EffectCallback) {
	useEffect(effect, []);
}

/**
 * Hook to run an effect when the component unmounts.
 * @param effect - The effect to run when the component unmounts.
 */
export function useUnmountEffect(effect: NonNull<ReturnType<EffectCallbackWithAsync>>) {
	useEffect(() => () => void effect(), []);
}

/**
 * Asynchronous effect hook.
 *
 * This hook allows you to execute an asynchronous effect when the component is mounted or when the specified dependencies change.
 *
 * @note In this hook, you cannot return a callback function to represent the effect to run when the component unmounts.
 *
 * @param effect - The asynchronous effect to be executed. It can be a function that returns a Promise or a void function.
 * @param deps - An optional array of dependencies. If provided, the effect will be re-executed when any of the dependencies change.
 */
export function useAsyncEffect(effect: () => Promise<void> | void, deps?: DependencyList | undefined) {
	useEffect(() => void effect(), deps);
}

/**
 * Hook to store the previous value of a state variable.
 *
 * @param value - The current value of the state variable.
 * @returns The previous value of the state variable, or `undefined` if it has not been set yet.
 */
export function usePrevious<T>(value: T): T | undefined {
	const ref = useRef<T>();
	useEffect(() => {
		ref.current = value;
	});
	return ref.current;
}

const usePreviousDeps = (deps: ChangeEffectDeps): ChangeEffectDeps => {
	const prevRef = useRef(deps);
	useEffect(() => {
		prevRef.current = deps;
	}, deps);
	return prevRef.current;
};

/**
 * A custom hook that allows you to use the `useEffect` hook with an optional dependency array.
 * This hook will only execute the effect function when the dependency array changes, and will not execute it on the initial render.
 *
 * @param effect - The effect function to be executed.
 * @param deps - An optional array of dependencies. If provided, the effect function will be re-executed whenever any of the dependencies change.
 * @returns Nothing. This hook is designed to be used with the `useEffect` hook to control when the effect function is executed.
 */
export const useUpdateEffect: typeof useEffect = (effect, deps) => {
	const initialMountCountdown = useRef(import.meta.env.DEV ? 2 : 1); // 开发环境下由于 React 的严格模式会渲染两次，因此多增加一次计数。

	useEffect(() => {
		if (initialMountCountdown.current > 0)
			initialMountCountdown.current--;
		else
			return effect();
	}, deps);
};

type ChangeEffectDeps = ReadonlyArray<unknown>;
/**
 * A custom React Hook that allows you to use the `useEffect` hook with a function that takes previous values as arguments.
 *
 * @param effect - The function to be executed when the dependencies change. This function should take previous values as arguments.
 * @param deps - The dependencies for the effect.
 * @returns Nothing. This hook is used to modify the behavior of the `useEffect` hook.
 */
export const useChangeEffect = (
	effect: (...prevValue: ChangeEffectDeps) => void,
	deps: ChangeEffectDeps,
): void => {
	const prevValue = usePreviousDeps(deps);
	useUpdateEffect(() => {
		effect(...prevValue);
	}, deps);
};

type Options = Partial<{
	/** 是否立即调用？ */
	immediate: boolean;
}>;

/**
 * 我们可以将添加和清除 DOM 事件监听器的逻辑也封装进一个组合式函数中。
 * @param target - 窗体对象，注意必须是字符串。
 * @param event - 事件。
 * @param callback - 回调函数。
 * @param options - 其它选项。
 */
export function useEventListener<K extends keyof WindowEventMap>(target: Window, event: K, callback: (this: Window, ev: WindowEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * 我们可以将添加和清除 DOM 事件监听器的逻辑也封装进一个组合式函数中。
 * @param target - 文档对象，注意必须是字符串。
 * @param event - 事件。
 * @param callback - 回调函数。
 * @param options - 其它选项。
 */
export function useEventListener<K extends keyof DocumentEventMap>(target: Document, event: K, callback: (this: Document, ev: DocumentEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * 我们可以将添加和清除 DOM 事件监听器的逻辑也封装进一个组合式函数中。
 * @param target - HTML DOM 元素。
 * @param event - 事件。
 * @param callback - 回调函数。
 * @param options - 其它选项。
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: E | null, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * 我们可以将添加和清除 DOM 事件监听器的逻辑也封装进一个组合式函数中。
 * @param target - HTML DOM 元素的引用。
 * @param event - 事件。
 * @param callback - 回调函数。
 * @param options - 其它选项。
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: MutableRefObject<E | null>, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * 我们可以将添加和清除 DOM 事件监听器的逻辑也封装进一个组合式函数中。
 * @param target - HTML DOM 元素。
 * @param event - 事件。
 * @param callback - 回调函数。
 * @param options - 其它选项。
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: E | Window | Document | null, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options: Options = {}, deps: DependencyList = []): void {
	// 如果你想的话，也可以用字符串形式的 CSS 选择器来寻找目标 DOM 元素。
	useEffect(() => {
		target = toValue(target);
		asserts<() => void>(callback);
		if (options.immediate) callback();
		target?.addEventListener(event, callback);
		return () => target?.removeEventListener(event, callback);
	}, deps);
}

/**
 * 能同时监听多个事件，相当于多次重复调用 `addEventListener` 使用相同的回调函数但不同的事件类型。
 * @template K - 窗体事件枚举类型。
 * @param element - 窗体对象。
 * @param listener - 监听事件回调函数。
 * @param types - 监听事件数组。
 */
export function addEventListeners<K extends keyof WindowEventMap>(...args: [element: Window, ...types: K[], listener: (this: Window, ev: WindowEventMap[K]) => void]): void;
/**
 * 能同时监听多个事件，相当于多次重复调用 `addEventListener` 使用相同的回调函数但不同的事件类型。
 * @template K - 文档事件枚举类型。
 * @param element - 文档对象。
 * @param listener - 监听事件回调函数。
 * @param types - 监听事件数组。
 */
export function addEventListeners<K extends keyof DocumentEventMap>(...args: [element: Document, ...types: K[], listener: (this: Document, ev: DocumentEventMap[K]) => void]): void;
/**
 * 能同时监听多个事件，相当于多次重复调用 `addEventListener` 使用相同的回调函数但不同的事件类型。
 * @template K - HTML DOM 元素事件枚举类型。
 * @template E - HTML DOM 元素类型。
 * @param element - HTML DOM 元素。
 * @param listener - 监听事件回调函数。
 * @param types - 监听事件数组。
 */
export function addEventListeners<K extends keyof HTMLElementEventMap, E extends HTMLElement>(...args: [element: E, ...types: K[], listener: (this: E, ev: HTMLElementEventMap[K]) => void]): void;
/**
 * 能同时监听多个事件，相当于多次重复调用 `addEventListener` 使用相同的回调函数但不同的事件类型。
 * @template K - HTML DOM 元素事件枚举类型。
 * @template E - HTML DOM 元素类型。
 * @param element - HTML DOM 元素。
 * @param listener - 监听事件回调函数。
 * @param types - 监听事件数组。
 */
export function addEventListeners<K extends keyof HTMLElementEventMap, E extends HTMLElement>(...args: [element: E, ...types: K[], listener: (this: E, ev: HTMLElementEventMap[K]) => void]): void {
	const element = args[0];
	const listener = args.last() as Parameters<typeof addEventListeners>[1];
	const types = args.slice(1, -1) as K[];
	types.forEach(type => element.addEventListener(type, listener as never));
}
