import { StyledButton } from "./Button";
import { inputInSettingsCardStyle } from "./TextBox";

const StyledPitchPicker = styled(StyledButton)`
	align-items: stretch;
	height: 1px; // A hack way to cheat its children with 100% height to match its min height.
	padding: 0;
	direction: ltr;

	${inputInSettingsCardStyle}

	.content {
		${styles.mixins.square("100%")};
		display: flex;
		align-items: stretch;

		* {
			align-content: center;
			width: 100%;
			padding: 4px 11px;
			font-variant-numeric: tabular-nums;
			text-align: center;

			&:not(:last-child) {
				border-right: 1px solid ${c("stroke-color-control-stroke-default")};
			}
		}
	}
`;

const REFERENCE_PITCH = new Pitch("C5");

export default function PitchPicker({ spn: _spn, ...htmlAttrs }: FCP<{
	/** Scientific pitch notation. */
	spn: VariousState<string>;
}, "button">) {
	const [spn, setSpn] = useVariousState(_spn);
	const pitch = useMemo(() => new Pitch(spn), [spn]);
	const offset = pitch.offsetTo(REFERENCE_PITCH);

	const showPitchPicker: MouseEventHandler<HTMLButtonElement> = async e => {
		const rect = e.currentTarget.getBoundingClientRect();
		const result = await bridges.bridge.showPitchPicker(rect, spn);
		setSpn?.(result);
	};

	return (
		<StyledPitchPicker
			role="combobox"
			aria-label={pitch.ariaLabel}
			aria-haspopup="listbox"
			aria-expanded={false}
			onClick={showPitchPicker}
			{...htmlAttrs}
		>
			<div className="content" aria-hidden>
				<div>{pitch.noteName}</div>
				<div>{pitch.octave}</div>
				<div>{getPitchOffsetDisplayText(offset)}</div>
			</div>
		</StyledPitchPicker>
	);
}

function getPitchOffsetDisplayText(offset: number) {
	const sign = Math.sign(offset);
	return (sign > 0 ? "+" : sign < 0 ? "−" : "±") + Math.abs(offset);
}
