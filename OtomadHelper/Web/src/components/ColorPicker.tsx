import { StyledButton } from "./Button";

const StyledColorPicker = styled(StyledButton).attrs({
	$fillColorName: "accent-color",
	$subtleFillColorName: "accent-color",
})`
	aspect-ratio: 1 / 1;
	min-inline-size: unset;
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

	const handleClick: MouseEventHandler = async e => {
		e.stopPropagation();
		if (window.isWebView) setColor?.(await bridges.bridge.showColorPicker(color || "#000000"));
		else inputColorEl.current?.click();
	};

	return (
		<StyledColorPicker {...htmlAttrs} style={{ "--color": color }} onClick={handleClick}>
			{!window.isWebView && <input ref={inputColorEl} type="color" value={color} onChange={e => setColor?.(e.currentTarget.value)} />}
		</StyledColorPicker>
	);
}
