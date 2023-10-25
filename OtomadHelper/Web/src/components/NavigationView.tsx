const StyledNavButton = styled(Button)`
	width: 48px;
	height: 40px;
	margin: 3px 5px;
	background-color: transparent;
	border: none !important;
	min-width: unset;
	font-size: 16px;

	.icon {
		display: block;
	}
`;

const NavButton: FC<{}, HTMLButtonElement> = ({ ...htmlAttrs }) => {
	return (
		<StyledNavButton icon="global_nav_button" {...htmlAttrs} />
	);
};

const CONTENT_MARGIN_X = 56;
// 865 670
const TITLE_LINE_HEIGHT = 40;

const StyledNavigationView = styled.div`
	${styles.mixins.square("100%")};
	display: flex;

	> * {
		display: flex;
		flex-direction: column;
	}

	.left {
		width: 320px;
		flex-shrink: 0;

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
	}

	.right {
		width: 100%;
		max-width: 1000px;
		margin: 0 auto;

		.title-wrapper {
			position: relative;
			margin: 22px ${CONTENT_MARGIN_X}px 18px;
			font-weight: 600;
			width: 100%;
			height: ${TITLE_LINE_HEIGHT}px;
			overflow: hidden;
			flex-shrink: 0;
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
			padding: 0 ${CONTENT_MARGIN_X}px;
			overflow-y: auto;
			height: 100%;
		}
	}
`;

interface NavItem {
	text: string;
	icon?: string;
	id: string;
	bottom?: boolean;
}

interface NavBrItem {
	type: "hr";
	bottom ?: boolean;
}

const NavigationView: FC<{
	currentNav: StateProperty<string[]>;
	navItems?: (NavItem | NavBrItem)[];
	titles?: string[];
}> = ({ currentNav, navItems = [], titles, children }) => {
	const [isNavItemsOverflowing, setIsNavItemsOverflowing] = useState(false);
	const navItemsRef = useRef<HTMLDivElement>(null);
	const currentNavTab = useMemo(() => Tuple(currentNav[0][0], (value: string) => currentNav[1]([value])), [currentNav]) as StateProperty<string>;

	const currentNavItem = useMemo(() =>
		navItems.find(item => !("type" in item) && item.id === currentNavTab[0]) as NavItem,
	[currentNav, navItems]);
	titles ??= [currentNavItem?.text ?? ""];

	const getNavItemNode = useCallback((item: typeof navItems[number], index: number) => {
		if ("type" in item) return item.type === "hr" ? <hr key={index} /> : undefined;
		const { text, icon, id } = item;
		return <TabItem key={id} id={id} icon={icon || "placeholder"}>{text}</TabItem>;
	}, []);

	const previousPageTitleKey = useRef<typeof pageTitleKey>();
	const pageTitleKey: [string, number] = [currentNavItem?.id ?? "", new Date().valueOf()];
	if (pageTitleKey[0] === previousPageTitleKey.current?.[0]) pageTitleKey[1] = previousPageTitleKey.current?.[1];
	previousPageTitleKey.current = pageTitleKey;

	useEventListener(window, "resize", () => {
		const navItems = navItemsRef.current;
		if (!navItems) return;
		setIsNavItemsOverflowing(navItems.scrollHeight !== navItems.offsetHeight);
	}, { immediate: true });

	return (
		<StyledNavigationView>
			<div className="left">
				<NavButton />
				<div ref={navItemsRef} className={classNames(["nav-items", { overflowing: isNavItemsOverflowing }])}>
					<TabBar current={currentNavTab}>
						{navItems.map((item, index) => {
							if (!item.bottom) return getNavItemNode(item, index);
						})}
					</TabBar>
				</div>
				<TabBar current={currentNavTab}>
					{navItems.map((item, index) => {
						if (item.bottom) return getNavItemNode(item, index);
					})}
				</TabBar>
			</div>
			<div className="right">
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
					{children}
				</div>
			</div>
		</StyledNavigationView>
	);
};

export default NavigationView;
