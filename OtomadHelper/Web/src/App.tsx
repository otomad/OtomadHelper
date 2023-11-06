import Navigation from "@/Navigation.tsx";
import GlobalStyle from "./styles/global.ts";

export default function App() {
	const [ready, setReady] = useState(false);
	useEffect(() => void delay(100).then(() => setReady(true)));

	const { i18n } = useTranslation();
	const forceUpdate = useForceUpdate();
	i18n.on("languageChanged", forceUpdate);

	return (
		<>
			<GlobalStyle $ready={ready} />
			<Navigation />
		</>
	);
}