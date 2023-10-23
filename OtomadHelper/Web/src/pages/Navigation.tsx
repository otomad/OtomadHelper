import Settings from "./Settings";

export default function Navigation() {
	const [currentNav, setCurrentNav] = useState("source");
	const navItems = ["source", "midi", "audio", "visual", "sonar", "track", "hr", "tools"];

	return (
		<NavigationView
			currentNav={[currentNav, setCurrentNav]}
			navItems={navItems.map(item =>
				item === "hr" ? "hr" : { text: t[item], id: item })}
		>
			<Settings />
		</NavigationView>
	);
}
