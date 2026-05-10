import cursor from "assets/cursors/pixelated_arrow.svg?cursor";
import tipsImage from "assets/images/tips/sand_with_swords.avif";

export default function PixelScaling() {
	const { enabled, replaceSource } = useSubConfig(c => c.visual.pixelScaling);
	const meta = metas.visual.pixelScaling;

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.pixelScaling}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} icon="lightbulb" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="pixelate_zoom" title="pixelScaling" enabled={enabled}>
				<PixelScalingScaleFactor />
				<Setting meta={meta.replaceSourceMedia} on={replaceSource} />
			</EmptyMessage.Typical>
		</div>
	);
}

function PixelScalingScaleFactor() {
	const meta = metas.visual.pixelScaling.scaleFactor;
	const { scaleFactor: [scaleFactor, _setScaleFactor], autoScaleFactor: [autoScaleFactor, setAutoScaleFactor] } = useSelectConfig(c => c.visual.pixelScaling);
	const isManuallyAutoScaleFactor = scaleFactor === 0;
	const setScaleFactor = setStateInterceptor(_setScaleFactor, undefined, () => setAutoScaleFactor(false));

	return (
		<Setting
			meta={meta}
			// checkInfo={isActuallyAutoScaleFactor ? t.auto : scaleFactor + t.units.percent}
			checkInfo={<PatternedNumberFlow specialValue={{ 0: t.auto }}>{(autoScaleFactor ? 0 : scaleFactor) + t.units.percent}</PatternedNumberFlow>}
			alwaysShowCheckInfo
			expanded
		>
			<ToggleSwitch on={[isManuallyAutoScaleFactor || autoScaleFactor, setAutoScaleFactor]} disabled={isManuallyAutoScaleFactor}>{t.auto}</ToggleSwitch>
			<Expander.ChildWrapper $single>
				<Slider value={[autoScaleFactor ? 0 : scaleFactor, setScaleFactor]} step={1} />
			</Expander.ChildWrapper>
		</Setting>
	);
}
