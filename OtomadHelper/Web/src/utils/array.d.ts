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
	pushUniquely(item: T): void;

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
	toUnique(): T[];

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
	unique(): void;

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

	/**
	 * Get elements in both this and the other.
	 * @param other - Another collection to compare.
	 */
	intersection(other: Iterable<T>): T[];

	/**
	 * Removes the last element from the copy of the array, and return the copy of the array.
	 */
	toPopped(): T[];

	/**
	 * Removes the first element from the copy of the array, and return the copy of the array.
	 */
	toShifted(): T[];

	/**
	 * Calls a defined callback function on each element of an array, and replace the elements in the original array with the corresponding results.
	 * @note This will modify the original array.
	 * @param callbackfn - A function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.
	 * @param thisArg - An object to which the this keyword can refer in the callbackfn function. If thisArg is omitted, undefined is used as the this value.
	 */
	mapImmer<U>(callbackfn: (value: T, index: number, array: T[]) => U, thisArg?: any): U[];

	/**
	 * Calls a defined callback asynchronous function on each element of an array, and returns an array promise that contains the results.
	 * @param callbackfn An asynchronous function that accepts up to three arguments. The map method calls the callbackfn function one time for each element in the array.
	 * @param thisArg An object to which the this keyword can refer in the callbackfn function. If thisArg is omitted, undefined is used as the this value.
	 */
	asyncMap<U>(callbackfn: (value: T, index: number, array: T[]) => MaybePromise<U>, thisArg?: any): Promise<U[]>;
}

declare interface ReadonlyArray<T> extends Pick<Array<T>,
	"mapObject",
> { }

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

	/**
	 * Toggle the item whether the set contains it.
	 * If the set contains the item, remove it; otherwise, add it.
	 * @param item - Item.
	 */
	toggle(item: T): void;

	/**
	 * Comparing two Sets for equality.
	 */
	equals(other: Set<T> | SetLike<T>): boolean;
}

declare interface Map<K, V> {
	/**
	 * Retrieves the value associated with the specified key in the Map.
	 * If the key does not exist in the Map, it will initialize the key with the provided default value and return it.
	 *
	 * @template K - The type of the keys in the Map.
	 * @template V - The type of the values in the Map.
	 *
	 * @param key - The key of the value to retrieve or initialize.
	 * @param defaultValue - Get the default value to use if the key does not exist in the Map.
	 * Note that the `defaultValue` is a function to get the value, not the value itself.
	 * If the key exists, the function will not be called again.
	 *
	 * @returns The value associated with the specified key. If the key does not exist, the default value is returned.
	 */
	getOrInit(key: K, defaultValue: () => MaybeRef<V>): V;
}
