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

	.base {
		${styles.mixins.square("18px")};
		${styles.mixins.circle()};
		background-color: ${c("fill-color-control-alt-secondary")};
		outline: 1px solid ${c("stroke-color-control-strong-stroke-default")};
	}

	&:hover .base {
		background-color: ${c("fill-color-control-alt-tertiary")};
	}

	&:active .base {
		background-color: ${c("fill-color-control-alt-quarternary")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};

		.bullet {
			scale: ${10 / 18};
		}
	}

	input[disabled] ~ {
		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
			outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.text {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	input:checked ~ {
		.base {
			background-color: ${c("accent-color")};
			outline-color: ${c("accent-color")};

			.bullet {
				scale: ${8 / 18};
			}
		}
	}

	&:hover input:checked ~ .base {
		opacity: 0.9;

		.bullet {
			scale: ${10 / 18};
		}
	}

	&:active input:checked ~ .base {
		opacity: 0.8;

		.bullet {
			scale: ${6 / 18};
		}
	}

	input:checked[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
	}

	&:focus-visible {
		box-shadow: none;

		.base {
			${styles.effects.focus()};
		}
	}
`;

export default function RadioButton<T>({ children, id, value: [value, setValue], disabled, onChange }: FCP<{
	/** 标识符。 */
	id: T;
	/** 当前单选框组中选中的值。 */
	value: StateProperty<T>;
	/** 已禁用？ */
	disabled?: boolean;
	/** 状态改变事件。 */
	onChange?: (e: { id: T; value: T; checked: boolean }) => void;
}>) {
	const labelRef = useRef<HTMLLabelElement>(null);
	const checked = value === id;
	const handleCheck = (checked: boolean = true) => {
		if (checked) {
			setValue?.(id);
			onChange?.({ id, value: id, checked });
		}
	};

	useOnFormKeyDown(labelRef, "radio", handleCheck);

	return (
		<StyledRadioButtonLabel tabIndex={checked ? 0 : -1} ref={labelRef}>
			<input type="radio" checked={checked} onChange={e => handleCheck(e.target.checked)} disabled={disabled} />
			<div className="base">
				<div className="bullet" />
			</div>
			<span className="text">{children}</span>
		</StyledRadioButtonLabel>
	);
}
