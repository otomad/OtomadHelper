namespace OtomadHelper.Models;

internal class ConsoleLog(string message, string severity = "log") : BaseWebMessageEvent {
	public string Severity => severity;
	public string Message => message;
}
