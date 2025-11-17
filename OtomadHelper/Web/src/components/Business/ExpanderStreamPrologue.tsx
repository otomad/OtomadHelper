export /* @internal */ const PrologueForms = Enum({
	straightforward: {},
	introduceOriginally: {},
	introduceEffectively: {},
}, { labelPrefix: t.stream.prologue });

export /* @internal */ const PrologueDurationUsings = Enum({
	upToOneBar: { label: t.stream.legato.upToOneBar },
	untilTheStart: { label: t.stream.prologue.untilTheStart },
	custom: { label: t.custom },
});

export default function ExpanderStreamPrologue({ stream }: {
	/** Audio or visual? */
	stream: StreamKind;
}) {
	const { form, durationUsing, customDuration, once, repeat } = useSelectConfig(c => c.prologue);
	const isAudio = stream === "audio";
	const meta = metas[stream].prologue;

	return (
		<Setting
			meta={meta}
			items={PrologueForms}
			value={form}
			view="tile"
		>
			<Setting meta={meta.duration} asSubtitle="closerAfter" noDivider="after" />
			<ItemsView view="tile" current={durationUsing}>
				{PrologueDurationUsings.map(({ key, label }) => key !== "custom" && <ItemsView.Item key={key} id={key}>{label}</ItemsView.Item>)}
			</ItemsView>
			<CustomItem current={durationUsing}>
				{setToCustom => <TimecodeBox value={customDuration} onChanging={setToCustom} />}
			</CustomItem>
			<Setting meta={meta.once} on={once} />
			<Setting
				meta={meta.repeat}
				actions={<TextBox.Number value={repeat} min={0} max={3} decimalPlaces={0} suffix={t(repeat[0]).units.times} />}
			/>
		</Setting>
	);
}
