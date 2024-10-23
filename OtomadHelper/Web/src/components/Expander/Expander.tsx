import ExpanderItem from "./ExpanderItem";

export const TRAILING_EXEMPTION = "trailing-exemption";

const ExpanderParent = styled(SettingsCard)<{
	/** Expanded? */
	$expanded?: boolean;
	/** Make expander child items disabled. */
	$childrenDisabled?: boolean;
}>`
	backdrop-filter: blur(4px);

	.check-info {
		// Check info should still take up space when hidden, to avoid layout jumps during the toggle animation.
		transition-behavior: allow-discrete;

		&.hidden {
			translate: 0 16px;
			opacity: 0;
			visibility: hidden;
		}

		&:not(.hidden) {
			transition: ${fallbackTransitions}, translate ${eases.easeOutElastic} 1250ms;
		}
	}

	> .base > .trailing > .trailing-icon > * {
		${styles.mixins.enableHardware3d()};
	}

	&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active)):active > .base > .trailing > .trailing-icon > * {
		translate: 0 ${({ $expanded }) => $expanded ? 2 : -2}px;
	}

	${ifProp("$childrenDisabled", css`
		& > .base > .trailing > .trailing-icon > * {
			color: ${c("fill-color-text-disabled")};
			translate: 0 !important;
		}
	`)}

	${({ $expanded }) => {
		const sharpBottom = css`
			border-bottom-right-radius: 0;
			border-bottom-left-radius: 0;
		`;
		return $expanded && css`
			${sharpBottom};

			position: sticky;
			top: -1px;
			z-index: 5;

			> .base {
				${sharpBottom};

				> .trailing > .trailing-icon > * {
					rotate: 180deg;
				}
			}
		`;
	}}
`;

const ExpanderChild = styled.div`
	inline-size: 100%;
	overflow: clip;
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-top-width: 0;
	border-radius: 0 0 3px 3px;

	.expander-child-items {
		background-color: ${c("background-fill-color-card-background-secondary")};
		border-radius: 0 0 2px 2px;

		> :not(:first-child) {
			border-top: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}

	&[disabled] {
		opacity: ${c("disabled-text-opacity")};
	}

	${tgs()} {
		block-size: 0;
		border-bottom-width: 0;

		.expander-child-items {
			translate: 0 -100%;
		}
	}

	&,
	.expander-child-items {
		transition: ${fallbackTransitions}, block-size ${eases.easeOutSmooth} 350ms, translate ${eases.easeOutSmooth} 350ms;
	}
`;

const ExpanderChildWrapper = styled.div`
	padding: 7px 38px;

	&:has(.slider) {
		padding: 21px 52px;
	}

	&:has(.timecode-box) {
		--layout: inline;
	}
`;

export default function Expander({ icon, title, details, actions, expanded = false, children, checkInfo, alwaysShowCheckInfo, clipChildren, childrenDisabled, selectInfo, selectValid, disabled }: FCP<PropsOf<typeof SettingsCard> & {
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/** Expanded initially? */
	expanded?: boolean;
	/** The text that displays the selected status of a radio button or checkbox in the expander, which is only displayed when the expander is closed. */
	checkInfo?: Readable;
	/** Regardless of whether the expander is on or off, the selected display text is always displayed. */
	alwaysShowCheckInfo?: boolean;
	/** Make sure expander children won't exceed the area. */
	clipChildren?: boolean;
	/** Make expander child items disabled. */
	childrenDisabled?: boolean;
}>) {
	const settingsCardProps = { icon, title, details, selectInfo, selectValid, disabled };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const handleClick = useOnNestedButtonClick(() => !childrenDisabled && setInternalExpanded(expanded => !expanded));
	useEffect(() => setInternalExpanded(expanded), [expanded]);
	useEffect(() => { if (disabled || childrenDisabled) setInternalExpanded(false); }, [disabled, childrenDisabled]);

	return (
		<div className="expander">
			<ExpanderParent
				{...settingsCardProps}
				type={childrenDisabled ? "container-but-button" : "expander"}
				trailingIcon="chevron_down"
				onClick={handleClick}
				$expanded={internalExpanded}
				$childrenDisabled={childrenDisabled}
			>
				{actions}
				{checkInfo != null && <div className={["check-info", TRAILING_EXEMPTION, { hidden: !(!internalExpanded || alwaysShowCheckInfo) }]}>{checkInfo}</div>}
			</ExpanderParent>
			<CssTransition in={internalExpanded} unmountOnExit>
				<ExpanderChild disabled={disabled || childrenDisabled} className={{ clipChildren }}>
					<div className="expander-child-items">
						{children}
					</div>
				</ExpanderChild>
			</CssTransition>
		</div>
	);
}

Expander.Item = ExpanderItem;
Expander.ChildWrapper = ExpanderChildWrapper;
