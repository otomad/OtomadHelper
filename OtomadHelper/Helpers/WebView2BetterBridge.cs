using System.Text.Json.Serialization;
using Microsoft.Web.WebView2.WinForms;
using OtomadHelper.Models;

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
				if (parameters.Length != jsonArgs.Length) return false;
				matchedParameterLength = true;
				for (int i = 0; i < parameters.Length; i++) {
					ParameterInfo parameter = parameters[i];
					string jsonArg = jsonArgs[i].Trim();
					if (jsonArg == "null" || jsonArg.StartsWith("{")) continue; // null 和对象暂时无法直接判断正确类型。
					Type type = parameter.ParameterType;
					if (type == typeof(string) && jsonArg.StartsWith("\"")) continue;
					else if (type == typeof(bool) && jsonArg is "true" or "false") continue;
					else if (typeof(IEnumerable).IsAssignableFrom(type) && jsonArg.StartsWith("[")) continue;
					else if (type.IsNumber() && Regex.IsMatch(jsonArg, @"^[-.\d]")) continue;
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

			object?[] typedArgs = new object[jsonArgs.Length];

			for (int i = 0; i < typedArgs.Length; i++) {
				object? typedObj = JsonSerializer.Deserialize(jsonArgs[i], parameters[i].ParameterType, jsonOptions);
				typedArgs[i] = typedObj;
			}

			object resultTyped = method.Invoke(bridgeClass, typedArgs);

			// Was it an async method (in bridgeClass?)
			string resultJson;

			// Was the method called async?
			if (resultTyped is not Task resultTypedTask) { // Regular method:
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
}

public static class MessageSender {
	private static WebView2? webView2;

	public static WebView2 PostWebMessage_SetWebView2 { set => webView2 = value; }

	private static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	public static void PostWebMessage<T>(T message) where T : BaseWebMessageEvent {
		webView2?.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));
	}
}
