import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";

const checkedOrInd = ":is(:checked, :indeterminate)";
const iconExiting = ":has(.icon.exit)";

const StyledRadioButtonLabel = styled.label`
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

	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};
	}

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

			${tgs(tgs.enter)} {
				clip-path: inset(0 100% 0 0);
			}

			${tgs(tgs.exit)} {
				clip-path: inset(0 0 0 100%);
			}
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

	input${checkedOrInd} ~ .base,
	.base${iconExiting} {
		background-color: ${c("accent-color")};
		outline-color: ${c("accent-color")};
	}

	&:hover input${checkedOrInd} ~ .base {
		opacity: 0.9;
	}

	&:active input${checkedOrInd} ~ .base,
	&:is(:hover, :active) .base${iconExiting} {
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
`;

interface SharedProps {
	/** 已禁用？ */
	disabled?: boolean;
	/** 详细描述。 */
	details?: ReactNode;
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
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled, onChange, details }: FCP<{
	id?: T;
	value: StateProperty<T[]> | StateProperty<boolean> | StateProperty<CheckState>;
	onChange?: Function;
} & SharedProps>) {
	const labelEl = useDomRef<HTMLLabelElement>();
	const checkboxEl = useDomRef<HTMLInputElement>();
	const singleMode = id === undefined, checkStateMode = typeof value === "string";
	const checked = checkStateMode ? value === "checked" : singleMode ? !!value : (value as T[]).includes(id);
	const indeterminate = value === "indeterminate";

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
	useEffect(() => { checkboxEl.current && (checkboxEl.current.indeterminate = indeterminate); }, [indeterminate]);
	useOnFormKeyDown(labelEl, "checkbox", handleCheck);
	const getCheckMarkName = useCallback(() => indeterminate ? "dash" : checked ? "accept" : "", [indeterminate, checked]);
	const [checkMarkName, setCheckMarkName] = useState(getCheckMarkName());
	useEffect(() => {
		setCheckMarkName(getCheckMarkName());
	}, [indeterminate, checked]);

	return (
		<StyledRadioButtonLabel tabIndex={0} ref={labelEl}>
			<input
				type="checkbox"
				checked={checked}
				onChange={e => handleCheck(e.target.checked)}
				disabled={disabled}
				ref={checkboxEl}
			/>
			<div className="base">
				<SwitchTransition>
					<CssTransition key={checkMarkName}>
						<Icon name={checkMarkName} />
					</CssTransition>
				</SwitchTransition>
			</div>
			<div className="text">
				<p className="title">{children}</p>
				<p className="details">{details}</p>
			</div>
		</StyledRadioButtonLabel>
	);
}
