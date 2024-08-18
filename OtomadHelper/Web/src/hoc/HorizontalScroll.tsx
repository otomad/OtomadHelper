/**
 * @see https://stackoverflow.com/a/70226036/19553213
 */
export default function HorizontalScroll({ enabled = true, children }: FCP<{
	/** When user use mouse wheel to scroll, should it scroll horizontally instead of default vertically? */
	enabled?: boolean;
}, "div">) {
	const _el = useDomRef<"section">(); // WARN: Wait for React 19 ref as a prop.
	/** Max `scrollLeft` value */
	const scrollWidth = useRef(0);
	/** Desired scroll distance per animation frame. You can adjust to your wish */
	const getScrollStep = () => scrollWidth.current / 50;
	/** Target value for `scrollLeft`. */
	const targetLeft = useRef(0);
	
	function scrollLeft() {
		const container = _el.current;
		if (!container) return;

		const beforeLeft = container.scrollLeft;
		const wantDeltaX = getScrollStep();
		const diff = targetLeft.current - container.scrollLeft;
		const deltaX = wantDeltaX >= Math.abs(diff) ? diff : Math.sign(diff) * wantDeltaX;

		// Performing horizontal scroll
		container.scrollBy({ left: deltaX, top: 0, behavior: "instant" });
		// Break if smaller `diff` instead of `wantDeltaX` was used
		if (deltaX === diff) return;
		// Break if can't scroll anymore or target reached
		if (beforeLeft === container.scrollLeft || container.scrollLeft === targetLeft.current) return;

		requestAnimationFrame(scrollLeft);
	}
	
	const onWheel = useCallback<WheelEventHandler<HTMLElement>>(e => {
		const el = e.currentTarget;
		if (!el) return;
		_el.current = el;
		if (!enabled || el.scrollWidth <= el.clientWidth) return;
		e.preventDefault();
		scrollWidth.current = el.scrollWidth - el.clientWidth;
		targetLeft.current = clamp(el.scrollLeft + (e.deltaX || e.deltaY), 0, scrollWidth.current);
		requestAnimationFrame(scrollLeft);
	}, [enabled]);

	return (
		<EventInjector onWheel={onWheel}>
			{children as ReactElement}
		</EventInjector>
	);
}
