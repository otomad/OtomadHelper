export /* @internal */ const startTimes = [
	{ id: "projectStart", name: t.source.startTime.projectStart, icon: "arrow_export" },
	{ id: "cursor", name: t.source.startTime.cursor, icon: "text_cursor" },
	{ id: "custom", name: t.custom, icon: "edit" },
] as const;
export /* @internal */ const selectGeneratedClipsType = [
	{ id: "audio", name: t.titles.audio, icon: "audio" },
	{ id: "visual", name: t.titles.visual, icon: "visual" },
	{ id: "staff", name: t.titles.staff, icon: "g_clef" },
	{ id: "sonar", name: t.titles.sonar, icon: "sonar" },
	{ id: "lyrics", name: t.titles.lyrics, icon: "lyrics" },
] as const;
const allSelectGeneratedClips = Object.freeze(selectGeneratedClipsType.map(item => item.id));
const getAllSelectGeneratedClips = () => allSelectGeneratedClips.slice();
export /* @internal */ const trackNames = [
	{ id: "track", name: t.source.trackName.track, icon: "track" },
	{ id: "trackIndex", name: t.source.trackName.trackIndex, icon: "track_number_list" },
	{ id: "instrument", name: t.source.trackName.instrument, icon: "score" },
	{ id: "channel", name: t.source.trackName.channel, icon: "midi" },
	{ id: "clip", name: t.source.trackName.clip, icon: "track_event" },
	{ id: "media", name: t.source.trackName.media, icon: "media" },
	{ id: "score", name: t.source.trackName.score, icon: "document_score" },
	{ id: "unnamed", name: t.source.trackName.unnamed, icon: "prohibited" },
] as const;

/** @deprecated */
const isUnderVegas16 = true;

export default function Source() {
	const {
		source, blindBoxForTrack, blindBoxForMarker, trimStart, trimEnd, startTime, customStartTime,
		belowAdjustmentTracks, preferredTrack: [preferredTrack, setPreferredTrack],
		trackGroup, collapseTrackGroup, trackName,
	} = selectConfig(c => c.source);
	const { removeSourceClips, selectSourceClips, selectGeneratedClips: _selectGeneratedClips } = selectConfig(c => c.source.afterCompletion);

	mutexSwitches(removeSourceClips, selectSourceClips);

	const selectGeneratedClips = useStateSelector(
		_selectGeneratedClips,
		items => typeof items === "boolean" ? getAllSelectGeneratedClips() : items === undefined ? [] : items,
		items => new Set(items).equals(new Set(allSelectGeneratedClips)) ? true : items,
		{ processPrevStateInSetterWithGetter: true },
	);

	const underVegas16 = isUnderVegas16 ?
		<p style={{ color: c("fill-color-system-critical") }}>{t.descriptions.source.preferredTrack.belowAdjustmentTracks.versionRequest({ version: 16 })}</p> :
		undefined;

	return (
		<div className="container">
			<Card className="media-pool">
				<TabBar current={source}>
					<TabBar.Item id="trackEvent" icon="track_event">{t.source.trackEvent}</TabBar.Item>
					<TabBar.Item id="projectMedia" icon="media">{t.source.projectMedia}</TabBar.Item>
					<TabBar.Item id="browseFile" icon="open_file">{t.source.browseFile}</TabBar.Item>
				</TabBar>
				<TestThumbnail />
			</Card>

			<Expander title={t.source.trim} details={t.descriptions.source.trim} icon="trim">
				<ExpanderChildTrim.Timecode start={trimStart} end={trimEnd} />
			</Expander>
			<ExpanderRadio
				title={t.source.startTime}
				details={t.descriptions.source.startTime}
				icon="start_point"
				items={startTimes}
				value={startTime}
				view="tile"
				idField="id"
				nameField="name"
				iconField="icon"
			>
				<CustomItem current={startTime}>
					{setToCustom => <TimecodeBox timecode={customStartTime} onFocus={setToCustom} />}
				</CustomItem>
			</ExpanderRadio>

			<Subheader>{t.subheaders.advanced}</Subheader>
			<Expander
				title={t.source.preferredTrack}
				selectInfo={preferredTrack === 0 ? t.source.preferredTrack.top : t(preferredTrack).source.preferredTrack.ordinal}
				icon="preferred_track"
			>
				<Expander.Item title={t.source.preferredTrack.index} details={t.descriptions.source.preferredTrack.fillingInstructions}>
					<TextBox.Number value={[preferredTrack, setPreferredTrack]} decimalPlaces={0} />
				</Expander.Item>
				<ToggleSwitch
					on={belowAdjustmentTracks}
					details={underVegas16}
					lock={isUnderVegas16 ? false : null}
				>
					{t.source.preferredTrack.belowAdjustmentTracks}
				</ToggleSwitch>
			</Expander>
			<Expander title={t.source.afterCompletion} icon="post_processing">
				<ToggleSwitch on={removeSourceClips}>{t.source.afterCompletion.removeSourceClips}</ToggleSwitch>
				<ToggleSwitch on={selectSourceClips}>{t.source.afterCompletion.selectSourceClips}</ToggleSwitch>
				<SelectAll value={selectGeneratedClips} all={getAllSelectGeneratedClips()} title={t.source.afterCompletion.selectGeneratedClips} />
				<ItemsView view="tile" multiple current={selectGeneratedClips}>
					{selectGeneratedClipsType.map(({ id, name, icon }) =>
						<ItemsView.Item id={id} key={id} icon={icon}>{name}</ItemsView.Item>)}
				</ItemsView>
			</Expander>
			<Expander title={t.source.blindBox} details={t.descriptions.source.blindBox} icon="dice">
				<ToggleSwitch on={blindBoxForTrack} details={t.descriptions.source.blindBox.track} icon="track">{t.source.blindBox.track}</ToggleSwitch>
				<ToggleSwitch on={blindBoxForMarker} details={t.descriptions.source.blindBox.marker} icon="marker">{t.source.blindBox.marker}</ToggleSwitch>
			</Expander>
			<SettingsCardToggleSwitch title={t.source.trackGroup} details={t.descriptions.source.trackGroup} icon="group" on={trackGroup}>
				<ToggleSwitch on={collapseTrackGroup} icon="arrow_minimize">{t.source.trackGroup.collapse}</ToggleSwitch>
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
			/>

			<DragToImport>{t.titles.source}</DragToImport>
		</div>
	);
}
