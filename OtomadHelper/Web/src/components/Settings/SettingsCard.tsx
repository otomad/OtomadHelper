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

	&,
	&:focus {
		outline: 1px solid ${c("black", 10)};
	}

	${ifColorScheme.light} & {
		background-color: ${c("white", 70)};

		&,
		&:focus {
			outline: 1px solid ${c("black", 5.78)};
		}
	}

	button& {
		&:hover {
			outline-color: ${c("white", 6.98)};
			background-color: ${c("white", 8.37)};
		}

		&:active {
			outline-color: ${c("white", 6.98)};
			background-color: ${c("white", 3)};

			> .icon,
			> .text,
			&.button .trailing-icon {
				opacity: 0.786;

				${ifColorScheme.light} & {
					opacity: 0.6063;
				}
			}
		}

		${ifColorScheme.light} & {
			&:hover {
				outline-color: ${c("black", 5.78)};
				background-color: ${c("#f9f9f9", 50)};
			}

			&:active {
				outline-color: ${c("black", 5.78)};
				background-color: ${c("#f9f9f9", 30)};
			}
		}
	}

	&.expander {
		&:hover {
			.trailing-icon {
				background-color: ${c("white", 6.05)};
			}
		}

		&:active {
			.trailing-icon {
				background-color: ${c("white", 4.19)};
				color: ${c("white", 78.6)};
			}
		}

		${ifColorScheme.light} & {
			&:hover {
				.trailing-icon {
					background-color: ${c("black", 3.73)};
				}
			}

			&:active {
				.trailing-icon {
					background-color: ${c("black", 2.41)};
					color: ${c("black", 60.63)};
				}
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
}, HTMLElement> = ({ icon = "placeholder", heading, caption, trailingIcon = "chevron_right", children, type = "container", className, ...htmlAttrs }) => {
	return (
		<StyledSettingsCard
			as={type === "container" ? "div" : "button"}
			className={classNames([className, type])}
			{...htmlAttrs}
		>
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
