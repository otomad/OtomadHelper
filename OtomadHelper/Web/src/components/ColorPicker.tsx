import { StyledButton } from "./Button";

const StyledColorPicker = styled(StyledButton).attrs({
	$fillColorName: "accent-color",
	$subtleFillColorName: "accent-color",
})`
	min-width: unset;
	aspect-ratio: 1 / 1;

	input {
		visibility: hidden;
	}
`;

export default function ColorPicker({ color: [color, setColor], ...htmlAttrs }: FCP<{
	/** Color. */
	color: StateProperty<string>;
	children?: never;
}, "button">) {
	const inputColorEl = useDomRef<"input">();

	return (
		<StyledColorPicker {...htmlAttrs} style={{ backgroundColor: color }} onClick={() => inputColorEl.current?.click()}>
			<input ref={inputColorEl} type="color" onChange={e => setColor?.(e.currentTarget.value)} />
		</StyledColorPicker>
	);
}
