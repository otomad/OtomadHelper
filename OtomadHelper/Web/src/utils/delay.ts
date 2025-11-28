// cSpell:ignore isb

/**
 * That's right! it's the famous **delay** function.\
 * This will execute asynchronously and will not block the thread.
 *
 * @param ms - Milliseconds.
 * @returns Empty promise.
 */
export function delay(ms: number, { ref, signal }: {
	/** Get the timeout ID and assign to a React ref object. */
	ref?: RefObject<Timeout | undefined | null>;
	/** Get the abort controller signal that to cancel a delay. */
	signal?: AbortSignal;
} = {}): Promise<void> {
	return new Promise(resolve => {
		if (signal?.aborted) resolve();
		const timeoutId = setTimeout(resolve, ms);
		if (ref) ref.current = timeoutId;
		signal?.addEventListener("abort", () => {
			clearTimeout(timeoutId);
			resolve();
		});
	});
}

const isb = new Int32Array(typeof SharedArrayBuffer !== "undefined" ? new SharedArrayBuffer(4) : 1 as unknown as ArrayBufferLike);
/**
 * Sleep function.\
 * This will block the thread.
 *
 * @param ms - Milliseconds.
 */
export function sleep(ms: number) {
	Atomics.wait(isb, 0, 0, ms);
}

/**
 * If you played with *React Hooks* for more than a few hours, you probably ran into an intriguing problem:
 * using `setInterval` just *doesn't work* as you'd expect.
 *
 * In the words of Ryan Florence:
 *
 * > I've had a lot of people point to setInterval with hooks as some sort of egg on React's face
 *
 * Honestly, I think these people have a point. It is confusing at first.
 *
 * But I've also come to see it not as a flaw of Hooks but as a mismatch between the React programming model and setInterval.
 * Hooks, being closer to the React programming model than classes, make that mismatch more prominent.
 *
 * There is a way to get them working together very well but it's a bit unintuitive.
 *
 * @see https://overreacted.io/making-setinterval-declarative-with-react-hooks/
 *
 * @param callback - The function to call when the timer elapses.
 * @param delay - The number of milliseconds to wait before calling the `callback`.
 */
export function useInterval(callback: () => void, delay: number) {
	const savedCallback = useRef<() => void>(undefined);

	// Remember the latest callback.
	useEffect(() => {
		savedCallback.current = callback;
	}, [callback]);

	// Set up the interval.
	useEffect(() => {
		function tick() {
			savedCallback.current?.();
		}
		if (delay != null) {
			const intervalId = setInterval(tick, delay);
			return () => clearInterval(intervalId);
		}
	}, [delay]);
}

/**
 * Hook that returns a boolean flag which is true briefly after the provided `state` value changes.
 *
 * The flag is set to `true` immediately when `state` changes and will automatically switch back to `false` after the
 * specified `duration` (in milliseconds). A pending timeout is cleared whenever `state` changes again or when the
 * component using the hook unmounts, ensuring only the latest timeout controls the flag.
 *
 * Notes:
 * - The hook treats changes to `state` according to React's dependency array semantics (referential equality). If
 * `state` is an object that is mutated in-place without changing its reference, the hook will not detect the change.
 * - The default `duration` is 250 ms.
 *
 * @param states - The value to watch for changes. Any change (per React's dependency comparison) will trigger the
 * "changing" flag.
 * @param duration - Optional duration in milliseconds to keep the "changing" flag true after a change.
 * Defaults to 250 ms.
 * @returns A boolean which is `true` immediately after `state` changes and becomes `false` after the specified
 * `duration`.
 *
 * @example
 * // Show an animation indicator for 300ms whenever `value` changes.
 * const isChanging = useChanging(value, 300);
 */
export function useChanging(states: unknown[], duration: number = 250) {
	const [changing, setChanging] = useState(false);
	const timeoutId = useRef<Timeout>(undefined);

	useUpdateEffect(() => {
		clearTimeout(timeoutId.current);
		setChanging(true);
		timeoutId.current = setTimeout(() => setChanging(false), duration);
		return () => clearTimeout(timeoutId.current);
	}, states);

	return changing;
}

/**
 * Schedule a callback to run a limited number of times using setInterval.
 *
 * @remarks
 * The provided `callback` will be called with a zero-based execution index representing the invocation's
 * position (the value passed to the callback is the current execution count before it is incremented).
 * If `immediate` is true, the callback is invoked once synchronously before scheduling the interval.
 * If `repeatTimes` is finite, the interval is automatically cleared once the total number of executions
 * reaches `repeatTimes`.
 *
 * @param callback - Function invoked on each execution with the current zero‑based execution index for that call.
 * @param delay - Interval delay in milliseconds between invocations. Optional; passed directly to `setInterval`.
 * @param repeatTimes - Total number of times the callback should be executed. Defaults to `Infinity` (no limit).
 * @param immediate - If true (default), invoke the callback immediately once before scheduling subsequent executions.
 * @returns The identifier returned by `setInterval`, or `0` if no interval was scheduled because the repeat limit
 * had already been reached.
 *
 * @example
 * ```javascript
 * // Call immediately, then every 1s, for a total of 5 executions:
 * const id = setIntervalWithTimes(count => console.log(count), 1000, 5, true);
 *
 * // Start after the first tick (no immediate call), run 3 times:
 * setIntervalWithTimes(count => doWork(count), 500, 3, false);
 * ```
 */
export function setIntervalWithTimes(callback: (executedCount: number) => void, delay?: number, repeatTimes: number = Infinity, immediate: boolean = true) {
	let executedCount = 0;

	if (immediate)
		callback(executedCount++);

	// If `executedCount` is 1, and `immediate` is true, then do not run the `setInterval` function.
	if (executedCount >= repeatTimes) return 0;

	const intervalId = setInterval(() => {
		callback(executedCount++);

		if (executedCount >= repeatTimes)
			clearInterval(intervalId);
	}, delay);

	return intervalId;
}
