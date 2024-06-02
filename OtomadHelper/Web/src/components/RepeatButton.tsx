// const wrapKeyCode = (code: string, action: (e: Any) => void) => (e: KeyboardEvent) => e.code === code && action(e);

export default function RepeatButton({ children, onClick, onRelease, ...htmlAttrs }: FCP<{
	/** Mouse release button event. */
	onRelease?: BaseEventHandler;
}, "button">) {
	const repeatTimeout = useRef<Timeout>();
	const clearRepeatInterval = () => clearInterval(repeatTimeout.current);
	const [pressing, setPressing] = useState(false);

	const handleRelease = useCallback<MouseEventHandler & KeyboardEventHandler>(e => {
		clearRepeatInterval();
		setPressing(false);
		onRelease?.(e);
	}, [onRelease]);

	const handlePress = useCallback<MouseEventHandler<HTMLButtonElement>>(e => {
		onClick?.(e);
		clearRepeatInterval();
		setPressing(true);
		const startTime = Date.now();
		repeatTimeout.current = setInterval(() => {
			if (Date.now() - startTime > 350)
				onClick?.(e);
		}, 50);
	}, [onClick]);

	useEventListener(document, "mouseup", handleRelease as never);

	const [handleKeyDown, handleKeyUp] = useKeyDownOnce((e, isMouseDown) => {
		if (e.code === "Space") {
			e.preventDefault();
			((isMouseDown ? handlePress : handleRelease) as BaseEventHandler)(e);
		}
	});

	return (
		<button
			type="button"
			onMouseDown={handlePress}
			onMouseUp={handleRelease}
			onKeyDown={handleKeyDown}
			onKeyUp={handleKeyUp}
			{...(pressing ? { "data-pressing": "" } : {})}
			{...htmlAttrs}
		>
			{children}
		</button>
	); // TODO: data-pressing is not elegant.
}
