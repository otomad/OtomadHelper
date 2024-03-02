export default function Track() {
	const selectedTracks = 3;
	const layout = selectConfig(c => c.track.layout);
	const [multitrackForChordsInVisual] = selectConfig(c => c.visual.multitrackForChords);
	const { pushPage } = usePageStore();

	return (
		<div className="container">
			<Subheader>{t.track.layout}</Subheader>
			<SettingsCardToggleSwitch
				title={t.track.useInGeneration}
				details={t.descriptions.track.useInGeneration}
				selectInfo={
					!layout[0] ? t(selectedTracks).selectInfo.videoTrack :
					multitrackForChordsInVisual ? t(selectedTracks).selectInfo.videoTrackGeneratedGeq :
					t(selectedTracks).selectInfo.videoTrackGenerated
				}
				icon="enabled"
				on={layout}
			/>
			<SettingsCard title={t.track.grid} type="button" icon="grid" onClick={() => pushPage("home")} />
			<SettingsCard title={t.track.box3d} type="button" icon="cube" />
			<SettingsCard title={t.track.gradient} details={t.descriptions.track.gradient} type="button" icon="highlight" />

			<Subheader>{t.stream.legato}</Subheader>
			<Expander title={t.track.legato} details={t.descriptions.track.legato} icon="legato" />

			<Subheader>{t.track.clear}</Subheader>
			<StackPanel $direction="horizontal">
				<Button icon="clear_motion" accent="critical">{t.track.clear.motion}</Button>
				<Button icon="clear_plugin" accent="critical">{t.track.clear.effect}</Button>
			</StackPanel>
		</div>
	);
}
