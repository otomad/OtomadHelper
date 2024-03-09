namespace OtomadHelper.Helpers;

public static partial class Extensions {
	public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection) =>
		collection.Select((item, index) => (item, index));
}
