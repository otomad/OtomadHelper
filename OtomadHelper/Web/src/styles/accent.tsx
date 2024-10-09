type AccentPalette = WebMessageEvents.AccentPalette;

const StyledDynamicAccentColor = createGlobalStyle<{
	$palette?: AccentPalette;
}>(({ $palette }) => $palette && css`
	:root${important()} {
		--colorization: ${$palette.colorization};
		--accent-color: ${$palette.darkAccentColor};

		&[data-scheme="light"] {
			--accent-color: ${$palette.lightAccentColor};
		}
	}
`);

export default function DynamicAccentColor() {
	const [palette, setPalette] = useState<AccentPalette>();

	useListen("host:accentPalette", setPalette);

	return <StyledDynamicAccentColor $palette={palette} />;
}
