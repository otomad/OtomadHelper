export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorMode();

	return (
		<div className="container">
			<section>
				{languages.map(language => (
					<label key={language}>
						<input type="radio" checked={language === i18n.language} onChange={e => e.target.checked && i18n.changeLanguage(language)} />
						{language}
					</label>
				))}
			</section>
			<section>
				{schemes.map(thisScheme => (
					<label key={thisScheme}>
						<input type="radio" checked={scheme === thisScheme} onChange={e => e.target.checked && setScheme(thisScheme)} />
						{thisScheme}
					</label>
				))}
			</section>
		</div>
	);
}
