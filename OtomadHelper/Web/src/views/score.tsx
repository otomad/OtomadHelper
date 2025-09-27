import { redirectIcon } from "../ShellPage";

export /* @internal */ const tempoUsings = [
	{ id: "variableScore", name: t.score.tempo.variableScore, icon: "score_variable" },
	{ id: "constantScore", name: t.score.tempo.constantScore, icon: "score" },
	{ id: "project", name: t.score.tempo.project, icon: "document_vegas" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const constrainNoteLengthTypes = [
	{ id: "none", icon: "prohibited" },
	{ id: "max", icon: "less_or_equal" },
	{ id: "fixed", icon: "equal" },
	{ id: "percentage", icon: "percent" },
	{ id: "fixedDecrement", icon: "line_horizontal" },
] as const;
export /* @internal */ const multipleSelectTrackItems = Object.freeze(["audio", "visual", "sonar", "lyrics"] as const);
const allMultipleSelectTrackItemSet = new Set(multipleSelectTrackItems);
export /* @internal */ const encodings = ["ANSI", "UTF-8", "Shift_JIS", "GBK", "Big5", "KS_C_5601-1987", "Windows-1252", "Macintosh"] as const;
export /* @internal */ const trackAndChannel = ["track", "channel"] as const;
/** @deprecated Test only! */
const tracks = [
	{ channel: 1, name: "Lead", noteCount: 100, beginNote: 60, pan: "leftVariable", isDrumKit: false, inst: 82 },
	{ channel: 2, name: "Chords", noteCount: 50, beginNote: 61, pan: "right", isDrumKit: false, inst: 51 },
	{ channel: 10, name: "Drums", noteCount: 150, beginNote: 45, pan: "center", isDrumKit: true, inst: 1 },
];

// #region Styles
const TrackToolbar = styled.div`
	justify-content: space-between;
	margin-inline: 16px;

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
	display: flex;
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

		&:dir(rtl) {
			translate: -34px;
		}
	}
`;
// #endregion

export default function Score() {
	const {
		format, encoding, trimStart, trimEnd, tempoUsing, customTempo,
		timeSignature: [timeSignature], trackOrChannel,
		selectedTrack: [selectedTrack, setSelectedTrack], multipleSelectTrackItems: [selectTrackItems, _setSelectTrackItems],
	} = useSelectConfig(c => c.score);
	const { type: constrainNoteLengthType, min: constrainNoteLengthMin, ...constrainNoteLengthValues } = useSelectConfig(c => c.score.constrainNoteLength);
	const { enabled: [ytpEnabled] } = useSelectConfig(c => c.ytp);
	const meta = metas.score;

	const setSelectTrackItems = (recipe: (draft: typeof selectTrackItems) => void) => _setSelectTrackItems(produce(recipe));

	const getAllMultipleSelectTrackItemSet = (index: number | typeof tracks[number]) => {
		const all = new Set(allMultipleSelectTrackItemSet), track = typeof index === "number" ? tracks[index] : index;
		if (track && !track.isDrumKit) all.delete("sonar");
		return all;
	};

	const [isMultiple, setIsMultiple] = useStateSelector(
		[selectedTrack, setSelectedTrack],
		selectedTrack => typeof selectedTrack !== "number",
		(isMultiple, selectedTrack) => {
			if (isMultiple && typeof selectedTrack === "number") {
				const items = {} as typeof selectTrackItems;
				for (const index of tracks.keys())
					items[index] = index === selectedTrack ? new Set(getAllMultipleSelectTrackItemSet(index)) : new Set();
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

	const indeterminatenesses = typeof selectedTrack === "number" ? [] : selectedTrack.filter(index =>
		!selectTrackItems[index]?.isSupersetOf(getAllMultipleSelectTrackItemSet(index)));
	function handleTrackItemsClick(index: number, item: typeof multipleSelectTrackItems[number]) {
		setSelectTrackItems(tracks => {
			tracks[index]?.toggle(item);
			setSelectedTrack?.(produce(selectedTrack => {
				if (typeof selectedTrack !== "number")
					if (tracks[index].size === 0) selectedTrack.removeItem(index);
					else selectedTrack.pushUniquely(index);
			}));
		});
	}
	function handleTrackClick(_index: PropertyKey, selected: CheckState) {
		if (!isMultiple) return;
		const index = _index as number;
		setSelectTrackItems(items => {
			if (selected !== "unchecked") items[index].clear();
			else items[index].adds(...getAllMultipleSelectTrackItemSet(index));
		});
	}

	const selectionMode = useSelectionMode([isMultiple, setIsMultiple]);
	// const selectAll = useSelectAll([selectedTrack, setSelectedTrack] as StateProperty<number[]>, [...tracks.keys()]);
	const selectAll = (() => {
		const selected = selectedTrack as number[], setSelected = setSelectedTrack, allSelection = [...tracks.keys()];
		return [
			selected.length === 0 ? "unchecked" : selected.length - indeterminatenesses.length === allSelection.length ? "checked" : "indeterminate",
			(checkState: CheckState) => {
				if (checkState === "unchecked") {
					setSelected([]);
					setSelectTrackItems(items => Object.values(items).forEach(item => item.clear()));
				} else if (checkState === "checked") {
					setSelected(allSelection.slice());
					setSelectTrackItems(items => Object.entries(items).forEach(([index, set]) => set.adds(...getAllMultipleSelectTrackItemSet(+index))));
				}
			},
			() => {
				const selectedDraft: typeof selected = [];
				setSelectTrackItems(items => Object.entries(items).forEach(([index, set]) => {
					getAllMultipleSelectTrackItemSet(+index).forEach(item => set.toggle(item));
					if (set.size > 0) selectedDraft.push(+index);
				}));
				setSelected(selectedDraft);
			},
		] as unknown as StatePropertyNonNull<CheckState> & { 2: () => void };
	})();

	return (
		<div className="container">
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.score.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}

			<Card className="media-pool" data-anchor={meta.from.meta.cssPath}>
				<TabBar current={format} aria-label={t.score.from}>
					<TabBar.Item id="midi" icon="midi">{t.score.midi}</TabBar.Item>
					<TabBar.Item id="singthesis" icon="ust">{t.score.singthesis}</TabBar.Item>
					<TabBar.Item id="refOtherTracks" icon="ref_other_tracks">{t.score.refOtherTracks}</TabBar.Item>
					<TabBar.Item id="pureNotes" icon="quarter_note">{t.score.pureNotes}</TabBar.Item>
					<TabBar.Item id="tts" icon="narrator">{t.score.tts}</TabBar.Item>
				</TabBar>
			</Card>

			<Setting meta={meta.trim}>
				<ExpanderChildTrim.Timecode start={trimStart} end={trimEnd} />
			</Setting>
			<Setting
				meta={meta.encoding}
				items={encodings}
				value={encoding}
				idField
				nameField={value => value === "ANSI" ? t.systemDefault : value}
				checkInfoCondition={value => value === "ANSI" ? t.systemDefault : value}
			/>
			<Setting
				meta={meta.tempo}
				items={tempoUsings}
				value={tempoUsing}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			>
				<CustomItem current={tempoUsing}>
					{setToCustom => <TextBox.Number value={customTempo} onChanging={setToCustom} suffix={t.units.beatPerMinute} />}
				</CustomItem>
			</Setting>
			<Setting meta={meta.timeSignature} actions={timeSignature} />
			<Setting
				meta={meta.constrain}
				items={constrainNoteLengthTypes}
				value={constrainNoteLengthType}
				view="tile"
				idField="id"
				nameField={t.score.constrain}
				detailsField={t.descriptions.score.constrain}
				iconField="icon"
			>
				{constrainNoteLengthType[0] !== "none" && (
					<Expander.Item
						title={t.score.constrain[constrainNoteLengthType[0]]}
						details={t.descriptions.score.constrain[constrainNoteLengthType[0]]}
						icon={constrainNoteLengthTypes.find(i => i.id === constrainNoteLengthType[0])!.icon}
						nowrap
					>
						{constrainNoteLengthType[0] === "percentage" ?
							<TextBox.Number value={constrainNoteLengthValues[constrainNoteLengthType[0]]} suffix={t.units.percent} min={0} max={100} decimalPlaces={2} /> :
							<TimecodeBox value={constrainNoteLengthValues[constrainNoteLengthType[0]]} />}
					</Expander.Item>
				)}
				{constrainNoteLengthType[0].in("percentage", "fixedDecrement") && (
					<Expander.Item title={t.score.constrain.min} details={t.descriptions.score.constrain.min} icon="greater_or_equal" nowrap>
						<TimecodeBox value={constrainNoteLengthMin} />
					</Expander.Item>
				)}
			</Setting>
			<Setting meta={meta.parser} actions={<ComboBox ids={["otomadHelper"]} options={["Otomad Helper"]} current={["otomadHelper"]} />} />
			<Setting
				meta={meta.trackOrChannel}
				disabled={format[0] !== "midi"}
				actions={(
					<Segmented current={trackOrChannel}>
						{trackAndChannel.map(option => {
							const icon: DeclaredIcons = option === "channel" ? "launchpad" : "layer";
							const label = option === "channel" ? t.score.channel : t.score.musicalTrack;
							return <Segmented.Item id={option} key={option} icon={icon}>{label}</Segmented.Item>;
						})}
					</Segmented>
				)}
			/>

			{tracks.length > 0 && (
				<>
					<Subheader>{withObject(t(tracks.length).score, t => trackOrChannel[0] === "channel" ? t.channel : t.musicalTrack)}</Subheader>
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
						{tracks.map((track, index) => {
							let beginNote = new Pitch(track.beginNote).spn;
							if (track.isDrumKit) beginNote = tf.shared.midi.percussions[track.beginNote] ?? `${tf.shared.midi.unknown} (${beginNote})`;
							const inst = tf.shared.midi.instruments[track.inst] ?? tf.shared.midi.unknown;
							// 	{ channel: 1, name: "Lead", noteCount: 100, beginNote: "C5", pan: t.variableBeginWith({ first: t.score.pan.left }), isDrumKit: false, inst: "Sawtooth" },
							const pan = !track.pan.endsWith("Variable") ? t.score.pan[track.pan] : t.variableBeginWith({ first: t.score.pan[track.pan.replaceEnd("Variable")] });
							return (
								<ItemsView.Item
									key={index}
									id={index}
									onClick={handleTrackClick}
									details={(
										<>
											<SubgridLayout className="row" name="score-track-note-details">
												<p><Icon name="music_note" />{t.score.noteCount}{t.colon}{track.noteCount}</p>
												<p><Icon name="start_point" />{t.score.beginNote}{t.colon}{beginNote}</p>
												<p><Icon name="stereo" />{t.score.pan}{t.colon}{pan}</p>
											</SubgridLayout>
											<SubgridLayout className="row" name="score-track-note-details">
												{track.isDrumKit && <p><Icon name="drum" />{t.score.drumKit}</p>}
												<p className="span-to-end"><Icon name="instrument" />{t.score.instrument}{t.colon}{inst}</p>
											</SubgridLayout>
										</>
									)}
									actions={(
										<CssTransition in={isMultiple} unmountOnExit>
											<MultipleSelectTrackItemsContainer>
												{Array.from(getAllMultipleSelectTrackItemSet(track), item => !track.isDrumKit && item === "sonar" ? undefined : (
													<Tooltip key={item} placement="y" title={t.titles[item]}>
														<ToggleButton
															icon={redirectIcon(item)}
															appearance="subtle"
															checked={[selectTrackItems[index]?.has(item)]}
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
							);
						})}
					</ItemsView>
				</>
			)}

			<DragToImport>{t.titles.score}</DragToImport>
		</div>
	);
}
// TODO: 修剪 → 筛选
// MIDI 裁剪改成筛选/过滤，然后新增选取音域范围、指定倍数（奇偶性）的音符。
// 筛选/过滤只提供小于或小于等于，可只填写一边（左侧0，右侧无穷大），两者填一样表示等于。
