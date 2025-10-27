import { changeColorScheme } from "helpers/color-mode";
import { ThemeProvider } from "styled-components";
import DynamicAccentColor from "styles/accent";
import GlobalStyle from "styles/global";
import ShellPage from "./ShellPage";

export default function App() {
	"use no memo"; // TODO: Use standard react-i18next, the custom hook seems break aria-label.
	const [ready, setReady] = useState(false);

	useMountEffect(() => {
		delay(100).then(() => setReady(true));
		const removedInitialStyles: (keyof CSSPropertiesHyphen)[] = ["background-color", "color-scheme"];
		removedInitialStyles.forEach(property => document.documentElement.style.removeProperty(property));
		changeColorScheme(undefined, undefined, undefined, "refresh");
	});

	const forceUpdate = useForceUpdate();
	i18n.on("languageChanged", forceUpdate);

	const [theme, setTheme] = useState(initialSystemConfig);
	useListen("host:systemConfig", setTheme);

	return (
		<ThemeProvider theme={theme}>
			<BackgroundImage />
			<GlobalStyle $ready={ready} />
			<DynamicAccentColor />
			<UnsupportedBrowserInfoBar />
			<ShellPage />
			<DevContextMenu />
			<Toast />
			<DefineSvgFilter.Portal />
			<div id="portals" />
			<meta name="description" content={t.descriptions.settings.about} />
		</ThemeProvider>
	);
}
