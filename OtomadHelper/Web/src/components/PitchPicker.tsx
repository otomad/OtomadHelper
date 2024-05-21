import { StyledButton } from "./Button";

const StyledPitchPicker = styled(StyledButton)`
	align-items: stretch;
	height: 1px; // A hack way to cheat its children with 100% height to match its min height.
	min-height: 32px;
	padding: 0;

	.content {
		${styles.mixins.square("100%")};
		display: flex;
		align-items: stretch;

		* {
			align-content: center;
			width: 100%;
			padding: 4px 11px;
			text-align: center;

			&:not(:last-child) {
				border-right: 1px solid ${c("stroke-color-control-stroke-default")};
			}
		}
	}
`;

export default function PitchPicker({ pitch: [pitch, setPitch], ...htmlAttrs }: FCP<{
	/** Scientific pitch notation. */
	pitch: StateProperty<string>;
}, "button">) {
	const noteNameAndOctave = useMemo(() => {
		const groups = pitch?.match(/(?<noteName>[A-G][#♯b♭]?)(?<octave>\d+)/i)?.groups as undefined ?? { noteName: "", octave: "" };
		let noteName = groups.noteName
			.toUpperCase()
			.replaceAll("♯", "#")
			.replace(/(?<=[A-G])[b♭]/i, "b");
		if (noteName.endsWith("b"))
			noteName = {
				Db: "C#",
				Eb: "D#",
				Gb: "F#",
				Ab: "G#",
				Bb: "A#",
			}[noteName] ?? noteName;
		groups.noteName = noteName;
		return groups;
	}, [pitch]);

	async function showPitchPicker(e: MouseEvent) {
		const rect = getBoundingClientRectTuple(e.currentTarget);
		const result = await bridges.bridge.showPitchPicker(rect, pitch!);
		setPitch?.(result);
	}

	return (
		<StyledPitchPicker onClick={showPitchPicker} {...htmlAttrs}>
			<div className="content">
				<div>{noteNameAndOctave.noteName}</div>
				<div>{noteNameAndOctave.octave}</div>
			</div>
		</StyledPitchPicker>
	);
}
