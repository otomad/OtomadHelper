const styledDivider = css`
	border-block-start: 1px solid ${c("stroke-color-divider-stroke-default")};
`;

const StyledSubExpander = styled.div`
	display: contents;

	> .expander-item,
	> .toggle-switch-label {
		${styledDivider};
	}

	> .expander-item {
		padding-inline-end: 15px;

		&:hover .action-icon {
			--state: hover;
		}

		&:active .action-icon {
			--state: active;
		}

		&.expanded .action-icon {
			--expansion: expanded;
		}
	}

	> .expander-child {
		overflow: hidden;

		&:has(> .expander-child-items:empty) {
			display: none;
		}

		> .expander-child-items {
			> * {
				${styledDivider};
				padding-inline-start: ${expanderItemPadding[1]}px;
			}
		}

		${tgs()} {
			block-size: 0;

			> .expander-child-items {
				translate: 0 -100%;
			}
		}
	}
`;

export /* @internal */ default function SubExpander({ icon, title, details, disabled, expanded = false, _requestExpanded, type = "chevron", anchor, actions: _actions, children, onChange: _onChange, ...htmlAttrs }: FCP<{
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
	/** Show chevron or switch as the expanding control. @default "chevron" */
	type?: "chevron" | "switch";
	/** Specify a search anchor landmark. Must be CSS escaped. */
	anchor?: string;
	/** The other action control area on the right side of the component. @warn Useless, not working now. */
	actions?: ReactNode;
}, GenericElement>) {
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
						{...htmlAttrs}
					>
						{title}
					</ToggleSwitch>
				) : (
					<Expander.Item
						title={title}
						icon={icon}
						details={details}
						disabled={disabled}
						anchor={anchor}
						clickable
						className={{ expanded }}
						onClick={() => setExpanded(expanded => !expanded)}
						{...htmlAttrs}
					>
						<div className={["action-icon", TRAILING_EXEMPTION, "expander-chevron"]} data-type={type}>
							<Icon name="chevron_down" />
						</div>
					</Expander.Item>
				)
			}
			<CssTransition in={expanded || requestExpanded} unmountOnExit transitionEndProperty={["height", "block-size"]} requestAnimationFrame>
				<div className="expander-child">
					<div className="expander-child-items">
						{children}
					</div>
				</div>
			</CssTransition>
		</StyledSubExpander>
	);
}
