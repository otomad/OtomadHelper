import ExpanderItem from "./ExpanderItem";

export const TRAILING_EXEMPTION = "trailing-exemption";

const ExpanderParent = styled(SettingsCard)<{
	/** Expanded? */
	$expanded?: boolean;
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
			transition-timing-function: ${eases.easeOutElastic};
			transition-duration: 1250ms;
		}
	}

	> .base > .trailing > .trailing-icon > * {
		${styles.mixins.enableHardware3d()};
	}

	&:not(:has(.trailing > :not(.${TRAILING_EXEMPTION}):active)):active > .base > .trailing > .trailing-icon > * {
		translate: 0 ${({ $expanded }) => $expanded ? 2 : -2}px;
	}

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
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-top-width: 0;
	border-radius: 0 0 3px 3px;

	.expander-child-items {
		background-color: ${c("background-fill-color-card-background-secondary")};

		> :not(:first-child) {
			border-top: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}

	&[disabled] {
		opacity: ${c("disabled-text-opacity")};
	}
`;

const ExpanderChildWrapper = styled.div`
	padding: 7px 51px;

	&:has(.slider) {
		padding: 21px 51px;
	}

	&:has(.timecode-box) {
		--layout: inline;
	}
`;

export default function Expander({ icon, title, details, actions, expanded = false, children, checkInfo, alwaysShowCheckInfo, selectInfo, selectValid, disabled }: FCP<PropsOf<typeof SettingsCard> & {
	/** The other action control area on the right side of the expander. */
	actions?: ReactNode;
	/** Expanded initially? */
	expanded?: boolean;
	/** The text that displays the selected status of a radio button or checkbox in the expander, which is only displayed when the expander is closed. */
	checkInfo?: Readable;
	/** Regardless of whether the expander is on or off, the selected display text is always displayed. */
	alwaysShowCheckInfo?: boolean;
}>) {
	const settingsCardProps = { icon, title, details, selectInfo, selectValid, disabled };
	const [internalExpanded, setInternalExpanded] = useState(expanded);

	const handleClick = useCallback<MouseEventHandler>(e => {
		const path = getPath(e);
		const currentTargetIndex = path.indexOf(e.currentTarget);
		if (path.slice(0, currentTargetIndex).find(element => element.tagName === "BUTTON")) return;
		setInternalExpanded(expanded => !expanded);
	}, []);

	return (
		<div className="expander">
			<ExpanderParent
				{...settingsCardProps}
				type="expander"
				trailingIcon="chevron_down"
				onClick={handleClick}
				$expanded={internalExpanded}
			>
				{actions}
				{checkInfo != null && <div className={["check-info", TRAILING_EXEMPTION, { hidden: !(!internalExpanded || alwaysShowCheckInfo) }]}>{checkInfo}</div>}
			</ExpanderParent>
			<Transitions.Size
				in={internalExpanded}
				specified="height"
				duration={350}
				enterOptions={{ startChildTranslate: "0 -100%", clientAdjustment: { endHeight: 1 } }}
				exitOptions={{ endChildTranslate: "0 -100%" }}
			>
				<ExpanderChild disabled={disabled}>
					<div className="expander-child-items">
						{children}
					</div>
				</ExpanderChild>
			</Transitions.Size>
		</div>
	);
}

Expander.Item = ExpanderItem;
Expander.ChildWrapper = ExpanderChildWrapper;
