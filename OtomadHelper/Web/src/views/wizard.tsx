import Score from "./score";

export default function Wizard() {
	"use no memo";
	const mode = useKichikuMode();
	const ytpEnabled = mode[0] === "ytp";
	const { sourceFrom } = useSubConfig(c => c.source);
	const { enabled: audioEnabled, stretch: audioStretchOption, tuningMethod, basePitch } = useSubConfig(c => c.audio);
	const { enabled: visualEnabled, stretch: visualStretchOption } = useSubConfig(c => c.visual);
	const { effects: prveEffects } = useSubConfig(c => c.visual.prve.general);
	const tuningEnabled = computedSubStore([tuningMethod],
		method => method !== "none",
		enabled => tuningMethod.current = enabled ? "elastic" : "none",
	);
	const hFlipEnabled = computedSubStore([prveEffects],
		effects => effects.some(effect => effect.fx === "hFlip"),
		enabled => prveEffects.current = enabled ? [{ fx: "hFlip", initial: [1, 2] }] : [{ fx: "normal", initial: [0] }],
	);
	const [audioStretch, visualStretch] = [audioStretchOption, visualStretchOption].map(option => computedSubStore([option],
		option => option !== "noStretching",
		enabled => option.current = enabled ? "flexingAndExtending" : "noStretching",
	));
	const isAudioDisabled = computedSubStore([audioEnabled], audioEnabled => ytpEnabled || !audioEnabled);
	const isAudioTuningDisabled = computedSubStore([audioEnabled, tuningEnabled], (audioEnabled, tuningEnabled) => ytpEnabled || !audioEnabled || !tuningEnabled);
	const isVisualDisabled = computedSubStore([visualEnabled], visualEnabled => ytpEnabled || !visualEnabled);
	const { thumbnail } = useThumbnail();
	const { changePage } = useSnapshot(pageStore);

	const previewModeVocaloid = <div><img src={thumbnail} style={{ width: "100%" }} /><PreviewKaraoke demoMode /></div>;
	return (
		<div className="container">
			<Subheader>{t.mode}</Subheader>
			<ItemsView view="grid" current={mode} inlineAlignment="start">
				<ItemsView.Item id="otomad" image={<PreviewLayout thumbnail={thumbnail} />}>{t.mode.otomad}</ItemsView.Item>
				<ItemsView.Item id="vocaloid" image={previewModeVocaloid}>{t.mode.vocaloid}</ItemsView.Item>
				<ItemsView.Item id="ytp" image={<PreviewModeYtp thumbnail={thumbnail} />}>{t.mode.ytp}</ItemsView.Item>
			</ItemsView>

			<Subheader>{t.source.from}</Subheader>
			<ItemsView view="tile" current={sourceFrom}>
				<ItemsView.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</ItemsView.Item>
				<ItemsView.Item id="projectMedia" icon="media">{t.source.projectMedia}</ItemsView.Item>
			</ItemsView>
			<Subheader>{t.score.from}</Subheader>

			<Subheader>{t.titles.audio}</Subheader>
			<SettingsCardToggleSwitch on={audioEnabled} title={t.stream.enabled.audio} icon="lightbulb" />
			<SettingsCardToggleSwitch disabled={isAudioDisabled} on={audioStretch} title={t.stream.stretch} details={t.descriptions.stream.stretch} icon="arrow_bidirectional_left_right" />
			<SettingsCardToggleSwitch disabled={isAudioDisabled} on={tuningEnabled} title={t.stream.tuning} details={t.descriptions.stream.tuning} icon="tuning" />
			<SettingsCard disabled={isAudioTuningDisabled} title={t.stream.tuning.basePitch} details={t.descriptions.stream.tuning.basePitch} icon="music_note">
				<PitchPicker spn={basePitch} />
			</SettingsCard>

			<Subheader>{t.titles.visual}</Subheader>
			<SettingsCardToggleSwitch on={visualEnabled} title={t.stream.enabled.visual} icon="lightbulb" />
			<SettingsCardToggleSwitch disabled={isVisualDisabled} on={visualStretch} title={t.stream.stretch} details={t.descriptions.stream.stretch} icon="arrow_bidirectional_left_right" />
			<SettingsCardToggleSwitch disabled={isVisualDisabled} on={hFlipEnabled} title={t.prve.effects.hFlip} details={t.descriptions.prve.hFlip} icon="flip_h" />

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
