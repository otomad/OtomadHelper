namespace OtomadHelper.Models;

public class HTMLElementAttributes {
	public string tag = "";
	public string? type;
	public string[] @class = null!;
	public string id = "";
	public Dictionary<string, string> data = new();
}
