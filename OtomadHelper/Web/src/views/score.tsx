const bpmUsings = [
	{ id: "dynamicMidi", name: t.score.dynamicMidiTempo },
	{ id: "midi", name: t.score.midiTempo },
	{ id: "project", name: t.score.projectTempo },
	{ id: "custom", name: t.custom },
];
const constraintNoteLengths = ["unconstrainted", "constraintMaxLength", "constraintFixedLength"];
const encodings = ["ANSI", "UTF-8", "Shift_JIS", "GBK", "Big5", "KS_C_5601-1987", "Windows-1252", "Macintosh"];

export default function Score() {
	const [format, setFormat] = useState("midi");
	const [bpmUsing, setBpmUsing] = useState("dynamicMidi");
	const timeSignature = "4/4";
	const [constraintNoteLength, setConstraintNoteLength] = useState("unconstrainted");
	const [encoding, setEncoding] = useState("ANSI");

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
			<ExpanderRadio
				heading={t.score.encoding}
				caption={t.descriptions.score.encoding}
				items={encodings}
				value={[encoding, setEncoding]}
				idField
				nameField
			/>
			<ExpanderRadio
				heading={t.score.bpm}
				caption={t.descriptions.score.bpm}
				items={bpmUsings}
				value={[bpmUsing, setBpmUsing]}
				idField="id"
				nameField="name"
			/>
			<SettingsCard heading={t.score.timeSignature} trailingIcon="">{timeSignature}</SettingsCard>
			<ExpanderRadio
				heading={t.score.constraint}
				caption={t.descriptions.score.constraint}
				items={constraintNoteLengths}
				value={[constraintNoteLength, setConstraintNoteLength]}
				idField
				nameField={item => t.score[item]}
				checkInfoCondition={t.score[constraintNoteLength]}
			/>
			<Subheader>{t(2).titles.track}</Subheader>
		</div>
	);
}
