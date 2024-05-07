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
					<TabBar.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</TabBar.Item>
					<TabBar.Item id="projectMedia" icon="media">{t.source.projectMedia}</TabBar.Item>
					<TabBar.Item id="browseFile" icon="open_file">{t.source.browseFile}</TabBar.Item>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Expander title={t.source.trim} details={t.descriptions.source.trim} icon="trim" />
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
			/>

			<Subheader>{t.subheaders.moreOptions}</Subheader>
			<Expander title={t.subheaders.advanced} expanded icon="more">
				<ToggleSwitch
					on={belowTopAdjustmentTracks}
					details={<Preserves>{t.descriptions.source.belowTopAdjustmentTracks + "\n" + t.descriptions.source.belowTopAdjustmentTracks.versionRequest({ version: 16 })}</Preserves>}
				>
					{t.source.belowTopAdjustmentTracks}
				</ToggleSwitch>
				<ToggleSwitch on={removeSourceEventsAfterCompletion}>{t.source.removeSourceEventsAfterCompletion}</ToggleSwitch>
				<ToggleSwitch on={selectAllEventsGenerated}>{t.source.selectAllEventsGenerated}</ToggleSwitch>
				<ToggleSwitch on={randomOffsetForTracks}>{t.source.randomOffsetForTracks}</ToggleSwitch>
			</Expander>
			<DragToImport>{t.titles.source}</DragToImport>
		</div>
	);
}
