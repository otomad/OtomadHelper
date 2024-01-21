export /* internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;

export default function Source() {
	const source = selectConfig(c => c.source.source);
	const startTime = selectConfig(c => c.source.startTime);
	const belowTopAdjustmentTracks = selectConfig(c => c.source.belowTopAdjustmentTracks);
	const removeSourceEventsAfterCompletion = selectConfig(c => c.source.removeSourceEventsAfterCompletion);
	const selectAllEventsGenerated = selectConfig(c => c.source.selectAllEventsGenerated);
	const randomOffsetForTracks = selectConfig(c => c.source.randomOffsetForTracks);

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={source}>
					<TabItem id="trackEvent" icon="track_event">{t.source.trackEvent}</TabItem>
					<TabItem id="projectMedia" icon="media">{t.source.projectMedia}</TabItem>
					<TabItem id="browseFile" icon="open_file">{t.source.browseFile}</TabItem>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Expander heading={t.source.trim} caption={t.descriptions.source.trim} icon="trim" />
			{/* <Expander heading={t.source.startTime} caption={t.descriptions.source.startTime} icon="start_point">
				<ListView view="tile" current={startTime}>
					{startTimes.map(item =>
						<ListViewItem id={item.id} key={item.id} icon={item.icon}>{item.name}</ListViewItem>)}
				</ListView>
			</Expander> */}
			<ExpanderRadio
				heading={t.source.startTime}
				caption={t.descriptions.source.startTime}
				icon="start_point"
				items={startTimes}
				value={startTime as StateProperty<string>}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			/>

			<Subheader>{t.subheader.moreOptions}</Subheader>
			<Expander heading={t.subheader.advanced} expanded icon="more">
				<ToggleSwitch on={belowTopAdjustmentTracks}>{t.source.belowTopAdjustmentTracks}</ToggleSwitch>
				<ToggleSwitch on={removeSourceEventsAfterCompletion}>{t.source.removeSourceEventsAfterCompletion}</ToggleSwitch>
				<ToggleSwitch on={selectAllEventsGenerated}>{t.source.selectAllEventsGenerated}</ToggleSwitch>
				<ToggleSwitch on={randomOffsetForTracks}>{t.source.randomOffsetForTracks}</ToggleSwitch>
			</Expander>
		</div>
	);
}
