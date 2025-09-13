const DEFAULT_FONT_SIZE = 14;

export function getUiScale1() {
	const { fontSize } = configStore.settings;
	return window.isWebView ? 1 : fontSize / DEFAULT_FONT_SIZE;
}

export function useUiScale1() {
	const { fontSize } = useSnapshot(configStore.settings);
	return window.isWebView ? 1 : fontSize / DEFAULT_FONT_SIZE;
}
