const StyledButton = styled.button<{
	/** 背景填充颜色名称。 */
	$fillColorName?: string;
	/** 是否根据书写方向来改变图标的朝向？ */
	$dirBased?: boolean;
}>`
	${styles.mixins.flexCenter()};
	display: inline-flex;
	min-height: 30px;
	padding: 4px 11px 6px;
	border: 1px solid;
	border-radius: 4px;

	&:hover {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active {
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

	${({ $fillColorName }) => !$fillColorName ? css`
		background-color: ${c("fill-color-control-default")};
		border-color: ${c("stroke-color-control-stroke-default")};

		${ifColorScheme.dark} &:not(:active, [disabled]) {
			border-top-color: ${c("stroke-color-control-stroke-secondary")};
		}

		${ifColorScheme.light} &:not(:active, [disabled]) {
			border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
		}
	` : css`
		--fill-color: ${c($fillColorName)};
		background-color: ${c($fillColorName)};
		border-color: ${c("stroke-color-control-stroke-on-accent-default")};

		* {
			color: ${c("fill-color-text-on-accent-primary")};
		}

		${ifColorScheme.dark} &:not(:active, [disabled]) {
			border-top-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		${ifColorScheme.light} &:not(:active, [disabled]) {
			border-bottom-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		&:hover {
			background-color: ${c("fill-color", 90)};
		}

		&:active {
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
	`}

	@layer components {
		min-width: 96px;
	}

	> .content > span {
		${styles.mixins.hideIfEmpty()};
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

		&:active {
			background-color: ${c("fill-color-subtle-tertiary")};
		}
	}

	&.hyperlink {
		color: ${c("accent-color")};
		cursor: pointer;
	}

	${({ $dirBased }) => $dirBased && css`
		&:dir(rtl) {
			.icon,
			.animated-icon .icon-box {
				scale: -1 1;
			}
		}
	`}
`;

export default forwardRef(function Button({ children, icon, animatedIcon, subtle, hyperlink, accent, dirBased, className, ...htmlAttrs }: FCP<{
	/** 按钮图标。 */
	icon?: DeclaredIcons;
	/** 按钮动态图标。 */
	animatedIcon?: DeclaredLotties;
	/** 是否使用无背景按钮？ */
	subtle?: boolean;
	/** 是否使用超链接按钮？ */
	hyperlink?: boolean;
	/** 是否按钮附着强调色？ */
	accent?: boolean | "critical" | "success" | "attention" | "caution";
	/** 是否根据书写方向来改变图标的朝向？ */
	dirBased?: boolean;
}, "button">, ref: ForwardedRef<"button">) {
	const fillColorName = !accent ? undefined : accent === true ? "accent-color" : `fill-color-system-${accent}`;

	return (
		<StyledButton
			ref={ref}
			type="button"
			className={[className, { subtle, hyperlink }]}
			$fillColorName={fillColorName}
			$dirBased={dirBased}
			{...htmlAttrs}
		>
			<StackPanel className="content">
				{icon && <Icon name={icon} />}
				{animatedIcon && <AnimatedIcon name={animatedIcon} />}
				<span>{children}</span>
			</StackPanel>
		</StyledButton>
	);
});
