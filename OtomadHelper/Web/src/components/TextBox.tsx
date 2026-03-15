// #region Spinner
const StyledSpinner = styled.div`
	position: fixed;
	position-area: center;
	z-index: 10;
	transition-behavior: allow-discrete;

	&,
	.base {
		inline-size: fit-content;
	}

	.base {
		padding: 4px;
		background-color: ${c("background-fill-color-acrylic-background-default")};
		border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
		border-radius: 8px;
		box-shadow: if(
			${ifColorScheme.contrast} or ${ifColorScheme.reduceTransparency}: none;
			else: 0 8px 16px ${c("shadows-flyout")};
		);
		backdrop-filter: blur(60px);
		cursor: default;

		button {
			${styles.mixins.square("40px")};
			display: flex;
			min-inline-size: unset;

			&:first-child:active .icon {
				translate: 0 -2px;
			}

			&:last-child:active .icon {
				translate: 0 2px;
			}
		}
	}

	${hiddenAndStartingStyle(css`
		.base {
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
	`)}
`;

type SpinValue = 1 | -1;

function Spinner({ disabled, step = 1, positionAnchor, shown = false, onSpin, onRelease }: FCP<{
	/** Disabled? */
	disabled?: boolean;
	/** The value to increase or decrease each time the knob of numeric up down box is clicked. @default 1 */
	step?: NumberLike;
	/** Provide the spinner icon anchor name. */
	positionAnchor?: string;
	/** Show the spinner? */
	shown?: boolean;
	/** Knob click event. It is 1 when the knob is clicked up and -1 when it is clicked down. */
	onSpin?(spinValue: NumberLike): void;
	/** Mouse release button event. */
	onRelease?: BaseEventHandler;
}>) {
	const hidden = disabled || !shown;

	function spinWithValue(spinValue: SpinValue) {
		const spin = typeof step === "bigint" ? BigInt(spinValue) * step : spinValue * step;
		onSpin?.(spin);
	}

	return (
		<StyledSpinner hidden={hidden} aria-hidden={hidden} style={{ positionAnchor }} onMouseDown={mod.prevent()}>
			<div className="base">
				<Button
					subtle
					icon="spinner/chevron_up"
					disabled={disabled}
					repeat
					tabIndex={-1}
					role="spinbutton"
					aria-label={t.increase}
					onClick={() => spinWithValue(1)}
					onRelease={onRelease}
				/>
				<Button
					subtle
					icon="spinner/chevron_down"
					disabled={disabled}
					repeat
					tabIndex={-1}
					role="spinbutton"
					aria-label={t.decrease}
					onClick={() => spinWithValue(-1)}
					onRelease={onRelease}
				/>
			</div>
		</StyledSpinner>
	);
}
// #endregion

const StyledTextBoxActionButton = styled.button.attrs({
	type: "button",
})`
	flex-shrink: 0;
	align-content: center;
	padding: 0 7px;
	overflow: hidden;
	background-clip: padding-box;
	border: 4px solid transparent;
	border-radius: 7px;
	cursor: default;

	.icon {
		display: flex;
		color: ${c("fill-color-text-secondary")};
		font-size: 16px;
	}

	&:hover {
		background-color: ${c("fill-color-subtle-secondary")};
	}

	&:active {
		background-color: ${c("fill-color-subtle-tertiary")};

		.icon {
			color: ${c("fill-color-text-tertiary")};
		}
	}

	${ifColorScheme.at.contrast} {
		&:is(:hover, :active) {
			background-color: ${cc("Highlight")};

			.icon {
				color: ${cc("HighlightText")};
			}
		}
	}
`;

function TextBoxActionButton({ icon, tooltip, nonFocusable = false, ...htmlAttrs }: FCP<{
	/** Button icon. */
	icon?: DeclaredIcons;
	/** Tooltip. */
	tooltip?: Readable;
	/** Apply tabIndex = -1? */
	nonFocusable?: boolean;
	children?: never;
}, "button">) {
	return (
		<Tooltip title={tooltip} placement="block">
			<StyledTextBoxActionButton tabIndex={nonFocusable ? -1 : undefined} {...htmlAttrs}>
				{icon && <Icon name={icon} />}
			</StyledTextBoxActionButton>
		</Tooltip>
	);
}

export /* @internal */ const inSettingsCardTrailing = ":where(.settings-card > .base, .expander-item, .checkbox-label) > :is(.trailing, .actions)";

export /* @internal */ const inputInSettingsCardStyle = css`
	${inSettingsCardTrailing} > .stack-panel > :where(&),
	${inSettingsCardTrailing} > :where(&) {
		inline-size: 200px;
		max-inline-size: calc(50cqw - 13px);
	}
`;

export /* @internal */ const StyledTextBox = styled.div<{
	/** The default text box width is 200px, you can change it to 100% when you set it to true. */
	$fullWidth?: boolean;
	/** Anchor name of the whole text box itself. */
	$anchorName?: string;
}>`
	position: relative;
	background-color: ${c("fill-color-control-default")};
	border-radius: 4px;
	box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-default")} inset;
	cursor: text;
	forced-color-adjust: none;

	${({ $anchorName }) => $anchorName && css`
		--text-box-anchor-name: ${$anchorName};
		anchor-name: ${$anchorName};
	`}

	&,
	* {
		font-feature-settings: "case" on, "halt" on;
	}

	&[data-type="number"] input {
		font-variant-numeric: tabular-nums;
	}

	.wrapper {
		display: flex;
		align-items: stretch;
	}

	${inputInSettingsCardStyle}

	${ifNotProp("$fullWidth", css`
		@layer base {
			inline-size: 200px;
		}
	`)}

	input {
		${styles.effects.text.body};
		z-index: 1;
		inline-size: 100%;
		padding: 6px 12px 7px;
		color: ${c("foreground-color")};
		caret-color: currentColor;
		speak-as: literal-punctuation;
		transition: ${fallbackTransitions}, padding-inline 0s;

		&:focus {
			box-shadow: none;
		}

		&::placeholder {
			color: ${c("fill-color-text-secondary")};
		}

		&:disabled::selection { // Have fixed the issue where text can still be selected even though the input box is disabled.
			color: inherit;
			background-color: transparent;
		}
	}

	.prefix,
	.leading-icon {
		margin-inline-end: -4px;
		padding-inline-start: 12px;
	}

	.leading-icon {
		color: ${c("fill-color-text-secondary")};
	}

	.suffix {
		margin-inline-start: -4px;
		padding-inline-end: 12px;
	}

	.positive-sign {
		margin-inline-start: 12px;

		+ input {
			padding-inline-start: 0;
		}
	}

	&:hover {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active,
	&:focus-within,
	.timecode-box:focus-within & {
		background-color: ${c("fill-color-control-input-active")};

		.stripes .focus-stripe {
			scale: 1;
		}
	}

	.prefix,
	.suffix,
	.positive-sign,
	.warn-icon,
	.spinner-icon,
	.leading-icon {
		${styles.mixins.hideIfEmpty()};
		${styles.mixins.gridCenter()};
		margin-block-end: 1px;
		white-space: nowrap;
	}

	.spinner-icon {
		position: relative;
		margin-inline: -6px 6px;
	}

	.warn-icon {
		position: relative;
		display: none;
		block-size: auto;
		inline-size: 0;
		margin-inline-end: 0;
		color: ${c("fill-color-system-critical")};
		scale: 0;
		transition-behavior: allow-discrete;

		// Expand the trigger area of the tooltip.
		&::after {
			content: "";
			position: absolute;
			inset: 0;
			inset-inline: -14px;
		}
	}

	&:has(input:invalid) {
		.stripes .focus-stripe {
			background-color: ${c("fill-color-system-critical")};
			scale: 1;
		}

		.suffix {
			padding-inline-end: 7px;
		}

		.warn-icon {
			display: flex;
			inline-size: 1em;
			margin-inline-end: 14px;
			scale: 1;

			@starting-style {
				inline-size: 0;
				margin-inline-end: 0;
				scale: 0;
			}
		}
	}

	.action-buttons {
		${StyledTextBoxActionButton} {
			scale: 1;
			transition-behavior: allow-discrete;

			${hiddenAndStartingStyle(css`
				inline-size: 0;
				margin-inline: 0;
				padding-inline: 0;
				border-inline-width: 0;
				scale: 0;
			`)}
		}
	}

	&:is(:has(input:disabled), [disabled] *) {
		color: ${c("fill-color-text-disabled")};
		background-color: ${c("fill-color-control-disabled")};
		cursor: not-allowed;

		.stripes * {
			scale: 0;
		}

		input {
			color: inherit;
		}
	}

	${ifColorScheme.at.contrast} {
		color: ${cc("ButtonText")};
	}

	[aria-readonly="true"] &,
	&:has(input:read-only) {
		background-color: ${c("fill-color-control-disabled")};

		.stripes * {
			scale: 0;
		}

		.spinner-icon {
			color: ${c("fill-color-text-disabled")};
		}
	}

	&:focus-within {
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
			inset-block-end: 0;
			block-size: 1px;
			inline-size: 100%;

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

	.custom-flyout {
		position: absolute !important;
		cursor: default;
	}
`;

type HTMLInputFormEvent = Parameters<FormEventHandler<HTMLInputElement>>[0];

export default function TextBox({ value: [value, _setValue], placeholder, disabled, readOnly, id, prefix, suffix, _spinner: spinner, _showPositiveSign: showPositiveSign, customFlyout, pattern, required, mouseDownTriggerOnChanging = true, fullWidth = false, showClearAll, icon, type = "text", "aria-label": ariaLabel, "aria-labelledby": ariaLabelledBy, "aria-description": ariaDescription, onChange, onChanging, onInput, onKeyDown, onFocusChange, onValidate, ref, inputRef, ...htmlAttrs }: FCP<{
	/** The value of the input box. */
	value: StateProperty<string>;
	/** Content placeholder. */
	placeholder?: string;
	/** Read-only? */
	readOnly?: boolean;
	/** Prefix. */
	prefix?: string;
	/** Suffix. */
	suffix?: string;
	/** @private Numeric up down box */
	_spinner?(inputId: string): ReactNode;
	/** @private Show the positive sign? */
	_showPositiveSign?: boolean;
	// /** Set the attributes for the input element. @deprecated */
	// inputAttrs?: FCP<{}, "input">;
	/** Ref to the input element. */
	inputRef?: MiscRef<HTMLInputElement>;
	/** Add your own flyout inside the text box, must be absolute or fixed position. */
	customFlyout?: ReactNode;
	/** Custom input allowed characters pattern. */
	pattern?: RegExp;
	/** Required to input any text. */
	required?: boolean;
	/** Trigger onChanging event while onMouseDown? @default true */
	mouseDownTriggerOnChanging?: boolean;
	/**
	 * The default text box width is 200px, you can change it to 100% when you set it to true.
	 * However, you can still change the width freely in the style attribute whenever you want.
	 * @default false
	 */
	fullWidth?: boolean;
	/** Show the clear all button? */
	showClearAll?: boolean;
	/** Custom leading icon of the text box. */
	icon?: DeclaredIcons;
	/** Input type. @default "text" */
	type?: React.HTMLInputTypeAttribute;
	/** Text change event. Only occurs after pasting text or after the input box is out of focus. */
	onChange?: BaseEventHandler<HTMLInputElement>;
	/** Text changing event. Occurs any time the text changes. */
	onChanging?: FormEventHandler<HTMLInputElement>;
	/** Text keyboard input event. */
	onInput?(newText: string, el: HTMLInputElement, event: HTMLInputFormEvent): boolean | string | void;
	/** Keyboard press event. */
	onKeyDown?: KeyboardEventHandler<HTMLInputElement>;
	/** Occurs when the input box focused or blurred. */
	onFocusChange?(focused: boolean, e: React.FocusEvent<HTMLInputElement>): void;
	onValidate?(value: string, el: HTMLInputElement): string | undefined;
	/** @deprecated Please use `disabled` instead. */
	"aria-disabled"?: never;
	/** @deprecated Please use `readOnly` instead. */
	"aria-readonly"?: never;
}, "div">) {
	const inputIdDef = "input" + useId();
	const inputId = id || inputIdDef;
	const inputEl = useDomRef<"input">();
	const wrapperEl = useDomRef<"div">();
	useImperativeHandleRef(ref, wrapperEl);
	useImperativeHandleRef(inputRef, inputEl);
	disabled = useContext(InteractionStateContext).disabled || disabled;

	const setValue = (value: string | undefined | ((value: string) => string | undefined)) =>
		value == null || _setValue?.(value as string);

	const clearAll = () => { _setValue?.(""); inputEl.current?.focus(); };

	const handleChange = useCallback((e: Any) => { onChanging?.(e); onChange?.(e); }, [onChange, onChanging]);

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
			setTimeout(() => caret !== null && Caret.set(el, caret));
		}
		setValue(newText); // true or undefined
		onChanging?.(e as never);
	}, [value, setValue, onChanging, onInput]);

	const handleKeyDown = useCallback<KeyboardEventHandler<HTMLInputElement>>(e => {
		onKeyDown?.(e);
		if (e.code === "Enter" || e.code === "NumpadEnter") {
			inputEl.current?.blur();
			handleChange(e);
			stopEvent(e);
		}
	}, [onKeyDown, inputEl, handleChange]);

	useEffect(() => {
		const el = inputEl.current;
		if (!el) return;
		const customValidity = onValidate?.(el.value, el);
		el.setCustomValidity(customValidity ?? "");
	}, [onValidate, value]);

	return (
		<StyledTextBox
			ref={wrapperEl}
			disabled={disabled}
			aria-disabled={disabled || undefined}
			$fullWidth={fullWidth}
			$anchorName={`--${inputId}-text-box`}
			onClick={mod.stop()}
			{...htmlAttrs}
		>
			<div className="wrapper">
				{icon && <label className="leading-icon" htmlFor={inputId} aria-hidden><Icon name={icon} /></label>}
				<label className="prefix" htmlFor={inputId}>{prefix}</label>
				{showPositiveSign && <label className="positive-sign" htmlFor={inputId}>+</label>}
				<input
					ref={inputEl}
					id={inputId}
					type={type}
					value={value}
					placeholder={placeholder}
					disabled={disabled || undefined}
					aria-disabled={disabled || undefined}
					aria-description={ariaDescription || undefined}
					readOnly={readOnly}
					aria-readonly={readOnly}
					aria-label={ariaLabel}
					aria-labelledby={ariaLabelledBy}
					autoComplete="off"
					title=""
					pattern={pattern?.source}
					required={required}
					onInput={handleInput}
					onPaste={handleChange}
					onKeyDown={handleKeyDown}
					onMouseDown={mouseDownTriggerOnChanging ? onChanging : undefined}
					onFocus={e => onFocusChange?.(true, e)}
					onBlur={e => onFocusChange?.(false, e)}
				/>
				<label className="suffix" htmlFor={inputId}>{suffix}</label>
				<Tooltip title={() => inputEl.current?.validationMessage} placement="block">
					<Icon name="error_circle" className="warn-icon" />
				</Tooltip>
				<Contents className="action-buttons">
					{showClearAll && <TextBoxActionButton icon="dismiss" hidden={!value} tooltip={t.clearAll} nonFocusable onClick={clearAll} />}
				</Contents>
				{spinner?.(inputId)}
			</div>
			<div className="stripes">
				<div className="large-stripe" />
				<div className="focus-stripe" />
			</div>
			{customFlyout && <div className="custom-flyout">{customFlyout}</div>}
		</StyledTextBox>
	);
}

type NumberLike = number | bigint;
function NumberTextBox<TNumber extends NumberLike>({ value: [value, _setValue], disabled, readOnly, decimalPlaces, keepTrailing0, min, max, spinnerStep, keyBigStepMultiplier, positiveSign, required = true, inputRef, ...textBoxProps }: Override<OmitConventionalPrivates<PropsOf<typeof TextBox>>, {
	/** The value of the number, which can be number or bigint type. */
	value: readonly [get: TNumber, set?: SetStateNarrow<TNumber>];
	/** The number of decimal places, leaving blank means no limit. */
	decimalPlaces?: number;
	/** Keep trailing zeros in the fractional part? */
	keepTrailing0?: boolean;
	/** Limit of the minimum value. */
	min?: TNumber;
	/** Limit of the maximum value. */
	max?: TNumber;
	/** The value to increase or decrease each time the knob of numeric up down box is clicked. @default 1 */
	spinnerStep?: TNumber;
	/**
	 * According to the Accessibility feature, when user press PageUp and PageDown key, it will adjust a larger number than the `keyStep`.
	 * Please specify a number which will multiply by the `keyStep`. @default 10
	 */
	keyBigStepMultiplier?: TNumber;
	/** Show the positive sign if the value is positive? */
	positiveSign?: boolean;
}>) {
	const inputEl = useDomRef<"input">();
	useImperativeHandleRef(inputRef, inputEl);
	const bigIntMode = typeof value === "bigint";
	keyBigStepMultiplier ??= (bigIntMode ? 10n : 10) as TNumber;
	const intMode = bigIntMode || decimalPlaces === 0;
	const [displayValue, setDisplayValue] = useState<string>();
	const [focused, setFocused] = useState(false);

	const setValue = (value: TNumber | undefined | ((value: TNumber) => TNumber | undefined)) => _setValue?.(prevValue => {
		if (typeof value === "function") value = value(prevValue);
		if (value == null || typeof value === "number" && !Number.isFinite(value) && required) return prevValue;
		return clamp(value, min!, max!);
	});

	const normalizeValue = useCallback((value?: NumberLike) => {
		if (isUndefinedNullNaN(value)) return "";
		value = clamp(value, min!, max!);
		let result = normalizeNumber(value);
		if (decimalPlaces !== undefined && typeof value === "number") {
			if (typeof decimalPlaces !== "number" || decimalPlaces < 0 || decimalPlaces > 100)
				throw new RangeError(`Decimal places argument must be between 0 and 100, got ${decimalPlaces}`);
			result = value.toFixed(decimalPlaces);
		}
		if (!keepTrailing0 && result.includes("."))
			result = result.replace(/\.?0+$/, "");
		return result;
	}, [decimalPlaces, keepTrailing0, max, min]);

	const parseText = useCallback((text: string) => {
		if (!text && !required)
			return NaN as TNumber;
		else if (intMode) {
			text = text.match(/-?\d+/)?.[0] ?? "";
			return (bigIntMode ? BigInt(text) : Number(text)) as TNumber;
		} else {
			text = text.replaceAll(/\.+/g, ".").match(/-?\d*\.\d+/)?.[0] ?? text.match(/-?\d+/)?.[0] ?? "";
			return Number(text) as TNumber;
		}
	}, [bigIntMode, intMode]);

	function handleInput(text: string) {
		if (text === "")
			return required ? undefined : "";
		else if (text.match(/[^\d.-]/) || text.indexOf("-", 1) >= 0 || text.count(".") >= 2)
			return false;
		const value = text.match(/-?\d*\.?\d*/)?.[0].replace(/(?<=^-?)0+(?=\d)/, "");
		if (!value) // undefined or ""
			return false;
		else if (intMode && text.includes("."))
			return false;
		else
			return value;
	}

	const setValueFromTextBox = (text: string) => {
		setDisplayValue(text);
		setValue(parseText(text));
	};

	const updateDisplayValue = useCallback((value?: NumberLike) => setDisplayValue(normalizeValue(value)), [normalizeValue]);

	const handleBlurChange: BaseEventHandler<HTMLInputElement> = e => {
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
	};

	useEffect(() => {
		const newValue = value, oldValue = displayValue && parseText(displayValue);
		if (newValue !== oldValue) updateDisplayValue(value);
	}, [value, decimalPlaces, displayValue, updateDisplayValue, parseText]);

	const setCaretToPoint = () => {
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
	};

	const handlePressSpin = (spinValue: NumberLike) => {
		setValue(value => {
			if (!(typeof value === "number" || typeof value === "bigint")) return undefined;
			const spin = typeof value === "bigint" ? BigInt(spinValue) : spinValue;
			const newValue = (typeof value === "number" && !Number.isFinite(value) ? 0 :
				(value as number) + (spin as number)) as TNumber;
			updateDisplayValue(newValue);
			return newValue;
		});
	};

	const handleReleaseSpin: BaseEventHandler<HTMLButtonElement> = e => {
		if (!(e.currentTarget instanceof HTMLElement)) return;
		if (!e.currentTarget.matches(":focus-visible")) // If the knob is pressed by the space on keyboard, do not auto focus on the input box.
			inputEl.current?.focus(); // If the knob is pressed by the mouse, it will auto focus on the input box.
		setCaretToPoint();
	};

	const handleKeyDown: KeyboardEventHandler<HTMLInputElement> = e => {
		const baseStep = spinnerStep ?? 1;
		let step: NumberLike | undefined, limit: number | undefined;
		if (e.code === "ArrowUp") step = baseStep;
		else if (e.code === "ArrowDown") step = -baseStep;
		else if (e.code === "PageUp") step = baseStep * keyBigStepMultiplier;
		else if (e.code === "PageDown") step = -baseStep * keyBigStepMultiplier;
		else if (e.code === "Home") limit = -1;
		else if (e.code === "End") limit = 1;
		if (step || limit) {
			if (step && !limit)
				handlePressSpin(step);
			else if (limit)
				if (limit < 0 && min !== undefined) setValue(min);
				else if (limit > 0 && max !== undefined) setValue(max);
				else return;
			textBoxProps.onChanging?.(e);
			setTimeout(() => setCaretToPoint());
			stopEvent(e);
		}
	};

	return (
		<TextBox
			{...textBoxProps}
			disabled={disabled}
			readOnly={readOnly}
			value={[displayValue ?? "", setValueFromTextBox]}
			inputRef={inputEl}
			_showPositiveSign={positiveSign && value > 0}
			data-type="number"
			required={required}
			aria-required={required}
			onChange={handleBlurChange}
			onInput={handleInput}
			onKeyDown={handleKeyDown}
			onFocusChange={focused => setFocused(focused)}
			_spinner={inputId => {
				const anchorName = `--${inputId}-spinner-icon`;
				return (
					<>
						<label className="spinner-icon" htmlFor={inputId} aria-hidden style={{ anchorName }}>
							<Icon name="scroll_up_down" />
						</label>
						{!readOnly && (
							<Portal container="main.page">
								<Spinner
									onSpin={handlePressSpin}
									onRelease={handleReleaseSpin}
									disabled={disabled}
									step={spinnerStep}
									positionAnchor={anchorName}
									shown={focused}
								/>
							</Portal>
						)}
					</>
				);
			}}
		/>
	);
}

const StyledNumberUnitTextBox = styled.div`
	display: flex;
	align-items: center;

	.combo-box {
		inline-size: 10px;
	}
`;

function NumberUnitTextBox<TUnit extends string>({ value: [[curValue, curUnit], set], units, unitNames, disabled, ...numberTextBoxProps }: Override<PropsOf<typeof NumberTextBox<number>>, {
	/** Numeric value and its unit type. */
	value: StatePropertyNonNull<Unit<TUnit>>;
	/** All unit types. */
	units: readonly TUnit[];
	/** Get the plural unit type names from the unit type and current value. */
	unitNames(unit: TUnit, currentValueNumeric: number, currentValueUnit: TUnit, thisUnitIndex: number, allUnits: readonly TUnit[]): Readable;
}>) {
	const setValue = (newValue: React.SetStateAction<number>) => set?.(([, unit]) => [typeof newValue === "function" ? newValue(curValue) : newValue, unit]);
	const setUnit = (newUnit: React.SetStateAction<TUnit>) => set?.(([value]) => [value, typeof newUnit === "function" ? newUnit(curUnit) : newUnit]);
	const mappedUnitNames = !unitNames ? units : units.map((unit, index, units) => unitNames(unit, curValue, curUnit, index, units));

	return (
		<StyledNumberUnitTextBox>
			<NumberTextBox value={[curValue, setValue]} {...numberTextBoxProps} disabled={disabled} />
			<ComboBox current={[curUnit, setUnit]} ids={units} options={mappedUnitNames} disabled={disabled} />
		</StyledNumberUnitTextBox>
	);
}

export const roughTimeUnits = Object.freeze(Object.assign(
	["millisecond", "second", "minute", "hour"] satisfies RoughTimeUnit[],
	{
		filter(this: readonly RoughTimeUnit[], units: readonly RoughTimeUnit[]): readonly RoughTimeUnit[] {
			return Array.prototype.filter.call(this, unit => units.includes(unit)).toReversed();
		},
		names(this: typeof roughTimeUnits, count: number, _units: readonly RoughTimeUnit[]) {
			return (unit: RoughTimeUnit) => t.units[unit]({ count });
		},
	},
));

function RoughTimeTextBox({ units = roughTimeUnits, value, ...numberUnitProps }: Override<PropsOf<typeof NumberUnitTextBox<RoughTimeUnit>>, {
	unitNames?: never;
	/** Filter the available rough time units. @default */
	units?: readonly RoughTimeUnit[];
}>) {
	return <NumberUnitTextBox {...numberUnitProps} value={value} units={roughTimeUnits.filter(units)} unitNames={roughTimeUnits.names(value[0][0], units)} />;
}

TextBox.Number = NumberTextBox;
TextBox.NumberUnit = NumberUnitTextBox;
TextBox.RoughTime = RoughTimeTextBox;
