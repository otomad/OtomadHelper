import mitt from "mitt";

type ApplicationEvents = {
	// Define events that need to be used globally here.
	[host: `host:${string}` & {}]: AnyObject;
	"host:dragOver": WebMessageEvents.DragOver;
	"host:consoleLog": WebMessageEvents.ConsoleLog;
};

const emitter = mitt<ApplicationEvents>();

export const useEvent = emitter.emit;
export const useListen = emitter.on;
