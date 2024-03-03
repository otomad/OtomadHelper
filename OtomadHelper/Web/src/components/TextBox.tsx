const StyledSpinner = styled.div`
	@layer props {
		--shown: false;
	}

	${styles.mixins.flexCenter()};
	position: absolute;
	top: 50%;
	right: 16px;
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

function Spinner({ disabled, onSpin, onRelease }: FCP<{
	/** 禁用？ */
	disabled?: boolean;
	/** 当点击旋钮时的事件，点击向上时为 1，向下时为 -1。 */
	onSpin?: (spin: SpinValue) => void;
	/** 鼠标释放按钮事件。 */
	onRelease?: BaseEventHandler;
}>) {
	const repeatTimeout = useRef<Timeout>();
	const clearRepeatInterval = () => clearInterval(repeatTimeout.current);

	const handleRelease = useCallback<MouseEventHandler & KeyboardEventHandler>(e => {
		clearRepeatInterval();
		onRelease?.(e);
	}, [onRelease]);

	const handlePress = useCallback((spin: SpinValue) => {
		onSpin?.(spin);
		clearRepeatInterval();
		const startTime = Date.now();
		repeatTimeout.current = setInterval(() => {
			if (Date.now() - startTime > 350)
				onSpin?.(spin);
		}, 50);
	}, [onSpin]);

	return (
		<StyledSpinner>
			<div className="base">
				<Button
					subtle
					icon="chevron_up"
					disabled={disabled}
					onMouseDown={() => handlePress(1)}
					onMouseUp={handleRelease}
					onKeyDown={wrapKeyCode("Space", () => handlePress(1))}
					onKeyUp={wrapKeyCode("Space", handleRelease)}
				/>
				<Button
					subtle
					icon="chevron_down"
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
		margin-right: -4px;
		padding-left: 12px;
	}

	.suffix {
		margin-left: -4px;
		padding-right: 12px;
	}

	.spinner-icon {
		position: relative;
		margin-left: -6px;
		padding-right: 6px;
	}

	.prefix,
	.suffix,
	.spinner-icon {
		${styles.mixins.hideIfEmpty()};
		${styles.mixins.gridCenter()};
		margin-bottom: 1px;
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
			margin-left: 10px;
		}
	}

	.stripes {
		position: absolute;
		inset: 0;
		top: 0;
		overflow: hidden;
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

interface IProps<V extends string | number> {
	/** 输入框的值。 */
	value: StateProperty<V>;
	/** 内容占位符。 */
	placeholder?: string;
	/** 前缀。 */
	prefix?: string;
	/** 后缀。 */
	suffix?: string;
	/** 输入框类型。 */
	type?: "text" | "number";
}

export default function TextBox(props: FCP<IProps<string> & { type?: "text" }, "div">): ReactElement;
export default function TextBox(props: FCP<IProps<number> & { type?: "number" }, "div">): ReactElement;
export default function TextBox({ value: [value, _setValue], placeholder, disabled, prefix, suffix, type = "text", ...htmlAttrs }: FCP<IProps<string> & { type?: "text" } | IProps<number> & { type?: "number" }, "div">) {
	const inputId = useId();
	const inputEl = useDomRef<HTMLInputElement>();

	const setValue = (value: string | number | ((value: string | number) => string | number | undefined) | undefined) =>
		value === undefined || typeof value === "number" && !Number.isFinite(value) || _setValue?.(value as never);

	const handleInput = useCallback<FormEventHandler<HTMLInputElement>>(e => {
		const newValueString = (e.currentTarget ?? e.target).value;
		setValue(typeof value === "number" ?
			newValueString === "" ? undefined : +newValueString : // TODO: 放弃 number 类型，改用字符串处理。
			newValueString.toString());
	}, [value, setValue]);

	const handlePressSpin = useCallback((spin: SpinValue) => {
		setValue(value => typeof value === "number" ? value + spin : undefined);
	}, [value]);

	const handleReleaseSpin = useCallback<BaseEventHandler<HTMLButtonElement>>(e => {
		if (!e.currentTarget.matches(":focus-visible")) // 如果是键盘空格键按下旋钮，则不要自动聚焦到输入框。
			inputEl.current?.focus(); // 如果是鼠标按下旋钮，则会自动聚焦到输入框。
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
				/>
				<label className="suffix" htmlFor={inputId}>{suffix}</label>
				{type === "number" && (
					<>
						<label className="spinner-icon" htmlFor={inputId}>
							<Icon name="scroll_up_down" />
						</label>
						<Spinner onSpin={handlePressSpin} onRelease={handleReleaseSpin} disabled={disabled} />
					</>
				)}
			</div>
			<div className="stripes">
				<div className="large-stripe" />
				<div className="focus-stripe" />
			</div>
		</StyledTextBox>
	);
}
