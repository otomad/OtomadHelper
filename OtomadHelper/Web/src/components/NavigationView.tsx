const NavButton = styled(Button).attrs({
	lite: true,
	icon: "global_nav_button",
}) <{
	/** 是否是影子？ */
	$shadow: boolean;
}>`
	width: 48px;
	height: 40px;
	margin: 4px 5px 1px;
	min-width: unset;
	z-index: 10;

	${({ $shadow }) => $shadow ? css`
		visibility: hidden;
	` : css`
		position: fixed;
	`}
`;

const floatUp = keyframes`
	from {
		opacity: 0;
		translate: 0 1rem;
	}
`;

const CONTENT_MARGIN_X = 56;
const TITLE_LINE_HEIGHT = 40;
const COMPACT_WIDTH = 58;

const StyledNavigationView = styled.div`
	${styles.mixins.square("100%")};
	display: flex;

	> * {
		display: flex;
		flex-direction: column;
	}

	.left {
		width: 320px;
		height: 100%;
		flex-shrink: 0;
		padding-bottom: 4px;

		> * {
			flex-shrink: 0;
		}

		.nav-items {
			height: 100%;
			flex-shrink: 1;
			overflow-y: auto;

			&.overflowing {
				border-bottom: 1px solid ${c("white", 8)};
			}
		}

		.nav-items,
		.nav-items-bottom {
			overflow-x: hidden;
		}

		&.compact {
			width: ${COMPACT_WIDTH}px;
		}

		&.minimal {
			width: 0;

			&:not(.flyout) {
				translate: -${COMPACT_WIDTH}px;
			}
		}

		&.flyout {
			position: fixed;
			border-radius: 0 7px 7px 0;
			background-color: ${c("white", 3)};
			box-shadow: 0 8px 16px ${c("black", 26)};
			backdrop-filter: blur(10px);
			z-index: 1;
		}
	}

	.right {
		width: 100%;

		&.hairtail {
			.title-wrapper,
			.content {
				scrollbar-gutter: stable;

				> * {
					width: 100%;
					max-width: 1000px;
					margin: 0 auto;
				}
			}
		}

		.title-wrapper {
			position: relative;
			margin: 22px 0 18px;
			font-weight: 600;
			overflow: hidden;
			flex-shrink: 0;

			> div {
				height: ${TITLE_LINE_HEIGHT}px;
			}
		}

		.title {
			font-size: 28px;
			position: absolute;
			transition: all ${eases.easeOutSmooth} 500ms;
			display: flex;
			gap: 8px;

			&.exit {
				translate: 0 -${TITLE_LINE_HEIGHT}px;
			}

			&.enter {
				translate: 0 ${TITLE_LINE_HEIGHT}px;
			}

			&.enter-active {
				translate: 0;
			}
		}

		.content {
			height: 100%;
			overflow-y: auto;

			&:has(.enter, .exit) {
				overflow-y: hidden;
			}

			> * > .container {
				margin-top: 2px;
				display: flex;
				flex-direction: column;
				gap: 6px;

				${forMap(20, i => css`
					> :nth-child(${i}) {
						animation: ${floatUp} 400ms ${125 * (i - 1)}ms ${eases.easeOutMax} backwards;
					}
				`)}
			}
		}

		.title-wrapper,
		.content {
			padding: 0 ${CONTENT_MARGIN_X}px;
		}
	}
`;

const StyledPage = styled.main`
	height: 100%;

	&.exit {
		opacity: 0;
		translate: 0 -2rem;
		transition: all ${eases.easeOutMax} 83ms;
	}

	&.enter {
		opacity: 0;
		translate: 0 5rem;
	}

	&.enter-active {
		opacity: 1;
		translate: 0;
		transition: all ${eases.easeOutMax} 250ms;
	}
`;

const NavigationViewLeftPanel: FC<{
	paneDisplayMode: PaneDisplayMode;
	isFlyoutShown: boolean;
	customContent?: ReactNode;
	currentNavTab: StateProperty<string>;
	navItems: (NavItem | NavBrItem)[];
	flyout: boolean;
}> = ({ paneDisplayMode, isFlyoutShown, customContent, currentNavTab, navItems, flyout }) => {
	const [isNavItemsOverflowing, setIsNavItemsOverflowing] = useState(false);
	const navItemsRef = useRef<HTMLDivElement>(null);

	const getNavItemNode = useCallback((item: typeof navItems[number], index: number) => {
		if ("type" in item) return item.type === "hr" ? <hr key={index} /> : undefined;
		const { text, icon, id } = item;
		return <TabItem key={id} id={id} icon={icon || "placeholder"} focusable={isFlyoutShown === flyout}>{text}</TabItem>;
	}, [isFlyoutShown]);

	useEventListener(window, "resize", () => {
		const navItems = navItemsRef.current;
		if (!navItems) return;
		setIsNavItemsOverflowing(navItems.scrollHeight !== navItems.offsetHeight);
	}, { immediate: true });

	return (
		<div className={classNames(["left", paneDisplayMode, { flyout }])}>
			<NavButton $shadow />
			<div ref={navItemsRef} className={classNames(["nav-items", { overflowing: isNavItemsOverflowing }])}>
				{customContent}
				<TabBar current={currentNavTab} collapsed={paneDisplayMode === "compact"}>
					{navItems.map((item, index) => {
						if (!item.bottom) return getNavItemNode(item, index);
					})}
				</TabBar>
			</div>
			<div className="nav-items-bottom">
				<TabBar current={currentNavTab} collapsed={paneDisplayMode === "compact"}>
					{navItems.map((item, index) => {
						if (item.bottom) return getNavItemNode(item, index);
					})}
				</TabBar>
			</div>
		</div>
	);
};

interface NavItem {
	/** 标签文本。 */
	text: string;
	/** 图标。 */
	icon?: string;
	/** 标识符。 */
	id: string;
	/** 是否将其放置于导航面板底部。 */
	bottom?: boolean;
}

interface NavBrItem {
	/** 类型：分割线。 */
	type: "hr";
	/** 是否将其放置于导航面板底部。 */
	bottom ?: boolean;
}

type PaneDisplayMode = "expanded" | "compact" | "minimal";
const getPaneDisplayMode = (): PaneDisplayMode =>
	window.innerWidth < 670 ? "minimal" :
	window.innerWidth < 865 ? "compact" : "expanded";
const usePaneDisplayMode = () => {
	const [paneDisplayMode, setPaneDisplayMode] = useState<PaneDisplayMode>(getPaneDisplayMode());
	useEventListener(window, "resize", () => setPaneDisplayMode(getPaneDisplayMode()));
	return paneDisplayMode;
};

const NavigationView: FC<{
	/** 当前导航页状态参数。 */
	currentNav: StateProperty<string[]>;
	/** 所有导航项。 */
	navItems?: (NavItem | NavBrItem)[];
	/** 面包屑导航标题数组。 */
	titles?: string[];
	/** 自定义区域。 */
	customContent?: ReactNode;
}> = ({ currentNav, navItems = [], titles, children, customContent }) => {
	const currentNavTab = useStateSelector(currentNav, nav => nav[0], value => [value]);
	const pagePath = currentNav.join("/");
	const responsive = usePaneDisplayMode();
	const [flyoutDisplayMode, setFlyoutDisplayMode] = useState<PaneDisplayMode>("minimal");
	const [isExpandedInExpandedMode, setIsExpandedInExpandedMode] = useState(true);
	const paneDisplayMode: PaneDisplayMode = responsive === "expanded" ?
		isExpandedInExpandedMode ? "expanded" : "compact" : responsive;

	const currentNavItem = useMemo(() =>
		navItems.find(item => !("type" in item) && item.id === currentNavTab[0]) as NavItem,
	[currentNav, navItems]);
	titles ??= [currentNavItem?.text ?? ""];

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
	useEffect(hideFlyoutNavMenu, [currentNav]);
	useEventListener(window, "resize", () => hideFlyoutNavMenu(), undefined, [hideFlyoutNavMenu]);

	return (
		<StyledNavigationView>
			<NavButton onClick={onNavButtonClick} />
			{forMap(2, i => {
				const isFlyout = i === 2;
				return (
					<NavigationViewLeftPanel
						key={i}
						paneDisplayMode={isFlyout ? flyoutDisplayMode : paneDisplayMode}
						isFlyoutShown={flyoutDisplayMode !== "minimal"}
						currentNavTab={currentNavTab}
						navItems={navItems}
						customContent={customContent}
						flyout={isFlyout}
					/>
				);
			})}
			<div
				className={classNames(["right", "hairtail", { minimal: paneDisplayMode === "minimal" }])}
				onClick={hideFlyoutNavMenu}
			>
				<div className="title-wrapper">
					<TransitionGroup>
						<CssTransition key={pageTitleKey.join()}>
							<h1 className="title">
								{titles.map((title, index) => <div key={index}>{title}</div>)}
							</h1>
						</CssTransition>
					</TransitionGroup>
				</div>
				<div className="content">
					<SwitchTransition>
						<CssTransition key={pagePath}>
							<StyledPage>
								{children}
							</StyledPage>
						</CssTransition>
					</SwitchTransition>
				</div>
			</div>
		</StyledNavigationView>
	);
};

export default NavigationView;
