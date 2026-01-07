export /* @internal */ const sourceFromEnums = ["trackEvent", "projectMedia", "browseFile"] as const;
export /* @internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const SelectGeneratedClips = Enum({
	audio: { icon: "volume" },
	visual: { icon: "image" },
	staff: { icon: "g_clef" },
	sonar: { icon: "sonar" },
	lyrics: { icon: "lyrics" },
}, { labelPrefix: t.titles });
export /* @internal */ const TrackGroupBy = Enum({
	ungrouped: { icon: "prohibited" },
	byScoreTrack: { icon: "layer" },
	byTaskSession: { icon: "chat_checkmark" },
}, { labelPrefix: t.source.trackGroup });
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
		sourceFrom, trimStart, trimEnd, startTime, customStartTime,
		belowAdjustmentTracks, preferredTrack: [preferredTrack, setPreferredTrack],
		trackGroup, collapseTrackGroup, reuseSameNameTrackGroup,
		otomadTrackName, vocaloidTrackName, ytpTrackName, otomadClipName, vocaloidClipName, ytpClipName,
		groupByTaskSessionName, groupByTaskSessionNameTreatSingleAsMultitrack, unsetBorrowedTrackName,
		luckyDip, consonant, matchCut, matchCutOrder, matchCutLoop, matchCutLuckyDip, linearMap, linearMapDescending,
		luckyDipLimitToSelected, luckyDipForTrack, luckyDipForMarker, luckyDipForBarOrBeat, luckyDipForBarOrBeatPeriod, luckyDipForBarOrBeatPreparation,
	} = useSelectConfig(c => c.source);
	const { removeSourceClips, removeSourceClipsWithTracks, selectSourceClips, selectGeneratedClips: _selectGeneratedClips } = useSelectConfig(c => c.source.afterCompletion);
	const { enabled: [ytpEnabled] } = useSelectConfig(c => c.ytp);
	const meta = metas.source;
	const [mode] = useKichikuMode();
	const namingSubExpanderExpanded = useStateList(true, true, true);
	/** @deprecated */ const manualEnabled = false;

	mutexSwitches(removeSourceClips, selectSourceClips);
	mutexSwitches(removeSourceClipsWithTracks, selectSourceClips);
	mutexSwitches(luckyDip, consonant, matchCut, linearMap);
	useEffect(() => { removeSourceClipsWithTracks[0] && removeSourceClips[1](true); }, [removeSourceClipsWithTracks[0]]);
	useEffect(() => { !removeSourceClips[0] && removeSourceClipsWithTracks[1](false); }, [removeSourceClips[0]]);

	const selectGeneratedClips = useStateSelector(
		_selectGeneratedClips,
		items => typeof items === "boolean" ? SelectGeneratedClips.keys : items === undefined ? [] : items,
		items => new Set(items).equals(new Set(SelectGeneratedClips.keys)) ? true : items,
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
				items={startTimes}
				value={startTime}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			>
				<CustomItem current={startTime}>
					{setToCustom => <TimecodeBox value={customStartTime} onFocus={setToCustom} />}
				</CustomItem>
			</Setting>

			<Subheader meta={meta.advanced} />
			<Setting meta={meta.afterCompletion}>
				<Setting meta={meta.afterCompletion.removeSourceClips} on={removeSourceClips} lock={lockRemoveOrSelectSourceClips} />
				<Setting meta={meta.afterCompletion.removeSourceClipsWithTracks} on={removeSourceClipsWithTracks} lock={lockRemoveOrSelectSourceClips} />
				<Setting meta={meta.afterCompletion.selectSourceClips} on={selectSourceClips} lock={lockRemoveOrSelectSourceClips} />
				<ItemsView view="tile" multiple current={selectGeneratedClips} selectAll={{ meta: meta.afterCompletion.selectGeneratedClips, icon: undefined }}>
					{SelectGeneratedClips.map(({ key, label, icon }) =>
						<ItemsView.Item id={key} key={key} icon={icon}>{label}</ItemsView.Item>)}
				</ItemsView>
			</Setting>

			<Setting
				meta={meta.preferredTrack}
				selectInfo={preferredTrack === 0 ? t.source.preferredTrack.top : t(preferredTrack).source.preferredTrack.ordinal}
			>
				<Setting
					meta={meta.preferredTrack.index}
					actions={(
						<StackPanel>
							<TextBox.Number value={[preferredTrack, setPreferredTrack]} decimalPlaces={0} />
							<QuicklySelectCurrentTrack />
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
				items={TrackGroupBy}
				value={trackGroup}
				view="tile"
				ieOff
			>
				<Setting meta={meta.trackGroup.collapse} on={collapseTrackGroup} />
				<Setting meta={meta.trackGroup.reuseSameName} on={reuseSameNameTrackGroup} />
			</Setting>
			<NamingSetting meta={meta.naming}>
				<Setting meta={meta.naming.trackName} asSubtitle expanded={namingSubExpanderExpanded[0]} noIndentation>
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
					selectInfo={trackGroup[0] === "byTaskSession" && t.current}
					actions={<ComboBox current={groupByTaskSessionName} ids={Namings.scoredTrackNames.map(({ id }) => id)} options={Namings.scoredTrackNames.map(({ name }) => name)} icons={Namings.scoredTrackNames.map(({ icon }) => icon)} />}
					expanded={namingSubExpanderExpanded[1]}
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
				<Setting meta={meta.naming.clipName} asSubtitle expanded={namingSubExpanderExpanded[2]} noIndentation>
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
				<Setting meta={meta.linearMap} on={linearMap}>
					<Setting meta={meta.linearMap.descending} on={linearMapDescending} />
				</Setting>
				<Setting meta={meta.matchCut} on={matchCut}>
					<Setting
						meta={meta.matchCut.order}
						actions={(
							<Segmented current={matchCutOrder}>
								{sequentialOrders.map(({ id, icon }) => <Segmented.Item id={id} key={id} icon={icon}>{t[id]}</Segmented.Item>)}
							</Segmented>
						)}
					/>
					<Setting meta={meta.matchCut.loop} on={matchCutLoop} />
					<Setting meta={meta.matchCut.luckyDip} on={matchCutLuckyDip} />
				</Setting>
				<Setting meta={meta.luckyDip} selectInfo={ytpEnabled && t.descriptions.source.luckyDip.ytpEnabled} on={luckyDip}>
					<Setting meta={meta.luckyDip.limitToSelected} on={luckyDipLimitToSelected} />
					<Setting meta={meta.luckyDip.track} on={luckyDipForTrack} />
					<Setting meta={meta.luckyDip.marker} on={luckyDipForMarker} />
					<Setting meta={meta.luckyDip.barOrBeat} expanded={luckyDipForBarOrBeat} type="switch">
						<Setting
							meta={meta.luckyDip.barOrBeat.period}
							actions={(
								<TextBox.NumberUnit value={luckyDipForBarOrBeatPeriod} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={1} />
							)}
						/>
						<Setting
							meta={meta.luckyDip.barOrBeat.preparation}
							actions={(
								<TextBox.NumberUnit value={luckyDipForBarOrBeatPreparation} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={0} />
							)}
						/>
					</Setting>
				</Setting>
				<Setting
					meta={meta.consonant}
					on={consonant}
					lock={manualEnabled ? true : null}
					selectInfo={manualEnabled ? t.descriptions.source.consonant.manualEnabled : undefined}
					selectValid={manualEnabled}
				/>
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
