export const IdleEffect = Enum({
	none: { icon: "prohibited", label: t.none, quantifiable: false },
	fade: { icon: "fade", label: t.fade, quantifiable: true },
	monochrome: { icon: "grayscale", label: t.ytp.effects.monochrome, quantifiable: true },
	negative: { icon: "invert_color", label: t.ytp.effects.negative, quantifiable: false },
});

export default function IdleEffectSetting({ value: [effect, setEffect], amount, pinToTop, disabled, showNone = true }: {
	value: StateProperty<Config.IdleEffect>;
	amount: StateProperty<number>;
	pinToTop?: Config.IdleEffect;
	disabled?: boolean;
	showNone?: boolean;
}) {
	const currentEffect = IdleEffect.all[effect!];

	const pinnedIdleEffects = useMemo(() => {
		const effects = IdleEffect.array;
		if (pinToTop !== undefined) {
			const fromIndex = effects.findIndex(({ key }) => key === pinToTop);
			if (fromIndex !== -1)
				effects.move(fromIndex, undefined, 1);
		}
		if (!showNone) effects.shift();
		return effects;
	}, [pinToTop, showNone]);

	return (
		<>
			<Expander.Item icon="sparkle" title={t.titles.effect} disabled={disabled}>
				<Segmented current={[effect, setEffect]}>
					{pinnedIdleEffects.map(({ key, icon, label }) =>
						<Segmented.Item key={key} id={key} icon={icon}>{label}</Segmented.Item>)}
				</Segmented>
			</Expander.Item>
			<Expander.Item
				title={t.amplitude}
				details={t.descriptions.amplitude({ effect: currentEffect.label })}
				icon="chevron_up_down"
				disabled={!currentEffect.quantifiable || disabled}
			>
				<SliderWithBox value={amount} suffix={t.units.percent} defaultValue={100} decimalPlaces={0} />
			</Expander.Item>
		</>
	);
}
