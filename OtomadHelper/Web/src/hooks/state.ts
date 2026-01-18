type Options = Partial<{
	delay: number;
	keep: number;
	allowInterrupt: boolean;
}>;

export function useDelayState<T>(): [T | undefined, (value: React.SetStateAction<T | undefined>, options?: Options) => Promise<void>];
export function useDelayState<T>(initialState: T): [T, (value: React.SetStateAction<T>, options?: Options) => Promise<void>];
export function useDelayState<T>(initialState?: T) {
	const [state, setStateInternal] = useState(initialState);
	const delayTimeoutId = useRef<Timeout>(undefined);
	const keepTimeoutId = useRef<Timeout>(undefined);
	const setState = async (value: React.SetStateAction<T>, options: Options = {}) => {
		if (keepTimeoutId.current && !options.allowInterrupt) return;
		clearTimeout(delayTimeoutId.current);
		clearTimeout(keepTimeoutId.current);
		if (options.delay)
			await delay(options.delay, { ref: delayTimeoutId });
		delayTimeoutId.current = undefined;
		setStateInternal(value as T);
		if (options.keep)
			await delay(options.keep, { ref: keepTimeoutId });
		keepTimeoutId.current = undefined;
	};

	return [state, setState];
}

type StatePropertyTuple<Tuple extends [...Any[]]> = {
	[Index in keyof Tuple]: StatePropertyNonNull<Tuple[Index]>;
} & { length: Tuple["length"] };

/**
 * Creates an array of state hooks from initial values.
 * @template T - A tuple type extending an array of any values.
 * @param initialValues - The initial values for each state hook.
 * @returns A tuple of state hooks corresponding to the initial values.
 * @example
 * ```javascript
 * const [[name, setName], [age, setAge]] = useStateList("John", 30);
 * const [expanded1, expanded2, expanded3] = useStateList(true, true, true);
 * ```
 */
export function useStateList<T extends Any[]>(...initialValues: T) {
	return Array.from({ length: initialValues.length }, (_, i) => useState(initialValues[i])) as StatePropertyTuple<T>;
}
