import { exactTuningMethods, tuningClassicModes, tuningElasticModes } from "views/audio";

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
	const { language: [language, setLanguage], openglInterop, autosaveInterval, defaultTextTool, defaultTuningMethod, defaultClassicMode, defaultElasticMode, preserveClipboardOnClose, eventGroupSelection } = useSelectConfig(c => c.settings.internal);
	return (
		<div className="container">
			<InfoBar status="warning" title={t.infoBar.warning}>{t.descriptions.settings.internal.info}</InfoBar>

			<ExpanderRadio
				title={t.settings.internal.language}
				details={t.descriptions.settings.internal.language}
				icon="globe"
				items={vegasLanguages}
				view="grid"
				value={[language, setLanguage]}
				idField="tag"
				nameField={({ tag: language }) => getLocaleName(language, currentLanguage)}
				checkInfoCondition={language => language && getLocaleName(language, currentLanguage)}
				imageField={({ tag: language }) => <PreviewLanguage language={language} showProgress={false} />}
				itemsViewItemAttrs={{ withBorder: true }}
			/>
			<SettingsCard title={t.settings.internal.autosaveInterval} details={t.descriptions.settings.internal.autosaveInterval} icon="save_clock">
				<TextBox.RoughTime value={autosaveInterval} />
			</SettingsCard>
			<Expander title={t.settings.internal.defaultTextTool} details={t.descriptions.settings.internal.defaultTextTool} icon="text_toolbox" />
			<ExpanderRadio
				title={t.settings.internal.defaultTuningMethod}
				details={t.descriptions.settings.internal.defaultTuningMethod}
				icon="tuning"
				view="tile"
				idField="id"
				iconField="icon"
				value={defaultTuningMethod}
				items={exactTuningMethods}
				nameField={({ id }) => t.stream.tuning.tuningMethod[id]}
				checkInfoCondition={id => t.stream.tuning.tuningMethod[id!]}
			/>
			<ExpanderRadio
				title={t.settings.internal.defaultElasticMode}
				details={t.descriptions.settings.internal.defaultElasticMode}
				icon="plus_minus"
				view="tile"
				idField
				value={defaultElasticMode}
				items={tuningElasticModes}
				nameField={t.stream.tuning.stretchAttributes.elastic}
			/>
			<ExpanderRadio
				title={t.settings.internal.defaultClassicMode}
				details={t.descriptions.settings.internal.defaultClassicMode}
				icon="history"
				view="tile"
				idField
				value={defaultClassicMode}
				items={tuningClassicModes}
				nameField={id => <TuningClassicModeListItem id={id} />}
				checkInfoCondition={id => id ? t.stream.tuning.stretchAttributes.classic[id] : ""}
			/>
			<SettingsCardToggleSwitch on={preserveClipboardOnClose} title={t.descriptions.settings.internal.preserveClipboardOnClose} details={t.settings.internal.preserveClipboardOnClose} icon="clipboard_checkmark" />
			<SettingsCardToggleSwitch on={eventGroupSelection} title={t.settings.internal.eventGroupSelection} details={t.descriptions.settings.internal.eventGroupSelection} icon="group_link" />
			<SettingsCardToggleSwitch on={openglInterop} title={t.settings.internal.openglInterop} details={t.descriptions.settings.internal.openglInterop} icon="opengl" />
		</div>
	);
}
