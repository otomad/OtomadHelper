namespace OtomadHelper.Models;

public class ConsoleLog : BaseWebMessageEvent {
	public string severity;
	public string message;

	public ConsoleLog(string message, string severity = "log") {
		this.message = message;
		this.severity = severity;
	}
}
