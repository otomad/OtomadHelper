import ExpanderItem from "./ExpanderItem";

const ExpanderParent = styled(SettingsCard)<{
	/** 已展开？ */
	$expanded?: boolean;
}>`
	backdrop-filter: blur(4px);

	.check-info {
		${tgs()} {
			translate: 0 16px;
			opacity: 0;
		}

		&.enter-active {
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
					rotate: -180deg;
				}
			}
		`;
	}}
`;

const ExpanderChild = styled.div`
	overflow: clip;
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-top-width: 0;
	border-radius: 0 0 3px 3px;

	.expander-child-items {
		background-color: ${c("background-fill-color-card-background-secondary")};

		> :not(:first-child) {
			border-top: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}
`;

const ExpanderChildWrapper = styled.div`
	padding: 21px 51px;
`;

export default function Expander({ icon, title, details, actions, expanded = false, children, checkInfo, alwaysShowCheckInfo, selectInfo }: FCP<PropsOf<typeof SettingsCard> & {
	/** 展开器右侧的其它操作控件区域。 */
	actions?: ReactNode;
	/** 初始状态下是否已展开？ */
	expanded?: boolean;
	/** 展开器中的单选框或复选框的选中情况的显示文本，仅在展开器关闭时才会显示。 */
	checkInfo?: string;
	/** 无论展开器开启或关闭，一律显示选中情况的显示文本。 */
	alwaysShowCheckInfo?: boolean;
}>) {
	const settingsCardProps = { icon, title, details, selectInfo };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const expanderChildEl = useDomRef<"div">();
	const [onEnter, onExit, endListener] = simpleAnimateSize(expanderChildEl, "height", 350, undefined, { startChildTranslate: "0 -100%", clientAdjustment: { endHeight: 1 } }, { endChildTranslate: "0 -100%" });

	return (
		<div className="expander">
			<ExpanderParent
				{...settingsCardProps}
				type="expander"
				trailingIcon="chevron_down"
				onClick={() => setInternalExpanded(expanded => !expanded)}
				$expanded={internalExpanded}
			>
				{actions}
				{checkInfo && (
					<CssTransition in={!internalExpanded || alwaysShowCheckInfo} unmountOnExit>
						<div className={["check-info", TRAILING_EXEMPTION]}>{checkInfo}</div>
					</CssTransition>
				)}
			</ExpanderParent>
			<Transition
				in={internalExpanded}
				addEndListener={endListener}
				onEnter={onEnter}
				onExit={onExit}
				unmountOnExit
			>
				<ExpanderChild ref={expanderChildEl}>
					<div className="expander-child-items">
						{children}
					</div>
				</ExpanderChild>
			</Transition>
		</div>
	);
}

Expander.Item = ExpanderItem;
Expander.ChildWrapper = ExpanderChildWrapper;
