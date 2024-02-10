const StyledListViewItem = styled.button`
	padding: 2px 4px;

	.base {
		display: flex;
		align-items: center;
		gap: 16px;
		padding: 8px 12px;
		border-radius: 3px;
		position: relative;
		min-height: 48px;

		&::before {
			${styles.mixins.oval()};
			content: "";
			width: 3px;
			height: ${100 / 3}%;
			background-color: ${c("accent-color")};
			position: absolute;
			left: 0;
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

	&:not(.active) .base::before {
		scale: 1 0;
	}

	${styles.mixins.forwardFocusRing()};

	.text {
		display: flex;
		flex-direction: column;

		.heading {
			line-height: 20px;
		}

		.caption {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};
		}
	}

	.image-wrapper {
		${styles.mixins.flexCenter()};
	}
`;

export /* internal */ default function ListViewItem({ image, icon, id: _id, active = false, caption, children, className, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string;
	/** 图标。如有图片时会被代替。 */
	icon?: string;
	/** 标识符。 */
	id: string;
	/** 是否活跃状态？ */
	active?: boolean;
	/** 详细描述。 */
	caption?: ReactNode;
}, "button">) {
	return (
		<StyledListViewItem className={[className, { active }]} tabIndex={0} {...htmlAttrs}>
			<div className="base">
				{(image || icon) &&
					<div className="image-wrapper">
						{image ? <img src={image} /> : icon ? <Icon name={icon} /> : undefined}
					</div>
				}
				<div className="text">
					<p className="heading">{children}</p>
					<p className="caption">{caption}</p>
				</div>
			</div>
		</StyledListViewItem>
	);
}
