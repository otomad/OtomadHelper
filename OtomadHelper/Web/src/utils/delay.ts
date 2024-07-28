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
