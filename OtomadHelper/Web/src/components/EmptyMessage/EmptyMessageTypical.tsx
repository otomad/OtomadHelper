import type { LocaleIdentifiers } from "locales/types";

export /* @internal */ default function EmptyMessageTypical({ name: staticName, title, icon, enabled: [enabled, setEnabled], children }: FCP<{
	/** Feature name. */
	name?: string;
	/** Feature name, but automatically get from the i18n strings. */
	title?: keyof LocaleIdentifiers["javascript"]["titles"];
	/** Feature icon. */
	icon: DeclaredIcons;
	/** Is enabled? */
	enabled: StatePropertyNonNull<boolean>;
}>) {
	if (enabled) return children;
	const name = staticName || title && t.titles[title];
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
