/// <summary>
/// <see href="https://github.com/johot/WebView2-better-bridge"/>
/// </summary>

using System.Runtime.CompilerServices;

using OtomadHelper.Models;
using OtomadHelper.Module;

namespace OtomadHelper.Helpers.WebView2BetterBridge;

/// <summary>
/// Subscribers to messages from TS/JS and invokes methods / parses JSON etc for a wrapping bridge class.
/// Giving us the ability to use any arguments, use async methods pass complex objects etc.
/// </summary>
public class BetterBridge {
	// Will invoke methods on this object
	private readonly object bridgeClass;
	private readonly Type bridgeClassType;

	public BetterBridge(object bridgeClass) {
		this.bridgeClass = bridgeClass;
		bridgeClassType = this.bridgeClass.GetType();
	}

	internal static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		// VEGAS environment (?) doesn't support JSON converters.
	};

	public string[] GetMethods() =>
		bridgeClassType.GetMethods().Select(m => m.Name).ToArray();

	/// <summary>
	/// Called from TS/JS side works on both async and regular methods of the wrapped class!
	/// </summary>
	/// <param name="methodName">The method name.</param>
	/// <param name="jsonArgsString">The method arguments but in JSON.</param>
	/// <param name="callId">Call ID.</param>
	/// <returns></returns>
	public async Task<string> RunMethod(string methodName, string jsonArgsString) {
		try {
			// We have stored each argument as json data in an array, the array is also encoded to a string
			// since webview can't invoke string[] array functions
			string[]? jsonArgs = JsonSerializer.Deserialize<string[]>(jsonArgsString, jsonOptions);
			if (jsonArgs is null)
				throw new TypeLoadException("Invalid arguments");

			bool matchedMethodName = false, matchedParameterLength = false;
			MethodInfo[] methods = bridgeClassType.GetMethods();
			MethodInfo? method = methods.FirstOrDefault(method => {
				if (method.Name != methodName) return false;
				matchedMethodName = true;
				ParameterInfo[] parameters = method.GetParameters();
				int parametersLength = parameters.Length;
				if (parametersLength > jsonArgs.Length) // If the method contains optional parameters
					for (int i = parametersLength - 1; i >= 0 && parametersLength > jsonArgs.Length; i--)
						if (parameters[i].HasDefaultValue)
							parametersLength--;
				if (parametersLength != jsonArgs.Length) return false;
				matchedParameterLength = true;
				for (int i = 0; i < parametersLength; i++) {
					ParameterInfo parameter = parameters[i];
					string jsonArg = jsonArgs[i].Trim();
					if (jsonArg == "null" || jsonArg.StartsWith("{")) continue;
					// Nulls and objects are temporarily unable to directly determine the correct type.
					Type type = parameter.ParameterType;
					if (type == typeof(string) && jsonArg.StartsWith("\"")) continue;
					else if (type == typeof(bool) && jsonArg is "true" or "false") continue;
					else if ((type.Extends(typeof(IEnumerable)) || type.Extends(typeof(ITuple))) &&
						jsonArg.StartsWith("[")) continue;
					else if (type.IsNumber() && jsonArg.IsMatch(new(@"^[-.\d]"))) continue;
					else return false;
				}
				return true;
			});

			if (method is null) {
				if (!matchedMethodName)
					throw new MissingMethodException(bridgeClassType.Name, methodName);
				else if (!matchedParameterLength)
					throw new ArgumentException($"No overload for method \"{methodName}\" takes {jsonArgs.Length} arguments");
				else
					throw new ArgumentException($"Method \"{methodName}\" contains overloads of {jsonArgs.Length} parameters, but the types are not matched");
			}

			ParameterInfo[] parameters = method.GetParameters();

			object?[] typedArgs = Enumerable.Repeat(Type.Missing, method.GetParameters().Length).ToArray();

			for (int i = 0; i < jsonArgs.Length; i++) {
				Type type = parameters[i].ParameterType;
				object? typedObj;
				if (type.Extends(typeof(ITuple)))
					typedObj = JsonDeserializeTuple(jsonArgs[i], type);
				else
					typedObj = JsonSerializer.Deserialize(jsonArgs[i], type, jsonOptions);
				typedArgs[i] = typedObj;
			}

			object resultTyped = method.Invoke(bridgeClass, typedArgs);

			// Was it an async method (in bridgeClass?)
			string resultJson;

			// Was the method called async?
			if (resultTyped is not Task resultTypedTask) { // Regular method
				// Package the result
				resultJson = JsonSerializer.Serialize(resultTyped, jsonOptions);
			} else { // Async method:
				await resultTypedTask;
				// If has a "Result" property return the value otherwise null (Task<void> etc)
				PropertyInfo resultProperty = resultTypedTask.GetType().GetProperty("Result");
				object? taskResult = resultProperty?.GetValue(resultTypedTask);

				// Package the result
				resultJson = JsonSerializer.Serialize(taskResult, jsonOptions);
			}

			return resultJson;
		} catch (Exception e) {
			PostWebMessage(new ConsoleLog(e.ToString(), "error"));
			return "null";
		}
	}

	public static ITuple JsonDeserializeTuple(string arrayJson, Type tupleType) {
		// VEGAS environment (?) doesn't support JSON converters.
		// So we can't use some like: <a href="https://github.com/arogozine/TupleAsJsonArray">TupleAsJsonArray</a>.
		JsonNode? node = JsonNode.Parse(arrayJson);
		if (node?.GetValueKind() != JsonValueKind.Array)
			throw new ArrayTypeMismatchException("The specified JSON arg is not the array type");
		JsonArray array = node.AsArray();
		Type[] genericTupleArgs = tupleType.GetGenericArguments();
		object[] arguments = new object[genericTupleArgs.Length];
		for (int i = 0; i < genericTupleArgs.Length; i++) {
			Type type = genericTupleArgs[i];
			arguments[i] = JsonSerializer.Deserialize(array[i], type, jsonOptions)!;
		}
		return arguments.ToTuple(tupleType);
	}
}

public static class MessageSender {
	public static MainDock MainDock { get; internal set; } = null!;

	private static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
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
		MainDock.Browser.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));

	private static void PostWebMessageFromJsonObject(JsonObject jsonObject) =>
		MainDock.Browser.CoreWebView2.PostWebMessageAsJson(jsonObject.ToJsonString(jsonOptions));

	private static readonly Dictionary<DateTime, TaskCompletionSource<JsonElement>> taskList = new();
	/// <summary>
	/// Asynchronously posts a web message and gets the result.
	/// </summary>
	/// <typeparam name="TReceive">The type of the result to be received.</typeparam>
	/// <param name="message">The message to be posted.</param>
	/// <param name="overriddenTypeName">An optional parameter to override the type name of the message class.</param>
	/// <returns>The deserialized result of the posted message.</returns>
	public static async Task<TReceive> PostWebMessageAndGetResult<TReceive>(object message!!, string? overriddenTypeName = null) {
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
}
