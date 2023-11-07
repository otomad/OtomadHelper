import Point from "classes/Point";

/**
 * 根据一个数值、数组、点创建一个平滑的响应式引用变量。
 * @param current - 不平滑的当前值。
 * @param speed - 平滑速度。
 * @returns 平滑值响应式引用变量。
 * @see https://codepen.io/nanonansen/pen/oRWmaY 参考自视差平滑移动。
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
