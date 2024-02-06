const pages = import.meta.glob<FC>("./views/**/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return <EmptyMessage icon="settings" heading={t.underConstruction} />;
}

export default function ShellPage() {
	const { page, changePage, getPagePath, transition, canBack, back } = usePageStore();
	// const [mode, setMode] = useState<Mode>("otomadOrYtpmv");
	const pageTitles = page.map((crumb, i, { length }) => ({
		name: t.titles[crumb]({ context: "full" }),
		link: i === length - 1 ? undefined : page.slice(0, i + 1),
	}));
	const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "lyrics", "shupelunker", "ytp"];
	const navToolItems = ["mosh", "tools"];
	const bottomNavItems = ["settings"] as const;
	const modes = ["otomadOrYtpmv", "ytp", "shupelunker"] as const;
	type Mode = typeof modes[number];
	const Page = pages[`./views/${getPagePath()}.tsx`] ?? EmptyPage;
	const [uiScale] = selectConfig(c => c.settings.uiScale);

	return (
		<NavigationView
			currentNav={[page, changePage]}
			navItems={[
				...navItems.map(item => ({ text: t.titles[item], id: item, icon: item })),
				{ type: "hr" },
				...navToolItems.map(item => ({ text: t(2).titles[item], id: item, icon: item })),
				...bottomNavItems.map(item => ({ text: t(2).titles[item], id: item, icon: item, bottom: true })),
			]}
			titles={pageTitles}
			transitionName={transition}
			canBack={canBack()}
			onBack={back}
			style={{ zoom: uiScale === 100 ? undefined : uiScale / 100 }}
		>
			<Page />
		</NavigationView>
	);
}
