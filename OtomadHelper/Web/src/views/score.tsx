export /* @internal */ const bpmUsings = [
	{ id: "variableMidi", name: t.score.bpm.variableMidi, icon: "variable_midi" },
	{ id: "constantMidi", name: t.score.bpm.constantMidi, icon: "midi" },
	{ id: "project", name: t.score.bpm.project, icon: "veg_file" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const constraintNoteLengths = [
	{ id: "none", icon: "prohibited" },
	{ id: "max", icon: "less_or_equal" },
	{ id: "fixed", icon: "equal" },
] as const;
export /* @internal */ const encodings = ["ANSI", "UTF-8", "Shift_JIS", "GBK", "Big5", "KS_C_5601-1987", "Windows-1252", "Macintosh"] as const;
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

	> .segmented {
		flex-shrink: 0;
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
	const {
		format, encoding, constraintNoteLength, trimStart, trimEnd, bpmUsing, customBpm,
		timeSignature: [timeSignature],
		isMultiple: [isMultiple, setIsMultiple],
	} = selectConfig(c => c.score);
	const { enabled: [ytpEnabled] } = selectConfig(c => c.ytp);
	const selectionMode = useSelectionMode([isMultiple, setIsMultiple]);

	const [selectedTrack, setSelectedTrack] = useState<number | number[]>(0);
	useEffect(() => setSelectedTrack(selectedTrack => {
		if (isMultiple && !Array.isArray(selectedTrack))
			return [selectedTrack];
		else if (!isMultiple && Array.isArray(selectedTrack))
			return selectedTrack.at(-1) ?? 0;
		else // Actually the multiple selection mode doesn't be changed and unexpected trigger this effect.
			return selectedTrack;
	}), [isMultiple]);
	const selectAll = useSelectAll([selectedTrack, setSelectedTrack] as StateProperty<number[]>, tracks.map((_, index) => index));

	return (
		<div className="container">
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.score.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}

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
			<Expander title={t.source.trim} details={t.descriptions.source.trim} icon="trim">
				<ExpanderChildTrim.Timecode start={trimStart} end={trimEnd} />
			</Expander>
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
			>
				<Expander.ChildWrapper>
					<TextBox.Number value={customBpm} style={{ width: "200px" }} onChanging={() => bpmUsing[1]("custom")} />
				</Expander.ChildWrapper>
			</ExpanderRadio>
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
							<CssTransition in={isMultiple} unmountOnExit>
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
					<ItemsView view="list" multiple={isMultiple} current={[selectedTrack, setSelectedTrack]}>
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
