export function useDelayState<T>(initialState: T) {
	const [state, setStateInternal] = useState(initialState);
	const delayTimeoutId = useRef<Timeout>();
	const keepTimeoutId = useRef<Timeout>();

	type Options = Partial<{
		delay: number;
		keep: number;
	}>;
	const setState = async (value: React.SetStateAction<T>, options: Options = {}) => {
		if (keepTimeoutId.current) return;
		clearTimeout(delayTimeoutId.current);
		if (options.delay)
			await delay(options.delay, delayTimeoutId);
		delayTimeoutId.current = undefined;
		setStateInternal(value);
		if (options.keep)
			await delay(options.keep, keepTimeoutId);
		keepTimeoutId.current = undefined;
	};

	return [state, setState];
}
