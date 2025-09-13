declare interface ObjectConstructor {
	/**
	 * Creates a new object composed of the picked `object` properties.
	 * @category Object
	 * @param object - Source object.
	 * @param pickedKeys - keys of properties you want to pick from the object, specified in arrays.
	 * @returns Returns the new object.
	 * @example
	 * ```javascript
	 * Object.pick({ a: 1, b: 2, c: 3 }, ["a", "c"]); // { a: 1, c: 3 }
	 * ```
	 */
	pick<T extends object, U extends keyof T>(object: T | undefined | null, pickedKeys: U[]): Pick<T, U>;
	/**
	 * Creates a new object composed of the `object` properties `predicate` returns truthy for.
	 * @category Object
	 * @param object - Source object.
	 * @param predicate - The function to predicted whether the property should be picked.
	 * - `currentValue`: the current value processed in the object.
	 * - `key`: the key of the `currentValue` in the object.
	 * - `object`: the object `pick` was called upon.
	 * @param thisArg - The object used as `this` inside the predicted function.
	 * @returns Returns the new object.
	 * @example
	 * ```javascript
	 * Object.pick({ a: 1, b: "2", c: 3 }, x => typeof x === "number"); // { a: 1, c: 3 }
	 * ```
	 */
	pick<T extends object>(object: T, predicate: (currentValue: T[keyof T], key: keyof T, object: T | undefined | null) => boolean, thisArg?: any): Partial<T>;

	/**
	 * Creates a new object composed of the own and inherited enumerable properties of `object` that are not omitted.
	 * @category Object
	 * @param object - Source object.
	 * @param omittedKeys - keys of properties you want to omit from the object, specified in arrays.
	 * @returns Returns the new object.
	 * @example
	 * ```javascript
	 * Object.omit({ a: 1, b: 2, c: 3 }, ["a", "c"]); // { b: 2 }
	 * ```
	 */
	omit<T extends object, U extends keyof T>(object: T | undefined | null, omittedKeys: U[]): Omit<T, U>;
	/**
	 * creates a new object composed of the own and inherited enumerable properties of `object` that `predicate` doesn't return truthy for.
	 * @category Object
	 * @param object - Source object.
	 * @param predicate - The function to predicted whether the property should be omitted.
	 * - `currentValue`: the current value processed in the object.
	 * - `key`: the key of the `currentValue` in the object.
	 * - `object`: the object `omit` was called upon.
	 * @param thisArg - The object used as `this` inside the predicted function.
	 * @returns Returns the new object.
	 * @example
	 * ```javascript
	 * Object.omit({ a: 1, b: "2", c: 3 }, x => typeof x === "number"); // { b: "2" }
	 * ```
	 */
	omit<T extends object>(object: T, predicate: (currentValue: T[keyof T], key: keyof T, object: T | undefined | null) => boolean, thisArg?: any): Partial<T>;

	/**
	 * Replaces the keys of an object with new keys obtained from a provided function.\
	 * Creates a new object with the replaced keys and their corresponding values.
	 *
	 * @template T - The type of the input object. Must be an object type.
	 * @param object - The object whose keys need to be replaced.
	 * @param replacement - A function that takes an old key as input and returns the new key.
	 * @returns A new object with the replaced keys and their corresponding values.
	 *
	 * @example
	 * ```typescript
	 * Object.replaceKeys({ a: 1, b: 2, c: 3 }, key => key.toUpperCase()); // { A: 1, B: 2, C: 3 }
	 * ```
	 */
	replaceKeys<T extends object>(object: T, replacement: (oldKey: keyof T) => string): T;

	/**
	 * Clear all keys of an object.
	 * @param object - Source object.
	 */
	clear(object: AnyObject): void;

	/**
	 * Get the key and value of an object by the index at the definition time.
	 * @param object - Source object.
	 * @param index - The index at the definition time.
	 * @returns The entry (a tuple of key and value) of the object, or empty array if cannot be found.
	 */
	indexOf<T extends object>(object: T, index: number): [key: keyof T, value: T[keyof T]];

	/**
	 * Delete all properties with `undefined` values from the object.
	 * @note This will modify the original object.
	 * @param object - The object to compact.
	 */
	compactUndefined(object: T): void;
}
