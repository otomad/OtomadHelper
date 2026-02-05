using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.Helpers;

public static class QsiCodec {
	/// <inheritdoc cref="EncodeVarint(uint)" />
	public static byte[] EncodeVarint(int @int) => EncodeVarint((uint)@int);
	/// <summary>
	/// Encodes an integer to a variable-length quantity (varint) format.
	/// </summary>
	/// <param name="int">The integer to encode. Must be a non-negative integer (uint).</param>
	/// <returns>A byte array containing the encoded varint bytes.</returns>
	public static byte[] EncodeVarint(uint @int) {
		List<byte> bytes = [];
		do {
			// Take the lower 7 bits and set the highest mark.
			byte @byte = (byte)(@int & 0b0111_1111);
			@int = @int >> 7;
			// If not the last byte, set the highest bit to 1.
			if (@int > 0) @byte |= 0b1000_0000;
			bytes.Add(@byte);
		} while (@int > 0);
		return bytes.ToArray();
	}

	/// <summary>
	/// Decodes a variable-length encoded integer from a byte array.
	/// </summary>
	/// <param name="bytes">The byte array to decode from.</param>
	/// <param name="byteLength">Get the number of bytes consumed (or <see cref="int.MaxValue" /> if the varint is incomplete).</param>
	/// <returns>The decoded integer value.</returns>
	public static int DecodeVarint(byte[] bytes, out int byteLength) {
		int value = 0;
		foreach ((byte @byte, int i) in bytes.WithIndex()) {
			value |= (@byte & 0b0111_1111) << i * 7;
			if ((@byte & 0b1000_0000) == 0) {
				byteLength = i;
				return value;
			}
		}
		// Even if traverses to the last byte, the highest bit is still not 1. Returns the byte length with Infinity.
		byteLength = int.MaxValue;
		return value;
	}

	/// <summary>
	/// Encodes a bit array into a byte array.
	/// </summary>
	/// <param name="bits">A byte array containing only values of 0 and 1 to be encoded.</param>
	/// <returns>A byte array containing the encoded bytes.</returns>
	/// <remarks>
	/// The encoding process:
	/// <list type="number">
	/// <item>Prepends a 1 bit to the input array.</item>
	/// <item>Pads the array with leading zeros to make its length a multiple of 8.</item>
	/// <item>Groups the bits into sets of 8 to form bytes.</item>
	/// </list>
	/// </remarks>
	public static byte[] EncodeBitArray(ICollection<bool> bits) {
		int byteLength = Math.CeilDiv(bits.Count + 1, 8);
		bool[] padBits = new bool[byteLength * 8 - bits.Count];
		padBits[padBits.Length - 1] = true;

		byte[] bytes = new byte[byteLength];
		IEnumerable<bool> iterable = padBits.Concat(bits);
		foreach ((bool bit, int index) in iterable.WithIndex())
			if (bit) bytes[index >> 3] |= (byte)(1 << 7 - (index & 7));

		return bytes;
	}

	/// <summary>
	/// Decodes a bit array from a byte array.
	/// </summary>
	/// <param name="bytes">The byte array to decode.</param>
	/// <returns>A bool array, starting from the first true item found.</returns>
	public static bool[] DecodeBitArray(byte[] bytes) {
		bool[] bits = new bool[bytes.Length * 8];
		for (int index = 0; index < bits.Length; index++)
			bits[index] = (bytes[index >> 3] & 1 << 7 - (index & 7)) != 0;
		int beginFlagIndex = bits.IndexOf(true);
		return bits[(beginFlagIndex + 1)..];
	}

	/// <summary>
	/// QSI magic string header. 1 means version 1.
	/// </summary>
	public const string QSI_MAGIC_STRING = "QSI1:";

	public static string EncodeQsiProtocol(ICollection<bool> bits, int column = 0) =>
		QSI_MAGIC_STRING + Convert.ToBase64StringOmitPadding(EncodeVarint(column).Concat(EncodeBitArray(bits)).ToArray());

	public static string EncodeQsiProtocol(ICollection<ICollection<bool>> bits) =>
		EncodeQsiProtocol(bits.SelectMany(row => row).ToArray(), bits.First().Count());

	public static (bool[] bits, int column) DecodeQsiProtocol(string base64) {
		if (!base64.StartsWith(QSI_MAGIC_STRING))
			throw new ArgumentException("The provided base64 string does not comply with QSI communication protocol: " + base64);
		byte[] bytes = Convert.FromBase64StringOmitPadding(base64[QSI_MAGIC_STRING.Length..]);
		int column = DecodeVarint(bytes, out int intByteLength);
		if (intByteLength == int.MaxValue)
			throw new ArgumentException("The provided base64 string which varint of the column value is incomplete: " + base64);
		bytes = bytes[intByteLength..];
		bool[] bits = DecodeBitArray(bytes);
		return (bits, column);
	}

	public static bool[] DecodeQsiProtocol1D(string base64) => DecodeQsiProtocol(base64).bits;

	public static bool[,] DecodeQsiProtocol2D(string base64) {
		(bool[] bits, int column) = DecodeQsiProtocol(base64);
		bool[,] result = new bool[Math.CeilDiv(bits.Length, column), column];
		for (int i = 0; i < bits.Length; i++) {
			int r = i / column, c = i % column;
			result[r, c] = bits[i];
		}
		return result;
	}
}
