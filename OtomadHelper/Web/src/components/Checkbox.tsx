import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";

const checkedOrInd = ":is(:checked, :indeterminate)";
const unchecked = ":not(:checked, :indeterminate)";
const iconExiting = ":has(.icon.exit)";

const StyledCheckboxLabel = styled.label<{
	/** 是否仅包含复选框本身，不包含文本标签？ */
	$plain?: boolean;
}>`
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

	${({ $plain }) => !$plain && css`
		.expander-child-items & {
			${styledExpanderItemBase};
			${styledExpanderItemContent};
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

			${tgs(tgs.enter)} {
				clip-path: inset(0 100% 0 0);
			}

			${tgs(tgs.exit)} {
				clip-path: inset(0 0 0 100%);
			}
		}
	}

	input${checkedOrInd} ~ .base,
	.base${iconExiting} {
		background-color: ${c("accent-color")} !important;
		outline-color: ${c("accent-color")} !important;
	}

	&:hover,
	.items-view-item:hover & {
		input${unchecked} ~ .base {
			background-color: ${c("fill-color-control-alt-tertiary")};
		}

		input${checkedOrInd} ~ .base {
			opacity: 0.9;
		}
	}

	&:active,
	.items-view-item:active & {
		input${unchecked} ~ .base {
			background-color: ${c("fill-color-control-alt-quarternary")};
			outline-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}
	}

	&:active input${checkedOrInd} ~ .base,
	.items-view-item:active & input${checkedOrInd} ~ .base,
	&:is(:hover, :active) .base${iconExiting} {
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

	input${checkedOrInd}[disabled] ~ .base {
		background-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
		outline-color: ${c("stroke-color-control-strong-stroke-disabled")} !important;
	}

	.items-view-item:active & {
		pointer-events: none;
	}

	${styles.mixins.forwardFocusRing()};
`;

interface SharedProps {
	/** 已禁用？ */
	disabled?: boolean;
	/** 详细描述。 */
	details?: ReactNode;
	/** 是否仅包含复选框本身，不包含文本标签？ */
	plain?: boolean;
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
export default function Checkbox<T>({ children, id, value: [value, setValue], disabled = false, onChange, details, plain = false }: FCP<{
	id?: T;
	value: StateProperty<T[]> | StateProperty<boolean> | StateProperty<CheckState>;
	onChange?: Function;
} & SharedProps>) {
	const labelEl = useDomRef<"label">();
	const checkboxEl = useDomRef<"input">();
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
	const getCheckMarkName = useCallback(() => indeterminate ? "checkbox/dash" : checked ? "checkbox/accept" : "", [indeterminate, checked]);
	const [checkMarkName, setCheckMarkName] = useState(getCheckMarkName());
	useEffect(() => {
		setCheckMarkName(getCheckMarkName());
	}, [indeterminate, checked]);

	return (
		<StyledCheckboxLabel tabIndex={0} ref={labelEl} $plain={plain}>
			<input
				type="checkbox"
				checked={checked}
				onChange={e => handleCheck(e.target.checked)}
				disabled={disabled}
				ref={checkboxEl}
			/>
			<div className="base">
				<SwitchTransition>
					<CssTransition key={checkMarkName} maxTimeout={500}>
						<Icon name={checkMarkName} />
					</CssTransition>
				</SwitchTransition>
			</div>
			{!plain && (
				<div className="text">
					<p className="title">{children}</p>
					<p className="details">{details}</p>
				</div>
			)}
		</StyledCheckboxLabel>
	);
}
