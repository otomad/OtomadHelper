export default function Navigation() {
	const [currentNav, setCurrentNav] = useState("source");

	return (
		<NavigationView
			currentNav={[currentNav, setCurrentNav]}
			navItems={[
				{ text: "Source", id: "source" },
				{ text: "MIDI", id: "midi" },
				{ text: "Audio", id: "audio" },
				{ text: "Visual", id: "visual" },
				{ text: "Track", id: "track" },
				"hr",
				{ text: "Tools", id: "tools" },
			]}
		></NavigationView>
	);
}
