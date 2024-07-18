import cursor from "assets/cursors/pixelated_cursor.cur";
import tipsImage from "assets/images/tips/sand_with_sword.png";

export default function PixelScaling() {
	const {
		enabled,
		scaleFactor: [scaleFactor, setScaleFactor],
		autoScaleFactor: [autoScaleFactor, setAutoScaleFactor],
		replaceSource: [replaceSource, setReplaceSource],
	} = selectConfig(c => c.visual.pixelScaling);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.pixelScaling}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="miscz" title="pixelScaling" enabled={enabled}>
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
			</EmptyMessage.Typical>
		</div>
	);
}
