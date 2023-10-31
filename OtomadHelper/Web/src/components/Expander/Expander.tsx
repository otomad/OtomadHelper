import SettingsCard from "../Settings/SettingsCard";

const ExpanderParent = styled(SettingsCard)<{
	/** 已展开？ */
	$expanded?: boolean;
}>(({ $expanded }) => $expanded && css`
	border-bottom-right-radius: 0;
	border-bottom-left-radius: 0;
`);

const ExpanderChild = styled.div`
	border: 1px solid ${c("black", 10)};
	border-top-width: 0;
	border-radius: 0 0 3px 3px;

	.child-items {
		background-color: ${c("white", 3.26)};

		> :not(:last-child) {
			border-bottom: 1px solid ${c("white", 8.37)};
		}
	}

	${ifColorScheme.light} & {
		border-color: ${c("black", 5.78)};

		.child-items {
			background-color: ${c("#f6f6f6", 50)};

			> :not(:last-child) {
				border-bottom-color: ${c("black", 8.03)};
			}
		}
	}
`;

const Expander: FC<PropsOf<typeof SettingsCard> & {
	/** 展开器右侧的其它操作控件区域。 */
	actions?: ReactNode;
	/** 初始状态下是否已展开？ */
	expanded?: boolean;
}> = ({ icon, heading, caption, actions, expanded = false, children }) => {
	const settingsCardProps = { icon, heading, caption, children: actions };
	const [internalExpanded, setInternalExpanded] = useState(expanded);
	const expanderChildRef = useRef<HTMLDivElement>(null);
	const [onEnter, onExit] = simpleAnimateSize(expanderChildRef, "height", 350, undefined, { startChildTranslate: "0 -100%", clientAdjustment: { endHeight: 1 } }, { endChildTranslate: "0 -100%" });

	return (
		<div>
			<ExpanderParent
				{...settingsCardProps}
				type="expander"
				trailingIcon={internalExpanded ? "chevron_up" : "chevron_down"}
				onClick={() => setInternalExpanded(expanded => !expanded)}
				$expanded={internalExpanded}
			/>
			<Transition
				nodeRef={expanderChildRef}
				in={internalExpanded}
				addEndListener={endListener(expanderChildRef)}
				onEnter={onEnter}
				onExit={onExit}
				unmountOnExit
			>
				<ExpanderChild ref={expanderChildRef}>
					<div className="child-items">
						{children}
					</div>
				</ExpanderChild>
			</Transition>
		</div>
	);
};

export default Expander;
