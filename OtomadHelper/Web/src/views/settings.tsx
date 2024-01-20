export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorModeStore();

	return (
		<div className="container">
			<ExpanderRadio
				heading={t.settings.language}
				items={languages}
				expanded
				value={[i18n.language, i18n.changeLanguage]}
				idField
				nameField={t.settings.language}
			/>
			<ExpanderRadio
				heading={t.settings.colorScheme}
				items={schemes}
				expanded
				value={[scheme, setScheme as SetState<string>]}
				idField
				nameField={t.settings.colorScheme}
			/>
		</div>
	);
}
