import { fillColorAccentOpacity } from "styles/colors";

const isPressed = ":is(:active, [data-pressed]):not(:has(button:active))", notPressedOrDisabled = ":not(:active, [data-pressed], [disabled])";
const inlinePadding = 11;

export const styledDirBasedIcon = ($dirBasedIcon?: DirBasedIcon | { $dirBasedIcon?: DirBasedIcon }) => {
	if (isObject($dirBasedIcon) && "$dirBasedIcon" in $dirBasedIcon) $dirBasedIcon = $dirBasedIcon.$dirBasedIcon;
	return $dirBasedIcon === true ? css`
		&:dir(rtl) {
			.icon,
			.animated-icon .icon-box {
				transform: scaleX(-1);
			}
		}
	` : typeof $dirBasedIcon === "string" || Array.isArray($dirBasedIcon) ? css`
		.icon,
		.animated-icon .icon-box {
			transform: ${(typeof $dirBasedIcon === "string" ? transformFlowDirection($dirBasedIcon) : transformFlowDirection(...$dirBasedIcon)) || "none"};
		}
	` : undefined;
};

export /* @internal */ const StyledButton = styled.button<{
	/** The name of the background fill color. */
	$fillColorName?: string;
	/** The name of the background fill color when on subtle mode. */
	$subtleFillColorName?: string;
	/** Is the orientation of the icon changed based on the writing direction? */
	$dirBasedIcon?: DirBasedIcon;
}>`
	${styles.mixins.flexCenter()};
	/* --border-outline-color: ${c("stroke-color-control-stroke-default")};
	--border-highlight-color: ${c("stroke-color-control-stroke-secondary-on-default")};
	--border-accent-color: transparent;
	--border-highlight-y-offset: 0; */
	position: relative;
	display: inline-flex;
	min-block-size: 32px;
	padding-block: 4px 6px;
	padding-inline: ${inlinePadding}px;
	background-clip: padding-box;
	border: 1px solid ${c("stroke-color-control-stroke-default")};
	border-radius: 4px;
	/* box-shadow:
		0 var(--border-highlight-y-offset) 0 0 var(--border-highlight-color),
		0 0 0 1px var(--border-outline-color),
		0 0 0 1px var(--border-accent-color) !important; */

	&:hover {
		background-color: ${c("fill-color-control-secondary")};
	}

	&${isPressed} {
		background-color: ${c("fill-color-control-tertiary")};

		> .content {
			opacity: ${c("pressed-text-opacity")};

			.animated-icon {
				--state: pressed;
			}
		}
	}

	&[disabled] {
		background-color: ${c("fill-color-control-disabled")};

		> .content {
			color: ${c("foreground-color")};
			opacity: ${c("disabled-text-opacity")};
		}
	}

	&.subtle,
	&.hyperlink {
		background-color: ${c("fill-color-subtle-transparent")};
		background-clip: border-box;
		border-color: transparent !important;

		&::before {
			display: none;
		}

		&:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&${isPressed} {
			background-color: ${c("fill-color-subtle-tertiary")};
		}
	}

	&.subtle {
		&:not(.small-icon) .icon:has(+ :empty) {
			font-size: 20px;
		}

		&:has(+ .action-icon[data-type="button"]),
		.contents:has(+ .action-icon[data-type="button"]) > & {
			margin-inline-end: -8px;
		}
	}

	&.hyperlink {
		color: ${c("accent-color")};
		cursor: pointer;
	}

	&.extruded {
		margin-inline: ${-inlinePadding}px;
	}

	${styledDirBasedIcon}

	${({ $fillColorName, $subtleFillColorName }) => !$fillColorName ? css`
		background-color: ${c("fill-color-control-default")};

		&${notPressedOrDisabled} {
			// Do not use CSS logical properties here, or will be ugly in vertical locales.
			${ifColorScheme.at.light} {
				border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
			}

			${ifColorScheme.at.dark} {
				border-top-color: ${c("stroke-color-control-stroke-secondary")};
			}
		}
	` : css`
		--fill-color: ${c($fillColorName)};
		/* --border-outline-color: ${c("stroke-color-control-stroke-on-accent-default")};
		--border-highlight-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		--border-accent-color: ${c($fillColorName)}; */
		background-color: ${c($fillColorName)};
		background-clip: border-box;
		border-color: ${c("stroke-color-control-stroke-on-accent-default")};

		&${notPressedOrDisabled} {
			border-bottom-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		> .content {
			color: ${c("fill-color-text-on-accent-primary")};
		}

		.icon svg,
		.icon svg * {
			color: inherit;
		}

		&:hover {
			background-color: ${c("fill-color", fillColorAccentOpacity.secondary)};
		}

		&${isPressed} {
			background-color: ${c("fill-color", fillColorAccentOpacity.tertiary)};
		}

		&[disabled] {
			background-color: ${c("fill-color-accent-disabled")};
			border-color: transparent !important;

			> .content {
				color: ${c("fill-color-text-on-accent-disabled")};
			}

			${ifColorScheme.at.light} {
				> .content {
					opacity: 1;
				}
			}

			${ifColorScheme.at.dark} {
				> .content {
					color: ${c("foreground-color")} !important;
				}
			}
		}

		&.subtle {
			--fill-color: ${c($subtleFillColorName!)};

			* {
				color: ${c($fillColorName)};
			}

			&:hover {
				background-color: ${c("fill-color", 75)};
			}

			&${isPressed} {
				background-color: ${c("fill-color", 65)};
			}

			&.neutral {
				&:hover,
				&${isPressed} {
					background-color: ${c("fill-color")};
				}
			}
		}

		${ifColorScheme.at.contrast} {
			background-color: ${cc("ActiveText")};
		}
	`}

	${ifColorScheme.at.contrast} {
		color: ${cc("ButtonText")};
		forced-color-adjust: none;

		&:hover,
		&${isPressed} {
			color: ${cc("HighlightText")};
			background-color: ${cc("Highlight")} !important;
			border-color: ${cc("Highlight")};
		}

		&${isPressed} {
			border-color: ${cc("ButtonFace")};
		}
	}

	@layer components {
		&:not(.min-width-unbounded) {
			min-inline-size: 96px;
		}
	}

	> .content > span {
		${styles.mixins.hideIfEmpty()};
		transition: none; // If enable transition, in Audio page, when toggle No Tuning in tuning method, the prelisten buttons text will be flickering.


		@layer components {
			&:is(:lang(en), :lang(vi), :lang(id)) {
				margin-block-start: -1px; // Solve bad font letter sinking.
			}
		}
	}
`;

export default function Button({ children, icon, animatedIcon, subtle, hyperlink, accent, dirBasedIcon, repeat, extruded, minWidthUnbounded, ariaHiddenForChildren, href, blank = true, meta, className, disabled, onRelease, onClick, ref, ...htmlAttrs }: FCP<{
	/** Button icon. */
	icon?: DeclaredIcons;
	/** Button animated icon. */
	animatedIcon?: DeclaredLotties;
	/**
	 * Use background-less button?
	 * - `true`: Besides background-less, when the button has only an icon and no text, the icon size will be increased additionally.
	 * - `"small-icon"`: Disable the additional effects of `true`.
	 */
	subtle?: boolean | "small-icon";
	/** Use hyperlink button? */
	hyperlink?: boolean;
	/** Attach accent color to the button? */
	accent?: boolean | "critical" | "success" | "attention" | "caution" | "neutral";
	/** Is the orientation of the icon changed based on the writing direction? */
	dirBasedIcon?: DirBasedIcon;
	/** Is repeat button? When on, the `onClick` events will be triggered continuously when the button is pressed. */
	repeat?: boolean;
	/** Extrude the inline paddings. */
	extruded?: boolean;
	/** No min width? */
	minWidthUnbounded?: boolean;
	/** Add aria-hidden to children to prevent them from being read by screen readers. */
	ariaHiddenForChildren?: boolean;
	/** Click the button to open a link. */
	href?: string;
	/** Open in new tab? Only available when `href` is provided. */
	blank?: boolean;
	/** Auto fill props from a setting meta. */
	meta?: PropsOf<typeof Setting>["meta"];
	/** Mouse release button event. Only works with `RepeatButton`. */
	onRelease?: BaseEventHandler;
}, "button">) {
	const fillColorName = !accent ? undefined : accent === true ? "accent-color" : `fill-color-system-${accent}`;
	const subtleFillColorName = `fill-color-system-${accent === true ? "accent" : accent}-background`;
	const handleClick = useOnNestedButtonClick(onClick);
	// eslint-disable-next-line no-var
	var { children, anchor, icon } = Setting.useMeta(meta, arguments);

	return (
		<StyledButton
			as={href ? "a" : repeat ? RepeatButton : "button"}
			ref={ref}
			type={href ? undefined : "button"}
			role={hyperlink || href ? "link" : undefined}
			href={href}
			target={href && blank ? "_blank" : undefined}
			disabled={disabled}
			aria-disabled={disabled}
			className={[
				className,
				{
					subtle,
					smallIcon: subtle === "small-icon",
					hyperlink,
					extruded,
					minWidthUnbounded,
				},
				accent && typeof accent === "string" ? accent : "accent",
			]}
			$fillColorName={fillColorName}
			$subtleFillColorName={subtleFillColorName}
			$dirBasedIcon={dirBasedIcon}
			data-anchor={anchor}
			onRelease={repeat ? onRelease : undefined}
			onClick={handleClick}
			{...htmlAttrs}
		>
			<StackPanel className="content" $nowrap>
				{icon && <Icon name={icon} />}
				{animatedIcon && <AnimatedIcon name={animatedIcon} />}
				<span aria-hidden={ariaHiddenForChildren || undefined}>{children}</span>
			</StackPanel>
		</StyledButton>
	);
}
