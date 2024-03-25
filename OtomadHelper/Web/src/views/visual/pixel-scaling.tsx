import tipsImage from "assets/images/tips/dirt_background_je2.png";

export default function PixelScaling() {
	const [enabled, setEnabled] = selectConfig(c => c.visual.enablePixelScaling);
	const [scaleFactor, setScaleFactor] = useState(100);
	const [autoScaleFactor, setAutoScaleFactor] = useState(true);
	const debugMode = useState(false);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="" />
			<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={[enabled, setEnabled]} resetTransitionOnChanging />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="miscz"
					title={t.empty.disabled.title({ name: t.titles.pixelScaling })}
					details={t.empty.disabled.details({ name: t.titles.pixelScaling })}
					iconOff
				>
					<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
				</EmptyMessage>
			) : (
				<>
					<Expander
						title={t.pixelScaling.scaleFactor}
						icon="zoom_in"
						checkInfo={autoScaleFactor ? t.auto : scaleFactor + "%"}
						alwaysShowCheckInfo
						expanded
					>
						<ToggleSwitch on={[autoScaleFactor, setAutoScaleFactor]}>{t.auto}</ToggleSwitch>
						<Expander.ChildWrapper>
							<Slider value={[scaleFactor, setScaleFactor]} step={1} disabled={autoScaleFactor} />
						</Expander.ChildWrapper>
					</Expander>
					<SettingsCardToggleSwitch title="调试模式" icon="devtools" on={debugMode} />
				</>
			)}
		</div>
	);
}
