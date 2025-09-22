import chevron from "assets/icons/chevron_down.svg?inline"; // TODO: DEV import only.
import colors from "styles/colors";
import { StyledButton } from "./Button";
import { styledSimpleIndicator } from "./ItemsView/ItemsViewItem";
import { inputInSettingsCardStyle } from "./TextBox";

const themedChevron = colors["foreground-color"].slice(0, 2).map(color =>
	chevron.replace("svg%20", `svg%20fill='${encodeURI(color)}'%20`));

// Apply focus-visible ring style only if it is webview environment (as a button) or the customizable select element.
// Not for default select element, or the focus ring will unexpectedly appear when mouse clicking without keyboard.
// However w3c treat it as a feature for similar behavior of input element.
const enabledFocusVisible = css`
	&:focus-visible {
		${styles.effects.focus(false, true)};
	}
`;

const StyledComboBox = styled(StyledButton)(() => css`
	padding: 4px 11px;

	${inputInSettingsCardStyle};

	.content {
		${styles.mixins.square("100%")};

		&,
		.text {
			display: flex;
			gap: 8px;
			align-items: center;
		}

		.text {
			${styles.effects.text.body};
			width: 100%;
		}
	}

	.icon {
		flex-shrink: 0;
		font-size: 16px;
	}

	&:active .content .icon.chevron {
		translate: 0 2px;
	}

	// Override the base button style.
	&::after {
		content: none;
	}

	button& {
		${enabledFocusVisible};
	}

	select& {
		margin-inline: 1px;

		option {
			background-color: ${c("background-color")};
		}

		&[disabled] {
			color: ${c("fill-color-text-disabled")};
		}

		@supports not (appearance: base-select) {
			background-image: url("${themedChevron[1]}");
			background-repeat: no-repeat;
			background-position: calc(100% - 11px) center;
			background-size: 16px;
			background-attachment: scroll;
			appearance: none;

			${ifColorScheme.light} & {
				background-image: url("${themedChevron[0]}");
			}

			&:active {
				background-position-y: calc(50% + 2px);
			}

			&:dir(rtl) {
				background-position-x: 11px;
			}
		}

		@supports (appearance: base-select) {
			${enabledFocusVisible};

			&,
			&::picker(select) {
				appearance: base-select;
			}

			&::picker-icon {
				display: none;
			}

			&::picker(select) {
				position-area: block-end;
				position-try: most-block-size flip-block;
				inline-size: calc(anchor-size(self-inline) + 12px);
				padding: 2px;
				background-color: ${c("background-fill-color-acrylic-background-command-bar")};
				border: none;
				border-radius: 7px;
				outline: 1px solid ${c("stroke-color-surface-stroke-flyout")};
				box-shadow: 0 8px 16px ${c("shadows-flyout")};
				opacity: 0;
				backdrop-filter: blur(60px);
				transition: ${fallbackTransitions}, width 0s, height 0s;
				transition-behavior: allow-discrete;

				&:popover-open {
					opacity: 1;

					@starting-style {
						opacity: 0;
					}
				}
			}

			option {
				${styles.effects.text.body};
				position: relative;
				margin: 3px;
				padding: 6px 12px;
				background-color: transparent;
				border-radius: 5px;

				&::checkmark {
					display: none;
				}

				${styledSimpleIndicator};

				&::before {
					block-size: ${100 * 32 / 66}%;
				}

				&:hover,
				&:checked {
					background-color: ${c("fill-color-subtle-secondary")};
				}

				&:not(:checked):active,
				&:checked:not(:active):hover {
					background-color: ${c("fill-color-subtle-tertiary")};
				}

				&:not(:checked)::before {
					scale: 1 0;
				}

				&:active {
					color: ${c("fill-color-text-secondary")};

					&::before {
						scale: 1 0.625;
					}
				}
			}
		}
	}

	.vertical-if-flex-wrap > & {
		transition: ${fallbackTransitions}, inline-size 0s, block-size 0s;
	}

	.vertical-if-flex-wrap.has-child-wrapped:not(.tentative-touch-approach) > & {
		inline-size: calc(100% - 2px);
	}
`);

export default function ComboBox<T extends string | number>(props: FCP<{
	/** The identifiers for each option of the combo box. */
	ids: readonly T[];
	/** The display texts for each option of the combo box. */
	options: readonly Readable[];
	/** Optional. The icons for each option of the combo box. */
	icons?: readonly DeclaredIcons[];
	/** The selected option of the combo box. */
	current: StateProperty<T>;
}, "select">): React.JSX.Element;
export default function ComboBox(props: FCP<{}, "select">): React.JSX.Element;
export default function ComboBox<T extends string | number>({ ids = [], options = [], icons = [], current: [current, setCurrent] = NEVER_MIND, disabled, ...htmlAttrs }: FCP<{
	ids?: readonly T[];
	options?: readonly Readable[];
	icons?: readonly DeclaredIcons[];
	current?: StateProperty<T>;
}, "select">) {
	const [iconSvgs, setIconSvgs] = useState<string[]>();
	const hasIcons = icons.length > 0;
	const currentOption = options[ids.indexOf(current!)] ?? `<${current}>`;
	const currentIcon = icons[ids.indexOf(current!)];
	disabled = useContext(InteractionStateContext).disabled || disabled;

	useEffect(() => {
		if (!hasIcons) setIconSvgs(undefined);
		else setIconSvgs(icons.map(icon => Icon.getRawSvg(icon) ?? ""));
	}, [icons, hasIcons]);

	const showComboBox: MouseEventHandler<HTMLButtonElement> = async e => {
		const rect = e.currentTarget.getBoundingClientRect();
		const result = await bridges.bridge.showComboBox(rect, current!, ids, toStringArray(options), iconSvgs) as T;
		setCurrent?.(result);
	};

	if (window.isWebView)
		return (
			<StyledComboBox
				role="combobox"
				aria-expanded={false}
				aria-haspopup
				disabled={disabled}
				onClick={showComboBox}
				{...htmlAttrs as FCP<{}, "button">}
			>
				<div className="content">
					{hasIcons && (currentIcon ? <Icon name={currentIcon} /> : <Icon shadow />)}
					<div className="text">{currentOption}</div>
					<Icon name="chevron_down" className="chevron" />
				</div>
			</StyledComboBox>
		);
	else // fallback in dev (a normal browser)
		return (
			<StyledComboBox
				as="select"
				role="combobox"
				disabled={disabled}
				value={current}
				onChange={e => setCurrent?.(e.currentTarget.value as T)}
				{...htmlAttrs}
			>
				<button type="button" className="content">
					<selectedcontent className="text" />
					<Icon name="chevron_down" className="chevron" />
				</button>
				{ids.map((id, i) => (
					<option key={id} value={id}>
						{hasIcons && (icons[i] ? <Icon name={icons[i]} /> : <Icon shadow />)}
						{options[i]}
					</option>
				))}
			</StyledComboBox>
		);
}

/* eslint-disable @typescript-eslint/no-wrapper-object-types */
// The `Object` means every JavaScript object base class (including string, boolean and almost everything),
// So here we use `Object` instead of `object` for correct typing. So does ESLint disable.
function toStringArray(array: readonly Object[]) {
	return array.map(item => item.toString());
}
