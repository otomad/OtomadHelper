/**
 * 返回一个新数组，该数组将被剔除任何虚值，如 undefined、null、false、""、±0、±0n。
 * @param array - 源数组。
 * @returns 不包含任何虚值的新数组。
 */
export function arrayToRemoveFalsy<T>(array: T[]) {
	return array.filter(item => item) as NonFalsy<T>[];
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
