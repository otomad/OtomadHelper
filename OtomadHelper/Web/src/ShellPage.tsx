const pages = import.meta.glob<FC>("./views/**/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return <EmptyMessage icon="settings" heading="功能研发中" />;
}

export default function ShellPage() {
	const { page, changePage, getPagePath, getTransition } = usePageStore();
	const [mode, setMode] = useState<Mode>("otomadOrYtpmv");
	const pageTitles = page.map((crumb, i, { length }) => ({
		name: t.titles[crumb]({ context: "full" }),
		link: i === length - 1 ? undefined : page.slice(0, i + 1),
	}));
	const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "shupelunker", "ytp"];
	const navToolItems = ["mosh", "tools"];
	const bottomNavItems = ["settings"] as const;
	const modes = ["otomadOrYtpmv", "ytp", "shupelunker"] as const;
	type Mode = typeof modes[number];
	const Page = pages[`./views/${getPagePath()}.tsx`] ?? EmptyPage;

	return (
		<NavigationView
			currentNav={[page, changePage]}
			navItems={[
				...navItems.map(item => ({ text: t.titles[item], id: item, icon: item })),
				{ type: "hr" },
				...navToolItems.map(item => ({ text: t.titles[item], id: item, icon: item })),
				...bottomNavItems.map(item => ({ text: t.titles[item], id: item, icon: item, bottom: true })),
			]}
			titles={pageTitles}
			transitionName={getTransition()}
		>
			<Page />
		</NavigationView>
	);
}
