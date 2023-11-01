const StyledRadioButtonLabel = styled.label`
	display: flex;
	align-items: center;
	gap: 8px;

	input {
		display: none;
	}

	.text {
		padding-bottom: 1px;
	}

	.bullet {
		${styles.mixins.square("0", true)};
		${styles.mixins.circle()};
		background-color: ${c("black")};
		outline: 0 solid ${c("white", 6.98)};
	}

	.radio {
		${styles.mixins.square("18px")};
		${styles.mixins.circle()};
		${styles.mixins.gridCenter()};
		background-color: ${c("black", 10)};
		outline: 1px solid ${c("white", 60.47)};
	}

	&:hover .radio {
		background-color: ${c("white", 4.19)};
	}

	&:active .radio {
		background-color: ${c("white", 6.98)};
		outline-color: ${c("white", 15.81)};

		.bullet {
			--size: 10px;
		}
	}

	input[disabled] ~ {
		.radio {
			background-color: transparent;
			outline-color: ${c("white", 15.81)};
		}

		.text {
			opacity: 0.3628;
		}
	}
`;

export default function RadioButton<T>({ children, id, value: [curValue, setValue] }: FCP<{
	/** 标识符。 */
	id: T;
	/** 当前单选框组中选中的值。 */
	value: StateProperty<T>;
}>) {
	return (
		<StyledRadioButtonLabel>
			<input type="radio" checked={curValue === id} onChange={e => e.target.checked && setValue?.(id)} />
			<div className="radio">
				<div className="bullet" />
			</div>
			<span className="text">{children}</span>
		</StyledRadioButtonLabel>
	);
}
