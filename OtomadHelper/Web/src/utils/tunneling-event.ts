type VoidCustomEvent<T = undefined> = globalThis.CustomEvent<T>;
type CustomEvents = FilterValueType<GlobalEventHandlersEventMap, CustomEvent>;
type CustomEventDetail<T extends keyof CustomEvents> = GlobalEventHandlersEventMap[T] extends CustomEvent<infer Detail> ? Detail : never;
/**
 * Creates a type-safe custom event with proper typing for event details.
 *
 * @remarks
 * This function provides the same functionality as the native `CustomEvent` constructor,
 * but with full TypeScript type safety for event types and their associated detail objects.
 *
 * @template T - The custom event type, constrained to keys of the `CustomEvents` interface.
 * @param type - The name of the custom event.
 * @param eventInitDict - The event initialization dictionary containing event options and the detail object.
 * @returns A new CustomEvent instance with proper type safety.
 *
 * @example
 * ```typescript
 * const event = createCustomEvent("myEvent", { detail: { foo: "bar" } });
 * ```
 */
export const createCustomEvent = <T extends keyof CustomEvents>(
	type: T,
	eventInitDict?: CustomEventInit<CustomEventDetail<T>>,
) => new CustomEvent(type, eventInitDict);

type TunnelingCustomEvent<T = undefined> = CustomEvent<{
	target: HTMLElement;
} & (T extends undefined ? {
	detail?: T;
} : {
	detail: T;
})>;
type TunnelingCustomEvents = FilterValueType<GlobalEventHandlersEventMap, TunnelingCustomEvent<Any>>;

function dispatchTunnelingEvent<T extends keyof TunnelingCustomEvents>(target: Element, type: T, eventInitDict?: TunnelingCustomEvents[T]["detail"]["detail"]) {
	// HTML/JS event cannot be implemented as a tunneling event which like it in WPF.
	// Although we have `capture` option in `addEventListener`, but it doesn't support custom events.
	// This function will dispatch the event with the target to the window,
	// and descendants can listen the events from window, and check if the target is their ancestor.
	// So that we can implement tunneling event like it in WPF now.
	const customEvent = createCustomEvent(type as never, { detail: { target, detail: eventInitDict } });
	window.dispatchEvent(customEvent);
}

function listenTunnelingEvent<T extends keyof TunnelingCustomEvents>(descendant: Element, type: T, listener: (detail: TunnelingCustomEvents[T] extends TunnelingCustomEvent<infer Detail> ? Detail : never, target: HTMLElement, originalEvent: WindowEventMap[T]) => void, { signal }: { signal?: AbortSignal } = {}) {
	window.addEventListener(type, e => {
		if (e.detail.target?.contains(descendant))
			listener(e.detail.detail as never, e.detail.target, e);
	}, { signal });
}

// NOTE: In theory, the removal of the listening function should also be implemented,
// but since it is not used, I will not implement it now.

export const TunnelingEvent = {
	/**
	 * Dispatches a custom tunneling event to the window object.
	 *
	 * @remarks
	 * This function simulates WPF-style tunneling events in the browser by dispatching
	 * custom events to the window object rather than directly to the target element.
	 * Descendant elements can listen to events from the window and check if the target
	 * is their ancestor, effectively implementing event tunneling.
	 *
	 * @template T - The type of tunneling event, constrained to keys of TunnelingCustomEvents.
	 * @param target - The element that is the source of the tunneling event.
	 * @param type - The type of the custom event to dispatch.
	 * @param eventInitDict - Optional event detail data to pass with the event.
	 *
	 * @example
	 * ```typescript
	 * TunnelingEvent.dispatch(element, "myCustomEvent", { value: "data" });
	 * ```
	 */
	dispatch: dispatchTunnelingEvent,
	/**
	 * Listens for tunneling custom events on the window and calls the listener when the event originates from a descendant element.
	 *
	 * @template T - The type of the tunneling custom event to listen for, must be a key of TunnelingCustomEvents.
	 * @param descendant - The element to check if the event target contains it.
	 * @param type - The event type to listen for.
	 * @param listener - Callback function invoked when a tunneling event is detected, receives the event detail, target element, and original window event.
	 * @param options - Optional configuration object.
	 * @param options.signal - Optional AbortSignal to control when the event listener should be removed.
	 */
	listen: listenTunnelingEvent,
};

declare global {
	// Add custom events
	interface GlobalEventHandlersEventMap {
		transitionExitCapture: TunnelingCustomEvent;
		customChange: VoidCustomEvent;
		animateSizeEnd: VoidCustomEvent;
	}
}
