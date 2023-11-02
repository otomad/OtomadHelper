/**
 * 获取某个值之前的值。
 * @param value - 值对象。
 * @returns 先前的值。
 */
export function usePrevious<T>(value: T): T | undefined {
	const ref = useRef<T>();

	useEffect(() => {
		ref.current = value;
	});

	return ref.current;
}
