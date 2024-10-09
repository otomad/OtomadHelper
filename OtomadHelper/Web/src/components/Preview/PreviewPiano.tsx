import colors from "styles/colors";

const intervalPattern = "全全半全全全半".replaceAll("全", "10").replaceAll("半", "1");
const LENGTH = 128;
const BLACK_KEY_WIDTH = 60;

const Wrapper = styled.div`
	overflow-x: auto;
	border-radius: 6px;
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

const textColors = colors["foreground-color"];
const fills = colors["fill-color-control-solid-default"];
const fillsHover = colors["fill-color-control-solid-secondary"];
const fillsPressed = colors["fill-color-control-solid-tertiary"];
const strokes = colors["stroke-color-control-stroke-default"];

const StyledPianoKey = styled.div`
	${styles.mixins.square("100%")};
	position: relative;

	button {
		display: flex;
		justify-content: center;
		align-items: flex-end;
		contain: strict;
		font-variant-numeric: tabular-nums;
		border: 1px solid;
	}

	.white {
		${styles.mixins.square("100%")};
		position: relative;
		z-index: 1;
		padding-bottom: 12px; // Avoid floating scrollbar taking up position.
		min-inline-size: 24px;
		color: ${textColors[0]};
		background-color: ${fills[0]};
		border-color: ${strokes[0]};

		&:hover {
			background-color: ${fillsHover[0]};
		}

		&:active {
			background-color: ${fillsPressed[0]};
		}

		&:focus-visible {
			z-index: 2;
		}
	}

	.black {
		${styles.mixins.square("100%")};
		position: absolute;
		top: 0;
		right: ${-BLACK_KEY_WIDTH / 2}%;
		z-index: 3;
		width: ${BLACK_KEY_WIDTH}%;
		height: 50%;
		color: ${textColors[1]};
		background-color: ${fills[1]};
		border-color: ${strokes[1]};
		border-radius: 0 0 4px 4px;

		&:hover {
			background-color: ${fillsHover[1]};
		}

		&:active {
			background-color: ${fillsPressed[1]};
		}
	}

	.active {
		color: ${c("fill-color-text-on-accent-primary")};
		background-color: ${c("accent-color")};

		&:hover {
			background-color: ${c("accent-color", 90)};
		}

		&:active {
			background-color: ${c("accent-color", 80)};
		}
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

	return (
		<StyledPianoKey>
			<button type="button" className={["white", { active: activeKey === whiteSpn }]} onClick={() => setActiveKey?.(whiteSpn)}>{whiteSpnShown}</button>
			{isBlackNext && <button type="button" className={["black", { active: activeKey === blackSpn }]} onClick={() => setActiveKey?.(blackSpn)}>{blackSpnShown}</button>}
		</StyledPianoKey>
	);
}

function isNoteNameC(noteName: string) {
	return noteName.startsWith("C") && !noteName.startsWith("C#");
}
