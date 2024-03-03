/// <reference path="array.d.ts" />

/*
 * JS Array 又不自带删除方法，想用 prototype 扩展语法，问题是又不推荐使用。
 */
{ // Init array extensions
	Array.prototype.removeAt = function (index) {
		this.splice(index, 1);
	};

	Array.prototype.removeItem = function (item) {
		const index = this.indexOf(item);
		if (index === -1) return false;
		this.splice(index, 1);
		return true;
	};

	Array.prototype.removeAllItem = function (item) {
		while (true) {
			const index = this.indexOf(item);
			if (index === -1) return;
			this.splice(index, 1);
		}
	};

	Array.prototype.pushDistinct = function (item) {
		if (!this.includes(item))
			this.push(item);
	};

	Array.prototype.clearAll = function () {
		this.splice(0, Infinity);
	};

	Array.prototype.relist = function (items) {
		this.splice(0, Infinity, ...items);
	};

	Array.prototype.toggle = function (item) {
		const index = this.indexOf(item);
		if (index === -1)
			this.push(item);
		else
			this.removeAt(index);
	};

	Array.prototype.mapObject = function (callbackFn) {
		const array = this;
		return Object.fromEntries(array.map((value, index, array) => callbackFn(value, index, array)));
	};

	Array.prototype.toRemoveDuplicates = function () {
		return [...new Set(this)];
	};

	/**
	 * 返回一个新数组，该数组将被剔除任何虚值，如 undefined、null、false、""、±0、±0n。
	 * @param this - 源数组。
	 * @returns 不包含任何虚值的新数组。
	 */
	Array.prototype.toRemoveFalsy = function () {
		return this.filter(item => item);
	};

	/**
	 * 判断两个数组是否相等，包括位置顺序。
	 * @returns 两个数组是否相等？
	 */
	Array.prototype.equals = function (another) {
		if (this === another) return true;
		if (!this || !another) return false;
		if (this.length !== another.length) return false;

		for (let i = 0; i < this.length; i++)
			if (this[i] !== another[i])
				return false;
		return true;
	};

	const protoKeys = Object.keys(Array.prototype);
	for (const protoKey of protoKeys)
		Object.defineProperty(Array.prototype, protoKey, {
			enumerable: false,
		});
}

/**
 * 通过一个常量数组映射到一个对象。
 * @remarks 此 JSDoc 的 `@param` 部分参数后故意没加 “-”，否则会出现 bug。
 * @param this **常量**字符串数组。
 * @param callbackFn 生成作为对象的值。
 * @returns 映射的对象。
 */
export function mapObjectConst<const T extends string, U>(array: T[], callbackFn: (value: T, index: number, array: T[]) => U) {
	return Object.fromEntries(array.map((value, index, array) => ([value, callbackFn(value, index, array)] as [T, U]))) as Record<T, U>;
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
