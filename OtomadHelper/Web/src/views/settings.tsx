const ExpanderChildSliderWrapper = styled.div`
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
			<ExpanderRadio
				heading={t.settings.language}
				items={languages}
				expanded
				value={[i18n.language, i18n.changeLanguage]}
				idField
				nameField={t.settings.language}
			/>
			<ExpanderRadio
				heading={t.settings.colorScheme}
				items={schemes}
				expanded
				value={[scheme, setScheme as SetState<string>]}
				idField
				nameField={t.settings.colorScheme}
			/>
			<Expander heading={t.settings.uiScale} checkInfo={uiScale[0] + "%"} alwaysShowCheckInfo expanded>
				<ExpanderChildSliderWrapper>
					<Slider value={uiScale} min={50} max={200} defaultValue={100} step={1} />
				</ExpanderChildSliderWrapper>
			</Expander>
			<SettingsCardToggleSwitch heading={t.settings.devMode} on={devMode} />
		</div>
	);
}
