// My original plan was to create a `BooleanArray` class that uses `Uint8ClampedArray` to store data internally,
// but due to the need to exchange data using JSON, I had to settle for the second best option.

// const padStartToEven = (hex: string) => hex.length % 2 ? "0" + hex : hex;

// export function uintToBase64(int: number | bigint) {
// 	if (!(Number.isInteger(int) || typeof int === "bigint")) throw new TypeError("The provided number is not an integer");
// 	else if (int < 0) throw new RangeError("The provided number must not be negative");
// 	return Uint8Array.fromHex(padStartToEven(int.toString(16))).toBase64({ omitPadding: true });
// }

// export function base64ToUint(base64: string, type?: "number"): number;
// export function base64ToUint(base64: string, type: "bigint"): bigint;
// export function base64ToUint(base64: string, type: "number" | "bigint" = "number") {
// 	const hex = Uint8Array.fromBase64(base64).toHex();
// 	if (type === "number") return parseInt(hex, 16);
// 	else return BigInt(`0x${hex}`);
// }

/**
 * Calculate the complement of `a` relative to `b` (i.e. the number that needs to be added to round to a multiple of `b`)
 * @param a - Dividend.
 * @param b - Divisor.
 * @returns The difference between `a` and the nearest multiple of `b`. If it is already a multiple of `b`, return 0.
 */
const padMod = (a: number, b: number) => b - (a % b || b);

export function bitArrayToBase64(bitArray: Uint8Array | Uint8ClampedArray | number[] | boolean[] | string) {
	if (typeof bitArray === "string") bitArray = Uint8Array.from(bitArray, char => +(char !== "0"));
	const byteArray = new Uint8Array(Math.ceil(bitArray.length / 8));
	bitArray.forEach((bit, index) => {
		if (bit) byteArray[index >> 3] |= 1 << 7 - (index & 7);
	});
	const paddingBits = padMod(bitArray.length, 8);
	return `${byteArray.toBase64({ omitPadding: true })}-${paddingBits}`;
}

export function base64ToBitArray(base64: string) {
	const [data, paddingBits_string] = base64.splitLastOnce("-");
	const paddingBits = +(paddingBits_string ?? 0);
	const byteArray = Uint8Array.fromBase64(data);
	const bitArray = new Uint8Array(byteArray.length * 8 - paddingBits);
	for (const index of bitArray.keys())
		bitArray[index] = +!!(byteArray[index >> 3] & 1 << 7 - (index & 7));
	return bitArray;
}

export function useBitArray(base64: StatePropertyNonNull<string>): StatePropertyNonNull<Uint8Array<ArrayBuffer>> {
	return useStateSelector(
		base64,
		base64 => base64ToBitArray(base64),
		bitArray => bitArrayToBase64(bitArray),
		{ processPrevStateInSetterWithGetter: true },
	);
}
