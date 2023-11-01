export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorMode();

	return (
		<div className="container">
			<Expander heading="Language" expanded>
				{languages.map(language => (
					<RadioButton key={language} id={language} value={[i18n.language, i18n.changeLanguage]}>
						{language}
					</RadioButton>
				))}
			</Expander>
			<Expander heading="Color scheme" expanded>
				{schemes.map(thisScheme => (
					<RadioButton key={thisScheme} id={thisScheme} value={[scheme, setScheme]}>
						{thisScheme}
					</RadioButton>
				))}
			</Expander>
		</div>
	);
}
