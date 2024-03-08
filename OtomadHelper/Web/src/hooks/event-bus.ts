import mitt from "mitt";
import type Receives from "services/receives";

type ApplicationEvents = {
	// 在此处定义需要全局使用的事件。
	[host: `host:${string}` & {}]: AnyObject;
	"host:dragOver": Receives.DragOver;
};

const emitter = mitt<ApplicationEvents>();

export const useEvent = emitter.emit;
export const useListen = emitter.on;
