import { styledExpanderItemText } from "components/Expander/ExpanderItem";

export const GRID_VIEW_ITEM_HEIGHT = 112;

const isPressed = ":active:not(:has(button:active))";

export /* @internal */ const styledSimpleIndicator = css`
	&::before {
		${styles.mixins.oval()};
		content: "";
		position: absolute;
		inset-inline-start: 0;
		align-self: center;
		block-size: ${100 / 3}%;
		inline-size: 3px;
		background-color: ${c("accent-color")};

		@starting-style {
			scale: 1 0;
		}
	}
`;

interface StyledItemsViewItemProps {
	/** View mode: list, tile, grid. */
	$view: ItemView;
	/** Add additional borders to the normal state of the image wrapper? */
	$withBorder?: boolean;
	/** Custom selection color. Defaults to accent color. */
	$selectionColor?: string;
	/** Is the orientation of the icon changed based on the writing direction? */
	$dirBasedIcon?: DirBasedIcon;
	/** `list`, `tile` - Align items (usually vertical alignment). */
	$alignItems?: "start" | "center";
}

const ItemsViewItemTextPart = styled.div({});

const StyledItemsViewItem = styled.button<StyledItemsViewItemProps>(() => css<StyledItemsViewItemProps>`
	${styles.mixins.forwardFocusRing()};
	--selection-color: ${({ $selectionColor }) => $selectionColor || c("accent-color")};

	&:hover,
	&:focus-visible {
		${useLottieStatus.animation("Hover")};
	}

	${styledDirBasedIcon}

	${({ $view, $withBorder, $alignItems }) => $view === "grid" ? css`
		display: flex;
		flex-direction: column;
		block-size: 100%;

		:where(.image-wrapper) {
			block-size: ${GRID_VIEW_ITEM_HEIGHT}px;
			inline-size: 100%;
		}

		.image-wrapper {
			position: relative;
			contain: paint;
			border-radius: inherit;
			isolation: isolate;

			.checkbox-label.items-view-item-checkbox {
				position: absolute;

				&.top {
					inset-block-start: 6px;
				}

				&.bottom {
					inset-block-end: 6px;
				}

				&.left {
					inset-inline-start: 6px;
				}

				&.right {
					inset-inline-end: 6px;
				}
			}
		}

		> .base {
			container: grid-view-item-image / inline-size;
			position: relative;
			overflow: clip;
			border-radius: 4px;

			> .badge {
				position: absolute;
				inset-block-end: 4px;
				inset-inline-start: 4px;
			}
		}

		.text-part {
			display: flex;
			gap: 10px;
			align-items: ${$alignItems};
			margin: 5px 0;
			text-align: start;
		}

		.selection {
			position: absolute;
			inset: 0;
			border-radius: inherit;
			pointer-events: none;
			forced-color-adjust: none;

			${$withBorder && css`
				box-shadow: 0 0 0 1px ${c("stroke-color-surface-stroke-default")} inset;
			`}
		}

		&:not(.selected) .selection {
			transition: ${fallbackTransitions}, box-shadow ${eases.easeOutQuad} 250ms;
		}

		&:hover .selection {
			background-color: ${c("fill-color-subtle-secondary")};

			${!$withBorder && css`
				box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-on-accent-tertiary")} inset;
			`}
		}

		&:not(.selected):has(.image-wrapper .no-border) .selection {
			box-shadow: none;
		}

		&${isPressed} .selection {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:is(:hover, ${isPressed}) .selection {
			${ifColorScheme.at.contrast} {
				background-color: transparent;
			}

			${ifColorScheme.at.reduceTransparency} {
				background-color: transparent;
			}
		}

		&.selected .selection {
			box-shadow:
				0 0 0 2px ${c("selection-color")} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected:hover .selection {
			box-shadow:
				0 0 0 2px ${c("selection-color", 90)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected${isPressed} .selection {
			box-shadow:
				0 0 0 2px ${c("selection-color", 80)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.critically-highlight .selection {
			box-shadow:
				0 0 0 2px ${c("fill-color-system-critical")} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset !important;
		}
	` : css`
		padding: 2px 4px;

		> .base {
			position: relative;
			display: flex;
			flex-wrap: nowrap;
			gap: 16px;
			align-items: ${$alignItems};
			block-size: 100%;
			min-block-size: 48px;
			padding-block: 12px;
			padding-inline: 8px;
			overflow: clip;
			border-radius: 3px;

			${$view === "tile" && css`padding-inline-start: 16px;`};

			> * {
				transition: ${fallbackTransitions}, translate 0s;
			}

			${styledSimpleIndicator};

			&::before {
				background-color: ${c("selection-color")};
			}
		}

		main.page > .container > .items-view & {
			padding-inline: 0;

			> .base {
				padding-inline: 16px;
			}
		}

		&:hover > .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&${isPressed} > .base {
			background-color: ${c("fill-color-subtle-tertiary")};

			&::before {
				scale: 1 0.625;
			}

			> :not(.checkbox-label) {
				opacity: ${c("pressed-text-opacity")};
			}
		}

		&:not(.selected) > .base::before {
			scale: 1 0;
		}

		${ifColorScheme.at.contrast} {
			&:is(:hover, ${isPressed}, .selected) > .base {
				background-color: ${cc("Highlight")};

				&,
				:not(button.button, button.button *) {
					color: ${cc("HighlightText")};
				}

				&::before {
					background-color: ${cc("HighlightText")};
				}
			}
		}

		&:has(.checkbox-label:not(.exit-done)) > .base::before {
			display: none;
		}

		${styledExpanderItemText};

		.image-wrapper {
			${styles.mixins.flexCenter()};
			${$alignItems === "start" && css`margin-block-start: 5px;`}
		}

		.flyout & {
			padding: 0 4px;

			> .base {
				gap: 12px;
				min-height: unset;
				padding-block: 8px;
				padding-inline: 13px 12px;
				border-radius: 4px;
			}
		}

		${$view === "list" && css`
			${ItemsViewItemTextPart} {
				flex: 1;
				contain: inline-size;
			}
		`}
	`}

	${ItemsViewItemTextPart} > * {
		${styles.effects.text.body};

		&:empty {
			display: none;
		}

		&.details {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};

			small {
				display: block;
				line-height: normal;
			}
		}
	}

	&.selected ${ItemsViewItemTextPart} .title {
		${styles.effects.text.bodyStrong};
	}

	.checkbox-label {
		${tgs()} {
			opacity: 0;

			&,
			& + * {
				translate: ${-(18 + 16)}px;

				&:dir(rtl) {
					translate: ${18 + 16}px;
				}
			}
		}

		&:is(.enter-active, .exit-active) {
			&,
			& + * {
				transition: translate ${eases.easeOutMax} 250ms, opacity ${eases.easeOutMax} 250ms;
			}
		}
	}
`);

const DefaultImage = styled.img`
	${styles.mixins.square("100%")};
	object-fit: cover;
`;

const ItemsViewItemStateContext = createContext<{
	hover: boolean;
}>({
	hover: false,
});

export type OnItemsViewItemClickEventHandler<T> = (id: T, selected: CheckState, e: React.MouseEvent<HTMLElement>) => void;

export /* @internal */ default function ItemsViewItem<T>({ image, icon, id, selected: _selected = "unchecked", details, actions, withBorder = false, alignItems = "center", baseAttrs, disableCheckmarkTransition, imageOverlay, selectionColor, dirBasedIcon, badge, tooltip, criticallyHighlight, checkmarkPosition = "top right", _view: view = undefined!, _multiple: multiple, _multipleChangeable: multipleChangeable = false, children, className, "aria-label": ariaLabel, "aria-description": ariaDescription, onSelectedChange, onClick, ...htmlAttrs }: FCP<{
	/** Image. */
	image?: string | ReactNode;
	/** Icon. */
	icon?: DeclaredIcons | Exclude<ReactNode, Iterable<ReactNode>>;
	/** Identifier. */
	id: T;
	/** Selected? */
	selected?: CheckState | StateProperty<boolean>;
	/** Detailed description. */
	details?: ReactNode;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/** Add additional borders to the normal state of the image wrapper? */
	withBorder?: boolean;
	/** `list`, `tile` - Align items (usually vertical alignment). */
	alignItems?: "start" | "center";
	/** Additional attributes for the base element of the items view item. */
	baseAttrs?: Partial<FCP<{}, "div">> & Record<string, Any>;
	/**
	 * Temporarily disable the transition of the checkbox's checkmark icon?
	 *
	 * It is as well to disable the checkmark transition when it appears abnormal or affects the user experience
	 * (for example, when executing the `startViewTransition` function in the View Transition API simultaneously).
	 *
	 * Effective only in multiple selection mode.
	 */
	disableCheckmarkTransition?: boolean;
	/** Add other elements overlay the image. */
	imageOverlay?: ReactNode;
	/** Custom selection color. Defaults to accent color. */
	selectionColor?: string;
	/** Is the orientation of the icon changed based on the writing direction? */
	dirBasedIcon?: DirBasedIcon;
	/** Show badge on the item. */
	badge?: BadgeValue | BadgeArgs;
	/** Custom tooltip. */
	tooltip?: TooltipProps | string;
	/** Show a red color border? */
	criticallyHighlight?: boolean;
	/** Change the checkmark position at the image, available only in grid view mode and multiple selection mode. @default "top right" */
	checkmarkPosition?: "top left" | "top right" | "bottom left" | "bottom right";
	/** @private View mode: list, tile, grid. */
	_view?: ItemView;
	/** @private Multiple selection mode? */
	_multiple?: boolean;
	/** @private Can `multiple` prop be dynamically changed? Set it to false for better performance, and set it to true to present a better animation. @default false */
	_multipleChangeable?: boolean;
	/** Occurs when the selection changed. */
	onSelectedChange?(id: T, selected: CheckState): void;
	/** Occurs when user click it. */
	onClick?: OnItemsViewItemClickEventHandler<T>;
}, "button">) {
	let selected: CheckState, setSelected: SetStateNarrow<boolean> | undefined;
	if (isReadonlyArray(_selected)) {
		setSelected = _selected[1] as never;
		selected = _selected[0] ? "checked" : "unchecked";
	} else
		selected = _selected;
	if (typeof tooltip === "string" || isI18nItem(tooltip))
		tooltip = { title: String(tooltip), placement: "block" };
	if (badge !== undefined && !Array.isArray(badge))
		badge = [badge, "neutual", false];

	const ariaId = useId();
	const textPart = (children || details) && (
		<ItemsViewItemTextPart aria-hidden>
			{children && <p className="title" id={`${ariaId}-title`}>{children}</p>}
			{details && <p className="details" id={`${ariaId}-details`}><Preserves>{details}</Preserves></p>}
		</ItemsViewItemTextPart>
	);
	const checkboxContent = <Checkbox className={["items-view-item-checkbox", checkmarkPosition]} value={[selected]} plain inert disableCheckmarkTransition={disableCheckmarkTransition} />;
	const checkbox = multipleChangeable ?
		<CssTransition in={multiple} timeout={250} requestAnimationFrame>{checkboxContent}</CssTransition> :
		multiple && checkboxContent;
	const iconOrElement = typeof icon === "string" ? <Icon name={icon} /> : icon;
	const el = useDomRef<"button">();

	const [hover, setHover] = useState(false);
	const handleAnimation = useCallback((e: AnimationEvent, isAnimationStart: boolean) => {
		if (e.animationName !== getLottieStatusName("Hover")) return;
		setHover(isAnimationStart);
	}, []);

	useEffect(() => onSelectedChange?.(id, selected), [selected, id, onSelectedChange]);

	useOnFormKeyDown(el, { item: ".items-view-item" });

	return (
		<ItemsViewItemStateContext value={{ hover }}>
			<Tooltip disabled={!tooltip} {...tooltip!}>
				<EventInjector ref={el} onAnimationStart={e => handleAnimation(e, true)} onAnimationCancel={e => handleAnimation(e, false)}>
					<StyledItemsViewItem
						$view={view}
						$withBorder={withBorder}
						$selectionColor={selectionColor}
						$dirBasedIcon={dirBasedIcon}
						$alignItems={alignItems}
						className={[className, view, { selected: selected !== "unchecked", criticallyHighlight }]}
						tabIndex={0}
						role={multiple ? "checkbox" : "radio"}
						aria-checked={checkStateToAriaChecked(selected)}
						aria-labelledby={ariaLabel !== undefined ? undefined : `${ariaId}-title`}
						aria-describedby={ariaDescription !== undefined ? undefined : `${ariaId}-details`}
						aria-label={ariaLabel}
						aria-description={ariaDescription}
						onClick={e => { onClick?.(id, selected, e); setSelected?.(selected => !selected); }}
						{...htmlAttrs}
					>
						<div className="base" {...baseAttrs}>
							{view === "grid" ? (
								<>
									<div className="image-wrapper">
										{typeof image === "string" ? <DefaultImage src={image} /> : image}
										{imageOverlay}
										{checkbox}
									</div>
									{badge && !badge[2] && <Badge status={badge[1]} transitionOnAppear={false}>{badge[0]}</Badge>}
									<div className="selection" />
								</>
							) : (
								<>
									{checkbox}
									{(image || icon) && (
										<div className="image-wrapper">
											{typeof image === "string" ? <img src={image} alt="" /> : iconOrElement}
										</div>
									)}
									{textPart}
									{actions}
								</>
							)}
						</div>
						{view === "grid" && (iconOrElement || textPart) && (
							<div className="text-part">
								{iconOrElement}
								{textPart}
							</div>
						)}
					</StyledItemsViewItem>
				</EventInjector>
			</Tooltip>
		</ItemsViewItemStateContext>
	);
}

ItemsViewItem.StateContext = ItemsViewItemStateContext;
