// cSpell:ignore isb

/**
 * That's right! it's the famous **delay** function.\
 * This will execute asynchronously and will not block the thread.
 *
 * @param ms - Milliseconds.
 * @param timeoutIdRef - Get the timeout ID and assign to a React ref object.
 * @returns Empty promise.
 */
export function delay(ms: number, timeoutIdRef?: MutableRefObject<Timeout | undefined | null>): Promise<void> {
	return new Promise(resolve => {
		const timeoutId = setTimeout(resolve, ms);
		if (timeoutIdRef) timeoutIdRef.current = timeoutId;
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
 * Schedules a callback to be executed repeatedly every delay of this many milliseconds.
 *
 * @param callback - The callback function to be called when the timer times out.
 * @param ms - The number of milliseconds to wait before calling the callback function.
 * @returns An ID reference used to cancel the timer.
 */
export function createInterval(callback: () => void, ms: number) {
	const timeoutId = useRef<Timeout>();

	useMountEffect(() => {
		timeoutId.current = setInterval(callback, ms);

		return () => clearInterval(timeoutId.current);
	});

	return timeoutId;
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
	const savedCallback = useRef<() => void>();

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
