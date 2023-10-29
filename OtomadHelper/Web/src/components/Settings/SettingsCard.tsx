const StyledSettingsCard = styled.div`
	padding: 13px 15px;
	display: flex;
	gap: 16px;
	align-items: center;
	border-radius: 3px;
	background-color: ${c("white", 5)};
	margin: 1px;
	width: -webkit-fill-available;
	text-align: initial;

	.icon {
		font-size: 16px;
	}

	.text {
		width: 100%;

		.heading {
			font-size: 14px;
		}

		.caption {
			font-size: 12px;
			color: ${c("white", 79)};
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

	&,
	&:focus {
		outline: 1px solid ${c("black", 10)};
	}

	button& {
		&:hover {
			outline: 1px solid ${c("white", 9)};
			background-color: ${c("white", 8)};
		}

		&:active {
			outline: 1px solid ${c("white", 7)};
			background-color: ${c("white", 3)};

			> * {
				opacity: 0.79;
			}
		}
	}

	&.expander {
		&:hover {
			.trailing-icon {
				background-color: ${c("white", 6)};
			}
		}

		&:active {
			.trailing-icon {
				background-color: ${c("white", 4)};
				color: ${c("white", 79)};
			}
		}
	}
`;

const SettingsCard: FC<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	heading?: ReactNode;
	/** 详细描述。 */
	caption?: ReactNode;
	/** 尾随图标。使用空字符串表示禁用。 */
	trailingIcon?: string;
	/** 组件形态。 */
	type?: "container" | "button" | "expander";
}, HTMLElement> = ({ icon = "placeholder", heading, caption, trailingIcon = "chevron_right", children, type = "container", ...htmlAttrs }) => {
	return (
		<StyledSettingsCard as={type === "container" ? "div" : "button"} className={type} {...htmlAttrs}>
			<Icon name={icon} />
			<div className="text">
				<div className="heading">{heading}</div>
				<div className="caption">{caption}</div>
			</div>
			<div className="trailing">
				{children}
				{trailingIcon && (
					<div className="trailing-icon">
						<Icon name={trailingIcon} />
					</div>
				)}
			</div>
		</StyledSettingsCard>
	);
};

export default SettingsCard;
