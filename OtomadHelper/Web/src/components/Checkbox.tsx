import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";
import { weights } from "styles/effects";

const checkedOrIndet = ":is(:checked, :indeterminate)";
const unchecked = ":not(:checked, :indeterminate)";
const pressed = ":active:not(:has(:is(.actions, .button):active))";

const StyledCheckboxLabel = styled.label<{
	/** Include just the checkbox itself, without the text label? */
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

	.base {
		${styles.mixins.square("18px")};
		position: relative;
		flex-shrink: 0;
		background-color: ${c("fill-color-control-alt-secondary")};
		background-clip: padding-box;
		border: 1px solid ${c("stroke-color-control-strong-stroke-default")};
		border-radius: 3px;

		.icon {
			position: absolute;
			margin: calc((100% - 1em) / 2);
			color: ${c("fill-color-text-on-accent-primary")};
			font-size: 12px;
			clip-path: inset(0);
			transition-behavior: allow-discrete !important;

			&,
			* {
				transition: all ${eases.easeOutMax} 250ms, color 0s, fill 0s;
			}

			@starting-style {
				&:not([data-prev="appear"] *) {
					clip-path: inset(0 100% 0 0);
				}
			}

			&[hidden] {
				clip-path: inset(0 0 0 100%);
			}
		}

		&[data-prev="checked"] .icon[data-value="indeterminate"]:not([hidden]),
		&[data-prev="indeterminate"] .icon[data-value="checked"]:not([hidden]) {
			transition-delay: 250ms;
		}
	}

	input${checkedOrIndet} ~ .base,
	input${unchecked} ~ .base.changing {
		background-color: ${c("accent-color")} !important;
		border-color: ${c("accent-color")} !important;
	}

	&:hover,
	.items-view-item:hover & {
		input${unchecked} ~ .base {
			background-color: ${c("fill-color-control-alt-tertiary")};
		}

		input${checkedOrIndet} ~ .base {
			opacity: 0.9;
		}
	}

	&${pressed},
	.items-view-item${pressed} & {
		input${unchecked} ~ .base {
			background-color: ${c("fill-color-control-alt-quarternary")};
			border-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}
	}

	&${pressed} input${checkedOrIndet} ~ .base,
	.items-view-item${pressed} & input${checkedOrIndet} ~ .base {
		opacity: 0.8;

		.icon {
			color: ${c("fill-color-text-on-accent-secondary")};
		}
	}

	input${unchecked}[disabled] ~ {
		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
			border-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.text {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	input${checkedOrIndet}[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
		border-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
	}

	.items-view-item${pressed} & {
		pointer-events: none;
	}

	.actions {
		${styles.mixins.hideIfEmpty()};
	}

	${styles.mixins.forwardFocusRing()};
`);

interface SharedProps {
	/** Disabled? */
	disabled?: boolean;
	/** Detailed description. */
	details?: ReactNode;
	/** Include just the checkbox itself, without the text label? */
	plain?: boolean;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/** Icon. */
	icon?: DeclaredIcons;
	/**
	 * Temporarily disable the transition of the checkbox's checkmark icon?
	 *
	 * It is as well to disable the checkmark transition when it appears abnormal or affects the user experience
	 * (for example, when executing the `startViewTransition` function in the View Transition API simultaneously).
	 */
	disableCheckmarkTransition?: boolean;
	/**
	 * When using checkbox for selecting all, you can provide the checked item count and all item count.\
	 * Then the font weight of "Select all" text will changing dynamically.\
	 * The result font weight is mapped to a value from normal through bold.
	 */
	dynamicFontWeight?: [checkedCount: number, allCount: number];
}

export default function Checkbox<T>(props: FCP<{
	/** Identifier. */
	id: T;
	/** An array of selected values in the current checkbox group. */
	value: StateProperty<T[]>;
	/** State change event. */
	onChange?(e: { id: T; value: T[]; checkState: CheckState; checked: boolean }): void;
} & SharedProps, "label">): React.JSX.Element;
export default function Checkbox(props: FCP<{
	/** Is the checkbox currently selected? */
	value: StateProperty<boolean>;
	/** State change event. */
	onChange?(e: { checkState: CheckState; checked: boolean }): void;
} & SharedProps, "label">): React.JSX.Element;
export default function Checkbox(props: FCP<{
	/** Checked status. */
	value: StateProperty<CheckState>;
	/** State change event. */
	onChange?(e: { checkState: CheckState; checked: boolean | null }): void;
} & SharedProps, "label">): React.JSX.Element;
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled = false, onChange, details, plain = false, actions, icon, disableCheckmarkTransition, dynamicFontWeight, ref, ...htmlAttrs }: FCP<{
	id?: T;
	value: StateProperty<T[]> | StateProperty<boolean> | StateProperty<CheckState>;
	onChange?: Function;
} & SharedProps, "label">) {
	const labelEl = useDomRef<"label">();
	const checkboxEl = useDomRef<"input">();
	const singleMode = id === undefined, checkStateMode = typeof value === "string";
	const checked = checkStateMode ? value === "checked" : singleMode ? !!value : (value as T[]).includes(id);
	const indeterminate = value === "indeterminate";
	const prevState = usePrevious({ checked, indeterminate });
	const prev = prevState === undefined ? "appear" :
		prevState.checked ? "checked" : prevState.indeterminate ? "indeterminate" : "unchecked";
	const prevChanging = useChanging([checked, indeterminate]);
	const ariaId = useId();
	const fontWeight = !dynamicFontWeight || dynamicFontWeight[1] === 0 ? undefined :
		map(dynamicFontWeight[0], 0, dynamicFontWeight[1], weights.normal, weights.bold);

	useImperativeHandleRef(ref, labelEl);

	const handleChange = (checked: boolean, indeterminate: boolean) => {
		const checkbox = checkboxEl.current;
		if (!checkbox) return;
		const checkState: CheckState = indeterminate ? "indeterminate" : checked ? "checked" : "unchecked";
		if (singleMode)
			onChange?.({ checked: indeterminate ? null : checked, checkState });
		else
			onChange?.({ id, value: value as T[], checked, checkState });
	};

	const handleCheck = (checked?: boolean) => {
		checked ??= !checkboxEl.current?.checked;
		if (indeterminate && !checked) {
			checkboxEl.current && (checkboxEl.current.checked = true);
			checked = true;
		}
		const checkState: CheckState = checked ? "checked" : "unchecked";
		if (checkStateMode)
			(setValue as SetStateNarrow<CheckState>)?.(checkState);
		else if (singleMode)
			(setValue as SetStateNarrow<boolean>)?.(checked);
		else
			(setValue as SetStateNarrow<T[]>)?.(produce(values => {
				const draftedId = id as Draft<T>;
				if (checked) values.push(draftedId);
				else values.removeAllItem(draftedId);
			}));
	};

	useChangeEffect(() => handleChange(checked, indeterminate), [indeterminate, checked]);
	useEffect(() => { checkboxEl.current && (checkboxEl.current.indeterminate = indeterminate); }, [indeterminate, checkboxEl]);
	useOnFormKeyDown(labelEl, { handleCheck });

	return (
		<StyledCheckboxLabel
			tabIndex={0}
			ref={labelEl}
			$plain={plain}
			role="checkbox"
			disabled={disabled}
			aria-disabled={disabled}
			inert={disabled}
			aria-checked={indeterminate ? "mixed" : checked}
			aria-labelledby={`${ariaId}-title`}
			aria-describedby={`${ariaId}-details`}
			{...htmlAttrs}
		>
			<input
				type="checkbox"
				checked={checked}
				onChange={e => handleCheck(e.target.checked)}
				disabled={disabled}
				ref={checkboxEl}
			/>
			<div className={["base", { changing: prevChanging && !disableCheckmarkTransition }]} data-prev={prev}>
				<Icon name="checkbox/accept" data-value="checked" hidden={!checked} />
				<Icon name="checkbox/dash" data-value="indeterminate" hidden={!indeterminate} />
			</div>
			{!plain && (
				<>
					{icon && <Icon name={icon} />}
					<div className="text" aria-hidden>
						<p className="title" id={`${ariaId}-title`} style={{ fontWeight }}>{children}</p>
						<p className="details" id={`${ariaId}-details`}>{details}</p>
					</div>
					<div className="actions">
						{actions}
					</div>
				</>
			)}
		</StyledCheckboxLabel>
	);
}
