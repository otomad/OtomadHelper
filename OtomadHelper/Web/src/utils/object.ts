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
 * 创建一个重复指定次数对象的数组，用于循环创建组件。
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
 * @returns - 生成的新 setter 函数。
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
 * @returns - 新的 useState。
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
 * @returns - 是否是 Ref 包装对象。
 */
export function isRef<T>(ref: MaybeRef<T>): ref is RefObject<T> {
	return ref && "current" in (ref as object) && Object.keys(ref).length === 1;
}

/**
 * 将一个可能是 Ref 包装对象的值还原其本身。
 * @param ref - Ref 包装对象。
 * @returns 原类型值。
 */
export function toValue<T>(ref: MaybeRef<T>): T {
	return isRef(ref) ? ref.current! : ref;
}