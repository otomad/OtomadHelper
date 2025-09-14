import cursor from "assets/cursors/poo.svg?cursor";
import tipsImage from "assets/images/tips/yoooo_a_boom.avif";
import exampleThumbnail from "assets/images/ヨハネの氷.avif";

type Stream = "audio" | "video" | "n/a";
const $s = (mainStream: Stream, ...sideEffects: [stream: Stream, name: string, likely?: boolean][]) =>
	({ stream: mainStream, sideEffects: sideEffects.map(([stream, name, likely]) => ({ stream, name, likely: !!likely })) });
const effects: Record<string, ReturnType<typeof $s>> = {
	chorus: $s("audio"),
	delay: $s("audio"),
	changePitch: $s("audio"),
	reverse: $s("n/a"),
	changeSpeed: $s("n/a"),
	vibrato: $s("audio", ["video", "wave", true]),
	changeHue: $s("video"),
	rotateHue: $s("video"),
	monochrome: $s("video"),
	negative: $s("video", ["audio", "pitchDown", true]),
	repeatRapidly: $s("n/a"),
	randomTuning: $s("audio", ["video", "hFlipWithRhythm"]),
	upsize: $s("video", ["audio", "loud"]),
	spherize: $s("video"),
	mirror: $s("video"),
	highContrast: $s("video", ["audio", "loud"]),
	oversaturation: $s("video", ["audio", "pitchUp", true]),
	emphasizeThrice: $s("n/a", ["audio", "pitchUpOrPitchDown"], ["video", "sporadicUpsizeFocusMotion"], ["video", "monochrome", true]),
	twist: $s("video"),
	pixelate: $s("video"),
	spectrum: $s("video"),
	thermal: $s("video"),
	emboss: $s("video"),
	bump: $s("video"),
	edge: $s("video"),
};
const effectNames = Object.keys(effects);

const StyledSideEffect = styled.div`
	display: flex;
	align-items: center;
	margin-block: 4px;

	.icon {
		font-size: 16px;

		+ .icon {
			margin-inline-start: 4px;
		}

		+ span {
			margin-inline-start: 6px;
		}
	}

	span {
		margin-block-start: -1px;
	}
`;

export default function Ytp() {
	const { enabled, clips, constraintStart, constraintEnd } = useSelectConfig(c => c.ytp);
	const [selectEffects, setSelectEffects] = useState<string[]>([]);
	const selectEffectCount = selectEffects.length;

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.ytp.slogan}<br />{t.descriptions.ytp}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} selectInfo={t(1).selectInfo.source} icon="enabled" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="ytp" title="ytp" enabled={enabled}>
				<Subheader>{t.subheaders.parameters}</Subheader>
				<Expander title={t.ytp.constrain} details={t.descriptions.ytp.constrain} icon="constraint">
					<ExpanderChildTrim.Value start={constraintStart} end={constraintEnd} min={1} decimalPlaces={0} spinnerStep={100} />
				</Expander>
				<SettingsCard title={t.ytp.clips} details={t.descriptions.ytp.clips} icon="number">
					<TextBox.Number value={clips} min={0} decimalPlaces={0} suffix={t.units.piece} />
				</SettingsCard>
				<Subheader>{t(2).titles.effect}</Subheader>
				<Expander
					title={t.ytp.effects}
					details={t.descriptions.ytp.effects}
					icon="sparkle"
					actions={(
						<OverlapLayout $horizontalAlign="end" $verticalAlign="center">
							{selectEffectCount === 1 && <span>{t.ytp.effects[selectEffects[0]]}</span>}
							<Badge hidden={selectEffectCount < 2}>{selectEffectCount}</Badge>
						</OverlapLayout>
					)}
				>
					<ItemsView view="grid" current={[selectEffects, setSelectEffects]} multiple selectAll>
						{effectNames.map(name => {
							const { stream, sideEffects } = effects[name];
							return (
								<ItemsView.Item
									key={name}
									id={name}
									image={<PreviewYtp thumbnail={exampleThumbnail} name={name} />}
									details={sideEffects.map(({ stream, name, likely }) => (
										<StyledSideEffect key={name}>
											<Icon name={stream === "audio" ? "volume" : stream === "video" ? "image" : ""} />
											{likely && <Icon name="dice" />}
											<span>{tf.ytp.sideEffects[name] ?? tf.ytp.effects[name]}</span>
										</StyledSideEffect>
									))}
								>
									<StackPanel>
										<Icon name={stream === "audio" ? "volume" : stream === "video" ? "image" : ""} style={{ fontSize: "20px" }} />
										<span>{t.ytp.effects[name]}</span>
									</StackPanel>
								</ItemsView.Item>
							);
						})}
					</ItemsView>
				</Expander>
			</EmptyMessage.Typical>
		</div>
	);
}
