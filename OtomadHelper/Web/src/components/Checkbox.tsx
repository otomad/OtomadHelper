import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

const checkedOrInd = ":is(:checked, :indeterminate)";

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

	.base {
		${styles.mixins.square("18px")};
		${styles.mixins.gridCenter()};
		border-radius: 3px;
		background-color: ${c("fill-color-control-alt-secondary")};
		outline: 1px solid ${c("stroke-color-control-strong-stroke-default")};

		.icon {
			font-size: 12px;
			color: ${c("fill-color-text-on-accent-primary")};
		}
	}

	&:hover .base {
		background-color: ${c("fill-color-control-alt-tertiary")};
	}

	&:active .base {
		background-color: ${c("fill-color-control-alt-quarternary")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
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

	input${checkedOrInd} ~ .base {
		background-color: ${c("accent-color")};
		outline-color: ${c("accent-color")};
	}

	&:hover input${checkedOrInd} ~ .base {
		opacity: 0.9;
	}

	&:active input${checkedOrInd} ~ .base {
		opacity: 0.8;

		.icon {
			color: ${c("fill-color-text-on-accent-secondary")};
		}
	}

	input${checkedOrInd}[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")};
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
	}

	&:focus-visible {
		box-shadow: none;

		.base {
			${styles.effects.focus()};
		}
	}

	.base .icon {
		&.enter {
			clip-path: inset(0 100% 0 0);
		}

		&.enter-active,
		&.exit {
			clip-path: inset(0 0 0 0);
		}

		&.exit-active {
			clip-path: inset(0 0 0 100%);
		}
	}
`;

interface SharedProps {
	/** 已禁用？ */
	disabled?: boolean;
	/** 是否是不定状态/半选状态？ */
	indeterminate?: StateProperty<boolean>;
}

export default function Checkbox<T>(props: FCP<{
	/** 标识符。 */
	id: T;
	/** 当前单选框组中选中的值数组。 */
	value: StateProperty<T[]>;
} & SharedProps>): JSX.Element;
export default function Checkbox(props: FCP<{
	/** 当前单选框是否被选中？ */
	value: StateProperty<boolean>;
} & SharedProps>): JSX.Element;
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled, indeterminate: [indeterminate, setIndeterminate] = [] }: FCP<{
	/** 标识符。 */
	id?: T;
	/** 当前单选框组中选中的值数组。 */
	value: StateProperty<T[]> | StateProperty<boolean>;
} & SharedProps>) {
	const labelRef = useRef<HTMLLabelElement>(null);
	const checkboxRef = useRef<HTMLInputElement>(null);
	const singleMode = id === undefined;
	const checked = indeterminate ? false : singleMode ? !!value : value === id;
	const handleCheck = (checked?: boolean) => {
		checked ??= !checkboxRef.current?.checked;
		if (indeterminate && !checked) {
			checkboxRef.current && (checkboxRef.current.checked = true);
			checked = true;
		}
		setIndeterminate?.(false);
		if (!singleMode)
			(setValue as SetStateNarrow<T[]>)?.(produce(values => {
				const draftedId = id as Draft<T>;
				if (checked) values.push(draftedId);
				else arrayRemoveAllItem(values, draftedId);
			}));
		else
			(setValue as SetStateNarrow<boolean>)?.(checked);
	};

	useEffect(() => { checkboxRef.current && (checkboxRef.current.indeterminate = !!indeterminate); }, [checkboxRef, indeterminate]);
	useOnFormKeydown(labelRef, "checkbox", handleCheck);
	const getCheckMarkName = useCallback(() => indeterminate ? "dash" : checked ? "accept" : "", [indeterminate, checked]);
	const [checkMarkName, setCheckMarkName] = useState(getCheckMarkName());
	const prevCheckMarkName = usePrevious(checkMarkName);
	useEffect(() => {
		setCheckMarkName(getCheckMarkName());
	}, [indeterminate, checked]);
	const switchTransitionMode = prevCheckMarkName === "" ? "in-out" : "out-in";

	return (
		<StyledRadioButtonLabel tabIndex={0} ref={labelRef}>
			<input
				type="checkbox"
				checked={checked}
				onChange={e => handleCheck(e.target.checked)}
				disabled={disabled}
				ref={checkboxRef}
			/>
			<div className="base">
				<SwitchTransition mode={switchTransitionMode}>
					<CssTransition key={checkMarkName}>
						<Icon name={checkMarkName} />
					</CssTransition>
				</SwitchTransition>
			</div>
			<span className="text">{children}</span>
		</StyledRadioButtonLabel>
	);
}
