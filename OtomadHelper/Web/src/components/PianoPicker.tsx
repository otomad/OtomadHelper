const StyledPianoPickerOutput = styled.div`
	position: relative;
	min-block-size: ${24 + expanderItemPadding[0] * 2}px;
	padding-block: ${expanderItemPadding[0]}px;
	padding-inline: 0 !important;

	output {
		${styles.effects.text.bodyLarge};
		display: block;
		font-feature-settings: "case" on;
		font-variant-numeric: tabular-nums;
		text-align: center;

		&.range {
			display: grid;
			grid-template-columns: 1fr auto 1fr;
			gap: 5.5px;

			span:first-of-type {
				text-align: end;
			}

			span:last-of-type {
				text-align: start;
			}
		}
	}

	button {
		position: absolute;
		inset-block: ${expanderItemPadding[0]}px;
		inset-inline-end: ${expanderItemPadding[1]}px;
		margin-block: auto;
	}
`;

type SPNRange = [start: string, end: string];
const DEFAULT_PITCH = "C5";
export const DEFAULT_PITCH_RANGE = Object.freeze(["C0", "F#10"] as const);

interface Props {
	/**
	 * Show the pitch or pitch range output result?
	 * Show output if the value is `true`, or if the value is `undefined` and `showReset` is `true`; otherwise hide output.
	 */
	showOutput?: boolean;
	/**
	 * Show the result button?
	 * Show reset if the value is `true`, or if the value is `undefined` and `showOutput` is `true`; otherwise hide reset.
	 */
	showReset?: boolean;
}

export default function PianoPicker(props: {
	/** A SPN. */
	pitch: StatePropertyNonNull<string>;
} & Props): React.JSX.Element;
export default function PianoPicker(props: {
	/** A range of SPNs. */
	pitch: StatePropertyNonNull<SPNRange>;
} & Props): React.JSX.Element;
export default function PianoPicker({ pitch: [pitch, setPitch], showOutput: _showOutput, showReset: _showReset }: { pitch: StatePropertyNonNull<string> | StatePropertyNonNull<SPNRange> } & Props) {
	const previewPianoEl = useDomRef<"div">();
	const rangeMode = typeof pitch !== "string";
	const showOutput = _showOutput || _showOutput === undefined && _showReset, showReset = _showReset || _showReset === undefined && _showOutput;

	const activeKeys = useMemo(() =>
		typeof pitch === "string" ? [pitch] :
		forMapFromTo(getNoteNumber(pitch[0]), getNoteNumber(pitch[1]), 1, noteNumber => new Pitch(noteNumber).spn),
	[pitch]);

	function onMouseDown(spn: string) {
		(setPitch as SetStateNarrow<string | SPNRange>)(pitch => {
			if (typeof pitch === "string") return spn;
			const [startSpn, endSpn] = pitch;
			const [startNoteNumber, endNoteNumber, currentNoteNumber] = [getNoteNumber(startSpn), getNoteNumber(endSpn), getNoteNumber(spn)];
			if (Math.abs(startNoteNumber - currentNoteNumber) < Math.abs(endNoteNumber - currentNoteNumber)) return [spn, endSpn];
			else return [startSpn, spn];
		});
	}

	function reset() {
		(setPitch as SetStateNarrow<string | SPNRange>)(pitch =>
			typeof pitch === "string" ? DEFAULT_PITCH : [...DEFAULT_PITCH_RANGE]);
	}

	useMountEffect(() => {
		const activeKeys = previewPianoEl.current?.querySelectorAll(".active, .source");
		if (!activeKeys?.length) return;
		else if (activeKeys.length === 1) activeKeys[0].scrollIntoView({ inline: "center", container: "nearest", behavior: "instant" });
		else {
			activeKeys[activeKeys.length / 2 | 0].scrollIntoView({ inline: "center", container: "nearest", behavior: "instant" });
			activeKeys[0].scrollIntoView({ inline: "nearest", container: "nearest", behavior: "instant" });
		}
	});

	return (
		<>
			{(showOutput || showReset) && (
				<StyledPianoPickerOutput>
					{showOutput && (!rangeMode ?
						<output>{pitch}</output> : (
							<output className="range">
								<span>{pitch[0]}</span>
								<span>{t.rangeDash}</span>
								<span>{pitch[1]}</span>
							</output>
						)
					)}
					{showReset && <Button icon="arrow_reset" accent="critical" subtle extruded onClick={reset}>{t.reset}</Button>}
				</StyledPianoPickerOutput>
			)}
			<PreviewPiano
				sourceKeys={rangeMode ? activeKeys : undefined}
				activeKeys={!rangeMode ? activeKeys : undefined}
				showKeyLabels={wrapIfNotArray(pitch)}
				onMouseDown={onMouseDown}
				ref={previewPianoEl}
			/>
		</>
	);
}

function getNoteNumber(pitch: string) {
	return new Pitch(pitch).noteNumber;
}
