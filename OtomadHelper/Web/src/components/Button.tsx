const StyledButton = styled.button.attrs({
	type: "button",
}) <{
	/** 是否使用无背景按钮？ */
	$lite?: boolean;
}>`
	${styles.mixins.flexCenter()};
	padding: 4px 11px 6px;
	border-radius: 4px;
	border: 1px solid ${c("white", 9)};
	background-color: ${c("white", 6)};

	@layer components {
		min-width: 96px;
	}

	&:hover {
		background-color: ${c("white", 8)};
	}

	&:active {
		border: 1px solid ${c("white", 7)};
		background-color: ${c("white", 3)};

		> .base {
			opacity: 0.79;
		}
	}

	&[disabled] {
		border: 1px solid ${c("white", 7)};
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
`;

const Button: FC<{
	/** 按钮图标。 */
	icon?: string;
}, HTMLButtonElement> = ({ children, icon, ...htmlAttrs }) => {
	return (
		<StyledButton {...htmlAttrs}>
			<div className="base">
				{icon && <Icon name={icon} />}
				<span>{children}</span>
			</div>
		</StyledButton>
	);
};

export default Button;
