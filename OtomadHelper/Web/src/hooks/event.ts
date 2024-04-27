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
	const initialMountCountdown = useRef(import.meta.env.DEV ? 2 : 1); // In the development environment, React strict mode renders twice, so the count increased one.

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
	/** Call the callback function immediately upon declaration. */
	immediate: boolean;
}>;

/**
 * A hook to add an event listener to the specified target element, with both addEventListener and removeEventListener in the lifecycle.
 *
 * @template K - The type of the event to listen for, must be a key of `WindowEventMap`.
 *
 * @param target - The window instance.
 * @param event - The type of the event to listen for. Must be a key of `WindowEventMap`.
 * @param callback - The function to be called when the event is triggered.
 * @param options - An optional object containing an `immediate` property, which when `true`, will immediately call the `callback` function when the event listener is added.
 * @param deps - An optional array of dependencies. When any of the dependencies change, the event listener will be added or removed.
 *
 * @example
 * ```typescript
 * useEventListener(myDivElRef, "keydown", event => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function useEventListener<K extends keyof WindowEventMap>(target: Window, event: K, callback: (this: Window, ev: WindowEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * A hook to add an event listener to the specified target element, with both addEventListener and removeEventListener in the lifecycle.
 *
 * @template K - The type of the event to listen for, must be a key of `DocumentEventMap`.
 *
 * @param target - The document instance.
 * @param event - The type of the event to listen for. Must be a key of `DocumentEventMap`.
 * @param callback - The function to be called when the event is triggered.
 * @param options - An optional object containing an `immediate` property, which when `true`, will immediately call the `callback` function when the event listener is added.
 * @param deps - An optional array of dependencies. When any of the dependencies change, the event listener will be added or removed.
 *
 * @example
 * ```typescript
 * useEventListener(myDivElRef, "keydown", event => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function useEventListener<K extends keyof DocumentEventMap>(target: Document, event: K, callback: (this: Document, ev: DocumentEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * A hook to add an event listener to the specified target element, with both addEventListener and removeEventListener in the lifecycle.
 *
 * @template K - The type of the event to listen for, must be a key of `HTMLElementEventMap`.
 *
 * @param target - The target HTML DOM element to listen for the event.
 * @param event - The type of the event to listen for. Must be a key of `HTMLElementEventMap`.
 * @param callback - The function to be called when the event is triggered.
 * @param options - An optional object containing an `immediate` property, which when `true`, will immediately call the `callback` function when the event listener is added.
 * @param deps - An optional array of dependencies. When any of the dependencies change, the event listener will be added or removed.
 *
 * @example
 * ```typescript
 * useEventListener(myDivElRef, "keydown", event => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: E | null, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * A hook to add an event listener to the specified target element, with both addEventListener and removeEventListener in the lifecycle.
 *
 * @template K - The type of the event to listen for, must be a key of `HTMLElementEventMap`.
 *
 * @param target - The target HTML DOM element reference to listen for the event.
 * @param event - The type of the event to listen for. Must be a key of `HTMLElementEventMap`.
 * @param callback - The function to be called when the event is triggered.
 * @param options - An optional object containing an `immediate` property, which when `true`, will immediately call the `callback` function when the event listener is added.
 * @param deps - An optional array of dependencies. When any of the dependencies change, the event listener will be added or removed.
 *
 * @example
 * ```typescript
 * useEventListener(myDivElRef, "keydown", event => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: MutableRefObject<E | null>, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options?: Options, deps?: DependencyList): void;
/**
 * A hook to add an event listener to the specified target element, with both addEventListener and removeEventListener in the lifecycle.
 *
 * @template K - The type of the event to listen for, must be a key of `HTMLElementEventMap`.
 *
 * @param target - The target element to listen for the event. Can be an `HTMLElement`, `Window`, `Document`, or `null`.
 * @param event - The type of the event to listen for. Must be a key of `HTMLElementEventMap`.
 * @param callback - The function to be called when the event is triggered.
 * @param options - An optional object containing an `immediate` property, which when `true`, will immediately call the `callback` function when the event listener is added.
 * @param deps - An optional array of dependencies. When any of the dependencies change, the event listener will be added or removed.
 *
 * @example
 * ```typescript
 * useEventListener(myDivElRef, "keydown", event => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function useEventListener<K extends keyof HTMLElementEventMap, E extends HTMLElement>(target: E | Window | Document | null, event: K, callback: (this: E, ev: HTMLElementEventMap[K]) => void, options: Options = {}, deps: DependencyList = []): void {
	// If you want, you can also use CSS selectors in string form to find the target DOM element.
	useEffect(() => {
		target = toValue(target);
		asserts<() => void>(callback);
		if (options.immediate) callback();
		target?.addEventListener(event, callback);
		return () => target?.removeEventListener(event, callback);
	}, deps);
}

/**
 * Adds an event listener to the specified node for the specified multiple event types simultaneously.
 *
 * @template K - The type of the event to listen for.
 *
 * @param element - The window instance to which the event listeners will be added.
 * @param types - The event types to listen for.
 * @param listener - The function to be called when the specified event occurs.
 *
 * @example
 * ```typescript
 * addEventListeners(document.body, "click", "keydown", (event: Event) => {
 *   console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function addEventListeners<K extends keyof WindowEventMap>(...args: [element: Window, ...types: K[], listener: (this: Window, ev: WindowEventMap[K]) => void]): void;
/**
 * Adds an event listener to the specified node for the specified multiple event types simultaneously.
 *
 * @template K - The type of the event to listen for.
 *
 * @param element - The document instance to which the event listeners will be added.
 * @param types - The event types to listen for.
 * @param listener - The function to be called when the specified event occurs.
 *
 * @example
 * ```typescript
 * addEventListeners(document.body, "click", "keydown", (event: Event) => {
 *   console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function addEventListeners<K extends keyof DocumentEventMap>(...args: [element: Document, ...types: K[], listener: (this: Document, ev: DocumentEventMap[K]) => void]): void;
/**
 * Adds an event listener to the specified node for the specified multiple event types simultaneously.
 *
 * @template K - The type of the event to listen for.
 * @template E - The type of HTML DOM element.
 *
 * @param element - The HTML element to which the event listeners will be added.
 * @param types - The event types to listen for.
 * @param listener - The function to be called when the specified event occurs.
 *
 * @example
 * ```typescript
 * addEventListeners(document.body, "click", "keydown", (event: Event) => {
 * 	console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function addEventListeners<K extends keyof HTMLElementEventMap, E extends HTMLElement>(...args: [element: E, ...types: K[], listener: (this: E, ev: HTMLElementEventMap[K]) => void]): void;
/**
 * Adds an event listener to the specified node for the specified multiple event types simultaneously.
 *
 * @template K - The type of the event to listen for.
 * @template E - The type of HTML DOM element.
 *
 * @param element - The HTML element to which the event listeners will be added.
 * @param types - The event types to listen for.
 * @param listener - The function to be called when the specified event occurs.
 *
 * @example
 * ```typescript
 * addEventListeners(document.body, "click", "keydown", (event: Event) => {
 *   console.log(`Event type: ${event.type}`);
 * });
 * ```
 */
export function addEventListeners<K extends keyof HTMLElementEventMap, E extends HTMLElement>(...args: [element: E, ...types: K[], listener: (this: E, ev: HTMLElementEventMap[K]) => void]): void {
	const element = args[0];
	const listener = args.last() as Parameters<typeof addEventListeners>[1];
	const types = args.slice(1, -1) as K[];
	types.forEach(type => element.addEventListener(type, listener as never));
}

/**
 * A custom React Hook that allows you to debounce a function call.
 *
 * @template T - The type of the function to debounce.
 * @param callback - The function to debounce.
 * @param deps - The dependencies for the effect.
 * @param wait - The number of milliseconds to delay.
 * @returns A new debounced function.
 */
export function useDebounceCallback<T extends AnyFunction>(callback: T, deps: DependencyList, wait?: number) {
	return useCallback(lodash.debounce(callback, wait), deps);
}

/**
 * A custom React Hook that allows you to throttle a function call.
 *
 * @template T - The type of the function to throttle.
 * @param callback - The function to throttle.
 * @param deps - The dependencies for the effect.
 * @param wait - The number of milliseconds to wait before calling the function.
 * @returns A new throttled function.
 */
export function useThrottleCallback<T extends AnyFunction>(callback: T, deps: DependencyList, wait?: number) {
	return useCallback(lodash.throttle(callback, wait), deps);
}

export function useSelectAll<T>([selected, setSelected]: StateProperty<T[]>, allSelection: T[]) {
	selected ??= [];
	setSelected ??= noop;
	return [
		!selected.length ? "unchecked" : selected.length === allSelection.length ? "checked" : "indeterminate",
		(checkState: CheckState) => {
			if (checkState === "unchecked") setSelected([]);
			else if (checkState === "checked") setSelected(allSelection.slice());
		},
		() => {
			const invertedItems = new Set(allSelection).difference(new Set(selected));
			setSelected(Array.from(invertedItems));
		},
	] as unknown as StatePropertyNonNull<CheckState> & { 2: () => void };
}
