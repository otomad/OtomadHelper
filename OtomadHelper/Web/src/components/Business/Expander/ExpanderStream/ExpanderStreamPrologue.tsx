export /* @internal */ const PrologueForms = Enum({
	straightforward: { icon: "flash_play" },
	introduceOriginally: { icon: "video_clip_asterisk_hourglass" },
	introduceEffectively: { icon: "video_clip_sparkle_hourglass" },
}, { labelPrefix: t.stream.prologue });

export /* @internal */ const PrologueDurationUsings = Enum({
	upToOneBar: { label: t.stream.legato.upToOneBar, icon: "music_bar" },
	sourceLength: { label: t.stream.prologue.emphasisDuration.sourceLength, icon: "video_clip_inbox" },
	untilTheStart: { label: t.stream.prologue.untilTheStart, icon: "start_point" },
	custom: { label: t.custom, icon: "edit" },
});

export /* @internal */ const PrologueEmphasisDurations = Enum({
	sourceLength: { label: t.stream.prologue.emphasisDuration.sourceLength, icon: "video_clip_inbox" },
	oneBeat: { label: () => `1 ${t(1).units.beat}`, icon: "quarter_note" },
	twoBeat: { label: () => `2 ${t(2).units.beat}`, icon: "half_note" },
	oneBar: { label: () => `1 ${t(2).units.bar}`, icon: "music_bar" },
	twoBar: { label: () => `2 ${t(2).units.bar}`, icon: "music_bar_2" },
});
console.log(PrologueEmphasisDurations);

const emphasisTimesOptions = forMapFromTo(0, 3);

export default function ExpanderStreamPrologue({ stream }: {
	/** Audio or visual? */
	stream: StreamKind;
}) {
	const { form, durationUsing, customDuration, once, visualIdleEffect, audioIdleEffect, emphasisTimes, emphasisDuration } = useSelectConfig(c => c.prologue);
	const isAudio = stream === "audio";
	const meta = metas[stream].prologue;

	return (
		<Setting
			meta={meta}
			items={PrologueForms}
			value={form}
			view="tile"
			detailsField={({ key }) => t.descriptions.stream.prologue[key]}
		>
			<Setting meta={meta.duration} asSubtitle="closerAfter" noDivider="after" />
			<ItemsView view="tile" current={durationUsing}>
				{PrologueDurationUsings.map(({ key, label, icon }) => key !== "custom" &&
					<ItemsView.Item key={key} id={key} icon={icon} details={t.descriptions.stream.prologue[key]}>{label}</ItemsView.Item>)}
			</ItemsView>
			<CustomItem current={durationUsing}>
				{setToCustom => <TimecodeBox value={customDuration} onChanging={setToCustom} />}
			</CustomItem>
			<IdleEffectSettings value={isAudio ? audioIdleEffect : visualIdleEffect} pinToTop="fade" stream={stream} />
			<Setting meta={meta.once} on={once} />
			<Setting
				meta={meta.emphasisTimes}
				actions={(
					<ComboBox
						current={emphasisTimes}
						ids={emphasisTimesOptions}
						options={emphasisTimesOptions.map(times => times === 0 ? t.off : `${times} ${t(times).units.times}`)}
					/>
				)}
			/>
			<Setting
				meta={meta.emphasisDuration}
				disabled={emphasisTimes[0] === 0}
				actions={<ComboBox current={emphasisDuration} ids={PrologueEmphasisDurations.keys} options={PrologueEmphasisDurations.labels} icons={PrologueEmphasisDurations.map(({ icon }) => icon)} />}
			/>
		</Setting>
	);
}
