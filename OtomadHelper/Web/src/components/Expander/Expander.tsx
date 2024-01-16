const ExpanderParent = styled(SettingsCard)<{
	/** 已展开？ */
	$expanded?: boolean;
}>`
	.check-info {
		font-size: 14px;

		&.exit-active,
		&.exit-done {
			opacity: 0;
			translate: 0 16px;
		}

		&.enter-active,
		&.enter-done {
			opacity: 1;
			translate: 0;
		}
	}

	${({ $expanded }) => {
		const sharpBottom = css`
			border-bottom-right-radius: 0;
			border-bottom-left-radius: 0;
		`;
		return $expanded && css`
			${sharpBottom};

			> .base {
				${sharpBottom};
			}
		`;
	}}
`;

export /* internal */ const abcdef = 123456;

const ExpanderChild = styled.div`
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-top-width: 0;
	border-radius: 0 0 3px 3px;
	overflow: hidden;

	.expander-child-items {
		background-color: ${c("background-fill-color-card-background-secondary")};

		> :not(:last-child) {
			border-bottom: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}
`;

export default function Expander({ icon, heading, caption, actions, expanded = false, children, checkInfo }: FCP<PropsOf<typeof SettingsCard> & {
	/** 展开器右侧的其它操作控件区域。 */
	actions?: ReactNode;
	/** 初始状态下是否已展开？ */
	expanded?: boolean;
	/** 展开器中的单选框或复选框的选中情况的显示文本，仅在展开器关闭时才会显示。 */
	checkInfo?: string;
}>) {
	const settingsCardProps = { icon, heading, caption };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const expanderChildRef = useRef<HTMLDivElement>(null);
	const [onEnter, onExit, endListener] = simpleAnimateSize(expanderChildRef, "height", 350, undefined, { startChildTranslate: "0 -100%", clientAdjustment: { endHeight: 1 } }, { endChildTranslate: "0 -100%" });

	return (
		<div>
			<ExpanderParent
				{...settingsCardProps}
				type="expander"
				trailingIcon={internalExpanded ? "chevron_up" : "chevron_down"}
				onClick={() => setInternalExpanded(expanded => !expanded)}
				$expanded={internalExpanded}
			>
				{actions}
				{checkInfo && (
					<CssTransition in={!internalExpanded}>
						<div className="check-info">{checkInfo}</div>
					</CssTransition>
				)}
			</ExpanderParent>
			<Transition
				nodeRef={expanderChildRef}
				in={internalExpanded}
				addEndListener={endListener}
				onEnter={onEnter}
				onExit={onExit}
				unmountOnExit
			>
				<ExpanderChild ref={expanderChildRef}>
					<div className="expander-child-items">
						{children}
					</div>
				</ExpanderChild>
			</Transition>
		</div>
	);
}
