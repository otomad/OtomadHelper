import { StyledCard } from "components/Card";
import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";
import SettingsCardSelectInfo from "./SettingsCardSelectInfo";

const isPressed = (ampersand = "&") => `${ampersand}:not(:has(:is(button, input):active)):active, .sortable-overlay:not(.dropping) > ${ampersand}`;

const StyledSettingsCard = styled(StyledCard)<{
	/** Override the default trailing gap. */
	$trailingGap?: number | string;
	/** Is the orientation of the icon changed based on the writing direction? */
	$dirBasedIcon?: DirBasedIcon;
}>(({ $trailingGap, $dirBasedIcon }) => css`
	${styledExpanderItemContent};
	backdrop-filter: blur(4px);

	${$trailingGap !== undefined && css`
		.trailing {
			gap: ${styles.toValue($trailingGap)};
		}
	`}

	> .base {
		${styledExpanderItemBase};
		position: relative;

		> .leading {
			${styledDirBasedIcon($dirBasedIcon)};
		}

		> .trailing {
			max-inline-size: 100%;

			> button.button,
			> .contents > button.button {
				align-self: stretch;
			}
		}
	}

	&[disabled] > .base {
		background-color: ${c("fill-color-control-disabled")};

		> .leading > .icon,
		> .leading > .text > :not(.select-info) {
			opacity: ${c("disabled-text-opacity")};
		}

		> .trailing {
			@layer layout {
				color: ${c("fill-color-text-disabled")};
			}
		}
	}

	button&:not(.container) {
		@container expander-parent scroll-state(stuck: top) {
			> .base {
				background-color: ${c("background-fill-color-expander-sticky-background-default")};
			}

			&:hover > .base {
				background-color: ${c("background-fill-color-expander-sticky-background-secondary")};
			}

			&[disabled] > .base {
				background-color: ${c("background-fill-color-expander-sticky-background-disabled")};
			}
		}

		&:hover,
		${isPressed()} {
			border-color: ${c("stroke-color-control-stroke-default")};
		}

		&:hover > .base {
			background-color: ${c("fill-color-control-secondary")};
		}

		${isPressed("&:not(:has(.trailing .toggle-switch-base))")},
		${isPressed("&:has(.trailing .toggle-switch-base:not(:active, .pressing, .pressed))")} {
			> .base {
				background-color: ${c("fill-color-control-tertiary")};

				@container expander-parent scroll-state(stuck: top) {
					background-color: ${c("background-fill-color-expander-sticky-background-tertiary")};
				}
			}

			> .base > .leading > .icon,
			> .base > .leading > .text,
			> .base > .trailing > .check-info,
			&.button > .base > .trailing > .action-icon {
				opacity: ${c("pressed-text-opacity")};
			}
		}
	}

	&.expander-parent {
		&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}, p, span):hover)):hover {
			.action-icon {
				background-color: ${c("fill-color-subtle-secondary")};
			}
		}

		${isPressed("&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active))")} {
			.action-icon {
				color: ${c("fill-color-text-secondary")};
				background-color: ${c("fill-color-subtle-tertiary")};
			}
		}
	}

	&:dir(rtl) .action-icon {
		scale: -1 1;
	}

	.drag-handle-shadow {
		${styles.mixins.gridCenter()};
		position: absolute;
		inset-block-start: 0;
		inset-inline-start: 0;
		z-index: 1;
		block-size: 100%;
		inline-size: ${15 + 20 + 16}px;
		border-radius: 3px;
		cursor: ${SortableView.Item.verticalDragHandleCursor};

		&::after {
			${styles.mixins.square("36px")};
			content: "";
			display: block;
			border-radius: 4px;

			.sortable-item.dragging & {
				box-shadow: none;
			}

			${ifColorScheme.contrast} & {
				display: none;
			}
		}

		&:hover:not(:active)::after {
			background-color: ${c("fill-color-subtle-secondary")};

			.sortable-overlay & {
				background-color: transparent;
			}
		}

		${styles.mixins.forwardFocusRing("&::after, body:has(&) .sortable-overlay .drag-handle-shadow::after")};
	}

	.sortable-overlay & {
		backdrop-filter: none;
	}

	> .base > .leading > .text {
		min-block-size: 20px;
		overflow-block: clip;
		transition: ${fallbackTransitions}, block-size ${eases.easeOutMaterialEmphasized} 250ms;
	}

	&.secondary {
		border: 1px solid ${c("stroke-color-card-stroke-default")};

		> .base {
			background-color: ${c("background-fill-color-card-background-secondary")};
		}
	}

	&,
	.expander:has(> &) {
		block-size: fit-content;
	}
`);

export default function SettingsCard({ icon = "placeholder", title, details, selectInfo, selectValid = true, actionIcon, disabled, children, type = "container", dragHandle, appearance = "primary", trailingGap, className, tabIndex, dirBasedIcon, anchor, ariaIdRef, ref, onClick, onFocus, ...htmlAttrs }: FCP<{
	/** Icon. Use an empty string or Boolean type to indicate disabling. */
	icon?: DeclaredIcons | "" | boolean | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Specifies the display string of the selection of tracks or track events. */
	selectInfo?: ReactNode;
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. */
	selectValid?: boolean | number;
	/** Trailing Action icon. Use an empty string or Boolean type to indicate disabling. */
	actionIcon?: DeclaredIcons | "" | boolean;
	/**
	 * Component form type.
	 * - `container` - A normal `<div>` box, cannot be clicked.
	 * - `button` - A button that can be clicked, default trailing icon is chevron right.
	 * - `expander` - An accordion item that can be expanded or collapsed, default trailing icon is chevron down.
	 * - `container-but-button` - Similar to `container`, but it is a `<button>` instead of a `<div>`,
	 * useful when dynamically change the `type`, the tag name will not be changed, thus avoid the re-rendering element issue.
	 */
	type?: "container" | "button" | "expander" | "container-but-button";
	/** Show the drag handle to represent that it is sortable? */
	dragHandle?: boolean;
	/** Appearance preference. */
	appearance?: "primary" | "secondary";
	/** Override the default trailing gap. */
	trailingGap?: number | string;
	/** Is the orientation of the icon changed based on the writing direction? */
	dirBasedIcon?: DirBasedIcon;
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/** Pass settings card aria ID to the parent component. */
	ariaIdRef?: RefObject<string | undefined | null>;
}, "div">) {
	actionIcon ??= type === "button" ? "chevron_right" :
		type === "expander" ? "chevron_down" : undefined;
	const dragHandleContext = useContext(SortableView.Item.Context);
	const ariaId = useId();
	useImperativeHandle(ariaIdRef, () => ariaId, [ariaId]);
	tabIndex ??= type.in("container", "container-but-button") ? -1 : 0;

	const handleFocus: FocusEventHandler<HTMLDivElement> = e => {
		onFocus?.(e);
		if (~tabIndex || e.target !== e.currentTarget) return;
		findFirstFocusableElement(e.target)?.focus();
	};

	return (
		<ClickOnSameElement onClick={onClick as never}>
			<StyledSettingsCard
				as={type === "container" ? "div" : "button"}
				$dirBasedIcon={dirBasedIcon}
				className={[
					className,
					type === "container-but-button" ? "container" : type === "expander" ? "expander-parent" : type,
					{
						secondary: appearance === "secondary",
						dragging: dragHandleContext.isDragging,
					},
				]}
				disabled={disabled}
				aria-disabled={disabled || undefined}
				aria-labelledby={`${ariaId}-title`}
				aria-describedby={`${ariaId}-details`}
				tabIndex={tabIndex}
				$trailingGap={trailingGap}
				data-anchor={anchor}
				ref={ref}
				onFocus={handleFocus}
				{...htmlAttrs}
			>
				<div className="base">
					<SettingsCardBase
						leading={(
							<>
								{dragHandle && (
									<>
										<div
											className="drag-handle-shadow"
											tabIndex={-1}
											ref={dragHandleContext.ref}
											role="button"
											aria-label={t.aria.reorderHandle}
											{...dragHandleContext.attributes}
											{...dragHandleContext.listeners}
										/>
										<Icon name="reorder_dots" className="drag-handle-icon" />
									</>
								)}
								{typeof icon === "object" ? icon : <Icon name={icon} />}
								<Transitions.DynamicAutoSize specified="height">
									<div className="text">
										<p className="title" id={`${ariaId}-title`} aria-hidden><Preserves>{title}</Preserves></p>
										<p className="details" id={`${ariaId}-details`} aria-hidden><Preserves>{details}</Preserves></p>
										<SettingsCardSelectInfo valid={selectValid}>{selectInfo}</SettingsCardSelectInfo>
									</div>
								</Transitions.DynamicAutoSize>
							</>
						)}
						trailing={(
							<Expander.Context value={{ place: "action" }}>
								{flattenReactChildren(children).map((child, i) => {
									const propsWithDisabled = {
										key: (() => {
											const key = isObject(child) && "key" in child && child.key;
											return key == null || key === false ? i : key;
										})(),
										disabled: disabled ?? false,
										"aria-disabled": disabled || undefined,
									};
									// React dislike using key with spread operator in JSX, but in React.cloneElement you have to do this,
									// so treat it individually.
									const { key: _key, ...propsWithDisabledWithoutKey } = propsWithDisabled;
									return !isValidElement(child) ?
										!child ? child : <p key={i} {...propsWithDisabledWithoutKey}>{child}</p> :
										React.cloneElement(child, propsWithDisabled);
								})}
								{actionIcon && typeof actionIcon === "string" && (
									<div className={["action-icon", TRAILING_EXEMPTION]} data-type={type}>
										<Icon name={actionIcon} />
									</div>
								)}
							</Expander.Context>
						)}
					/>
				</div>
			</StyledSettingsCard>
		</ClickOnSameElement>
	);
}

const SETTINGS_CARD_TRAILING_MAX_WIDTH = 200;

const StyledLeading = styled.div`
	&.contents {
		display: contents !important;
	}
`;

function SettingsCardBase({ threshold = SETTINGS_CARD_TRAILING_MAX_WIDTH, leading, trailing }: FCP<{
	/** Specified the min width threshold, if the trailing part is wider then it, the settings card base will be wrapped. */
	threshold?: number;
	/** Leading part. */
	leading?: ReactNode;
	/** Trailing part. */
	trailing?: ReactNode;
	children?: never;
}, "div">) {
	const [wrapped, setWrapped] = useState(false);
	const trailingEl = useDomRef<"div">();

	useMountEffect(() => {
		if (!trailingEl.current) return;
		const observer = new ResizeObserver(([{ borderBoxSize: [{ inlineSize }] }]) => {
			setWrapped(inlineSize > threshold);
		});
		observer.observe(trailingEl.current);
		return () => observer.disconnect();
	});

	return (
		<>
			<StyledLeading className={!wrapped && "contents"}>{leading}</StyledLeading>
			{trailing && <div ref={trailingEl} className="trailing">{trailing}</div>}
		</>
	);
}

SettingsCard.Base = SettingsCardBase;
SettingsCard.SelectInfo = SettingsCardSelectInfo;
