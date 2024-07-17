declare interface Array<T> {
	/**
	 * Delete the item in the array with the specified index value.
	 * @param index - Index value.
	 */
	removeAt(index: number): void;

	/**
	 * Delete the specified item in the array. If there are multiple duplicate items, only the first one will be deleted.
	 * @param items - Items.
	 * @returns Successfully deleted count.
	 */
	removeItem(...items: T[]): number;

	/**
	 * Delete all specified items in the array.
	 * @param items - Items.
	 */
	removeAllItem(...items: T[]): void;

	/**
	 * Insert items to the specified index in the array.
	 * @param index - Index.
	 * @param items - Items.
	 */
	insert(index: number, ...items: T[]): void;

	/**
	 * Append the item to the end of the array only if it is not included.
	 * @param item - Item.
	 */
	pushDistinct(item: T): void;

	/**
	 * Empty the array.
	 */
	clearAll(): void;

	/**
	 * Empty the source array and then inject new data.
	 * @param items - New data.
	 */
	relist(items: Iterable<T>): void;

	/**
	 * Toggle the item whether the array contains it.
	 * If the array contains the item, remove it; otherwise, add it.
	 * @param item - Item.
	 */
	toggle(item: T): void;

	/**
	 * Returns a random item from the array.
	 * @param record - Random records. If provided, identical items will not be drawn until all items have been randomly drawn.
	 * @returns A random item in the array.
	 */
	randomOne(record?: MaybeRef<number[]>): T;

	/**
	 * Map to an object via any array.
	 * @param callbackFn - Generate key value tuples as objects.
	 * @returns The mapped object.
	 */
	mapObject<K extends PropertyKey, U>(callbackFn: (value: T, index: number, array: T[]) => [K, U]): Record<T, U>;

	/**
	 * Array deduplication. This will return a new array.
	 * @returns Note that a new array will be returned.
	 */
	toDeduplicated(): T[];

	/**
	 * Returns a new array that will exclude any falsy values, such as undefined, null, false, "", ±0, ±0n。
	 * @returns A new array without any falsy values.
	 */
	toRemoveFalsy(): NonFalsy<T>[];

	/**
	 * Determine whether two arrays are equal, including positional order.
	 * @returns Are two arrays equal?
	 */
	equals(another: T[]): boolean;

	/**
	 * Get the last element of the array.
	 * @remarks If the array is empty, it will return undefined. However, at the TypeScript type level, there is an implicit empty removal, which is consistent with the type got when accessing elements directly using index values in `[]`. But if the type of the array already contains undefined, it will not remove empty.
	 * @return The last element of the array.
	 */
	last(): T;

	/**
	 * Array deduplication. This will modify the original array.
	 */
	deduplicate(): void;

	/**
	 * Remove undefined, null, NaN, and strings containing only white space characters from the array. This will return a new array.
	 */
	toTrimmed(): T[];

	/**
	 * Remove undefined, null, NaN, and strings containing only white space characters from the array. This will modify the original array.
	 */
	trim(): void;

	/**
	 * Swap two items by their index in the array. This will modify the original array.
	 * @param index1 - The index of the first item.
	 * @param index2 - The index of the second item.
	 */
	swap(index1: number, index2: number): T[];
}

declare interface Set<T> {
	/**
	 * Appends multiple new elements with multiple specified values to the end of the Set.
	 */
	adds(...values: T[]): this;

	/**
	 * Removes multiple specified values from the Set.
	 * @returns Returns the number of the element in the Set existed and has been removed.
	 */
	deletes(...values: T[]): number;
}
