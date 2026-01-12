class _BooleanArrayArgumentInternalWrapper {
	constructor(public args: IArguments) { }
}

/**
 * A boolean array class that uses `Uint8ClampedArray` internally to store and provides TypedArray performance.
 *
 * Getting elements will return boolean type, and setting boolean values will automatically convert to 0 or 1.
 */
class BooleanArray {
	private readonly data!: Uint8ClampedArray;

	constructor(internal: _BooleanArrayArgumentInternalWrapper);
	constructor() {
		if (arguments[0] instanceof _BooleanArrayArgumentInternalWrapper)
			this.data = new Uint8ClampedArray(...arguments[0].args as unknown as [number]);
		else {
			if (typeof arguments[0] === "string") arguments[0] = Array.from(arguments[0]);
			const target = new BooleanArray(new _BooleanArrayArgumentInternalWrapper(arguments));
			const hasIndex = (index: string | number | symbol): index is number =>
				// eslint-disable-next-line no-restricted-globals
				index in target.data && index !== "" && typeof index !== "symbol" && isFinite(index as number);
			return new Proxy(target, {
				get(target, prop) {
					if (hasIndex(prop)) return target.data[prop] !== 0;
					return target[prop as keyof BooleanArray];
				},
				set(target, prop, value) {
					if (hasIndex(prop)) {
						target.data[prop] = value ? 1 : 0;
						return true;
					}
					return false;
				},
				has: (target, prop) => prop in target || hasIndex(prop),
				ownKeys: target => Reflect.ownKeys(target.data),
			});
		}
	}

	get length() { return this.data.length; }

	get [Symbol.toStringTag]() { return "BooleanArray"; }

	toString() { return this.data.join(""); }

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
		return Array.from(this.data, value => value !== 0);
	}

	*[Symbol.iterator]() {
		for (const value of this.data)
			yield value !== 0;
	}

	forEach(callback: (value: boolean, index: number, array: BooleanArray) => void) {
		this.data.forEach((value, index) => callback(value !== 0, index, this));
	}

	map(callback: (value: boolean, index: number, array: BooleanArray) => boolean) {
		return this.data.map((value, index) => callback(value !== 0, index, this) ? 1 : 0);
	}

	every(callback: (value: boolean, index: number, array: BooleanArray) => boolean) {
		return this.data.every((value, index) => callback(value !== 0, index, this));
	}

	some(callback: (value: boolean, index: number, array: BooleanArray) => boolean) {
		return this.data.some((value, index) => callback(value !== 0, index, this));
	}

	/**
	 * Get the internal `Uint8ClampedArray` (for performance critical operations).
	 * @returns `Uint8ClampedArray`.
	 */
	getRawData() {
		return this.data;
	}
}

export default BooleanArray as unknown as {
	new(length: number): BooleanArray & { [index: number]: boolean };
	new(array: ArrayLike<boolean> | Iterable<boolean> | ArrayLike<number> | Iterable<number>): BooleanArray & { [index: number]: boolean };
};

// /**
//  * 一个布尔数组类，内部使用 Uint8ClampedArray 存储，提供 TypedArray 的性能
//  * 读取元素会返回 boolean 类型，写入布尔值会自动转换为 0 或 1
//  */
// export class BooleanArray implements ArrayBuffer {
// 	private readonly data: Uint8ClampedArray;

// 	/**
// 	 * 创建一个新的 BooleanArray
// 	 * @param length - 数组长度
// 	 */
// 	constructor(length: number) {
// 		this.data = new Uint8ClampedArray(length);
// 	}

// 	/**
// 	 * 获取数组长度
// 	 */
// 	get length(): number {
// 		return this.data.length;
// 	}

// 	/**
// 	 * 获取指定索引处的布尔值
// 	 * @param index - 数组索引
// 	 * @returns boolean 值
// 	 */
// 	get(index: number): boolean {
// 		return this.data[index] !== 0;
// 	}

// 	/**
// 	 * 设置指定索引处的值
// 	 * @param index - 数组索引
// 	 * @param value - 要设置的布尔值
// 	 */
// 	set(index: number, value: boolean): void {
// 		this.data[index] = value ? 1 : 0;
// 	}

// 	/**
// 	 * 使用 Proxy 支持数组访问语法
// 	 */
// 	static create(length: number): BooleanArray & { [key: number]: boolean; } {
// 		const arr = new BooleanArray(length);
// 		return new Proxy(arr as any, {
// 			get(target, prop) {
// 				const index = Number(prop);
// 				if (!Number.isNaN(index) && index >= 0 && index < target.length)
// 					return target.get(index);
// 				return target[prop as keyof BooleanArray];
// 			},
// 			set(target, prop, value) {
// 				const index = Number(prop);
// 				if (!Number.isNaN(index) && index >= 0 && index < target.length) {
// 					target.set(index, Boolean(value));
// 					return true;
// 				}
// 				return false;
// 			},
// 		});
// 	}

// 	/**
// 	 * 填充数组元素
// 	 * @param value - 要填充的布尔值
// 	 * @param start - 起始索引
// 	 * @param end - 结束索引
// 	 */
// 	fill(value: boolean, start?: number, end?: number): void {
// 		this.data.fill(value ? 1 : 0, start, end);
// 	}

// 	/**
// 	 * 将数组转换为布尔数组
// 	 */
// 	toArray(): boolean[] {
// 		return Array.from(this.data, (v) => v !== 0);
// 	}

// 	/**
// 	 * 迭代数组元素
// 	 */
// 	*[Symbol.iterator](): IterableIterator<boolean> {
// 		for (let i = 0; i < this.data.length; i++)
// 			yield this.data[i] !== 0;
// 	}

// 	/**
// 	 * forEach 方法
// 	 */
// 	forEach(callback: (value: boolean, index: number, array: BooleanArray) => void): void {
// 		for (let i = 0; i < this.data.length; i++)
// 			callback(this.data[i] !== 0, i, this);
// 	}

// 	/**
// 	 * map 方法
// 	 */
// 	map<T>(callback: (value: boolean, index: number, array: BooleanArray) => T): T[] {
// 		const result: T[] = [];
// 		for (let i = 0; i < this.data.length; i++)
// 			result.push(callback(this.data[i] !== 0, i, this));
// 		return result;
// 	}

// 	/**
// 	 * every 方法
// 	 */
// 	every(callback: (value: boolean, index: number, array: BooleanArray) => boolean): boolean {
// 		for (let i = 0; i < this.data.length; i++)
// 			if (!callback(this.data[i] !== 0, i, this))
// 				return false;
// 		return true;
// 	}

// 	/**
// 	 * some 方法
// 	 */
// 	some(callback: (value: boolean, index: number, array: BooleanArray) => boolean): boolean {
// 		for (let i = 0; i < this.data.length; i++)
// 			if (callback(this.data[i] !== 0, i, this))
// 				return true;
// 		return false;
// 	}

// 	/**
// 	 * 获取内部的 Uint8ClampedArray（用于性能关键的操作）
// 	 */
// 	getRawData(): Uint8ClampedArray {
// 		return this.data;
// 	}
// }
