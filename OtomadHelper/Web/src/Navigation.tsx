const pages = import.meta.glob<FC>("./views/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return (
		<h1>未找到页面！</h1>
	);
}

export default function Navigation() {
	const [currentNav, setCurrentNav] = useState(["source"]);
	const [mode, setMode] = useState<Mode>("otomadOrYtpmv");
	const pageTitles = currentNav.map(page => t[page]);
	const pagePath = currentNav.join("/");
	const navItems = useMemo(() => arrayToRemoveFalsy([
		"source",
		mode !== "ytp" && "midi",
		"audio",
		"visual",
		mode === "otomadOrYtpmv" && "sonar",
		mode === "ytp" && "ytp",
		mode === "shupelunker" && "shupelunker",
		"track",
	]), [mode]);
	const bottomNavItems = ["tools", "settings"] as const;
	const modes = ["otomadOrYtpmv", "ytp", "shupelunker"] as const;
	type Mode = typeof modes[number];
	const Page = pages[`./views/${pagePath}.tsx`] ?? EmptyPage;

	return (
		<PageContext.Provider value={[currentNav, setCurrentNav]}>
			<NavigationView
				currentNav={[currentNav, setCurrentNav]}
				navItems={[
					...navItems.map(item => ({ text: t[item], id: item })),
					...bottomNavItems.map(item => ({ text: t[item], id: item, bottom: true })),
				]}
				titles={pageTitles}
				customContent={
					<TabBar current={[mode, setMode]}>
						{modes.map(mode =>
							<TabItem key={mode} id={mode} icon="placeholder">{t[mode]}</TabItem>)}
					</TabBar>
				}
			>
				<Page />
			</NavigationView>
		</PageContext.Provider>
	);
}
