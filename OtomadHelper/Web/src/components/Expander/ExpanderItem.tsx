const StyledExpanderItem = styled.div`
	padding: 7px 15px;
	padding-right: 47px;
	display: flex;
	gap: 16px;
	align-items: center;

	> :not(.text) {
		flex-shrink: 0;
	}

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
			color: ${c("white", 78.6)};

			${ifColorScheme.light} & {
				color: ${c("black", 60.63)};
			}
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

const ExpanderItem: FC<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	heading?: ReactNode;
	/** 详细描述。 */
	caption?: ReactNode;
}> = ({ icon, heading, caption, children }) => {
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
};

export default ExpanderItem;
