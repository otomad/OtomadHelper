// const wrapKeyCode = (code: string, action: (e: Any) => void) => (e: KeyboardEvent) => e.code === code && action(e);

export default function RepeatButton({ children, onClick, onRelease, ...htmlAttrs }: FCP<{
	/** Pointer release button event. */
	onRelease?: BaseEventHandler;
}, "button">) {
	const repeatTimeout = useRef<Timeout>(undefined);
	const clearRepeatInterval = () => clearInterval(repeatTimeout.current);
	const [pressed, setPressed] = useState(false);

	const handleRelease = useCallback<PointerEventHandler & KeyboardEventHandler>(e => {
		clearRepeatInterval();
		setPressed(false);
		onRelease?.(e);
	}, [onRelease]);

	const handlePress = useCallback<PointerEventHandler<HTMLButtonElement>>(e => {
		if (e.nativeEvent instanceof MouseEvent && e.button !== 0) return;
		onClick?.(e);
		clearRepeatInterval();
		setPressed(true);
		const startTime = Date.now();
		repeatTimeout.current = setInterval(() => {
			if (Date.now() - startTime > 350)
				onClick?.(Object.assign(e, { repeat: true }));
		}, 50);
	}, [onClick]);

	useEventListener(document, "pointerup", handleRelease as never);

	const [handleKeyDown, handleKeyUp] = useKeyDownOnce((e, isPointerDown) => {
		if (e.code === "Space") {
			e.preventDefault();
			((isPointerDown ? handlePress : handleRelease) as BaseEventHandler)(e);
		}
	});

	return (
		<button
			type="button"
			onPointerDown={handlePress}
			onPointerUp={handleRelease}
			onKeyDown={handleKeyDown}
			onKeyUp={handleKeyUp}
			onContextMenu={mod.prevent()}
			data-pressed={pressed}
			{...htmlAttrs}
		>
			{children}
		</button>
	);
}
