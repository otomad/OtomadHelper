export /* internal */ const bpmUsings = [
	{ id: "variableMidi", name: t.score.bpm.variableMidi, icon: "variable_midi" },
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
					<TabBar.Item id="midi" icon="midi">{t.score.midi}</TabBar.Item>
					<TabBar.Item id="st" icon="ust">{t.score.st}</TabBar.Item>
					<TabBar.Item id="refOtherTracks" icon="ref_other_tracks">{t.score.refOtherTracks}</TabBar.Item>
					<TabBar.Item id="pureNotes" icon="quarter_note">{t.score.pureNotes}</TabBar.Item>
					<TabBar.Item id="tts" icon="narrator">{t.score.tts}</TabBar.Item>
				</TabBar>
			</Card>

			<Subheader>{t.subheaders.config}</Subheader>{/* TODO: 该行应该会被删除。 */}
			<Expander title={t.source.trim} details={t.descriptions.score.trim} icon="trim" />
			<ExpanderRadio
				title={t.score.encoding}
				details={t.descriptions.score.encoding}
				icon="globe"
				items={encodings}
				value={encoding}
				idField
				nameField
				checkInfoCondition
				detailsField={value => value === "ANSI" ? t.systemDefault : undefined}
			/>
			<ExpanderRadio
				title={t.score.bpm}
				details={t.descriptions.score.bpm}
				icon="speed"
				items={bpmUsings}
				value={bpmUsing as StateProperty<string>}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			/>
			<SettingsCard title={t.score.timeSignature} icon="health">{timeSignature}</SettingsCard>
			<ExpanderRadio
				title={t.score.constraint}
				details={t.descriptions.score.constraint}
				icon="constraint"
				items={constraintNoteLengths}
				value={constraintNoteLength as StateProperty<string>}
				view="tile"
				idField="id"
				nameField={t.score.constraint}
				iconField="icon"
			/>

			<Subheader>{t(2).titles.track}</Subheader>

			<DragToImport>{t.titles.score}</DragToImport>
		</div>
	);
}
