import type { BackgroundImageRowWithMore } from "hooks/background-images";
import { useLanguage } from "locales/config";

export default function Settings() {
	const { i18n } = useTranslation();
	const [language, setLanguage] = useLanguage();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme: [scheme, setScheme] } = useStoreState(colorModeStore);
	const { uiScale, hideUseTips } = selectConfig(c => c.settings);
	const backgroundImages = useBackgroundImages();

	// Dev mode
	const { devMode, rtl } = useStoreState(devStore);

	async function addBackgroundImage() {
		const file = await openFile({ accept: "image/*" });
		if (file) backgroundImages.add(file);
	}

	return (
		<div className="container">
			<SettingsAbout />
			<ExpanderRadio
				title={t.settings.language}
				icon="globe"
				items={languages}
				expanded
				view="grid"
				value={[language, setLanguage]}
				idField
				nameField={t.settings.language}
				imageField={language => <PreviewLanguage language={language} />}
				itemsViewItemAttrs={{ $withBorder: true }}
			/>

			<Subheader>{t.settings.appearance}</Subheader>
			<ExpanderRadio<BackgroundImageRowWithMore>
				title={t.settings.appearance.backgroundImage}
				items={backgroundImages.items}
				expanded
				view="grid"
				value={backgroundImages.backgroundImage}
				idField="key"
				imageField="url"
				onItemContextMenu={(item, e) => { if (item.key !== "-1") { stopEvent(e); backgroundImages.delete(item.key); } }}
			>
				<Expander.ChildWrapper>
					<Button onClick={addBackgroundImage}>Browse</Button>
				</Expander.ChildWrapper>
			</ExpanderRadio>
			<ExpanderRadio
				title={t.settings.appearance.colorScheme}
				icon="paint_brush"
				items={schemes}
				expanded
				view="grid"
				value={[scheme, setScheme as SetState<string>]}
				idField
				nameField={t.settings.appearance.colorScheme}
				imageField={colorScheme => <PreviewColorScheme colorScheme={colorScheme} currentColorScheme={scheme} />}
				itemsViewItemAttrs={{ $withBorder: true }}
				$itemWidth={112}
			/>
			<Expander
				title={t.settings.appearance.uiScale}
				icon="zoom_in"
				checkInfo={uiScale[0] + "%"}
				alwaysShowCheckInfo
				expanded
			>
				<Expander.ChildWrapper>
					<Slider value={uiScale} min={50} max={200} defaultValue={100} step={1} />
				</Expander.ChildWrapper>
			</Expander>

			<Subheader>{t.subheaders.config}</Subheader>
			<SettingsCardToggleSwitch title={t.settings.config.hideUsageTips} icon="chat_help_off" on={hideUseTips} />

			<Subheader>{t.settings.dev}</Subheader>
			<SettingsCardToggleSwitch title={t.settings.dev.devMode} icon="devtools" on={devMode} />
			<SettingsCardToggleSwitch title={t.settings.dev.rtl} icon="text_paragraph_direction_left" on={rtl} />
		</div>
	);
}
