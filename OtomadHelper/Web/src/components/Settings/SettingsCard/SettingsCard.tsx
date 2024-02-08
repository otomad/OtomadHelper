import { StyledCard } from "components/Card";
import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

const StyledSettingsCard = styled(StyledCard)`
	${styledExpanderItemContent};

	> .base {
		${styledExpanderItemBase};

		.select-info {
			color: ${c("accent-color")};
		}
	}

	button& {
		&:hover,
		&:active {
			border-color: ${c("stroke-color-control-stroke-default")};
		}

		&:hover > .base {
			background-color: ${c("fill-color-control-secondary")};
		}

		&:not(:has(.trailing .toggle-switch-base)):active,
		&:has(.trailing .toggle-switch-base:not(:active, .pressing, .pressed)):active {
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
	selectInfo,
	trailingIcon,
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
	/** 指定轨道或轨道事件的选择情况。 */
	selectInfo?: ReactNode;
	/** 尾随图标。使用空字符串或布尔类型表示禁用。 */
	trailingIcon?: string | boolean;
	/** 组件形态。 */
	type?: "container" | "button" | "expander";
}, "div">) {
	trailingIcon ??= type === "button" ? "chevron_right" :
		type === "expander" ? "chevron_down" : undefined;

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
					<div className="caption select-info">{selectInfo}</div>
				</div>
				<div className="trailing">
					{children}
					{trailingIcon && typeof trailingIcon === "string" && (
						<div className="trailing-icon">
							<Icon name={trailingIcon} />
						</div>
					)}
				</div>
			</div>
		</StyledSettingsCard>
	);
}
