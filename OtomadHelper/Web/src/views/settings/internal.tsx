export /* @internal */ const vegasLanguages = [
	{ id: 1031, tag: "de" },
	{ id: 1033, tag: "en" },
	{ id: 1034, tag: "es" },
	{ id: 1036, tag: "fr" },
	{ id: 1041, tag: "ja" },
	{ id: 1042, tag: "ko" },
	{ id: 1045, tag: "pl" },
	{ id: 1046, tag: "pt-BR" },
	{ id: 2052, tag: "zh-CN" },
];

export default function Internal() {
	const [currentLanguage] = useLanguage();
	const { language: [language, setLanguage], openglInterop } = useSelectConfig(c => c.settings.internal);

	return (
		<div className="container">
			<InfoBar status="warning" title={t.infoBar.warning}>{t.descriptions.settings.internal.info}</InfoBar>

			<ExpanderRadio
				title={t.settings.internal.language}
				icon="globe"
				items={vegasLanguages}
				view="grid"
				value={[language, setLanguage]}
				idField="tag"
				nameField={({ tag: language }) => getLocaleName(language, currentLanguage)}
				checkInfoCondition={t.metadata.name}
				imageField={({ tag: language }) => <PreviewLanguage language={language} showProgress={false} />}
				itemsViewItemAttrs={{ withBorder: true }}
			/>
			<SettingsCardToggleSwitch on={openglInterop} title={t.settings.internal.language} />
		</div>
	);
}
