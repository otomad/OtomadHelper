import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";
import { useInContextLocalization } from "helpers/jipt-activator";

const THUMB_SIZE = 18;
const THUMB_PRESSED_WIDTH = 22;

const isHoverPseudo = "&:hover, .settings-card-toggle-switch.toggle-switch-hoverable:hover .trailing:not(:has(:hover)) &";
const isPressedPseudo = "&:active, &.pressed, .settings-card-toggle-switch.toggle-switch-hoverable:active .trailing:not(:has(:active)) &";

const TOGGLE_SWITCH_LABEL_GAP = 12;

const StyledToggleSwitchLabel = styled.button(() => css`
	display: flex;
	gap: ${TOGGLE_SWITCH_LABEL_GAP}px;
	justify-content: space-between;
	align-items: center;
	overflow-inline: clip;
	text-align: start;

	:where(&) {
		width: 100%;
	}

	.right {
		${styles.text.body};
		display: flex;
		gap: ${TOGGLE_SWITCH_LABEL_GAP}px; // When use \`inherit\` will produce new issues.
		align-items: center;
		margin-inline-start: auto;

		.text.label {
			${styles.text.body};
			block-size: 1lh;
			inline-size: unset !important;
			overflow: clip;
			text-align: end;

			.label-container {
				--progress: 0;
				translate: 0 calc(var(--progress) * -1lh);
			}

			span {
				display: block;
				overflow-block: clip;
				white-space: nowrap;
			}

			div.on {
				block-size: 1lh;
				overflow-block: clip;

				.label-container${important(2)} {
					&.actually-off {
						--progress: 0;
					}

					&:not(.actually-off) {
						--progress: 1;
					}
				}
			}
		}
	}

	${styledExpanderItemText};

	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};

		&:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:last-child {
			border-radius: inherit;
		}
	}

	&:where(:not(.expander-child-items &)) {
		margin-inline-end: -5px;
		padding-inline-end: 5px;
	}

	&:has(> .icon) {
		padding-inline-start: ${expanderItemWithIconPaddingInlineStart}px;
	}

	.flyout & {
		inline-size: stretch;
		margin-inline: 4px;
		padding-block: 8px;
		padding-inline: 13px 12px;
		border-radius: 4px;

		&:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active {
			background-color: ${c("fill-color-subtle-tertiary")};
		}
	}

	.base {
		${styles.mixins.oval()};
		position: relative;
		block-size: 20px;
		inline-size: 40px;
		background-color: ${c("fill-color-control-alt-secondary")};
		background-clip: padding-box;
		border: 1px solid ${c("stroke-color-control-strong-stroke-default")};
	}

	.thumb {
		${styles.mixins.square(`${THUMB_SIZE}px`)};
		${styles.mixins.oval()};
		position: absolute;
		inset-inline-start: 0;
		background-color: if(
			${ifColorScheme.contrast}: ${cc("ButtonText")};
			else: ${c("fill-color-text-secondary")};
		);
		scale: calc(12 / ${THUMB_SIZE});
		touch-action: pinch-zoom;

		&::after { // Enlarge the drag area.
			${styles.mixins.square("100%")};
			content: "";
			display: block;
			background-color: transparent;
			scale: 10 3;
		}
	}

	${isHoverPseudo} {
		.base {
			background-color: ${c("fill-color-control-alt-tertiary")};

			.thumb {
				scale: calc(14 / ${THUMB_SIZE});
			}
		}
	}

	${isPressedPseudo} {
		opacity: 0.8 !important;

		.base {
			background-color: ${c("fill-color-control-alt-quarternary")};

			.thumb {
				width: ${THUMB_PRESSED_WIDTH}px;
				scale: calc(14 / ${THUMB_SIZE});
			}
		}
	}

	&[disabled] {
		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
			border-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.thumb {
			background-color: ${c("fill-color-text-disabled")};
		}


		> .icon,
		.text {
			color: ${c("fill-color-text-disabled")};
		}

		.text .details:not(.select-info) {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	${styles.mixins.forwardFocusRing(".toggle-switch-base")};

	&[aria-checked="true"] {
		.base {
			background-color: ${c("accent-color")} !important;
			border-color: ${c("accent-color")};
		}

		.thumb {
			inset-inline-start: calc(100% - ${THUMB_SIZE}px);
			background-color: ${c("fill-color-text-on-accent-primary")};
			outline: if(
				${ifColorScheme.contrast}: none;
				else: 1px solid ${c("stroke-color-control-stroke-secondary")};
			);
		}

		.label-container${important()} {
			--progress: 1;
		}

		${isHoverPseudo} {
			opacity: 0.9;
		}

		${isPressedPseudo} {
			.thumb {
				inset-inline-start: calc(100% - ${THUMB_PRESSED_WIDTH}px);
			}
		}

		&[disabled] {
			.base {
				background-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
				border-color: ${c("stroke-color-control-strong-stroke-disabled")};
			}

			.thumb {
				background-color: ${c("fill-color-text-on-accent-disabled")};
			}
		}

		&.colored {
			--highlight-color: contrast-color(${c("accent-color")});

			.thumb {
				background-color: ${c("highlight-color")};
			}

			.stroke {
				border-color: ${c("highlight-color")};
			}
		}

		&.actually-off .base {
			background-color: ${c("fill-color-system-caution")} !important;
			border-color: ${c("fill-color-system-caution")};
		}
	}

	> .text > * {
		white-space: pre-line;
	}
`);

export default function ToggleSwitch({ on: _on, disabled: _disabled = false, hideLabel, as, details, resetTransitionOnChanging = false, color, lock, icon, selectInfo, selectValid = false, anchor, actions, actuallyOn, noIndentation, ariaIdRef, className, children, onChange, ...htmlAttrs }: FCP<{
	/** Is on? */
	on: VariousState<boolean>;
	/** Disabled */
	disabled?: VariousStateWithSelf<boolean>;
	/** Hide "on/off" text label? */
	hideLabel?: boolean;
	/** Change the tag name. */
	as?: WebTarget;
	/** Detailed description. */
	details?: ReactNode;
	/** Use special accent color for the toggle switch. */
	color?: string;
	/**
	 * Reset the page's transition effect when toggling the switch.
	 * @remarks This is business logic, but present in the base component.
	 */
	resetTransitionOnChanging?: boolean;
	/**
	 * Sets the displayed value of the toggle switch and disables it.
	 *
	 * This only changes its appearance, not its internal data.
	 *
	 * Useful when you need to disable user input without affecting configuration saving.
	 */
	lock?: boolean | null;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Specifies the display string of the selection of tracks or track events. */
	selectInfo?: ReactNode;
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. */
	selectValid?: boolean | number;
	/**
	 * @deprecated
	 * - For Expander Item title, please use children slot instead;
	 * - For tooltip title, please use Tooltip HoC instead.
	 */
	title?: never;
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/**
	 * If when the toggle switch is on but useless, please pass this prop with `false`, and the label will show "On (Actually Off)".
	 *
	 * If you do not need this feature, please pass `undefined` for better performance.
	 *
	 * Note that this prop either toggles between `true` and `false` or is always `undefined`, never toggles between `true` and `undefined`.
	 *
	 * @default undefined
	 */
	actuallyOn?: boolean;
	/**
	 * Should not it auto-add indentation at the start while it is in a sub-expander?
	 * - `true`: It should NOT auto-add indentation.
	 * - `false`: It should AUTO-ADD indentation forcibly.
	 * - `undefined`: Keep default behavior, inherit the setting in the same name prop of the sub-expander.
	 * @default undefined
	 */
	noIndentation?: boolean;
	/** Pass aria ID to the parent component. */
	ariaIdRef?: AriaIdRef;
	/** Occurs while toggling. */
	onChange?(on: boolean): void;
}, "button">) {
	const [__on, setOn] = useVariousState(_on);
	const on = typeof lock === "boolean" ? lock : __on;
	const __disabled = useReadonlyVariousState(_disabled);
	const disabled = typeof lock === "boolean" || __disabled;
	const [isDragging, setIsDragging] = useState(false);
	const [thumbLeft, setThumbLeft] = useState<number>();
	const [labelTranslate, setLabelTranslate] = useState<number>();
	const ariaId = useId();
	useImperativeHandleAriaId(ariaIdRef, ariaId);
	// CAUTION: Parameter changes using styled-components directly will affect performance.
	const thumbStyle = useMemo(() => thumbLeft === undefined ? undefined : {
		insetInlineStart: thumbLeft + "px",
		transition: "none",
	} as CSSProperties, [thumbLeft]);
	const isContrast = useSnapshot(colorModeStore).contrast;
	if (isContrast) color = undefined;
	const [inContextLocalization] = useInContextLocalization();
	const reduceMotion = useMediaQuery.reduceMotion();
	const actuallyOff = actuallyOn === false;

	const { resetTransition } = useSnapshot(pageStore);
	useUpdateEffect(() => {
		if (resetTransitionOnChanging)
			resetTransition();
	}, [resetTransitionOnChanging, resetTransition, on, disabled]);

	const handleCheck = useCallback((on: boolean, e?: MouseEvent) => {
		stopEvent(e);
		if (!isDragging) {
			setOn?.(on);
			onChange?.(on);
		}
		setIsDragging(false);
	}, [isDragging, setOn, onChange]);

	const onThumbDown = useCallback<PointerEventHandler<HTMLDivElement>>(e => {
		stopEvent(e);
		const thumb = e.currentTarget;
		const control = thumb.parentElement!;
		const controlRect = control.getBoundingClientRect();
		const left = controlRect.left, max = controlRect.width - THUMB_PRESSED_WIDTH - 2;
		const x = e.pageX - left - thumb.offsetLeft;
		const { clientX } = e;
		const aborter = new AbortController();
		let isMoved = false, prevE: PointerEvent | undefined;
		const pointerMove = (e: PointerEvent) => {
			if (Math.abs(e.clientX - clientX) > 2) isMoved = true; // anti-shake
			let value = clamp(e.pageX - left - x, 0, max);
			if (isRtl()) value = max - value;
			setLabelTranslate(value / max);
			setThumbLeft(value);
			prevE = e;
		};
		const pointerUp = (e: PointerEvent) => {
			aborter.abort();
			thumb.releasePointerCapture(e.pointerId);
			if (!(e instanceof MouseEvent) && prevE) e = prevE;
			let isOn = e.pageX - x > left + max / 2;
			if (isRtl()) isOn = !isOn;
			handleCheck(isOn);
			setThumbLeft(undefined);
			setLabelTranslate(undefined);
			setIsDragging(isMoved); // Define recognition as drag instead of click.
		};
		thumb.setPointerCapture(e.pointerId);
		thumb.addEventListener("pointermove", pointerMove, { signal: aborter.signal });
		thumb.addEventListener("pointerup", pointerUp, { signal: aborter.signal });
	}, [handleCheck, reduceMotion]);

	return (
		<StyledToggleSwitchLabel
			as={as as "button"}
			className={[className, parseNoIndentationProp(noIndentation), { /* selected: on, */ colored: !!color, actuallyOff }]}
			disabled={disabled}
			aria-disabled={disabled || undefined}
			onClick={e => handleCheck(!on, e)}
			tabIndex={0}
			style={{ "--accent-color": color }}
			role="switch"
			aria-checked={on}
			aria-labelledby={`${ariaId}-title`}
			aria-describedby={`${ariaId}-details`}
			data-anchor={anchor}
			{...htmlAttrs}
		>
			{icon && <Icon name={icon} />}
			{(children || details) && (
				<div className="text" aria-hidden>
					{children && <p className="title" id={`${ariaId}-title`}>{children}</p>}
					{details && <p className="details" id={`${ariaId}-details`}>{details}</p>}
					<SettingsCard.SelectInfo valid={selectValid}>{selectInfo}</SettingsCard.SelectInfo>
				</div>
			)}
			<div className="right">
				{actions}
				{!hideLabel && (
					<output className="text label" aria-hidden>
						{!inContextLocalization && !reduceMotion ? (
							<div className="label-container" style={{ "--progress": labelTranslate }}>
								<span className="off">{t.off}</span>
								{actuallyOn === undefined ? <span className="on">{t.on}</span> : (
									<div className="on">
										<div className={["label-container", { actuallyOff }]}>
											<span className="off">{t.onActuallyOff}</span>
											<span className="on">{t.on}</span>
										</div>
									</div>
								)}
							</div>
						) :
							on ? actuallyOff ? t.onActuallyOff : t.on : t.off}
					</output>
				)}
				<div className={["base", "toggle-switch-base"]}>
					<div className="thumb" style={thumbStyle} onPointerDown={onThumbDown} />
				</div>
			</div>
		</StyledToggleSwitchLabel>
	);
}
