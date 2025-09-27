import { type SettingMeta, search } from "helpers/settings-metas";

const MAX_LENGTH = 10;

const StyledSearchResult = styled.button`
	display: flex;
	gap: 16px;
	align-items: flex-start;
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

	.text {
		display: flex;
		flex-direction: column;
		gap: 2px;
		inline-size: 100%;
	}

	> .icon {
		flex-shrink: 0;
		margin-block-start: 4px;
	}

	&:has(.text .title:only-child) > .icon {
		align-self: center;
		margin-block-start: 0;
	}

	.subtitle {
		${styles.effects.text.caption};
		color: ${c("fill-color-text-secondary")};

		&.details {
			${styles.mixins.noScrollbar()};
			${styles.mixins.overflowGradient("x", "1em")};
			position: relative;
			contain: inline-size;
			overflow-inline: scroll;
			pointer-events: none;
			interactivity: inert;

			&,
			* {
				white-space: nowrap;
			}
		}
	}
`;

export default function HandleSearchResults({ query, onSearchResultSelect }: {
	/** Search keyword. */
	query?: string;
	/** Occurs when user click the search result item. */
	onSearchResultSelect?(): void;
}) {
	const [language] = useLanguage();
	const { goto } = useSnapshot(pageStore);
	const searchResults = useMemo(() => search(query).slice(0, MAX_LENGTH), [query, language]);
	const [detailsEls, setDetailsEls] = useDomRefs<"p">();

	useEffect(() => {
		for (const detailsEl of detailsEls.current) {
			const mark = detailsEl?.querySelector("mark");
			if (!detailsEl || !mark) continue;
			const { offsetLeft } = mark;
			mark.scrollIntoView({ behavior: "instant", inline: "center" });
			if (detailsEl.scrollLeft > offsetLeft) detailsEl.scrollTo({ behavior: "instant", left: offsetLeft });
		}
	});

	return searchResults.map(({ prop, meta, keyword }, i) => {
		const title = meta.translatedTitle;
		return (
			<StyledSearchResult key={meta.path} onClick={() => { onSearchResultSelect?.(); goto(meta.path); }}>
				{meta.icon && meta.icon !== "placeholder" ? <Icon name={meta.icon} /> : <Icon shadow />}
				<div className="text">
					<p className="title">{prop === "title" ? <HighlightText wholeText={title} keyword={query} /> : title}</p>
					{prop !== "title" && (
						<p className={["subtitle", prop]} ref={prop === "details" ? setDetailsEls(i) : undefined}>
							<HighlightText wholeText={keyword} keyword={query} />
						</p>
					)}
					<SearchResultPath meta={meta} />
				</div>
			</StyledSearchResult>
		);
	});
}

const StyledSearchResultPath = styled.div`
	${styles.effects.text.caption};
	color: ${c("fill-color-text-secondary")};

	.icon {
		display: inline-flex;
		margin-inline: 4px;
		font-size: 10px;
		vertical-align: -1.25px;

		&:is(:lang(zh), :lang(ja), :lang(ko)) {
			vertical-align: -0.75px;
		}
	}
`;

function SearchResultPath({ meta }: { meta: SettingMeta }) {
	const { pages, anchors } = meta.translatedPath;
	if (pages.length === 0 && anchors.length === 0) return;

	return (
		<StyledSearchResultPath>
			{pages.interpose(i => <Icon key={`sep-page-${i}`} name="chevron_right" />)}
			{pages.length > 0 && anchors.length > 0 && <Icon name="chevron_double_right" />}
			{anchors.interpose(i => <Icon key={`sep-anchor-${i}`} name="chevron_right" />)}
		</StyledSearchResultPath>
	);
}

function HighlightText({ wholeText, keyword }: { wholeText?: string; keyword?: string }) {
	keyword = keyword?.trim().replaceAll(/\s{2,}/g, " ");
	if (!wholeText || !keyword) return;
	const tokens = wholeText
		.replaceAll(new RegExp(RegExp.escape(keyword), "gi"), "\0$&\0")
		.replaceAll(/\0{2,}/g, "")
		.split("\0")
		.map((text, i) => i % 2 ? <mark key={i}>{text}</mark> : text);
	return tokens;
}
