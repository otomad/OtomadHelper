export /* internal */ const styledExpanderItemBase = css`
	padding: 7px 47px;
	display: flex;
	gap: 16px;
	align-items: center;
	min-height: 48px;

	> :not(.text) {
		flex-shrink: 0;
	}
`;

export /* internal */ const styledExpanderItemContent = css`
	.icon {
		font-size: 16px;
	}

	.icon-placeholder {
		${styles.mixins.square("16px")};
	}

	.text {
		width: 100%;

		> :empty {
			display: none;
		}

		.heading {
			font-size: 14px;
		}

		.caption {
			font-size: 12px;
			color: ${c("fill-color-text-secondary")};
		}
	}

	.trailing {
		display: flex;
		align-items: center;
		gap: 10px;

		.trailing-icon {
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			margin-right: -7px;
			border-radius: 3px;
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
