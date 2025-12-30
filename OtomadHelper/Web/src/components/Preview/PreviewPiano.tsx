import colors from "styles/colors";

/**
 * Represents the interval pattern for a musical scale, where "全" (whole step) is replaced with "10"
 * and "半" (half step) is replaced with "1". The resulting string encodes the sequence of intervals.
 *
 * In the encoded string, "1" represents the white keys on the piano keyboard, and "0" represents the
 * black keys on the piano keyboard.
 *
 * Example:
 * - Original: "全全半全全全半"
 * - Encoded:  "101011010101"
 */
const intervalPattern = "全全半全全全半".replaceAll("全", "10").replaceAll("半", "1");
const LENGTH = 128;
const BLACK_KEY_WIDTH = 60;

const Wrapper = styled(HorizontalScroll)`
	flex-shrink: 0;
	overflow-inline: auto;
	border-radius: 6px;

	> * {
		border-radius: inherit;

		> :first-child,
		> :first-child > :first-child {
			${styles.mixins.inherit("border-start-start-radius", "border-end-start-radius")};
		}
	}

	&:hover {
		will-change: scroll-position;
	}

	.expander-child-items & {
		padding: 0 !important;
		border-block-start: none !important;
		border-radius: 0;
	}
`;

const StyledPreviewPiano = styled.div`
	display: flex;
	justify-content: stretch;
	align-items: stretch;
	height: 150px;
`;

const PIANO_INSTRUCTIONS_ID = "piano-instructions";

export default function PreviewPiano({ activeKeys: _activeKeys, sourceKeys = [], fallbackKeys = [], showKeyLabels = [], onClick: _onClick, onMouseDown, onSpaceEnterKeyDown, onSpaceEnterKeyUp, ...htmlAttrs }: FCP<{
	/** Active keys. */
	activeKeys?: string[];
	/** Source keys. */
	sourceKeys?: string[];
	/** Fallback-default keys. */
	fallbackKeys?: string[];
	/** Provide keys that will force to show the pitch note name if `showCOnly` is true. */
	showKeyLabels?: string[];
	/** Occurs when the piano key is clicked. */
	onClick?(spn: string): void;
	/** Occurs when the piano key is pressed, or mouse entered while pressing. */
	onMouseDown?(spn: string): void;
	/** Occurs when the piano key is focused and the keyboard key Space or Enter is pressed. */
	onSpaceEnterKeyDown?(spn: string): void;
	/** Occurs when the piano key is focused and the keyboard key Space or Enter is released. */
	onSpaceEnterKeyUp?(spn: string): void;
}, "div">) {
	// Demo mode, if provide no props.
	const [internalActiveKeys, setInternalActiveKeys] = useState<string[]>([]);
	const demoMode = !_activeKeys && !_onClick && !onMouseDown;
	const activeKeys = !demoMode ? _activeKeys : internalActiveKeys;
	const onClick: typeof _onClick = !demoMode ? _onClick : spn => setInternalActiveKeys([spn]);

	return (
		<Wrapper>
			<StyledPreviewPiano role="application" aria-label={t.aria.previewPiano.pianoKeyboard} {...htmlAttrs}>
				<SrOnly id={PIANO_INSTRUCTIONS_ID}>{t.aria.previewPiano.instructions}</SrOnly>
				{forMap(LENGTH, i => {
					if (intervalPattern[(i + 1) % 12] === "0") return;
					const isBlackNext = intervalPattern[i % 12] === "0" && i < LENGTH - 1;
					return (
						<PianoKey
							key={i}
							isBlackNext={isBlackNext}
							midiNote={i}
							showCOnly
							activeKeys={activeKeys}
							sourceKeys={sourceKeys}
							fallbackKeys={fallbackKeys}
							showKeyLabels={showKeyLabels}
							aria-describedby={PIANO_INSTRUCTIONS_ID}
							onClick={onClick}
							onMouseDown={onMouseDown}
							onSpaceEnterKeyDown={onSpaceEnterKeyDown}
							onSpaceEnterKeyUp={onSpaceEnterKeyUp}
						/>
					);
				})}
			</StyledPreviewPiano>
		</Wrapper>
	);
}

const fills = ["rgba(255, 255, 255, 0.7)", "rgba(69, 69, 69)"];
const fillsDark = ["rgba(255, 255, 255, 0.1)", "rgb(32, 32, 32)"] as const;
const strokes = colors["stroke-color-card-stroke-default"];
const strokeDarkWhite = "rgba(0, 0, 0, 0.12)";

const StyledPianoKey = styled.div`
	${styles.effects.text.caption};
	${styles.mixins.square("100%")};
	position: relative;

	button {
		--highlight: transparent;
		--level-highlight: 0%;
		display: flex;
		justify-content: center;
		align-items: end;
		/* contain: strict; */
		color: ${c("foreground-color")};
		/* font-variant-numeric: tabular-nums; */
		background-color: color-mix(in srgb, var(--fill), var(--highlight) var(--level-highlight));
		background-clip: border-box;
		border: 1px solid;

		&:hover {
			background-color: color-mix(in srgb, color-mix(in hsl, var(--fill), var(--mixed-hover) var(--level-hover, 10%)), var(--highlight) var(--level-highlight));
		}
	}

	&:not(.has-mouse-down) button:active,
	button:hover:active {
		background-color: color-mix(in srgb, color-mix(in hsl, var(--fill), var(--mixed-active, var(--mixed-hover)) var(--level-active, 20%)), var(--highlight) var(--level-highlight));
	}

	.white {
		${styles.mixins.square("100%")};
		--fill: light-dark(${fills[0]}, ${fillsDark[0]});
		--mixed-hover: light-dark(black, white);
		--level-hover: 6.5%;
		--level-active: --light-dark(13%, 11%);
		position: relative;
		z-index: 1;
		min-inline-size: 24px;
		padding-block-end: 12px; // Avoid floating scrollbar taking up position.
		border-color: light-dark(${strokes[0]}, ${strokeDarkWhite});

		&:focus-visible {
			z-index: 2;
		}
	}

	.black {
		${styles.mixins.square("100%")};
		--fill: light-dark(${fills[1]}, ${fillsDark[1]});
		--mixed-hover: white;
		--level-hover: --light-dark(12%, 6%);
		--level-active: --light-dark(24%, 9%);
		position: absolute;
		top: 0;
		right: ${-BLACK_KEY_WIDTH / 2}%;
		z-index: 3;
		width: ${BLACK_KEY_WIDTH}%;
		height: 50%;
		border-color: ${strokes[1]};
		border-radius: 0 0 4px 4px;
	}

	.active {
		--fill: ${c("colorization")} !important;
		--mixed-hover: white;
		--mixed-active: black;
		--level-hover: 10%;
		--level-active: 10%;
		color: --contrast-color(${c("colorization")});
	}

	&:not(:last-child, :hover, :has(> .white.active)) > .white {
		border-inline-end-color: transparent;
	}

	&:has(> .white:is(:hover, .active)) + * > .white {
		border-inline-start-color: transparent;
	}

	.source:not(.active) {
		--highlight: ${c("colorization")};
		--level-highlight: 15%;
	}

	.fallback:not(.source, .active) {
		--highlight: ${c("fill-color-system-caution")};
		--level-highlight: 15%;
	}

	&:not(.has-mouse-down) .is-not-c:not(:hover, :active),
	.is-not-c:not(:hover, :hover:active) {
		color: transparent;
		content-visibility: hidden;
		transition-behavior: allow-discrete;
	}
`;

function PianoKey({ isBlackNext, midiNote, showCOnly, activeKeys = [], sourceKeys = [], fallbackKeys = [], showKeyLabels = [], "aria-describedby": ariaDescribedby, onClick, onMouseDown, onSpaceEnterKeyDown, onSpaceEnterKeyUp }: {
	/** Is the next key a black key? */
	isBlackNext: boolean;
	/** MIDI note number. */
	midiNote?: number;
	/** Show note name with C only? */
	showCOnly?: boolean;
	/** Active keys. */
	activeKeys?: string[];
	/** Source keys. */
	sourceKeys?: string[];
	/** Fallback-default keys. */
	fallbackKeys?: string[];
	/** Provide keys that will force to show the pitch note name if `showCOnly` is true. */
	showKeyLabels?: string[];
	/** Identifies the element (or elements) that describes the object. */
	"aria-describedby"?: string;
	/** Occurs when the key is clicked. */
	onClick?(spn: string): void;
	/** Occurs when the key is pressed, or mouse entered while pressing. */
	onMouseDown?(spn: string): void;
	/** Occurs when the piano key is focused and the keyboard key Space or Enter is pressed. */
	onSpaceEnterKeyDown?(spn: string): void;
	/** Occurs when the piano key is focused and the keyboard key Space or Enter is released. */
	onSpaceEnterKeyUp?(spn: string): void;
}) {
	let whiteSpn = "", blackSpn = "";
	if (midiNote !== undefined) {
		whiteSpn = midiNoteToSPN(midiNote - (isBlackNext ? 1 : 0));
		blackSpn = midiNoteToSPN(midiNote);
	}
	const isLastWhiteKey = midiNote === 126;
	const whitePitch = new Pitch(whiteSpn);
	const getAriaLabel = (sharp = false) =>
		t.aria.previewPiano.spn({ context: sharp ? "sharp" : "natural", noteName: whitePitch.noteName, octave: whitePitch.octave });

	return (
		<StyledPianoKey className={{ hasMouseDown: !!onMouseDown }}>
			<button
				type="button"
				className={["white", {
					active: activeKeys.includes(whiteSpn),
					source: sourceKeys.includes(whiteSpn),
					fallback: fallbackKeys.includes(whiteSpn),
					isNotC: showCOnly && !isNoteNameC(whiteSpn) && !showKeyLabels.includes(whiteSpn),
				}]}
				aria-label={getAriaLabel()}
				aria-pressed={activeKeys.includes(whiteSpn) || sourceKeys.includes(whiteSpn)}
				aria-describedby={ariaDescribedby}
				onClick={() => onClick?.(whiteSpn)}
				onMouseDown={leftDownModifier(() => onMouseDown?.(whiteSpn))}
				onMouseEnter={leftDownModifier(() => onMouseDown?.(whiteSpn))}
				onKeyDown={nonRepeatedSpaceEnterKeyModifier(() => onSpaceEnterKeyDown?.(whiteSpn))}
				onKeyUp={nonRepeatedSpaceEnterKeyModifier(() => onSpaceEnterKeyUp?.(whiteSpn))}
			>
				{whiteSpn}
			</button>
			{isBlackNext && !isLastWhiteKey && (
				<button
					type="button"
					className={["black", {
						active: activeKeys.includes(blackSpn),
						source: sourceKeys.includes(blackSpn),
						fallback: fallbackKeys.includes(blackSpn),
						isNotC: showCOnly && !isNoteNameC(blackSpn) && !showKeyLabels.includes(blackSpn),
					}]}
					aria-label={getAriaLabel(true)}
					aria-pressed={activeKeys.includes(blackSpn) || sourceKeys.includes(blackSpn)}
					aria-describedby={ariaDescribedby}
					onClick={() => onClick?.(blackSpn)}
					onMouseDown={leftDownModifier(() => onMouseDown?.(blackSpn))}
					onMouseEnter={leftDownModifier(() => onMouseDown?.(blackSpn))}
					onKeyDown={nonRepeatedSpaceEnterKeyModifier(() => onSpaceEnterKeyDown?.(blackSpn))}
					onKeyUp={nonRepeatedSpaceEnterKeyModifier(() => onSpaceEnterKeyUp?.(blackSpn))}
				>
					{blackSpn}
				</button>
			)}
		</StyledPianoKey>
	);
}

function isNoteNameC(noteName: string) {
	return noteName.match(/^C(?!#)/i);
}

function leftDownModifier(handler: MouseEventHandler<HTMLButtonElement>): MouseEventHandler<HTMLButtonElement> {
	return e => {
		if (e.buttons === 1) handler(e);
	};
}

function nonRepeatedSpaceEnterKeyModifier(handler: KeyboardEventHandler<HTMLButtonElement>): KeyboardEventHandler<HTMLButtonElement> {
	return e => {
		if (e.repeat) return;
		if (["Space", "Enter", "NumpadEnter"].includes(e.code)) handler(e);
	};
}
