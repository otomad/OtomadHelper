export /* @internal */ const sourceFromEnums = ["trackEvent", "projectMedia", "browseFile"] as const;
export /* @internal */ const StartTimes = Enum({
	projectStart: { label: t.source.startTime.projectStart, icon: "arrow_export" },
	cursor: { label: t.source.startTime.cursor, icon: "text_cursor" },
	custom: { label: t.custom, icon: "edit" },
}, { labelPrefix: t(11).titles });
export /* @internal */ const SelectGeneratedClips = Enum({
	audio: { icon: "volume" },
	visual: { icon: "image" },
	staff: { icon: "g_clef" },
	sonar: { icon: "sonar" },
	lyrics: { icon: "lyrics" },
}, { labelPrefix: t(11).titles });
export /* @internal */ const SelectGeneratedTracks = Enum({
	audio: { icon: "volume" },
	video: { icon: "video" },
}, { labelPrefix: t(11).titles });
export /* @internal */ const GroupTrackBy = Enum({
	off: { icon: "prohibited" },
	track: { icon: "layer" },
	session: { icon: "chat_checkmark" },
});
export /* @internal */ const MoveCursorTo = Enum({
	original: { icon: "subtract" },
	start: { icon: "start_point" },
	beforeFirst: { icon: "arrow_left_text_cursor" },
	afterLast: { icon: "arrow_right_text_cursor" },
}, { labelPrefix: t.source.moveCursorTo });
export /* @internal */ namespace Namings {
	const baseTrackNames = [
		{ id: "clip", name: t.source.naming.clip, icon: "track_event" },
		{ id: "media", name: t.source.naming.media, icon: "media" },
		{ id: "unnamed", name: t.source.naming.unnamed, icon: "prohibited" },
	] as const;
	export const scoredTrackNames = [
		{ id: "score", name: t.source.naming.score, icon: "document_score" },
		...baseTrackNames,
	] as const;
	const baseClipNames = [
		{ id: "unset", name: t.unset, icon: "subtract" },
	] as const;
	export const otomadTrackNames = [
		{ id: "track", name: t.source.naming.track, icon: "layer" },
		{ id: "trackIndex", name: t.source.naming.trackIndex, icon: "layer_number" },
		{ id: "instrument", name: t.source.naming.instrument, icon: "instrument" },
		{ id: "channel", name: t.source.naming.channel, icon: "midi" },
		...scoredTrackNames,
	] as const;
	export const vocaloidTrackNames = [
		{ id: "voicebank", name: t.source.naming.voicebank, icon: "person_mic" },
		{ id: "voicebankIndex", name: t.source.naming.voicebankIndex, icon: "person_number" },
		{ id: "project", name: t.source.naming.project, icon: "ust" },
		...scoredTrackNames,
	] as const;
	export const ytpTrackNames = baseTrackNames;
	export const otomadClipNames = [
		...otomadTrackNames,
		...baseClipNames,
	] as const;
	export const vocaloidClipNames = [
		{ id: "lyric", name: t.source.naming.lyric, icon: "script_a" },
		...vocaloidTrackNames,
		...baseClipNames,
	] as const;
	export const ytpClipNames = [
		{ id: "effect", name: t.source.naming.effect, icon: "sparkle" },
		...baseTrackNames,
		...baseClipNames,
	] as const;
}
export /* @internal */ const sequentialOrders = [
	{ id: "sequential", icon: "arrow_right_double" },
	{ id: "reversed", icon: "arrow_left_double" },
	{ id: "shuffled", icon: "shuffle" },
] as const;

export /* @internal */ const barOrBeatUnitTypes = ["bar", "beat"] as const;

const NamingSetting = styled(Setting)`
	& + .expander-child .combo-box {
		min-inline-size: 250px;
	}
`;

/** @deprecated */
const isUnderVegas16 = true;

export default function Source() {
	const {
		sourceFrom, trimStart, trimEnd, startTime, customStartTime, moveCursorTo,
		belowAdjustmentTracks, preferredTrack: [preferredTrack, setPreferredTrack],
		trackGroup, collapseTrackGroup, reuseSameNameTrackGroup, audioBusTrack, reuseSameNameAudioBusTrack,
	} = useSelectConfig(c => c.source);
	const {
		otomadTrackName, vocaloidTrackName, ytpTrackName, otomadClipName, vocaloidClipName, ytpClipName,
		groupByTaskSessionName, groupByTaskSessionNameTreatSingleAsMultitrack, unsetBorrowedTrackName,
	} = useSelectConfig(c => c.source.naming);
	const {
		mysteryBox, consonant, syncopator,
		syncopatorOrder, syncopatorMysteryBox, syncopatorRepeatOne, syncopatorRepeatRound, syncopatorApplyEffectsByRound,
		syncopatorAccumulateHarmonics, syncopatorSustain, syncopatorPitchCacheCapacity,
		orchestra, orchestraDescending, orchestraAllowReuseExisted, orchestraMysteryBox,
		mysteryBoxLimitToSelected, mysteryBoxForTrack, mysteryBoxForMarker, lotionBath,
		mysteryBoxForBarOrBeat, mysteryBoxForBarOrBeatPeriod, mysteryBoxForBarOrBeatPreparation,
	} = useSelectConfig(c => c.source.multisourceComb);
	const {
		removeSourceClips, removeSourceClipsWithTracks, selectSourceClips, selectGeneratedClips: _selectGeneratedClips,
		keepOriginalTrackSelection, selectGeneratedTracks: _selectGeneratedTracks,
	} = useSelectConfig(c => c.source.removeOrSelect);
	const { enabled: [ytpEnabled] } = useSelectConfig(c => c.ytp);
	const { enabled: shupelunkerEnabled } = useSelectConfig(c => c.shupelunker);
	const meta = metas.source;
	const [mode] = useKichikuMode();
	const namingSubExpanderExpanded = useStateList(true, true, true);
	/** @deprecated */ const manualEnabled = false;

	mutexSwitches(removeSourceClips, selectSourceClips);
	mutexSwitches(removeSourceClipsWithTracks, selectSourceClips);
	mutexSwitches(mysteryBox, consonant, syncopator, orchestra);
	useEffect(() => { removeSourceClipsWithTracks[0] && removeSourceClips[1](true); }, [removeSourceClipsWithTracks[0]]);
	useEffect(() => { !removeSourceClips[0] && removeSourceClipsWithTracks[1](false); }, [removeSourceClips[0]]);

	const selectGeneratedClips = useStateSelector(
		_selectGeneratedClips,
		items => typeof items === "boolean" ? SelectGeneratedClips.keys : items === undefined ? [] : items,
		items => new Set(items).equals(new Set(SelectGeneratedClips.keys)) ? true : items,
		{ processPrevStateInSetterWithGetter: true },
	);
	const selectGeneratedTracks = useStateSelector(
		_selectGeneratedTracks,
		items => typeof items === "boolean" ? SelectGeneratedTracks.keys : items === undefined ? [] : items,
		items => new Set(items).equals(new Set(SelectGeneratedTracks.keys)) ? true : items,
		{ processPrevStateInSetterWithGetter: true },
	);

	const lockRemoveOrSelectSourceClips = sourceFrom[0] !== "trackEvent" ? false : null;

	return (
		<div className="container">
			<Card className="media-pool" data-anchor={meta.from.meta.cssPath}>
				<TabBar current={sourceFrom} aria-label={t.source.from}>
					<TabBar.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</TabBar.Item>
					<TabBar.Item id="projectMedia" icon="media">{t.source.projectMedia}</TabBar.Item>
					<TabBar.Item id="browseFile" icon="open_file">{t.source.browseFile}</TabBar.Item>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Setting meta={meta.trim} checkInfo={trimStart[0] !== trimEnd[0] ? t.on : t.off} alwaysShowCheckInfo>
				{/* TODO: 当 trimStart 小于或等于 trimEnd 时，checkInfo 显示“关”，否则显示“开”。 */}
				<ExpanderChildTrim.Timecode start={trimStart} end={trimEnd} />
				<TrimIgnoredInfoBar />
			</Setting>
			<Setting
				meta={meta.startTime}
				items={StartTimes}
				value={startTime}
				view="tile"
			>
				<CustomItem current={startTime}>
					{setToCustom => <TimecodeBox value={customStartTime} onFocus={setToCustom} />}
				</CustomItem>
			</Setting>

			<Subheader meta={meta.afterCompletion} />
			<Setting meta={meta.removeOrSelect}>
				<Setting meta={meta.removeOrSelect.removeSourceClips} on={removeSourceClips} lock={lockRemoveOrSelectSourceClips} />
				<Setting meta={meta.removeOrSelect.removeSourceClipsWithTracks} on={removeSourceClipsWithTracks} lock={lockRemoveOrSelectSourceClips} />
				<Setting meta={meta.removeOrSelect.selectSourceClips} on={selectSourceClips} lock={lockRemoveOrSelectSourceClips} />
				<ItemsView view="tile" multiple current={selectGeneratedClips} selectAll={{ meta: meta.removeOrSelect.selectGeneratedClips, icon: undefined }}>
					{SelectGeneratedClips.map(({ key, label, icon }) =>
						<ItemsView.Item id={key} key={key} icon={icon}>{label}</ItemsView.Item>)}
				</ItemsView>
				<Setting meta={meta.removeOrSelect.keepOriginalTrackSelection} on={keepOriginalTrackSelection} />
				<ItemsView
					view="tile"
					multiple
					current={selectGeneratedTracks}
					selectAll={{ meta: meta.removeOrSelect.selectGeneratedTracks, icon: undefined }}
					disabled={keepOriginalTrackSelection[0]}
					indeterminatenesses={keepOriginalTrackSelection[0] ? true : undefined}
				>
					{SelectGeneratedTracks.map(({ key, label, icon }) =>
						<ItemsView.Item id={key} key={key} icon={icon}>{label}</ItemsView.Item>)}
				</ItemsView>
			</Setting>
			<Setting
				meta={meta.moveCursorTo}
				items={MoveCursorTo}
				value={moveCursorTo}
				view="tile"
				detailsField={({ key }) => (
					<>
						{t.descriptions.source.moveCursorTo[key]}
						{key === "start" && <SettingsCard.SelectInfo>{StartTimes.label(startTime[0])}</SettingsCard.SelectInfo>}
					</>
				)}
			/>

			<Subheader meta={meta.advanced} />
			<Setting
				meta={meta.preferredTrack}
				selectInfo={preferredTrack === 0 ? t.source.preferredTrack.top : t(preferredTrack).source.preferredTrack.ordinal}
			>
				<Setting
					meta={meta.preferredTrack.index}
					actions={(
						<StackPanel>
							<TextBox.Number value={[preferredTrack, setPreferredTrack]} decimalPlaces={0} />
							<QuickSelectCurrentTrack />
						</StackPanel>
					)}
				/>
				<Setting
					meta={meta.preferredTrack.belowAdjustmentTracks}
					on={belowAdjustmentTracks}
					selectInfo={isUnderVegas16 && t.descriptions.versionRequest.min({ current: 16, min: 19 })}
					lock={isUnderVegas16 ? false : null}
				/>
			</Setting>
			<Setting
				meta={meta.trackGroup}
				items={GroupTrackBy}
				nameField={({ key }) => t.source.trackGroup[key]}
				detailsField={({ key }) => t.descriptions.source.trackGroup[key]}
				checkInfoCondition={key => t.source.trackGroup[key!]}
				value={trackGroup}
				view="tile"
				ieOff
			>
				<Setting meta={meta.trackGroup.collapse} on={collapseTrackGroup} />
				<Setting meta={meta.trackGroup.reuseSameName} on={reuseSameNameTrackGroup} />
			</Setting>
			<Setting
				meta={meta.audioBusTrack}
				items={GroupTrackBy}
				nameField={({ key }) => t.source.audioBusTrack[key]}
				detailsField={({ key }) => t.descriptions.source.audioBusTrack[key]}
				checkInfoCondition={key => t.source.audioBusTrack[key!]}
				value={audioBusTrack}
				view="tile"
				ieOff
			>
				<Setting meta={meta.audioBusTrack.reuseSameName} on={reuseSameNameAudioBusTrack} />
			</Setting>
			<NamingSetting meta={meta.naming}>
				<Setting meta={meta.naming.trackName} asSubtitle expanded={namingSubExpanderExpanded[0]} noIndentation wrapActionsWhenNarrow={false}>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.otomad })} selectInfo={mode === "otomad" && t.mode.current}>
						<ComboBox current={otomadTrackName} ids={Namings.otomadTrackNames.map(({ id }) => id)} options={Namings.otomadTrackNames.map(({ name }) => name)} icons={Namings.otomadTrackNames.map(({ icon }) => icon)} />
					</Expander.Item>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.vocaloid })} selectInfo={mode === "vocaloid" && t.mode.current}>
						<ComboBox current={vocaloidTrackName} ids={Namings.vocaloidTrackNames.map(({ id }) => id)} options={Namings.vocaloidTrackNames.map(({ name }) => name)} icons={Namings.vocaloidTrackNames.map(({ icon }) => icon)} />
					</Expander.Item>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.ytp })} selectInfo={mode === "ytp" && t.mode.current}>
						<ComboBox current={ytpTrackName} ids={Namings.ytpTrackNames.map(({ id }) => id)} options={Namings.ytpTrackNames.map(({ name }) => name)} icons={Namings.ytpTrackNames.map(({ icon }) => icon)} />
					</Expander.Item>
					<Setting meta={meta.naming.unsetBorrowedTrackName} on={unsetBorrowedTrackName} noIndentation={false} />
				</Setting>
				<Setting
					meta={meta.naming.groupByTaskSessionName}
					selectInfo={trackGroup[0] === "session" && t.current}
					actions={<ComboBox current={groupByTaskSessionName} ids={Namings.scoredTrackNames.map(({ id }) => id)} options={Namings.scoredTrackNames.map(({ name }) => name)} icons={Namings.scoredTrackNames.map(({ icon }) => icon)} />}
					expanded={namingSubExpanderExpanded[1]}
					asSubtitle
					wrapActionsWhenNarrow={false}
				>
					<Setting
						meta={meta.naming.groupByTaskSessionNameTreatSingleAsMultitrack}
						on={groupByTaskSessionNameTreatSingleAsMultitrack}
						selectInfo={t.descriptions.source.naming.groupByTaskSessionNameTreatSingleAsMultitrack({
							name: groupByTaskSessionNameTreatSingleAsMultitrack[0] ? Namings.scoredTrackNames.find(({ id }) => id === groupByTaskSessionName[0])?.name :
							mode === "vocaloid" ? Namings.vocaloidTrackNames.find(({ id }) => id === vocaloidTrackName[0])?.name :
							mode === "ytp" ? Namings.ytpTrackNames.find(({ id }) => id === ytpTrackName[0])?.name :
							Namings.otomadTrackNames.find(({ id }) => id === otomadTrackName[0])?.name,
						})}
						selectValid
					/>
				</Setting>
				<Setting meta={meta.naming.clipName} asSubtitle expanded={namingSubExpanderExpanded[2]} noIndentation wrapActionsWhenNarrow={false}>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.otomad })} selectInfo={mode === "otomad" && t.mode.current}>
						<ComboBox current={otomadClipName} ids={Namings.otomadClipNames.map(({ id }) => id)} options={Namings.otomadClipNames.map(({ name }) => name)} icons={Namings.otomadClipNames.map(({ icon }) => icon)} />
					</Expander.Item>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.vocaloid })} selectInfo={mode === "vocaloid" && t.mode.current}>
						<ComboBox current={vocaloidClipName} ids={Namings.vocaloidClipNames.map(({ id }) => id)} options={Namings.vocaloidClipNames.map(({ name }) => name)} icons={Namings.vocaloidClipNames.map(({ icon }) => icon)} />
					</Expander.Item>
					<Expander.Item title={t.mode.whichMode({ mode: t.mode.ytp })} selectInfo={mode === "ytp" && t.mode.current}>
						<ComboBox current={ytpClipName} ids={Namings.ytpClipNames.map(({ id }) => id)} options={Namings.ytpClipNames.map(({ name }) => name)} icons={Namings.ytpClipNames.map(({ icon }) => icon)} />
					</Expander.Item>
				</Setting>
			</NamingSetting>

			<Subheader meta={meta.multisource} />
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.source.multisource.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}
			<Attrs disabled={ytpEnabled ? true : undefined}>
				<Setting meta={meta.orchestra} on={orchestra}>
					<Setting
						meta={meta.orchestra.selectionMode}
						actions={<DualStateSwitch current={orchestraMysteryBox} falseText={t.source.syncopator} trueText={t.source.mysteryBox} falseIcon="flag_auto_beat" trueIcon="question_square" />}
					/>
					<Setting meta={meta.orchestra.descending} on={orchestraDescending} disabled={orchestraMysteryBox[0]} />
					<Setting meta={meta.orchestra.allowReuseExisted} on={orchestraAllowReuseExisted} lock={orchestraMysteryBox[0] ? true : undefined} />
				</Setting>
				<Setting meta={meta.syncopator} on={syncopator}>
					<Setting
						meta={meta.syncopator.order}
						actions={(
							<Segmented current={syncopatorOrder}>
								{sequentialOrders.map(({ id, icon }) => <Segmented.Item id={id} key={id} icon={icon}>{t[id]}</Segmented.Item>)}
							</Segmented>
						)}
					/>
					<Setting meta={meta.syncopator.repeatOne} title={t(syncopatorRepeatOne[0]).source.syncopator.repeatOne} actions={<TextBox.Number value={syncopatorRepeatOne} min={1} max={100} decimalPlaces={0} />} />
					<Setting meta={meta.syncopator.repeatRound} title={t(syncopatorRepeatRound[0]).source.syncopator.repeatRound} actions={<TextBox.Number value={syncopatorRepeatRound} min={0} max={100} decimalPlaces={0} />} selectInfo={t.descriptions.source.syncopator.repeatRoundInfinityInfo} selectValid={syncopatorRepeatRound[0] === 0 ? true : ["info"]} />
					<Setting meta={meta.syncopator.applyEffectsByRound} on={syncopatorApplyEffectsByRound} />
					<Setting meta={meta.syncopator.mysteryBox} on={syncopatorMysteryBox} details={t.descriptions.source.mysteryBox.splitOnce("\n")[0]} />
					<Setting meta={meta.syncopator.accumulateHarmonics} on={syncopatorAccumulateHarmonics} />
					<Setting meta={meta.syncopator.sustain} on={syncopatorSustain} />
					<Setting meta={meta.syncopator.pitchCacheCapacity} actions={<TextBox.Number value={syncopatorPitchCacheCapacity} min={1} max={200} decimalPlaces={0} />} disabled={!syncopatorSustain[0]} />
				</Setting>
				<Setting meta={meta.mysteryBox} selectInfo={ytpEnabled && t.descriptions.source.mysteryBox.ytpEnabled} on={mysteryBox}>
					<Setting meta={meta.mysteryBox.limitToSelected} on={mysteryBoxLimitToSelected} />
					<Setting meta={meta.mysteryBox.track} on={mysteryBoxForTrack} />
					<Setting meta={meta.mysteryBox.marker} on={mysteryBoxForMarker} />
					<Setting meta={meta.mysteryBox.barOrBeat} expanded={mysteryBoxForBarOrBeat} type="switch">
						<Setting
							meta={meta.mysteryBox.barOrBeat.period}
							actions={(
								<TextBox.NumberUnit value={mysteryBoxForBarOrBeatPeriod} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={1} />
							)}
						/>
						<Setting
							meta={meta.mysteryBox.barOrBeat.preparation}
							actions={(
								<TextBox.NumberUnit value={mysteryBoxForBarOrBeatPreparation} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={0} />
							)}
						/>
					</Setting>
					<Setting meta={meta.mysteryBox.lotionBath} on={lotionBath} />
				</Setting>
				<Setting
					meta={meta.consonant}
					on={consonant}
					lock={manualEnabled ? true : null}
					selectInfo={manualEnabled ? t.descriptions.source.consonant.manualEnabled : undefined}
					selectValid={manualEnabled}
				/>
				<Setting meta={metas.shupelunker} actions={<ToggleSwitch on={shupelunkerEnabled} />} />
			</Attrs>

			<DragToImport>{t.titles.source}</DragToImport>
		</div>
	);
}

export /* @internal */ function TrimIgnoredInfoBar() {
	const { hideUseTips } = useSnapshot(configStore.settings);
	if (hideUseTips) return;
	return <InfoBar status="accent" title={t.descriptions.trimIgnored} />;
}
