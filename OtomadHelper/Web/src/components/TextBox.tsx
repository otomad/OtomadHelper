const StyledSpinner = styled.div`
	@layer props {
		--shown: false;
	}

	${styles.mixins.flexCenter()};
	position: absolute;
	inset-block-start: 50%;
	inset-inline-end: 16px;
	z-index: 2;
	contain: size;

	.base {
		padding: 4px;
		background-color: ${c("background-fill-color-acrylic-background-default")};
		border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
		border-radius: 8px;
		box-shadow: 0 8px 16px ${c("shadows-flyout")};
		backdrop-filter: blur(60px);
		cursor: default;

		button {
			${styles.mixins.square("40px")};
			display: flex;
			min-width: unset;

			&:first-child:active .icon {
				translate: 0 -2px;
			}

			&:last-child:active .icon {
				translate: 0 2px;
			}
		}

		@container style(--shown: false) {
			scale: 0.75;
			opacity: 0;
			pointer-events: none;

			button:first-child .icon {
				translate: 0 14px;
			}

			button:last-child .icon {
				translate: 0 -14px;
			}
		}
	}
`;

type SpinValue = 1 | -1;

const wrapKeyCode = (code: string, action: (e: Any) => void) => (e: KeyboardEvent) => e.code === code && action(e);

function Spinner({ disabled, step = 1, onSpin, onRelease }: FCP<{
	/** 禁用？ */
	disabled?: boolean;
	/** 每次点按数字上下调节旋钮时增加或减少的数值，默认为 1。 */
	step?: NumberLike;
	/** 当点击旋钮时的事件，点击向上时为 1，向下时为 -1。 */
	onSpin?: (spinValue: NumberLike) => void;
	/** 鼠标释放按钮事件。 */
	onRelease?: BaseEventHandler;
}>) {
	const repeatTimeout = useRef<Timeout>();
	const clearRepeatInterval = () => clearInterval(repeatTimeout.current);

	const handleRelease = useCallback<MouseEventHandler & KeyboardEventHandler>(e => {
		clearRepeatInterval();
		onRelease?.(e);
	}, [onRelease]);

	const handlePress = useCallback((spinValue: SpinValue) => {
		const spin = typeof step === "bigint" ? BigInt(spinValue) * step : spinValue * step;
		onSpin?.(spin);
		clearRepeatInterval();
		const startTime = Date.now();
		repeatTimeout.current = setInterval(() => {
			if (Date.now() - startTime > 350)
				onSpin?.(spin);
		}, 50);
	}, [onSpin]);

	useEventListener(document, "mouseup", handleRelease as never);

	return (
		<StyledSpinner>
			<div className="base">
				<Button
					subtle
					icon="spinner/chevron_up"
					disabled={disabled}
					onMouseDown={() => handlePress(1)}
					onMouseUp={handleRelease}
					onKeyDown={wrapKeyCode("Space", () => handlePress(1))}
					onKeyUp={wrapKeyCode("Space", handleRelease)}
				/>
				<Button
					subtle
					icon="spinner/chevron_down"
					disabled={disabled}
					onMouseDown={() => handlePress(-1)}
					onMouseUp={handleRelease}
					onKeyDown={wrapKeyCode("Space", () => handlePress(-1))}
					onKeyUp={wrapKeyCode("Space", handleRelease)}
				/>
			</div>
		</StyledSpinner>
	);
}

const StyledTextBox = styled.div`
	position: relative;
	background-color: ${c("fill-color-control-default")};
	border-radius: 4px;
	box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-default")} inset;
	cursor: text;

	.wrapper {
		display: flex;
		align-items: stretch;
	}

	input {
		z-index: 1;
		width: 100%;
		padding: 6px 12px 7px;

		&:focus {
			box-shadow: none;
		}

		&::placeholder {
			color: ${c("fill-color-text-secondary")};
		}

		&::selection {
			color: ${c("fill-color-text-on-accent-selected-text")};
			background-color: ${c("accent-color")};
		}

		&[disabled]::selection { // 修复输入框被禁用却仍然可以被选中文本的问题。
			color: inherit;
			background-color: transparent;
		}
	}

	.prefix {
		margin-inline-end: -4px;
		padding-inline-start: 12px;
	}

	.suffix {
		margin-inline-start: -4px;
		padding-inline-end: 12px;
	}

	.spinner-icon {
		position: relative;
		margin-inline-start: -6px;
		padding-inline-end: 6px;
	}

	.prefix,
	.suffix,
	.spinner-icon {
		${styles.mixins.hideIfEmpty()};
		${styles.mixins.gridCenter()};
		margin-block-end: 1px;
	}

	&:hover {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active,
	&:focus-within {
		background-color: ${c("fill-color-control-input-active")};

		.stripes .focus-stripe {
			scale: 1;
		}
	}

	&:has(input[disabled]) {
		background-color: ${c("fill-color-control-disabled")};
		cursor: not-allowed;

		.wrapper {
			opacity: ${c("disabled-text-opacity")};
		}

		.stripes .focus-stripe {
			scale: 0;
		}
	}

	&:focus-within {
		.spinner {
			--shown: true;
		}

		.spinner-icon {
			margin-inline-start: 10px;
		}
	}

	.stripes {
		position: absolute;
		inset: 0;
		overflow: clip;
		border-radius: inherit;
		pointer-events: none;

		> * {
			position: absolute;
			bottom: 0;
			width: 100%;
			height: 1px;

			&.large-stripe {
				background-color: ${c("fill-color-control-strong-default")};
			}

			&.focus-stripe {
				height: 2px;
				background-color: ${c("accent-color")};
				scale: 0 1;
			}
		}
	}
`;

const TextBox = forwardRef(function TextBox({ value: [value, _setValue], placeholder, disabled, prefix, suffix, _spinner: spinner, onChange, onInput, ...htmlAttrs }: FCP<{
	/** 输入框的值。 */
	value: StateProperty<string>;
	/** 内容占位符。 */
	placeholder?: string;
	/** 前缀。 */
	prefix?: string;
	/** 后缀。 */
	suffix?: string;
	/** @private 数字上下调节旋钮。 */
	_spinner?: (inputId: string) => ReactNode;
	/** 文本改变事件，仅在粘贴文本或输入框失焦后触发。 */
	onChange?: BaseEventHandler<HTMLInputElement>;
	/** 文本键盘输入事件。 */
	onInput?: (newText: string, el: HTMLInputElement, ...event: Parameters<FormEventHandler<HTMLInputElement>>) => boolean | string | void;
}, "div">, ref: ForwardedRef<"input">) {
	const inputId = useId();
	const inputEl = useDomRef<"input">();
	useImperativeHandle(ref, () => inputEl.current!);

	const setValue = (value: string | undefined | ((value: string) => string | undefined)) =>
		value == null || _setValue?.(value as string);

	const handleInput = useCallback<FormEventHandler<HTMLInputElement>>(e => {
		const el = e.currentTarget ?? e.target;
		let newText = el.value;
		const caret = Caret.get(el);
		const handled = onInput?.(newText, el, e);
		if (handled === false) {
			setTimeout(() => caret !== null && Caret.set(el, caret - 1));
			return;
		} else if (typeof handled === "string") {
			newText = handled;
			Caret.set(el, caret);
		}
		setValue(newText); // true or undefined
	}, [value, setValue]);

	const handleKeyDown = useCallback<KeyboardEventHandler<HTMLInputElement>>(e => {
		if (e.key === "Enter") { // Don't use `e.code` or NumpadEnter won't work
			inputEl.current?.blur();
			stopEvent(e);
		}
	}, []);

	return (
		<StyledTextBox {...htmlAttrs}>
			<div className="wrapper">
				<label className="prefix" htmlFor={inputId}>{prefix}</label>
				<input
					ref={inputEl}
					id={inputId}
					type="text"
					value={value}
					placeholder={placeholder}
					disabled={disabled}
					autoComplete="off"
					onInput={handleInput}
					onPaste={onChange}
					onBlur={onChange}
					onKeyDown={handleKeyDown}
				/>
				<label className="suffix" htmlFor={inputId}>{suffix}</label>
				{spinner?.(inputId)}
			</div>
			<div className="stripes">
				<div className="large-stripe" />
				<div className="focus-stripe" />
			</div>
		</StyledTextBox>
	);
});

type NumberLike = number | bigint;
function NumberTextBox<TNumber extends NumberLike>({ value: [value, _setValue], disabled, decimalPlaces, keepTrailing0, min, max, spinnerStep, ...textBoxProps }: Override<PropsOf<typeof TextBox>, {
	/** 数字的值，可以是数字或大整数类型。 */
	value: StateProperty<TNumber>;
	/** 小数点位数，缺省表示不限。 */
	decimalPlaces?: number;
	/** 保留小数部分的尾随零。 */
	keepTrailing0?: boolean;
	/** 限定最小值。 */
	min?: TNumber;
	/** 限定最大值。 */
	max?: TNumber;
	/** 每次点按数字上下调节旋钮时增加或减少的数值，默认为 1。 */
	spinnerStep?: TNumber;
}>) {
	const inputEl = useDomRef<"input">();
	const bigIntMode = typeof value === "bigint";
	const [displayValue, setDisplayValue] = useState("");

	const setValue = (value: TNumber | undefined | ((value: TNumber) => TNumber | undefined)) => (_setValue as SetStateNarrow<TNumber>)?.(prevValue => {
		if (typeof value === "function") value = value(prevValue);
		if (value == null || typeof value === "number" && !Number.isFinite(value)) return prevValue;
		return clamp(value, min, max);
	});

	function normalizeValue(value?: NumberLike) {
		if (isUndefinedNullNaN(value)) return "";
		value = clamp(value, min, max);
		let result = normalizeNumber(value);
		if (decimalPlaces !== undefined && typeof value === "number") {
			if (typeof decimalPlaces !== "number" || decimalPlaces < 0 || decimalPlaces > 100)
				throw new Error(`Decimal places argument must be between 0 and 100, got ${decimalPlaces}`);
			result = value.toFixed(decimalPlaces);
		}
		if (!keepTrailing0 && result.includes("."))
			result = result.replace(/\.?0+$/, "");
		return result;
	}

	function parseText(text: string) {
		if (bigIntMode) {
			text = text.match(/-?\d+/)?.[0] ?? "";
			return BigInt(text) as TNumber;
		} else {
			text = text.replaceAll(/\.+/g, ".").match(/-?\d*\.\d+/)?.[0] ?? text.match(/-?\d+/)?.[0] ?? "";
			return Number(text) as TNumber;
		}
	}

	function handleInput(text: string) {
		if (text === "")
			return;
		else if (text.match(/[^\d.-]/) || text.indexOf("-", 1) >= 0 || text.countChar(".") >= 2)
			return false;
		const value = text.match(/-?\d*\.?\d*/)?.[0].replace(/(?<=^-?)0+(?=\d)/, "");
		if (!value) // undefined or ""
			return false;
		else
			return value;
	}

	const setValueFromTextBox = (text: string) => {
		setDisplayValue(text);
		setValue(parseText(text));
	};

	const updateDisplayValue = (value?: NumberLike) => setDisplayValue(normalizeValue(value));

	const handleBlurChange = useCallback<BaseEventHandler<HTMLInputElement>>(e => {
		const el = e.currentTarget ?? e.target;
		let value = el.value;
		if (e.nativeEvent instanceof ClipboardEvent && e.nativeEvent.clipboardData) {
			e.preventDefault();
			const { clipboardData } = e.nativeEvent;
			if (![...clipboardData.items].find(item => item.kind === "string")) return;
			value = e.nativeEvent.clipboardData.getData("text");
		} else if (e.type === "blur" && e.nativeEvent instanceof FocusEvent && isInPath(e.nativeEvent.relatedTarget, ".spinner"))
			return;
		updateDisplayValue(parseText(value));
	}, []);

	useEffect(() => {
		const newValue = value, oldValue = parseText(displayValue);
		if (newValue !== oldValue) updateDisplayValue(value);
	}, [value, decimalPlaces]);

	const handlePressSpin = useCallback((spinValue: NumberLike) => {
		setValue(value => {
			if (!(typeof value === "number" || typeof value === "bigint")) return undefined;
			const spin = typeof value === "bigint" ? BigInt(spinValue) : spinValue;
			const newValue = ((value as number) + (spin as number)) as TNumber;
			updateDisplayValue(newValue);
			return newValue;
		});
	}, [value]);

	const handleReleaseSpin = useCallback<BaseEventHandler<HTMLButtonElement>>(e => {
		if (!(e.currentTarget instanceof HTMLElement)) return;
		if (!e.currentTarget.matches(":focus-visible")) // 如果是键盘空格键按下旋钮，则不要自动聚焦到输入框。
			inputEl.current?.focus(); // 如果是鼠标按下旋钮，则会自动聚焦到输入框。
		if (inputEl.current) {
			const pointIndex = inputEl.current.value.indexOf(".");
			let offset = 0;
			if (spinnerStep) {
				const step = normalizeNumber(spinnerStep);
				if (step.includes(".")) {
					const part = step.match(/(?<=\.)\d*/)?.[0].replace(/0+$/, "");
					if (part) offset = part.length + 1;
				} else {
					const part = step.match(/0+$/)?.[0];
					if (part) offset = -part.length;
				}
			}
			Caret.set(inputEl, pointIndex + offset);
		}
	}, [spinnerStep]);

	return (
		<TextBox
			{...textBoxProps}
			disabled={disabled}
			value={[displayValue, setValueFromTextBox]}
			ref={inputEl}
			onChange={handleBlurChange}
			onInput={handleInput}
			_spinner={inputId => (
				<>
					<label className="spinner-icon" htmlFor={inputId}>
						<Icon name="scroll_up_down" />
					</label>
					<Spinner onSpin={handlePressSpin} onRelease={handleReleaseSpin} disabled={disabled} step={spinnerStep} />
				</>
			)}
		/>
	);
}

export default functionModule(TextBox, {
	Number: NumberTextBox,
});
