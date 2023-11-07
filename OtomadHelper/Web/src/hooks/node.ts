/**
 * 创建一个强制更新的函数。
 * @returns - 强制更新函数。
 */
export function useForceUpdate() {
	return React.useReducer(() => ({}), {})[1] as () => void;
}

/**
 * 控制在单选框和复选框中按下键盘按键的事件。
 * @param element - HTML DOM 元素。
 * @param type - 类型：单选框或复选框。
 * @param handleCheck - 改变勾选状态事件。
 */
export function useOnFormKeydown(element: RefObject<HTMLElement>, type: "radio" | "checkbox", handleCheck: (checked?: boolean) => void) {
	useEffect(() => {
		const el = element.current;
		const CUTSOM_CHANGE_EVENT = "customchange";
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
			if (type === "radio") thatComponent.dispatchEvent(new CustomEvent(CUTSOM_CHANGE_EVENT));
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
		el.addEventListener(CUTSOM_CHANGE_EVENT, onCustomChange);

		return () => {
			el.removeEventListener("keydown", onKeydown);
			el.removeEventListener("keyup", onKeyup);
			el.removeEventListener(CUTSOM_CHANGE_EVENT, onCustomChange);
		};
	}, [element]);
}
