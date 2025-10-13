import type { TransitionUpdateStatus } from "react-transition-group-fc";

const navButtonSize = { width: 44, height: 40, verticalWidth: 52 };
const NAV_ITEMS_ASSUMED_COUNT = 20;
const NAV_ITEMS_BOTTOM_ASSUMED_COUNT = 3;
const TITLE_ANCHOR_NAME = "--navigation-view-title";
const hasUnsupportedBrowserInfoBar = `body:has(.${nameof.kebab({ UnsupportedBrowserInfoBar })}) &`;
export const CONTAINER_CLASSNAMES = ".container, .container-preview";

const TooltipTitleWithShortcut = ({ title, shortcut }: { title: string; shortcut: string[] }) =>
	<>{title}<Kbd>{shortcut}</Kbd></>;

export const styledContainer = css`
	display: flex;
	flex-direction: column;
	gap: 6px;
	inline-size: 100%;
`;

const NavButton = styled(Button).attrs({
	subtle: true,
})`
	&& {
		position: absolute;
		block-size: ${navButtonSize.height}px;
		inline-size: ${navButtonSize.width}px;
		min-inline-size: unset;
	}
`;

const StyledTopLeftButtons = styled.div`
	position: relative;
	z-index: 10;
	block-size: ${navButtonSize.height}px;
	margin-block: 4px 1px;
	margin-inline: 9px 5px;

	&.vertical {
		block-size: ${navButtonSize.height * 2}px;
		margin-inline-start: 5px;

		${NavButton} {
			inline-size: ${navButtonSize.verticalWidth}px;
		}
	}

	&:not(.shadow) {
		position: fixed;
		z-index: 11;
	}

	&:not(.vertical) ${NavButton}:nth-of-type(2) {
		inset-block-start: 0;
		inset-inline-start: ${navButtonSize.width}px;
	}

	&.vertical ${NavButton}:nth-of-type(2) {
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
		if (shadow) return;
		if (e.altKey && e.code === "ArrowLeft" && !e.repeat) onBack?.();
		else if (e.altKey && e.code === "KeyH" && !e.repeat) onNavButton?.();
	}, undefined, null);

	return (
		<StyledTopLeftButtons className={{ shadow, vertical }}>
			{!shadow && (
				<>
					<Tooltip placement={tooltipPlacement} title={<TooltipTitleWithShortcut title={t.back} shortcut={["Alt", "←"]} />}>
						<NavButton animatedIcon="back" disabled={!canBack} onClick={onBack} aria-label={t.back} dirBasedIcon />
					</Tooltip>
					<Tooltip placement={tooltipPlacement} title={<TooltipTitleWithShortcut title={t.navigation} shortcut={["Alt", "H"]} />}>
						<NavButton animatedIcon="global_nav_button" onClick={onNavButton} aria-label={t.navigation} />
						{/* Do not use `accessKey="H"`, it do repeat the keydown, which is not we wanted. */}
					</Tooltip>
				</>
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
	${styles.mixins.square("100%", false, true)};
	display: flex;

	${hasUnsupportedBrowserInfoBar} {
		block-size: calc(100% - ${UnsupportedBrowserInfoBar.height}px);
	}

	> * {
		display: flex;
		flex-direction: column;
	}

	> .left {
		flex-shrink: 0;
		block-size: 100%;
		inline-size: 320px;
		max-inline-size: calc(100dvw / var(--zoom, 1));
		padding-block-end: 4px;
		overflow: hidden;

		@media (horizontal-viewport-segments >= 2) {
			inline-size: calc((env(viewport-segment-left 1 0) - env(viewport-segment-left 0 0)) / var(--zoom, 1));

			&.expanded:not(.flyout) {
				padding-inline-end: calc((env(viewport-segment-left 1 0) - env(viewport-segment-right 0 0)) / var(--zoom, 1));
			}

			&.expanded.flyout {
				inline-size: calc((env(viewport-segment-right 0 0) - env(viewport-segment-left 0 0)) / var(--zoom, 1));
			}
		}

		> * {
			flex-shrink: 0;
		}

		.nav-items {
			container: nav-items / scroll-state;
			position: relative;
			flex-shrink: 1;
			block-size: 100%;
			overflow-block: auto;

			&::after {
				content: "";
				position: sticky;
				inset-block-end: 0;
				display: none;
				border-block-end: 1px solid ${c("stroke-color-divider-stroke-default")};

				@container not scroll-state(scrollable: none) {
					display: block;
				}
			}
		}

		.nav-items,
		.nav-items-bottom {
			overflow-inline: hidden;
		}

		&:is(.compact, .minimal):not(.flyout) .nav-items {
			${styles.mixins.noScrollbar()};
		}

		&.compact {
			inline-size: ${COMPACT_WIDTH}px;
		}

		&.minimal {
			inline-size: 0;

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
			outline: 1px solid ${c("stroke-color-surface-stroke-flyout-navigation-panel")};
			box-shadow: 0 8px 16px ${c("shadows-flyout")};
			backdrop-filter: blur(60px);
			transition-behavior: allow-discrete;

			${hasUnsupportedBrowserInfoBar} {
				block-size: calc(100% - ${UnsupportedBrowserInfoBar.height}px);
			}

			body:has(.background-image) & {
				background-color: transparent;
			}
		}

		search {
			transition: ${fallbackTransitions}, margin-inline 0s;

			&:not(.collapsed) {
				margin-block: 4px 3px;
				margin-inline: 9px;

				.text-box .leading-icon {
					margin-inline-end: 4px;
				}
			}

			&.collapsed {
				/* margin-block-end: 5px; */

				button {
					block-size: ${navButtonSize.height}px;
					inline-size: ${navButtonSize.verticalWidth}px;
					margin-inline-start: 5px;
				}
			}
		}

		&:not(.compact, .compact + .flyout) search {
			margin-block-end: 8px;
		}
	}

	> .right {
		inline-size: 100%;

		&.hairtail {
			> .title-wrapper,
			> .page-content {
				scrollbar-gutter: stable;

				> * {
					inline-size: 100%;
					max-inline-size: 1000px;
					margin: 0 auto;
				}
			}
		}

		&.minimal > .title-wrapper {
			margin-block-start: 40px;
		}

		.title-wrapper {
			position: relative;
			flex-shrink: 0;
			margin: 12px 0 8px;
			overflow: hidden;
			font-weight: 600;

			> .title-wrapper-inner {
				anchor-name: ${TITLE_ANCHOR_NAME};
				display: flex;
				justify-content: space-between;
				align-items: center;
				block-size: ${TITLE_LINE_HEIGHT}px;
				inline-size: 100%;

				> div {
					${styles.mixins.square("100%")};
				}

				.command-bar-wrapper {
					--inset-block-start: 12px;
					flex-shrink: 0;
					block-size: ${TITLE_LINE_HEIGHT}px;
					transition: ${fallbackTransitions}, inset-inline 0s;

					@supports (anchor-name: ${TITLE_ANCHOR_NAME}) {
						position: fixed;
						position-anchor: ${TITLE_ANCHOR_NAME};
						inset-block-start: var(--inset-block-start);
						inset-inline-end: anchor(end);

						${hasUnsupportedBrowserInfoBar} {
							inset-block-start: calc(var(--inset-block-start) + ${UnsupportedBrowserInfoBar.height}px);
						}
					}
				}
			}
		}

		&.minimal .title-wrapper > .title-wrapper-inner .command-bar-wrapper {
			--inset-block-start: 4px !important;
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

			&.exit:has(+ .title.exit) {
				transition-duration: 1s;
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
			container: page-scroll / scroll-state;
			block-size: 100%;
			contain: layout;
			overflow-block: auto;
			overflow-inline: hidden;
			overscroll-behavior: contain;

			&:has(> .enter, > .exit),
			&:has(> main > .container-preview) {
				overflow-block: hidden;
			}

			> main {
				@layer layout {
					> :not(.flyout) {
						${styledContainer}
					}
				}

				> .container {
					position: relative;
					margin-block-start: 2px;

					&:not(:has(> .empty-message))::after {
						content: "";
						display: block;
						flex-shrink: 0;
						block-size: 18px;
					}

					> * {
						animation: ${({ $transitionName }) => $transitionName === "jump" ? floatUp : ""}
							300ms calc(50ms * --sibling-index-0()) ${eases.easeOutMax} backwards;
					}

					.card.media-pool > .base {
						padding: 2px;
					}

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

				> .container-preview {
					${styles.mixins.square("100%")};
					position: absolute;
				}
			}

			> :is(.exit, .exit-done) > .container > * {
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
			`&:has(.nav-items .tooltip-child-wrapper:nth-of-type(${i}) .tab-item:active) .nav-items .tooltip-child-wrapper:nth-of-type(${i}) .tab-item .animated-icon`, 1);
		selectors.push(...forMap(NAV_ITEMS_BOTTOM_ASSUMED_COUNT, i =>
			`&:has(.nav-items-bottom .tooltip-child-wrapper:nth-of-type(${i}) .tab-item:active) .nav-items-bottom .tooltip-child-wrapper:nth-of-type(${i}) .tab-item .animated-icon`, 1));
		return css`
			${selectors.join(", ")} {
				--state: pressed;
			}
		`;
	}}
`;

const StyledPage = styled.main`
	container: page / size;
	position: relative;
	display: flex;
	block-size: 100%;
	transition: none;

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

function NavigationViewLeftPanel({ paneDisplayMode, isFlyoutShown, customContent, currentNavTab, navItems, navItemsId, flyout, isCompact, searchValue, onRequestHide, onRequestExpand, onSearch }: FCP<{
	paneDisplayMode: PaneDisplayMode;
	isFlyoutShown: boolean;
	customContent?: ReactNode;
	currentNavTab: StateProperty<string>;
	navItems: (NavItem | NavBrItem)[];
	navItemsId?: string;
	flyout: boolean;
	isCompact: boolean;
	searchValue: StateProperty<string>;
	onRequestHide(): void;
	onRequestExpand(): void;
	onSearch?: PropsOf<typeof SearchBox>["onSearch"];
}>) {
	const navItemsEl = useDomRef<"div">();
	const focusable = !flyout && paneDisplayMode === "minimal" ? false : isFlyoutShown === flyout;
	const covered = !flyout && isFlyoutShown;
	const isHidden = paneDisplayMode === "minimal" || covered;
	const { t } = useTranslation();
	const searchPlaceholder = t("search");

	const getNavItemNode = useCallback((item: typeof navItems[number], index: number) => {
		if ("type" in item) return item.type === "hr" ? <hr key={index} aria-hidden /> : undefined;
		const { text, icon, animatedIcon, id, badge } = item;
		return (
			<TabBar.Item
				key={id}
				id={id}
				icon={icon || (!animatedIcon ? "placeholder" : undefined)}
				animatedIcon={animatedIcon}
				focusable={focusable}
				badge={badge}
				ariaCurrentWhenSelected="page"
				onClick={onRequestHide}
			>
				{text}
			</TabBar.Item>
		);
	}, [isFlyoutShown, focusable, onRequestHide]);

	const onNavItemsScroll = useCallback<UIEventHandler<HTMLDivElement>>(e => {
		const currentElement = e.currentTarget;
		const panel = currentElement.closest(".left")!;
		if (panel.classList.containsAny("minimal", "covered")) return;
		const { scrollTop, dataset: { navItemsId } } = currentElement;
		if (!navItemsId) return;
		document.querySelectorAll(`[data-nav-items-id="${navItemsId}"]`).forEach(element => {
			if (element === currentElement) return;
			element.scrollTo({ top: scrollTop, behavior: "instant" });
		});
	}, []);

	const [mainTabBar, bottomTabBar] = [false, true].map(isBottom => (
		<TabBar key={`bottom-${isBottom}`} current={currentNavTab} collapsed={paneDisplayMode === "compact"} vertical>
			{navItems.map((item, index) =>
				isBottom === !!item.bottom && getNavItemNode(item, index))}
		</TabBar>
	));

	return (
		<aside className={["left", paneDisplayMode, { flyout, covered }]} aria-hidden={isHidden} aria-label={t("aria.navMenu")}>
			<TopLeftButtons shadow paneDisplayMode={isCompact ? "compact" : paneDisplayMode} />
			<SearchBox
				value={searchValue}
				collapsed={paneDisplayMode !== "expanded" && !flyout}
				inert={isHidden}
				collapsedButtonTooltip={{ title: <TooltipTitleWithShortcut title={searchPlaceholder} shortcut={["Ctrl", "F"]} />, placement: "right" }}
				enableShortcutKey={!flyout}
				placeholder={searchPlaceholder}
				onCollapsedButtonClick={onRequestExpand}
				onSearch={onSearch}
			/>
			<div
				ref={navItemsEl}
				data-nav-items-id={navItemsId}
				className="nav-items"
				tabIndex={-1}
				onScroll={onNavItemsScroll}
			>
				{customContent}
				{mainTabBar}
			</div>
			<div className="nav-items-bottom">
				{bottomTabBar}
			</div>
		</aside>
	);
}

const StyledBreadCrumbChevronRight = styled.div`
	${styles.mixins.flexCenter()};
	margin-block-start: 4px;

	.icon {
		color: ${c("fill-color-text-secondary")};
		font-size: 16px;
	}
`;

const BreadCrumbChevronRight = ({ ref }: FCP<{}, "div">) => (
	<StyledBreadCrumbChevronRight ref={ref}>
		<Icon name="chevron_right" />
	</StyledBreadCrumbChevronRight>
);

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
const getPaneDisplayMode = (zoom: number = 1): PaneDisplayMode => {
	if (window.isWebView) zoom = 1;
	return (
		window.innerWidth < 641 * zoom ? "minimal" :
		window.innerWidth < 1008 * zoom ? "compact" : "expanded"
	);
};
const usePaneDisplayMode = () => {
	const uiScale1 = useUiScale1();
	const [paneDisplayMode, setPaneDisplayMode] = useState<PaneDisplayMode>(getPaneDisplayMode(getUiScale1()));
	const onResize = () => setPaneDisplayMode(getPaneDisplayMode(getUiScale1()));
	useEventListener(window, "resize", onResize);
	useEffect(() => onResize(), [uiScale1]);
	return paneDisplayMode;
};

export const MainPageTransitionContext = createContext({ status: "entered" as TransitionUpdateStatus });

export default function NavigationView({ currentNav: [currentNav, setCurrentNav], navItems = [], titles, transitionName = "", children, customContent, canBack = true, onBack, commandBar, pageContentId, poppedScroll, searchValue, onSearch, onEnter, ...htmlAttrs }: FCP<{
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
	/** The current search box text. */
	searchValue: StateProperty<string>;
	/** Get search results. */
	onSearch?: PropsOf<typeof SearchBox>["onSearch"];
	/** Occurs when the new page enter. */
	onEnter?(): void;
}, "div">) {
	const currentNavTab = useStateSelector([currentNav, setCurrentNav], nav => nav[0], value => [value]);
	const pagePath = currentNav!.join("/");
	const responsive = usePaneDisplayMode();
	const [flyoutDisplayMode, setFlyoutDisplayMode] = useState<PaneDisplayMode>("minimal");
	const [isExpandedInExpandedMode, setIsExpandedInExpandedMode] = useState(true);
	const paneDisplayMode: PaneDisplayMode = responsive === "expanded" ?
		isExpandedInExpandedMode ? "expanded" : "compact" : responsive;
	const pageContentEl = useDomRef<"div">();

	function scrollToTopOrPrevious() {
		onEnter?.();
		const pageContent = pageContentEl.current;
		if (!pageContent) return;
		const container = pageContent.lastElementChild?.firstElementChild;
		while (poppedScroll && container?.classList.contains("container")) { // Cheat `if` as `while` to use `break` in it.
			let child = container.children[poppedScroll.elementIndex] as HTMLElement | undefined;
			while (isElementContents(child))
				child = child!.firstElementChild as HTMLElement;
			if (isElementHidden(child) || !child) break;
			let { offsetY } = poppedScroll;
			if (child.offsetHeight < offsetY) offsetY = child.offsetHeight;
			child.scrollIntoView({ behavior: "instant" });
			pageContent.scrollBy({ top: offsetY, behavior: "instant" });
			return;
		}
		pageContent.scrollTo({ top: 0, left: 0, behavior: "instant" });
	}

	const navItemsId = useId();
	const [mainPageTransitionStatus, setMainPageTransitionStatus] = useState<TransitionUpdateStatus>("entered");
	const reduceMotion = useMediaQuery.reduceMotion();

	const currentNavItem = useMemo(() =>
		navItems.find(item => !("type" in item) && item.id === currentNavTab[0]) as NavItem,
	[currentNav, navItems]);
	titles ??= [{ name: currentNavItem?.text ?? "" }];

	const previousPageTitleKey = useRef<typeof pageTitleKey>(undefined);
	const pageTitleKey: [string, number] = [currentNavItem?.id ?? "", new Date().valueOf()];
	if (pageTitleKey[0] === previousPageTitleKey.current?.[0]) pageTitleKey[1] = previousPageTitleKey.current?.[1];
	previousPageTitleKey.current = pageTitleKey;

	const onNavButtonClick = () => responsive === "expanded" ?
		setIsExpandedInExpandedMode(expanded => !expanded) :
		setFlyoutDisplayMode(mode => mode === "expanded" ? "minimal" : "expanded");
	const onRequestExpand = () => responsive === "expanded" ?
		setIsExpandedInExpandedMode(true) :
		setFlyoutDisplayMode("expanded");
	const hideFlyoutNavMenu = () => { flyoutDisplayMode !== "minimal" && setFlyoutDisplayMode("minimal"); };
	const handleSearch: typeof onSearch = ({ onSelect, ...props }) => onSearch?.({
		onSelect() {
			(document.activeElement as HTMLElement)?.blur?.();
			setFlyoutDisplayMode("minimal");
			onSelect?.();
		},
		...props,
	});

	const windowWidth = useWindowWidth();
	useEffect(hideFlyoutNavMenu, [currentNav, windowWidth]);

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
						searchValue={searchValue}
						onRequestHide={hideFlyoutNavMenu}
						onRequestExpand={onRequestExpand}
						onSearch={handleSearch}
					/>
				);
			})}
			<div
				className={[
					"right",
					"hairtail",
					{
						minimal: paneDisplayMode === "minimal",
					},
				]}
				onClick={hideFlyoutNavMenu}
			>
				<Attrs inert={flyoutDisplayMode !== "minimal"}>
					<header className="title-wrapper">
						<div className="title-wrapper-inner">
							<div>
								<TransitionGroup>
									<CssTransition key={pageTitleKey.join()}>
										<h1 className="title" role="navigation" aria-label={t.aria.breadcrumb}>
											<TransitionGroup>
												{titles.flatMap((title, i, { length }) => {
													const last = i === length - 1;
													const crumb = (
														<button
															key={i}
															className={["crumb", { parent: !last }]}
															tabIndex={last ? -1 : 0}
															type="button"
															role="link"
															aria-current={last && "page"}
															value={title.name}
															onClick={() => title.link?.length && setCurrentNav?.(title.link)}
														>
															{title.name}
														</button>
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
							</div>
							<section className="command-bar-wrapper">
								{commandBar}
							</section>
						</div>
					</header>
					<div className={["page-content", transitionName]} ref={pageContentEl} id={pageContentId}>
						<MainPageTransitionContext value={{ status: mainPageTransitionStatus }}>
							<SwitchTransition mode={transitionName === "jump" ? "out-in" : "out-in-preload"}>
								<CssTransition
									key={pagePath}
									onUpdated={(_, status) => {
										setMainPageTransitionStatus(status);
										if (status === (reduceMotion ? "entered" : "enter")) scrollToTopOrPrevious();
									}}
									moreCoherentWhenCombo
									maxTimeout={1000}
								>
									<StyledPage data-path={pagePath} aria-label={titles.last().name}>
										{children}
									</StyledPage>
								</CssTransition>
							</SwitchTransition>
						</MainPageTransitionContext>
					</div>
				</Attrs>
			</div>
		</StyledNavigationView>
	);
}
