const styledDivider = css`
	border-block-start: 1px solid ${c("stroke-color-divider-stroke-default")};
`;

const StyledSubExpander = styled.div`
	display: contents;

	&:not(:first-child) {
		> .expander-item,
		> .toggle-switch-label {
			${styledDivider};
		}
	}

	> .expander-item {
		padding-inline-end: 15px;

		&:hover:not(:has(.trailing > :not(.expander-chevron):hover)) .action-icon {
			--state: hover;
		}

		&:active:not(:has(.trailing > :not(.expander-chevron):active)) .action-icon {
			--state: active;
		}

		&.expanded .action-icon {
			--expansion: expanded;
		}
	}

	> .expander-child {
		${styledDivider};
		overflow: hidden;

		&:has(> .expander-child-items:empty) {
			display: none;
		}

		> .expander-child-items:not(.no-indentation) > :not(.no-indentation, .info-bar),
		> .expander-child-items > .do-indentation {
			padding-inline-start: ${expanderItemPadding[1]}px;
		}

		> .expander-child-items > :not(:first-child) {
			${styledDivider};
		}

		${tgs()} {
			block-size: 0;

			> .expander-child-items {
				translate: 0 -100%;
			}
		}

		&.exit-active {
			border-block-start-width: 0;
		}
	}
`;

export /* @internal */ default function SubExpander({ icon, title, details, disabled, expanded = false, _requestExpanded, type = "chevron", noIndentation, anchor, actions, actuallyOn, asSubtitle, selectInfo, children, onChange: _onChange, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Set expansion or expand it initially. */
	expanded?: boolean | StatePropertyNonNull<boolean>;
	/** @private Request to expanded because user search something inside the expander. */
	_requestExpanded?: TransientValue<boolean>;
	/**
	 * Show chevron or switch as the expanding control.
	 *
	 * The difference is that, when user search something inside the sub-expander, if the type is switch, it will not
	 * auto turn on the toggle switch, and the children are also disabled; if the type is chevron, it will auto set it
	 * to expanded, and the children are still enabled.
	 *
	 * @default "chevron"
	 */
	type?: "chevron" | "switch";
	/** Should not it auto-add indentation at the start of child items? */
	noIndentation?: boolean;
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/**
	 * If when the toggle switch is on but useless, please pass this prop with `false`, and the label will show "On (Actually Off)".
	 *
	 * If you do not need this feature, please pass `undefined` for better performance.
	 *
	 * Note that this prop either toggles between `true` and `false` or is always `undefined`, never toggles between `true` and `undefined`.
	 *
	 * Available for switch type only.
	 *
	 * @default undefined
	 */
	actuallyOn?: boolean;
	/** As sub title style? (`"chevron"` type only.) */
	asSubtitle?: PropsOf<typeof Expander.Item>["asSubtitle"];
	/** Specifies the display string of the selection of tracks or track events. */
	selectInfo?: ReactNode;
}, GenericElement>) {
	const [, setAriaId, withAriaId] = useAriaIdRefState();
	let setExpanded: SetStateNarrow<boolean>;
	const internalExpanded = useState(typeof expanded === "boolean" ? expanded : expanded[0]);
	[expanded, setExpanded] = typeof expanded === "boolean" ? internalExpanded : expanded;
	const [requestExpanded, setRequestExpanded] = useState<boolean>();
	useEffect(() => { setRequestExpanded(undefined); }, [expanded]);
	useTransientValue(_requestExpanded, expanded => {
		if (expanded) {
			setRequestExpanded(true);
			if (type === "chevron") setExpanded(true);
		}
	});

	return (
		<StyledSubExpander>
			{
				type === "switch" ? (
					<ToggleSwitch
						on={[expanded, setExpanded]}
						icon={icon}
						details={details}
						disabled={disabled}
						anchor={anchor}
						actions={actions}
						actuallyOn={actuallyOn}
						ariaIdRef={setAriaId}
						aria-controls={withAriaId("children")}
						selectInfo={selectInfo}
						{...htmlAttrs}
					>
						{title}
					</ToggleSwitch>
				) : (
					<ClickOnSameElement
						onClick={() => setExpanded(expanded => !expanded)}
					>
						<Expander.Item
							title={title}
							icon={icon}
							details={details}
							disabled={disabled}
							anchor={anchor}
							clickable
							asSubtitle={asSubtitle}
							ariaIdRef={setAriaId}
							aria-expanded={expanded}
							aria-controls={withAriaId("children")}
							className={{ expanded }}
							selectInfo={selectInfo}
							{...htmlAttrs}
						>
							{actions}
							<div className={["action-icon", TRAILING_EXEMPTION, "expander-chevron"]} data-type={type}>
								<Icon name="chevron_down" />
							</div>
						</Expander.Item>
					</ClickOnSameElement>
				)
			}
			<CssTransition in={expanded || requestExpanded} unmountOnExit transitionEndProperty={["height", "block-size"]} requestAnimationFrame>
				<div className="expander-child" id={withAriaId("children")} aria-labelledby={withAriaId("title")}>
					<div className={["expander-child-items", { noIndentation }]}>
						<InteractionStateContext value={{ disabled: type === "switch" && !expanded ? true : undefined }}>
							{children}
						</InteractionStateContext>
					</div>
				</div>
			</CssTransition>
		</StyledSubExpander>
	);
}

export function parseNoIndentationProp(noIndentation?: boolean) {
	return noIndentation ? "no-indentation" : noIndentation === false ? "do-indentation" : undefined;
}

SubExpander.parseIndentationProp = parseNoIndentationProp;
