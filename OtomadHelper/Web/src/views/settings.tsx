const ExpanderChildWrapper = styled.div`
	padding: 21px 51px;
`;

export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorModeStore();
	const uiScale = selectConfig(c => c.settings.uiScale);
	const devMode = selectConfig(c => c.settings.devMode);

	return (
		<div className="container">
			<Expander heading={t.settings.about} icon="info">
				<ExpanderChildWrapper>
					<p>Otomad Helper<br />v7.0</p>
					<div>
						<OpenLink>仓库地址</OpenLink>
					</div>
					<Button>{t.settings.about.checkForUpdates}</Button>
				</ExpanderChildWrapper>
			</Expander>
			<ExpanderRadio
				heading={t.settings.language}
				icon="globe"
				items={languages}
				expanded
				value={[i18n.language, i18n.changeLanguage]}
				idField
				nameField={t.settings.language}
			/>

			<Subheader>{t.settings.appearance}</Subheader>
			<ExpanderRadio
				heading={t.settings.appearance.colorScheme}
				icon="paint_brush"
				items={schemes}
				expanded
				value={[scheme, setScheme as SetState<string>]}
				idField
				nameField={t.settings.appearance.colorScheme}
			/>
			<Expander
				heading={t.settings.appearance.uiScale}
				icon="zoom_in"
				checkInfo={uiScale[0] + "%"}
				alwaysShowCheckInfo
				expanded
			>
				<ExpanderChildWrapper>
					<Slider value={uiScale} min={50} max={200} defaultValue={100} step={1} />
				</ExpanderChildWrapper>
			</Expander>

			<Subheader>{t.settings.dev}</Subheader>
			<SettingsCardToggleSwitch heading={t.settings.dev.devMode} icon="devtools" on={devMode} />
		</div>
	);
}
