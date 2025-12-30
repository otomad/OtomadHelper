import cursor from "assets/cursors/pixelated_arrow.svg?cursor";
import tipsImage from "assets/images/tips/sand_with_swords.avif";

export default function PixelScaling() {
	const {
		enabled,
		scaleFactor: [scaleFactor, _setScaleFactor],
		autoScaleFactor: [autoScaleFactor, setAutoScaleFactor],
		replaceSource: [replaceSource, setReplaceSource],
	} = useSelectConfig(c => c.visual.pixelScaling);
	const meta = metas.visual.pixelScaling;

	const [displayScaleFactor, setDisplayScaleFactor] = useState<Readable | undefined>(scaleFactor);
	const isManuallyAutoScaleFactor = scaleFactor === 0;
	// eslint-disable-next-line eqeqeq
	const isActuallyAutoScaleFactor = displayScaleFactor == 0;
	const setScaleFactor = setStateInterceptor(_setScaleFactor, undefined, () => setAutoScaleFactor(false));

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.pixelScaling}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} icon="lightbulb" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="pixelate_zoom" title="pixelScaling" enabled={enabled}>
				<Setting
					meta={meta.scaleFactor}
					checkInfo={isActuallyAutoScaleFactor ? t.auto : displayScaleFactor + t.units.percent}
					alwaysShowCheckInfo
					expanded
				>
					<ToggleSwitch on={[isManuallyAutoScaleFactor || autoScaleFactor, setAutoScaleFactor]} disabled={isManuallyAutoScaleFactor}>{t.auto}</ToggleSwitch>
					<Expander.ChildWrapper $single>
						<Slider value={[autoScaleFactor ? 0 : scaleFactor, setScaleFactor]} step={1} displayValue onDisplayValueChanged={setDisplayScaleFactor} />
					</Expander.ChildWrapper>
				</Setting>
				<Setting meta={meta.replaceSourceMedia} on={[replaceSource, setReplaceSource]} />
			</EmptyMessage.Typical>
		</div>
	);
}
