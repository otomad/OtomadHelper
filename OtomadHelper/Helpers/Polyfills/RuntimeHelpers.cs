// https://github.com/dotnet/runtime/blob/419e949d258ecee4c40a460fb09c66d974229623/src/libraries/System.Private.CoreLib/src/System/Index.cs
// https://github.com/dotnet/runtime/blob/419e949d258ecee4c40a460fb09c66d974229623/src/libraries/System.Private.CoreLib/src/System/Range.cs
// Implementation at https://www.meziantou.net/assets/range.zip
// This file polyfills System.Index and System.Range to .NET Standard 2.0 or .NET Framework.

namespace System.Runtime.CompilerServices;

internal static class RuntimeHelpers {
	/// <summary>
	/// Slices the specified array using the specified range.
	/// </summary>
	public static T[] GetSubArray<T>(T[] array, Range range) {
		if (array == null)
			throw new ArgumentNullException(nameof(array));

		(int offset, int length) = range.GetOffsetAndLength(array.Length);

		if (default(T) != null || typeof(T[]) == array.GetType()) {
			// We know the type of the array to be exactly T[].

			if (length == 0)
				return [];

			T[] dest = new T[length];
			Array.Copy(array, offset, dest, 0, length);
			return dest;
		} else {
			// The array is actually a U[] where U:T.
			T[] dest = (T[])Array.CreateInstance(array.GetType().GetElementType(), length);
			Array.Copy(array, offset, dest, 0, length);
			return dest;
		}
	}
}
