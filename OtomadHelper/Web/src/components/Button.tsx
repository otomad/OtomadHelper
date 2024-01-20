const StyledButton = styled.button<{
	/** 背景填充颜色名称。 */
	$fillColorName?: string;
}>`
	${styles.mixins.flexCenter()};
	display: inline-flex;
	border-radius: 4px;
	border: 1px solid;
	padding: 4px 11px 6px;
	border-radius: 3px;
	min-height: 30px;

	&:hover {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active {
		background-color: ${c("fill-color-control-tertiary")};

		> .content {
			opacity: ${c("pressed-text-opacity")};
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

		${ifColorScheme.dark} &:not(:active, [disabled]) {?
			border-top-color: ${c("stroke-color-control-stroke-secondary")};
		}

		${ifColorScheme.light} &:not(:active, [disabled]) {
			border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
		}
	` : css`
		--fill-color: ${c($fillColorName)};
		background-color: ${c($fillColorName)};
		border-color: ${c("stroke-color-control-stroke-on-accent-default")};
		color: ${c("fill-color-text-on-accent-primary")};

		${ifColorScheme.dark} &:not(:active, [disabled]) {
			border-top-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		${ifColorScheme.light} &:not(:active, [disabled]) {
			border-bottom-color: ${c("stroke-color-control-stroke-on-accent-secondary")};
		}

		&:hover {
			background-color: rgb(from ${c("fill-color")} r g b / 90%);
		}

		&:active {
			background-color: rgb(from ${c("fill-color")} r g b / 80%);
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

	> .content > span:empty {
		display: none;
	}

	&.subtle {
		border: none;
		padding: 0;
		background-color: ${c("fill-color-subtle-transparent")};

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
`;

export default function Button({ children, icon, subtle, accent, className, ...htmlAttrs }: FCP<{
	/** 按钮图标。 */
	icon?: string;
	/** 是否使用无背景按钮？ */
	subtle?: boolean;
	/** 是否按钮附着强调色？ */
	accent?: boolean | "critical" | "success" | "attention" | "caution";

}, HTMLButtonElement>) {
	const fillColorName = !accent ? undefined : accent === true ? "accent-color" : `fill-color-system-${accent}`;

	return (
		<StyledButton type="button" className={[className, { subtle }]} $fillColorName={fillColorName} {...htmlAttrs}>
			<StackPanel className="content">
				{icon && <Icon name={icon} />}
				<span>{children}</span>
			</StackPanel>
		</StyledButton>
	);
}
