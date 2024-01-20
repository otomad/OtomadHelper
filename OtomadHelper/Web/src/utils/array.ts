/*
 * JS Array 又不自带删除方法，想用 prototype 扩展语法，问题是又不推荐使用。
 */

/**
 * 删除数组指定索引值的项目。
 * @param array - 数组。
 * @param index - 索引值。
 */
export function arrayRemoveAt<T>(array: T[], index: number): void {
	array.splice(index, 1);
}

/**
 * 删除数组的指定项目，如有多个重复项目只删除第一个。
 * @param array - 数组。
 * @param item - 项目。
 * @returns 是否成功删除。
 */
export function arrayRemoveItem<T>(array: T[], item: T): boolean {
	const index = array.indexOf(item);
	if (index === -1) return false;
	array.splice(index, 1);
	return true;
}

/**
 * 删除数组的所有指定项目。
 * @param array - 数组。
 * @param item - 项目。
 */
export function arrayRemoveAllItem<T>(array: T[], item: T) {
	while (true) {
		const index = array.indexOf(item);
		if (index === -1) return;
		array.splice(index, 1);
	}
}

/**
 * 仅在数组不包含该项目时，在数组末尾追加该项目。
 * @param array - 数组。
 * @param item - 项目。
 */
export function arrayPushDistinct<T>(array: T[], item: T) {
	if (!array.includes(item))
		array.push(item);
}

/**
 * 清空数组。
 * @param array - 数组。
 */
export function arrayClearAll<T>(array: T[]): void {
	array.splice(0, Infinity);
}

/**
 * 将源数组清空后重新注入新的数据。
 * @param array - 源数组。
 * @param items - 新的数据。
 */
export function arrayRelist<T>(array: T[], items: Iterable<T>): void {
	array.splice(0, Infinity, ...items);
}

/**
 * 切换数组是否包含项目。如果数组包含该项目则移除，反之则添加。
 * @param array - 数组。
 * @param item - 项目。
 */
export function arrayToggle<T>(array: T[], item: T): void {
	const index = array.indexOf(item);
	if (index === -1)
		array.push(item);
	else
		arrayRemoveAt(array, index);
}

/**
 * 通过一个常量数组映射到一个对象。
 * @remarks 此 JSDoc 的 `@param` 部分参数后故意没加 “-”，否则会出现 bug。
 * @param array **常量**字符串数组。
 * @param callbackFn 生成作为对象的值。
 * @returns 映射的对象。
 */
export function arrayMapObjectConst<const T extends string, U>(array: T[], callbackFn: (value: T, index: number, array: T[]) => U) {
	return Object.fromEntries(array.map((value, index, array) => ([value, callbackFn(value, index, array)] as [T, U]))) as Record<T, U>;
}

/**
 * 通过一个任意数组映射到一个对象。
 * @param array - 任意数组。
 * @param callbackFn - 生成作为对象的键值对元组。
 * @returns 映射的对象。
 */
export function arrayMapObject<T, K extends ObjectKey, U>(array: T[], callbackFn: (value: T, index: number, array: T[]) => [K, U]) {
	return Object.fromEntries(array.map((value, index, array) => callbackFn(value, index, array))) as Record<K, U>;
}

/**
 * 类似于 Python 的 `enumerate` 函数。
 *
 * `enumerate()` 函数用于将一个可遍历的数据对象（如列表、元组或字符串）组合为一个索引序列，同时列出数据和数据下标，一般用在 for...of 循环当中。
 * @param array - 一个序列、迭代器或其他支持迭代对象。
 * @param start - 下标起始位置的值，默认为 0。
 * @returns 返回依次包含索引值、元素值、源数组的元组。
 */
export function enumerate<T>(array: T[], start: number = 0) {
	return array.map((value, index, array) => [index + start, value, array] as const);
}

/**
 * 数组去重。
 * @param array - 数组。
 * @returns 注意是会返回一个新的数组。
 */
export function arrayToRemoveDuplicates<T>(array: T[]) {
	return [...new Set(array)];
}

/**
 * 返回一个新数组，该数组将被剔除任何虚值，如 undefined、null、false、""、±0、±0n。
 * @param array - 源数组。
 * @returns 不包含任何虚值的新数组。
 */
export function arrayToRemoveFalsy<T>(array: T[]) {
	return array.filter(item => item) as NonFalsy<T>[];
}

/**
 * 判断两个数组是否相等，包括位置顺序。
 * @returns 两个数组是否相等？
 */
export function arrayEquals<T>(a: T[], b: T[]) {
	if (a === b) return true;
	if (!a || !b) return false;
	if (a.length !== b.length) return false;

	for (let i = 0; i < a.length; i++)
		if (a[i] !== b[i])
			return false;
	return true;
}

// #region Tuples
/** 创建可供 TypeScript 正确识别的一元组。 */
export function Tuple<T>(arg1: T): [T];
/** 创建可供 TypeScript 正确识别的二元组。 */
export function Tuple<T, U>(arg1: T, arg2: U): [T, U];
/** 创建可供 TypeScript 正确识别的三元组。 */
export function Tuple<T, U, V>(arg1: T, arg2: U, arg3: V): [T, U, V];
/** 创建可供 TypeScript 正确识别的四元组。 */
export function Tuple<T, U, V, W>(arg1: T, arg2: U, arg3: V, arg4: W): [T, U, V, W];
/** 创建可供 TypeScript 正确识别的五元组。 */
export function Tuple<T, U, V, W, X>(arg1: T, arg2: U, arg3: V, arg4: W, arg5: X): [T, U, V, W, X];
/** 创建可供 TypeScript 正确识别的六元组。 */
export function Tuple<T, U, V, W, X, Y>(arg1: T, arg2: U, arg3: V, arg4: W, arg5: X, arg6: Y): [T, U, V, W, X, Y];
/** 创建可供 TypeScript 正确识别的七元组。 */
export function Tuple<T, U, V, W, X, Y, Z>(arg1: T, arg2: U, arg3: V, arg4: W, arg5: X, arg6: Y, arg7: Z): [T, U, V, W, X, Y, Z];
export function Tuple(...args: unknown[]) {
	return [...args];
}
// #endregion
