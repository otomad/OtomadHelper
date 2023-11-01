import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

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

	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};
	}

	.bullet {
		${styles.mixins.square("100%")};
		${styles.mixins.circle()};
		background-color: ${c("fill-color-text-on-accent-primary")};
		outline: 1px solid ${c("stroke-color-control-stroke-secondary")};
		scale: 0;
	}

	.radio {
		${styles.mixins.square("18px")};
		${styles.mixins.circle()};
		background-color: ${c("fill-color-control-alt-secondary")};
		outline: 1px solid ${c("stroke-color-control-strong-stroke-default")};
	}

	&:hover .radio {
		background-color: ${c("fill-color-control-alt-tertiary")};
	}

	&:active .radio {
		background-color: ${c("fill-color-control-alt-quarternary")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};

		.bullet {
			scale: ${10 / 18};
		}
	}

	input[disabled] ~ {
		.radio {
			background-color: ${c("fill-color-control-alt-disabled")};
			outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.text {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	input:checked ~ {
		.radio {
			background-color: ${c("accent-color")};
			outline-color: ${c("accent-color")};

			.bullet {
				scale: ${8 / 18};
			}
		}
	}

	&:hover input:checked ~ .radio .bullet {
		scale: ${10 / 18};
	}

	&:active input:checked ~ .radio .bullet {
		scale: ${6 / 18};
	}

	input:checked[disabled] ~ .radio {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
	}

	&:focus-visible {
		box-shadow: none;

		.radio {
			${styles.effects.focus()};
		}
	}
`;

export default function RadioButton<T>({ children, id, value: [curValue, setValue] }: FCP<{
	/** 标识符。 */
	id: T;
	/** 当前单选框组中选中的值。 */
	value: StateProperty<T>;
}>) {
	const checked = curValue === id;

	return (
		<StyledRadioButtonLabel tabIndex={checked ? 0 : -1}>
			<input type="radio" checked={checked} onChange={e => e.target.checked && setValue?.(id)} />
			<div className="radio">
				<div className="bullet" />
			</div>
			<span className="text">{children}</span>
		</StyledRadioButtonLabel>
	);
}
