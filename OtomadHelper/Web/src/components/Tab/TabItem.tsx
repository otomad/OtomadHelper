const StyledTabItemWrapper = styled.div`
	.tab-bar.vertical & {
		padding: 1.5px 5px;
	}
`;

const StyledTabItem = styled.button`
	position: relative;
	display: flex;
	align-items: center;
	width: -webkit-fill-available;
	min-height: 30px;
	overflow-x: hidden;
	border-radius: 3px;

	.tab-bar.vertical & {
		gap: 16px;
		padding: 9px 16px 11px;

		&:hover,
		&.selected {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:not(.selected):active,
		&.selected:not(:active):hover {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:active > * {
			opacity: ${c("pressed-text-opacity")}
		}
	}

	.icon,
	.animated-icon {
		display: flex;
		margin-bottom: -1px;
	}

	.animated-icon {
		&,
		* {
			color: ${c("fill-color-text-primary-solid")};
		}
	}

	.text {
		line-height: 20px;
		white-space: nowrap;
	}

	.fill {
		width: 100%;
		text-align: left;
	}

	.badge-wrapper {
		position: relative;

		.badge {
			position: absolute;
			top: 0;
			right: 0;
			translate: 50% -50%;
		}
	}

	.tab-bar.horizontal & {
		gap: 8px;
		padding: 14px 12px;

		&:hover {
			opacity: 0.79; // 使用 opacity 而并非 color 是因为 svg 动画变换速度比文字慢的异常。
		}

		&:active {
			opacity: 0.54;
		}

		${ifColorScheme.light} & {
			&:hover {
				opacity: 0.61;
			}

			&:active {
				opacity: 0.45;
			}
		}
	}

	&:active .animated-icon {
		--state: pressed;
	}

	&.selected .animated-icon {
		--selected: true;
	}
`;

export /* internal */ default function TabItem({ icon, animatedIcon, children, selected = false, collapsed, id: _id, focusable = true, badge, vertical, ...htmlAttrs }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 动态图标。 */
	animatedIcon?: string;
	/** 标识符。 */
	id: string;
	/** 是否已选中？ */
	selected?: boolean;
	/** 是否隐藏文本标签，仅显示图标？ */
	collapsed?: boolean;
	/** 是否可被聚焦？ */
	focusable?: boolean;
	/** 角标。 */
	badge?: string | number;
	/** @private 是否使用纵向的 NavigationView 样式？ */
	vertical?: boolean;
}, "section">) {
	const tabItemEl = useDomRef<"button">();

	const onEnter = async () => {
		const el = tabItemEl.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", false));
		el.dispatchEvent(new Event("transitionend"));
	};

	const onExit = async () => {
		const el = tabItemEl.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", true));
		el.dispatchEvent(new Event("transitionend"));
	};

	const BadgeItem = useCallback(({ hidden }: { hidden?: boolean }) =>
		<Badge status="accent" hidden={badge === undefined || hidden}>{badge}</Badge>, [badge]);

	return (
		<Transition
			nodeRef={tabItemEl}
			in={!collapsed}
			addEndListener={endListener(tabItemEl)}
			onEnter={onEnter}
			onExit={onExit}
		>
			<StyledTabItemWrapper {...htmlAttrs}>
				<StyledTabItem
					type="button"
					ref={tabItemEl}
					tabIndex={focusable ? 0 : -1}
					{...htmlAttrs}
					className={{ selected }}
				>
					{(icon || animatedIcon) && (
						<div className="badge-wrapper">
							{icon && !animatedIcon && <Icon name={icon} />}
							{animatedIcon && <AnimatedIcon name={animatedIcon} />}
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
