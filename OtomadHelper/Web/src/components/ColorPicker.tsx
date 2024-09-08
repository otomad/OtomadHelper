import { StyledButton } from "./Button";

const StyledColorPicker = styled(StyledButton).attrs({
	$fillColorName: "accent-color",
	$subtleFillColorName: "accent-color",
})`
	min-inline-size: unset;
	aspect-ratio: 1 / 1;
	background-color: ${c("color")} !important;
	outline: 1px solid ${getClearColorFromBackgroundColor("color")} !important;

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
		<StyledColorPicker {...htmlAttrs} style={{ "--color": color }} onClick={() => inputColorEl.current?.click()}>
			<input ref={inputColorEl} type="color" value={color} onChange={e => setColor?.(e.currentTarget.value)} />
		</StyledColorPicker>
	);
}
