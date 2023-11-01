const StyledSettingsCard = styled.div`
	border-radius: 3px;
	width: -webkit-fill-available;
	text-align: initial;

	> .base {
		background-color: ${c("white", 5)};
		padding: 13px 15px;
		display: flex;
		gap: 16px;
		align-items: center;
		border-radius: inherit;

		> :not(.text) {
			flex-shrink: 0;
		}
	}

	.icon {
		font-size: 16px;
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

	&,
	&:focus {
		border: 1px solid ${c("black", 10)};
	}

	${ifColorScheme.light} & {
		> .base {
			background-color: ${c("white", 70)};
		}

		&,
		&:focus {
			border-color: ${c("black", 5.78)};
		}
	}

	button& {
		&:hover {
			border-color: ${c("white", 6.98)};

			> .base {
				background-color: ${c("white", 8.37)};
			}
		}

		&:active {
			border-color: ${c("white", 6.98)};

			> .base {
				background-color: ${c("white", 3)};
			}

			> .base > .icon,
			> .base > .text,
			&.button > .base .trailing-icon {
				opacity: 0.786;

				${ifColorScheme.light} & {
					opacity: 0.6063;
				}
			}
		}

		${ifColorScheme.light} & {
			&:hover {
				border-color: ${c("black", 5.78)};

				> .base {
					background-color: ${c("#f9f9f9", 50)};
				}
			}

			&:active {
				border-color: ${c("black", 5.78)};

				> .base {
					background-color: ${c("#f9f9f9", 30)};
				}
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

export default function SettingsCard({
	icon = "placeholder",
	heading,
	caption,
	trailingIcon = "chevron_right",
	children,
	type = "container",
	className,
	...htmlAttrs
}: FCP<{
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
}, HTMLElement>) {
	return (
		<StyledSettingsCard
			as={type === "container" ? "div" : "button"}
			className={[className, type]}
			{...htmlAttrs}
		>
			<div className="base">
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
			</div>
		</StyledSettingsCard>
	);
}
