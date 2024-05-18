export const useDevStore = createStore(() => ({
	devMode: import.meta.env.DEV,
	rtl: false,
}));

useDevStore.subscribe(({ devMode, rtl }) => {
	document.dir = rtl ? "rtl" : "ltr";
	bridges.bridge.setIsDevMode(devMode);
});
