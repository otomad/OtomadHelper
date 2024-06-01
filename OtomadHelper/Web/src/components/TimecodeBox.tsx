import { StyledTextBox } from "./TextBox";

const SPINNER_BUTTON_HEIGHT = 30, TEXTBOX_BASE_HEIGHT = 32;

const StyledTimecodeBox = styled.div`
	position: relative;
	display: grid;
	grid-auto-flow: column;
	grid-template-rows: repeat(3, auto);
	justify-content: start;
	place-items: center;
	font-variant-numeric: tabular-nums;

	.mark {
		grid-row: span 3;

		&:nth-child(2) {
			margin-inline-start: 8px;
		}
	}

	button {
		min-width: unset;
		height: ${SPINNER_BUTTON_HEIGHT}px;

		&.up:active .icon {
			translate: 0 -2px;
		}

		&.down:active .icon {
			translate: 0 2px;
		}
	}

	.value {
		${styles.effects.text.body};
		padding-inline: 8px;
		line-height: ${TEXTBOX_BASE_HEIGHT}px;
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
	const tokens = useMemo(() => getTimecodeTokens(timecode), [timecode]);

	function handleSpinnerClick(itemIndex: number, step: number) {
		(setTimecode as SetStateNarrow<string>)?.(timecode => {
			if (timecode?.startsWith("-")) step = -step;
			const values = getTimecodeTokens(timecode).map(item => item.value);
			values[itemIndex] = String(+values[itemIndex] + step);
			const supposedTimecode = getSupposedTimecode(values.join(""));
			return supposedTimecode;
		});
	}

	return (
		<StyledTimecodeBox {...htmlAttrs}>
			<StyledTextBox>
				<div className="stripes">
					<div className="focus-stripe" />
				</div>
			</StyledTextBox>
			{tokens.map(({ value, token }, i) => {
				const key = tokens.length - i;
				return token === "mark" ? <div className="mark" key={key}>{optimizeMarks(value)}</div> : (
					<Fragment key={key}>
						<Button subtle icon="spinner/chevron_up" className="up" repeat onClick={() => handleSpinnerClick(i, 1)} />
						<div className="value">{value}</div>
						<Button subtle icon="spinner/chevron_down" className="down" repeat onClick={() => handleSpinnerClick(i, -1)} />
					</Fragment>
				);
			})}
		</StyledTimecodeBox>
	);
}

function getSupposedTimecode(input?: string) {
	const hms = input?.match(/(((?<h>[\d-]+):)?(?<m>[\d-]+):)?(?<s>[\d-]*\.[\d-]+|[\d-]+)/)?.groups as
		Record<"h" | "m" | "s", string | undefined> ?? {};
	let negative = Object.values(hms).filter(value => value?.startsWith("-")).length % 2 !== 0;
	const seconds = Math.abs(+(hms.s || 0) + +(hms.m || 0) * 60 + +(hms.h || 0) * 3600);
	if (seconds === 0) negative = false;
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
	let value = "", token: Token | "" = "";
	for (const char of supposedTimecode) {
		const charToken: Token = char >= "0" && char <= "9" ? "digit" : "mark";
		if (charToken !== token && token !== "") {
			tokens.push({ value, token });
			value = "";
		}
		token = charToken;
		value += char;
	}
	if (value.length && token !== "") tokens.push({ value, token });
	return tokens;
}
