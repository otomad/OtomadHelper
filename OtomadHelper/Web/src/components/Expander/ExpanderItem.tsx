export /* internal */ const styledExpanderItemBase = css`
	display: flex;
	gap: 16px;
	align-items: center;
	min-height: 48px;

	:where(&) {
		padding: 7px 51px;
	}

	> :not(.text) {
		flex-shrink: 0;
	}
`;

export /* internal */ const styledExpanderItemContent = css`
	.icon-placeholder {
		${styles.mixins.square("20px")};
	}

	.text {
		width: 100%;

		> :empty {
			display: none;
		}

		.heading {
			line-height: 20px;
		}

		.caption {
			font-size: 12px;
			line-height: 16px;
			color: ${c("fill-color-text-secondary")};
		}
	}

	.trailing {
		display: flex;
		align-items: center;
		gap: 1rem;

		.trailing-icon {
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			margin-right: -7px;
			border-radius: 3px;
			flex-shrink: 0;

			.icon {
				font-size: 16px;
			}
		}
	}
`;

const StyledExpanderItem = styled.div`
	${styledExpanderItemBase};
	padding-left: 15px;

	${styledExpanderItemContent};
`;

export default function ExpanderItem({ icon, heading, caption, children }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	heading?: ReactNode;
	/** 详细描述。 */
	caption?: ReactNode;
}>) {
	return (
		<StyledExpanderItem>
			{icon ? <Icon name={icon} /> : <div className="icon-placeholder" />}
			<div className="text">
				<div className="heading">{heading}</div>
				<div className="caption">{caption}</div>
			</div>
			<div className="trailing">
				{children}
			</div>
		</StyledExpanderItem>
	);
}