export /* internal */ const bpmUsings = [
	{ id: "variableMidi", name: t.score.bpm.variableMidi, icon: "variable_midi" },
	{ id: "constantMidi", name: t.score.bpm.constantMidi, icon: "midi" },
	{ id: "project", name: t.score.bpm.project, icon: "veg_file" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* internal */ const constraintNoteLengths = [
	{ id: "none", icon: "prohibited" },
	{ id: "max", icon: "less_or_equal" },
	{ id: "fixed", icon: "equal" },
] as const;
export /* internal */ const encodings = ["ANSI", "UTF-8", "Shift_JIS", "GBK", "Big5", "KS_C_5601-1987", "Windows-1252", "Macintosh"] as const;
/** @deprecated Test only! */
const tracks = [
	{ channel: 1, name: "Lead", noteCount: 100, beginNote: "C5", pan: t.variableBeginWith({ first: t.score.pan.left }), isDrumKit: false, inst: "Sawtooth" },
	{ channel: 2, name: "Chords", noteCount: 50, beginNote: "C#5", pan: t.score.pan.right, isDrumKit: false, inst: "Strings" },
	{ channel: 10, name: "Drums", noteCount: 150, beginNote: "A3", pan: t.score.pan.center, isDrumKit: true, inst: "Piano" },
];

const TrackToolbar = styled.div`
	justify-content: space-between;
	margin-inline-start: 3px;

	&,
	.left .content {
		display: flex;
		gap: 8px;
		align-items: center;
	}

	.left .content {
		${tgs()} {
			scale: 0.85;
			opacity: 0;
		}

		.button {
			min-width: unset;

			.icon {
				font-size: 20px;
			}
		}
	}

	+ .items-view {
		.title {
			display: flex;
			gap: 8px;
			align-items: baseline;
			margin-block: -1px 3.5px;

			.badge-wrapper {
				display: inline-block;
			}
		}

		.details .row {
			&,
			> .contents {
				display: flex;
				flex-wrap: wrap;
				column-gap: 12px;

				> p {
					display: flex;
					column-gap: 5px;
					align-items: center;
					white-space: nowrap;

					.icon {
						font-size: 12px;
					}
				}
			}
		}
	}
`;

export default function Score() {
	const format = selectConfig(c => c.score.format);
	const encoding = selectConfig(c => c.score.encoding);
	const bpmUsing = selectConfig(c => c.score.bpmUsing);
	const [timeSignature] = selectConfig(c => c.score.timeSignature);
	const constraintNoteLength = selectConfig(c => c.score.constraintNoteLength);
	const [isMultipleSelectionMode, setIsMultipleSelectionMode] = selectConfig(c => c.score.isMultipleSelectionMode);
	const selectionMode = useStateSelector(
		[isMultipleSelectionMode, setIsMultipleSelectionMode],
		multiple => multiple ? "multiple" : "single",
		selectionMode => selectionMode === "multiple",
	);

	const [selectedTrack, setSelectedTrack] = useState<number | number[]>(0);
	useEffect(() => setSelectedTrack(selectedTrack => {
		if (isMultipleSelectionMode && !Array.isArray(selectedTrack))
			return [selectedTrack];
		else if (!isMultipleSelectionMode && Array.isArray(selectedTrack))
			return selectedTrack.at(-1) ?? 0;
		else // Actually the multiple selection mode doesn't be changed and unexpected trigger this effect.
			return selectedTrack;
	}), [isMultipleSelectionMode]);
	const selectAll = useSelectAll([selectedTrack, setSelectedTrack] as StateProperty<number[]>, tracks.map((_, index) => index));

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

			<Subheader>{t.subheaders.config}</Subheader>{/* TODO: This line should be deleted. */}
			<Expander title={t.source.trim} details={t.descriptions.score.trim} icon="trim" />
			<ExpanderRadio
				title={t.score.encoding}
				details={t.descriptions.score.encoding}
				icon="globe"
				items={encodings}
				value={encoding}
				idField
				nameField={value => value === "ANSI" ? t.systemDefault : value}
				checkInfoCondition={value => value === "ANSI" ? t.systemDefault : value}
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

			{!!tracks.length && (
				<>
					<Subheader>{t(tracks.length).score.musicalTrack}</Subheader>
					<TrackToolbar>
						<div className="left">
							<CssTransition in={isMultipleSelectionMode} unmountOnExit>
								<div className="content">
									<Checkbox value={selectAll}>{t.selectAll}</Checkbox>
									<Button subtle icon="invert_selection" onClick={selectAll[2]}>{t.invertSelection}</Button>
									<Badge>{(selectedTrack as number[]).length ?? 1}</Badge>
								</div>
							</CssTransition>
						</div>
						<Segmented current={selectionMode}>
							<Segmented.Item id="single" icon="single_select">{t.selectionMode.single}</Segmented.Item>
							<Segmented.Item id="multiple" icon="multiselect">{t.selectionMode.multiple}</Segmented.Item>
						</Segmented>
					</TrackToolbar>
					<ItemsView view="list" multiple={isMultipleSelectionMode} current={[selectedTrack, setSelectedTrack]}>
						{tracks.map((track, index) => (
							<ItemsView.Item
								key={index}
								id={index}
								details={(
									<>
										<SubgridLayout className="row" name="score-track-note-details">
											<p><Icon name="music_note" />{t.score.noteCount}{t.colon}{track.noteCount}</p>
											<p><Icon name="start_point" />{t.score.beginNote}{t.colon}{track.beginNote}</p>
											<p><Icon name="stereo" />{t.score.pan}{t.colon}{track.pan}</p>
										</SubgridLayout>
										<SubgridLayout className="row" name="score-track-note-details">
											{track.isDrumKit && <p><Icon name="drum" />{t.score.drumKit}</p>}
											<p className="span-to-end"><Icon name="score" />{t.score.instrument}{t.colon}{track.inst}</p>
										</SubgridLayout>
									</>
								)}
							>
								<SubgridLayout name="score-track-name">
									{track.channel != null && <div className="badge-wrapper"><Badge transitionOnAppear={false}>{track.channel}</Badge></div>}
									<span>{track.name}</span>
								</SubgridLayout>
							</ItemsView.Item>
						))}
					</ItemsView>
				</>
			)}

			<DragToImport>{t.titles.score}</DragToImport>
		</div>
	);
}
