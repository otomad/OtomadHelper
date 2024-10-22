namespace OtomadHelper.Models;

internal class DragOver : BaseWebMessageEvent {
	public string? extension;
	public string? contentType;
	public bool? isDirectory;
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public bool? isDragging = null;
}
