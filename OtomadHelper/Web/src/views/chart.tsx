export default function Midi() {
	const [format, setFormat] = useState("midi");

	return (
		<div className="container">
			<TabBar current={[format, setFormat]}>
				<TabItem id="midi" icon="placeholder">{t.midi}</TabItem>
				<TabItem id="ust" icon="placeholder">{t.ust}</TabItem>
				<TabItem id="refOtherTracks" icon="placeholder">{t.refOtherTracks}</TabItem>
				<TabItem id="pureNotes" icon="placeholder">{t.pureNotes}</TabItem>
			</TabBar>
		</div>
	);
}
