import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";

const StyledRadioButtonLabel = styled.label<{
	/** Include just the radio button itself, not the text label? */
	$plain?: boolean;
}>(({ $plain }) => css`
	display: flex;
	gap: 8px;
	align-items: center;

	input {
		display: none;
	}

	.text {
		padding-bottom: 1px;
	}

	${styledExpanderItemText};

	${!$plain && css`
		.expander-child-items & {
			${styledExpanderItemBase};
			${styledExpanderItemContent};
		}
	`}

	${$plain && css`
		.text {
			display: none;
		}
	`}

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
	}

	&:hover:active .base .bullet {
		scale: ${10 / 18};
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

		.text {
			${styles.effects.text.bodyStrong};
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
	}

	&:hover:active input:checked ~ .base .bullet {
		scale: ${6 / 18};
	}

	input:checked[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
	}

	${styles.mixins.forwardFocusRing()};
`);

export default function RadioButton<T>({ children, id, value: [value, setValue], disabled, onChange, details, radioGroup, plain, icon, readOnly, ...htmlAttrs }: FCP<{
	/** Identifier. */
	id: T;
	/** The selected value in the current radio button group. */
	value: StateProperty<T>;
	/** Disabled? */
	disabled?: boolean;
	/** State change event. */
	onChange?(e: { id: T; value: T; checked: boolean }): void;
	/** Detailed description. */
	details?: ReactNode;
	/** Radio button group name, optional. */
	radioGroup?: string;
	/** Include just the radio button itself, not the text label? */
	plain?: boolean;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Make items cannot be selected? */
	readOnly?: boolean;
}, "label">) {
	const labelEl = useDomRef<"label">();
	const checked = value === id;
	const ariaId = useId();
	const handleCheck = (checked: boolean = true) => {
		if (checked) {
			setValue?.(id);
			onChange?.({ id, value: id, checked });
		}
	};

	useOnFormKeyDown(labelEl, { handleCheck, changeWhenMoveFocus: true, item: "*" });

	return (
		<StyledRadioButtonLabel
			tabIndex={checked ? 0 : -1}
			ref={labelEl}
			$plain={plain}
			role="radio"
			disabled={disabled}
			aria-disabled={disabled}
			inert={disabled}
			aria-checked={checked}
			aria-labelledby={`${ariaId}-title`}
			aria-describedby={`${ariaId}-details`}
			aria-readonly={readOnly}
			{...htmlAttrs}
		>
			<input
				type="radio"
				checked={checked}
				name={radioGroup}
				onChange={e => handleCheck(e.target.checked)}
				disabled={disabled}
				readOnly={readOnly}
			/>
			<div className="base">
				<div className="bullet" />
			</div>
			{icon && <Icon name={icon} />}
			<div className="text" aria-hidden>
				<p className="title" id={`${ariaId}-title`}>{children}</p>
				<p className="details" id={`${ariaId}-details`}>{details}</p>
			</div>
		</StyledRadioButtonLabel>
	);
}
