export /* internal */ const bpmUsings = [
	{ id: "dynamicMidi", name: t.score.bpm.dynamicMidi, icon: "dynamic_midi" },
	{ id: "midi", name: t.score.bpm.midi, icon: "midi" },
	{ id: "project", name: t.score.bpm.project, icon: "veg_file" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* internal */ const constraintNoteLengths = [
	{ id: "none", icon: "prohibited" },
	{ id: "max", icon: "less_or_equal" },
	{ id: "fixed", icon: "equal" },
] as const;
export /* internal */ const encodings = ["ANSI", "UTF-8", "Shift_JIS", "GBK", "Big5", "KS_C_5601-1987", "Windows-1252", "Macintosh"] as const;

export default function Score() {
	const format = selectConfig(c => c.score.format);
	const encoding = selectConfig(c => c.score.encoding);
	const bpmUsing = selectConfig(c => c.score.bpmUsing);
	const [timeSignature] = selectConfig(c => c.score.timeSignature);
	const constraintNoteLength = selectConfig(c => c.score.constraintNoteLength);

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={format}>
					<TabItem id="midi" icon="midi">{t.score.midi}</TabItem>
					<TabItem id="ust" icon="ust">{t.score.ust}</TabItem>
					<TabItem id="refOtherTracks" icon="ref_other_tracks">{t.score.refOtherTracks}</TabItem>
					<TabItem id="pureNotes" icon="quarter_note">{t.score.pureNotes}</TabItem>
				</TabBar>
			</Card>

			<Subheader>{t.subheader.config}</Subheader>
			<Expander heading={t.source.trim} caption={t.descriptions.score.trim} icon="trim" />
			<ExpanderRadio
				heading={t.score.encoding}
				caption={t.descriptions.score.encoding}
				icon="globe"
				items={encodings}
				value={encoding}
				idField
				nameField
			/>
			<ExpanderRadio
				heading={t.score.bpm}
				caption={t.descriptions.score.bpm}
				icon="speed"
				items={bpmUsings}
				value={bpmUsing as StateProperty<string>}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			/>
			<SettingsCard heading={t.score.timeSignature} icon="health">{timeSignature}</SettingsCard>
			<ExpanderRadio
				heading={t.score.constraint}
				caption={t.descriptions.score.constraint}
				icon="constraint"
				items={constraintNoteLengths}
				value={constraintNoteLength as StateProperty<string>}
				view="tile"
				idField="id"
				nameField={t.score.constraint}
				iconField="icon"
			/>

			<Subheader>{t(2).titles.track}</Subheader>
		</div>
	);
}