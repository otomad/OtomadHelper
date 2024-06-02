import { StyledTextBox } from "./TextBox";

const SPINNER_BUTTON_HEIGHT = 30, TEXTBOX_BASE_HEIGHT = 32, BUTTON_BORDER_RADIUS = 3;

const StyledTimecodeBox = styled.div`
	position: relative;
	display: grid;
	grid-auto-flow: column;
	grid-template-rows: repeat(3, auto);
	justify-content: center;
	place-items: center;
	font-variant-numeric: tabular-nums;
	direction: ltr; // Arabic, Hebrew... won't reverse time code

	.mark {
		grid-row: span 3;

		&:is(.text-box + .mark) {
			margin-inline-start: 8px;
		}
	}

	button {
		width: calc(100% - 6px);
		min-width: unset;
		height: ${SPINNER_BUTTON_HEIGHT - 2}px;
		min-height: unset;
		border-radius: ${BUTTON_BORDER_RADIUS}px;

		&.up {
			margin-block-end: 2px;

			&:is(:active, [data-pressing]) .icon {
				translate: 0 -2px;
			}
		}

		&.down {
			margin-block-start: 2px;

			&:is(:active, [data-pressing]) .icon {
				translate: 0 2px;
			}
		}
	}

	.value-wrapper {
		height: ${TEXTBOX_BASE_HEIGHT}px;
		padding: 3px;
		padding-block: 2px 4px;

		.value {
			${styles.effects.text.body};
			${styles.mixins.flexCenter()};
			height: 100%;
			padding-inline: 5px;
			border-radius: ${BUTTON_BORDER_RADIUS}px;

			&:focus {
				background-color: ${c("fill-color-control-default")};
			}
		}
	}

	> * {
		${styles.mixins.square("100%")};
		place-content: center;
		text-align: center;
	}

	${StyledTextBox} {
		position: absolute;
		z-index: -1;
		height: ${TEXTBOX_BASE_HEIGHT}px;
		pointer-events: none;
	}
`;

export default function TimecodeBox({ timecode: [timecode, setTimecode], ...htmlAttrs }: FCP<{
	/** The current time code or time span. */
	timecode: StateProperty<string>;
}, "div">) {
	const timecodeBoxEl = useDomRef<"div">();

	const lastActiveItemLastIndex = useRef<number>();

	const tokens = useMemo(() => getTimecodeTokens(timecode), [timecode]);

	const focusValue = useDebounceCallback((itemLastIndex?: number) =>
		setTimeout(() => timecodeBoxEl.current?.querySelector<HTMLElement>(
			itemLastIndex !== undefined ? `[data-last-index="${itemLastIndex}"]` : "[data-last-index]",
		)?.focus()), []);

	function handleSpinnerClick(itemLastIndex: number, step: number) {
		(setTimecode as SetStateNarrow<string>)?.(timecode => {
			const stepTimecodeTokens = getTimecodeTokens(timecode.replace(/^-/, ""));
			stepTimecodeTokens.forEach(item => item.token === "digit" && (item.value = "0".padStart(item.value.length, "0")));
			const selectedItem = stepTimecodeTokens.at(itemLastIndex);
			selectedItem && (selectedItem.value = String(Math.abs(step)).padStart(selectedItem.value.length, "0"));
			const stepTimecode = (step < 0 ? "-" : "") + stepTimecodeTokens.join("");
			const supposedTimecode = getSupposedTimecode(timecode, stepTimecode);
			return supposedTimecode;
		});
		lastActiveItemLastIndex.current = itemLastIndex;
		focusValue(itemLastIndex);
	}

	const handleTimecodeBoxMouseDown = useCallback<MouseEventHandler>(e => {
		if (e.target instanceof HTMLElement && e.target.dataset.lastIndex !== undefined) {
			const lastIndex = +e.target.dataset.lastIndex;
			if (Number.isFinite(lastIndex)) lastActiveItemLastIndex.current = lastIndex;
		} else if (!isInPath(e.target, ".value", "button"))
			focusValue(lastActiveItemLastIndex.current);
	}, []);

	return (
		<StyledTimecodeBox ref={timecodeBoxEl} onMouseDown={handleTimecodeBoxMouseDown} {...htmlAttrs}>
			<StyledTextBox>
				<div className="stripes">
					<div className="focus-stripe" />
				</div>
			</StyledTextBox>
			{tokens.map(({ value, token }, index) => {
				const lastIndex = -(tokens.length - index);
				return token === "mark" ? <div className="mark" key={lastIndex}>{optimizeMarks(value)}</div> : (
					<Fragment key={lastIndex}>
						<Button
							subtle
							icon="spinner/chevron_up"
							className="up"
							repeat
							tabIndex={-1}
							onClick={() => handleSpinnerClick(lastIndex, 1)}
						/>
						<div className="value-wrapper">
							<div className="value" data-last-index={lastIndex} tabIndex={0}>{value}</div>
						</div>
						<Button
							subtle
							icon="spinner/chevron_down"
							className="down"
							repeat
							tabIndex={-1}
							onClick={() => handleSpinnerClick(lastIndex, -1)}
						/>
					</Fragment>
				);
			})}
		</StyledTimecodeBox>
	);
}

function getSupposedTimecode(input?: string, increment?: string) {
	function getSeconds(timecode?: string) {
		const hms = timecode?.match(/(((?<h>[\d-]+):)?(?<m>[\d-]+):)?(?<s>[\d-]*\.[\d-]+|[\d-]+)/)?.groups as
			Record<"h" | "m" | "s", string | undefined> ?? {};
		const negative = !(+(hms.h || 0)).isPositive;
		const seconds = +(hms.s || 0) + +(hms.m || 0) * 60 + Math.abs(+(hms.h || 0)) * 3600;
		return seconds * (negative ? -1 : 1);
	}

	let seconds = getSeconds(input);
	if (increment)
		seconds += getSeconds(increment);

	const negative = seconds < 0;
	seconds = Math.abs(seconds);
	const [s, ms] = (seconds % 60).toFixed(3).split("."),
		m = String(seconds / 60 % 60 | 0), h = String(seconds / 3600 | 0);
	const padStart = (numberString: string) => numberString.padStart(2, "0");
	return `${negative ? "-" : ""}${padStart(h)}:${padStart(m)}:${padStart(s)}.${ms}`;
}

function optimizeMarks(marks: string) {
	return marks.replaceAll(":", "∶").replaceAll("-", "−");
}

function getTimecodeTokens(timecode?: string) {
	const supposedTimecode = getSupposedTimecode(timecode);
	type Token = "digit" | "mark";
	const tokens: { value: string; token: Token }[] = [];
	tokens[Symbol.toPrimitive] = function () { return this.join(""); };
	function addItem(value: string, token: Token | "") {
		if (token === "") return;
		const item = { value, token, [Symbol.toPrimitive]() { return this.value; } };
		tokens.push(item);
	}
	let value = "", token: Token | "" = "";
	for (const char of supposedTimecode) {
		const charToken: Token = char >= "0" && char <= "9" ? "digit" : "mark";
		if (charToken !== token) {
			addItem(value, token);
			value = "";
		}
		token = charToken;
		value += char;
	}
	if (value.length) addItem(value, token);
	return tokens;
}
