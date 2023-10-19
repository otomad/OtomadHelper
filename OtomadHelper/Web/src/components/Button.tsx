const StyledButton = styled.button`
	${styles.mixins.flexCenter};
	padding: 4px 11px 6px;
	border-radius: 4px;
	border: 1px solid ${c("white", 9)};
	background-color: ${c("white", 6)};

	@layer components {
		& {
			min-width: 96px;
		}
	}

	&:hover {
		background-color: ${c("white", 8)};
	}

	&:active {
		border: 1px solid ${c("white", 7)};
		background-color: ${c("white", 3)};

		> span {
			opacity: 0.79;
		}
	}

	&[disabled] {
		border: 1px solid ${c("white", 7)};
		background-color: ${c("white", 4)};

		> span {
			opacity: 0.36;
		}
	}

	&:focus-visible {
		box-shadow: 0 0 0 3px white;
	}
`;

const Button: FC = props => {
	const { children, ...htmlAttrs } = props;

	return (
		<StyledButton {...htmlAttrs}>
			<span>{children}</span>
		</StyledButton>
	);
};

export default Button;
