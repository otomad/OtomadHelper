const InlineBlock = styled.span`
	display: inline-block; // Allow transform for span (inline).
	white-space: pre-wrap; // Do not remove the spaces.
`;

export default function LetterByLetter({ as: As = "p", children = "", granularity = "grapheme", className, ...htmlAttrs }: FCP<{
	/** Specify the wrapper type. */
	as?: AsTarget;
	/** The children must be string. */
	children?: string;
	/** The type of input to be split. */
	granularity?: Intl.SegmenterOptions["granularity"];
}, "p">) {
	const [language] = useLanguage();

	const characters = useMemo(() => {
		const segmenter = new Intl.Segmenter(language, { granularity });
		return [...segmenter.segment(children)].map(({ segment, index }) =>
			<InlineBlock key={index}>{segment}</InlineBlock>);
	}, [children, granularity, language]);

	return (
		<As className={[className, nameof.kebab({ LetterByLetter })]} {...htmlAttrs as object}>
			{characters}
		</As>
	);
}
