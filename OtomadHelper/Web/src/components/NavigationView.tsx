const navButtonSize = { width: 44, height: 40 };
const CONTENT_ITEMS_ASSUMED_COUNT = 20;
const NAV_ITEMS_ASSUMED_COUNT = 20;
const NAV_ITEMS_BOTTOM_ASSUMED_COUNT = 3;

const NavButton = styled(Button).attrs({
	subtle: true,
})`
	position: absolute;
	width: ${navButtonSize.width}px;
	min-width: unset;
	height: ${navButtonSize.height}px;
`;

const StyledTopLeftButtons = styled.div`
	z-index: 10;
	height: ${navButtonSize.height}px;
	margin: 4px 5px 1px 9px;

	&.vertical {
		height: ${navButtonSize.height * 2}px;
		margin-left: 5px;

		${NavButton} {
			width: 52px;
		}
	}

	&:not(.shadow) {
		position: fixed;
		z-index: 11;
	}

	.base {
		position: relative;
	}

	&:not(.vertical) ${NavButton}:nth-of-type(2) {
		top: 0;
		left: ${navButtonSize.width}px;
	}

	&.vertical ${NavButton}:nth-of-type(2) {
		top: ${navButtonSize.height}px;
		left: 0;
	}
`;

function TopLeftButtons({ shadow, paneDisplayMode, canBack = true, onBack, onNavButton }: FCP<{
	/** 是否是影子？ */
	shadow?: boolean;
	/** 导航面板显示模式。 */
	paneDisplayMode: PaneDisplayMode;
	/** 能否返回？ */
	canBack?: boolean;
	/** 点击返回按钮事件。 */
	onBack?: () => void;
	/** 点击汉堡菜单按钮事件。 */
	onNavButton?: () => void;
}>) {
	return (
		<StyledTopLeftButtons className={{ shadow, vertical: paneDisplayMode === "compact" }}>
			{!shadow && (
				<div className="base">
					<NavButton animatedIcon="back" disabled={!canBack} onClick={onBack} />
					<NavButton animatedIcon="global_nav_button" onClick={onNavButton} />
				</div>
			)}
		</StyledTopLeftButtons>
	);
}

const floatUp = keyframes`
	from {
		opacity: 0;
		translate: 0 1rem;
	}
`;

const CONTENT_MARGIN_X = 20;
const TITLE_LINE_HEIGHT = 40;
const COMPACT_WIDTH = 62;

const StyledNavigationView = styled.div<{
	$transitionName: string;
}>`
	${styles.mixins.square("100%")};
	display: flex;

	> * {
		display: flex;
		flex-direction: column;
	}

	> .left {
		flex-shrink: 0;
		width: 320px;
		height: 100%;
		padding-bottom: 4px;

		> * {
			flex-shrink: 0;
		}

		.nav-items {
			flex-shrink: 1;
			height: 100%;
			overflow-y: auto;

			&.overflowing {
				border-bottom: 1px solid ${c("stroke-color-divider-stroke-default")};
			}
		}

		.nav-items,
		.nav-items-bottom {
			overflow-x: hidden;
		}

		&:is(.compact, .minimal):not(.flyout) .nav-items::-webkit-scrollbar {
			width: 0;
		}

		&.compact {
			width: ${COMPACT_WIDTH}px;
		}

		&.minimal {
			width: 0;

			&:not(.flyout) {
				translate: -${COMPACT_WIDTH}px;
			}

			&.flyout .nav-items::-webkit-scrollbar {
				width: 0;
			}
		}

		&.flyout {
			position: fixed;
			z-index: 8;
			background-color: ${c("background-fill-color-acrylic-background-default")};
			border-radius: 0 8px 8px 0;
			outline: 1px solid ${c("stroke-color-surface-stroke-flyout")};
			box-shadow: 0 8px 16px ${c("shadows-flyout")};
			backdrop-filter: blur(60px);
		}
	}

	> .right {
		width: 100%;

		&.hairtail {
			> .title-wrapper,
			> .page-content {
				scrollbar-gutter: stable;

				> * {
					width: 100%;
					max-width: 1000px;
					margin: 0 auto;
				}
			}
		}

		&.show-flyout {
			*,
			::before,
			::after {
				pointer-events: none !important;
			}
		}

		&.minimal > .title-wrapper {
			margin-top: 40px;
		}

		.title-wrapper {
			position: relative;
			flex-shrink: 0;
			margin: 12px 0 8px;
			overflow: hidden;
			font-weight: 600;

			> div {
				display: flex;
				justify-content: space-between;
				align-items: center;
				width: 100%;
				height: ${TITLE_LINE_HEIGHT}px;

				> div {
					${styles.mixins.square("100%")};
				}

				.command-bar {
					flex-shrink: 0;
					height: 100%;
				}
			}
		}

		.title-wrapper .title {
			${styles.effects.text.title};
			position: absolute;
			display: flex;
			gap: 14px;
			align-items: center;
			transition: all ${eases.easeOutSmooth} 750ms;

			* {
				white-space: nowrap;
			}

			${tgs(tgs.exit)} {
				translate: 0 -${TITLE_LINE_HEIGHT}px;
			}

			${tgs(tgs.enter)} {
				translate: 0 ${TITLE_LINE_HEIGHT}px;
			}

			> div {
				display: contents;

				.enter,
				.exit-active {
					translate: 20px;
					opacity: 0;
				}

				.enter-active {
					translate: 0;
					opacity: 1;
					transition-delay: 200ms;
					transition-duration: 300ms;

					&.crumb {
						transition-delay: 300ms;
					}
				}

				.exit-active {
					transition-timing-function: ${eases.easeInMax};

					&.bread-crumb-chevron-right {
						transition-delay: 50ms;
					}
				}

				> .parent {
					color: ${c("fill-color-text-secondary")};

					&:hover {
						color: ${c("foreground-color")};
					}

					&:active {
						color: ${c("fill-color-text-tertiary")};
					}
				}
			}
		}

		.page-content {
			height: 100%;
			overflow: hidden auto;

			&:has(> .enter, > .exit) {
				overflow-y: hidden;
			}

			&:has(.empty-message) {
				overflow: hidden;
			}

			> * > .container {
				display: flex;
				flex-direction: column;
				gap: 6px;
				margin-top: 2px;

				&::after {
					content: "";
					height: 18px;
				}

				.card.media-pool > .base {
					padding: 2px;
				}

				${({ $transitionName }) => forMap(CONTENT_ITEMS_ASSUMED_COUNT, i => css`
					> :nth-child(${i}) {
						animation: ${$transitionName === "jump" ? floatUp : ""}
							300ms ${50 * (i - 1)}ms ${eases.easeOutMax} backwards;
					}
				`)}

				> .contents > * {
					animation: inherit;
				}
			}

			> .exit > .container > * {
				animation: none !important;
			}
		}

		.title-wrapper,
		.page-content {
			padding: 0 ${CONTENT_MARGIN_X}px;
		}
	}

	${() => {
		const selectors = forMap(NAV_ITEMS_ASSUMED_COUNT, i =>
			`&:has(.nav-items .tab-item-wrapper:nth-of-type(${i}) .tab-item:active) .nav-items .tab-item-wrapper:nth-of-type(${i}) .tab-item .animated-icon`);
		selectors.push(...forMap(NAV_ITEMS_BOTTOM_ASSUMED_COUNT, i =>
			`&:has(.nav-items-bottom .tab-item-wrapper:nth-of-type(${i}) .tab-item:active) .nav-items-bottom .tab-item-wrapper:nth-of-type(${i}) .tab-item .animated-icon`));
		return css`
			${selectors.join(", ")} {
				--state: pressed;
			}
		`;
	}}
`;

const StyledPage = styled.main`
	.jump > &.exit {
		translate: 0 -2rem;
		opacity: 0;
		transition: all ${eases.easeOutMax} 83ms;
	}

	.jump > &.enter {
		translate: 0 5rem;
		opacity: 0;
	}

	.jump > &.enter-active {
		translate: 0;
		opacity: 1;
		transition: all ${eases.easeOutMax} 300ms;
	}

	.forward > &.exit,
	.backward > &.exit {
		transition: all ${eases.easeInExpo} 300ms;
	}

	.forward > &.exit,
	.backward > &.enter {
		translate: -20%;
	}

	.forward > &.enter,
	.backward > &.exit {
		translate: 20%;
	}

	.forward > &.enter-active,
	.backward > &.enter-active {
		translate: 0;
		transition: all ${eases.easeOutExpo} 300ms;
	}

	&:has(.empty-message) {
		height: calc(100% - 2px);

		> * {
			height: 100%;
		}
	}
`;

const useWindowWidth = () => {
	const [width, setWidth] = useState(window.innerWidth);
	useEventListener(window, "resize", () => setWidth(window.innerWidth), undefined, [width]);
	return width;
};

function NavigationViewLeftPanel({ paneDisplayMode, isFlyoutShown, customContent, currentNavTab, navItems, navItemsId, flyout, isCompact }: FCP<{
	paneDisplayMode: PaneDisplayMode;
	isFlyoutShown: boolean;
	customContent?: ReactNode;
	currentNavTab: StateProperty<string>;
	navItems: (NavItem | NavBrItem)[];
	navItemsId?: string;
	flyout: boolean;
	isCompact: boolean;
}>) {
	const [isNavItemsOverflowing, setIsNavItemsOverflowing] = useState(false);
	const navItemsEl = useDomRef<HTMLDivElement>();
	const focusable = !flyout && paneDisplayMode === "minimal" ? false : isFlyoutShown === flyout;

	const getNavItemNode = useCallback((item: typeof navItems[number], index: number) => {
		if ("type" in item) return item.type === "hr" ? <hr key={index} /> : undefined;
		const { text, icon, animatedIcon, id } = item;
		return (
			<TabBar.Item
				key={id}
				id={id}
				icon={icon || (!animatedIcon ? "placeholder" : undefined)}
				animatedIcon={animatedIcon}
				focusable={focusable}
			>
				{text}
			</TabBar.Item>
		);
	}, [isFlyoutShown, focusable]);

	useEventListener(window, "resize", () => {
		const navItems = navItemsEl.current;
		if (!navItems) return;
		setIsNavItemsOverflowing(navItems.scrollHeight > navItems.offsetHeight);
	}, { immediate: true });

	const onNavItemsScroll = useCallback<UIEventHandler<HTMLDivElement>>(e => {
		const currentElement = e.currentTarget;
		const { scrollTop, dataset: { navItemsId } } = currentElement;
		if (!navItemsId) return;
		document.querySelectorAll(`[data-nav-items-id="${navItemsId}"]`).forEach(element => {
			if (element === currentElement) return;
			element.scrollTo({ top: scrollTop, behavior: "instant" });
		});
	}, []);

	return (
		<div className={["left", paneDisplayMode, { flyout }]}>
			<TopLeftButtons shadow paneDisplayMode={isCompact ? "compact" : paneDisplayMode} />
			<div ref={navItemsEl} data-nav-items-id={navItemsId} className={["nav-items", { overflowing: isNavItemsOverflowing }]} onScroll={onNavItemsScroll}>
				{customContent}
				<TabBar current={currentNavTab} collapsed={paneDisplayMode === "compact"} vertical>
					{navItems.map((item, index) => {
						if (!item.bottom) return getNavItemNode(item, index);
					})}
				</TabBar>
			</div>
			<div className="nav-items-bottom">
				<TabBar current={currentNavTab} collapsed={paneDisplayMode === "compact"} vertical>
					{navItems.map((item, index) => {
						if (item.bottom) return getNavItemNode(item, index);
					})}
				</TabBar>
			</div>
		</div>
	);
}

const StyledBreadCrumbChevronRight = styled.div`
	${styles.mixins.flexCenter()};
	margin-top: 4px;

	.icon {
		color: ${c("fill-color-text-secondary")};
		font-size: 13px;
	}
`;

const BreadCrumbChevronRight = forwardRef<HTMLDivElement>((_, ref) => (
	<StyledBreadCrumbChevronRight ref={ref}>
		<Icon name="chevron_right" />
	</StyledBreadCrumbChevronRight>
));

interface NavItem {
	/** 标签文本。 */
	text: string;
	/** 图标。 */
	icon?: string;
	/** 动态图标。 */
	animatedIcon?: string;
	/** 标识符。 */
	id: string;
	/** 是否将其放置于导航面板底部。 */
	bottom?: boolean;
}

interface NavBrItem {
	/** 类型：分割线。 */
	type: "hr";
	/** 是否将其放置于导航面板底部。 */
	bottom?: boolean;
}

type PaneDisplayMode = "expanded" | "compact" | "minimal";
const getPaneDisplayMode = (zoom: number = 1): PaneDisplayMode =>
	window.innerWidth < 641 * zoom ? "minimal" :
	window.innerWidth < 1008 * zoom ? "compact" : "expanded";
const usePaneDisplayMode = () => {
	const { getUiScale1 } = useConfigStore().settings;
	const [paneDisplayMode, setPaneDisplayMode] = useState<PaneDisplayMode>(getPaneDisplayMode(getUiScale1()));
	const onResize = () => setPaneDisplayMode(getPaneDisplayMode(getUiScale1()));
	useEventListener(window, "resize", onResize);
	subscribeStoreWithSelector(useConfigStore, c => c.settings.uiScale, onResize);
	return paneDisplayMode;
};

export default function NavigationView({ currentNav, navItems = [], titles, transitionName = "", children, customContent, canBack = true, onBack, commandBar, ...htmlAttrs }: FCP<{
	/** 当前导航页状态参数。 */
	currentNav: StateProperty<string[]>;
	/** 所有导航项。 */
	navItems?: (NavItem | NavBrItem)[];
	/** 面包屑导航标题数组。 */
	titles?: { name: string; link?: string[] }[];
	/** 自定义区域。 */
	customContent?: ReactNode;
	/** 页面过渡名称。 */
	transitionName?: string;
	/** 能否返回？ */
	canBack?: boolean;
	/** 点击返回按钮事件。 */
	onBack?: () => void;
	/** 命令栏，可选。 */
	commandBar?: ReactNode;
}, "div">) {
	const currentNavTab = useStateSelector(currentNav, nav => nav[0], value => [value]);
	const pagePath = currentNav.join("/");
	const responsive = usePaneDisplayMode();
	const [flyoutDisplayMode, setFlyoutDisplayMode] = useState<PaneDisplayMode>("minimal");
	const [isExpandedInExpandedMode, setIsExpandedInExpandedMode] = useState(true);
	const paneDisplayMode: PaneDisplayMode = responsive === "expanded" ?
		isExpandedInExpandedMode ? "expanded" : "compact" : responsive;
	const pageContentEl = useDomRef<HTMLDivElement>();
	const scrollToTop = useCallback(() => pageContentEl.current?.scrollTo({ top: 0, left: 0, behavior: "instant" }), [pageContentEl]);
	const navItemsId = useId();

	const currentNavItem = useMemo(() =>
		navItems.find(item => !("type" in item) && item.id === currentNavTab[0]) as NavItem,
	[currentNav, navItems]);
	titles ??= [{ name: currentNavItem?.text ?? "" }];

	const previousPageTitleKey = useRef<typeof pageTitleKey>();
	const pageTitleKey: [string, number] = [currentNavItem?.id ?? "", new Date().valueOf()];
	if (pageTitleKey[0] === previousPageTitleKey.current?.[0]) pageTitleKey[1] = previousPageTitleKey.current?.[1];
	previousPageTitleKey.current = pageTitleKey;

	const onNavButtonClick = useCallback(() => {
		responsive === "expanded" ?
			setIsExpandedInExpandedMode(expanded => !expanded) :
			setFlyoutDisplayMode(mode => mode === "expanded" ? "minimal" : "expanded");
	}, [responsive, flyoutDisplayMode, isExpandedInExpandedMode]);
	const hideFlyoutNavMenu = useCallback(() => void (flyoutDisplayMode !== "minimal" && setFlyoutDisplayMode("minimal")), [flyoutDisplayMode]);
	useEffect(hideFlyoutNavMenu, [currentNav, useWindowWidth()]);

	return (
		<StyledNavigationView $transitionName={transitionName} {...htmlAttrs}>
			<TopLeftButtons paneDisplayMode={paneDisplayMode} onNavButton={onNavButtonClick} onBack={onBack} canBack={canBack} />
			{forMap(2, i => {
				const isFlyout = i === 2;
				return (
					<NavigationViewLeftPanel
						key={i}
						paneDisplayMode={isFlyout ? flyoutDisplayMode : paneDisplayMode}
						isFlyoutShown={flyoutDisplayMode !== "minimal"}
						currentNavTab={currentNavTab}
						navItems={navItems}
						navItemsId={navItemsId}
						customContent={customContent}
						flyout={isFlyout}
						isCompact={paneDisplayMode === "compact"}
					/>
				);
			})}
			<div
				className={[
					"right",
					"hairtail",
					{
						minimal: paneDisplayMode === "minimal",
						showFlyout: flyoutDisplayMode !== "minimal",
					},
				]}
				onClick={hideFlyoutNavMenu}
			>
				<div className="title-wrapper">
					<div>
						<TransitionGroup>
							<CssTransition key={pageTitleKey.join()}>
								<h1 className="title">
									<TransitionGroup>
										{titles.flatMap((title, i, { length }) => {
											const last = i === length - 1;
											const crumb = (
												<div
													key={i}
													className={["crumb", { parent: !last }]}
													onClick={() => title.link?.length && currentNav[1]?.(title.link)}
												>
													{title.name}
												</div>
											);
											const result = [crumb];
											if (!last) result.push(<BreadCrumbChevronRight key={i + "-chevron"} />);
											return result.map((node, j) =>
												<CssTransition key={i + "-" + j}>{node}</CssTransition>);
										})}
									</TransitionGroup>
								</h1>
							</CssTransition>
						</TransitionGroup>
						<section className="command-bar">
							{commandBar}
						</section>
					</div>
				</div>
				<div className={["page-content", transitionName]} ref={pageContentEl}>
					<SwitchTransition>
						<CssTransition key={pagePath} onExited={scrollToTop}>
							<StyledPage>
								{children}
							</StyledPage>
						</CssTransition>
					</SwitchTransition>
				</div>
			</div>
		</StyledNavigationView>
	);
}
