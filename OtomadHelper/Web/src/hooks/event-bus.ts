import mitt from "mitt";

type ApplicationEvents = {
	// 在此处定义需要全局使用的事件。
	"tooltip:show": void; // TooltipEvent;
	"tooltip:hide": void; // HTMLElement;
	"tooltip:update": void; // TooltipEvent;
	"tooltip:refresh": void; // WeakMap<HTMLElement, { value: VTooltipBindingValue; symbol: symbol }>;
};

const emitter = mitt<ApplicationEvents>();

export const useEvent = emitter.emit;
export const useListen = emitter.on;
