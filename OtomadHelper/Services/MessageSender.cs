using OtomadHelper.Models;
using OtomadHelper.Module;

namespace OtomadHelper.Services;

public static class MessageSender {
	public static Host Host { get; internal set; } = null!;

	private static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Converters = {
			new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
			new ColorJsonConverter(),
		},
	};

	private static readonly JsonSerializerOptions jsonOptionsForGeneralClasses = new(jsonOptions) { IncludeFields = false };

	/// <summary>
	/// Posts a web message from the <see cref="Microsoft.Web.WebView2.Core.CoreWebView2"/>.
	/// </summary>
	/// <remarks>
	/// <list type="bullet">
	/// <item>This method is used to send a <paramref name="message"/> to the JavaScript code running in the WebView2 control.</item>
	/// <item>The message must be a subclass of <see cref="BaseWebMessageEvent"/>.</item>
	/// <item>The message is serialized to JSON using the jsonOptions before being posted.</item>
	/// </list>
	/// </remarks>
	/// <typeparam name="T">The type of the <paramref name="message"/>.
	/// Must be a subclass of <see cref="BaseWebMessageEvent"/>.</typeparam>
	/// <param name="message">The message to be posted.</param>
	public static void PostWebMessage<T>(T message) where T : BaseWebMessageEvent =>
		Host.Browser.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));

	private static void PostWebMessageFromJsonObject(JsonObject jsonObject) =>
		Host.Browser.CoreWebView2.PostWebMessageAsJson(jsonObject.ToJsonString(jsonOptions));

	private static readonly Dictionary<DateTime, TaskCompletionSource<JsonElement>> taskList = [];
	/// <summary>
	/// Asynchronously posts a web message and gets the result.
	/// </summary>
	/// <typeparam name="TReceive">The type of the result to be received.</typeparam>
	/// <param name="message">The message to be posted.</param>
	/// <param name="overriddenTypeName">An optional parameter to override the type name of the message class.</param>
	/// <returns>The deserialized result of the posted message.</returns>
	public static async Task<TReceive> PostWebMessageAndGetResult<TReceive>(object message, string? overriddenTypeName = null) {
		if (message is null) throw new ArgumentNullException(nameof(message));
		TaskCompletionSource<JsonElement> taskCompletionSource = new();
		void AddToTaskList(DateTime timestamp) => taskList.Add(timestamp, taskCompletionSource);
		if (message is BaseWebMessageEvent e) {
			DateTime timestamp = e.Timestamp;
			AddToTaskList(timestamp);
			PostWebMessage(e);
		} else {
			JsonObject jsonObject = BaseWebMessageEvent.Wrap(message, jsonOptionsForGeneralClasses, overriddenTypeName);
			DateTime timestamp = jsonObject[nameof(timestamp)]!.GetValue<DateTime>();
			AddToTaskList(timestamp);
			PostWebMessageFromJsonObject(jsonObject);
		}
		JsonElement json = await taskCompletionSource.Task;
		return JsonSerializer.Deserialize<TReceive>(json, jsonOptions)!;
	}

	internal static void OnReceiveAcknowledgement(string json) {
		using JsonDocument document = JsonDocument.Parse(json);
		JsonElement value = document.RootElement;
		if (!value.TryGetProperty("timestamp", out JsonElement timestampElement)) return;
		if (!timestampElement.TryGetDateTime(out DateTime timestamp)) return;
		if (!taskList.TryGetValue(timestamp, out TaskCompletionSource<JsonElement> taskCompletionSource)) return;
		if (value.TryGetProperty("value", out JsonElement nonObjectValue) &&
			value.EnumerateObject().Count() == 2)
			value = nonObjectValue;
		taskCompletionSource.SetResult(value);
		taskList.Remove(timestamp);
	}

	internal static TReceive ParseJson<TReceive>(string json) =>
		JsonSerializer.Deserialize<TReceive>(json, jsonOptions)!;
}
