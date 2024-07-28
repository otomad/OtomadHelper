/**
 * A custom hook that returns a function that triggers a re-render of the component.
 *
 * This is useful when you need to force a re-render of a component without using the `useState` hook.
 *
 * @returns A function that triggers a re-render of the component.
 */
export function useForceUpdate() {
	return React.useReducer(() => ({}), {})[1] as () => void;
}

/**
 * A custom hook that controls the keyboard events for radio and checkbox elements.
 *
 * @param element - A reference to the HTML DOM element.
 * @param type - The type of the element, either "radio" or "checkbox".
 * @param handleCheck - A function that handles the change of the checkbox or radio button state.
 *
 * @returns A cleanup function that removes the event listeners when the component unmounts.
 */
export function useOnFormKeyDown(element: RefObject<HTMLElement>, type: "radio" | "checkbox", handleCheck: (checked?: boolean) => void) {
	useEffect(() => {
		const el = element.current;
		const CUSTOM_CHANGE_EVENT = "customchange";
		if (!el) return;

		const onKeydown = (e: KeyboardEvent) => {
			if (e.code === "Space") {
				stopEvent(e);
				return;
			}
			const movePrev = e.code === "ArrowUp" || e.code === "ArrowLeft";
			const moveNext = e.code === "ArrowDown" || e.code === "ArrowRight";
			if (!movePrev && !moveNext) return;
			stopEvent(e);
			let thisComponent = e.currentTarget as HTMLElement;
			let thatComponent: HTMLElement | null;
			while (true) {
				const _thatComponent = thisComponent[movePrev ? "previousElementSibling" : "nextElementSibling"];
				if (!(_thatComponent instanceof HTMLElement)) break;
				const input = _thatComponent.querySelector<HTMLInputElement>(`:scope > input[type=${type}]`);
				if (!input) break;
				thisComponent = _thatComponent;
				if (input.disabled) continue;
				thatComponent = _thatComponent;
				break;
			}
			thatComponent ??= thisComponent;
			thatComponent.focus();
			if (type === "radio") thatComponent.dispatchEvent(new CustomEvent(CUSTOM_CHANGE_EVENT));
		};

		const onKeyup = (e: KeyboardEvent) => {
			if (e.code === "Space") {
				stopEvent(e);
				handleCheck();
			}
		};

		const onCustomChange = () => handleCheck();

		el.addEventListener("keydown", onKeydown);
		el.addEventListener("keyup", onKeyup);
		el.addEventListener(CUSTOM_CHANGE_EVENT, onCustomChange);

		return () => {
			el.removeEventListener("keydown", onKeydown);
			el.removeEventListener("keyup", onKeyup);
			el.removeEventListener(CUSTOM_CHANGE_EVENT, onCustomChange);
		};
	}, []);
}

/**
 * Forward the ref from a local ref.
 * @param forwardedRef - Forwarded ref argument from the `forwardRef` function.
 * @param localRef - Local `useDomRef` variable.
 */
export function useImperativeHandleRef<T>(forwardedRef: React.ForwardedRef<T>, localRef: MutableRefObject<T | null | undefined>) {
	useImperativeHandle(forwardedRef, () => localRef.current!);
}

/**
 * If user click a button that inside another button, do not trigger outside button event.
 * @param handler - Mouse event handler.
 */
export function useOnNestedButtonClick(handler?: MouseEventHandler) {
	return useCallback<MouseEventHandler>(e => {
		const path = getPath(e);
		const currentTargetIndex = path.indexOf(e.currentTarget);
		if (currentTargetIndex !== -1) {
			if (path.slice(0, currentTargetIndex).find(element => element.tagName === "BUTTON")) return;
			if (path.slice(currentTargetIndex + 1).find(element => element.tagName === "BUTTON")) stopEvent(e);
		}
		handler?.(e);
	}, [handler]);
}
