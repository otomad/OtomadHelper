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
	/// <param name="argsJson">The method arguments but in JSON.</param>
	/// <param name="callId">Call ID.</param>
	/// <returns></returns>
	public async Task<string> RunMethod(string methodName, string argsJson) {
		try {
			// We have stored each argument as json data in an array, the array is also encoded to a string
			// since webview can't invoke string[] array functions
			string[]? jsonDataArray = JsonSerializer.Deserialize<string[]>(argsJson, jsonOptions);
			if (jsonDataArray is null)
				throw new Exception("Invalid arguments");

			MethodInfo method = bridgeClassType.GetMethod(methodName);
			ParameterInfo[] parameters = bridgeClassType.GetMethod(methodName).GetParameters();

			if (parameters.Length != jsonDataArray.Length)
				throw new Exception($"Wrong number of arguments, expected: {parameters.Length} but got: {jsonDataArray.Length}");

			object?[] typedArgs = new object[jsonDataArray.Length];

			for (int i = 0; i < typedArgs.Length; i++) {
				object? typedObj = JsonSerializer.Deserialize(jsonDataArray[i], parameters[i].ParameterType, jsonOptions);
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

	public static WebView2 SetPostWebMessageWebView2 { set => webView2 = value; }

	private static readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		IncludeFields = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	public static void PostWebMessage<T>(T message) where T : BaseWebMessageEvent {
		webView2?.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));
	}
}
