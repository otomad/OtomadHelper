/// <reference path="array.d.ts" />

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

	Array.prototype.randomOne = function (record) {
		if (!this.length) return null;
		record = toValue(record);
		let index = randBetween(0, this.length - 1);
		if (record !== undefined) {
			if (record.length !== this.length + 1 || record.every((n, i) => !i || n)) {
				let last = +record[0];
				if (!Number.isFinite(last)) last = -1;
				record.relist(Array(this.length + 1).fill(0));
				record[0] = last;
			}
			while (record[index + 1] || index === record[0])
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

	Array.prototype.toDeduplicated = function () {
		return [...new Set(this)];
	};

	Array.prototype.toRemoveFalsy = function () {
		return this.filter(item => item);
	};

	Array.prototype.equals = function (another) {
		if (this === another) return true;
		if (!this || !another) return false;
		if (this.length !== another.length) return false;

		for (let i = 0; i < this.length; i++)
			if (this[i] !== another[i])
				return false;
		return true;
	};

	Array.prototype.last = function () {
		return this.at(-1);
	};

	Array.prototype.deduplicate = function () {
		return this.relist(new Set(this));
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

	makePrototypeKeysNonEnumerable(Array);
}

/**
 * Map to an object via a constant array.
 * @note This JSDoc deliberately does not add "-" after the `@param` parameter, otherwise bugs will occur.
 * @param this **Constant** string array.
 * @param callbackFn - Generate key value tuples as objects.
 * @returns The mapped object.
 */
export function mapObjectConst<const T extends string, U>(array: T[], callbackFn: (value: T, index: number, array: T[]) => U) {
	return Object.fromEntries(array.map((value, index, array) => ([value, callbackFn(value, index, array)] as [T, U]))) as Record<T, U>;
}

// #region Tuples
/** Creates a new 1-tuple or singleton that is correctly recognized by TypeScript. */
export function Tuple<T1>(item1: T1): [T1];
/** Creates a new 2-tuple or pair that is correctly recognized by TypeScript. */
export function Tuple<T1, T2>(item1: T1, item2: T2): [T1, T2];
/** Creates a new 3-tuple or triple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3>(item1: T1, item2: T2, item3: T3): [T1, T2, T3];
/** Creates a new 4-tuple or quadruple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3, T4>(item1: T1, item2: T2, item3: T3, item4: T4): [T1, T2, T3, T4];
/** Creates a new 5-tuple or quintuple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3, T4, T5>(item1: T1, item2: T2, item3: T3, item4: T4, item5: T5): [T1, T2, T3, T4, T5];
/** Creates a new 6-tuple or sextuple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3, T4, T5, T6>(item1: T1, item2: T2, item3: T3, item4: T4, item5: T5, item6: T6): [T1, T2, T3, T4, T5, T6];
/** Creates a new 7-tuple or septuple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3, T4, T5, T6, T7>(item1: T1, item2: T2, item3: T3, item4: T4, item5: T5, item6: T6, item7: T7): [T1, T2, T3, T4, T5, T6, T7];
/** Creates a new 8-tuple or octuple that is correctly recognized by TypeScript. */
export function Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1: T1, item2: T2, item3: T3, item4: T4, item5: T5, item6: T6, item7: T7, item8: T8): [T1, T2, T3, T4, T5, T6, T7, T8];
export function Tuple(...args: unknown[]) {
	return [...args];
}
// #endregion
