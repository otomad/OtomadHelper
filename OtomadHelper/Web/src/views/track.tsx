export default function Track() {
	const selectedTracks = 3;
	const layout = selectConfig(c => c.track.layout);
	const [multitrackForChordsInVisual] = selectConfig(c => c.visual.multitrackForChords);
	const { pushPage } = usePageStore();

	return (
		<div className="container">
			<Subheader>{t.track.layout}</Subheader>
			<SettingsCardToggleSwitch
				heading={t.track.useInGeneration}
				caption={t.descriptions.track.useInGeneration}
				selectInfo={
					!layout[0] ? t(selectedTracks).selectInfo.videoTrack :
					multitrackForChordsInVisual ? t(selectedTracks).selectInfo.videoTrackGeneratedGeq :
					t(selectedTracks).selectInfo.videoTrackGenerated
				}
				icon="enabled"
				on={layout}
			/>
			<SettingsCard heading={t.track.grid} type="button" icon="grid" onClick={() => pushPage("grid")} />
			<SettingsCard heading={t.track.box3d} type="button" icon="cube" />
			<SettingsCard heading={t.track.gradient} caption={t.descriptions.track.gradient} type="button" icon="highlight" />

			<Subheader>{t.stream.legato}</Subheader>
			<Expander heading={t.track.legato} caption={t.descriptions.track.legato} icon="legato" />

			<Subheader>{t.track.clear}</Subheader>
			<StackPanel $direction="horizontal">
				<Button icon="clear_motion" accent="critical">{t.track.clear.motion}</Button>
				<Button icon="clear_plugin" accent="critical">{t.track.clear.effect}</Button>
			</StackPanel>
		</div>
	);
}
