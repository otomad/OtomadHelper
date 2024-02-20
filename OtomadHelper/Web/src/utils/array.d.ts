declare interface Array<T> {
	/**
	 * 删除数组指定索引值的项目。
	 * @param index - 索引值。
	 */
	removeAt(index: number): void;

	/**
	 * 删除数组的指定项目，如有多个重复项目只删除第一个。
	 * @param item - 项目。
	 * @returns 是否成功删除。
	 */
	removeItem(item: T): boolean;

	/**
	 * 删除数组的所有指定项目。
	 * @param item - 项目。
	 */
	removeAllItem(item: T): void;

	/**
	 * 仅在数组不包含该项目时，在数组末尾追加该项目。
	 * @param item - 项目。
	 */
	pushDistinct(item: T): void;

	/**
	 * 清空数组。
	 */
	clearAll(): void;

	/**
	 * 将源数组清空，然后重新注入新的数据。
	 * @param items - 新的数据。
	 */
	relist(items: Iterable<T>): void;

	/**
	 * 切换数组是否包含项目。如果数组包含该项目则移除，反之则添加。
	 * @param item - 项目。
	 */
	toggle(item: T): void;

	/**
	 * 通过一个任意数组映射到一个对象。
	 * @param callbackFn - 生成作为对象的键值对元组。
	 * @returns 映射的对象。
	 */
	mapObject<K extends ObjectKey, U>(callbackFn: (value: T, index: number, array: T[]) => [K, U]): Record<T, U>;

	/**
	 * 数组去重。
	 * @returns 注意是会返回一个新的数组。
	 */
	toRemoveDuplicates(): T[];

	/**
	 * 返回一个新数组，该数组将被剔除任何虚值，如 undefined、null、false、""、±0、±0n。
	 * @returns 不包含任何虚值的新数组。
	 */
	toRemoveFalsy(): NonFalsy<T>[];

	/**
	 * 判断两个数组是否相等，包括位置顺序。
	 * @returns 两个数组是否相等？
	 */
	equals(another: T[]): boolean;
}
