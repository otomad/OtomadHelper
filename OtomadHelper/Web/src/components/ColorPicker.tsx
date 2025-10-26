import { StyledButton } from "./Button";

const PADDING = 3;

const StyledColorButton = styled(StyledButton)`
	position: relative;
	min-inline-size: 40px;
	aspect-ratio: 1 / 1;
	margin: ${-PADDING}px !important;
	padding: ${PADDING}px;
	border: 1px solid transparent;

	&,
	.fill {
		${styles.mixins.circle()};
	}

	input {
		visibility: hidden;
	}

	.fill {
		position: absolute;
		inset: ${PADDING}px;
		background-color: ${c("color")};
		border-radius: inherit;
		box-shadow: 0 0 0 1px --contrast-color(var(--color), 0.37) inset;

		&.spectrum {
			--background-color: light-dark(white, black);
			background:
				radial-gradient(closest-side, ${c("background-color")}, transparent),
				conic-gradient(in oklch longer hue, red, red);
			background-color: ${c("color")};
			box-shadow: 0 0 0 1px ${c("foreground-color", 37)} inset;
		}
	}

	.icon,
	.animated-icon {
		position: absolute;
		color: --contrast-color(var(--color));
		font-size: 16px;

		&:is(.spectrum ~ *) {
			color: ${c("foreground-color")};
		}
	}

	.icon:not(.animating, :hover),
	.animated-icon:not(.animating) {
		opacity: 0;
	}

	&:hover {
		border-color: ${c("stroke-color-control-stroke-secondary")};

		.animated-icon {
			opacity: 1;
		}

		.fill {
			opacity: 0.9;
		}
	}

	&:active {
		background-color: ${c("fill-color-subtle-secondary")};
		border-color: ${c("stroke-color-control-stroke-tertiary")};

		.animated-icon {
			--state: pressed;
			opacity: ${c("pressed-text-opacity")};
		}

		.fill {
			opacity: 0.8;
		}
	}

	&:not(:hover, :active) {
		background: none;

		&:not(:focus-visible) {
			box-shadow: none !important;
		}
	}

	&[disabled] {
		opacity: 0.5;
		filter: grayscale(0.5);
	}

	&[aria-checked="true"] {
		border-color: ${c("stroke-color-focus-stroke-outer")};

		// Change selected focused outline color.
		&[data-selected-outline-color="colored"] {
			border-color: lch(from ${c("color")} 50 c h);
		}

		&[data-selected-outline-color]:not([data-selected-outline-color="colored"]) {
			border-color: attr(data-selected-outline-color type(<color>));
		}
	}
`;

export function ColorButton({ color, icon, animatedIcon, selected = false, value: [value, setValue] = NEVER_MIND, showIconWhenHovering = false, colorAlt, showSpectrum = false, autoStartViewTransition, selectedOutlineColor, style, role = "radio", onClick, children, ...htmlAttrs }: FCP<{
	/** Color. */
	color?: string;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Animated icon. */
	animatedIcon?: DeclaredLotties;
	/** Selected? */
	selected?: boolean;
	/** Model value. Current color and set the color. */
	value?: StateProperty<string>;
	/** Show the icon or animated icon only when hovering? */
	showIconWhenHovering?: boolean;
	/** If specified, the visual color will be replaced, but the internal logic will still use the original color. */
	colorAlt?: string;
	/** Show color spectrum? */
	showSpectrum?: boolean;
	/** Auto start color palette view transition? */
	autoStartViewTransition?: boolean;
	/**
	 * Specify the selected outline color.
	 * - `undefined`: Use default focus stroke outer color.
	 * - `"colored"`: Use the same color as `color` prop.
	 * - `string`: Custom outline color.
	 * @default undefined
	 */
	selectedOutlineColor?: "colored" | (string & {});
}, "button">) {
	const [isIconAnimating, setIsIconAnimating] = useState(false);
	// The edit icon will keep showing until the animation finishes playing.

	if (value !== undefined) selected ||= value === color;

	const handleClick: MouseEventHandler<HTMLButtonElement> = async e => {
		if (autoStartViewTransition && !selected) {
			emit("app:startColorPaletteViewTransition");
			await delay(0);
		}
		onClick?.(e);
		if (color) setValue?.(color);
	};

	return (
		<StyledColorButton
			{...htmlAttrs}
			style={{ ...style, "--color": colorAlt ?? color }}
			aria-checked={selected}
			role={role}
			type="button"
			data-selected-outline-color={selectedOutlineColor}
			onClick={handleClick}
		>
			<div className={["fill", { spectrum: showSpectrum }]} />
			{animatedIcon ? <AnimatedIcon name={animatedIcon} className={{ animating: !showIconWhenHovering || isIconAnimating }} onPlayStateChange={setIsIconAnimating} /> :
			icon && <Icon name={icon} className={{ animating: !showIconWhenHovering }} />}
			{children}
		</StyledColorButton>
	);
}

const DEFAULT_COLOR = "#000000";
const toHex = (color: string = "black") => {
	try {
		return new Color(color).toString({ format: "hex", collapse: false });
	} catch {
		return DEFAULT_COLOR;
	}
};

export default function ColorPicker({ color: [color, setColor], computedColor, role = "button", showIconWhenHovering = true, showSpectrumWhenUnselected = false, selected, autoStartViewTransition, ...htmlAttrs }: FCP<Override<PropsOf<typeof ColorButton>, {
	/** Color. */
	color: StateProperty<string>;
	/** If the color is dynamic generated, then get the correct color. */
	computedColor?(): string;
	/** Show color spectrum when it is not selected? */
	showSpectrumWhenUnselected?: boolean;
	children?: never;
}>>) {
	const inputColorEl = useDomRef<"input">();
	// const [_correctColor, setCorrectColor] = useState(color);
	// const correctColor = toHex(computedColor ? _correctColor : color);
	const correctColor = useMemo(() => {
		return toHex(computedColor?.() ?? color);
	}, [color, computedColor]);

	const setColorDelayed = async (color: string) => {
		if (!setColor) return;
		if (autoStartViewTransition) {
			emit("app:startColorPaletteViewTransition");
			await delay(0);
		}
		setColor(color);
	};

	const handleClick: MouseEventHandler = async e => {
		e.stopPropagation();
		if (window.isWebView) {
			const [ok, newHex] = await bridges.bridge.showColorPicker(correctColor || DEFAULT_COLOR);
			if (ok) setColorDelayed(newHex);
		} else inputColorEl.current?.click();
	};

	return (
		<ColorButton
			{...htmlAttrs}
			color={correctColor}
			animatedIcon="edit"
			role={role}
			selected={selected}
			showIconWhenHovering={showIconWhenHovering}
			showSpectrum={!selected && showSpectrumWhenUnselected}
			onClick={handleClick}
		>
			{!window.isWebView && <input ref={inputColorEl} type="color" value={correctColor} onChange={e => setColorDelayed(e.currentTarget.value)} />}
		</ColorButton>
	);
}
