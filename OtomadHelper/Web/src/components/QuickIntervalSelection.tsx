const MAX_VISIBLE_WIDTH = 998;
const ELEMENT_SIZE = 50, ELEMENT_GAP = 5;
const paddingX = expanderItemPadding[1] - ELEMENT_GAP;

export /* @internal */ const QuickIntervalSelectionPresets = Enum({
	odd: { bits: new Uint8Array([1, 0]), interval: 2, label: t.odd, icon: "parity/odd_columns" },
	even: { bits: new Uint8Array([0, 1]), interval: 2, label: t.even, icon: "parity/even_columns" },
	custom: { bits: undefined, interval: undefined, label: t.custom, icon: "edit" },
});

const StyledPreviewQuickIntervalSelection = styled(HorizontalScroll)`
	inline-size: 100%;
	padding-block: ${expanderItemPadding[0]}px;
	padding-inline: 0 !important;
	overflow-inline: auto;

	&,
	.repetitive {
		display: flex;
		gap: ${ELEMENT_GAP}px;
		align-items: center;
	}

	&::before {
		content: "";
		position: sticky;
		inset-inline-start: 0;
		display: block;
		inline-size: ${paddingX}px;
	}

	&,
	* {
		&,
		&::before,
		&::after {
			flex-shrink: 0;
		}
	}

	button {
		${styles.mixins.square(`${ELEMENT_SIZE}px`)};
		${styles.effects.text.bodyLarge};
		min-inline-size: unset;
		cursor: pointer;
	}

	> button {
		z-index: 1;
	}

	.repetitive-shadow {
		anchor-name: var(--anchor-name);
		inline-size: ${paddingX}px;
	}

	.repetitive {
		position: absolute;
		position-anchor: var(--anchor-name);
		inset-inline-start: anchor(start);
		opacity: 0.35;
		cursor: not-allowed;

		> * {
			pointer-events: none;
			interactivity: inert;
		}
	}

	:has(> &) {
		contain: paint;
	}

	&.is-preset {
		&,
		+ * {
			opacity: 0.5;
			cursor: not-allowed;

			> * {
				pointer-events: none;
				interactivity: inert;
			}
		}
	}
`;

function PreviewQuickIntervalSelection({ interval, bits: [bits, setBits], isPreset = false }: {
	interval: number;
	bits: [get: Uint8Array<ArrayBuffer>, set?: SetStateNarrow<Uint8Array<ArrayBuffer>>];
	isPreset: boolean;
}) {
	const extendBitsLength = useEffectEvent(() => {
		if (interval > bits.length)
			setBits?.(oldBits => {
				const newBits = new Uint8Array(interval);
				newBits.set(oldBits);
				return newBits;
			});
	});

	useEffect(() => { extendBitsLength(); }, [interval]);

	const repetitiveCount = Math.max(Math.ceil(MAX_VISIBLE_WIDTH / (ELEMENT_SIZE + ELEMENT_GAP) - interval), 1);
	const id = useId();

	return (
		<StyledPreviewQuickIntervalSelection className={{ isPreset }} style={{ "--anchor-name": "--" + id }}>
			{forMap(interval, i => (
				<ToggleButton
					key={i}
					appearance="obvious"
					checked={[!!bits[i]]}
					onToggled={checked => setBits?.(bits => bits.slice(0, interval).with(i, +!!checked))}
					aria-label={t.descriptions.prve.stepAria({ step: i + 1, frames: interval })}
				>
					{i + 1}
				</ToggleButton>
			))}
			<div className="repetitive-shadow" aria-hidden />
			<div className="repetitive" aria-hidden>
				{forMap(repetitiveCount, i => (
					<ToggleButton
						key={`${i / interval | 0}-${i % interval}`}
						appearance="obvious"
						checked={[!!bits[i % interval]]}
					>
						{i % interval + 1}
					</ToggleButton>
				))}
			</div>
		</StyledPreviewQuickIntervalSelection>
	);
}

export default function QuickIntervalSelection({ interval, bits: bitsBase64, preset }: {
	interval: StatePropertyNonNull<number>;
	bits: StatePropertyNonNull<string>;
	preset?: StateProperty<Config.QuickIntervalSelectionPreset>;
}) {
	const bits = useBitArray(bitsBase64);
	const currentPreset = QuickIntervalSelectionPresets.all[preset?.[0] ?? "custom"];
	const isCustom = !currentPreset.bits || !currentPreset.interval;

	return (
		<>
			{preset && (
				<Expander.Item title={t.preset} details={t.descriptions.tools.selector.quickIntervalSelection.preset} icon="preset">
					<Segmented.Enum items={QuickIntervalSelectionPresets} current={preset} />
				</Expander.Item>
			)}
			<PreviewQuickIntervalSelection interval={isCustom ? interval[0] : currentPreset.interval} bits={isCustom ? bits : [currentPreset.bits]} isPreset={!isCustom} />
			<Expander.Item title={t.tools.selector.quickIntervalSelection.interval} details={t.descriptions.tools.selector.quickIntervalSelection.interval} icon="table_simple_include">
				<TextBox.Number min={1} max={100} decimalPlaces={0} value={isCustom ? interval : [currentPreset.interval]} />
			</Expander.Item>
		</>
	);
}
