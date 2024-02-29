import ShellPage from "@/ShellPage.tsx";
import GlobalStyle from "./styles/global.ts";

export default function App() {
	const [ready, setReady] = useState(false);
	useMountEffect(() => {
		delay(100).then(() => setReady(true));
		document.documentElement.style.removeProperty("background-color");
	});

	const { i18n } = useTranslation();
	const forceUpdate = useForceUpdate();
	i18n.on("languageChanged", forceUpdate);

	return (
		<>
			<GlobalStyle $ready={ready} />
			<ShellPage />
		</>
	);
}
