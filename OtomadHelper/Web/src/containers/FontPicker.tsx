export default function FontPicker({ font }: {
	/** Font family. */
	font: VariousState<string>;
}) {
	const [fonts, setFonts] = useAtom(fontsAtom);
	const fontNames = useFontDisplayNames();

	async function updateLocalFonts() {
		setFonts(await queryLocalFonts());
	}

	return (
		<ComboBox
			forceBaseSelectAppearance
			style={{ inlineSize: "100%" }}
			current={font}
			ids={fonts.map(font => font.family)}
			options={fontNames}
			optionAttrs={fontFamily => ({ style: { fontFamily: getCssFontFamily(fontFamily) } })}
			onClick={updateLocalFonts}
		/>
	);
}

interface LocalFontData {
	readonly family: string;
	readonly displayName: string;
}

const
	DEFAULT_FONT_KEY = "",
	SYSTEM_FONT_KEY = "-system",
	SYSTEM_FONT_FAMILY = "ui-sans-serif, system-ui";
const defaultFontFamilies = (): LocalFontData[] => [
	{ family: DEFAULT_FONT_KEY, displayName: "" },
	{ family: SYSTEM_FONT_KEY, displayName: "" },
];
const useDefaultFontFamiliesDisplayName = () => {
	const t = useT();
	return {
		defaultFontFamilyDisplayName: t.settings.appearance.defaultFontFamily,
		systemFontFamilyDisplayName: t.settings.appearance.systemFontFamily,
	};
};

const fontsAtom = atom(defaultFontFamilies());

async function queryLocalFonts() {
	const localFonts = await window.queryLocalFonts();
	const fontFamiliesMap = new Map<string, LocalFontData>();
	for (const { family, fullName, style } of localFonts)
		if (!fontFamiliesMap.has(family))
			fontFamiliesMap.set(family, {
				family,
				displayName: fullName.includes(family) ? family : fullName.replaceEnd(style).trim(),
			});
	return [...defaultFontFamilies(), ...fontFamiliesMap.values()];
}

export function useFontDisplayName(fontFamily: string) {
	const { defaultFontFamilyDisplayName, systemFontFamilyDisplayName } = useDefaultFontFamiliesDisplayName();
	const fonts = useAtomValue(fontsAtom);
	if (!fontFamily) return defaultFontFamilyDisplayName;
	const font = fonts.find(font => font.family === fontFamily);
	if (!font) return fontFamily;
	if (font.family === DEFAULT_FONT_KEY) return defaultFontFamilyDisplayName;
	else if (font.family === SYSTEM_FONT_KEY) return systemFontFamilyDisplayName;
	else return font.displayName;
}

function useFontDisplayNames() {
	const { defaultFontFamilyDisplayName, systemFontFamilyDisplayName } = useDefaultFontFamiliesDisplayName();
	const fonts = useAtomValue(fontsAtom);
	return fonts.map(font => {
		if (font.family === DEFAULT_FONT_KEY) return defaultFontFamilyDisplayName;
		else if (font.family === SYSTEM_FONT_KEY) return systemFontFamilyDisplayName;
		else return font.displayName;
	});
}

function getCssFontFamily(fontFamily: string) {
	return fontFamily === DEFAULT_FONT_KEY ? "" : fontFamily === SYSTEM_FONT_KEY ? SYSTEM_FONT_FAMILY : JSON.stringify(fontFamily);
}

export function applyFontFamilyToDocument(fontFamily: string) {
	document.documentElement.style.setProperty("--custom-font-family", getCssFontFamily(fontFamily));
}
