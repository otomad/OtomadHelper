import ExpanderAequilateTextItems from "./ExpanderAequilateTextItems";
import ExpanderContext from "./ExpanderContext";
import ExpanderGroup from "./ExpanderGroup";
import ExpanderItem from "./ExpanderItem";
import SubExpander from "./SubExpander";

export const TRAILING_EXEMPTION = "trailing-exemption";
const TRANSLATE_TRANSITION_DURATION = 350, TRANSLATE_TRANSITION_DELAY = 75;

const ExpanderParent = styled(SettingsCard)<{ // FIXME: After auto resize, when scrolling page, some content will not display immediately.
	/** Expanded? */
	$expanded?: boolean;
	/** Make expander child items disabled. */
	$childrenDisabled?: boolean;
}>`
	.check-info {
		${tgs()} {
			translate: 0 16px;
			opacity: 0 !important;
		}

		&.enter-active {
			transition: ${fallbackTransitions}, translate ${eases.easeOutElastic} 1250ms;
		}

		&.exit-active {
			transition: ${fallbackTransitions}, opacity ${eases.easeOutSmooth} 250ms;
			transition-delay: ${TRANSLATE_TRANSITION_DELAY}ms;
		}
	}

	${ifProp("$childrenDisabled", css`
		& > .base > .trailing > .action-icon {
			--state: disabled !important;
		}
	`)}

	${({ $expanded }) => {
		const sharpBottom = css`
			border-bottom-right-radius: 0;
			border-bottom-left-radius: 0;
		`;
		return $expanded && css`
			${sharpBottom};
			container: expander-parent / scroll-state;
			position: sticky;
			top: if(
				${ifColorScheme.reduceTransparency} or ${ifColorScheme.contrast}: 0;
				else: -1px;
			);
			z-index: 5;

			> .base {
				${sharpBottom};

				> .trailing > .action-icon {
					--expansion: expanded;
				}
			}
		`;
	}}
`;

const ExpanderChildItems = styled.div`
	background-color: ${c("background-fill-color-card-background-secondary")};
	background-clip: padding-box;
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-block-start-width: 0;
	border-end-start-radius: 3px;
	border-end-end-radius: 3px;

	@layer layout {
		> * {
			background-clip: padding-box;
		}
	}

	> :not(
		:first-child,
		.content + *,
		[style*="display: none"] + *,
		[style*="display:none"] + *
	) {
		border-block-start: 1px solid ${c("stroke-color-divider-stroke-default")};
	}

	> :last-child,
	> .sub-expander:last-child > :last-child:not(.expander-child),
	> .sub-expander:last-child .expander-child-items > :last-child {
		border-end-start-radius: 2px;
		border-end-end-radius: 2px;
	}
`;

const ExpanderChild = styled.div`
	--placeholder-transition-longest-property: 100%;
	inline-size: 100%;

	&:not(.enter-done) {
		overflow: clip;
	}

	&[disabled] {
		opacity: ${c("disabled-text-opacity")};
	}

	${tgs()} {
		--placeholder-transition-longest-property: 0%;
		block-size: 0;
		border-block-end-width: 0;

		${ExpanderChildItems} {
			translate: 0 -100%;
		}
	}

	&,
	.expander-child,
	.expander-child-items { // Also apply styles to sub-expander.
		--transition-options: ${eases.easeInOutMaterialEmphasized} ${TRANSLATE_TRANSITION_DURATION}ms;
		transition:
			block-size var(--transition-options),
			translate var(--transition-options),
			--placeholder-transition-longest-property ${TRANSLATE_TRANSITION_DURATION + TRANSLATE_TRANSITION_DELAY}ms;
	}

	&.enter-active .expander-child-items,
	&.exit-active {
		transition-delay: ${TRANSLATE_TRANSITION_DELAY}ms;
	}
`;

const ExpanderChildWrapper = styled.div<{
	/** Remove the top split line and top padding from the expand child. */
	$noDivider?: boolean;
	/** Override padding inline with presets. */
	$tilePadding?: "tile view" | "subtle button to item" | "standard button to item";
	/** A style that suitable for when expander contains only a single component and even without title, icon and details? */
	$single?: boolean;
}>`
	padding-block: ${expanderItemPadding[0]}px;
	padding-inline: ${expanderItemPadding[1]}px;

	&:has(.timecode-box) {
		--layout: inline;
	}

	${ifProp("$single", css`
		padding: 21px 52px;
	`)}

	${ifProp("$noDivider", css`
		padding-block-start: 0;
		border-block-start-width: 0 !important;
	`)}

	${({ $tilePadding }) => $tilePadding === "tile view" ? css`
		padding-inline: 32px;
	` : $tilePadding === "subtle button to item" ? css`
		padding-inline: 40px;
	` : $tilePadding === "standard button to item" ? css`
		padding-inline: 51px;
	` : undefined}
`;

export default function Expander({ icon, title, details, actions, expanded = false, children, checkInfo, alwaysShowCheckInfo, clipChildren, childrenDisabled, childRole, selectInfo, selectValid, disabled, className, role, trailingGap, dirBasedIcon, anchor, wrapActionsWhenNarrow, _requestExpanded, _isInExpanderGroupAndAutoCollapse, onClickWhenChildrenDisabled, onToggle, ref }: FCP<Override<PropsOf<typeof SettingsCard>, {
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/** Expanded initially? */
	expanded?: boolean;
	/**
	 * The text that displays the selected status of a radio button or checkbox in the expander, which is only displayed when the expander is closed.
	 *
	 * Note: Do not pass boolean value!
	 */
	checkInfo?: Exclude<ReactNode, true>;
	/** Regardless of whether the expander is on or off, the selected display text is always displayed. */
	alwaysShowCheckInfo?: boolean;
	/** Make sure expander children won't exceed the area. */
	clipChildren?: boolean;
	/** Make expander child items disabled. */
	childrenDisabled?: boolean;
	/** Define the role of expander child. */
	childRole?: AriaRole;
	/** @private Request to expanded because user search something inside the expander. */
	_requestExpanded?: TransientValue<boolean>;
	/** @private Is it put in an expander group and the expander group is auto collapsed? */
	_isInExpanderGroupAndAutoCollapse?: true;
	/** Occurs when the expander parent has been clicked where the child items disabled. */
	onClickWhenChildrenDisabled?(): void;
	/** Occurs when the expander expanded or collapsed. */
	onToggle?(expanded: boolean): void;
}>>) {
	const settingsCardProps = { icon, title, details, selectInfo, selectValid, disabled, className, role, trailingGap, dirBasedIcon, anchor, wrapActionsWhenNarrow };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const handleClick = useOnNestedButtonClick(e => !childrenDisabled ? setInternalExpanded(expanded => {
		if (expanded || _isInExpanderGroupAndAutoCollapse) handleStickyCollapse(e, _isInExpanderGroupAndAutoCollapse && !expanded);
		return !expanded;
	}) : onClickWhenChildrenDisabled?.());
	useUpdateEffect(() => setInternalExpanded(expanded), [expanded]);
	useEffect(() => onToggle?.(internalExpanded), [internalExpanded]);
	useEffect(() => { if (disabled || childrenDisabled) setInternalExpanded(false); }, [disabled, childrenDisabled]);
	useTransientValue(_requestExpanded, expanded => { expanded && setInternalExpanded(true); }); // Set to true only, do not set to false.
	const [, setAriaId, withAriaId] = useAriaIdRefState();

	// When the header is sticky and user want to collapse the expander, it will go out of viewport.
	function handleStickyCollapse(e: React.MouseEvent, combo = false) {
		const expanderParent = (e.target as HTMLElement).closest(".expander-parent") as HTMLButtonElement, expander = expanderParent?.parentElement;
		if (!expanderParent || !expander) return;
		setIntervalWithTimes(() => {
			if (expanderParent.offsetTop > expander.offsetTop)
				expander.scrollIntoView({ block: "start", behavior: combo ? "instant" : "auto" });
		}, 10, combo ? TRANSLATE_TRANSITION_DURATION / 10 : 1);
	}

	return (
		<div className="expander">
			<ExpanderParent
				{...settingsCardProps}
				ref={ref}
				type={childrenDisabled ? onClickWhenChildrenDisabled ? "button" : "container-but-button" : "expander"}
				actionIcon="chevron_down"
				ariaIdRef={setAriaId}
				aria-controls={withAriaId("children")}
				aria-expanded={internalExpanded}
				$expanded={internalExpanded}
				$childrenDisabled={childrenDisabled}
				_isExpander
				onClick={handleClick}
			>
				{actions}
				{checkInfo != null && (
					<CssTransition
						in={!internalExpanded || alwaysShowCheckInfo}
						timeout={TRANSLATE_TRANSITION_DURATION + TRANSLATE_TRANSITION_DELAY} // Explicitly specified for better performance.
						hiddenOnExit
						requestAnimationFrame
					>
						<output className={["check-info", TRAILING_EXEMPTION]}>{checkInfo}</output>
					</CssTransition>
				)}
			</ExpanderParent>
			<CssTransition in={internalExpanded} unmountOnExit transitionEndProperty={["--placeholder-transition-longest-property"]} requestAnimationFrame>
				<ExpanderChild
					disabled={disabled || childrenDisabled}
					className={{ clipChildren }}
					role={childRole || undefined}
					id={withAriaId("children")}
					aria-labelledby={withAriaId("title")}
				>
					<ExpanderChildItems>
						<ExpanderContext value={{ place: "children" }}>
							{children}
						</ExpanderContext>
					</ExpanderChildItems>
				</ExpanderChild>
			</CssTransition>
		</div>
	);
}

Expander.Item = ExpanderItem;
Expander.ChildWrapper = ExpanderChildWrapper;
Expander.Group = ExpanderGroup;
Expander.AequilateTextItems = ExpanderAequilateTextItems;
Expander.Context = ExpanderContext;
Expander.Sub = SubExpander;
