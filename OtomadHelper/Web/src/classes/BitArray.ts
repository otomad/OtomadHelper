import { decodeQsiProtocol, encodeQsiProtocol } from "helpers/qsi-codec";

/**
 * A boolean array class that uses `Uint8Array` internally to store and provides TypedArray performance.
 *
 * Getting elements will return boolean type, and setting boolean values will automatically convert to 0 or 1.
 *
 * @template TSubclass - (Internal) Specify the subclass, which will used for the return type of the `map` and `toResized` methods.
 */
class BitArray<TSubclass = Any> {
	private readonly data!: Uint8Array;
	private readonly column: number = 0;
	get is2d() { return this.column !== 0; }
	get is1d() { return this.column === 0; }
	get length() { return this.is1d ? this.data.length : Math.ceil(this.data.length / this.column); }

	constructor(column: number, row: number);
	constructor(length: number);
	constructor(data: Uint8Array, column: number);
	constructor(elements: ArrayLike<boolean | number | bigint> | ArrayLike<ArrayLike<boolean | number | bigint>>);
	constructor() {
		const invalidArgumentsError = new TypeError(`Invalid BitArray arguments: ${Array.prototype.join.call(arguments, ", ")}`);
		if (arguments.length === 2 && typeof arguments[0] === "number" && typeof arguments[1] === "number") {
			this.data = new Uint8Array(arguments[0] * arguments[1]);
			this.column = arguments[0];
		} else if (arguments.length === 2 && arguments[0] instanceof Uint8Array && typeof arguments[1] === "number") {
			this.data = arguments[0];
			this.column = arguments[1];
		} else if (arguments.length === 1 && typeof arguments[0] === "number")
			this.data = new Uint8Array(arguments[0]);
		else if (arguments.length === 1 && arguments[0] != null && 0 in arguments[0] && "length" in arguments[0]) {
			const arrayLike = arguments[0];
			if (typeof arrayLike[0] === "boolean" || typeof arrayLike[0] === "number" || typeof arrayLike[0] === "bigint")
				this.data = new Uint8Array(arrayLike);
			else if (arrayLike[0] != null && "length" in arrayLike[0]) {
				this.column = arrayLike[0].length;
				this.data = new Uint8Array(Array.prototype.flat.call(arrayLike) as ArrayLike<number>);
			} else throw invalidArgumentsError;
		} else throw invalidArgumentsError;
		return BitArray.createProxy(this);
	}

	get [Symbol.toStringTag]() { return "BitArray"; }

	toString() {
		if (this.is1d) return this.data.join("");
		const result = Array(this.length);
		for (let i = 0; i < this.length; i++)
			result[i] = this.data.slice(i * this.column, (i + 1) * this.column).join("");
		return result.join("\n");
	}

	toJSON = this.toBase64;

	/**
	 * Changes all array elements from `start` to `end` index to a static `value` and returns the modified array
	 * @param value - Value to fill array section with.
	 * @param start - Index to start filling the array at. If start is negative, it is treated as length+start where
	 * length is the length of the array.
	 * @param end - Index to stop filling the array at. If end is negative, it is treated as length+end.
	 */
	fill(value: boolean, start?: number, end?: number) {
		this.data.fill(value ? 1 : 0, start, end);
	}

	/**
	 * Convert the `BooleanArray` to a normal boolean array.
	 * @returns `boolean[]`.
	 */
	toArray() {
		if (this.is1d) return Array.from(this.toString(), i => i !== "0");
		return this.toString().split("\n").map(row => Array.from(row.toString(), i => i !== "0"));
	}

	*[Symbol.iterator]() {
		for (const value of this.data)
			yield value !== 0;
	}

	private get(column: number, row?: number) {
		return this.data[(row ?? 0) * this.column + column] !== 0;
	}

	private set(value: boolean | number | bigint, column: number, row?: number) {
		return this.data[(row ?? 0) * this.column + column] = Number(value);
	}

	private hasIndex(column: number, row: number = 0) {
		if (!Number.isInteger(column) || !Number.isInteger(row) || column < 0 || row < 0) return false;
		return this.is2d ? column < this.column && row < this.length : column < this.data.length;
	}

	private getRow(row: number) {
		if (this.is1d) throw new TypeError("Not supported");
		return new BitArray(this.data.subarray(row * this.column, (row + 1) * this.column), 0);
	}

	private static createProxy<T>(bitArrayInstance: BitArray<T>) {
		return new Proxy(bitArrayInstance, {
			get(target, prop) {
				if (prop in target) return target[prop as keyof typeof target];
				const row = propToIndex(prop);
				if (target.hasIndex(row))
					if (target.is1d) return target.get(row);
					else return new Proxy(target.getRow(row), {
						get(_target, prop) {
							if (prop === "length") return target.column;
							const column = propToIndex(prop);
							if (target.hasIndex(column, row))
								return target.get(column, row);
						},
						set(_target, prop, value) {
							const column = propToIndex(prop);
							if (target.hasIndex(column, row)) {
								target.set(value as number, column, row);
								return true;
							}
							return false;
						},
						has: (_target, prop) => prop in target || target.hasIndex(propToIndex(prop), row),
						ownKeys: _target => getOwnKeys(target.column),
					});
			},
			set(target, prop, value) {
				const row = propToIndex(prop);
				if (target.is1d && target.hasIndex(row)) {
					target.set(value as number, row);
					return true;
				}
				return false;
			},
			has: (target, prop) => prop in target || target.hasIndex(propToIndex(prop)),
			ownKeys: target => getOwnKeys(target.length),
		});
	}

	private getColRow(index: number): TwoD {
		return this.is2d ? [index % this.column, Math.trunc(index / this.column)] : [index, 0];
	}

	forEach(callback: (value: boolean, column: number, row: number, array: BitArray) => void) {
		this.data.forEach((value, index) => callback(value !== 0, ...this.getColRow(index), this));
	}

	map(callback: (value: boolean, column: number, row: number, array: BitArray) => boolean) {
		const newData = this.data.map((value, index) => callback(value !== 0, ...this.getColRow(index), this) ? 1 : 0);
		return new BitArray(newData, this.column) as IsAny<TSubclass> extends true ? BitArray : TSubclass;
	}

	every(callback: (value: boolean, column: number, row: number, array: BitArray) => boolean) {
		return this.data.every((value, index) => callback(value !== 0, ...this.getColRow(index), this));
	}

	some(callback: (value: boolean, column: number, row: number, array: BitArray) => boolean) {
		return this.data.some((value, index) => callback(value !== 0, ...this.getColRow(index), this));
	}

	toResized(newLength?: number, newColumn?: number) {
		let { data, column } = this;
		if (newLength !== undefined)
			data = this.data.toResized(newLength);
		if (newColumn !== undefined)
			column = newColumn;
		return new BitArray(data === this.data ? data.slice() : data, column) as IsAny<TSubclass> extends true ? BitArray : TSubclass;
	}

	/**
	 * Get the internal `Uint8ClampedArray` (for performance critical operations).
	 * @returns `Uint8ClampedArray`.
	 */
	getRawData() { return this.data; }

	toBase64() {
		return encodeQsiProtocol(this.data, this.column);
	}

	static fromBase64(base64: string) {
		const [bits, column] = decodeQsiProtocol(base64);
		return new BitArray(bits, column);
	}
}

function propToIndex(prop: PropertyKey) {
	if (typeof prop === "symbol") return NaN;
	const index = Number(prop);
	return Number.isInteger(index) ? index : NaN;
}

function getOwnKeys(length: number) {
	const keys = Array.from({ length }, (_, i) => String(i));
	keys.push("length");
	return keys;
}

type Bit1DArray = BitArray<Bit1DArray> & WritableArrayLike<boolean>;
const Bit1DArray = BitArray as {
	new(length: number): Bit1DArray;
	new(array: ArrayLike<boolean> | Iterable<boolean> | ArrayLike<number> | Iterable<number>): Bit1DArray;
} & typeof BitArray<Bit1DArray>;

type Bit2DArray = BitArray<Bit2DArray> & ArrayLike<WritableArrayLike<boolean>>;
const Bit2DArray = BitArray as {
	new(column: number, row: number): Bit2DArray;
	new(array: ArrayLike<boolean> | Iterable<boolean> | ArrayLike<number> | Iterable<number>): Bit2DArray;
} & typeof BitArray<Bit2DArray>;

export function useBitArray<TDimension extends 1 | 2 = 1>(base64: StatePropertyNonNull<string>): StatePropertyNonNull<TDimension extends 1 ? Bit1DArray : TDimension extends 2 ? Bit2DArray : never> {
	return useStateSelector(
		base64,
		base64 => BitArray.fromBase64(base64),
		bitArray => bitArray.toBase64(),
		{ processPrevStateInSetterWithGetter: true },
	) as never;
}

export default Bit1DArray;
export { Bit1DArray as BitArray, Bit2DArray };

type IfAny<T, Y, N> = 0 extends (1 & T) ? Y : N;
type IsAny<T> = IfAny<T, true, false>;
type WritableArrayLike<T> = {
	readonly length: number;
	[n: number]: T;
};
