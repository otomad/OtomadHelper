import { redirectIcon } from "../ShellPage";

export /* @internal */ const tempoUsings = [
	{ id: "variableScore", name: t.score.tempo.variableScore, icon: "score_variable" },
	{ id: "constantScore", name: t.score.tempo.constantScore, icon: "score" },
	{ id: "project", name: t.score.tempo.project, icon: "document_vegas" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const constraintNoteLengthTypes = [
	{ id: "none", icon: "prohibited" },
	{ id: "max", icon: "less_or_equal" },
	{ id: "fixed", icon: "equal" },
] as const;
export /* @internal */ const multipleSelectTrackItems = Object.freeze(["audio", "visual", "sonar", "lyrics"] as const);
const allMultipleSelectTrackItemSet = new Set(multipleSelectTrackItems);
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
			min-inline-size: unset;

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

const MultipleSelectTrackItemsContainer = styled.div`
	place-self: stretch;
	margin-inline-start: auto;
	transition: ${fallbackTransitions} !important;

	button {
		block-size: 100%;
		min-inline-size: unset;
	}

	${tgs()} {
		translate: 34px;
		opacity: 0;
	}
`;

export default function Score() {
	const {
		format, encoding, constraintNoteLengthType, constraintNoteLengthValue, trimStart, trimEnd, tempoUsing, customTempo,
		timeSignature: [timeSignature],
		selectedTrack: [selectedTrack, setSelectedTrack], multipleSelectTrackItems: [selectTrackItems, _setSelectTrackItems],
	} = selectConfig(c => c.score);
	const { enabled: [ytpEnabled] } = selectConfig(c => c.ytp);

	const setSelectTrackItems = (recipe: (draft: typeof selectTrackItems) => void) => _setSelectTrackItems(produce(recipe));

	const [isMultiple, setIsMultiple] = useStateSelector(
		[selectedTrack, setSelectedTrack],
		selectedTrack => typeof selectedTrack !== "number",
		(isMultiple, selectedTrack) => {
			if (isMultiple && typeof selectedTrack === "number") {
				const items = {} as typeof selectTrackItems;
				for (const index of tracks.keys())
					items[index] = index === selectedTrack ? new Set(allMultipleSelectTrackItemSet) : new Set();
				_setSelectTrackItems(items);
				return [selectedTrack];
			} else if (!isMultiple && typeof selectedTrack !== "number")
				return selectedTrack.at(-1) ?? 0;
			else // Actually the multiple selection mode doesn't be changed and unexpected trigger this effect.
				return selectedTrack;
		},
	);
	useMountEffect(() => {
		setSelectTrackItems(items => {
			for (const index of tracks.keys())
				if (!(items[index] instanceof Set)) items[index] = new Set();
			new Set(Object.keys(items)).difference(new Set(Object.keys(tracks))).forEach(index => delete items[+index]);
		});
	});

	const indeterminatenesses = typeof selectedTrack === "number" ? undefined! : selectedTrack.filter(index => !selectTrackItems[index]?.isSupersetOf(allMultipleSelectTrackItemSet));
	function handleTrackItemsClick(index: number, item: typeof multipleSelectTrackItems[number]) {
		setSelectTrackItems(tracks => {
			tracks[index]?.toggle(item);
			setSelectedTrack?.(produce(selectedTrack => {
				if (typeof selectedTrack !== "number")
					if (!tracks[index].size) selectedTrack.removeItem(index);
					else selectedTrack.pushUniquely(index);
			}));
		});
	}
	function handleTrackClick(_index: PropertyKey, selected: CheckState) {
		if (!isMultiple) return;
		const index = _index as number;
		setSelectTrackItems(items => {
			if (selected !== "unchecked") items[index].clear();
			else items[index].adds(...multipleSelectTrackItems);
		});
	}

	const selectionMode = useSelectionMode([isMultiple, setIsMultiple]);
	// const selectAll = useSelectAll([selectedTrack, setSelectedTrack] as StateProperty<number[]>, [...tracks.keys()]);
	const selectAll = (() => {
		const selected = selectedTrack as number[], setSelected = setSelectedTrack, allSelection = [...tracks.keys()];
		return [
			!selected.length ? "unchecked" : selected.length - indeterminatenesses.length === allSelection.length ? "checked" : "indeterminate",
			(checkState: CheckState) => {
				if (checkState === "unchecked") {
					setSelected([]);
					setSelectTrackItems(items => Object.values(items).forEach(item => item.clear()));
				} else if (checkState === "checked") {
					setSelected(allSelection.slice());
					setSelectTrackItems(items => Object.values(items).forEach(item => item.adds(...multipleSelectTrackItems)));
				}
			},
			() => {
				const selectedDraft: typeof selected = [];
				setSelectTrackItems(items => Object.entries(items).forEach(([index, set]) => {
					multipleSelectTrackItems.forEach(item => set.toggle(item));
					if (set.size) selectedDraft.push(+index);
				}));
				setSelected(selectedDraft);
			},
		] as unknown as StatePropertyNonNull<CheckState> & { 2: () => void };
	})();

	return (
		<div className="container">
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.score.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}

			<Card className="media-pool">
				<TabBar current={format}>
					<TabBar.Item id="midi" icon="midi">{t.score.midi}</TabBar.Item>
					<TabBar.Item id="singthesis" icon="ust">{t.score.singthesis}</TabBar.Item>
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
				title={t.score.tempo}
				details={t.descriptions.score.tempo}
				icon="speed"
				items={tempoUsings}
				value={tempoUsing}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			>
				<CustomItem current={tempoUsing}>
					{setToCustom => <TextBox.Number value={customTempo} onChanging={setToCustom} suffix={t.units.beatsPerMinute} />}
				</CustomItem>
			</ExpanderRadio>
			<SettingsCard title={t.score.timeSignature} icon="health">{timeSignature}</SettingsCard>
			<ExpanderRadio
				title={t.score.constraint}
				details={t.descriptions.score.constraint}
				icon="constraint"
				items={constraintNoteLengthTypes}
				value={constraintNoteLengthType}
				view="tile"
				idField="id"
				nameField={t.score.constraint}
				iconField="icon"
			>
				<Expander.ChildWrapper>
					<TimecodeBox timecode={constraintNoteLengthValue} />
				</Expander.ChildWrapper>
			</ExpanderRadio>

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
					<ItemsView view="list" multiple={isMultiple} current={[selectedTrack, setSelectedTrack]} indeterminatenesses={indeterminatenesses}>
						{tracks.map((track, index) => (
							<ItemsView.Item
								key={index}
								id={index}
								onClick={handleTrackClick}
								details={(
									<>
										<SubgridLayout className="row" name="score-track-note-details">
											<p><Icon name="music_note" />{t.score.noteCount}{t.colon}{track.noteCount}</p>
											<p><Icon name="start_point" />{t.score.beginNote}{t.colon}{track.beginNote}</p>
											<p><Icon name="stereo" />{t.score.pan}{t.colon}{track.pan}</p>
										</SubgridLayout>
										<SubgridLayout className="row" name="score-track-note-details">
											{track.isDrumKit && <p><Icon name="drum" />{t.score.drumKit}</p>}
											<p className="span-to-end"><Icon name="instrument" />{t.score.instrument}{t.colon}{track.inst}</p>
										</SubgridLayout>
									</>
								)}
								actions={(
									<CssTransition in={isMultiple} unmountOnExit>
										<MultipleSelectTrackItemsContainer>
											{multipleSelectTrackItems.map(item => (
												<Tooltip key={item} placement="y" title={t.titles[item]}>
													<Button
														icon={redirectIcon(item)}
														subtle
														accent={selectTrackItems[index]?.has(item) ? true : "neutral"}
														onClick={() => handleTrackItemsClick(index, item)}
													/>
												</Tooltip>
											))}
										</MultipleSelectTrackItemsContainer>
									</CssTransition>
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
