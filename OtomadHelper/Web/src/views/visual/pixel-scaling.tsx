import cursor from "assets/cursors/pixelated_cursor.cur";
import tipsImage from "assets/images/tips/sand_with_sword.png";

export default function PixelScaling() {
	const [enabled, setEnabled] = selectConfig(c => c.visual.enablePixelScaling);
	const [scaleFactor, setScaleFactor] = useState(100);
	const [autoScaleFactor, setAutoScaleFactor] = useState(true);
	const [replaceSource, setReplaceSource] = useState(true);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.pixelScaling}</SettingsPageControl>
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
					<SettingsCardToggleSwitch title={t.pixelScaling.replaceSourceMedia} icon="replace" on={[replaceSource, setReplaceSource]} />
				</>
			)}
		</div>
	);
}
