import legatoPortatoImage from "assets/images/tutorials/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/tutorials/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/tutorials/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/tutorials/legato_config/legato_up_to_1beat.png";

export /* @internal */ const LegatoDurations = Enum({
	portato: { icon: "prohibited", image: legatoPortatoImage, limited: false },
	upToOneBeat: { icon: "quarter_note", image: legatoUpTo1BeatImage, limited: true },
	upToOneBar: { icon: "music_bar", image: legatoUpTo1BarImage, limited: true },
	unlimited: { icon: "infinity", image: legatoUnlimitedImage, limited: false },
}, { labelPrefix: t.stream.legato });
export /* @internal */ const LegatoModes = Enum({
	stacking: { noteApplicable: false },
	stackingAllAfter: { noteApplicable: false },
	stackingAllTracks: { noteApplicable: false },
	limitStretch: { noteApplicable: true },
	stretch: { noteApplicable: true },
	lengthen: { noteApplicable: true },
	increaseSpacing: { noteApplicable: false },
	increaseSpacingAllTracks: { noteApplicable: false },
}, { labelPrefix: t.track.legato });

export default function ExpanderLegato({ stream, children }: FCP<{
	/** Audio, visual, or track? */
	stream: StreamKind | "track";
}>) {
	const isTrack = stream === "track";
	const { legatoDuration, legatoToMaxGap, legatoMode } = selectConfig(c => isTrack ? c.track.legato : c[stream]);
	const meta = metas[stream].legato;
	const currentLegatoDuration = LegatoDurations.all[legatoDuration[0]];

	return (
		<Setting meta={meta} checkInfo={!isTrack && currentLegatoDuration.label}>
			<Setting meta={meta.duration} asSubtitle="closerAfter" noDivider="after" />
			<ItemsView view="grid" current={legatoDuration} itemWidth={566 / 196 * GRID_VIEW_ITEM_HEIGHT}>
				{LegatoDurations.map(({ key, label, icon, image }) => {
					const isOff = key === "portato";
					if (isOff && isTrack) return;
					return (
						<ItemsView.Item
							id={key}
							key={key}
							icon={icon}
							image={image}
						>
							{label}
							{isOff && t.parenOff}
						</ItemsView.Item>
					);
				})}
			</ItemsView>
			<Setting meta={meta.toMaxGap} on={legatoToMaxGap} title={t({ context: legatoDuration[0] }).stream.legato.toMaxGap} disabled={!currentLegatoDuration.limited} />
			<Setting meta={meta.mode} asSubtitle="closerAfter" noDivider="after" />
			{isTrack ? <TrackLegato /> : (
				<ItemsView view="grid" current={legatoMode} itemWidth={320}>
					{LegatoModes.array.filter(({ noteApplicable }) => noteApplicable).map(({ key: mode, label }) => {
						const multiline = label.split("\n");
						return (
							<ItemsView.Item
								id={mode}
								key={mode}
								details={multiline[1]}
								image={<PreviewTrackLegato mode={mode} />}
								withBorder
							>
								{multiline[0]}
							</ItemsView.Item>
						);
					})}
				</ItemsView>
			)}
			{children}
		</Setting>
	);
}

function TrackLegato() {
	const { legatoMode, increaseSpacing, forClips: legatoForClips, includeGroup: legatoIncludeGroup, backwards: legatoBackwards } = selectConfig(c => c.track.legato);
	const meta = metas.track;

	useEffect(() => {
		if (!legatoForClips[0] && legatoMode[0] === "stackingAllAfter") legatoMode[1]("stacking");
	}, [legatoMode, legatoForClips]);

	return (
		<>
			<ItemsView view="grid" current={legatoMode} itemWidth={320} key={String(legatoForClips[0])}>
				{/* When `legatoForClips` change, re-render the entire component to avoid the animation of the newly added item being out of sync with other items. */}
				{LegatoModes.keys.map(mode => {
					if (mode === "stackingAllAfter" && !legatoForClips[0]) return;
					const displayMode = mode === "stacking" && legatoForClips[0] ? "stackingSelected" : mode;
					const multiline = t.track.legato[displayMode].split("\n");
					return (
						<ItemsView.Item
							id={mode}
							key={mode}
							details={multiline[1]}
							image={<PreviewTrackLegato mode={mode} />}
							withBorder
						>
							{multiline[0]}
						</ItemsView.Item>
					);
				})}
			</ItemsView>
			<Setting meta={meta.legato.forClips} on={legatoForClips} />
			<Setting meta={meta.legato.includeGroup} on={legatoIncludeGroup} />
			<Setting meta={meta.legato.backwards} on={legatoBackwards} />
			<Setting
				meta={meta.legato.increaseSpacing}
				disabled={!legatoMode[0].in("increaseSpacing", "increaseSpacingAllTracks")}
				actions={<TimecodeBox value={increaseSpacing} />}
			/>
			<Expander.ChildWrapper>
				<Button icon="checkmark">{t.apply}</Button>
			</Expander.ChildWrapper>
		</>
	);
}
