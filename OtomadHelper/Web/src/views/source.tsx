export /* @internal */ const sourceFromEnums = ["trackEvent", "projectMedia", "browseFile"] as const;
export /* @internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
/* const SelectGeneratedClips = Enum({
	audio: { icon: "volume" },
	visual: { icon: "image" },
	staff: { icon: "g_clef" },
	sonar: { icon: "sonar" },
	lyrics: { icon: "lyrics" },
}, { labelPrefix: t.titles }); */
export /* @internal */ const selectGeneratedClipsType = [
	{ id: "audio", name: t.titles.audio, icon: "volume" },
	{ id: "visual", name: t.titles.visual, icon: "image" },
	{ id: "staff", name: t.titles.staff, icon: "g_clef" },
	{ id: "sonar", name: t.titles.sonar, icon: "sonar" },
	{ id: "lyrics", name: t.titles.lyrics, icon: "lyrics" },
] as const;
const allSelectGeneratedClips = Object.freeze(selectGeneratedClipsType.map(item => item.id));
const getAllSelectGeneratedClips = () => allSelectGeneratedClips.slice();
export /* @internal */ namespace Namings {
	const baseTrackNames = [
		{ id: "clip", name: t.source.naming.clip, icon: "track_event" },
		{ id: "media", name: t.source.naming.media, icon: "media" },
		{ id: "unnamed", name: t.source.naming.unnamed, icon: "prohibited" },
	] as const;
	const scoredTrackNames = [
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
		trackGroup, collapseTrackGroup, otomadTrackName, vocaloidTrackName, ytpTrackName, otomadClipName, vocaloidClipName, ytpClipName,
		secretBox, consonant, matchCut, matchCutOrder, matchCutLoop, matchCutSecretBox, linearMap, linearMapDescending,
		secretBoxLimitToSelected, secretBoxForTrack, secretBoxForMarker, secretBoxForBarOrBeat, secretBoxForBarOrBeatPeriod, secretBoxForBarOrBeatPreparation,
	} = useSelectConfig(c => c.source);
	const { removeSourceClips, removeSourceClipsWithTracks, selectSourceClips, selectGeneratedClips: _selectGeneratedClips } = useSelectConfig(c => c.source.afterCompletion);
	const { enabled: [ytpEnabled] } = useSelectConfig(c => c.ytp);
	const meta = metas.source;
	const [mode] = useKichikuMode();
	/** @deprecated */ const manualEnabled = false;

	mutexSwitches(removeSourceClips, selectSourceClips);
	mutexSwitches(removeSourceClipsWithTracks, selectSourceClips);
	mutexSwitches(secretBox, consonant, matchCut, linearMap);
	useEffect(() => { removeSourceClipsWithTracks[0] && removeSourceClips[1](true); }, [removeSourceClipsWithTracks[0]]);
	useEffect(() => { !removeSourceClips[0] && removeSourceClipsWithTracks[1](false); }, [removeSourceClips[0]]);

	const selectGeneratedClips = useStateSelector(
		_selectGeneratedClips,
		items => typeof items === "boolean" ? getAllSelectGeneratedClips() : items === undefined ? [] : items,
		items => new Set(items).equals(new Set(allSelectGeneratedClips)) ? true : items,
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
					{selectGeneratedClipsType.map(({ id, name, icon }) =>
						<ItemsView.Item id={id} key={id} icon={icon}>{name}</ItemsView.Item>)}
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
			<Setting meta={meta.trackGroup} on={trackGroup}>
				<Setting meta={meta.trackGroup.collapse} on={collapseTrackGroup} />
			</Setting>
			<NamingSetting meta={meta.naming}>
				<Setting meta={meta.naming.trackName} asSubtitle />
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.otomad })} selectInfo={mode === "otomad" && t.mode.current}>
					<ComboBox current={otomadTrackName} ids={Namings.otomadTrackNames.map(({ id }) => id)} options={Namings.otomadTrackNames.map(({ name }) => name)} icons={Namings.otomadTrackNames.map(({ icon }) => icon)} />
				</Expander.Item>
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.vocaloid })} selectInfo={mode === "vocaloid" && t.mode.current}>
					<ComboBox current={vocaloidTrackName} ids={Namings.vocaloidTrackNames.map(({ id }) => id)} options={Namings.vocaloidTrackNames.map(({ name }) => name)} icons={Namings.vocaloidTrackNames.map(({ icon }) => icon)} />
				</Expander.Item>
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.ytp })} selectInfo={mode === "ytp" && t.mode.current}>
					<ComboBox current={ytpTrackName} ids={Namings.ytpTrackNames.map(({ id }) => id)} options={Namings.ytpTrackNames.map(({ name }) => name)} icons={Namings.ytpTrackNames.map(({ icon }) => icon)} />
				</Expander.Item>
				<Setting meta={meta.naming.clipName} asSubtitle />
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.otomad })} selectInfo={mode === "otomad" && t.mode.current}>
					<ComboBox current={otomadClipName} ids={Namings.otomadClipNames.map(({ id }) => id)} options={Namings.otomadClipNames.map(({ name }) => name)} icons={Namings.otomadClipNames.map(({ icon }) => icon)} />
				</Expander.Item>
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.vocaloid })} selectInfo={mode === "vocaloid" && t.mode.current}>
					<ComboBox current={vocaloidClipName} ids={Namings.vocaloidClipNames.map(({ id }) => id)} options={Namings.vocaloidClipNames.map(({ name }) => name)} icons={Namings.vocaloidClipNames.map(({ icon }) => icon)} />
				</Expander.Item>
				<Expander.Item title={t.mode.whichMode({ mode: t.mode.ytp })} selectInfo={mode === "ytp" && t.mode.current}>
					<ComboBox current={ytpClipName} ids={Namings.ytpClipNames.map(({ id }) => id)} options={Namings.ytpClipNames.map(({ name }) => name)} icons={Namings.ytpClipNames.map(({ icon }) => icon)} />
				</Expander.Item>
			</NamingSetting>

			<Subheader meta={meta.multisource} />
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.source.multisource.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}
			<Attrs disabled={ytpEnabled ? true : undefined}>
				<Setting meta={meta.secretBox} selectInfo={ytpEnabled && t.descriptions.source.secretBox.ytpEnabled} on={secretBox}>
					<Setting meta={meta.secretBox.limitToSelected} on={secretBoxLimitToSelected} />
					<Setting meta={meta.secretBox.track} on={secretBoxForTrack} />
					<Setting meta={meta.secretBox.marker} on={secretBoxForMarker} />
					<Setting meta={meta.secretBox.barOrBeat} expanded={secretBoxForBarOrBeat} type="switch">
						<Setting
							meta={meta.secretBox.barOrBeat.period}
							actions={(
								<TextBox.NumberUnit value={secretBoxForBarOrBeatPeriod} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={1} />
							)}
						/>
						<Setting
							meta={meta.secretBox.barOrBeat.preparation}
							actions={(
								<TextBox.NumberUnit value={secretBoxForBarOrBeatPreparation} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={0} />
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
					<Setting meta={meta.matchCut.secretBox} on={matchCutSecretBox} />
				</Setting>
				<Setting meta={meta.linearMap} on={linearMap}>
					<Setting meta={meta.linearMap.descending} on={linearMapDescending} />
				</Setting>
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
