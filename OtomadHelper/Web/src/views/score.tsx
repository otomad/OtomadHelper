export default function Score() {
	const [format, setFormat] = useState("midi");
	const [bpmUsing, setBpmUsing] = useState("dynamicMidi");
	const bpmUsings = [
		{ id: "dynamicMidi", name: t.score.dynamicMidiTempo },
		{ id: "midi", name: t.score.midiTempo },
		{ id: "project", name: t.score.projectTempo },
		{ id: "custom", name: t.custom },
	];
	const timeSignature = "4/4";
	const [constraintNoteLength, setConstraintNoteLength] = useState("unconstrainted");
	const constraintNoteLengths = ["unconstrainted", "constraintMaxLength", "constraintFixedLength"];
	const [encoding, setEncoding] = useState("UTF-8");
	const encodings = ["UTF-8", "GBK", "Shift-JIS"];

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={[format, setFormat]}>
					<TabItem id="midi" icon="placeholder">{t.score.midi}</TabItem>
					<TabItem id="ust" icon="placeholder">{t.score.ust}</TabItem>
					<TabItem id="refOtherTracks" icon="placeholder">{t.score.refOtherTracks}</TabItem>
					<TabItem id="pureNotes" icon="placeholder">{t.score.pureNotes}</TabItem>
				</TabBar>
			</Card>
			<Subheader>{t.subheader.config}</Subheader>
			<Expander heading={t.source.trim} caption={t.descriptions.score.trim} />
			<Expander heading={t.score.encoding} caption={t.descriptions.score.encoding} checkInfo={encoding}>
				{encodings.map(item =>
					<RadioButton value={[encoding, setEncoding]} id={item} key={item}>{item}</RadioButton>)}
			</Expander>
			<Expander heading={t.score.bpm} caption={t.descriptions.score.bpm} checkInfo={bpmUsings.find(item => item.id === bpmUsing)?.name}>
				{bpmUsings.map(item =>
					<RadioButton value={[bpmUsing, setBpmUsing]} id={item.id} key={item.id}>{item.name}</RadioButton>)}
			</Expander>
			<SettingsCard heading={t.score.timeSignature} trailingIcon="">{timeSignature}</SettingsCard>
			<Expander heading={t.score.constraint} caption={t.descriptions.score.constraint} checkInfo={t.score[constraintNoteLength]}>
				{constraintNoteLengths.map(item =>
					<RadioButton value={[constraintNoteLength, setConstraintNoteLength]} id={item} key={item}>{t.score[item]}</RadioButton>)}
			</Expander>
		</div>
	);
}
