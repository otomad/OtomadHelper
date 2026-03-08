export default function FontPicker({ font }: {
	/** Font family. */
	font: StatePropertyNonNull<string>;
}) {
	const [fonts, setFonts] = useAtom(fontsAtom);

	async function updateLocalFonts() {
		setFonts(await queryLocalFonts());
	}

	return (
		<ComboBox
			style={{ inlineSize: "100%" }}
			current={font}
			ids={fonts.map(font => font.family)}
			options={fonts.map(font => font.displayName)}
			optionAttrs={fontFamily => ({ style: { fontFamily } })}
			onClick={updateLocalFonts}
		/>
	);
}

interface LocalFontData {
	readonly family: string;
	readonly displayName: string;
}

const defaultFontFamily = (): LocalFontData => ({ family: "", displayName: t.settings.appearance.defaultFontFamily });

const fontsAtom = atom([defaultFontFamily()]);

async function queryLocalFonts() {
	const localFonts = await window.queryLocalFonts();
	const fontFamiliesMap = new Map<string, LocalFontData>();
	for (const { family, fullName, style } of localFonts)
		if (!fontFamiliesMap.has(family))
			fontFamiliesMap.set(family, {
				family,
				displayName: fullName.replaceEnd(" " + style).replaceEnd(style),
			});
	return [defaultFontFamily(), ...fontFamiliesMap.values()];
}
