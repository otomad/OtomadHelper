// import { t as $t } from "i18next";

const pages = import.meta.glob<FC>("./views/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return (
		<h1>功能研发中……</h1>
	);
}

export default function ShellPage() {
	const [currentNav, setCurrentNav] = useState(["source"]);
	const [mode, setMode] = useState<Mode>("otomadOrYtpmv");
	const pageTitles = currentNav.map(page => t.titles[page]({ context: "full" }));
	const pagePath = currentNav.join("/");
	const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "shupelunker", "ytp"];
	const navToolItems = ["mosh", "tools"];
	const bottomNavItems = ["settings"] as const;
	const modes = ["otomadOrYtpmv", "ytp", "shupelunker"] as const;
	type Mode = typeof modes[number];
	const Page = pages[`./views/${pagePath}.tsx`] ?? EmptyPage;

	return (
		<PageContext.Provider value={[currentNav, setCurrentNav]}>
			<NavigationView
				currentNav={[currentNav, setCurrentNav]}
				navItems={[
					...navItems.map(item => ({ text: t.titles[item], id: item })),
					{ type: "hr" },
					...navToolItems.map(item => ({ text: t.titles[item], id: item })),
					...bottomNavItems.map(item => ({ text: t.titles[item], id: item, bottom: true })),
				]}
				titles={pageTitles}
			>
				<Page />
			</NavigationView>
		</PageContext.Provider>
	);
}
