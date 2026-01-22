/*
 * ## Encode **Varint** format example:
 * 314159
 * ↓ (Binary)
 * 0b100_1100_1011_0010_1111
 * ↓ (Group bits in sets of 7)
 * 0b10011_0010110_0101111
 * ↓ (Reverse groups)
 * 0101111, 0010110, 10011
 * ↓ (Pad start with 0 if less than 7 bits)
 * 0101111, 0010110, (00)10011
 * ↓ (Prepend 1 to each group, except for prepending 0 to the last group)
 * (1)0101111, (1)0010110, (0)0010011
 * ↓ (Done)
 * Uint8Array [0xAF, 0x96, 0x13]
 */

/**
 * Encodes an integer to a variable-length quantity (varint) format.
 *
 * @param int - The integer to encode. Must be a non-negative integer (number or bigint).
 * @returns A Uint8Array containing the encoded varint bytes.
 * @throws {RangeError} If the input is a decimal number.
 * @throws {RangeError} If the input is a negative integer.
 *
 * @example
 * ```typescript
 * const encoded = encodeVarint(300); // Uint8Array [0xAC, 0x02]
 * ```
 */
export function encodeVarint(int: number | bigint) {
	if (typeof int === "number" && !Number.isInteger(int)) throw new RangeError("Cannot encode a decimal number!");
	if (int < 0) throw new RangeError("Cannot encode a negative integer number!");

	const bytes: number[] = [];
	// Performance considerations: For large integers, bigint operation is appropriate;
	// For very small integers, bigint is not too slow, because the number of cycles is very small.
	int = BigInt(int);

	do {
		// Take the lower 7 bits and set the highest mark.
		let byte = Number(int & 0b0111_1111n);
		int = int >> 7n;
		// If not the last byte, set the highest bit to 1.
		if (int > 0n) byte |= 0b1000_0000;
		bytes.push(byte);
	} while (int > 0n);

	return new Uint8Array(bytes);
}

/**
 * Decodes a variable-length encoded integer from a byte array.
 *
 * @template TLiteral - The literal type for the return value type, either "number" or "bigint". Defaults to "number".
 * @template TType - The actual return type, inferred from TLiteral. Equals to number or bigint based on TLiteral.
 *
 * @param bytes - The byte array to decode from.
 * @param type - The type to return the decoded value as, either "number" or "bigint". Defaults to "number".
 * @param offset - The offset in the byte array to start decoding from. Defaults to 0.
 *
 * @returns A tuple containing:
 * - The decoded integer value of type TType (number or bigint).
 * - The number of bytes consumed (or Infinity if the varint is incomplete).
 *
 * @example
 * const bytes = new Uint8Array([0xE5, 0x8E, 0x26]);
 * const [value, length] = decodeVarint(bytes);
 * // value = 624485, length = 3
 */
export function decodeVarint<TLiteral extends "number" | "bigint" = "number", TType = TLiteral extends "bigint" ? bigint : number>(bytes: Uint8Array, type = "number" as TLiteral, offset: number = 0): [int: TType, byteLength: number] {
	if (offset) bytes = bytes.subarray(offset);
	// Get the correct value that matches the return type.
	const getNum = (bigint: bigint) => (type === "bigint" ? bigint : Number(bigint)) as TType;
	let value = 0n;
	// Let the iterator drop the offset.
	for (const [i, byte] of bytes.entries()) {
		value |= (BigInt(byte) & 0b0111_1111n) << BigInt(i) * 7n;
		if (!(byte & 0b1000_0000)) return [getNum(value), i + 1];
	}
	// Even if traverses to the last byte, the highest bit is still not 1. Returns the byte length with Infinity.
	return [getNum(value), Infinity];
}

/*
 * ## Encode **BitArray** example:
 * [0, 0, 1, 0, 1, 0, 1, 1, 1, 0]
 * ↓ (Prepend 1)
 * [1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0]
 * ↓ (Pad the array length to a multiple of 8)
 * [0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0]
 * ↓ (Group bits in sets of 8)
 * [0b0000_0100, 0b1010_1110]
 * ↓ (Done)
 * Uint8Array [0x04, 0xAE]
 */

/**
 * Encodes a bit array into a byte array.
 *
 * @param bits - A Uint8Array containing only values of 0 and 1 to be encoded
 * @returns A Uint8Array containing the encoded bytes
 *
 * @example
 * ```typescript
 * const bits = new Uint8Array([0, 0, 1, 0, 1, 0, 1, 1, 1, 0]);
 * const encoded = encodeBitArray(bits); // Uint8Array [0x04, 0xAE]
 * ```
 *
 * @remarks
 * The encoding process:
 * 1. Prepends a 1 bit to the input array.
 * 2. Pads the array with leading zeros to make its length a multiple of 8.
 * 3. Groups the bits into sets of 8 to form bytes.
 */
export function encodeBitArray(bits: Uint8Array) {
	// bitArray is a Uint8Array which contains only values of 0 and 1.

	const byteLength = Math.ceil((bits.length + 1) / 8);
	const padBits = new Uint8Array(byteLength * 8 - bits.length);
	padBits[padBits.length - 1] = 1;

	const bytes = new Uint8Array(byteLength);
	const iterable = concatIter(padBits, bits);
	for (const [index, bit] of iterable.entries())
		if (bit) bytes[index >> 3] |= 1 << 7 - (index & 7);

	return bytes;
}

/**
 * Decodes a bit array from a byte array.
 * @param bytes - The byte array to decode.
 * @param offset - The offset to start reading from. Defaults to 0.
 * @returns A Uint8Array where each element represents a single bit (0 or 1), starting from the first 1 bit found.
 */
export function decodeBitArray(bytes: Uint8Array, offset = 0) {
	if (offset) bytes = bytes.subarray(offset);
	const bits = new Uint8Array(bytes.length * 8);
	for (const index of bits.keys())
		bits[index] = +!!(bytes[index >> 3] & 1 << 7 - (index & 7));
	const beginFlagIndex = bits.indexOf(1);
	return bits.subarray(beginFlagIndex + 1);
}

// const encodeAsciiString = (text: string) => new TextEncoder().encode(text);
// const decodeAsciiString = (bytes: Uint8Array) => new TextDecoder().decode(bytes);

// QSI magic string header. 1 means version 1.
const QSI_MAGIC_STRING = "QSI1:";

export function encodeQsiProtocol(bits: Uint8Array, column: number = 0) {
	return QSI_MAGIC_STRING + concatUint8Array(encodeVarint(column), encodeBitArray(bits)).toBase64({ omitPadding: true });
}

export function decodeQsiProtocol(base64: string): [bits: Uint8Array, column: number] {
	if (!base64.startsWith(QSI_MAGIC_STRING))
		throw new TypeError("The provided base64 string does not comply with QSI communication protocol: " + base64);
	let bytes = Uint8Array.fromBase64(base64.slice(QSI_MAGIC_STRING.length));
	const [column, intByteLength] = decodeVarint(bytes);
	if (intByteLength === Infinity)
		throw new TypeError("The provided base64 string which varint of the column value is incomplete: " + base64);
	bytes = bytes.subarray(intByteLength);
	const bits = decodeBitArray(bytes);
	return [bits, column];
}
