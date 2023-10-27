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
	overflow: hidden;

	&.collapsed {
		width: 48px;
	}

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
	/** 是否隐藏文本标签，仅显示图标？ */
	collapsed?: boolean;
}, HTMLButtonElement> = ({ icon, children, active, collapsed, id: _id, ...htmlAttrs }) => {
	const tabItemRef = useRef<HTMLButtonElement>(null);

	const onEnter = async () => {
		const el = tabItemRef.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", false));
		el.dispatchEvent(new Event("transitionend"));
	};

	const onExit = async () => {
		const el = tabItemRef.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", true));
		el.dispatchEvent(new Event("transitionend"));
	};

	return (
		<Transition
			nodeRef={tabItemRef}
			in={!collapsed}
			addEndListener={endListener(tabItemRef)}
			onEnter={onEnter}
			onExit={onExit}
		>
			<StyledTabItem ref={tabItemRef} tabIndex={0} {...htmlAttrs} className={classNames({ active })}>
				<Icon name={icon} />
				<div className="text">{children}</div>
			</StyledTabItem>
		</Transition>
	);
};

export default TabItem;
