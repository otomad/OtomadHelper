import Point from "classes/Point";

type SmoothValueAcceptType = number | number[] | Point;
type SmoothValueChangeHandler<T extends SmoothValueAcceptType> = (current: T, previous: T) => void;
interface SmoothValueOptions<T extends SmoothValueAcceptType> {
	onChange?: SmoothValueChangeHandler<T>;
	onStopChange?: SmoothValueChangeHandler<T>;
}

const EPSILON = 0.01; // Number.EPSILON
const isValueNotChanged = (cur: number, prev: number) => Math.abs(cur - prev) < EPSILON;
const DEFAULT_FPS = 60;

/**
 * Create a smooth responsive reference variable based on a numerical value, array, or point.
 * @param current - Unsmooth current value.
 * @param speed - Smooth speed.
 * @param options - Other smooth value options.
 * @returns Smooth value state variable.
 * @see https://codepen.io/nanonansen/pen/oRWmaY Reference: Parallax smooth movement.
 */
export function useSmoothValue<T extends SmoothValueAcceptType>(current: T, speed: number, options: SmoothValueOptions<T> = {}) {
	if (speed <= 0 || speed > 1)
		throw new RangeError(`useSmoothValue speed parameter value range error. The parameter value must be within the range of (0 ~ 1], the current value is ${speed}.`);
	const animationId = useRef<number>();
	const FRACTION_DIGITS = 6; // Round to 6 decimal places.
	const isStopChanging = useRef(true);
	const [smoothValue, _setSmoothValue] = useState(current);
	const setSmoothValue = setStateInterceptor(_setSmoothValue, undefined, (cur, prev) => {
		options.onChange?.(cur, prev);
		if (options.onStopChange) {
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
				isStopChanging.current = false;
			} while (false);
			if (!isStopChanging.current) {
				options.onStopChange?.(cur, prev);
				isStopChanging.current = true;
			}
		}
	});
	const fps = useMonitorFps();
	useEffect(() => {
		const animation = () => {
			const value = current;
			const getNewValue = (cur: number, prev: number) => +(prev + (cur - prev) * (speed / fps * DEFAULT_FPS)).toFixed(FRACTION_DIGITS);
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
		animation();
		return () => cancelAnimationFrame(animationId.current!);
	}, [current, speed]);
	return smoothValue;
}
