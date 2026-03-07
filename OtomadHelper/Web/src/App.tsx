import { changeColorScheme } from "helpers/color-mode";
import { ThemeProvider } from "styled-components";
import DynamicAccentColor from "styles/accent";
import GlobalStyle from "styles/global";
import ShellPage from "./ShellPage";

export default function App() {
	const [ready, setReady] = useState(false);
	const t = useT();

	useMountEffect(() => {
		delay(100).then(() => setReady(true));
		const removedInitialStyles: (keyof CSSPropertiesHyphen)[] = ["color-scheme"];
		if (!initialSystemConfig.panelBackgroundColor) removedInitialStyles.unshift("background-color");
		removedInitialStyles.forEach(property => document.documentElement.style.removeProperty(property));
		changeColorScheme(undefined, undefined, undefined, "refresh");
	});

	const [theme, setTheme] = useState(initialSystemConfig);
	useListen("host:systemConfig", config => emit("app:startColorPaletteViewTransition", () => setTheme(config)));

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
