import { PREVIEW_LANGUAGE_TEXT_MARGIN as TEXT_MARGIN } from "./PreviewLanguage";

export const Encodings = Enum({
	ANSI: { value: 0, label: t.default, aliases: ["ANSI"], tags: [] },
	"UTF-8": { value: 65001, label: "$unicode", aliases: [], tags: ["unicode"] },
	Shift_JIS: { value: 932, label: "Jpan", aliases: [], tags: [] },
	GBK: { value: 936, label: "$hans", aliases: ["GB18030", "GB2312"], tags: ["chinese"] },
	Big5: { value: 950, label: "$hant", aliases: [], tags: ["chinese"] },
	"EUC-KR": { value: 949, label: "Kore", aliases: ["KS X 1001", "KS_C_5601-1987"], tags: [] },
	"Windows-1252": { value: 1252, label: "$westernEuropean", aliases: ["ISO-8859-1"], tags: ["windows", "latin"] },
	Macintosh: { value: 10000, label: "$westernEuropean", aliases: ["Mac OS Roman"], tags: ["latin"] },
	"UTF-16BE": { value: 1201, label: "$utf16Be", aliases: ["BigEndianUnicode"], tags: ["unicode"] },
	"UTF-16LE": { value: 1200, label: "$utf16Le", aliases: ["Unicode"], tags: ["unicode"] },
	"KOI8-R": { value: 20866, label: "Cyrl", aliases: [], tags: [] },
	"Windows-874": { value: 874, label: "Thai", aliases: [], tags: ["windows"] },
	"Windows-1250": { value: 1250, label: "$centralEuropean", aliases: [], tags: ["windows", "latin"] },
	"Windows-1251": { value: 1251, label: "Cyrl", aliases: [], tags: ["windows"] },
	"Windows-1253": { value: 1253, label: "Grek", aliases: [], tags: ["windows"] },
	"Windows-1254": { value: 1254, label: "$tr", aliases: [], tags: ["windows", "latin"] },
	"Windows-1255": { value: 1255, label: "Hebr", aliases: [], tags: ["windows"] },
	"Windows-1256": { value: 1256, label: "Arab", aliases: [], tags: ["windows"] },
	"Windows-1257": { value: 1257, label: "$bat", aliases: [], tags: ["windows", "latin"] },
	"Windows-1258": { value: 1258, label: "$vi", aliases: [], tags: ["windows", "latin"] },
	"ISO-8859-2": { value: 28592, label: "$centralEuropean", aliases: [], tags: ["iso", "latin"] },
	"ISO-8859-7": { value: 28597, label: "Grek", aliases: [], tags: ["iso"] },
	"ISO-8859-8": { value: 28598, label: "Hebr", aliases: [], tags: ["iso"] },
}, { localize: localizeCharset });

export const EncodingTags = Enum({
	all: { label: t.all },
	unicode: { label: "$unicode" },
	chinese: { label: "zh" },
	latin: { label: "Latn" },
	windows: { label: "@Windows" },
	iso: { label: "@ISO" },
}, { localize: localizeCharset });

export type EncodingTagGroup = typeof EncodingTags.keyType;

function localizeCharset(label: unknown): string {
	if (typeof label === "function") return label();
	const charsetCode = (label as object)?.toString();
	const { language } = i18n;
	if (charsetCode.startsWith("@")) return charsetCode.slice(1);
	else if (charsetCode.startsWith("$")) return t.charsets[charsetCode.slice(1)];
	else if (charsetCode.length === 4) return new Intl.DisplayNames(language, { type: "script" }).of(charsetCode)!;
	else return new Intl.DisplayNames(language, { type: "language" }).of(charsetCode)!;
}

const StyledPreviewEncoding = styled.div`
	position: relative;
	height: 100%;
	padding-block: ${TEXT_MARGIN[1]}px;
	padding-inline: ${TEXT_MARGIN[0]}px;
	border-radius: inherit;

	.text:not(.badge *) {
		&.title {
			${styles.effects.text.subtitle};
		}

		&.alias {
			${styles.effects.text.bodyStrong};
		}

		.items-view-item.selected & {
			color: ${c("accent-color")};
		}
	}

	.badge {
		float: inline-end;
		margin-block-start: 4px;

		.items-view-item.selected & {
			--status: accent;
		}
	}

	.garbled {
		${styles.effects.text.title};
		position: absolute;
		inset-block-end: ${TEXT_MARGIN[1]}px;
		inset-inline-end: ${TEXT_MARGIN[0]}px;
		z-index: -1;
		opacity: 0.3;
	}

	.items-view-item.grid:has(&) {
		.text-part .text {
			padding-inline-start: ${TEXT_MARGIN[0] + 1}px;
		}

		&.selected .text-part .title {
			color: ${c("accent-color")};
		}
	}
`;

export default function PreviewEncoding({ encoding }: FCP<{
	/** Text encoding. */
	encoding: Config.Encoding;
	children?: never;
}, "div">) {
	const encodingInfo = Encodings.all[encoding];

	return (
		<StyledPreviewEncoding>
			<Badge transitionOnAppear={false}>{encodingInfo.value}</Badge>
			<p className="text title">{encoding === "ANSI" ? t.systemDefault : encoding}</p>
			{encodingInfo.aliases.map(alias => <p className="text alias" key={alias}>{alias}</p>)}
			<p className="text garbled">{锟斤拷(encoding)}</p>
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

const StyledEncodingFilterExpanderChildWrapper = styled(Expander.ChildWrapper)`
	& + * {
		border-block-start: none !important;
	}

	@media (width < 641px) {
		padding-inline: 15px;
	}
`;

export function EncodingFilter({ current }: { current: StateProperty<EncodingTagGroup> }) {
	return (
		<StyledEncodingFilterExpanderChildWrapper>
			<Filter current={current}>
				{EncodingTags.map(({ key, label }) => <Filter.Item key={key} id={key}>{label}</Filter.Item>)}
			</Filter>
		</StyledEncodingFilterExpanderChildWrapper>
	);
}
