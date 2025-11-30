export const Encodings = Enum({
	ANSI: { value: 0, charset: undefined, aliases: ["ANSI"] },
	"UTF-8": { value: 65001, charset: undefined, aliases: [] },
	Shift_JIS: { value: 932, charset: "Jpan", aliases: [] },
	GBK: { value: 936, charset: "Hans", aliases: ["GB18030", "GB2312"] },
	Big5: { value: 950, charset: "Hant", aliases: [] },
	"EUC-KR": { value: 949, charset: "Kore", aliases: ["KS X 1001", "KS_C_5601-1987"] },
	"Windows-1252": { value: 1252, charset: "Latn", aliases: ["ISO-8859-1"] },
	Macintosh: { value: 10000, charset: "Latn", aliases: ["Mac OS Roman"] },
	"UTF-16BE": { value: 1201, charset: undefined, aliases: ["BigEndianUnicode"] },
	"UTF-16LE": { value: 1200, charset: undefined, aliases: ["Unicode"] },
	"KOI8-R": { value: 20866, charset: "Cyrl", aliases: [] },
	"Windows-874": { value: 874, charset: "Thai", aliases: [] },
	"Windows-1250": { value: 1250, charset: "Latn", aliases: [] },
	"Windows-1251": { value: 1251, charset: "Cyrl", aliases: [] },
	"Windows-1254": { value: 1254, charset: "tr", aliases: [] },
	"Windows-1255": { value: 1255, charset: "Hebr", aliases: [] },
	"Windows-1256": { value: 1256, charset: "Arab", aliases: [] },
	"Windows-1257": { value: 1257, charset: "Latn", aliases: [] },
	"Windows-1258": { value: 1258, charset: "vi", aliases: [] },
	"ISO-8859-2": { value: 28592, charset: "Latn", aliases: [] },
	"ISO-8859-7": { value: 28597, charset: "Grek", aliases: [] },
	"ISO-8859-8": { value: 28598, charset: "Hebr", aliases: [] },
});

const StyledPreviewEncoding = styled.div`
	position: relative;

	.title,
	.details {
		display: flex !important;
		gap: 8px;
		align-items: center;
	}

	.charset {
		color: ${c("fill-color-text-secondary")};
	}

	span {
		display: inline-block;
	}

	.details {
		gap: 10px;
	}

	.garbled {
		${styles.effects.text.title};
		position: absolute;
		inset-block: -100%;
		inset-inline-end: 0;
		align-content: center;
		font-weight: 200;
		opacity: 0.25;
		pointer-events: none;
	}
`;

export default function PreviewEncoding({ encoding }: FCP<{
	/** Text encoding. */
	encoding: Config.Encoding;
	children?: never;
}, "div">) {
	const [language] = useLanguage();
	const { ariaId } = useContext(RadioButton.Context);
	const encodingInfo = Encodings.all[encoding];
	const charset = useMemo(() => {
		const charsetCode = encodingInfo.charset;
		if (!charsetCode) return;
		else if (charsetCode.length === 4) return new Intl.DisplayNames(language, { type: "script" }).of(charsetCode);
		else return new Intl.DisplayNames(language, { type: "language" }).of(charsetCode);
	}, [language, encodingInfo]);

	return (
		<StyledPreviewEncoding className="text" aria-hidden>
			<p className="title">
				<span id={`${ariaId}-title`}>{encoding === "ANSI" ? t.systemDefault : encoding}</span>
				{charset && <span className="charset">{charset}</span>}
				<Badge>{encodingInfo.value}</Badge>
			</p>
			<p className="details">
				{encodingInfo.aliases.map(alias => <span key={alias}>{alias}</span>)}
			</p>
			<p className="garbled">{锟斤拷(encoding)}</p>
		</StyledPreviewEncoding>
	);
}

function 锟斤拷(encoding: string) {
	// cspell:disable-next-line
	const replacementCharacter = "efbfbd"; // U+FFFD
	let decoder: TextDecoder;
	try {
		decoder = new TextDecoder(encoding);
	} catch {
		return "";
	}
	let result = decoder.decode(Uint8Array.fromHex(replacementCharacter));
	if (result.codePointAt(result.length - 1) === 0xfffd)
		result = decoder.decode(Uint8Array.fromHex(replacementCharacter.repeat(2)));
	return result;
}
