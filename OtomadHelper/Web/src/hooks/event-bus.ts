import mitt from "mitt";

type ApplicationEvents = {
	// Define events that need to be used globally here.
	[host: `host:${string}` & {}]: AnyObject;
	"host:dragOver": WebMessageEvents.DragOver;
	"host:consoleLog": WebMessageEvents.ConsoleLog;
	"host:contextMenuItemClickEventArgs": WebMessageEvents.ContextMenuItemClickEventArgs;
	"dev:showContextMenu": [e: MouseEvent, menu: typeof window["contextMenu"] & {}];
};

const emitter = mitt<ApplicationEvents>();

/**
 * Emits an event to the global event emitter.
 *
 * @template TKey - The type of event key.
 * @param type - The event type.
 * @param event - The event data.
 *
 * @example
 * ```typescript
 * // fire an event
 * useEvent("foo", { a: "b" });
 * ```
 */
export const useEvent = emitter.emit;

/**
 * Listens to an event on the global event emitter.
 *
 * @template TKey - The type of event key.
 * @param type - The event type.
 * @param handler - The event handler function.
 *
 * @example
 * ```typescript
 * // listen to an event
 * useListen("foo", e => console.log("foo", e));
 *
 * // listen to all events
 * useListen("*", (type, e) => console.log(type, e));
 * ```
 */
export const useListen = emitter.on;

/**
 * Listens to an event on the global event emitter once.
 *
 * The handler function is called when the event is emitted, and then the event listener is removed.
 *
 * @template TKey - The type of event key.
 * @param type - The event type.
 * @param handler - The event handler function.
 *
 * @example
 * ```typescript
 * // listen to an event once
 * listenOnce("foo", e => console.log("foo", e));
 * ```
 */
export function listenOnce<TKey extends keyof ApplicationEvents>(type: TKey, handler: (event: ApplicationEvents[TKey]) => void) {
	const handlerWithOff: typeof handler = event => {
		emitter.off(type, handlerWithOff);
		handler(event);
	};
	emitter.on(type, handlerWithOff);
}

/**
 * Listens to an event on the global event emitter and returns an acknowledgement to the C# host immediately.
 *
 * The returned acknowledgement includes the original handler function result with a timestamp property.
 *
 * @template TKey - The type of event key.
 * @param type - The event type.
 * @param handler - The event handler function.
 */
export function useListenAndReturnAck<TKey extends keyof ApplicationEvents>(type: TKey, handler: (event: ApplicationEvents[TKey]) => Any) {
	useListen(type, e => {
		if (!("timestamp" in e)) {
			console.error(e);
			throw new TypeError("Received event is invalid, it has no timestamp property");
		}
		type DateTimeInJson = string; // C# System.Text.Json will convert DateTime to string.
		const timestamp = e.timestamp as DateTimeInJson;
		let result = handler(e);
		if (!(isObject(result) && !Array.isArray(result)))
			result = { value: result, timestamp: undefined! as DateTimeInJson };
		result.timestamp = timestamp;
		window.chrome.webview.hostObjects.webMessageAcknowledgement.Ack(JSON.stringify(result));
	});
}
