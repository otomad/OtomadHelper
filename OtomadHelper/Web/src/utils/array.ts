{ // Init array extensions
	Array.prototype.removeAt = function (index) {
		this.splice(index, 1);
	};

	Array.prototype.removeItem = function (...items) {
		let successes = 0;
		for (const item of items) {
			const index = this.indexOf(item);
			if (index === -1) continue;
			this.splice(index, 1);
			successes++;
		}
		return successes;
	};

	Array.prototype.removeAllItem = function (...items) {
		let successes = 0;
		for (let i = this.length - 1; i >= 0; i--)
			if (items.includes(this[i])) {
				this.splice(i, 1);
				successes++;
			}
		return successes;
	};

	Array.prototype.insert = function (index, ...items) {
		if (index < 0) index = this.length + 1 + index;
		this.splice(index, 0, ...items);
	};

	Array.prototype.pushUniquely = function (...items) {
		for (const item of items)
			if (!this.includes(item))
				this.push(item);
	};

	Array.prototype.clearAll = function () {
		this.splice(0, Infinity);
	};

	Array.prototype.relist = function (items) {
		this.splice(0, Infinity, ...items);
	};

	Array.prototype.toggle = function (item, force) {
		const index = this.indexOf(item);
		if (index === -1 || force)
			this.push(item);
		else
			this.removeAt(index);
	};

	Array.prototype.randomOne = function (record) {
		if (this.length === 0) return null;
		record = toValue(record);
		let index = randBetween(0, this.length - 1);
		if (record !== undefined) {
			if (record.length !== this.length + 1 || record.every((n, i) => i === 0 || n)) {
				let last = +record[0];
				if (!Number.isFinite(last)) last = -1;
				record.relist(Array(this.length + 1).fill(0));
				record[0] = last;
			}
			while (record[index + 1] !== 0 || index === record[0])
				index = randBetween(0, this.length - 1);
			record[index + 1] = 1;
			record[0] = index;
		}
		return this[index];
	};

	Array.prototype.mapObject = function (callbackFn) {
		const array = this;
		return Object.fromEntries(array.map((value, index, array) => callbackFn(value, index, array)));
	};

	Array.prototype.toUnique = function (callbackFn) {
		if (!callbackFn) return [...new Set(this)];
		const seen = new Map();
		this.map(callbackFn).forEach((ret, i) => {
			const item = this[i];
			if (!seen.has(ret)) seen.set(ret, item);
		});
		return [...seen.values()];
	};

	Array.prototype.toCompacted = function () {
		return this.filter(Boolean);
	};

	Array.prototype.equals = function (another) {
		if (this === another) return true;
		if (this == null || another == null) return false;
		if (this.length !== another.length) return false;

		for (let i = 0; i < this.length; i++)
			if (this[i] !== another[i])
				return false;
		return true;
	};

	Array.prototype.last = function () {
		return this.at(-1);
	};

	Array.prototype.first = function () {
		return this[0];
	};

	Array.prototype.unique = function (callbackFn) {
		return this.relist(this.toUnique(callbackFn));
	};

	Array.prototype.toTrimmed = function () {
		return this.filter(item => !(isUndefinedNullNaN(item) || typeof item === "string" && item.trim() === ""));
	};

	Array.prototype.trim = function () {
		return this.relist(this.toTrimmed());
	};

	Array.prototype.swap = function (index1, index2) {
		[this[index1], this[index2]] = [this[index2], this[index1]];
		return this;
	};

	Array.prototype.intersection = function (other) {
		return [...new Set(this).intersection(new Set(other))];
	};

	Array.prototype.toPopped = function () {
		return this.slice(0, -1);
	};

	Array.prototype.toShifted = function () {
		return this.slice(1);
	};

	Array.prototype.toPushed = function (...items) {
		return this.concat(items);
	};

	Array.prototype.toUnshifted = function (...items) {
		return this.toSpliced(0, 0, ...items);
	};

	Array.prototype.mapImmer = function (callbackfn, thisArg) {
		for (const [index, element] of this.entries()) {
			const result = callbackfn.call(thisArg, element, index, this);
			if (result instanceof Promise) result.then(value => this[index] = value);
			else this[index] = result;
		}
		return this;
	};

	Array.prototype.asyncMap = function (callbackfn, thisArg) {
		return Promise.all(this.map(callbackfn, thisArg));
	};

	Array.prototype.interpose = function (...separators) {
		const getSeparators = separators.length === 1 && typeof separators[0] === "function" ? separators[0] as Function : undefined;
		return this.reduce((result, current, index) => (result.push(...[...index ? getSeparators ? wrapIfNotArray(getSeparators(index, current, this)) : separators : [], current]), result), []);
	};

	Array.prototype.nextItem = function (currentItem, offset = 1, defaultIndex = 0) {
		if (this.length === 0) return undefined; // Prevent divided by 0.
		const currentIndex = this.indexOf(currentItem);
		// If current item is not in the array.
		// Note that the default index may also exceed the index range of the array.
		// But do not use `Array.prototype.at()`. For example, if the array length is 4, and the default index:
		// input: -1, output: 3;
		// input: 4, output: 0; (← `Array.prototype.at()` won't support this)
		return this.circularAt(currentIndex === -1 ? defaultIndex : currentIndex + offset);
	};

	Array.prototype.shouldReversed = function (reverse = true) {
		return reverse ? this.toReversed() : this;
	};

	Array.prototype.trimEnd = function <T>(nullish?: T[] | ((value: T, index: number, obj: T[]) => unknown)) {
		for (let i = this.length - 1; i >= 0; i--) {
			const item = this[i];
			if (
				!nullish && (isUndefinedNullNaN(item) || typeof item === "string" && item.trim() === "") ||
				Array.isArray(nullish) && nullish.includes(item) ||
				typeof nullish === "function" && nullish(item, i, this)
			) this.pop();
			else break;
		}
	};

	Array.prototype.includesDeep = function (searchElement) {
		return this.indexOfDeep(searchElement) !== -1;
	};

	Array.prototype.toggleDeep = function (item, force) {
		const index = this.indexOfDeep(item);
		if (index === -1 || force)
			this.push(item);
		else
			this.removeAt(index);
	};

	Array.prototype.indexOfDeep = function (searchElement, fromIndex = 0) {
		return this.findIndex((element, index) => index >= fromIndex && lodash.isEqual(element, searchElement));
	};

	Array.prototype.circularAt = function (index) {
		return this.length === 0 ? undefined : this[floorMod(index, this.length)];
	};

	Array.prototype.split = function (delimiter) {
		return this.reduce((accumulator, currentValue) => {
			const test = typeof delimiter === "function" ? delimiter(currentValue) : currentValue === delimiter;
			if (test)
				accumulator.push([]); // Start a new sub-array.
			else {
				if (accumulator.length === 0) // Handle case where delimiter is the first element.
					accumulator.push([]);
				accumulator[accumulator.length - 1].push(currentValue); // Add element to the current sub-array.
			}
			return accumulator;
		}, []);
	};

	Array.prototype.firstDefined = function (predicate, check = "undefined", thisArg) {
		let result: Any;
		this.some((value, index, array) => {
			const currentResult = predicate.call(thisArg, value, index, array);
			let matched = false;
			switch (check) {
				case "undefined": matched = currentResult !== undefined; break;
				case "null": matched = currentResult !== null; break;
				case "nullish": matched = currentResult != null; break; // != checks both null and undefined
				case "false": matched = currentResult !== false; break;
				case "falsy": matched = !!currentResult; break; // truthy check
				default: break;
			}
			if (matched) result = currentResult;
			return matched;
		});
		return result;
	};

	Array.prototype.indexOfDefault = function (searchElement, fromIndex) {
		const index = this.indexOf(searchElement, fromIndex);
		return index !== -1 ? index : undefined;
	};

	Array.prototype.hole = function (start, end?: number) {
		return this.slice(0, start).concat(this.slice(end ?? start + 1));
	};

	Array.prototype.move = function (fromStart, fromEnd = fromStart + 1, toIndex = 0) {
		if (this.length === 0) return this;
		if (fromStart < 0) fromStart += this.length;
		if (fromEnd < 0) fromEnd += this.length;
		if (toIndex < 0) toIndex += this.length + 1;
		if (fromStart >= fromEnd || toIndex >= fromStart && toIndex <= fromEnd) return this;
		const movedItems = this.splice(fromStart, fromEnd - fromStart);
		const adjustedTarget = toIndex > fromStart ? toIndex - movedItems.length : toIndex;
		this.splice(adjustedTarget, 0, ...movedItems);
		return this;
	};

	Array.prototype.toMoved = function (fromStart, fromEnd, toIndex) {
		return this.slice().move(fromStart, fromEnd, toIndex);
	};

	Array.prototype.moveItem = function (fromItem, toIndex = 0) {
		const fromIndex = this.indexOf(fromItem);
		if (fromIndex === -1) return this;
		return this.move(fromIndex, undefined, toIndex);
	};

	makePrototypeKeysNonEnumerable(Array);
}

{ // Init set extensions
	Set.prototype.adds = function (...values) {
		for (const value of values)
			this.add(value);
		return this;
	};

	Set.prototype.deletes = function (...values) {
		let successes = 0;
		for (const value of values)
			if (this.delete(value))
				successes++;
		return successes;
	};

	Set.prototype.toggle = function (item) {
		if (!this.has(item))
			this.add(item);
		else
			this.delete(item);
	};

	Set.prototype.equals = function (other) {
		return this.symmetricDifference(other).size === 0;
	};

	makePrototypeKeysNonEnumerable(Set);
}

{ // Init map extensions
	Map.prototype.getOrInsertAsync = async function (key, asyncComputeFn) {
		if (this.has(key))
			return this.get(key);
		else {
			const value = await asyncComputeFn(key);
			this.set(key, value);
			return value;
		}
	};

	Map.prototype.map = function (callbackfn) {
		return Array.from(this, ([key, value], index) => callbackfn(key, value, index, this));
	};

	Map.prototype.getEntry = function (key) {
		if (!this.has(key)) return;
		return [key, this.get(key)] as const;
	};

	makePrototypeKeysNonEnumerable(Map);
}

{
	defineGetterInPrototype(Iterator, "length", function () {
		let length = 0, _item: unknown;
		for (_item of this)
			++length;
		return length;
	});

	Iterator.prototype.at = function (index) {
		if (index < 0) index = this.length + index;
		for (const item of this) {
			if (index === 0) return item;
			index--;
		}
		return undefined;
	};

	Iterator.prototype.entries = function* () {
		let i = 0;
		for (const element of this)
			yield [i++, element];
	};
}

{
	Uint8Array.prototype.toResized = function (newLength, returnNewInstanceIfLengthNotChanged = true) {
		if (newLength > this.length) {
			const newArray = new Uint8Array(newLength);
			newArray.set(this);
			return newArray;
		} else if (newLength < this.length)
			return this.slice(0, newLength);
		else
			return returnNewInstanceIfLengthNotChanged ? this.slice() : this;
	};

	Uint8Array.prototype.concat = function (...arrays) {
		return concatUint8Array(this, ...arrays);
	};

	makePrototypeKeysNonEnumerable(Uint8Array);
}

/**
 * Map to an object via a constant array.
 * @template T - The item type of the `constArray`, also the key type of the result object.
 * @template U - The value type of the result object.
 * @param constArray - **Constant** string array.
 * @param callbackFn - Generate key value tuples as objects.
 * @returns The mapped object.
 */
export function mapObjectConst<const T extends string, U>(constArray: readonly T[], callbackFn: (value: T, index: number, array: typeof constArray) => U) {
	return Object.fromEntries(constArray.map((value, index, array) => ([value, callbackFn(value, index, array)] as [T, U]))) as Record<T, U>;
}

/**
 * If the passed parameter is not an array, wrap it into an array that only one element,
 * otherwise return the array parameter itself.\
 * To ensure that the returned object is always an array.
 * @template T - Maybe an array, or something else.
 * @param maybeArray - Maybe an array, or something else.
 * @returns The original array or an array containing only one original parameter.
 * @example
 * ```typescript
 * wrapIfNotArray(["foo", "bar", "baz"]); // ["foo", "bar", "baz"]
 * wrapIfNotArray("foo"); // ["foo"]
 * ```
 */
export function wrapIfNotArray<T>(maybeArray: T): T extends Any[] ? T : [T] {
	return (Array.isArray(maybeArray) ? maybeArray : [maybeArray]) as never;
}

/**
 * Applies an asynchronous callback function to each value yielded by an AsyncGenerator,
 * collects the resulting promises, and returns a promise that resolves to an array of results.
 *
 * @template TIn - The type of values yielded by the input AsyncGenerator.
 * @template TOut - The type of values returned by the callback function.
 * @param asyncIter - The AsyncGenerator to iterate over.
 * @param callbackfn - A function that takes a value from the generator and returns a value or a promise of a value.
 * @returns A promise that resolves to an array of results from the callback function.
 */
export async function asyncIterMap<TIn, TOut>(asyncIter: AsyncGenerator<TIn>, callbackfn: (value: TIn) => MaybePromise<TOut>) {
	const promises = [];
	for await (const value of asyncIter)
		promises.push(callbackfn(value));
	return await Promise.all(promises);
}

/**
 * Concatenates multiple iterables or iterators into a single generator.
 *
 * @template TIterable - The type of elements in the iterables.
 * @template TIterator - The type of elements in the iterators.
 * @param iterators - A list of iterables to concatenate.
 * @yields {TIterable | TIterator} Elements from each iterable in the order they are provided.
 *
 * @example
 * ```typescript
 * const a = [1, 2];
 * const b = [3, 4];
 * for (const value of concatIter(a, b)) {
 *     console.log(value); // 1, 2, 3, 4
 * }
 * ```
 */
export const concatIter = function* <TIterable = never, TIterator = never>(...iterators: (Iterable<TIterable> | Iterator<TIterator>)[]) {
	for (const it of iterators)
		yield* it as Iterable<TIterable | TIterator>;
};

/**
 * Concatenates multiple Uint8Array buffers into a single Uint8Array.
 * @param arrays - Variable number of Uint8Array buffers to concatenate
 * @returns A new Uint8Array containing all input arrays concatenated in order
 * @example
 * ```javascript
 * const buffer1 = new Uint8Array([1, 2, 3]);
 * const buffer2 = new Uint8Array([4, 5, 6]);
 * const result = concatUint8Array(buffer1, buffer2); // Uint8Array [1, 2, 3, 4, 5, 6]
 * ```
 */
export function concatUint8Array(...arrays: Uint8Array[]) {
	const length = sum(...arrays.map(array => array.length));
	const stream = new Uint8Array(length);
	let offset = 0;
	for (const array of arrays) {
		stream.set(array, offset);
		offset += array.length;
	}
	return stream;
}

/**
 * Creates a new tuple that is correctly recognized by TypeScript.
 * @param args - Tuple arguments.
 * @returns A tuple.
 */
export const Tuple = <T extends Any[]>(...args: T): T => args;

/** Aren't you teaching me what to do? */
export const NEVER_MIND = [] as never;

/**
 * I don't know why the fucking `Array.isArray()` will not inverted (else branch) narrow the readonly array type.
 * @param test - The variable to be tested.
 * @returns I guess it might be a state property.
 */
export function isReadonlyArray(test: unknown): test is readonly Any[] {
	return Array.isArray(test);
}
