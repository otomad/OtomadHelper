export /* internal */ default function EmptyMessageTypical({ name, icon, enabled: [enabled, setEnabled], children }: FCP<{
	/** Feature name. */
	name: string;
	/** Feature icon. */
	icon: DeclaredIcons;
	/** Is enabled? */
	enabled: StatePropertyNonNull<boolean>;
}>) {
	if (enabled) return children;
	return (
		<EmptyMessage
			key="disabled"
			icon={icon}
			title={t.empty.disabled.title({ name })}
			details={t.empty.disabled.details({ name })}
			iconOff
		>
			<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
		</EmptyMessage>
	);
}
