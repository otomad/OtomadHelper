import { autoColorPalettes } from "helpers/basic-color-palette";
import colors from "./colors";
const getAutoColor = (prefix: string, color: typeof autoColorPalettes[number] | (string & {})) =>
	autoColorPalettes.includes(color) ? `var(--${prefix}-${color})` : color;

const StyledDynamicAccentColor = createGlobalStyle<{
	$customize: {
		accentColor: string;
		backgroundColor: string;
		currentDominantColor?: string;
	};
}>(({ theme: $palette, $customize: { accentColor, backgroundColor, currentDominantColor } }) => [
	css`
		--colorization: #005fb8;
		--accent-color-windows: light-dark(${colors["accent-color"].slice(0, 2).join(",")});
		--accent-color-vegas: #0078d7; // #198cfe
		--accent-color-wallpaper: var(--image-dominant-color, var(--accent-color-windows));
		--background-color-windows: light-dark(${colors["background-color"].slice(0, 2).join(",")});
		--background-color-vegas: light-dark(#eee, #222);
		--background-color-wallpaper: var(--image-dominant-color, var(--background-color-windows));
	`,
	$palette?.colorization && css`
		--colorization: ${$palette.colorization};
		--accent-color: if(
			style(--color-scheme-contrast: true): ${colors["accent-color"][2]};
			else: light-dark(${$palette.lightAccentColor}, ${$palette.darkAccentColor});
		);
		--accent-color-windows: light-dark(${$palette.lightAccentColor}, ${$palette.darkAccentColor});
		--accent-color-vegas: light-dark(${$palette.lightAccentColor}, ${$palette.darkAccentColor});
	`,
	css`
		${currentDominantColor && css`--image-dominant-color: ${currentDominantColor};`}
		${!(accentColor === "windows" || accentColor === "wallpaper" && !currentDominantColor) &&
		css`--colorization: if(style(--color-scheme-contrast: true): transparent; else: var(--accent-color));`}
		--accent-color: if(
			style(--color-scheme-contrast: true): ${colors["accent-color"][2]};
			else: ${getAutoColor("accent-color", accentColor)};
		);
		--background-color: if(
			style(--color-scheme-black: true): black;
			style(--color-scheme-contrast: true): ${colors["background-color"][2]};
			else: ${getAutoColor("background-color", backgroundColor)};
		);
	`,
].map((rules, i) => css`
	[data-scheme]${important(i)} {
		${rules}
	}
`));

export default function DynamicAccentColor() {
	const { accentColor, backgroundColor } = useSnapshot(configStore).settings;
	const { currentDominantColor } = useBackgroundImages();
	const resolveViewTransition = useRef<() => void>(undefined);

	useListen("app:startColorPaletteViewTransition", async () => {
		if (resolveViewTransition.current) return; // Avoid recursion, or transitions will break.
		const { promise, resolve } = Promise.withResolvers<void>();
		resolveViewTransition.current = resolve;
		const restoreTransitions = stopTransition();
		try {
			await startColorViewTransition(() => promise, [], { cursor: "wait", types: [] });
		} finally {
			restoreTransitions();
			resolveViewTransition.current = undefined;
		}
	});

	useEffect(() => resolveViewTransition.current?.());

	return <StyledDynamicAccentColor $customize={{ accentColor, backgroundColor, currentDominantColor }} />;
}
