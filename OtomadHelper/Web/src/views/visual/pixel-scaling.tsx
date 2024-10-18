import cursor from "assets/cursors/pixelated_cursor.svg?cursor";
import tipsImage from "assets/images/tips/sand_with_sword.png";

export default function PixelScaling() {
	const {
		enabled,
		scaleFactor: [scaleFactor, _setScaleFactor],
		autoScaleFactor: [autoScaleFactor, setAutoScaleFactor],
		replaceSource: [replaceSource, setReplaceSource],
	} = selectConfig(c => c.visual.pixelScaling);

	const isManuallyAutoScaleFactor = scaleFactor === 0;
	const isActuallyAutoScaleFactor = autoScaleFactor || isManuallyAutoScaleFactor;
	const setScaleFactor = setStateInterceptor(_setScaleFactor, undefined, () => setAutoScaleFactor(false));

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.pixelScaling}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="miscz" title="pixelScaling" enabled={enabled}>
				<Expander
					title={t.pixelScaling.scaleFactor}
					icon="zoom_in"
					checkInfo={isActuallyAutoScaleFactor ? t.auto : scaleFactor + "%"}
					alwaysShowCheckInfo
					expanded
				>
					<ToggleSwitch on={[isManuallyAutoScaleFactor || autoScaleFactor, setAutoScaleFactor]} disabled={isManuallyAutoScaleFactor}>{t.auto}</ToggleSwitch>
					<Expander.ChildWrapper>
						<Slider value={[autoScaleFactor ? 0 : scaleFactor, setScaleFactor]} step={1} />
					</Expander.ChildWrapper>
				</Expander>
				<SettingsCardToggleSwitch title={t.pixelScaling.replaceSourceMedia} icon="replace" on={[replaceSource, setReplaceSource]} />
			</EmptyMessage.Typical>
		</div>
	);
}
