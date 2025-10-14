import pluginCreditRoll from "src/assets/images/plugins/credit_roll.webp";
import pluginLegacyText from "src/assets/images/plugins/legacy_text.webp";
import pluginProtypeTitler from "src/assets/images/plugins/protype_titler.webp";
import pluginCreditTitlesAndText from "src/assets/images/plugins/titles_and_text.webp";
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

export /* @internal */ const textPlugins = [
	// cspell:disable-next-line
	{ id: "titlesAndText", uid: "{Svfx:com.vegascreativesoftware:titlesandtext}", image: pluginCreditTitlesAndText },
	{ id: "legacyText", uid: "{0FE8789D-0C47-442A-AFB0-0DAF97669317}", image: pluginLegacyText },
	{ id: "protypeTitler", uid: "{53FC0B44-BD58-4716-A90F-3EB43168DE81}", image: pluginProtypeTitler },
	{ id: "creditRoll", uid: "{02FD3B60-986B-11D4-9F6F-00485481BD31}", image: pluginCreditRoll },
] as const;

const TextPluginPreviewImage = styled.img`
	${styles.mixins.square("100%")};
	object-fit: cover;
	object-position: top;

	.items-view-item:hover & {
		object-position: bottom;
	}

	.items-view-item:hover:active & {
		object-position: center;
	}
`;

export default function Internal() {
	const [currentLanguage] = useLanguage();
	const { language: [language, setLanguage], openglInterop, autosaveInterval, defaultTextPlugin, defaultTuningMethod, defaultClassicMode, defaultElasticMode, preserveClipboardOnClose, eventGroupSelection } = useSelectConfig(c => c.settings.internal);
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
			<SettingsCard title={t.settings.internal.autosaveInterval} details={t.descriptions.settings.internal.autosaveInterval({ default: 5 })} icon="save_clock">
				<TextBox.RoughTime value={autosaveInterval} />
			</SettingsCard>
			<ExpanderRadio
				title={t.settings.internal.defaultTextPlugin}
				details={t.descriptions.settings.internal.defaultTextPlugin}
				icon="text_plugin"
				view="grid"
				items={textPlugins}
				value={defaultTextPlugin}
				idField="id"
				nameField={t.shared.plugins}
				checkInfoCondition={id => id && tf.shared.plugins[id] || t.custom} // Do not to refactor it to ternary operator.
				imageField={({ image, id }) => <TextPluginPreviewImage src={image} alt={t.shared.plugins[id]} />}
			/>
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
				detailsField={({ id, originalName }) => t.stream.tuning.tuningMethod[id].toString() !== originalName && originalName}
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
				icon="hourglass"
				view="tile"
				idField
				value={defaultClassicMode}
				items={tuningClassicModes}
				nameField={id => <TuningClassicModeListItem id={id} />}
				checkInfoCondition={id => id ? t.stream.tuning.stretchAttributes.classic[id] : ""}
			/>
			<SettingsCardToggleSwitch on={preserveClipboardOnClose} title={t.settings.internal.preserveClipboardOnClose} details={t.descriptions.settings.internal.preserveClipboardOnClose} icon="clipboard_checkmark" />
			<SettingsCardToggleSwitch on={eventGroupSelection} title={t.settings.internal.eventGroupSelection} details={t.descriptions.settings.internal.eventGroupSelection} icon="group_link" />
			<SettingsCardToggleSwitch on={openglInterop} title={t.settings.internal.openglInterop} details={t.descriptions.settings.internal.openglInterop} icon="opengl" />
		</div>
	);
}
