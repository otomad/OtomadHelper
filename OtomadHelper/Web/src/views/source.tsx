export /* @internal */ const sourceFromEnums = ["trackEvent", "projectMedia", "browseFile"] as const;
export /* @internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const selectGeneratedClipsType = [
	{ id: "audio", name: t.titles.audio, icon: "volume" },
	{ id: "visual", name: t.titles.visual, icon: "image" },
	{ id: "staff", name: t.titles.staff, icon: "g_clef" },
	{ id: "sonar", name: t.titles.sonar, icon: "sonar" },
	{ id: "lyrics", name: t.titles.lyrics, icon: "lyrics" },
] as const;
const allSelectGeneratedClips = Object.freeze(selectGeneratedClipsType.map(item => item.id));
const getAllSelectGeneratedClips = () => allSelectGeneratedClips.slice();
export /* @internal */ const trackNames = [
	{ id: "track", name: t.source.trackName.track, additional: t.source.trackName.voicebank, icon: "layer" },
	{ id: "trackIndex", name: t.source.trackName.trackIndex, icon: "layer_number" },
	{ id: "instrument", name: t.source.trackName.instrument, additional: t.source.trackName.voicebank, icon: "instrument" },
	{ id: "channel", name: t.source.trackName.channel, icon: "midi" },
	{ id: "clip", name: t.source.trackName.clip, icon: "track_event" },
	{ id: "media", name: t.source.trackName.media, icon: "media" },
	{ id: "score", name: t.source.trackName.score, icon: "document_score" },
	{ id: "unnamed", name: t.source.trackName.unnamed, icon: "prohibited" },
] as const;
export /* @internal */ const sequentialOrders = [
	{ id: "sequential", icon: "arrow_right_double" },
	{ id: "reversed", icon: "arrow_left_double" },
	{ id: "shuffled", icon: "shuffle" },
] as const;

export /* @internal */ const barOrBeatUnitTypes = ["bar", "beat"] as const;

/** @deprecated */
const isUnderVegas16 = true;

export default function Source() {
	const {
		sourceFrom, trimStart, trimEnd, startTime, customStartTime,
		belowAdjustmentTracks, preferredTrack: [preferredTrack, setPreferredTrack],
		trackGroup, collapseTrackGroup, trackName, secretBox, consonant, matchCut, matchCutOrder, matchCutLoop, matchCutSecretBox, linearMap, linearMapDescending,
		secretBoxLimitToSelected, secretBoxForTrack, secretBoxForMarker, secretBoxForBarOrBeat, secretBoxForBarOrBeatPeriod, secretBoxForBarOrBeatPreparation,
	} = useSelectConfig(c => c.source);
	const { removeSourceClips, removeSourceClipsWithTracks, selectSourceClips, selectGeneratedClips: _selectGeneratedClips } = useSelectConfig(c => c.source.afterCompletion);
	const { enabled: [ytpEnabled] } = useSelectConfig(c => c.ytp);
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
			<Card className="media-pool">
				<TabBar current={sourceFrom} aria-label={t.source.from}>
					<TabBar.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</TabBar.Item>
					<TabBar.Item id="projectMedia" icon="media">{t.source.projectMedia}</TabBar.Item>
					<TabBar.Item id="browseFile" icon="open_file">{t.source.browseFile}</TabBar.Item>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Setting meta={metas.source.trim}>
				<ExpanderChildTrim.Timecode start={trimStart} end={trimEnd} />
			</Setting>
			<Setting
				meta={metas.source.startTime}
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

			<Subheader>{t.subheaders.advanced}</Subheader>
			<Expander title={t.source.afterCompletion} icon="post_processing">
				<ToggleSwitch on={removeSourceClips} lock={lockRemoveOrSelectSourceClips} icon="delete_track_event">{t.source.afterCompletion.removeSourceClips}</ToggleSwitch>
				<ToggleSwitch on={removeSourceClipsWithTracks} lock={lockRemoveOrSelectSourceClips} icon="delete_layer">{t.source.afterCompletion.removeSourceClipsWithTracks}</ToggleSwitch>
				<ToggleSwitch on={selectSourceClips} lock={lockRemoveOrSelectSourceClips} icon="select_all">{t.source.afterCompletion.selectSourceClips}</ToggleSwitch>
				<ItemsView view="tile" multiple current={selectGeneratedClips} selectAll={{ title: t.source.afterCompletion.selectGeneratedClips }}>
					{selectGeneratedClipsType.map(({ id, name, icon }) =>
						<ItemsView.Item id={id} key={id} icon={icon}>{name}</ItemsView.Item>)}
				</ItemsView>
			</Expander>

			<Expander
				title={t.source.preferredTrack}
				selectInfo={preferredTrack === 0 ? t.source.preferredTrack.top : t(preferredTrack).source.preferredTrack.ordinal}
				icon="preferred_track"
			>
				<Expander.Item title={t.source.preferredTrack.index} details={t.descriptions.source.preferredTrack.fillingInstructions} icon="layer_number">
					<StackPanel>
						<TextBox.Number value={[preferredTrack, setPreferredTrack]} decimalPlaces={0} />
						<QuicklySelectCurrentTrack />
					</StackPanel>
				</Expander.Item>
				<ToggleSwitch
					on={belowAdjustmentTracks}
					selectInfo={isUnderVegas16 && t.descriptions.source.preferredTrack.belowAdjustmentTracks.versionRequest({ version: 16 })}
					icon="layer_sparkle_add_below"
					lock={isUnderVegas16 ? false : null}
				>
					{t.source.preferredTrack.belowAdjustmentTracks}
				</ToggleSwitch>
			</Expander>
			<SettingsCardToggleSwitch title={t.source.trackGroup} details={t.descriptions.source.trackGroup} icon="group" on={trackGroup}>
				<ToggleSwitch on={collapseTrackGroup} icon="chevron_down_up">{t.source.trackGroup.collapse}</ToggleSwitch>
			</SettingsCardToggleSwitch>
			<ExpanderRadio
				title={t.source.trackName}
				details={t.descriptions.source.trackName}
				icon="rename"
				items={trackNames}
				value={trackName}
				view="tile"
				idField="id"
				nameField={t.source.trackName}
				iconField="icon"
				detailsField="additional"
			/>

			<Subheader>{t.source.multisource}</Subheader>
			{ytpEnabled && <InfoBar status="warning" title={t.descriptions.source.multisource.ytpEnabled} button={<EmptyMessage.YtpDisabled.Buttons />} />}
			<Attrs disabled={ytpEnabled ? true : undefined}>
				<SettingsCardToggleSwitch
					title={t.source.secretBox}
					details={t.descriptions.source.secretBox}
					selectInfo={ytpEnabled && t.descriptions.source.secretBox.ytpEnabled}
					icon="dice"
					on={secretBox}
				>
					<ToggleSwitch on={secretBoxLimitToSelected} details={t.descriptions.source.secretBox.limitToSelected} icon="video_clip_multiple_checkmark">{t.source.secretBox.limitToSelected}</ToggleSwitch>
					<ToggleSwitch on={secretBoxForTrack} details={t.descriptions.source.secretBox.track} icon="layer">{t.source.secretBox.track}</ToggleSwitch>
					<ToggleSwitch on={secretBoxForMarker} details={t.descriptions.source.secretBox.marker} icon="flag">{t.source.secretBox.marker}</ToggleSwitch>
					<ToggleSwitch on={secretBoxForBarOrBeat} details={t.descriptions.source.secretBox.barOrBeat} icon="music_bar">{t.source.secretBox.barOrBeat}</ToggleSwitch>
					<Attrs disabled={!secretBoxForBarOrBeat[0]}>
						<Expander.Item title={t.source.secretBox.barOrBeat.period} details={t.descriptions.source.secretBox.barOrBeat.period} icon="timer">
							<TextBox.NumberUnit value={secretBoxForBarOrBeatPeriod} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={1} />
						</Expander.Item>
						<Expander.Item title={t.source.secretBox.barOrBeat.preparation} details={t.descriptions.source.secretBox.barOrBeat.preparation} icon="hourglass">
							<TextBox.NumberUnit value={secretBoxForBarOrBeatPreparation} units={barOrBeatUnitTypes} unitNames={(unit, count) => t(count).units[unit]} decimalPlaces={0} min={0} />
						</Expander.Item>
					</Attrs>
				</SettingsCardToggleSwitch>
				<SettingsCardToggleSwitch
					title={t.source.consonant}
					details={t.descriptions.source.consonant}
					icon="consonant"
					on={consonant}
					lock={manualEnabled ? true : null}
					selectInfo={manualEnabled ? t.descriptions.source.consonant.manualEnabled : undefined}
					selectValid={manualEnabled}
				/>
				<SettingsCardToggleSwitch
					title={t.source.matchCut}
					details={t.descriptions.source.matchCut}
					icon="flag_auto_beat"
					on={matchCut}
				>
					<Expander.Item title={t.order} icon="arrow_sort_horizontal" details={t.descriptions.source.matchCut.order}>
						<Segmented current={matchCutOrder}>
							{sequentialOrders.map(({ id, icon }) => <Segmented.Item id={id} key={id} icon={icon}>{t[id]}</Segmented.Item>)}
						</Segmented>
					</Expander.Item>
					<ToggleSwitch on={matchCutLoop} icon="arrow_repeat_all" details={t.descriptions.source.matchCut.loop}>{t.stream.loop}</ToggleSwitch>
					<ToggleSwitch on={matchCutSecretBox} icon="dice" details={t.descriptions.source.matchCut.secretBox}>{t.source.secretBox}</ToggleSwitch>
				</SettingsCardToggleSwitch>
				<SettingsCardToggleSwitch
					title={t.source.linearMap}
					details={t.descriptions.source.linearMap}
					icon="launchpad"
					on={linearMap}
				>
					<ToggleSwitch on={linearMapDescending} details={t.descriptions.source.linearMap.descending} icon="descending">{t.descending}</ToggleSwitch>
				</SettingsCardToggleSwitch>
			</Attrs>

			<DragToImport>{t.titles.source}</DragToImport>
		</div>
	);
}
