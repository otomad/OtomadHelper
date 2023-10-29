const StyledButton = styled.button.attrs({
	type: "button",
}) <{
	/** 是否使用无背景按钮？ */
	$lite?: boolean;
}>`
	${styles.mixins.flexCenter()};
	padding: 4px 11px 6px;
	border-radius: 4px;
	background-color: ${c("white", 6)};
	margin: 1px;

	@layer components {
		min-width: 96px;
	}

	&:hover {
		background-color: ${c("white", 8)};
	}

	&:active {
		outline: 1px solid ${c("white", 7)};
		background-color: ${c("white", 3)};

		> .base {
			opacity: 0.79;
		}
	}

	&,
	&:focus {
		outline: 1px solid ${c("white", 9)};
	}

	&[disabled] {
		outline: 1px solid ${c("white", 7)};
		background-color: ${c("white", 4)};

		> .base {
			opacity: 0.36;
		}
	}

	> .base {
		${styles.mixins.flexCenter()};
		gap: 8px;

		> span:empty {
			display: none;
		}
	}

	${({ $lite }) => $lite && css`
		background-color: transparent;
		outline: none !important;
	`}
`;

const Button: FC<{
	/** 按钮图标。 */
	icon?: string;
	/** 是否使用无背景按钮？ */
	lite?: boolean;
}, HTMLButtonElement> = ({ children, icon, lite, ...htmlAttrs }) => {
	return (
		<StyledButton $lite={lite} {...htmlAttrs}>
			<div className="base">
				{icon && <Icon name={icon} />}
				<span>{children}</span>
			</div>
		</StyledButton>
	);
};

export default Button;
