import { changeColorScheme } from "helpers/color-mode";
import GlobalStyle from "styles/global";
import ShellPage from "./ShellPage";

export default function App() {
	const [ready, setReady] = useState(false);

	useMountEffect(() => {
		delay(100).then(() => setReady(true));
		document.documentElement.style.removeProperty("background-color");
		changeColorScheme(undefined, "refresh");
	});

	useEffect(() => {
		updateOrCreateMetaTag("description", t.descriptions.settings.about); // DELETE
	});

	const { i18n } = useTranslation();
	const forceUpdate = useForceUpdate();
	i18n.on("languageChanged", forceUpdate);

	return (
		<>
			<BackgroundImage />
			<GlobalStyle $ready={ready} />
			<ShellPage />
			<DevContextMenu />
			<div id="portals" />
		</>
	);
}
