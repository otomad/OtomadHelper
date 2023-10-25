export { };

declare global {
	/**
	 * 重写某个对象部分字段的类型。
	 * @template T - 源对象。
	 * @template U - 重写后的字段及其类型。
	 */
	type Override<T, U> = Omit<T, keyof U> & U;

	/**
	 * 从一个接口中筛选值为指定类型的子集接口。
	 * @template Source - 源接口。
	 * @template Condition - 筛选值的类型。
	 */
	type FilterValueType<Source, Condition> = Pick<
		Source,
		{
			[K in keyof Source]: Source[K] extends Condition ? K : never
		}[keyof Source]
		>;

	/**
	 * 可能是 Ref 包装的类型对象也可能是其类型本身。
	 */
	type MaybeRef<T> = RefObject<T> | T;
}
