/**
 * Wait for the next animation frame to update refresh.
 * @returns Empty promise.
 */
export function nextAnimationTick() {
	return new Promise<void>(resolve => {
		window.requestAnimationFrame(() => {
			window.requestAnimationFrame(() => {
				resolve();
			});
		});
	});
}

/**
 * That's right! it's the famous **delay** function.\
 * This will execute asynchronously and will not block the thread.
 *
 * @param ms - Milliseconds.
 * @returns Empty promise.
 */
export function delay(ms: number): Promise<void> {
	return new Promise(resolve => setTimeout(resolve, ms));
}
