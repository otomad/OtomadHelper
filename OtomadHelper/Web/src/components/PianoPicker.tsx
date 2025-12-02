type SPNRange = [start: string, end: string];

export default function PianoPicker({ pitch }: {
	/** A SPN. */
	pitch: StatePropertyNonNull<string>;
}): React.JSX.Element;
export default function PianoPicker({ pitch }: {
	/** A range of SPNs. */
	pitch: StatePropertyNonNull<SPNRange>;
}): React.JSX.Element;
export default function PianoPicker({ pitch: [pitch, setPitch] }: { pitch: StatePropertyNonNull<string> | StatePropertyNonNull<SPNRange> }) {
	const previewPianoEl = useDomRef<"div">();
	const rangeMode = typeof pitch !== "string";

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
		<PreviewPiano
			sourceKeys={rangeMode ? activeKeys : undefined}
			activeKeys={!rangeMode ? activeKeys : undefined}
			showKeyLabels={wrapIfNotArray(pitch)}
			onMouseDown={onMouseDown}
			ref={previewPianoEl}
		/>
	);
}

function getNoteNumber(pitch: string) {
	return new Pitch(pitch).noteNumber;
}
