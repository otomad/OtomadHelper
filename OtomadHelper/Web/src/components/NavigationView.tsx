import type { PageScroll } from "stores/page";

const navButtonSize = { width: 44, height: 40 };
const CONTENT_ITEMS_ASSUMED_COUNT = 20;
const NAV_ITEMS_ASSUMED_COUNT = 20;
const NAV_ITEMS_BOTTOM_ASSUMED_COUNT = 3;

const NavButton = styled(Button).attrs({
	subtle: true,
})`
	position: absolute;
	width: ${navButtonSize.width}px;
	height: ${navButtonSize.height}px;
	min-inline-size: unset;
`;

const StyledTopLeftButtons = styled.div`
	z-index: 10;
	height: ${navButtonSize.height}px;
	margin-block: 4px 1px;
	margin-inline: 9px 5px;

	&.vertical {
		height: ${navButtonSize.height * 2}px;
		margin-inline-start: 5px;

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

	&:not(.vertical) :nth-of-type(2) > ${NavButton} {
		inset-block-start: 0;
		inset-inline-start: ${navButtonSize.width}px;
	}

	&.vertical :nth-of-type(2) > ${NavButton} {
		inset-block-start: ${navButtonSize.height}px;
		inset-inline-start: 0;
	}
`;

function TopLeftButtons({ shadow, paneDisplayMode, canBack = true, onBack, onNavButton }: FCP<{
	/** Is it a shadow? */
	shadow?: boolean;
	/** Navigation panel display mode. */
	paneDisplayMode: PaneDisplayMode;
	/** Can go back? */
	canBack?: boolean;
	/** Back button click event. */
	onBack?(): void;
	/** Global navigation button click event. */
	onNavButton?(): void;
}>) {
	const vertical = paneDisplayMode === "compact";
	const tooltipPlacement: Placement = vertical ? "right" : "bottom";

	useEventListener(window, "keydown", e => {
		if (e.altKey && e.code === "ArrowLeft") onBack?.();
		else if (e.altKey && e.code === "KeyH") onNavButton?.();
	});

	const TooltipTitle = useCallback(({ title, shortcut }: { title: string; shortcut: string }) =>
		<>{title}<code style={{ marginLeft: "0.25em" }}>({shortcut})</code></>, []);

	return (
		<StyledTopLeftButtons className={{ shadow, vertical }}>
			{!shadow && (
				<div className="base">
					<Tooltip placement={tooltipPlacement} title={<TooltipTitle title={t.back} shortcut="Alt + ←" />}>
						<NavButton animatedIcon="back" disabled={!canBack} onClick={onBack} aria-label="Back" dirBased />
					</Tooltip>
					<Tooltip placement={tooltipPlacement} title={<TooltipTitle title={t.navigation} shortcut="Alt + H" />}>
						<NavButton animatedIcon="global_nav_button" onClick={onNavButton} aria-label="Navigation" />
					</Tooltip>
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
		max-width: calc(100dvw / var(--zoom));
		height: 100%;
		padding-bottom: 4px;

		@media (horizontal-viewport-segments >= 2) {
			width: calc((env(viewport-segment-left 1 0) - env(viewport-segment-left 0 0)) / var(--zoom, 1));

			&.expanded:not(.flyout) {
				padding-inline-end: calc((env(viewport-segment-left 1 0) - env(viewport-segment-right 0 0)) / var(--zoom, 1));
			}

			&.expanded.flyout {
				width: calc((env(viewport-segment-right 0 0) - env(viewport-segment-left 0 0)) / var(--zoom, 1));
			}
		}

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

		&:is(.compact, .minimal):not(.flyout) .nav-items {
			${styles.mixins.noScrollbar()};
		}

		&.compact {
			width: ${COMPACT_WIDTH}px;
		}

		&.minimal {
			width: 0;

			&:not(.flyout) {
				translate: -${COMPACT_WIDTH}px;
			}

			&.flyout {
				outline-width: 0;

				.nav-items {
					${styles.mixins.noScrollbar()};
				}
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
			transition-behavior: allow-discrete;
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
			transition: all ${eases.easeInOutMaterialEmphasized} 700ms;

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
					transition-duration: 300ms;
					transition-delay: 200ms;

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
			overscroll-behavior: contain;

			&:has(> .enter, > .exit) {
				overflow-y: hidden;
			}

			> * > .container {
				display: flex;
				flex-direction: column;
				gap: 6px;
				width: 100%;
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
				`, 1)}

				> .contents > * {
					animation: inherit;
				}

				> div:is(:not([class]), .stack-panel):has(> button) {
					display: flex;
					flex-wrap: wrap;
					gap: 8px;

					> button {
						min-inline-size: 120px;
					}
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
			`&:has(.nav-items .tooltip-child-wrapper:nth-of-type(${i}) .tab-item-wrapper .tab-item:active) .nav-items .tooltip-child-wrapper:nth-of-type(${i}) .tab-item-wrapper .tab-item .animated-icon`, 1);
		selectors.push(...forMap(NAV_ITEMS_BOTTOM_ASSUMED_COUNT, i =>
			`&:has(.nav-items-bottom .tooltip-child-wrapper:nth-of-type(${i}) .tab-item-wrapper .tab-item:active) .nav-items-bottom .tooltip-child-wrapper:nth-of-type(${i}) .tab-item-wrapper .tab-item .animated-icon`, 1));
		return css`
			${selectors.join(", ")} {
				--state: pressed;
			}
		`;
	}}
`;

const StyledPage = styled.main`
	container: page / inline-size;
	display: flex;
	min-height: 100%;

	&.exit {
		pointer-events: none; // Prevent users from quickly clicking buttons to enter sub-pages.
	}

	&.exit-done {
		display: none;
	}

	// #region Page transitions
	.jump > &:is(.exit, .exit-done) {
		translate: 0 -2rem;
		opacity: 0;
		transition: all ${eases.easeInExpo} 150ms;
	}

	.jump > &.enter {
		translate: 0 5rem;
		opacity: 0;
	}

	.jump > &.enter-active {
		translate: 0;
		opacity: 1;
		transition: all ${eases.easeOutExpo} 500ms;
	}

	.forward > &.exit,
	.backward > &.exit {
		transition: all ${eases.easeInExpo} 300ms;
	}

	.forward > &:is(.exit, .exit-done),
	.backward > &.enter {
		translate: -20%;

		&:dir(rtl) {
			translate: 20%;
		}
	}

	.forward > &.enter,
	.backward > &:is(.exit, .exit-done) {
		translate: 20%;

		&:dir(rtl) {
			translate: -20%;
		}
	}

	.forward > &.enter-active,
	.backward > &.enter-active {
		translate: 0 !important;
		transition: all ${eases.easeOutExpo} 300ms;
	}
	// #endregion
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
	const navItemsEl = useDomRef<"div">();
	const focusable = !flyout && paneDisplayMode === "minimal" ? false : isFlyoutShown === flyout;

	const getNavItemNode = useCallback((item: typeof navItems[number], index: number) => {
		if ("type" in item) return item.type === "hr" ? <hr key={index} /> : undefined;
		const { text, icon, animatedIcon, id, badge } = item;
		return (
			<TabBar.Item
				key={id}
				id={id}
				icon={icon || (!animatedIcon ? "placeholder" : undefined)}
				animatedIcon={animatedIcon}
				focusable={focusable}
				badge={badge}
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
		font-size: 16px;
	}
`;

const BreadCrumbChevronRight = forwardRef<HTMLDivElement>((_, ref) => (
	<StyledBreadCrumbChevronRight ref={ref}>
		<Icon name="chevron_right" />
	</StyledBreadCrumbChevronRight>
));

interface NavItem {
	/** Label text. */
	text: string;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Animated icon. */
	animatedIcon?: DeclaredLotties;
	/** Identifier. */
	id: string;
	/** Place it at the bottom of the navigation panel? */
	bottom?: boolean;
	/** Show the badge, or **beacon** by `true`. */
	badge?: BadgeArgs;
}

interface NavBrItem {
	/** Type: dividing line. */
	type: "hr";
	/** Place it at the bottom of the navigation panel? */
	bottom?: boolean;
}

type PaneDisplayMode = "expanded" | "compact" | "minimal";
const getPaneDisplayMode = (zoom: number = 1): PaneDisplayMode =>
	window.innerWidth < 641 * zoom ? "minimal" :
	window.innerWidth < 1008 * zoom ? "compact" : "expanded";
const usePaneDisplayMode = () => {
	const { uiScale1 } = useSnapshot(configStore.settings);
	const [paneDisplayMode, setPaneDisplayMode] = useState<PaneDisplayMode>(getPaneDisplayMode(configStore.settings.uiScale1));
	const onResize = () => setPaneDisplayMode(getPaneDisplayMode(configStore.settings.uiScale1));
	useEventListener(window, "resize", onResize);
	useEffect(() => onResize(), [uiScale1]);
	// subscribeStoreKey(configStore.settings, "uiScale", onResize);
	return paneDisplayMode;
};

export default function NavigationView({ currentNav, navItems = [], titles, transitionName = "", children, customContent, canBack = true, onBack, commandBar, pageContentId, poppedScroll, ...htmlAttrs }: FCP<{
	/** Current navigation page status parameters. */
	currentNav: StateProperty<string[]>;
	/** All navigation items. */
	navItems?: (NavItem | NavBrItem)[];
	/** Array of breadcrumb navigation titles. */
	titles?: { name: string; link?: string[] }[];
	/** Custom content area. */
	customContent?: ReactNode;
	/** Page transition name. */
	transitionName?: string;
	/** Can go back? */
	canBack?: boolean;
	/** Back button click event. */
	onBack?(): void;
	/** Command bar, optional. */
	commandBar?: ReactNode;
	/** Manually specify the identifier for the page content element. */
	pageContentId?: string;
	/** The page scroll value popped from the stack. */
	poppedScroll?: PageScroll;
}, "div">) {
	const currentNavTab = useStateSelector(currentNav, nav => nav[0], value => [value]);
	const pagePath = currentNav.join("/");
	const responsive = usePaneDisplayMode();
	const [flyoutDisplayMode, setFlyoutDisplayMode] = useState<PaneDisplayMode>("minimal");
	const [isExpandedInExpandedMode, setIsExpandedInExpandedMode] = useState(true);
	const paneDisplayMode: PaneDisplayMode = responsive === "expanded" ?
		isExpandedInExpandedMode ? "expanded" : "compact" : responsive;
	const pageContentEl = useDomRef<"div">();
	const scrollToTopOrPrevious = () => {
		const pageContent = pageContentEl.current;
		if (!pageContent) return;
		const container = pageContent.lastElementChild?.firstElementChild;
		while (poppedScroll && container?.classList.contains("container")) { // Cheat `if` as `while` to use `break` in it.
			let child = container.children[poppedScroll.elementIndex] as HTMLElement | undefined;
			while (isElementContents(child))
				child = child!.firstElementChild as HTMLElement;
			if (isElementHidden(child)) break;
			let { offsetY } = poppedScroll;
			if (child.offsetHeight < offsetY) offsetY = child.offsetHeight;
			child.scrollIntoView({ behavior: "instant" });
			pageContent.scrollBy({ top: offsetY, behavior: "instant" });
			return;
		}
		pageContent.scrollTo({ top: 0, left: 0, behavior: "instant" });
	};
	const navItemsId = useId();

	const currentNavItem = useMemo(() =>
		navItems.find(item => !("type" in item) && item.id === currentNavTab[0]) as NavItem,
	[currentNav, navItems]);
	titles ??= [{ name: currentNavItem?.text ?? "" }];

	const previousPageTitleKey = useRef<typeof pageTitleKey>();
	const pageTitleKey: [string, number] = [currentNavItem?.id ?? "", new Date().valueOf()];
	if (pageTitleKey[0] === previousPageTitleKey.current?.[0]) pageTitleKey[1] = previousPageTitleKey.current?.[1];
	previousPageTitleKey.current = pageTitleKey;

	const onNavButtonClick = () => responsive === "expanded" ?
		setIsExpandedInExpandedMode(expanded => !expanded) :
		setFlyoutDisplayMode(mode => mode === "expanded" ? "minimal" : "expanded");

	const hideFlyoutNavMenu = () => void (flyoutDisplayMode !== "minimal" && setFlyoutDisplayMode("minimal"));
	useEffect(hideFlyoutNavMenu, [currentNav, useWindowWidth()]);

	return (
		<StyledNavigationView $transitionName={transitionName} {...htmlAttrs}>
			<TopLeftButtons paneDisplayMode={paneDisplayMode} onNavButton={onNavButtonClick} onBack={onBack} canBack={canBack} />
			{forMap(2, i => {
				const isFlyout = !!i;
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
						<div>
							<SwitchTransition mode="default">
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
							</SwitchTransition>
						</div>
						<section className="command-bar">
							{commandBar}
						</section>
					</div>
				</div>
				<div className={["page-content", transitionName]} ref={pageContentEl} id={pageContentId}>
					<SwitchTransition mode={transitionName === "jump" ? "out-in" : "out-in-preload"}>
						<CssTransition key={pagePath} onEnter={scrollToTopOrPrevious}>
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
