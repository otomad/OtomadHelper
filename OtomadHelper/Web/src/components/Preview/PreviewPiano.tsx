import colors from "styles/colors";

const intervalPattern = "全全半全全全半".replaceAll("全", "10").replaceAll("半", "1");
const LENGTH = 128;
const BLACK_KEY_WIDTH = 60;

const Wrapper = styled.div`
	overflow-x: auto;
	border-radius: 6px;

	> * {
		border-radius: inherit;

		> :first-child,
		> :first-child > :first-child {
			border-start-start-radius: inherit;
			border-end-start-radius: inherit;
		}
	}
`;

const StyledPreviewPiano = styled.div`
	display: flex;
	justify-content: stretch;
	align-items: stretch;
	height: 150px;
`;

export default function PreviewPiano({ ...htmlAttrs }: FCP<{}, "div">) {
	const [activeKey, setActiveKey] = useState("");

	return (
		<HorizontalScroll>
			<Wrapper>
				<StyledPreviewPiano {...htmlAttrs}>
					{forMap(LENGTH, i => {
						if (intervalPattern[(i + 1) % 12] === "0") return;
						const isBlackNext = intervalPattern[i % 12] === "0" && i < LENGTH - 2;
						return <PianoKey key={i} isBlackNext={isBlackNext} midiNote={i} showCOnly activeKey={[activeKey, setActiveKey]} />;
					})}
				</StyledPreviewPiano>
			</Wrapper>
		</HorizontalScroll>
	);
}

const fills = ["rgba(255, 255, 255, 0.7)", "rgba(69, 69, 69)"];
const fillsDark = ["rgba(255, 255, 255, 0.1)", "rgb(32, 32, 32)"] as const;
const strokes = colors["stroke-color-card-stroke-default"];
const strokeDarkWhite = "rgba(0, 0, 0, 0.12)";

const StyledPianoKey = styled.div`
	${styles.mixins.square("100%")};
	position: relative;

	button {
		--highlight: transparent;
		--level-highlight: 0%;
		display: flex;
		justify-content: center;
		align-items: flex-end;
		contain: strict;
		color: ${c("foreground-color")};
		font-variant-numeric: tabular-nums;
		background-color: color-mix(in srgb, var(--fill), var(--highlight) var(--level-highlight));
		border: 1px solid;

		&:hover {
			background-color: color-mix(in srgb, color-mix(in hsl, var(--fill), var(--mixed-hover) var(--level-hover, 10%)), var(--highlight) var(--level-highlight));
		}

		&:active {
			background-color: color-mix(in srgb, color-mix(in hsl, var(--fill), var(--mixed-active, var(--mixed-hover)) var(--level-active, 20%)), var(--highlight) var(--level-highlight));
		}
	}

	.white {
		${styles.mixins.square("100%")};
		--fill: ${fills[0]};
		--mixed-hover: black;
		--level-hover: 6.5%;
		--level-active: 13%;
		position: relative;
		z-index: 1;
		padding-bottom: 12px; // Avoid floating scrollbar taking up position.
		min-inline-size: 24px;
		border-color: ${strokes[0]};

		${ifColorScheme.dark} & {
			--fill: ${fillsDark[0]};
			--mixed-hover: white;
			--level-active: 11%;
			border-color: ${strokeDarkWhite};
		}

		&:focus-visible {
			z-index: 2;
		}
	}

	.black {
		${styles.mixins.square("100%")};
		--fill: ${fills[1]};
		--mixed-hover: white;
		--level-hover: 12%;
		--level-active: 24%;
		position: absolute;
		top: 0;
		right: ${-BLACK_KEY_WIDTH / 2}%;
		z-index: 3;
		width: ${BLACK_KEY_WIDTH}%;
		height: 50%;
		border-color: ${strokes[1]};
		border-radius: 0 0 4px 4px;

		${ifColorScheme.dark} & {
			--fill: ${fillsDark[1]};
			--level-hover: 6%;
			--level-active: 9%;
		}
	}

	.active {
		--fill: ${c("colorization")} !important;
		--mixed-hover: white;
		--mixed-active: black;
		--level-hover: 10%;
		--level-active: 10%;
		color: ${getClearColorFromBackgroundColor("colorization")};
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

	.default:not(.source, .active) {
		--highlight: ${c("fill-color-system-caution")};
		--level-highlight: 15%;
	}
`;

function PianoKey({ isBlackNext, midiNote, showCOnly, activeKey: [activeKey, setActiveKey] = [] }: {
	/** Is the next key a black key? */
	isBlackNext: boolean;
	/** MIDI note number. */
	midiNote?: number;
	/** Show note name with C only? */
	showCOnly?: boolean;
	/** Active key. */
	activeKey?: StateProperty<string>;
}) {
	let whiteSpn = "", blackSpn = "", whiteSpnShown = "", blackSpnShown = "";
	if (midiNote !== undefined) {
		whiteSpn = whiteSpnShown = midiNoteToSPN(midiNote - (isBlackNext ? 1 : 0));
		blackSpn = blackSpnShown = midiNoteToSPN(midiNote);
	}
	if (showCOnly) {
		if (!isNoteNameC(whiteSpn)) whiteSpnShown = "";
		if (!isNoteNameC(blackSpn)) blackSpnShown = "";
	}

	// FOR TEST ONLY!
	const hasSource = whiteSpn.endsWith("5");
	const hasDefault = whiteSpn.endsWith("3");

	return (
		<StyledPianoKey>
			<button type="button" className={["white", { active: activeKey === whiteSpn, source: hasSource, default: hasDefault }]} onClick={() => setActiveKey?.(whiteSpn)}>{whiteSpnShown}</button>
			{isBlackNext && <button type="button" className={["black", { active: activeKey === blackSpn, source: hasSource, default: hasDefault }]} onClick={() => setActiveKey?.(blackSpn)}>{blackSpnShown}</button>}
		</StyledPianoKey>
	);
}

function isNoteNameC(noteName: string) {
	return noteName.startsWith("C") && !noteName.startsWith("C#");
}
