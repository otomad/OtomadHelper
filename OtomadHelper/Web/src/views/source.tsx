const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart },
	{ id: "cursor", name: t.source.startTime.cursor },
	{ id: "custom", name: t.custom },
];

export default function Source() {
	const [source, setSource] = useState("trackEvent");
	const [startTime, setStartTime] = useState("projectStart");

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={[source, setSource]}>
					<TabItem id="trackEvent" icon="track_event">{t.source.trackEvent}</TabItem>
					<TabItem id="projectMedia" icon="media">{t.source.projectMedia}</TabItem>
					<TabItem id="browseFile" icon="open_file">{t.source.browseFile}</TabItem>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Expander heading={t.source.trim} caption={t.descriptions.source.trim} icon="trim" />
			<ExpanderRadio
				heading={t.source.startTime}
				caption={t.descriptions.source.startTime}
				icon="start_point"
				items={startTimes}
				value={[startTime, setStartTime]}
				idField="id"
				nameField="name"
			/>

			<Subheader>{t.subheader.moreOptions}</Subheader>
			<Expander heading={t.subheader.advanced} expanded icon="more">
				<ToggleSwitch on={[true]}>{t.source.belowTopAdjustmentTracks}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.removeSourceEventsAfterCompletion}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.selectAllEventsGenerated}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.randomOffsetForTracks}</ToggleSwitch>
			</Expander>
		</div>
	);
}
