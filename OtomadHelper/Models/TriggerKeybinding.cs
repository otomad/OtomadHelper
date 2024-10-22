using OtomadHelper.Module;

namespace OtomadHelper.Models;

public class TriggerKeybinding() : BaseWebMessageEvent {
	public VegasKeybindingEventType @event;
}
