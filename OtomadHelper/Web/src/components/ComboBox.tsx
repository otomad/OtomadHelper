import { StyledButton } from "./Button";
import { styledSimpleIndicator } from "./ItemsView/ItemsViewItem";
import { inputInSettingsCardStyle } from "./TextBox";

// Apply focus-visible ring style only if it is webview environment (as a button) or the customizable select element.
// Not for default select element, or the focus ring will unexpectedly appear when mouse clicking without keyboard.
// However w3c treat it as a feature for similar behavior of input element.
const enabledFocusVisible = css`
	&:focus-visible {
		${styles.effects.focus(false, true)};
	}
`;

const OPTION_HEIGHT = 35;

const StyledComboBox = styled(StyledButton)(() => css`
	padding: 4px 9px;

	${inputInSettingsCardStyle};

	.content {
		${styles.mixins.square("100%")};
		gap: 0;
		opacity: 1 !important;

		&,
		.text {
			display: flex;
			align-items: center;
		}

		.text {
			${styles.text.body};
			gap: 8px;
			width: 100%;
			contain: inline-size;
			overflow-inline: clip;
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
		option {
			background-color: ${c("background-color")};
		}

		&[disabled] {
			color: ${c("fill-color-text-disabled")};
		}

		@supports (appearance: base-select) {
			${enabledFocusVisible};
			--selected-index: attr(data-selected-index type(<integer>), 0);
			--item-length: attr(data-item-length type(<integer>), 0);

			&,
			&::picker(select) {
				appearance: base-select;
			}

			&::picker-icon {
				display: none;
			}

			&::picker(select) {
				position: fixed;
				position-area: none;
				inset-block-start: clamp(
					0dvh,
					calc(anchor(start) - 3px - ${OPTION_HEIGHT}px * var(--selected-index)),
					calc(100dvb - 3px - ${OPTION_HEIGHT}px * var(--item-length))
				);
				inset-inline-start: calc(anchor(start) - 3px);
				max-block-size: 100dvb;
				inline-size: calc(anchor-size(self-inline) + 7px);
				padding: 2px;
				background-color: ${c("background-fill-color-acrylic-background-command-bar")};
				border: none;
				border-radius: 7px;
				outline: 1px solid ${c("stroke-color-surface-stroke-flyout")};
				box-shadow: if(
					${ifColorScheme.contrast} or ${ifColorScheme.reduceTransparency}: none;
					else: 0 8px 16px ${c("shadows-flyout")};
				);
				opacity: 0;
				backdrop-filter: blur(60px);
				transition: if(
					${ifColorScheme.reduceMotion}: none;
					else: ${fallbackTransitions}, width 0s, height 0s, inset-block-start step-end 250ms;
				);
				transition-behavior: allow-discrete;

				${styles.effects.refreshedBackdropIfHasBackgroundImage};

				&:popover-open {
					opacity: 1;

					@starting-style {
						opacity: 0;
					}
				}

				&:not(:popover-open) {
					--top-transition-timing-function: step-end;
				}
			}

			&:open .chevron {
				rotate: 0.5turn;
			}

			option {
				${styles.text.body};
				position: relative;
				display: flex;
				gap: 8px;
				block-size: ${OPTION_HEIGHT}px;
				padding-block: 6px 8px;
				padding-inline: 9.5px;
				background-color: transparent;
				background-clip: padding-box;
				border: 1.5px solid transparent;
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

				&:focus-visible {
					--focus-ring-length-outer: 2px;
					--focus-ring-length-inner: 0;
				}

				${ifColorScheme.at.contrast} {
					&:is(:hover, :active, :checked)${important()} {
						background-color: ${cc("Highlight")};

						&,
						* {
							color: ${cc("HighlightText")};
						}

						&::before {
							background-color: ${cc("HighlightText")};
						}
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

interface Props<T extends string | number> {
	/** The identifiers for each option of the combo box. */
	ids: readonly T[];
	/** The display texts for each option of the combo box. */
	options: readonly Readable[];
	/** Optional. The icons for each option of the combo box. */
	icons?: readonly DeclaredIcons[];
	/** The selected option of the combo box. */
	current: VariousState<T>;
	/** Additional attributes mapped by ID for option elements (base select appearance only). */
	optionAttrs?: (id: T) => PropsOf<"option">;
	/** Force to use CSS base-select appearance select combo box? */
	forceBaseSelectAppearance?: boolean;
	value?: never;
}

export default function ComboBox<T extends string | number>(props: FCP<Props<T>, "select">): React.JSX.Element;
export default function ComboBox(props: FCP<{ value?: never }, "select">): React.JSX.Element;
export default function ComboBox<T extends string | number>({ ids = [], options = [], icons = [], current: _current, disabled, optionAttrs, forceBaseSelectAppearance = false, ...htmlAttrs }: FCP<Partial<Props<T>>, "select">) {
	const [current, setCurrent] = useVariousState(_current);
	const [iconSvgs, setIconSvgs] = useState<string[]>();
	const hasIcons = icons.length > 0;
	const currentIndex = ids.indexOf(current);
	const currentOption = options[currentIndex] ?? `<${current}>`;
	const currentIcon = icons[currentIndex];
	disabled = useContext(InteractionStateContext).disabled || disabled;
	const numberAsKey = typeof ids[0] === "number";

	useEffect(() => {
		if (!hasIcons) setIconSvgs(undefined);
		else setIconSvgs(icons.map(icon => Icon.getRawSvg(icon) ?? ""));
	}, [icons, hasIcons]);

	const showComboBox: MouseEventHandler<HTMLButtonElement> = async e => {
		const rect = e.currentTarget.getBoundingClientRect();
		const result = await bridges.bridge.showComboBox(rect, current, ids, toStringArray(options), iconSvgs) as T;
		setCurrent?.(result);
	};

	if (window.isWebView && !forceBaseSelectAppearance)
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
					<div className="text">
						{hasIcons && (currentIcon ? <Icon name={currentIcon} /> : <Icon shadow />)}
						{currentOption}
					</div>
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
				data-selected-index={currentIndex}
				data-item-length={ids.length}
				onChange={e => {
					let value = e.currentTarget.value as T;
					if (numberAsKey) value = +value as T;
					setCurrent?.(value);
				}}
				{...htmlAttrs}
			>
				<button type="button" className="content">
					<selectedcontent className="text" />
					<Icon name="chevron_down" className="chevron" />
				</button>
				{ids.map((id, i) => (
					<option key={id} value={id} {...optionAttrs?.(id)}>
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
