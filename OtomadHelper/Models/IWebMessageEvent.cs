namespace OtomadHelper.Models;

public abstract class BaseWebMessageEvent {
	public string Type {
		get {
			string type = GetType().Name;
			return char.ToLowerInvariant(type[0]) + type.Substring(1);
		}
	}
}
