export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});

	return (
		<div>
			{languages.map(language => (
				<label key={language}>
					<input type="radio" checked={language === i18n.language} onChange={e => e.target.checked && i18n.changeLanguage(language)} />
					{language}
				</label>
			))}
		</div>
	);
}
