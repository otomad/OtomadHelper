import Settings from "./Settings";

export default function Navigation() {
	const [currentNav, setCurrentNav] = useState("source");
	const navItems = ["source", "midi", "audio", "visual", "sonar", "track"];
	const bottomNavItems = ["tools", "settings"];

	return (
		<NavigationView
			currentNav={[currentNav, setCurrentNav]}
			navItems={[
				...navItems.map(item => ({ text: t[item], id: item })),
				...bottomNavItems.map(item => ({ text: t[item], id: item, bottom: true })),
			]}
		>
			<Settings />
		</NavigationView>
	);
}
