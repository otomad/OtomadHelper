import Point from "classes/Point";

/**
 * Create a smooth responsive reference variable based on a numerical value, array, or point.
 * @param current - Unsmooth current value.
 * @param speed - Smooth speed.
 * @returns Smooth value state variable.
 * @see https://codepen.io/nanonansen/pen/oRWmaY Reference: Parallax smooth movement.
 */
export function useSmoothValue<T extends number | number[] | Point>(current: T, speed: number) {
	if (speed <= 0 || speed > 1)
		throw new RangeError(`useSmoothValue speed parameter value range error. The parameter value must be within the range of (0 ~ 1], the current value is ${speed}.`);
	const [smoothValue, setSmoothValue] = useState(current);
	const animationId = useRef<number>();
	const FRACTION_DIGITS = 6; // 保留 6 位小数。
	useEffect(() => {
		const animation = () => {
			const value = current;
			const getNewValue = (cur: number, prev: number) => +(prev + (cur - prev) * speed).toFixed(FRACTION_DIGITS);
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
			animationId.current = requestAnimationFrame(animation);
		};
		animation();
		return () => cancelAnimationFrame(animationId.current!);
	}, [current, speed]);
	return smoothValue;
}
