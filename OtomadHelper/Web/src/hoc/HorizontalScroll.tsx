const WILL_SCROLL_X_ATTR = "willScrollX";

export default function HorizontalScroll({ enabled = true, children }: FCP<{
	/** When user use mouse wheel to scroll, should it scroll horizontally instead of default vertically? */
	enabled?: boolean;
}, "div">) {
	const deleteWillScrollX = (el?: HTMLElement): void => void delete el?.dataset[WILL_SCROLL_X_ATTR];
	
	const onWheel = useCallback<WheelEventHandler<HTMLElement>>(e => {
		const el = e.currentTarget;
		if (!el) return;
		notPrevent: do {
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
		deleteWillScrollX(el);
	}, [enabled]);
	
	const onScrollEnd = useCallback<BaseEventHandler<HTMLElement>>(e => deleteWillScrollX(e.currentTarget), []);

	return (
		<EventInjector onWheel={onWheel} onScrollEnd={onScrollEnd}>
			{children as ReactElement}
		</EventInjector>
	);
}
