/**
 * Get user's current monitor frame rate per seconds.
 * @returns Monitor FPS.
 * @see https://stackoverflow.com/questions/6131051
 * @note Data will be abnormal when dropping frames.
 */
export const getMonitorFps = lodash.throttle(function () {
	return new Promise<number>(resolve =>
		requestAnimationFrame(t1 =>
			requestAnimationFrame(t2 => resolve(1000 / (t2 - t1))),
		),
	);
}, 1000);

const monitorFpsStore = createJotaiStore();
const monitorFpsFromHostAtom = atom<number | undefined>(undefined);
const monitorFpsAtom = atom(60);
monitorFpsAtom.onMount = setMonitorFps => {
	const intervalId = setInterval(async () => {
		setMonitorFps(clamp(await getMonitorFps() || 60, 10, 300));
		if (monitorFpsStore.get(monitorFpsFromHostAtom) !== undefined)
			clearInterval(intervalId);
	}, 1000);
};

useListen("host:fpsUpdated", ({ fps }) => monitorFpsStore.set(monitorFpsFromHostAtom, fps));

/**
 * Get user's current monitor frame rate per seconds.
 * @returns Monitor FPS.
 */
export const useMonitorFps = () => {
	const fpsFromBrowser = useAtomValue(monitorFpsAtom);
	const fpsFromHost = useAtomValue(monitorFpsFromHostAtom);
	return fpsFromHost ?? fpsFromBrowser;
};
