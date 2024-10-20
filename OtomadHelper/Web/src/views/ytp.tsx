import cursor from "assets/cursors/poo.cur";
import tipsImage from "assets/images/tips/yoooo_a_boom.png";
import exampleThumbnail from "assets/images/ヨハネの氷.png";

const effects = ["chorus", "delay", "changePitch", "reverse", "changeSpeed", "vibrato", "changeHue", "rotateHue", "monochrome", "negative", "repeatRapidly", "randomTuning", "upsize", "spherize", "mirror", "highContrast", "oversaturation", "emphasizeThrice", "twist", "mosaic", "thermal", "emboss", "bump", "edge"];

export default function Ytp() {
	const { enabled, clips, constraintStart, constraintEnd } = selectConfig(c => c.ytp);
	const [selectEffects, setSelectEffects] = useState<string[]>([]);
	const selectEffectCount = selectEffects.length;

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.ytp}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} selectInfo={t(1).selectInfo.source} icon="enabled" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="ytp" title="ytp" enabled={enabled}>
				<Subheader>{t.subheaders.parameters}</Subheader>
				<Expander title={t.ytp.constraint} details={t.descriptions.ytp.constraint} icon="constraint">
					<ExpanderChildTrim.Value start={constraintStart} end={constraintEnd} min={1} decimalPlaces={0} spinnerStep={100} />
				</Expander>
				<SettingsCard title={t.ytp.clips} details={t.descriptions.ytp.clips} icon="number">
					<TextBox.Number value={clips} min={0} decimalPlaces={0} suffix={t.units.pieces} />
				</SettingsCard>
				<Subheader>{t(2).titles.effect}</Subheader>
				<Expander
					title={t.ytp.effects}
					details={t.descriptions.ytp.effects}
					icon="sparkle"
					actions={(
						<OverlapLayout $horizontalAlign="end" $verticalAlign="center">
							{selectEffectCount === 1 && <span>{selectEffects[0]}</span>}
							<Badge hidden={selectEffectCount < 2}>{selectEffectCount}</Badge>
						</OverlapLayout>
					)}
				>
					<SelectAll value={[selectEffects, setSelectEffects]} all={effects} />
					<ItemsView view="grid" current={[selectEffects, setSelectEffects]} multiple>
						{effects.map(name => (
							<ItemsView.Item
								key={name}
								id={name}
								image={<PreviewYtp thumbnail={exampleThumbnail} name={name} />}
							>
								{name}
							</ItemsView.Item>
						))}
					</ItemsView>
				</Expander>
			</EmptyMessage.Typical>
		</div>
	);
}
