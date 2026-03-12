export default function FontPicker({ font }: {
	/** Font family. */
	font: StatePropertyNonNull<string>;
}) {
	const [fonts, setFonts] = useAtom(fontsAtom);
	const defaultFontFamilyDisplayName = useDefaultFontFamilyDisplayName();

	async function updateLocalFonts() {
		setFonts(await queryLocalFonts());
	}

	return (
		<ComboBox
			forceBaseSelectAppearance
			style={{ inlineSize: "100%" }}
			current={font}
			ids={fonts.map(font => font.family)}
			options={fonts.map(font => font.displayName || defaultFontFamilyDisplayName)}
			optionAttrs={fontFamily => ({ style: { fontFamily } })}
			onClick={updateLocalFonts}
		/>
	);
}

interface LocalFontData {
	readonly family: string;
	readonly displayName: string;
}

const defaultFontFamily = (): LocalFontData => ({ family: "", displayName: "" });
const useDefaultFontFamilyDisplayName = () => { const t = useT(); return t.settings.appearance.defaultFontFamily; };

const fontsAtom = atom([defaultFontFamily()]);

async function queryLocalFonts() {
	const localFonts = await window.queryLocalFonts();
	const fontFamiliesMap = new Map<string, LocalFontData>();
	for (const { family, fullName, style } of localFonts)
		if (!fontFamiliesMap.has(family))
			fontFamiliesMap.set(family, {
				family,
				displayName: fullName.includes(family) ? family : fullName.replaceEnd(style).trim(),
			});
	return [defaultFontFamily(), ...fontFamiliesMap.values()];
}

export function useFontDisplayName(fontFamily: string) {
	const defaultFontFamilyDisplayName = useDefaultFontFamilyDisplayName();
	const [fonts] = useAtom(fontsAtom);
	if (!fontFamily) return defaultFontFamilyDisplayName;
	return (fonts.find(font => font.family === fontFamily)?.displayName ?? fontFamily) || defaultFontFamilyDisplayName;
}
