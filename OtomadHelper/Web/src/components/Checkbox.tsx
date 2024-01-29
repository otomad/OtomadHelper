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

	${styles.mixins.forwardFocusRing()};

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
}

export default function Checkbox<T>(props: FCP<{
	/** 标识符。 */
	id: T;
	/** 当前单选框组中选中的值数组。 */
	value: StateProperty<T[]>;
	/** 状态改变事件。 */
	onChange?: (e: { id: T; value: T[]; checkState: CheckState; checked: boolean }) => void;
} & SharedProps>): JSX.Element;
export default function Checkbox(props: FCP<{
	/** 当前单选框是否被选中？ */
	value: StateProperty<boolean>;
	/** 状态改变事件。 */
	onChange?: (e: { checkState: CheckState; checked: boolean }) => void;
} & SharedProps>): JSX.Element;
export default function Checkbox(props: FCP<{
	/** 复选状态。 */
	value: StateProperty<CheckState>;
	/** 状态改变事件。 */
	onChange?: (e: { checkState: CheckState; checked: boolean | null }) => void;
} & SharedProps>): JSX.Element;
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled, onChange }: FCP<{
	id?: T;
	value: StateProperty<T[]> | StateProperty<boolean> | StateProperty<CheckState>;
	onChange?: Function;
} & SharedProps>) {
	const labelRef = useRef<HTMLLabelElement>(null);
	const checkboxRef = useRef<HTMLInputElement>(null);
	const singleMode = id === undefined, checkStateMode = typeof value === "string";
	const checked = checkStateMode ? value === "checked" : singleMode ? !!value : (value as T[]).includes(id);
	const indeterminate = value === "indeterminate";

	const handleChange = (checked: boolean, indeterminate: boolean) => {
		const checkbox = checkboxRef.current;
		if (!checkbox) return;
		const checkState: CheckState = indeterminate ? "indeterminate" : checked ? "checked" : "unchecked";
		if (singleMode)
			onChange?.({ checked: indeterminate ? null : checked, checkState });
		else
			onChange?.({ id, value: value as T[], checked, checkState });
	};

	const handleCheck = (checked?: boolean) => {
		checked ??= !checkboxRef.current?.checked;
		if (indeterminate && !checked) {
			checkboxRef.current && (checkboxRef.current.checked = true);
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
				else arrayRemoveAllItem(values, draftedId);
			}));
	};

	useChangeEffect(() => handleChange(checked, indeterminate), [indeterminate, checked]);
	useEffect(() => { checkboxRef.current && (checkboxRef.current.indeterminate = indeterminate); }, [checkboxRef, indeterminate]);
	useOnFormKeyDown(labelRef, "checkbox", handleCheck);
	const getCheckMarkName = useCallback(() => indeterminate ? "dash" : checked ? "accept" : "", [indeterminate, checked]);
	const [checkMarkName, setCheckMarkName] = useState(getCheckMarkName());
	const prevCheckMarkName = usePrevious(checkMarkName);
	useEffect(() => {
		setCheckMarkName(getCheckMarkName());
	}, [indeterminate, checked]);
	const switchTransitionTimeout = prevCheckMarkName === "" ? 70 : undefined;

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
				<SwitchTransition>
					<CssTransition key={checkMarkName} timeout={switchTransitionTimeout}>
						<Icon name={checkMarkName} />
					</CssTransition>
				</SwitchTransition>
			</div>
			<span className="text">{children}</span>
		</StyledRadioButtonLabel>
	);
}
