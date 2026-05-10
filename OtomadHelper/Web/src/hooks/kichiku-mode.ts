export function useKichikuMode(): StatePropertyNonNull<KichikuMode> {
	const { enabled: [ytpEnabled, setYtpEnabled] } = useSelectConfig(c => c.ytp);

	const mode = ytpEnabled ? "ytp" : "otomad";
	const setMode = setStateNarrow<KichikuMode>(value => setYtpEnabled(value === "ytp"), () => mode);

	return [mode, setMode];
}
