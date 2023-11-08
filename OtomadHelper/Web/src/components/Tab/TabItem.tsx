const StyledTabItemWrapper = styled.div`
	.tab-bar.vertical & {
		padding: 1.5px 5px 1.5px;
	}
`;

const StyledTabItem = styled.button`
	border-radius: 3px;
	display: flex;
	align-items: center;
	width: -webkit-fill-available;
	min-height: 30px;
	position: relative;
	overflow-x: hidden;

	.tab-bar.vertical & {
		padding: 9px 16px 11px;
		gap: 16px;
		
		&:hover,
		&.active {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:not(.active):active,
		&.active:not(:active):hover {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:active > * {
			opacity: ${c("pressed-text-opacity")}
		}
	}

	.icon {
		display: flex;
		margin-bottom: -1px;
	}

	.text {
		white-space: nowrap;
		line-height: 20px;
	}

	.fill {
		width: 100%;
		text-align: left;
	}

	.badge-wrapper {
		position: relative;

		.badge {
			position: absolute;
			right: 0;
			top: 0;
			translate: 50% -50%;
		}
	}

	.tab-bar.horizontal & {
		padding: 14px 12px;
		gap: 8px;

		&:hover {
			color: ${c("fill-color-text-secondary")};
		}

		&:active {
			color: ${c("fill-color-text-tertiary")};
		}
	}
`;

export default function TabItem({ icon, children, active, collapsed, id: _id, focusable = true, badge, vertical, ...htmlAttrs }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 标识符。 */
	id: string;
	/** 是否活跃状态？ */
	active?: boolean;
	/** 是否隐藏文本标签，仅显示图标？ */
	collapsed?: boolean;
	/** 是否可被聚焦？ */
	focusable?: boolean;
	/** 角标。 */
	badge?: string | number;
	/** 是否使用纵向的 NavigationView 样式？ */
	vertical?: boolean;
}, HTMLElement>) {
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

	const BadgeItem = useCallback(({ hidden }: { hidden?: boolean }) =>
		<Badge status="asterisk" hidden={badge === undefined || hidden}>{badge}</Badge>, [badge]);

	return (
		<Transition
			nodeRef={tabItemRef}
			in={!collapsed}
			addEndListener={endListener(tabItemRef)}
			onEnter={onEnter}
			onExit={onExit}
		>
			<StyledTabItemWrapper {...htmlAttrs}>
				<StyledTabItem
					type="button"
					ref={tabItemRef}
					tabIndex={focusable ? 0 : -1}
					{...htmlAttrs}
					className={{ active }}
				>
					{icon && (
						<div className="badge-wrapper">
							<Icon name={icon} />
							<BadgeItem hidden={!(vertical && collapsed)} />
						</div>
					)}
					<div className="badge-wrapper fill">
						<div className="text">{children}</div>
						{!vertical && <BadgeItem />}
					</div>
					{vertical && <BadgeItem />}
				</StyledTabItem>
			</StyledTabItemWrapper>
		</Transition>
	);
}
