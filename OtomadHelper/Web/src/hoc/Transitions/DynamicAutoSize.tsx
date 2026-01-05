const DURATION = 350;

export default function DynamicAutoSize({ specified, skipTransition, children }: FCP<{
	/** Explicitly specify which direction needs to be animated. Defaults to height animation. */
	specified?: "width" | "height" | "both";
	// /** Temporarily lock the content size? */
	// lockSize?: boolean;
	/** Selectively skip transitions based on element predicates. */
	skipTransition?(el: HTMLElement): boolean;
}, "section">) {
	const el = useDomRef<"section">();
	const reduceMotion = useMediaQuery.reduceMotion();
	const size = useRef({ width: NaN, height: NaN });
	const animating = useRef(false);
	const pxify = (...numbers: number[]) => numbers.map(number => number + "px");
	const validateSize = (size: number) => Number.isFinite(size) && size !== 0; // Special requirements: The use case will not be met at the moment when the size is 0.

	useEffect(() => {
		if (!el.current || reduceMotion) return;
		const observer = new ResizeObserver(([{ contentRect: { width, height } }]) => {
			if (el.current && skipTransition?.(el.current)) {
				removeExistAnimations(el.current);
				return;
			}
			width = Math.round(width); height = Math.round(height);
			const { width: prevWidth, height: prevHeight } = size.current;
			const enableWidth = specified?.in("width", "both"), enableHeight = specified?.in("height", "both");
			size.current = { width, height };
			if (!animating.current && el.current && (
				prevWidth !== width && enableWidth && validateSize(prevWidth) && validateSize(width) ||
				prevHeight !== height && enableHeight && validateSize(prevHeight) && validateSize(height))) {
				animating.current = true;
				removeExistAnimations(el.current);
				// nextAnimationTick().then(() => {
				el.current.animate({
					...enableWidth && { width: pxify(prevWidth, width) },
					...enableHeight && { height: pxify(prevHeight, height) },
				}, { duration: DURATION, easing: eases.easeOutMax }).finished.then(() => {
					animating.current = false;
				});
				// });
			}
		});
		observer.observe(el.current);
		return () => observer.disconnect();
	}, [specified, reduceMotion]);

	return cloneRef(children, el);
}
