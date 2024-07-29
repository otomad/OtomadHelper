export default function HorizontalScroll({ enabled = true, children }: FCP<{
	/** When user use mouse wheel to scroll, should it scroll horizontally instead of default vertically? */
	enabled?: boolean;
}, "div">) {
	const onWheel = useCallback<WheelEventHandler>(e => {
		if (!enabled || e.deltaX !== 0 && e.deltaY === 0) return; // Consider about touchpad horizontal scrolling.
		const el = e.currentTarget as HTMLDivElement;
		if (!el || el.scrollWidth <= el.clientWidth) return;
		el.scrollLeft += e.deltaY;
		e.preventDefault();
	}, []);

	return (
		<EventInjector onWheel={onWheel}>
			{children as ReactElement}
		</EventInjector>
	);
}
