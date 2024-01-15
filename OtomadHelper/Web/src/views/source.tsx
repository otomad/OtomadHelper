export default function Source() {
	const [source, setSource] = useState("trackEvent");
	const [startTime, setStartTime] = useState("projectStart");
	const startTimes = [
		{ id: "projectStart", name: t.source.generateAtBegin },
		{ id: "cursor", name: t.source.generateAtCursor },
		{ id: "custom", name: t.custom },
	];

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={[source, setSource]}>
					<TabItem id="trackEvent" icon="placeholder">{t.source.trackEvent}</TabItem>
					<TabItem id="projectMedia" icon="placeholder">{t.source.projectMedia}</TabItem>
					<TabItem id="browseFile" icon="placeholder">{t.source.browseFile}</TabItem>
				</TabBar>
				<TestThumbnail />
			</Card>
			<Expander heading={t.source.trim} caption={t.descriptions.source.trim} />
			<Expander heading={t.source.startTime} caption={t.descriptions.source.startTime} checkInfo={startTimes.find(item => item.id === startTime)?.name}>
				{startTimes.map(item =>
					<RadioButton value={[startTime, setStartTime]} id={item.id} key={item.id}>{item.name}</RadioButton>)}
			</Expander>
			<Subheader>{t.subheader.moreOptions}</Subheader>
			<Expander heading={t.subheader.advanced} expanded>
				<ToggleSwitch on={[true]}>{t.source.belowTopAdjustmentTracks}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.removeSourceEventsAfterCompletion}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.selectAllEventsGenerated}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.source.randomOffsetForTracks}</ToggleSwitch>
			</Expander>
		</div>
	);
}
