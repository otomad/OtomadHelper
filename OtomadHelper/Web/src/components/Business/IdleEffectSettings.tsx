export const IdleEffects = Enum({
	fade: { icon: "fade", label: t.fade, quantifiable: true, defaultValue: 50 },
	monochrome: { icon: "grayscale", label: t.ytp.effects.monochrome, quantifiable: true, defaultValue: 100 },
	negative: { icon: "invert_color", label: t.ytp.effects.negative, quantifiable: false, defaultValue: 100 },
});

export default function IdleEffectSetting({ value: values, pinToTop, disabled, details }: {
	/** Each effects value, includes enabled and amount. */
	value: StateProperty<Config.IdleEffectValue>;
	/** Pin a specific effect to the first of all. */
	pinToTop?: Config.IdleEffect;
	/** Disabled? */
	disabled?: boolean;
	/** Detailed description. */
	details?: string;
}) {
	const pinnedIdleEffects = useMemo(() => {
		const effects = IdleEffects.array;
		if (pinToTop !== undefined) {
			const fromIndex = effects.findIndex(({ key }) => key === pinToTop);
			if (fromIndex !== -1)
				effects.move(fromIndex);
		}
		return effects;
	}, [pinToTop]);

	function selectNone() {
		(values[1] as SetStateNarrow<Config.IdleEffectValue>)?.(produce(draft => {
			for (const effect in draft)
				if (hasOwn(draft, effect))
					draft[effect].enabled = false;
		}));
	}

	return (
		<Attrs disabled={disabled}>
			<Expander.Item icon="sparkle" title={t.titles.effect} details={details} asSubtitle>
				<Button icon="select_none" disabled={disabled} onClick={selectNone}>{t.selectNone}</Button>
			</Expander.Item>
			{pinnedIdleEffects.map(({ key, icon, label, quantifiable, defaultValue }) => {
				const enabled = useStateSelector(
					values,
					values => values[key].enabled,
					(enabled, values) => values[key].enabled = enabled,
					{ immer: true },
				);
				const amount = useStateSelector(
					values,
					values => values[key].amount,
					(amount, values) => values[key].amount = amount,
					{ immer: true },
				);
				return (
					<Checkbox
						key={key}
						value={enabled}
						icon={icon}
						actions={quantifiable && <SliderWithBox value={amount} suffix={t.units.percent} defaultValue={defaultValue} decimalPlaces={0} />}
					>
						{label}
					</Checkbox>
				);
			})}
		</Attrs>
	);
}
