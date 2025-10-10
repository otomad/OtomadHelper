import ExpanderItemCurve from "components/Business/ExpanderItemCurve";

export /* @internal */ const styledExpanderItemBase = css`
	container: setting-card-base;
	display: flex;
	flex-wrap: wrap;
	gap: 16px;
	align-items: center;
	min-block-size: 48px;
	overflow-inline: clip;

	:where(&) {
		padding: ${expanderItemPadding[0]}px ${expanderItemPadding[1]}px;
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

		.title {
			${styles.effects.text.body};
		}

		.details {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};
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
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			flex-shrink: 0;
			margin-block: -4px;
			margin-inline-end: -7px;
			border-radius: 3px;

			.icon {
				font-size: 16px;
			}

			&:last-child:not(:first-child) {
				margin-inline-start: -3px;
			}
		}
	}
`;

const StyledExpanderItem = styled.div<{
	/** With clickable style? */
	$clickable?: boolean;
	/** As sub title style? */
	$asSubtitle?: boolean;
	/** Remove the top split line and top padding from the expand child. */
	$noDivider?: boolean;
	/** Do not wrap the action children to the second line if the text is too long? */
	$nowrap?: boolean;
}>`
	${styledExpanderItemBase};
	padding-inline-start: ${expanderItemWithIconPaddingInlineStart}px;

	${styledExpanderItemContent};

	&[disabled] > .leading > :is(.text, .icon) {
		opacity: var(--disabled-text-opacity);
	}

	${ifProp("$clickable", css`
		:not(.sortable-item) > &:hover,
		.sortable-item:not(.dragging) > &:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		.sortable-item:not(.dragging) > &:active,
		.sortable-overlay:not(.dropping) &${important()} {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		.sortable-item:last-child > &,
		:not(.sortable-item, .sortable-overlay) > &:last-child {
			border-radius: 0 0 2px 2px;
		}
	`)}

	${ifProp("$asSubtitle", css`
		padding-block-end: 0;

		.text .title {
			${styles.effects.text.bodyStrong};
		}
	`)}

	${ifProp("$noDivider", css`
		padding-block: 0;
		border-block-start-width: 0 !important;
	`)}

	${ifProp("$nowrap", css`
		flex-wrap: nowrap;

		.leading {
			flex-shrink: unset;
			min-inline-size: unset;
		}
	`)}
`;

export /* @internal */ default function ExpanderItem({ icon, title, details, clickable, asSubtitle, noDivider, ariaHiddenForText, nowrap = false, anchor, children, disabled = false, actionsMinWidthThreshold, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** With clickable style? */
	clickable?: boolean;
	/** As sub title style? */
	asSubtitle?: boolean;
	/** Remove the top split line and top padding from the expand child. */
	noDivider?: boolean;
	/** Remove text from aria tree? */
	ariaHiddenForText?: boolean;
	/** Do not wrap the action children to the second line if the text is too long? */
	nowrap?: boolean;
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/** Specified the min width threshold, if the actions part is wider then it, the settings card base will be wrapped. */
	actionsMinWidthThreshold?: number;
}, "div">) {
	disabled = useContext(InteractionStateContext).disabled || disabled;
	return (
		<StyledExpanderItem
			$clickable={clickable}
			$asSubtitle={asSubtitle}
			$noDivider={noDivider}
			$nowrap={nowrap}
			disabled={disabled}
			aria-disabled={disabled || undefined}
			data-anchor={anchor}
			{...htmlAttrs}
		>
			<InteractionStateContext value={{ disabled }}>
				<SettingsCard.Base
					threshold={actionsMinWidthThreshold}
					leading={(
						<>
							{icon ? typeof icon === "string" ? <Icon name={icon} /> : icon : <Icon shadow />}
							<div className="text" aria-hidden={ariaHiddenForText}>
								<p className="title"><Preserves>{title}</Preserves></p>
								<p className="details"><Preserves>{details}</Preserves></p>
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
