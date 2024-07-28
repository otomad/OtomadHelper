import ExpanderItemCurve from "./ExpanderItemCurve";

export /* @internal */ const styledExpanderItemBase = css`
	container: setting-card-base;
	display: flex;
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
	.icon-placeholder {
		${styles.mixins.square("20px")};
		${styles.mixins.gridCenter()};
	}

	${styledExpanderItemText};

	.text {
		width: 100%;
	}

	.trailing {
		display: flex;
		gap: 1rem;
		align-items: center;

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
}>`
	${styledExpanderItemBase};
	padding-left: 15px;

	${styledExpanderItemContent};

	${ifProp("$clickable", css`
		:not(.sortable-item) > &:hover,
		.sortable-item:not(.dragging) > &:hover {
			background-color: ${c("fill-color-control-secondary")};
		}

		.sortable-item:not(.dragging) > &:active,
		.sortable-overlay:not(.dropping) &${important()} {
			background-color: ${c("fill-color-control-tertiary")};
		}

		.sortable-item:last-child > &,
		:not(.sortable-item, .sortable-overlay) > &:last-child {
			border-radius: 0 0 2px 2px;
		}
	`)}
`;

export /* @internal */ default function ExpanderItem({ icon, title, details, clickable, children }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** With clickable style? */
	clickable?: boolean;
}>) {
	return (
		<StyledExpanderItem $clickable={clickable}>
			{icon ? typeof icon === "string" ? <Icon name={icon} /> : icon : <div className="icon-placeholder" />}
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
