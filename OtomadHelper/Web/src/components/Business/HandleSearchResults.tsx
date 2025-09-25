import { $t, search } from "helpers/settings-metas";

const MAX_LENGTH = 10;

const StyledSearchResult = styled.button`
	inline-size: 100%;
	padding: 8px;
	border-radius: 4px;

	&:hover {
		background-color: ${c("fill-color-subtle-secondary")};
	}

	&:active {
		color: ${c("fill-color-text-secondary")};
		background-color: ${c("fill-color-subtle-tertiary")};
	}
`;

export default function HandleSearchResults({ keyword }: {
	/** Search keyword. */
	keyword?: string;
}) {
	const { goto } = useSnapshot(pageStore);
	const searchResults = useMemo(() => search(keyword).slice(0, MAX_LENGTH), [keyword]);

	return searchResults.map(([, meta]) => (
		<StyledSearchResult key={meta.path} onClick={() => goto(meta.path)}>
			{$t(meta.title)}
		</StyledSearchResult>
	));
}
