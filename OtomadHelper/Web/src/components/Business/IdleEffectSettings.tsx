import { SELECT_ALL_PADDING_INLINE } from "components/SelectAll";

export const VisualIdleEffects = Enum({
	fade: { icon: "fade", iconForAudio: "speaker_arrow_bidirectional", quantifiable: true, defaultValue: 50 },
	monochrome: { icon: "grayscale", iconForAudio: undefined, quantifiable: true, defaultValue: 100 },
	negative: { icon: "invert_color", iconForAudio: undefined, quantifiable: false, defaultValue: 100 },
}, { labelPrefix: t.stream.idleEffect });

export default function IdleEffectSettings({ value: [value, setValue], pinToTop, disabled, details, disabledInfo, stream = "visual" }: {
	/** Each effects value, includes enabled and amount. */
	value: StateProperty<Config.VisualIdleEffectValue> | StateProperty<Config.AudioIdleEffectValue>;
	/** Pin a specific effect to the first of all. */
	pinToTop?: Config.VisualIdleEffect;
	/** Disabled? */
	disabled?: boolean;
	/** Detailed description. */
	details?: string;
	/** If provided and also disabled, it will replace the details. */
	disabledInfo?: string;
	/** Audio or visual? */
	stream?: StreamKind;
}) {
	const isAudio = stream === "audio";
	const values = [value, setValue] as StateProperty<Config.VisualIdleEffectValue>;

	const pinnedIdleEffects = useMemo(() => {
		if (isAudio) return [VisualIdleEffects.all.fade];
		const effects = VisualIdleEffects.array;
		if (pinToTop !== undefined) {
			const fromIndex = effects.findIndex(({ key }) => key === pinToTop);
			if (fromIndex !== -1)
				effects.move(fromIndex);
		}
		return effects;
	}, [pinToTop, isAudio]);

	const enabledEffectCount = useMemo(() => !value ? 0 : Object.values(value).filter(({ enabled }) => enabled).length, [value]);
	const effectTitlePlural = pinnedIdleEffects.length === 1 ? 1 : enabledEffectCount;

	function selectNone() {
		(setValue as SetStateNarrow<Config.VisualIdleEffectValue>)?.(produce(draft => {
			for (const effect in draft)
				if (hasOwn(draft, effect))
					draft[effect].enabled = false;
		}));
	}

	return (
		<Attrs disabled={disabled}>
			<Expander.Item
				icon="sparkle"
				title={t(effectTitlePlural).titles.effect}
				style={{ paddingInlineEnd: SELECT_ALL_PADDING_INLINE[1] }}
				asSubtitle
				{...disabled && disabledInfo ? {
					selectInfo: disabledInfo,
					selectValid: false,
				} : {
					details,
				}}
			>
				<Button
					icon="select_none"
					disabled={disabled || enabledEffectCount === 0}
					subtle
					onClick={selectNone}
				>
					{t.selectNone}
				</Button>
			</Expander.Item>
			{pinnedIdleEffects.map(({ key, icon, iconForAudio, label, quantifiable, defaultValue }) => {
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
						icon={isAudio && iconForAudio || icon}
						actions={quantifiable && <SliderWithBox value={amount} suffix={t.units.percent} defaultValue={defaultValue} decimalPlaces={0} />}
					>
						{label}
					</Checkbox>
				);
			})}
		</Attrs>
	);
}
