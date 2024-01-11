export default function Source() {
	const [source, setSource] = useState("trackEvent");
	const [startTime, setStartTime] = useState("projectStart");
	const startTimes = [
		{ id: "projectStart", name: t.generateAtBegin },
		{ id: "cursor", name: t.generateAtCursor },
		{ id: "custom", name: t.custom },
	];

	return (
		<div className="container">
			<TabBar current={[source, setSource]}>
				<TabItem id="trackEvent" icon="placeholder">{t.trackEvent}</TabItem>
				<TabItem id="projectMedia" icon="placeholder">{t.projectMedia}</TabItem>
				<TabItem id="browseFile" icon="placeholder">{t.browseFile}</TabItem>
			</TabBar>
			<TestThumbnail />
			<SettingsCard heading={t.trim} caption={t.descriptions.trim} type="button" />
			<Expander heading={t.startTime} caption={t.descriptions.startTime} checkInfo={startTimes.find(item => item.id === startTime)?.name}>
				{startTimes.map(item =>
					<RadioButton value={[startTime, setStartTime]} id={item.id} key={item.id}>{item.name}</RadioButton>)}
			</Expander>
			<Subheader>{t.moreOptions}</Subheader>
			<Expander heading={t.advanced} expanded>
				<ToggleSwitch on={[true]}>{t.belowTopAdjustmentTracks}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.removeSourceEventsAfterCompletion}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.selectAllEventsGenerated}</ToggleSwitch>
				<ToggleSwitch on={[false]}>{t.randomOffsetForTracks}</ToggleSwitch>
			</Expander>
		</div>
	);
}
