export const devStore = createStore({
	devMode: import.meta.env.DEV,
	rtl: false,
});

subscribeStore(devStore, () => {
	document.dir = devStore.rtl ? "rtl" : "ltr";
	bridges.bridge.setIsDevMode(devStore.devMode);
});

export const isDevMode = () => devStore.devMode;
export const isProdMode = () => !devStore.devMode;
