export function useKichikuMode(): StatePropertyNonNull<KichikuMode> {
	const { enabled: [ytpEnabled, setYtpEnabled] } = useSelectConfig(c => c.ytp);

	const mode = ytpEnabled ? "ytp" : "otomad";
	const setMode: SetStateNarrow<KichikuMode> = value => {
		value = typeof value === "function" ? value(mode) : value;
		setYtpEnabled(value === "ytp");
	};

	return [mode, setMode];
}
