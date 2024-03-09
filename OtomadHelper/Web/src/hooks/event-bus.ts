import mitt from "mitt";

type ApplicationEvents = {
	// 在此处定义需要全局使用的事件。
	[host: `host:${string}` & {}]: AnyObject;
	"host:dragOver": WebMessageEvents.DragOver;
	"host:consoleLog": WebMessageEvents.ConsoleLog;
};

const emitter = mitt<ApplicationEvents>();

export const useEvent = emitter.emit;
export const useListen = emitter.on;
