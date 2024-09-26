import { StyledButton } from "./Button";
import { inputInSettingsCardStyle } from "./TextBox";

const StyledComboBox = styled(StyledButton)`
	padding: 4px 11px;

	${inputInSettingsCardStyle};

	.content {
		${styles.mixins.square("100%")};
		display: flex;
		gap: 8px;
		align-items: center;

		.text {
			${styles.effects.text.body};
			width: 100%;
		}

		.icon {
			flex-shrink: 0;
			font-size: 16px;
		}
	}

	&:active .content .icon {
		translate: 0 2px;
	}

	select& option {
		background-color: ${c("background-color")};
	}
`;

export default function ComboBox<T extends string | number>({ ids, options, current: [current, setCurrent], ...htmlAttrs }: FCP<{
	/** The identifiers of the combo box. */
	ids: readonly T[];
	/** The options of the combo box. */
	options: readonly Readable[];
	/** The selected option of the combo box. */
	current: StateProperty<T>;
}, "select">) {
	const currentOption = options[ids.indexOf(current!)] ?? `<${current}>`;

	const showComboBox: MouseEventHandler<HTMLButtonElement> = async e => {
		const rect = e.currentTarget.getBoundingClientRect();
		const result = await bridges.bridge.showComboBox(rect, current!, ids, toStringArray(options)) as T;
		setCurrent?.(result);
	};

	if (window.isWebView)
		return (
			<StyledComboBox onClick={showComboBox} {...htmlAttrs as FCP<{}, "button">}>
				<div className="content">
					<div className="text">{currentOption}</div>
					<Icon name="chevron_down" />
				</div>
			</StyledComboBox>
		);
	else // fallback in dev (a normal browser)
		return (
			<StyledComboBox as="select" defaultValue={current} onChange={e => setCurrent?.(e.currentTarget.value as T)} {...htmlAttrs}>
				{ids.map((id, i) => <option key={id} value={id}>{options[i]}</option>)}
			</StyledComboBox>
		);
}

/* eslint-disable @typescript-eslint/no-wrapper-object-types */
// The `Object` means every JavaScript object base class (including string, boolean and almost everything),
// So here we use `Object` instead of `object` for correct typing. So does ESLint disable.
function toStringArray(array: readonly Object[]) {
	return array.map(item => item.toString());
}
