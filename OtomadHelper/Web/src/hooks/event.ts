export function useMountEffect(effect: React.EffectCallback) {
	useEffect(effect, []);
}

export function useUnmountEffect(effect: NonNull<ReturnType<React.EffectCallback>>) {
	useEffect(() => effect, []);
}

/**
 * 获取某个值之前的值。
 * @param value - 值对象。
 * @returns 先前的值。
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

const useUpdateEffect: typeof useEffect = (effect, deps) => {
	const firstMount = useRef(true);
	useEffect(() => {
		if (firstMount.current) {
			firstMount.current = false;
			return;
		}
		effect();
	}, deps);
};

type ChangeEffectDeps = ReadonlyArray<unknown>;
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
		if (options.immediate) (callback as () => void)();
		target?.addEventListener(event, callback as never);
		return () => target?.removeEventListener(event, callback as never);
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
	const listener = args.at(-1) as Parameters<typeof addEventListeners>[1];
	const types = args.slice(1, -1) as K[];
	types.forEach(type => element.addEventListener(type, listener as never));
}
