const MAX_VISIBLE_WIDTH = 896;
const ELEMENT_SIZE = 50, ELEMENT_GAP = 5;
const paddingX = expanderItemPadding[1] - ELEMENT_GAP;

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
		min-inline-size: unset;
		cursor: pointer;
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
`;

function PreviewQuickIntervalSelection({ interval, bits: [bits, setBits] }: {
	interval: number;
	bits: StatePropertyNonNull<Uint8Array<ArrayBuffer>>;
}) {
	const extendBitsLength = useEffectEvent(() => {
		if (interval > bits.length)
			setBits(oldBits => {
				const newBits = new Uint8Array(interval);
				newBits.set(oldBits);
				return newBits;
			});
	});

	useEffect(() => { extendBitsLength(); }, [interval]);

	const repetitiveCount = Math.max(Math.ceil(MAX_VISIBLE_WIDTH / (ELEMENT_SIZE + ELEMENT_GAP) - interval), 1);
	const id = useId();

	return (
		<StyledPreviewQuickIntervalSelection style={{ "--anchor-name": "--" + id }}>
			{forMap(interval, i => (
				<ToggleButton
					key={i}
					appearance="obvious"
					checked={[!!bits[i]]}
					onToggled={checked => setBits(bits => bits.slice(0, interval).with(i, +!!checked))}
				/>
			))}
			<div className="repetitive-shadow" />
			<div className="repetitive">
				{forMap(repetitiveCount, i => (
					<ToggleButton
						key={`${i / interval | 0}-${i % interval}`}
						appearance="obvious"
						checked={[!!bits[i % interval]]}
					/>
				))}
			</div>
		</StyledPreviewQuickIntervalSelection>
	);
}

export default function QuickIntervalSelection({ interval, bits: bitsBase64 }: {
	interval: StatePropertyNonNull<number>;
	bits: StatePropertyNonNull<string>;
}) {
	const bits = useBitArray(bitsBase64);

	return (
		<>
			<PreviewQuickIntervalSelection interval={interval[0]} bits={bits} />
			<Expander.Item title="Interval" icon="placeholder">
				<TextBox.Number min={1} max={100} decimalPlaces={0} value={interval} />
			</Expander.Item>
		</>
	);
}
