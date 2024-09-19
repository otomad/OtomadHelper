import type { BackgroundImageRowWithMore } from "hooks/background-images";
import { useLanguage } from "locales/config";

export default function Settings() {
	const { i18n } = useTranslation();
	const [language, setLanguage] = useLanguage();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme: [scheme, setScheme] } = useStoreState(colorModeStore);
	const {
		uiScale, hideUseTips,
		backgroundImageOpacity, backgroundImageTint, backgroundImageBlur,
	} = selectConfig(c => c.settings);
	const backgroundImages = useBackgroundImages();
	const showBackgroundImage = backgroundImages.backgroundImage[0] !== -1;

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
			<ExpanderRadio
				title={t.settings.appearance.backgroundImage}
				icon="wallpaper"
				items={backgroundImages.items}
				expanded
				view="grid"
				value={backgroundImages.backgroundImage}
				idField="key"
				imageField="url"
				checkInfoCondition={showBackgroundImage ? t.on : t.off}
				onItemContextMenu={(item, e) => {
					if (item.key !== -1) createContextMenu([
						{ label: t.menu.delete, onClick: () => backgroundImages.delete(item.key) },
					])(e);
				}}
				before={(
					<Expander.ChildWrapper>
						<Button icon="open_file" onClick={addBackgroundImage}>{t.browse}</Button>
					</Expander.ChildWrapper>
				)}
			>
				{showBackgroundImage && (
					<>
						<Expander.Item title={t.settings.appearance.backgroundImage.opacity} icon="opacity">
							<Slider
								value={backgroundImageOpacity}
								min={0}
								max={1}
								step={0.01}
								defaultValue={0.2}
								displayValue
							/>
						</Expander.Item>
						<Expander.Item title={t.settings.appearance.backgroundImage.tint} icon="saturation">
							<Slider
								value={backgroundImageTint}
								min={0}
								max={1}
								step={0.01}
								defaultValue={0}
								displayValue
							/>
						</Expander.Item>
						<Expander.Item title={t.settings.appearance.backgroundImage.blur} icon="blur">
							<Slider
								value={backgroundImageBlur}
								min={0}
								max={64}
								step={1}
								defaultValue={0}
								displayValue
							/>
						</Expander.Item>
					</>
				)}
			</ExpanderRadio>
			<ExpanderRadio
				title={t.settings.appearance.colorScheme}
				icon="paint_brush"
				items={schemes}
				expanded
				view="grid"
				value={[scheme, setScheme]}
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
