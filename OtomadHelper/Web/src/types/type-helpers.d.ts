export { };

declare global {
	/**
	 * 重写某个对象部分字段的类型。
	 * @template T - 源对象。
	 * @template U - 重写后的字段及其类型。
	 */
	type Override<T, U> = Omit<T, keyof U> & U;
}
