namespace OtomadHelper.Models;

public class DragOver : BaseWebMessageEvent {
	public string? extension;
	public string? contentType;
	public bool? isDirectory;
	public bool isDragging = false;
}
