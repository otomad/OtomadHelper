export /* internal */ const boxShadow = (color: string) => css`box-shadow: 0 0 0 1px ${color} inset`;

const StyledButton = styled.button`
	display: inline-flex;
	border-radius: 4px;
	--gradient-direction: bottom;
	${styles.mixins.gradientBorder(css`linear-gradient(to var(--gradient-direction), ${c("stroke-color-control-stroke-tertiary")} 0%, transparent 10%)`)};
	${boxShadow(c("stroke-color-control-stroke-default"))};
	padding: 1px; // 修复边框与填充之间在特殊分辨率下的裂缝问题。

	${ifColorScheme.light} & {
		--gradient-direction: top;
	}

	&:focus-visible {
		${boxShadow(c("stroke-color-control-stroke-default"))}, ${styles.effects.focus(true)};
	}

	> .base {
		${styles.mixins.square("100%")};
		${styles.mixins.flexCenter()};
		padding: 4px 11px 6px;
		background-color: ${c("fill-color-control-default")};
		border-radius: 3px;
	}

	@layer components {
		min-width: 96px;
	}

	&:hover > .base {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active {
		&::before {
			opacity: 0;
		}

		> .base {
			background-color: ${c("fill-color-control-tertiary")};

			> .content {
				opacity: ${c("pressed-text-opacity")};
			}
		}
	}

	&[disabled] {
		&::before {
			opacity: 0;
		}

		> .base {
			background-color: ${c("fill-color-control-disabled")};

			> .content {
				opacity: ${c("disabled-text-opacity")};
			}
		}
	}

	> .base > .content {
		${styles.mixins.flexCenter()};
		gap: 8px;

		> span:empty {
			display: none;
		}
	}

	&.subtle {
		box-shadow: none;
		padding: 0;

		&:focus-visible {
			${styles.effects.focus()};
		}

		.base {
			background-color: ${c("fill-color-subtle-transparent")};
		}

		&::before {
			display: none;
		}

		&:hover > .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active > .base {
			background-color: ${c("fill-color-subtle-tertiary")};
		}
	}
`;

export default function Button({ children, icon, subtle, className, ...htmlAttrs }: FCP<{
	/** 按钮图标。 */
	icon?: string;
	/** 是否使用无背景按钮？ */
	subtle?: boolean;
}, HTMLButtonElement>) {
	return (
		<StyledButton type="button" className={[className, { subtle }]} {...htmlAttrs}>
			<div className="base">
				<div className="content">
					{icon && <Icon name={icon} />}
					<span>{children}</span>
				</div>
			</div>
		</StyledButton>
	);
}
