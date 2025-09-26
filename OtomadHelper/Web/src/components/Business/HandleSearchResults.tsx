import { $t, type SettingMeta, search } from "helpers/settings-metas";

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

	.subtitle {
		${styles.effects.text.caption};
		color: ${c("fill-color-text-secondary")};

		&.details {
			${styles.mixins.noScrollbar()};
			${styles.mixins.overflowGradient("x", "1em")};
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

export default function HandleSearchResults({ query }: {
	/** Search keyword. */
	query?: string;
}) {
	const [language] = useLanguage();
	const { goto } = useSnapshot(pageStore);
	const searchResults = useMemo(() => search(query).slice(0, MAX_LENGTH), [query, language]);
	const [detailsEls, setDetailsEls] = useDomRefs<"p">();

	useEffect(() => {
		for (const detailsEl of detailsEls.current)
			detailsEl?.querySelector("mark")?.scrollIntoView({ behavior: "instant", inline: "center" });
	});

	return searchResults.map(([, property, meta, keyword], i) => {
		const title = $t(meta.title);
		return (
			<StyledSearchResult key={meta.path} onClick={() => goto(meta.path)}>
				{meta.icon && meta.icon !== "placeholder" ? <Icon name={meta.icon} /> : <Icon shadow />}
				<div className="text">
					<p className="title">{property === "title" ? <HighlightText wholeText={title} keyword={query} /> : title}</p>
					{property !== "title" && (
						<p className={["subtitle", property]} ref={property === "details" ? setDetailsEls(i) : undefined}>
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
	const path = meta.path!.replace(/[:/][^:/]*?$/, "");
	const [_page = "", _anchor = ""] = path.split(":");
	const pages = _page.split("/").map(subpage => tf.titles[subpage]).toCompacted();
	let metaRoot = _page.split("/").reduce<AnyObject>((root, subpage) => root[subpage], metas);
	const anchors = _anchor.split("/").map(anchor => { metaRoot = metaRoot?.[anchor]; return $t(metaRoot?.meta?.title); }).toCompacted();

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
		.split("\0")
		.map((text, i) => i % 2 ? <mark key={i}>{text}</mark> : text);
	return tokens;
}
