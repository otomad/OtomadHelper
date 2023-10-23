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
			margin: 22px ${CONTENT_MARGIN_X}px;
			font-weight: 600;
			width: 100%;
			height: 36px;
			overflow: hidden;
			flex-shrink: 0;
		}

		.content {
			padding: 0 ${CONTENT_MARGIN_X}px;
			overflow-y: auto;
		}

		.title {
			font-size: 28px;
			position: absolute;
			transition: all ${eases.easeOutSmooth} 700ms;

			&.exit {
				translate: 0 -100%;
			}

			&.enter {
				translate: 0 100%;
			}

			&.enter-active {
				translate: 0;
			}
		}
	}
`;

interface NavItem {
	text: string;
	icon?: string;
	id: string;
}

const NavigationView: FC<{
	currentNav: StateProperty<string>;
	navItems?: (NavItem | "hr")[];
	showSettings?: boolean;
}> = ({ currentNav, navItems = [], showSettings = true, children }) => {
	const [isNavItemsOverflowing, setIsNavItemsOverflowing] = useState(false);
	const navItemsRef = useRef<HTMLDivElement>(null);

	const currentNavItem = useMemo(() => {
		const items = navItems.filter(item => typeof item === "object") as NavItem[];
		const SETTINGS = "settings";
		items.push({ text: t[SETTINGS], id: SETTINGS });
		return items.find(item => item.id === currentNav[0]);
	}, [currentNav, navItems]);

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
					<TabBar current={currentNav}>
						{navItems.map((item, index) => {
							if (item === "hr") return <hr key={index} />;
							const { text, icon, id } = item;
							return <TabItem key={id} id={id} icon={icon || "placeholder"}>{text}</TabItem>;
						})}
					</TabBar>
				</div>
				{showSettings && <TabBar current={currentNav}>
					<TabItem id="settings" icon="settings">{t.settings}</TabItem>
				</TabBar>}
			</div>
			<div className="right">
				<div className="title-wrapper">
					<TransitionGroup>
						<Transition key={pageTitleKey.join()} addEndListener={endListener}>
							<h1 className="title">{currentNavItem?.text ?? ""}</h1>
						</Transition>
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
