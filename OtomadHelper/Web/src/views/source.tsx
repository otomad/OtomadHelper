export /* internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;

export default function Source() {
	const source = selectConfig(c => c.source.source);
	const trimStart = selectConfig(c => c.source.trim.start);
	const trimEnd = selectConfig(c => c.source.trim.end);
	const startTime = selectConfig(c => c.source.startTime.use);
	const customStartTime = selectConfig(c => c.source.startTime.custom);
	const [preferredTrack, setPreferredTrack] = selectConfig(c => c.source.preferredTrack.value);
	const belowAdjustmentTracks = selectConfig(c => c.source.preferredTrack.belowAdjustmentTracks);
	const removeSourceClips = selectConfig(c => c.source.afterCompletion.removeSourceClips);
	const selectSourceClips = selectConfig(c => c.source.afterCompletion.selectSourceClips);
	const selectGeneratedAudioClips = selectConfig(c => c.source.afterCompletion.selectGeneratedAudioClips);
	const selectGeneratedVideoClips = selectConfig(c => c.source.afterCompletion.selectGeneratedVideoClips);
	const randomOffsetForTracks = selectConfig(c => c.source.randomOffsetForTracks);

	mutexSwitches(removeSourceClips, selectSourceClips);

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={source}>
					<TabBar.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</TabBar.Item>
					<TabBar.Item id="projectMedia" icon="media">{t.source.projectMedia}</TabBar.Item>
					<TabBar.Item id="browseFile" icon="open_file">{t.source.browseFile}</TabBar.Item>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Expander title={t.source.trim} details={t.descriptions.source.trim} icon="trim">
				<ExpanderChildTrim start={trimStart} end={trimEnd} />
			</Expander>
			{/* <Expander title={t.source.startTime} details={t.descriptions.source.startTime} icon="start_point">
				<ItemsView view="tile" current={startTime}>
					{startTimes.map(item =>
						<ItemsView.Item id={item.id} key={item.id} icon={item.icon}>{item.name}</ItemsView.Item>)}
				</ItemsView>
			</Expander> */}
			<ExpanderRadio
				title={t.source.startTime}
				details={t.descriptions.source.startTime}
				icon="start_point"
				items={startTimes}
				value={startTime as StateProperty<string>}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			>
				<Expander.ChildWrapper>
					<TimecodeBox timecode={customStartTime} onFocus={() => startTime[1]("custom")} />
				</Expander.ChildWrapper>
			</ExpanderRadio>

			<Subheader>{t.subheaders.advanced}</Subheader>
			<Expander
				title={t.source.preferredTrack}
				selectInfo={preferredTrack === 0 ? t.source.preferredTrack.top : t(preferredTrack).source.preferredTrack.ordinal}
				icon="preferred_track"
			>
				<Expander.Item title={t.source.preferredTrack.index} details={t.descriptions.source.preferredTrack.fillingInstructions}>
					<TextBox.Number value={[preferredTrack, setPreferredTrack]} decimalPlaces={0} />
				</Expander.Item>
				<ToggleSwitch
					on={belowAdjustmentTracks}
					details={t.descriptions.source.preferredTrack.belowAdjustmentTracks.versionRequest({ version: 16 })}
				>
					{t.source.preferredTrack.belowAdjustmentTracks}
				</ToggleSwitch>
			</Expander>
			<Expander title={t.source.afterCompletion} icon="post_processing">
				<ToggleSwitch on={removeSourceClips}>{t.source.afterCompletion.removeSourceClips}</ToggleSwitch>
				<ToggleSwitch on={selectSourceClips}>{t.source.afterCompletion.selectSourceClips}</ToggleSwitch>
				<ToggleSwitch on={selectGeneratedAudioClips}>{t.source.afterCompletion.selectGeneratedAudioClips}</ToggleSwitch>
				<ToggleSwitch on={selectGeneratedVideoClips}>{t.source.afterCompletion.selectGeneratedVideoClips}</ToggleSwitch>
			</Expander>
			<SettingsCardToggleSwitch title={t.source.randomOffsetForTracks} details={t.descriptions.source.randomOffsetForTracks} icon="dice" on={randomOffsetForTracks} />
			<DragToImport>{t.titles.source}</DragToImport>
		</div>
	);
}
