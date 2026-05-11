import { SELECT_ALL_PADDING_INLINE } from "components/SelectAll";

export /* @internal */ const VisualIdleEffects = Enum({
	fade: { icon: "fade", iconForAudio: "speaker_arrow_bidirectional", amountType: "quantifiable", defaultValue: 50 },
	monochrome: { icon: "grayscale", iconForAudio: undefined, amountType: "quantifiable", defaultValue: 100 },
	negative: { icon: "invert_color", iconForAudio: undefined, amountType: "negative", defaultValue: 100 },
}, { labelPrefix: t.stream.idleEffect });

export /* @internal */ const NegativeTypes = Enum({
	hueInvert: 1,
	luminInvert: 2,
	colorInvert: 3,
}, { labelPrefix: t.prve.effects });

export default function IdleEffectSettings({ value: _value, pinToTop, disabled, details, disabledInfo, stream = "visual" }: {
	/** Each effects value, includes enabled and amount. */
	value: StoreSubscribedProperty<Config.VisualIdleEffectValue> | StoreSubscribedProperty<Config.AudioIdleEffectValue>;
	/** Pin a specific effect to the first of all. */
	pinToTop?: Config.VisualIdleEffect;
	/** Disabled? */
	disabled?: boolean;
	/** Detailed description. It can also be get the selected effect count. */
	details?: string | ((selectedEffectCount: number) => string);
	/** If provided and also disabled, it will replace the details. */
	disabledInfo?: string;
	/** Audio or visual? */
	stream?: StreamKind;
}) {
	const isAudio = stream === "audio";
	const [value] = useVariousState(_value as StoreSubscribedProperty<Config.VisualIdleEffectValue>);
	// NOTE: For better performance, do not use `value` except for computing `enabledEffectCount`.

	const pinnedIdleEffects = useMemo(() => {
		if (isAudio) return [VisualIdleEffects.allKeys.fade];
		const effects = VisualIdleEffects.array;
		if (pinToTop !== undefined)
			effects.pinToTopComputed(({ key }) => key === pinToTop);
		return effects;
	}, [pinToTop, isAudio]);

	const enabledEffectCount = useMemo(() => !value ? 0 : Object.values(value).filter(({ enabled }) => enabled).length, [value]);
	const effectTitlePlural = pinnedIdleEffects.length === 1 ? 1 : enabledEffectCount;

	function selectNone() {
		const effects = _value.value;
		for (const effect in effects)
			if (hasOwn(effects, effect))
				effects[effect].enabled = false;
	}

	return (
		<Attrs disabled={disabled}>
			<Expander.Item
				icon="coffee_sparkle"
				title={t(effectTitlePlural).stream.idleEffect}
				style={{ paddingInlineEnd: SELECT_ALL_PADDING_INLINE[1] }}
				asSubtitle
				{...disabled && disabledInfo ? {
					selectInfo: disabledInfo,
					selectValid: false,
				} : {
					details: typeof details === "function" ? details(effectTitlePlural) : details,
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
			{pinnedIdleEffects.map(effect => <PinnedIdleEffect key={effect.key} isAudio={isAudio} disabled={disabled} effect={effect} value={_value} />)}
		</Attrs>
	);
}

function PinnedIdleEffect({ isAudio, disabled, effect: { key, icon, iconForAudio, label, amountType, defaultValue }, value }: {
	isAudio: boolean;
	disabled?: boolean;
	effect: typeof VisualIdleEffects.array[number];
	value: PropsOf<typeof IdleEffectSettings>["value"];
}) {
	const { enabled, amount: amountOrNegativeType } = currySubscribeStore(value.value[key as "fade"]);
	const amount = amountOrNegativeType, negativeType = amountOrNegativeType as never as StoreSubscribedProperty<typeof NegativeTypes.keyType>;
	return (
		<Checkbox
			key={key}
			value={enabled}
			icon={isAudio && iconForAudio || icon}
			actions={
				amountType === "quantifiable" ? <SliderWithBox disabled={disabled} value={amount} suffix={t.units.percent} defaultValue={defaultValue} decimalPlaces={0} /> :
				amountType === "negative" ? <ComboBox disabled={disabled} current={negativeType} ids={NegativeTypes.keys} options={NegativeTypes.labels} /> :
				undefined
			}
		>
			{label}
		</Checkbox>
	);
}
