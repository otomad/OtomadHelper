export /* internal */ default function EmptyMessageYtpDisabled({ fully: feature, children }: FCP<{
	/** If YTP enabled, the whole feature is unavailable, pass the feature name to this parameter. */
	fully?: string;
}>) {
	const { enabled: [ytpEnabled, setYtpEnabled] } = selectConfig(c => c.ytp);
	const { changePage } = useSnapshot(pageStore);

	if (!ytpEnabled) return children;
	return (
		<EmptyMessage
			key="ytpEnabled"
			icon="ytp"
			title={feature ? t.empty.ytpEnabled.fully.title({ feature }) : t.empty.ytpEnabled.partial.title}
			details={feature ? t.empty.ytpEnabled.fully.details({ feature }) : t.empty.ytpEnabled.partial.details}
			noSideEffect
		>
			<StackPanel>
				<Button onClick={() => setYtpEnabled(false)}>{t.empty.ytpEnabled.disableYtp}</Button>
				<Button onClick={() => changePage(["ytp"])} accent>{t.empty.ytpEnabled.gotoYtp}</Button>
			</StackPanel>
		</EmptyMessage>
	);
}
