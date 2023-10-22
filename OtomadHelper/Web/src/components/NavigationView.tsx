import { CSSTransition, SwitchTransition, TransitionGroup } from "react-transition-group";

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

const StyledNavigationView = styled.div`
	${styles.mixins.square("100%")};
	display: flex;

	.left {
		display: flex;
		flex-direction: column;
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

		.title-wrapper {
			position: relative;
			margin: 22px 56px;
			font-weight: 600;
			width: 100%;
			height: 36px;
			overflow: hidden;
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

	const currentNavItem = useMemo(() =>
		navItems.find(item => typeof item === "object" && item.id === currentNav[0]) as NavItem | undefined, currentNav);

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
					<TabItem id="settings" icon="settings">Settings</TabItem>
				</TabBar>}
			</div>
			<div className="right">
				<div className="title-wrapper">
					<TransitionGroup>
						<CSSTransition key={currentNavItem?.id ?? ""} timeout={350}>
							<h1 className="title">{currentNavItem?.text ?? ""}</h1>
						</CSSTransition>
					</TransitionGroup>
				</div>
				{children}
			</div>
		</StyledNavigationView>
	);
};

export default NavigationView;
