import SettingsCard from "../Settings/SettingsCard";

const ExpanderParent = styled(SettingsCard)<{
	/** 已展开？ */
	$expanded?: boolean;
}>(({ $expanded }) => $expanded && css`
	border-bottom-right-radius: 0;
	border-bottom-left-radius: 0;
`);

const ExpanderChild = styled.div`
	background-color: ${c("white", 3.26)};
	outline: 1px solid ${c("black", 10)};
	margin: 1px;
	margin-top: 0;
	border-radius: 0 0 3px 3px; // FIXME: border-radius 会造成 exit 动画短暂异常，暂不清楚原因。

	.child-items > :not(:last-child) {
		border-bottom: 1px solid ${c("white", 8.37)};
	}

	${ifColorScheme.light} & {
		background-color: ${c("#f6f6f6", 50)};
		outline: 1px solid ${c("black", 5.78)};

		.child-items > :not(:last-child) {
			border-bottom-color: ${c("black", 8.03)};
		}
	}
`;

const Expander: FC<PropsOf<typeof SettingsCard> & {
	actions?: ReactNode;
}> = ({ icon, heading, caption, actions, children }) => {
	const settingsCardProps = { icon, heading, caption, children: actions };
	const [expanded, setExpanded] = useState(false);
	const expanderChildRef = useRef<HTMLDivElement>(null);
	// const [onEnter, onExit] = simpleAnimateSize(expanderChildRef, "height", undefined, undefined, { startChildTranslate: "0 -100%" }, { endChildTranslate: "0 -100%" });

	const onEnter = async () => {
		const el = expanderChildRef.current;
		if (!el) return;
		await animateSize(el, null, { specified: "height", startHeight: 0, startChildTranslate: "0 -100%" });
		el.dispatchEvent(new Event("transitionend"));
	};

	const onExit = async () => {
		const el = expanderChildRef.current;
		if (!el) return;
		await animateSize(el, null, { specified: "height", endHeight: 0, endChildTranslate: "0 -100%" });
		el.dispatchEvent(new Event("transitionend"));
		el.hidden = true;
	};

	return (
		<div>
			<ExpanderParent
				{...settingsCardProps}
				type="expander"
				trailingIcon={expanded ? "chevron_up" : "chevron_down"}
				onClick={() => setExpanded(expanded => !expanded)}
				$expanded={expanded}
			/>
			<Transition
				nodeRef={expanderChildRef}
				in={expanded}
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
