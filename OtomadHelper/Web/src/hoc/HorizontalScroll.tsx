// const WILL_SCROLL_X_ATTR = "willScrollX";

export default function HorizontalScroll({ enabled = true, children }: FCP<{
	/** When user use mouse wheel to scroll, should it scroll horizontally instead of default vertically? */
	enabled?: boolean;
}, "div">) {
	// const willScrollX = useRef<number>();
	const [scrollX, setScrollX] = useState(0);
	const smoothScrollX = useSmoothValue(scrollX, 1);
	const _el = useDomRef<"section">(); // WARN: Wait for React 19 ref as a prop.
	
	const onWheel = useCallback<WheelEventHandler<HTMLElement>>(e => {
		const el = e.currentTarget;
		if (!el) return;
		_el.current = el;
		if (!enabled || el.scrollWidth <= el.clientWidth) return;
		e.preventDefault();
		setScrollX(scrollX => clamp(scrollX + (e.deltaX || e.deltaY), 0, el.scrollWidth - el.offsetWidth));
		
		/* notPrevent: do {
			prevent: do {
				if (!enabled ||
					e.deltaX !== 0 || e.deltaY === 0 || // Consider about touchpad horizontal scrolling.
					el.scrollWidth <= el.clientWidth
				) break notPrevent;
				const scrollX = +(el.dataset[WILL_SCROLL_X_ATTR] ?? el.scrollLeft);
				if (!Number.isFinite(scrollX)) break prevent;
				const arrived =
					e.deltaY < 0 && el.scrollLeft <= 0 ||
					e.deltaY > 0 && el.offsetWidth + el.scrollLeft >= el.scrollWidth; // Check if scroll to the end.
				if (arrived) break prevent;
				if (WILL_SCROLL_X_ATTR in el.dataset)
					el.scrollTo({ left: scrollX, behavior: "instant" });
				const willScrollX = scrollX + e.deltaY;
				el.dataset[WILL_SCROLL_X_ATTR] = String(willScrollX);
				el.scrollTo({ left: willScrollX, behavior: "smooth" });
				e.preventDefault();
				return;
			} while (false);
			e.preventDefault();
		} while (false);
		deleteWillScrollX(el); */
	}, [enabled]);
	
	useEffect(() => {
		if (!_el.current) return;
		_el.current.scrollLeft = smoothScrollX;
	}, [smoothScrollX]);
	
	// const onScrollEnd = useCallback<BaseEventHandler<HTMLElement>>(e => deleteWillScrollX(e.currentTarget), []);

	return (
		<EventInjector onWheel={onWheel}>
			{children as ReactElement}
		</EventInjector>
	);
}
