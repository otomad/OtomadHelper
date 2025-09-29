import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";

const checkedOrIndet = ":is(:checked, :indeterminate)";
const unchecked = ":not(:checked, :indeterminate)";
const iconExiting = ":has(.icon.exit, .icon.enter-done)";
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
		${styles.mixins.gridCenter()};
		background-color: ${c("fill-color-control-alt-secondary")};
		border-radius: 3px;
		outline: 1px solid ${c("stroke-color-control-strong-stroke-default")};

		.icon {
			color: ${c("fill-color-text-on-accent-primary")};
			font-size: 12px;
			clip-path: inset(0);

			&,
			* {
				transition: all ${eases.easeOutMax} 250ms, color 0s, fill 0s;
			}

			${tgs(tgs.enter)} {
				clip-path: inset(0 100% 0 0);
			}

			${tgs(tgs.exit)} {
				clip-path: inset(0 0 0 100%);
			}
		}
	}

	input${checkedOrIndet} ~ .base:has(.icon),
	.base.disable-checkmark-transition${iconExiting} {
		background-color: ${c("accent-color")} !important;
		outline-color: ${c("accent-color")} !important;
	}

	.base${iconExiting} {
		transition: ${fallbackTransitions}, background-color ${eases.easeInSmooth} 250ms, outline-color ${eases.easeInSmooth} 250ms, var(--fallback-transitions-for-contrast-scheme);
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
			outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}
	}

	&${pressed} input${checkedOrIndet} ~ .base,
	.items-view-item${pressed} & input${checkedOrIndet} ~ .base,
	&${pressed} .base${iconExiting} {
		opacity: 0.8;

		.icon {
			color: ${c("fill-color-text-on-accent-secondary")};
		}
	}

	input${unchecked}[disabled] ~ {
		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
			outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.text {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	input${checkedOrIndet}[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
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
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled = false, onChange, details, plain = false, actions, icon, disableCheckmarkTransition, ref, ...htmlAttrs }: FCP<{
	id?: T;
	value: StateProperty<T[]> | StateProperty<boolean> | StateProperty<CheckState>;
	onChange?: Function;
} & SharedProps, "label">) {
	const labelEl = useDomRef<"label">();
	const checkboxEl = useDomRef<"input">();
	const singleMode = id === undefined, checkStateMode = typeof value === "string";
	const checked = checkStateMode ? value === "checked" : singleMode ? !!value : (value as T[]).includes(id);
	const indeterminate = value === "indeterminate";
	const ariaId = useId();

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
	const getCheckMarkName = useCallback(() => indeterminate ? "checkbox/dash" : checked ? "checkbox/accept" : "", [indeterminate, checked]);
	const [checkMarkName, setCheckMarkName] = useState<ReturnType<typeof getCheckMarkName>>(getCheckMarkName()); // What the hell is TypeScript's problem?
	useEffect(() => {
		setCheckMarkName(getCheckMarkName());
	}, [indeterminate, checked, getCheckMarkName]);

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
			<div className={["base", { disableCheckmarkTransition }]}>
				<SwitchTransition>
					<CssTransition key={checkMarkName} maxTimeout={500} disableTransition={disableCheckmarkTransition} requestAnimationFrame>
						<Icon name={checkMarkName} />
					</CssTransition>
				</SwitchTransition>
			</div>
			{!plain && (
				<>
					{icon && <Icon name={icon} />}
					<div className="text" aria-hidden>
						<p className="title" id={`${ariaId}-title`}>{children}</p>
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
