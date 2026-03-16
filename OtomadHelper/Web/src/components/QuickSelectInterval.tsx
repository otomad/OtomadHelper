const MAX_VISIBLE_WIDTH = 998;
const ELEMENT_SIZE = 50, ELEMENT_GAP = 5;
const paddingX = expanderItemPadding[1] - ELEMENT_GAP;

export /* @internal */ const QuickSelectIntervalPresets = Enum({
	odd: { bits: new BitArray([1, 0]), interval: 2, label: t.odd, icon: "parity/odd_columns" },
	even: { bits: new BitArray([0, 1]), interval: 2, label: t.even, icon: "parity/even_columns" },
	custom: { bits: undefined, interval: undefined, label: t.custom, icon: "edit" },
});

const StyledPreviewQuickSelectInterval = styled(HorizontalScroll)`
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
		${styles.text.bodyLarge};
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
		~ * {
			opacity: 0.5;
			cursor: not-allowed;

			> * {
				pointer-events: none;
				interactivity: inert;
			}
		}
	}
`;

function PreviewQuickSelectInterval({ interval, bits: [bits, setBits], isPreset = false }: {
	interval: number;
	bits: readonly [get: BitArray, set?: SetStateNarrow<BitArray>];
	isPreset: boolean;
}) {
	const extendBitsLength = useEffectEvent(() => {
		if (interval > bits.length)
			setBits?.(oldBits => oldBits.toResized(interval));
	});

	useEffect(() => { extendBitsLength(); }, [interval]);

	const repetitiveCount = Math.max(Math.ceil(MAX_VISIBLE_WIDTH / (ELEMENT_SIZE + ELEMENT_GAP) - interval), 1);
	const id = useId();

	return (
		<StyledPreviewQuickSelectInterval className={{ isPreset }} style={{ "--anchor-name": "--" + id }}>
			{forMap(interval, i => (
				<ToggleButton
					key={i}
					appearance="obvious"
					checked={[!!bits[i]]}
					onToggled={checked => setBits?.(bits => { const newBits = bits.toResized(interval); newBits[i] = !!checked; return newBits; })}
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
		</StyledPreviewQuickSelectInterval>
	);
}

export default function QuickSelectInterval({ interval, bits: bitsBase64, preset }: {
	interval: StatePropertyNonNull<number>;
	bits: StatePropertyNonNull<string>;
	preset?: StateProperty<Config.QuickSelectIntervalPreset>;
}) {
	const bits = useBitArray(bitsBase64);
	const [, setBits] = bits;
	const currentPreset = QuickSelectIntervalPresets.allKeys[preset?.[0] ?? "custom"];
	const isCustom = !currentPreset.bits || !currentPreset.interval;

	function invert() {
		setBits(bits => bits.map(bit => !bit));
	}

	function clear() {
		setBits(bits => new BitArray(bits.length));
	}

	return (
		<>
			{preset && (
				<Expander.Item title={t.preset} details={t.descriptions.tools.selector.quickSelectInterval.preset} icon="preset">
					<Segmented.Enum items={QuickSelectIntervalPresets} current={preset} />
				</Expander.Item>
			)}
			<PreviewQuickSelectInterval interval={isCustom ? interval[0] : currentPreset.interval} bits={isCustom ? bits : [currentPreset.bits]} isPreset={!isCustom} />
			<Expander.Item>
				<StackPanel>
					<Tooltip title={t.shared.descriptions.quickSelectIntervalEditor.invert} placement="block">
						<Button icon="invert_selection" onClick={invert}>{t.shared.quickSelectIntervalEditor.invert}</Button>
					</Tooltip>
					<Tooltip title={t.shared.descriptions.quickSelectIntervalEditor.clear} placement="block">
						<Button icon="select_none" onClick={clear}>{t.shared.quickSelectIntervalEditor.clear}</Button>
					</Tooltip>
				</StackPanel>
			</Expander.Item>
			<Expander.Item title={t.tools.selector.quickSelectInterval.interval} details={t.descriptions.tools.selector.quickSelectInterval.interval} icon="table_simple_include">
				<TextBox.Number min={1} max={100} decimalPlaces={0} value={isCustom ? interval : [currentPreset.interval]} />
			</Expander.Item>
		</>
	);
}
