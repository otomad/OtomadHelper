const StyledTabItem = styled.button.attrs({
	type: "button",
})`
	border-radius: 3px;
	margin: 3px 5px 4px;
	display: flex;
	align-items: center;
	padding: 9px 16px 11px;
	gap: 16px;
	cursor: pointer;
	width: -webkit-fill-available;
	min-height: 40px;
	position: relative;

	&:hover,
	&.active {
		background-color: ${c("white", 6)};
	}

	&:not(.active):active,
	&.active:not(:active):hover {
		background-color: ${c("white", 4)};
	}

	&:active > * {
		opacity: 0.79;
	}

	.icon {
		display: flex;
		margin-bottom: -1px;
	}

	.text {
		white-space: nowrap;
		line-height: 20px;
	}

	// 允许点击元素外边距。
	&::before {
		content: "";
		position: absolute;
		inset: -1.5px 0;
	}
`;

const TabItem: FC<{
	/** 图标。 */
	icon: string;
	/** 标识符。 */
	id: string;
	/** 是否活跃状态？ */
	active?: boolean;
}, HTMLButtonElement> = ({ icon, children, active, id: _id, ...htmlAttrs }) => {
	const textRef = useRef<HTMLDivElement>(null);
	const [showText, setShowText] = useState(true);
	const [onEnter, onExit] = simpleAnimateSize(textRef, "width");

	useEventListener(window, "resize", () => {
		setShowText(window.innerWidth >= 865);
	});

	return (
		<StyledTabItem tabIndex={0} {...htmlAttrs} className={classNames({ active })}>
			<Icon name={icon} />
			<Transition nodeRef={textRef} in={showText} addEndListener={endListener(textRef)} onEnter={onEnter} onExit={onExit} unmountOnExit>
				<div className="text" ref={textRef}>{children}</div>
			</Transition>
		</StyledTabItem>
	);
};

export default TabItem;
