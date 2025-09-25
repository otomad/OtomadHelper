import ExpanderAequilateTextItems from "./ExpanderAequilateTextItems";
import ExpanderContext from "./ExpanderContext";
import ExpanderGroup from "./ExpanderGroup";
import ExpanderItem from "./ExpanderItem";

export const TRAILING_EXEMPTION = "trailing-exemption";

const ExpanderParent = styled(SettingsCard)<{ // BUG: After auto resize, when scrolling page, some content will not display immediately.
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
	}

	> .base > .trailing > .action-icon > * {
		${styles.mixins.enableHardware3d()};
	}

	&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active)):active > .base > .trailing > .action-icon > * {
		translate: 0 ${({ $expanded }) => $expanded ? 2 : -2}px;
	}

	${ifProp("$childrenDisabled", css`
		& > .base > .trailing > .action-icon {
			background-color: transparent !important;

			> * {
				color: ${c("fill-color-text-disabled")};
				translate: 0 !important;
			}
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
			top: -1px;
			z-index: 5;

			${ifColorScheme.reduceTransparency} {
				top: 0;
			}

			${ifColorScheme.contrast} & {
				top: 0;
			}

			> .base {
				${sharpBottom};

				> .trailing > .action-icon > * {
					rotate: 180deg;
				}
			}
		`;
	}}
`;

const ExpanderChild = styled.div`
	inline-size: 100%;
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-radius: 0 0 3px 3px;
	border-block-start-width: 0;

	&:not(.enter-done) {
		overflow: clip;
	}

	.expander-child-items {
		background-color: ${c("background-fill-color-card-background-secondary")};
		border-radius: 0 0 2px 2px;

		> :not(:first-child) {
			border-block-start: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}

	&[disabled] {
		opacity: ${c("disabled-text-opacity")};
	}

	${tgs()} {
		block-size: 0;
		border-block-end-width: 0;

		.expander-child-items {
			translate: 0 -100%;
		}
	}

	&,
	.expander-child-items {
		transition-property: block-size, translate;
		transition-duration: 350ms;
		transition-timing-function: ${eases.easeInOutMaterialEmphasized};
	}
`;

const ExpanderChildWrapper = styled.div<{
	/** Remove the top split line and top padding from the expand child. */
	$noDivider?: boolean;
	/** Override padding inline with presets. */
	$tilePadding?: "tile view" | "button to item";
	/** A style that suitable for when expander contains only a single component and even without title, icon and details? */
	$single?: boolean;
}>`
	padding: ${expanderItemPadding[0]}px ${expanderItemPadding[1]}px;

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
		padding-inline: 35px;
	` : $tilePadding === "button to item" ? css`
		padding-inline: 40px;
	` : undefined}
`;

export default function Expander({ icon, title, details, actions, expanded = false, children, checkInfo, alwaysShowCheckInfo, clipChildren, childrenDisabled, childRole, selectInfo, selectValid, disabled, className, role, trailingGap, dirBasedIcon, anchor, _requestExpanded, onClickWhenChildrenDisabled, onToggle, ref }: FCP<Override<PropsOf<typeof SettingsCard>, {
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/** Expanded initially? */
	expanded?: boolean;
	/** The text that displays the selected status of a radio button or checkbox in the expander, which is only displayed when the expander is closed. */
	checkInfo?: ReactNode;
	/** Regardless of whether the expander is on or off, the selected display text is always displayed. */
	alwaysShowCheckInfo?: boolean;
	/** Make sure expander children won't exceed the area. */
	clipChildren?: boolean;
	/** Make expander child items disabled. */
	childrenDisabled?: boolean;
	/** Define the role of expander child. */
	childRole?: AriaRole;
	/** @private Request the expanded because user search something inside the expander. */
	// eslint-disable-next-line @typescript-eslint/no-wrapper-object-types
	_requestExpanded?: Boolean;
	/** Occurs when the expander parent has been clicked where the child items disabled. */
	onClickWhenChildrenDisabled?(): void;
	/** Occurs when the expander expanded or collapsed. */
	onToggle?(expanded: boolean): void;
}>>) {
	const settingsCardProps = { icon, title, details, selectInfo, selectValid, disabled, className, role, trailingGap, dirBasedIcon, anchor };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const handleClick = useOnNestedButtonClick(() => !childrenDisabled ? setInternalExpanded(expanded => !expanded) : onClickWhenChildrenDisabled?.());
	useUpdateEffect(() => setInternalExpanded(expanded), [expanded]);
	useEffect(() => onToggle?.(internalExpanded), [internalExpanded]);
	useEffect(() => { if (disabled || childrenDisabled) setInternalExpanded(false); }, [disabled, childrenDisabled]);
	useEffect(() => { if (_requestExpanded) setInternalExpanded(_requestExpanded.valueOf()); }, [_requestExpanded]);
	const ariaId = useRef<string>(null);
	const withAriaId = (suffix: string) => !ariaId.current ? undefined : ariaId.current + suffix;

	return (
		<div className="expander">
			<ExpanderParent
				{...settingsCardProps}
				ref={ref}
				type={childrenDisabled ? onClickWhenChildrenDisabled ? "button" : "container-but-button" : "expander"}
				actionIcon="chevron_down"
				ariaIdRef={ariaId}
				aria-controls={withAriaId("-child")}
				aria-expanded={internalExpanded}
				$expanded={internalExpanded}
				$childrenDisabled={childrenDisabled}
				onClick={handleClick}
			>
				{actions}
				{checkInfo != null && (
					<CssTransition
						in={!internalExpanded || alwaysShowCheckInfo}
						timeout={350} // Explicitly specified for better performance.
						hiddenOnExit
						requestAnimationFrame
					>
						<output className={["check-info", TRAILING_EXEMPTION]}>{checkInfo}</output>
					</CssTransition>
				)}
			</ExpanderParent>
			<CssTransition in={internalExpanded} unmountOnExit transitionEndProperty={["height", "block-size"]} requestAnimationFrame>
				<ExpanderChild
					disabled={disabled || childrenDisabled}
					className={{ clipChildren }}
					role={childRole || "region"}
					id={withAriaId("-child")}
					aria-labelledby={withAriaId("-title")}
				>
					<div className="expander-child-items">
						<ExpanderContext value={{ place: "children" }}>
							{children}
						</ExpanderContext>
					</div>
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
