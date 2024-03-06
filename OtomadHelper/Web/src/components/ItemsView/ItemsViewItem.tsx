import { styledExpanderItemText } from "components/Expander/ExpanderItem";

export const GRID_VIEW_ITEM_HEIGHT = 112;

const StyledItemsViewItem = styled.button<{
	/** 显示方式：列表、平铺、网格。 */
	$view: "list" | "tile" | "grid";
}>`
	${styles.mixins.forwardFocusRing()};

	${({ $view }) => $view === "grid" ? css`
		:where(.image-wrapper) {
			width: 100%;
			height: ${GRID_VIEW_ITEM_HEIGHT}px;
		}

		.base {
			position: relative;
			overflow: clip;
			border-radius: 4px;
		}

		.text-part {
			display: flex;
			gap: 10px;
			align-items: center;
			margin: 5px 0;
			text-align: left;
		}

		.selection {
			position: absolute;
			inset: 0;
			border-radius: inherit;
			pointer-events: none;
		}

		&:hover .selection {
			background-color: ${c("fill-color-subtle-secondary")};
			box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-on-accent-tertiary")} inset;
		}

		&:active .selection {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&.selected .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color")} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected:hover .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color", 90)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected:active .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color", 80)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}
	` : css`
		padding: 2px 4px;

		.base {
			position: relative;
			display: flex;
			gap: 16px;
			align-items: center;
			min-height: 48px;
			padding: 8px 12px;
			border-radius: 3px;

			&::before {
				${styles.mixins.oval()};
				content: "";
				position: absolute;
				left: 0;
				width: 3px;
				height: ${100 / 3}%;
				background-color: ${c("accent-color")};
			}
		}

		&:hover .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active .base {
			background-color: ${c("fill-color-subtle-tertiary")};

			&::before {
				scale: 1 0.625;
			}

			> * {
				opacity: ${c("pressed-text-opacity")}
			}
		}

		&:not(.selected) .base::before {
			scale: 1 0;
		}

		${styledExpanderItemText};

		.image-wrapper {
			${styles.mixins.flexCenter()};
		}
	`}
`;

const DefaultImage = styled.img`
	${styles.mixins.square("100%")};
	object-fit: cover;
`;

export /* internal */ default function ItemsViewItem({ image, icon, id: _id, selected = false, details, view, children, className, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string | ReactNode;
	/** 图标。 */
	icon?: string;
	/** 标识符。 */
	id: string;
	/** 是否已选中？ */
	selected?: boolean;
	/** 详细描述。 */
	details?: ReactNode;
	/** @private 显示方式：列表、平铺、网格。 */
	view?: "list" | "tile" | "grid";
}, "button">) {
	const textPart = (
		<div className="text">
			<p className="title">{children}</p>
			<p className="details">{details}</p>
		</div>
	);

	return (
		<StyledItemsViewItem $view={view!} className={[className, view, { selected }]} tabIndex={0} {...htmlAttrs}>
			<div className="base">
				{view === "grid" ? (
					<>
						<div className="image-wrapper">
							{typeof image === "string" ? <DefaultImage src={image} /> : image}
						</div>
						<div className="selection" />
					</>
				) : (
					<>
						{(image || icon) && (
							<div className="image-wrapper">
								{typeof image === "string" ? <img src={image} /> : icon ? <Icon name={icon} /> : undefined}
							</div>
						)}
						{textPart}
					</>
				)}
			</div>
			{view === "grid" && (
				<div className="text-part">
					{icon && <Icon name={icon} />}
					{textPart}
				</div>
			)}
		</StyledItemsViewItem>
	);
}
