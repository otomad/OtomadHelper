import { StyledCard } from "components/Card";
import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

export const TRAILING_EXEMPTION = "trailing-exemption";

const StyledSettingsCard = styled(StyledCard)`
	${styledExpanderItemContent};

	> .base {
		${styledExpanderItemBase};

		.select-info {
			color: ${c("accent-color")};
		}
	}

	&[disabled] > .base {
		background-color: ${c("fill-color-control-disabled")};

		> * {
			opacity: ${c("disabled-text-opacity")};
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
		&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):hover)):hover {
			.trailing-icon {
				background-color: ${c("fill-color-subtle-secondary")};
			}
		}

		&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active)):active {
			.trailing-icon {
				color: ${c("fill-color-text-secondary")};
				background-color: ${c("fill-color-subtle-tertiary")};
			}
		}
	}

	&:dir(rtl) .trailing-icon {
		scale: -1 1;
	}
`;

export default function SettingsCard({
	icon = "placeholder",
	title,
	details,
	selectInfo,
	trailingIcon,
	children,
	type = "container",
	className,
	...htmlAttrs
}: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Specifies the selection of tracks or track events. */
	selectInfo?: ReactNode;
	/** Trailing icon. Use an empty string or Boolean type to indicate disabling. */
	trailingIcon?: DeclaredIcons | "" | boolean;
	/** Component form type. */
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
					<p className="title">{title}</p>
					<p className="details">{details}</p>
					<p className="details select-info">{selectInfo}</p>
				</div>
				<div className="trailing">
					{children}
					{trailingIcon && typeof trailingIcon === "string" && (
						<div className={["trailing-icon", TRAILING_EXEMPTION]}>
							<Icon name={trailingIcon} />
						</div>
					)}
				</div>
			</div>
		</StyledSettingsCard>
	);
}
