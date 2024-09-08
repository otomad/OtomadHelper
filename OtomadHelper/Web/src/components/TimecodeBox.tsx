import { StyledTextBox } from "./TextBox";

const SPINNER_BUTTON_HEIGHT = 30, TEXTBOX_BASE_HEIGHT = 32, BUTTON_BORDER_RADIUS = 2;
const isPressed = ":is(:active, [data-pressed])";

const StyledTimecodeBox = styled.div`
	@layer props {
		/** The layout of the timecode box, expected: center | left | inline. */
		--layout: center;
	}

	position: relative;
	display: grid;
	grid-auto-flow: column;
	grid-template-rows: repeat(3, auto);
	justify-content: center;
	place-items: center;
	font-variant-numeric: tabular-nums;
	direction: ltr; // Arabic, Hebrew... won't reverse time code


	@container style(--layout: left) {
		justify-content: flex-start;
	}

	@container style(--layout: inline) {
		inline-size: fit-content;
	}

	.mark {
		grid-row: span 3;

		&:is(.text-box + .mark) {
			margin-inline-start: 8px;
		}
	}

	button {
		width: calc(100% - 6px);
		min-inline-size: unset;
		height: ${SPINNER_BUTTON_HEIGHT - 2}px;
		min-height: unset;
		border-radius: ${BUTTON_BORDER_RADIUS}px;

		&.up {
			margin-block-end: 2px;

			&${isPressed} .icon {
				translate: 0 -2px;
			}
		}

		&.down {
			margin-block-start: 2px;

			&${isPressed} .icon {
				translate: 0 2px;
			}
		}
	}

	.value-wrapper {
		height: ${TEXTBOX_BASE_HEIGHT}px;
		padding: 3px;
		padding-block-end: 4px;

		.value {
			${styles.effects.text.body};
			${styles.mixins.flexCenter()};
			height: 100%;
			padding-inline: 5px;
			border-radius: ${BUTTON_BORDER_RADIUS}px;

			&:focus {
				color: ${c("fill-color-text-on-accent-selected-text")};
				background-color: ${c("accent-color")};
				box-shadow: none;
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

export default function TimecodeBox({ timecode: [timecode, setTimecode], onFocus, ...htmlAttrs }: FCP<{
	/** The current time code or time span. */
	timecode: StateProperty<string>;
	/** Fired when the component is focused or changed. */
	onFocus?: PartialArgsFunc<BaseEventHandler>;
}, "div">) {
	const timecodeBoxEl = useDomRef<"div">();

	const lastActiveItemLastIndex = useRef<number>();

	const tokens = useMemo(() => getTimecodeTokens(timecode), [timecode]);

	const focusValue = useDebounceCallback((itemLastIndex?: number) =>
		setTimeout(() => timecodeBoxEl.current?.querySelector<HTMLElement>(
			itemLastIndex !== undefined ? `[data-last-index="${itemLastIndex}"]` : "[data-last-index]",
		)?.focus()), []);

	function handleSpinnerClick(itemLastIndex: number, step: number) {
		onFocus?.();
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
		onFocus?.(e);
		if (e.target instanceof HTMLElement && e.target.dataset.lastIndex !== undefined) {
			const lastIndex = +e.target.dataset.lastIndex;
			if (Number.isFinite(lastIndex)) lastActiveItemLastIndex.current = lastIndex;
		} else if (!isInPath(e.target, ".value", "button"))
			focusValue(lastActiveItemLastIndex.current);
	}, []);

	const handleValueWheel = useCallback<WheelEventHandler<HTMLDivElement>>(e => {
		e.preventDefault();
		const valueEl = e.currentTarget;
		const delta = -Math.sign(e.deltaY);
		handleSpinnerClick(+valueEl.dataset.lastIndex!, delta);
	}, []);

	const getValueElementFromLastIndex = (lastIndex: number) => timecodeBoxEl.current?.querySelector(`[data-last-index="${lastIndex}"]`) ?? null;

	function moveFocus(valueElement: Element | number | null, direction: 1 | -1) {
		if (typeof valueElement === "number") valueElement = getValueElementFromLastIndex(valueElement);
		valueElement = valueElement?.parentElement ?? null;
		while (valueElement) {
			valueElement = direction === -1 ?
				valueElement.previousElementSibling :
				valueElement.nextElementSibling;
			if (valueElement?.classList.contains("value-wrapper")) {
				(valueElement.firstElementChild as HTMLElement).focus();
				return;
			}
		}
	}

	const handleValueKeyDown = useCallback<KeyboardEventHandler<HTMLDivElement>>(e => {
		const valueEl = e.currentTarget;

		if (e.code.in("ArrowUp", "ArrowDown")) {
			stopEvent(e);
			handleSpinnerClick(+valueEl.dataset.lastIndex!, e.code === "ArrowUp" ? 1 : -1);
		} else if (e.code.in("ArrowLeft", "ArrowRight", "Semicolon", "Period", "Equal", "NumpadAdd", "Space", "Enter", "NumpadEnter", "NumpadDecimal")) {
			stopEvent(e);
			moveFocus(valueEl, e.code === "ArrowLeft" ? -1 : 1);
		} else if (e.code.in("Minus", "NumpadSubtract")) {
			onFocus?.(e);
			(setTimecode as SetStateNarrow<string>)?.(timecode => getSupposedTimecode(timecode, "-"));
		}
	}, []);

	const handleItemChange = useCallback<TimecodeItemValueChangeEventHandler>((value, lastIndex) => {
		onFocus?.();
		(setTimecode as SetStateNarrow<string>)?.(timecode => {
			const tokens = getTimecodeTokens(timecode);
			const selectedItem = tokens.at(lastIndex);
			selectedItem && (selectedItem.value = value);
			const supposedTimecode = getSupposedTimecode(tokens.join(""));
			return supposedTimecode;
		});
	}, [timecode]);

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
						<TimecodeItemValue
							lastIndex={lastIndex}
							onWheel={handleValueWheel}
							onKeyDown={handleValueKeyDown}
							onChange={handleItemChange}
							onFinishInput={(...e) => { moveFocus(lastIndex, 1); handleItemChange(...e); }}
							onRequestFocusLeft={(...e) => { moveFocus(lastIndex, -1); handleItemChange(...e); }}
						>
							{value}
						</TimecodeItemValue>
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

type TimecodeItemValueChangeEventHandler = (value: string, lastIndex: number) => void;

function TimecodeItemValue({ lastIndex, children, onChange, onFinishInput, onRequestFocusLeft, onKeyDown, onBlur, onWheel = noop, ...htmlAttrs }: FCP<{
	/** The index of the value item to the last. */
	lastIndex: number;
	/** The value of the item. */
	children: string;
	/** Fired when the item blurred and the value has changed. */
	onChange?: TimecodeItemValueChangeEventHandler;
	/** Fired when user finishes editing the value of this item. */
	onFinishInput?: TimecodeItemValueChangeEventHandler;
	/** Fired when user press BackSpace key and want to move focus to the left. */
	onRequestFocusLeft?: TimecodeItemValueChangeEventHandler;
}, "div">) {
	const [userInput, setUserInput] = useState<string>();
	const displayUserInput = useMemo(() => userInput !== undefined ? userInput.padStart(children.length, "\u2007") : children, [userInput, children]);

	const handleKeyDown = useCallback<KeyboardEventHandler<HTMLDivElement>>(e => {
		onKeyDown?.(e);
		if (e.code === "Backspace") {
			setUserInput(userInput => {
				if (userInput === "") onRequestFocusLeft?.("0", lastIndex);
				else if (userInput) return userInput.slice(0, -1);
				else if (children) return children.slice(0, -1);
			});
			return;
		}
		const number = e.code.match(/^(Digit|Numpad)(?<number>\d)$/i)?.groups?.number;
		if (number !== undefined)
			new Promise<string>(resolve =>
				setUserInput(userInput => {
					userInput ??= "";
					userInput += number;
					if (userInput.length === children.length) {
						resolve(userInput); // To solve the problem of triggering events twice due to strict mode in the dev.
						userInput = undefined;
					}
					return userInput;
				}),
			).then(userInput => onFinishInput?.(userInput, lastIndex));
	}, [onKeyDown, onFinishInput, userInput, children, lastIndex]);

	const handleBlur = useCallback<FocusEventHandler<HTMLDivElement>>(e => {
		onBlur?.(e);
		if (userInput !== undefined) {
			onChange?.(userInput, lastIndex);
			setUserInput(undefined);
		}
	}, [onBlur, userInput, lastIndex]);

	// Cannot directly use `onWheel` in the JSX in React, or will raise an error "Unable to preventDefault inside passive event listener invocation."
	// So we have to pass a *ref* with `passive: false`.
	// See: https://stackoverflow.com/a/76406673/19553213

	return (
		<div className="value-wrapper">
			<EventInjector onWheel={onWheel}>
				<div
					className="value"
					data-last-index={lastIndex}
					tabIndex={0}
					onBlur={handleBlur}
					onKeyDown={handleKeyDown}
					{...htmlAttrs}
				>
					{displayUserInput}
				</div>
			</EventInjector>
		</div>
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
		if (increment === "-")
			seconds = -seconds;
		else
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
