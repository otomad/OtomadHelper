const keyNotFound = Symbol("serialize-keyed-map-and-set.key_not_found");
const getKeyMethod = Symbol("serialize-keyed-map-and-set.get_key_method"); // Cannot use private field directly.

/**
 * A Map subclass that uses deep equality (via `lodash.isEqual`) for key comparison
 * instead of the default reference equality. This allows objects with the same structure
 * and values to be considered equal as keys.
 *
 * @template K - The type of the keys in the map.
 * @template V - The type of the values in the map.
 *
 * @remarks
 * - All key-based operations (`get`, `has`, `delete`, `set`) use deep equality checks.
 * - This class is useful when you need to use complex objects as keys and want value-based equality semantics.
 *
 * @example
 * ```typescript
 * import SerializeKeyedMap from "./SerializeKeyedMap";
 * const map = new SerializeKeyedMap<object, string>();
 * map.set({ a: 1 }, "value");
 * console.log(map.get({ a: 1 })); // "value"
 * ```
 */
export class SerializeKeyedMap<K, V> extends Map<K, V> {
	private [getKeyMethod](key: K) {
		for (const [k] of this)
			if (lodash.isEqual(k, key))
				return k;
		return keyNotFound;
	}

	override get(key: K) {
		const k = this[getKeyMethod](key);
		return k === keyNotFound ? undefined : super.get(k);
	}

	override has(key: K) {
		const k = this[getKeyMethod](key);
		return k !== keyNotFound;
	}

	override delete(key: K) {
		const k = this[getKeyMethod](key);
		if (k === keyNotFound) return false;
		super.delete(k);
		return true;
	}

	override set(key: K, value: V) {
		const k = this[getKeyMethod](key);
		if (k !== keyNotFound) key = k;
		super.set(key, value);
		return this;
	}
}

/**
 * A Set subclass that uses deep equality (via `lodash.isEqual`) for value comparison.
 *
 * This class overrides the standard `has`, `add`, and `delete` methods to ensure
 * that elements are compared using deep equality rather than reference equality.
 *
 * @template T - The type of elements in the set.
 *
 * @remarks
 * - Uses `lodash.isEqual` to compare elements.
 * - Prevents duplicate elements based on deep equality.
 *
 * @example
 * ```typescript
 * const set = new SerializeKeyedSet<object>();
 * set.add({ a: 1 });
 * set.has({ a: 1 }); // true
 * set.delete({ a: 1 }); // true
 * ```
 */
export class SerializeKeyedSet<T> extends Set<T> {
	private [getKeyMethod](value: T) {
		for (const element of this)
			if (lodash.isEqual(element, value))
				return element;
		return keyNotFound;
	}

	override has(value: T) {
		const k = this[getKeyMethod](value);
		return k !== keyNotFound;
	}

	override add(value: T) {
		if (!this.has(value)) super.add(value);
		return this;
	}

	override delete(value: T) {
		const k = this[getKeyMethod](value);
		if (k === keyNotFound) return false;
		super.delete(k);
		return true;
	}

	override union<U>(other: ReadonlySetLike<U>) {
		return new SerializeKeyedSet(concatIter(this, other.keys()));
	}

	override intersection<U>(other: ReadonlySetLike<U>) {
		const copy = new SerializeKeyedSet(this) as SerializeKeyedSet<T & U>;
		for (const element of copy)
			if (!SerializeKeyedSet.prototype.has.call(other, element))
				Set.prototype.delete.call(copy, element);
		return copy;
	}

	override difference<U>(other: ReadonlySetLike<U>) {
		const copy = new SerializeKeyedSet(this);
		for (const element of copy)
			if (SerializeKeyedSet.prototype.has.call(other, element))
				Set.prototype.delete.call(copy, element);
		return copy;
	}

	override symmetricDifference<U>(other: ReadonlySetLike<U>) {
		return this.union(other).difference(this.intersection(other));
	}
}
