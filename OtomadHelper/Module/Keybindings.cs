using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

public class Keybindings {
	internal CustomCommand Parent { get; } = new(Module.COMMAND_CATEGORY, "Otomad Helper - Keybindings");
	internal List<CustomCommand> Commands = [];

	public void Initialize() {
		AddKeybinding(nameof(SetSourceToTrackEvent), SetSourceToTrackEvent);
		AddKeybinding(nameof(SetSourceToProjectMedia), SetSourceToProjectMedia);
		AddKeybinding(nameof(EnableYtp), EnableYtp);
		AddKeybinding(nameof(DisableYtp), DisableYtp);
		AddKeybinding(nameof(StartGenerate), StartGenerate);
	}

	public event Action? SetSourceToTrackEvent;
	public event Action? SetSourceToProjectMedia;
	public event Action? EnableYtp;
	public event Action? DisableYtp;
	public event Action? StartGenerate;

	private void AddKeybinding(string name, Action? invoked) {
		CustomCommand command = new(Module.COMMAND_CATEGORY, name);
		command.MenuItemName = command.DisplayName + "!!!";
		command.Invoked += (sender, e) => invoked?.Invoke();
		Parent.AddChild(command);
		Commands.Add(command);
	}
}
