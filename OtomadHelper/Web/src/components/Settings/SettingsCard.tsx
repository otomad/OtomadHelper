import { boxShadow } from "components/Button";

const StyledSettingsCard = styled.div`
	border-radius: 3px;
	width: -webkit-fill-available;
	text-align: initial;
	${boxShadow(c("stroke-color-card-stroke-default"))};
	padding: 1px;

	> .base {
		background-color: ${c("background-fill-color-card-background-default")};
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

	button& {
		&:hover,
		&:active {
			${boxShadow(c("stroke-color-control-stroke-default"))};
		}

		&:hover > .base {
			background-color: ${c("fill-color-control-secondary")};
		}

		&:active {
			> .base {
				background-color: ${c("fill-color-control-tertiary")};
			}

			> .base > .icon,
			> .base > .text,
			&.button > .base .trailing-icon {
				opacity: ${c("pressed-text-opacity")}
			}
		}
	}

	&.expander {
		&:hover {
			.trailing-icon {
				background-color: ${c("fill-color-subtle-secondary")};
			}
		}

		&:active {
			.trailing-icon {
				background-color: ${c("fill-color-subtle-tertiary")};
				color: ${c("fill-color-text-secondary")};
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
