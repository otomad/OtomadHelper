import ExpanderItemCurve, { ExpanderItemCrossfadeCurve } from "components/Business/Expander/ExpanderItemCurve";

export /* @internal */ const styledExpanderItemBase = css`
	container: setting-card-base;
	display: flex;
	flex-wrap: wrap;
	gap: 16px;
	align-items: center;
	min-block-size: 48px;
	inline-size: 100%;
	overflow-inline: clip;
	background-clip: padding-box;

	:where(&) {
		padding-block: ${expanderItemPadding[0]}px;
		padding-inline: ${expanderItemPadding[1]}px;
	}

	> :not(.text) {
		flex-shrink: 0;
	}
`;

export /* @internal */ const styledExpanderItemText = css`
	.text {
		> * {
			${styles.mixins.hideIfEmpty()};
		}

		.title:where(:not(:is(.radio-button-label, .checkbox-label) *)) {
			${styles.effects.text.body};
		}

		.details {
			${styles.effects.text.caption};
			display: -webkit-box;
			-webkit-box-orient: vertical;
			color: ${c("fill-color-text-secondary")};
			text-overflow: ellipsis;
		}
	}
`;

export /* @internal */ const styledExpanderItemContent = css`
	${styledExpanderItemText};

	.leading {
		display: flex;
		flex-wrap: nowrap;
		gap: inherit;
		align-items: center;
		min-inline-size: fit-content;
		max-inline-size: 100%;
	}

	.text {
		flex: 1;
		inline-size: 100%;

		.title {
			hyphens: none;
		}
	}

	.trailing {
		display: flex;
		gap: 16px;
		align-items: center;
		margin-inline-start: auto;

		> * {
			@layer layout {
				gap: 8px;
			}
		}

		.action-icon {
			@layer props {
				--state: normal;
			}

			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			flex-shrink: 0;
			margin-block: -4px;
			margin-inline-end: -7px;
			color: if(
				style(--state: active): ${c("fill-color-text-secondary")};
				style(--state: disabled): ${c("fill-color-text-disabled")};
				else: ${c("foreground-color")};
			);
			pointer-events: none;

			&:is([disabled], [disabled] *) {
				--state: disabled;
			}

			.icon {
				font-size: 16px;
			}

			&:last-child:not(:first-child) {
				margin-inline-start: -3px;
			}

			> * {
				${styles.mixins.enableHardware3d()};
			}

			&.expander-chevron {
				@layer props {
					--expansion: collapsed;
				}

				background-color: if(
					style(--state: hover): ${c("fill-color-subtle-secondary")};
					style(--state: active): ${c("fill-color-subtle-tertiary")};
					else: transparent;
				);
				border-radius: 3px;

				.icon {
					translate: if(
						style((--state: active) and (--expansion: collapsed)): 0 -2px;
						style((--state: active) and (--expansion: expanded)): 0 2px;
						else: 0;
					);
					rotate: if(
						style(--expansion: expanded): 180deg;
						else: 0deg;
					);
				}
			}
		}
	}
`;

const StyledExpanderItem = styled.div<{
	/** With clickable style? */
	$clickable?: boolean;
	/** As sub title style? */
	$asSubtitle?: boolean | "closerAfter";
	/** Remove the top split line and top padding from the expand child. */
	$noDivider?: "before" | "after";
	/** Do not wrap the action children to the second line if the text is too long? */
	$nowrap?: boolean;
}>`
	${styledExpanderItemBase};
	padding-inline-start: ${expanderItemWithIconPaddingInlineStart}px;
	${styledExpanderItemContent};

	&[disabled] > .leading {
		> .icon,
		> .text > :not(.select-info) {
			opacity: var(--disabled-text-opacity);
		}
	}

	${ifProp("$clickable", css`
		:not(.sortable-item) > &:hover,
		.sortable-item:not(.dragging) > &:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		:not(.sortable-item) > &:active,
		.sortable-item:not(.dragging) > &:active,
		.sortable-overlay:not(.dropping) &${important()} {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		.sortable-item:last-child > &,
		:not(.sortable-item, .sortable-overlay) > &:last-child {
			border-end-start-radius: 2px;
			border-end-end-radius: 2px;
		}
	`)}

	${({ $asSubtitle }) => $asSubtitle && css`
		${$asSubtitle === "closerAfter" && css`
			min-block-size: unset;
			padding-block: ${expanderItemPadding[0] * 2}px ${expanderItemPadding[0] * 0.5}px;
		`}

		.text .title {
			${styles.effects.text.bodyStrong};
		}
	`}

	${({ $noDivider }) =>
		$noDivider === "before" ? css`
			border-block-start-width: 0 !important;
		` : $noDivider === "after" ? css`
			& + * {
				border-block-start-width: 0 !important;
			}
		` : undefined}
`;

export /* @internal */ default function ExpanderItem({ icon, title, details, clickable, nonFocusable, asSubtitle, noDivider, anchor, children, disabled = false, wrapActionsWhenNarrow, selectInfo, selectValid = true, onClick, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** With clickable style? */
	clickable?: boolean;
	/** Apply tabIndex = -1? Only available when `clickable` is true. */
	nonFocusable?: boolean;
	/** As sub title style? */
	asSubtitle?: boolean | "closerAfter";
	/** Remove the top split line and top padding from the expand child. */
	noDivider?: "before" | "after" | true;
	/** Remove text from aria tree? */
	// ariaHiddenForText?: boolean;
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/**
	 * Specify that when the window size is too narrow, if the actions cannot be fitted in, should the actions be wrapped to the second row?
	 * - `true`: The actions wrapped to the second row.
	 * - `false`: The actions stay on the first row and squeeze the text content before them.
	 * - `undefined`: It will auto detect whether the wrapping is needed. The default behavior is: wrapping when the actions are wider than 200px.
	 * @default undefined
	 */
	wrapActionsWhenNarrow?: boolean;
	/** Specifies the display string of the selection of tracks or track events. */
	selectInfo?: ReactNode;
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. @default true */
	selectValid?: boolean | number;
}, "div">) {
	const ariaId = useId();
	const cardBaseEls: PropsOf<typeof SettingsCard.Base>["ref"] = useRef({ leading: null, trailing: null });
	disabled = useContext(InteractionStateContext).disabled || disabled;
	if (noDivider === true) noDivider = "before";

	const handleClick: MouseEventHandler<HTMLDivElement> = e => {
		onClick?.(e);
		const expanderItemEl = e.currentTarget;
		if (!clickable && (e.target === expanderItemEl || isInPath(e, cardBaseEls.current.leading)) && cardBaseEls.current.trailing)
			findFirstFocusableElement(cardBaseEls.current.trailing)?.focus();
	};

	return (
		<StyledExpanderItem
			$clickable={clickable}
			$asSubtitle={asSubtitle}
			$noDivider={noDivider}
			disabled={disabled}
			tabIndex={nonFocusable ? -1 : clickable ? 0 : undefined}
			aria-disabled={disabled || undefined}
			data-anchor={anchor}
			as={clickable && !nonFocusable ? "button" as never : undefined}
			aria-labelledby={`${ariaId}-title`}
			aria-describedby={`${ariaId}-details`}
			onClick={handleClick}
			{...htmlAttrs}
		>
			<InteractionStateContext value={{ disabled }}>
				<SettingsCard.Base
					ref={cardBaseEls}
					wrap={wrapActionsWhenNarrow}
					leading={(
						<>
							{icon ? typeof icon === "string" ? <Icon name={icon} /> : icon : <Icon shadow />}
							<div className="text" aria-hidden>
								<p className="title" id={`${ariaId}-title`}><Preserves>{title}</Preserves></p>
								<p className="details" id={`${ariaId}-details`}><Preserves>{details}</Preserves></p>
								<SettingsCard.SelectInfo valid={selectValid}>{selectInfo}</SettingsCard.SelectInfo>
							</div>
						</>
					)}
					trailing={children}
				/>
			</InteractionStateContext>
		</StyledExpanderItem>
	);
}

ExpanderItem.Curve = ExpanderItemCurve;
ExpanderItem.CrossfadeCurve = ExpanderItemCrossfadeCurve;
