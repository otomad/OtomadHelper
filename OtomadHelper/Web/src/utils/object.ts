
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
 * 对 useState 中的 setter 函数进行拦截。
 * @param setter - useState 中的 setter 函数。
 * @param interceptor - 拦截器。
 * @returns - 生成的新 setter 函数。
 */
export function setStateInterceptor<T>(setter: SetState<T>, interceptor: (userInput: Any) => T) {
	return (userInput: React.SetStateAction<T>) => {
		if (userInput instanceof Function)
			setter(prevState => interceptor(userInput(prevState)));
		else
			setter(interceptor(userInput));
	};
}

/**
 * 判断对象是否包含该键名，同时对类型捍卫。
 * @param obj - 对象。
 * @param k - 键名。
 * @returns 对象是否包含该键名。
 */
export const hasKey = <T extends object>(obj: T, k: keyof Any): k is keyof T => k in obj;

/**
 * 将原 useState 映射到例如其子属性上的新属性。
 * @param stateProperty - 原 useState。
 * @param getter - 映射到新的 getter。
 * @param setter - 映射到新的 setter。
 * @returns - 新的 useState。
 */
export function useStateSelector<T, U>(stateProperty: StateProperty<T>, getter: (original: T) => U, setter: (original: SetState<T>) => SetState<U>) {
	return [
		getter(stateProperty[0]),
		setter(stateProperty[1]),
	] as StateProperty<U>;
}
