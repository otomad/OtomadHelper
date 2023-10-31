export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorMode();

	return (
		<div className="container">
			<Expander heading="Language" expanded>
				{languages.map(language => (
					<div key={language}>
						<label>
							<input type="radio" checked={language === i18n.language} onChange={e => e.target.checked && i18n.changeLanguage(language)} />
							{language}
						</label>
					</div>
				))}
			</Expander>
			<Expander heading="Color scheme" expanded>
				{schemes.map(thisScheme => (
					<div key={thisScheme}>
						<label>
							<input type="radio" checked={scheme === thisScheme} onChange={e => e.target.checked && setScheme(thisScheme)} />
							{thisScheme}
						</label>
					</div>
				))}
			</Expander>
		</div>
	);
}
