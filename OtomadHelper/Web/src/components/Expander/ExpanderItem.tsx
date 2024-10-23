import ExpanderItemCurve from "./ExpanderItemCurve";

export /* @internal */ const expanderItemWithIconPaddingInlineStart = 15;

export /* @internal */ const styledExpanderItemBase = css`
	container: setting-card-base;
	display: flex;
	flex-wrap: wrap;
	gap: 16px;
	align-items: center;
	min-height: 48px;
	overflow-x: clip;

	:where(&) {
		padding: 7px 51px;
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
			line-height: 20px;
		}

		.details {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};
		}
	}
`;

export /* @internal */ const styledExpanderItemContent = css`
	${styledExpanderItemText};

	&[disabled],
	&[disabled] .text .details {
		color: ${c("fill-color-text-disabled")};
	}

	.text {
		flex: 1;
		width: 100%;

		.title {
			hyphens: none;
		}
	}

	.trailing {
		display: flex;
		gap: 1rem;
		align-items: center;
		margin-inline-start: auto;

		.trailing-icon {
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			flex-shrink: 0;
			margin-right: -7px;
			border-radius: 3px;

			.icon {
				font-size: 16px;
			}
		}
	}
`;

const StyledExpanderItem = styled.div<{
	/** With clickable style? */
	$clickable?: boolean;
	/** As sub title style? */
	$asSubtitle?: boolean;
}>`
	${styledExpanderItemBase};
	padding-inline-start: ${expanderItemWithIconPaddingInlineStart}px;

	${styledExpanderItemContent};

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
`;

export /* @internal */ default function ExpanderItem({ icon, title, details, clickable, asSubtitle, children, disabled, ...htmlAttrs }: FCP<{
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
}, "div">) {
	return (
		<StyledExpanderItem $clickable={clickable} $asSubtitle={asSubtitle} disabled={disabled} {...htmlAttrs}>
			{icon ? typeof icon === "string" ? <Icon name={icon} /> : icon : <Icon shadow />}
			<div className="text">
				<p className="title"><Preserves>{title}</Preserves></p>
				<p className="details"><Preserves>{details}</Preserves></p>
			</div>
			<div className="trailing">
				{children}
			</div>
		</StyledExpanderItem>
	);
}

ExpanderItem.Curve = ExpanderItemCurve;
