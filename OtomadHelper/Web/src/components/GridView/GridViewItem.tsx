export const GRID_VIEW_ITEM_HEIGHT = 112;

const StyledGridViewItem = styled.button`
	:where(.image-wrapper) {
		width: 100%;
		height: ${GRID_VIEW_ITEM_HEIGHT}px;
	}

	.base {
		position: relative;
		border-radius: 4px;
		overflow: hidden;
	}

	.heading {
		margin: 5px 0;
		text-align: left;
		display: flex;
		align-items: center;
		gap: 10px;
	}

	.selection {
		position: absolute;
		inset: 0;
		border-radius: inherit;
		pointer-events: none;
	}

	&:hover .selection {
		box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-on-accent-tertiary")} inset;
		background-color: ${c("fill-color-subtle-secondary")};
	}

	&:active .selection {
		background-color: ${c("fill-color-subtle-tertiary")};
	}

	&.active .selection {
		box-shadow:
			0 0 0 2px ${c("accent-color")} inset,
			0 0 0 3px ${c("fill-color-control-solid-default")} inset;
	}

	&.active:hover .selection {
		box-shadow:
			0 0 0 2px ${c("accent-color", 90)} inset,
			0 0 0 3px ${c("fill-color-control-solid-default")} inset;
	}

	&.active:active .selection {
		box-shadow:
			0 0 0 2px ${c("accent-color", 80)} inset,
			0 0 0 3px ${c("fill-color-control-solid-default")} inset;
	}

	${styles.mixins.forwardFocusRing()};
`;

const DefaultImage = styled.img`
	${styles.mixins.square("100%")};
	object-fit: cover;
`;

export default function GridViewItem({ image, icon, id: _id, active = false, children, className, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string | ReactNode;
	/** 图标。 */
	icon?: string;
	/** 标识符。 */
	id: string;
	/** 是否活跃状态？ */
	active?: boolean;
}, HTMLButtonElement>) {
	return (
		<StyledGridViewItem className={[className, { active }]} tabIndex={0} {...htmlAttrs}>
			<div className="base">
				<div className="image-wrapper">
					{typeof image === "string" ? <DefaultImage src={image} draggable={false} /> : image}
				</div>
				<div className="selection" />
			</div>
			<div className="heading">
				{icon && <Icon name={icon} />}
				<p>{children}</p>
			</div>
		</StyledGridViewItem>
	);
}
