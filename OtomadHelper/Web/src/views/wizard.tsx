import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import Score from "./score";

export default function Wizard() {
	const mode = useKichikuMode();
	const ytpEnabled = mode[0] === "ytp";
	const { sourceFrom } = useSelectConfig(c => c.source);
	const { enabled: audioEnabled, tuningMethod, basePitch } = useSelectConfig(c => c.audio);
	const { enabled: visualEnabled } = useSelectConfig(c => c.visual);
	const { effects: prveEffects } = useSelectConfig(c => c.visual.prve.general);
	const tuningEnabled = useStateSelector(tuningMethod,
		method => method !== "none",
		enabled => enabled ? "elastic" : "none",
		{ processPrevStateInSetterWithGetter: true },
	);
	const hFlipEnabled = useStateSelector(prveEffects,
		effects => effects.some(effect => effect.fx === "hFlip"),
		enabled => enabled ? [{ fx: "hFlip", initial: [1, 2] }] : [{ fx: "normal", initial: [0] }],
		{ processPrevStateInSetterWithGetter: true },
	);
	const { changePage } = useSnapshot(pageStore);

	const previewModeVocaloid = <div><img src={exampleThumbnail} style={{ width: "100%" }} /><PreviewKaraoke demoMode /></div>;
	return (
		<div className="container">
			<Subheader>{t.mode}</Subheader>
			<ItemsView view="grid" current={mode} inlineAlignment="start">
				<ItemsView.Item id="otomad" image={<PreviewLayout thumbnail={exampleThumbnail} />}>{t.mode.otomad}</ItemsView.Item>
				<ItemsView.Item id="vocaloid" image={previewModeVocaloid}>{t.mode.vocaloid}</ItemsView.Item>
				<ItemsView.Item id="ytp" image={<PreviewModeYtp thumbnail={exampleThumbnail} />}>{t.mode.ytp}</ItemsView.Item>
			</ItemsView>

			<Subheader>{t.source.from}</Subheader>
			<ItemsView view="tile" current={sourceFrom}>
				<ItemsView.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</ItemsView.Item>
				<ItemsView.Item id="projectMedia" icon="media">{t.source.projectMedia}</ItemsView.Item>
			</ItemsView>
			<Subheader>{t.score.from}</Subheader>

			<Subheader>{t.titles.audio}</Subheader>
			<SettingsCardToggleSwitch on={audioEnabled} title={t.stream.enabled.audio} icon="lightbulb" />
			<SettingsCardToggleSwitch disabled={ytpEnabled || !audioEnabled[0]} on={tuningEnabled} title={t.stream.tuning} details={t.descriptions.stream.tuning} icon="tuning" />
			<SettingsCard disabled={ytpEnabled || !audioEnabled[0] || !tuningEnabled[0]} title={t.stream.tuning.basePitch} details={t.descriptions.stream.tuning.basePitch} icon="music_note">
				<PitchPicker spn={basePitch} />
			</SettingsCard>

			<Subheader>{t.titles.visual}</Subheader>
			<SettingsCardToggleSwitch on={visualEnabled} title={t.stream.enabled.visual} icon="lightbulb" />
			<SettingsCardToggleSwitch disabled={ytpEnabled || !visualEnabled[0]} on={hFlipEnabled} title={t.prve.effects.hFlip} details={t.descriptions.prve.hFlip} icon="flip_h" />

			<Activity visible={!ytpEnabled}>
				<Score _trackSelectorOnly />
			</Activity>

			<InfoBar
				title={t.empty.wizardMode.title}
				button={<Button accent icon="contract_down_left" onClick={() => changePage(["source"])}>{t.empty.wizardMode.gotoStandard}</Button>}
				length="long"
			>
				{t.empty.wizardMode.details}
			</InfoBar>
		</div>
	);
}
