const boxShadow = (color: string) => css`box-shadow: 0 0 0 1px ${color} inset`;

const StyledButton = styled.button.attrs({
	type: "button",
})`
	display: inline-flex;
	border-radius: 4px;
	${styles.mixins.gradientBorder(css`linear-gradient(to bottom, ${c("white", 3)} 0%, transparent 10%)`)};
	${boxShadow(c("white", 6.98))};
	padding: 0.5px; // 修复边框与填充之间在特殊分辨率下的裂缝问题。

	&:focus-visible {
		${boxShadow(c("white", 6.98))}, ${styles.effects.focus(true)};
	}

	> .base {
		${styles.mixins.square("100%")};
		${styles.mixins.flexCenter()};
		padding: 4px 11px 6px;
		background-color: ${c("white", 6.05)};
		border-radius: 3px;
	}

	@layer components {
		min-width: 96px;
	}

	&:hover > .base {
		background-color: ${c("white", 8.37)};
	}

	&:active {
		&::before {
			opacity: 0;
		}

		> .base {
			background-color: ${c("white", 3.26)};

			> .content {
				opacity: 0.786;
			}
		}
	}

	&[disabled] {
		&::before {
			opacity: 0;
		}

		> .base {
			background-color: ${c("white", 4.19)};

			> .content {
				opacity: 0.3628;
			}
		}
	}

	${ifColorScheme.light} & {
		${boxShadow(c("black", 5.78))};

		&:focus-visible {
			${boxShadow(c("black", 5.78))}, ${styles.effects.focus(true)};
		}

		&::before {
			background: linear-gradient(to bottom, transparent 90%, ${c("black", 13)} 100%) border-box;
		}

		> .base {
			background-color: ${c("white", 70)};
		}

		&:hover > .base {
			background-color: ${c("#f9f9f9", 50)};
		}

		&:active,
		&[disabled] {
			> .base {
				background-color: ${c("#f9f9f9", 30)};

				> .content {
					opacity: 0.6063;
				}
			}
		}

		&[disabled] > .base > .content {
			opacity: 0.3614;
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
			background-color: transparent;
		}

		&::before {
			display: none;
		}

		&:hover > .base {
			background-color: ${c("white", 6.05)};
		}

		&:active > .base {
			background-color: ${c("white", 4.19)};
		}

		${ifColorScheme.light} & {
			&:hover > .base {
				background-color: ${c("black", 3.73)};
			}

			&:active > .base {
				background-color: ${c("black", 2.41)};
			}
		}
	}
`;

const Button: FC<{
	/** 按钮图标。 */
	icon?: string;
	/** 是否使用无背景按钮？ */
	subtle?: boolean;
}, HTMLButtonElement> = ({ children, icon, subtle, className, ...htmlAttrs }) => {
	return (
		<StyledButton className={[className, { subtle }]} {...htmlAttrs}>
			<div className="base">
				<div className="content">
					{icon && <Icon name={icon} />}
					<span>{children}</span>
				</div>
			</div>
		</StyledButton>
	);
};

export default Button;
