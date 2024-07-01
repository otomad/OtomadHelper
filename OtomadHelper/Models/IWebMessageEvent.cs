namespace OtomadHelper.Models;

public abstract class BaseWebMessageEvent {
	public string Type => GetTypeName(this);

	public DateTime Timestamp { get; internal set; } = DateTime.Now;

	private static string GetTypeName(object obj) => new VariableName(obj.GetType().Name).Camel;

	public static JsonObject Wrap(object obj!!, JsonSerializerOptions options, string? overriddenTypeName = null) {
		JsonNode node = JsonSerializer.SerializeToNode(obj, options)!;
		if (node.GetValueKind() != JsonValueKind.Object)
			node = new JsonObject() { ["value"] = node };
		JsonObject objectNode = node.AsObject();
		objectNode[nameof(Type).ToLowerInvariant()] =
			string.IsNullOrEmpty(overriddenTypeName) ? GetTypeName(obj) : overriddenTypeName;
		objectNode[nameof(Timestamp).ToLowerInvariant()] = DateTime.Now;
		return objectNode;
	}
}