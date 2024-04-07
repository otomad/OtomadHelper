export const useDevStore = createStore(() => ({
	devMode: import.meta.env.DEV,
	rtl: false,
}));

useDevStore.subscribe(({ rtl }) => {
	document.dir = rtl ? "rtl" : "ltr";
});
