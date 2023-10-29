const StyledTabItemWrapper = styled.div`
	padding: 1.5px 5px 1.5px;
`;

const StyledTabItem = styled.button.attrs({
	type: "button",
})`
	border-radius: 3px;
	display: flex;
	align-items: center;
	padding: 9px 16px 11px;
	gap: 16px;
	cursor: pointer;
	width: -webkit-fill-available;
	min-height: 40px;
	position: relative;
	overflow-x: hidden;

	&:hover,
	&.active {
		background-color: ${c("white", 6.05)};

		${ifColorScheme.light} & {
			background-color: ${c("black", 3.73)};
		}
	}

	&:not(.active):active,
	&.active:not(:active):hover {
		background-color: ${c("white", 4.19)};

		${ifColorScheme.light} & {
			background-color: ${c("black", 2.41)};
		}
	}

	&:active > * {
		opacity: 0.786;

		${ifColorScheme.light} & {
			opacity: 0.6063;
		}
	}

	${ifColorScheme.light} & {

	}

	.icon {
		display: flex;
		margin-bottom: -1px;
	}

	.text {
		white-space: nowrap;
		line-height: 20px;
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
	/** 是否可被聚焦？ */
	focusable?: boolean;
}, HTMLElement> = ({ icon, children, active, collapsed, id: _id, focusable = true, ...htmlAttrs }) => {
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
			<StyledTabItemWrapper {...htmlAttrs}>
				<StyledTabItem ref={tabItemRef} tabIndex={focusable ? 0 : -1} {...htmlAttrs} className={classNames({ active })}>
					<Icon name={icon} />
					<div className="text">{children}</div>
				</StyledTabItem>
			</StyledTabItemWrapper>
		</Transition>
	);
};

export default TabItem;
