const isPressed = ":is(:active, [data-pressed]):not(:has(button:active))", notPressedOrDisabled = ":not(:active, [data-pressed], [disabled])";
const inlinePadding = 11;

export /* @internal */ const StyledButton = styled.button<{
	/** The name of the background fill color. */
	$fillColorName?: string;
	/** The name of the background fill color when on subtle mode. */
	$subtleFillColorName?: string;
	/** Is the orientation of the icon changed based on the writing direction? */
	$dirBased?: boolean;
}>`
	${styles.mixins.flexCenter()};
	display: inline-flex;
	min-height: 32px;
	padding: 4px ${inlinePadding}px 6px;
	border: 1px solid;
	border-radius: 4px;

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
			opacity: ${c("disabled-text-opacity")};
		}
	}

	&.subtle,
	&.hyperlink {
		padding: 0 11px;
		background-color: ${c("fill-color-subtle-transparent")};
		border: none;

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
		.icon:has(+ :empty) {
			font-size: 20px;
		}

		&:has(+ .trailing-icon[data-type="button"]),
		.contents:has(+ .trailing-icon[data-type="button"]) > & {
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

	${({ $dirBased }) => $dirBased && css`
		&:dir(rtl) {
			.icon,
			.animated-icon .icon-box {
				scale: -1 1;
			}
		}
	`}

	${({ $fillColorName, $subtleFillColorName }) => !$fillColorName ? css`
		background-color: ${c("fill-color-control-default")};
		border-color: ${c("stroke-color-control-stroke-default")};

		${ifColorScheme.dark} &${notPressedOrDisabled} {
			border-top-color: ${c("stroke-color-control-stroke-secondary")};
		}

		${ifColorScheme.light} &${notPressedOrDisabled} {
			border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
		}
	` : css`
		--fill-color: ${c($fillColorName)};
		background-color: ${c($fillColorName)};
		border-color: ${c("stroke-color-control-stroke-on-accent-default")};

		* {
			color: ${c("fill-color-text-on-accent-primary")};
		}

		${ifColorScheme.dark} &${notPressedOrDisabled} {
			border-top-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		${ifColorScheme.light} &${notPressedOrDisabled} {
			border-bottom-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		&:hover {
			background-color: ${c("fill-color", 90)};
		}

		&${isPressed} {
			background-color: ${c("fill-color", 80)};
		}

		&[disabled] {
			background-color: ${c("fill-color-accent-disabled")};

			${ifColorScheme.light} & > .content {
				opacity: 1;
			}

			${ifColorScheme.dark} & {
				color: ${c("foreground-color")};
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
	`}

	@layer components {
		&:not(.min-width-unbounded) {
			min-inline-size: 96px;
		}
	}

	> .content > span {
		${styles.mixins.hideIfEmpty()};
	}
`;

export default forwardRef(function Button({ children, icon, animatedIcon, subtle, hyperlink, accent, dirBased, repeat, extruded, minWidthUnbounded, className, onRelease, onClick, ...htmlAttrs }: FCP<{
	/** Button icon. */
	icon?: DeclaredIcons;
	/** Button animated icon. */
	animatedIcon?: DeclaredLotties;
	/** Use background-less button? */
	subtle?: boolean;
	/** Use hyperlink button? */
	hyperlink?: boolean;
	/** Attach accent color to the button? */
	accent?: boolean | "critical" | "success" | "attention" | "caution" | "neutral";
	/** Is the orientation of the icon changed based on the writing direction? */
	dirBased?: boolean;
	/** Is repeat button? When on, the `onClick` events will be triggered continuously when the button is pressed. */
	repeat?: boolean;
	/** Extrude the inline paddings. */
	extruded?: boolean;
	/** No min width? */
	minWidthUnbounded?: boolean;
	/** Mouse release button event. Only works with `RepeatButton`. */
	onRelease?: BaseEventHandler;
}, "button">, ref: ForwardedRef<"button">) {
	const fillColorName = !accent ? undefined : accent === true ? "accent-color" : `fill-color-system-${accent}`;
	const subtleFillColorName = `fill-color-system-${accent}-background`;
	const handleClick = useOnNestedButtonClick(onClick);

	return (
		<StyledButton
			as={repeat ? RepeatButton : "button"}
			ref={ref}
			type="button"
			className={[className, { subtle, hyperlink, extruded, minWidthUnbounded }, accent && typeof accent === "string" ? accent : "accent"]}
			$fillColorName={fillColorName}
			$subtleFillColorName={subtleFillColorName}
			$dirBased={dirBased}
			onRelease={repeat ? onRelease : undefined}
			onClick={handleClick}
			{...htmlAttrs}
		>
			<StackPanel className="content" $nowrap>
				{icon && <Icon name={icon} />}
				{animatedIcon && <AnimatedIcon name={animatedIcon} />}
				<span>{children}</span>
			</StackPanel>
		</StyledButton>
	);
});
