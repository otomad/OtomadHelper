type SmoothValueAcceptType = number | number[] | Point;
type SmoothValueChangeHandler<T extends SmoothValueAcceptType> = (current: T, previous: T) => void;
interface SmoothValueOptions<T extends SmoothValueAcceptType> {
	/** Reduce motion and disable smooth value. */
	disabled?: boolean;
	/** Occurs when the smooth value is changing. */
	onChange?: SmoothValueChangeHandler<T>;
	/** Occurs when the smooth value stops changing. */
	onStopChange?: SmoothValueChangeHandler<T>;
}

const FRACTION_DIGITS = 6; // Round to 6 decimal places.
const EPSILON = 10 ** -FRACTION_DIGITS; // 0.000001; // Number.EPSILON
const isValueNotChanged = (cur: number, prev: number) => Math.abs(cur - prev) < EPSILON;

/**
 * Create a smooth responsive reference variable based on a numerical value, array, or point.
 * @template T - Can be `number`, `number[]`, or `Point`.
 * @param current - Unsmooth current value.
 * @param spring - Smooth speed.
 * @param options - Other smooth value options.
 * @returns Smooth value state variable.
 * @throws {RangeError} If the `speed` is out of the range (0 ~ 1].
 * @see [Reference: Parallax smooth movement.](https://codepen.io/nanonansen/pen/oRWmaY)
 */
export function useSmoothValue<T extends SmoothValueAcceptType>(current: T, spring: number, options: SmoothValueOptions<T> = {}) {
	const reduceMotion = options.disabled || useMediaQuery.reduceMotion();
	if (spring <= 0 || spring > 1)
		throw new RangeError(`useSmoothValue speed parameter value range error. The parameter value must be within the range of (0 ~ 1], the current value is ${spring}.`);
	const animationId = useRef<number>(undefined);
	const prevTimestamp = useRef<DOMHighResTimeStamp>(undefined);
	const [smoothValue, _setSmoothValue] = useState(current);
	const setSmoothValue = setStateInterceptor(_setSmoothValue, undefined, (cur, prev) => {
		options.onChange?.(cur, prev);
		do {
			if (typeof cur === "number") {
				asserts<number>(prev);
				if (isValueNotChanged(cur, prev)) break;
			} else if (cur instanceof Point) {
				asserts<Point>(prev);
				if (isValueNotChanged(cur.x, prev.x) && isValueNotChanged(cur.y, prev.y)) break;
			} else {
				asserts<number[]>(prev);
				if (cur.length === prev.length && cur.every((c, i) => isValueNotChanged(c, prev[i]))) break;
			}
			return;
		} while (false);
		options.onStopChange?.(cur, prev);
		_setSmoothValue(current);
	});
	useEffect(() => {
		const animation = (timestamp?: DOMHighResTimeStamp) => {
			const value = current;
			const getNewValue = (cur: number, prev: number) => {
				if (!Number.isFinite(cur) || !Number.isFinite(prev)) return cur;
				const springByFps = getSpringByFps(timestamp, prevTimestamp, spring);
				return +(prev + (cur - prev) * springByFps).toFixed(FRACTION_DIGITS);
			};
			if (typeof value === "number")
				(setSmoothValue as SetStateNarrow<number>)(prev => getNewValue(value, prev));
			else if (value instanceof Point)
				(setSmoothValue as SetStateNarrow<Point>)(prev => new Point(
					getNewValue(value.x, prev.x),
					getNewValue(value.y, prev.y),
				));
			else
				(setSmoothValue as SetStateNarrow<number[]>)(prevs => prevs.map((prev, i) =>
					getNewValue(value[i], prev)));
			animationId.current = requestAnimationFrame(animation); // Note that `requestAnimationFrame` speed depends on your monitor FPS.
		};
		if (!reduceMotion) animation();
		return () => cancelAnimationFrame(animationId.current);
		// CAUTION: Do not add `setSmoothValue` inside `deps`, or will produce more bugs!!!
	}, [current, spring, reduceMotion, options.disabled]);
	if (reduceMotion) return current;
	// Do not put this line at the top of this function. Or when user is toggling the
	// prefers-reduced-motion option, it will crashed because of some useState won't be triggered.
	return smoothValue;
}

export function getSpringByFps(timestamp: DOMHighResTimeStamp | undefined, prevTimestamp: RefObject<DOMHighResTimeStamp | undefined>, spring: number) {
	let fps = 60;
	if (timestamp) {
		if (prevTimestamp.current) fps = Math.max(1000 / (timestamp - prevTimestamp.current), 1);
		prevTimestamp.current = timestamp;
	}
	const springByFps = spring / (fps / 60);
	if (springByFps <= 0 || springByFps > 1) return 0; // Avoid the value goes flying when stuck.
	return springByFps;
}
