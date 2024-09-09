import { StyledCard } from "components/Card";
import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

const isPressed = (ampersand = "&") => `${ampersand}:not(:has(button:active)):active, .sortable-overlay:not(.dropping) > ${ampersand}`;

const StyledSettingsCard = styled(StyledCard)`
	${styledExpanderItemContent};

	> .base {
		${styledExpanderItemBase};
		position: relative;

		.select-info {
			color: ${c("accent-color")};

			&.invalid {
				color: ${c("fill-color-system-critical")};
			}
		}

		> .trailing {
			> button.button,
			> .contents > button.button {
				align-self: stretch;
			}
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
		${isPressed()} {
			border-color: ${c("stroke-color-control-stroke-default")};
		}

		&:hover > .base {
			background-color: ${c("fill-color-control-secondary")};
		}

		${isPressed("&:not(:has(.trailing .toggle-switch-base))")},
		${isPressed("&:has(.trailing .toggle-switch-base:not(:active, .pressing, .pressed))")} {
			> .base {
				background-color: ${c("fill-color-control-tertiary")};
			}

			> .base > .icon,
			> .base > .text,
			> .base > .trailing > .check-info,
			&.button > .base > .trailing > .trailing-icon {
				opacity: ${c("pressed-text-opacity")};
			}
		}
	}

	&.expander {
		&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):hover)):hover {
			.trailing-icon {
				background-color: ${c("fill-color-subtle-secondary")};
			}
		}

		${isPressed("&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active))")} {
			.trailing-icon {
				color: ${c("fill-color-text-secondary")};
				background-color: ${c("fill-color-subtle-tertiary")};
			}
		}
	}

	&:dir(rtl) .trailing-icon {
		scale: -1 1;
	}

	.drag-handle-shadow {
		${styles.mixins.gridCenter()};
		position: absolute;
		inset-block-start: 0;
		inset-inline-start: 0;
		z-index: 1;
		block-size: 100%;
		inline-size: ${15 + 20 + 16}px;
		border-radius: 3px;
		cursor: ${SortableList.Item.dragHandleCursor};

		&::after {
			${styles.mixins.square("36px")};
			content: "";
			display: block;
			background-color: ${c("fill-color-subtle-secondary")};
			border-radius: 4px;
			opacity: 0;
		}

		&:hover:not(:active)::after {
			opacity: 1;

			.sortable-overlay & {
				opacity: 0;
			}
		}

		&:focus-visible {
			${styles.effects.focus(true)};
		}
	}
`;

export default function SettingsCard({ icon = "placeholder", title, details, selectInfo, selectValid = true, trailingIcon, disabled, children, type = "container", dragHandle, className, ...htmlAttrs }: FCP<{
	/** Icon. Use an empty string or Boolean type to indicate disabling. */
	icon?: DeclaredIcons | "" | boolean | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Specifies the display string of the selection of tracks or track events. */
	selectInfo?: ReactNode;
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. */
	selectValid?: boolean | number;
	/** Trailing icon. Use an empty string or Boolean type to indicate disabling. */
	trailingIcon?: DeclaredIcons | "" | boolean;
	/** Component form type. */
	type?: "container" | "button" | "expander";
	/** Show the drag handle to represent that it is sortable? */
	dragHandle?: boolean;
}, "div">) {
	trailingIcon ??= type === "button" ? "chevron_right" :
		type === "expander" ? "chevron_down" : undefined;

	const dragHandleContext = useContext(SortableList.Item.Context);

	return (
		<StyledSettingsCard
			as={type === "container" ? "div" : "button"}
			className={[className, type]}
			disabled={disabled}
			aria-disabled={disabled || undefined}
			{...htmlAttrs}
		>
			<div className="base">
				{dragHandle && (
					<>
						<div className="drag-handle-shadow" ref={dragHandleContext.ref} {...dragHandleContext.attributes} {...dragHandleContext.listeners} />
						<Icon name="reorder_dots" className="drag-handle-icon" />
					</>
				)}
				{typeof icon === "object" ? icon : <Icon name={icon} />}
				<div className="text">
					<p className="title"><Preserves>{title}</Preserves></p>
					<p className="details"><Preserves>{details}</Preserves></p>
					<p className={["details", "select-info", { invalid: !selectValid }]}><Preserves>{selectInfo}</Preserves></p>
				</div>
				<div className="trailing">
					{children}
					{trailingIcon && typeof trailingIcon === "string" && (
						<div className={["trailing-icon", TRAILING_EXEMPTION]} data-type={type}>
							<Icon name={trailingIcon} />
						</div>
					)}
				</div>
			</div>
		</StyledSettingsCard>
	);
}
