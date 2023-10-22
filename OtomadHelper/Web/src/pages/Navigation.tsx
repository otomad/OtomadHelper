export default function Navigation() {
	const { t, i18n } = useTranslation();
	const [currentNav, setCurrentNav] = useState("source");

	return (
		<NavigationView
			currentNav={[currentNav, setCurrentNav]}
			navItems={[
				{ text: t("source"), id: "source" },
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
