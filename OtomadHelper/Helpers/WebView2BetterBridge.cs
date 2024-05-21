/// <summary>
/// <see href="https://github.com/johot/WebView2-better-bridge"/>
/// </summary>

using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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
	public static MainDock Host { get; internal set; } = null!;

	private static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	public static void PostWebMessage<T>(T message) where T : BaseWebMessageEvent {
		Host.Browser.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));
	}
}
