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
			text-align: center;

			&:not(:last-child) {
				border-right: 1px solid ${c("stroke-color-control-stroke-default")};
			}
		}
	}
`;

export default function PitchPicker({ spn: [spn, setSpn], ...htmlAttrs }: FCP<{
	/** Scientific pitch notation. */
	spn: StateProperty<string>;
}, "button">) {
	const pitch = useMemo(() => new Pitch(spn!), [spn]);

	const showPitchPicker: MouseEventHandler<HTMLButtonElement> = async e => {
		const rect = e.currentTarget.getBoundingClientRect();
		const result = await bridges.bridge.showPitchPicker(rect, spn!);
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
			</div>
		</StyledPitchPicker>
	);
}
