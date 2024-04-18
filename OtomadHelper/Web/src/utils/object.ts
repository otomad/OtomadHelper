/**
 * 返回一个给定对象自身可枚举属性的键值对数组，其排列与使用 `for...in` 循环遍历该对象时返回的顺序一致
 * （区别在于 for-in 循环还会枚举原型链中的属性）。
 *
 * 相较于 `Object.entries`，返回的值的类型不会那么恶心。
 * @template K - 对象的键名枚举类型。
 * @template V - 对象的值类型。
 * @param obj - 可以返回其可枚举属性的键值对的对象。
 * @returns 给定对象自身可枚举属性的键值对数组。
 */
export function entries<K extends string | number | symbol, V>(obj: { [s in K]?: V }) {
	return Object.entries(obj) as [K, V][];
}

/**
 * 返回一个由给定对象自身的可枚举的字符串键属性名组成的数组。
 *
 * 相较于 `Object.keys`，返回的值的类型不会那么恶心。
 * @param obj - 一个对象。
 * @returns 一个由给定对象自身可枚举的字符串键属性键组成的数组。
 */
export function keys<K extends object>(obj: K) {
	return Object.keys(obj) as (keyof K)[];
}

/**
 * 将所有可枚举自身属性的值从一个或多个源对象复制到目标对象。返回目标对象。
 * @param target — 要复制到的目标对象。
 * @param sources — 要从中复制属性的源对象。
 */
export function assign<T extends object>(target: T, ...sources: Partial<T>[]): T {
	return Object.assign(target, ...sources);
}

/**
 * 创建一个重复指定次数对象的数组，用于循环创建组件。
 *
 * 但是只需关心循环次数，不关心数组的内容。
 * @template T - 要重复的对象类型。
 * @param length - 循环次数。
 * @param callback - map 回调函数。
 * @param flat - 是否扁平化数组？
 * @returns 重复指定次数对象的数组。
 */
export function forMap<T>(length: number, callback: (index: number) => T, flat: boolean = false) {
	const mapAction = (flat ? "flatMap" : "map") as "map";
	return Array<void>(length).fill(undefined)[mapAction]((_, index) => callback(index + 1));
}

/**
 * 从开始到结束数值 map 数组。
 * @param start - 开始数值。
 * @param end - 结束数值。
 * @param callback - map 回调函数。
 * @returns 重复指定次数对象的数组。
 */
export function forMapFromTo<T>(start: number, end: number, callback: (index: number) => T) {
	const result: T[] = [];
	for (let i = start; i <= end; i++)
		result.push(callback(i));
	return result;
}

/**
 * 判断对象是否包含该键名，同时对类型捍卫。
 * @param obj - 对象。
 * @param k - 键名。
 * @returns 对象是否包含该键名。
 */
export const hasKey = <T extends object>(obj: T, k: keyof Any): k is keyof T => k in obj;

/**
 * 对 useState 中的 setter 函数进行拦截。
 * @param setter - useState 中的 setter 函数。
 * @param interceptor - 拦截器。
 * @returns 生成的新 setter 函数。
 */
export function setStateInterceptor<T>(setter: SetState<T>, interceptor: (userInput: Any) => T) {
	return (userInput: React.SetStateAction<T>) => {
		type PrevStateSetter = (value: (prevState: T) => void) => void;
		if (userInput instanceof Function)
			(setter as PrevStateSetter)(prevState => interceptor(userInput(prevState)));
		else
			setter(interceptor(userInput));
	};
}

/**
 * 将原 useState 映射到例如其子属性上的新属性。
 * @param stateProperty - 原 useState。
 * @param getter - 映射到新的 getter。
 * @param setter - 映射到新的 setter。
 * @returns 新的 useState。
 */
export function useStateSelector<T, U>(stateProperty: StateProperty<T>, getter: (original: T) => U, setter: (userInput: U) => T) {
	return [
		getter(stateProperty[0]!),
		setStateInterceptor(stateProperty[1]!, setter),
	] as StateProperty<U>;
}

/**
 * 判断一个变量是否是 Ref 包装对象。
 * @param ref - Ref 包装对象。
 * @returns 是否是 Ref 包装对象。
 */
export function isRef<T>(ref: MaybeRef<T>): ref is RefObject<T> {
	return ref && Object.hasOwn(ref, "current") && Object.keys(ref).length === 1;
}

/**
 * 将一个可能是 Ref 包装对象的值还原其本身。
 * @param ref - Ref 包装对象。
 * @returns 原类型值。
 */
export function toValue<T>(ref: MaybeRef<T>): T {
	return isRef(ref) ? ref.current! : ref;
}

/**
 * 创建一个 HTML DOM 元素的引用，不必再初始化为 null 了。
 * @param initialValue - 初始值，一般都为空。
 * @returns HTML DOM 元素的引用。
 */
export function useDomRef<E extends keyof ElementTagNameMap | Element>(initialValue: TagNameToElement<E> | null = null) {
	return useRef<TagNameToElement<E> | null>(initialValue);
}

/**
 * 创建一个 HTML DOM 元素的引用的状态，不必再初始化为 null 了。
 *
 * **注意：**这将会返回一个 `StateProperty`，即对引用的修改也会引发组件重新渲染。
 * @param initialValue - 初始值，一般都为空。
 * @returns HTML DOM 元素的引用的状态。
 */
export function useDomRefState<E extends keyof ElementTagNameMap | Element>(initialValue: TagNameToElement<E> | null = null) {
	return useState<TagNameToElement<E> | null>(initialValue);
}

/**
 * 返回全局环境，用于定义全局变量。
 *
 * 同时规避 TypeScript 的警告。
 */
export const globals = globalThis as AnyObject;

/**
 * 检查值是否类似对象。如果值不是“null”，并且具有“object”的“typeof”结果，则该值与 object 类似。
 * @param value - 要检查的值。
 * @return 如果值是对象，则返回 true，否则返回 false。
 */
export function isObject(value: unknown): value is object {
	return lodash.isObjectLike(value);
}

/**
 * 断言指定的对象。注意由于 TypeScript 的限制，它只能缩小类型。
 * @param object - 要断言的对象。
 */
export function asserts<T>(object: unknown): asserts object is T { }

export function isUndefinedNullNaN(object: unknown): object is undefined | null {
	return [undefined, null, NaN].includes(object as never);
}

export function makePrototypeKeysNonEnumerable(constructor: AnyConstructor) {
	const protoKeys = Object.keys(constructor.prototype);
	for (const protoKey of protoKeys)
		Object.defineProperty(constructor.prototype, protoKey, {
			enumerable: false,
		});
}

/**
 * A no-operation function that returns undefined regardless of the arguments it receives.
 *
 * @return undefined
 */
export const noop = lodash.noop;

/**
 * @deprecated 等待 React 19 发布，支持 ref as a prop 之后，即可删除本函数。
 */
export function functionModule<F extends Function, O>(
	func: F,
	object: O,
) {
	return Object.assign(func, object) as F & Readonly<O>;
}
