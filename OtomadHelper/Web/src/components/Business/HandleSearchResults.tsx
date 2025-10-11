import { styledSimpleIndicator } from "components/ItemsView/ItemsViewItem";
import { type SettingMeta, search } from "helpers/settings-metas";

const MAX_LENGTH = 10;

const IconWrapper = styled.div({});

const StyledSearchResult = styled.button`
	--icon-size: 20px;
	position: relative;
	display: flex;
	gap: 16px;
	align-items: flex-start;
	inline-size: 100%;
	padding: 7px;
	background-clip: padding-box;
	border: 1px solid transparent;
	border-radius: 4px;

	${styledSimpleIndicator};

	&::before {
		block-size: var(--icon-size);
	}

	&:hover,
	&.selected {
		background-color: ${c("fill-color-subtle-secondary")};
	}

	&:not(.selected):active,
	&.selected:not(:active):hover {
		background-color: ${c("fill-color-subtle-tertiary")};
	}

	&:not(.selected)::before {
		scale: 1 0;
		opacity: 0;
	}

	&:active {
		color: ${c("fill-color-text-secondary")};

		&::before {
			scale: 1 0.625;
		}
	}

	.text {
		display: flex;
		flex-direction: column;
		gap: 2px;
		inline-size: 100%;
	}

	${IconWrapper} {
		container-type: size;
		flex-shrink: 0;
		align-self: stretch;
		block-size: auto;
		inline-size: var(--icon-size);

		.icon {
			translate: 0 4px;
		}
	}

	&.selected ${IconWrapper} .icon,
	&:has(.text .title:only-child) ${IconWrapper} .icon {
		translate: 0 calc((100cqh - 2px - var(--icon-size)) / 2);
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

export default function HandleSearchResults({ query, onSelect, handler }: SearchBox.SearchResultProps) {
	"use no memo";
	const [language] = useLanguage();
	const { goto } = useSnapshot(pageStore);
	const searchResults = useMemo(() => search(query).slice(0, MAX_LENGTH), [query, language]); // When enable React Compiler, it won't care if language changed, and take the useMemo deps alone.
	const [detailsEls, setDetailsEls] = useDomRefs<"p">();
	const [keyboardSelectIndex, setKeyboardSelectIndex] = useState(0);
	const searchResultEl = useDomRef<"button">();

	useEffect(() => {
		setKeyboardSelectIndex(0);
		for (const detailsEl of detailsEls.current) {
			const mark = detailsEl?.querySelector("mark");
			if (!detailsEl || !mark) continue;
			const { offsetLeft } = mark;
			mark.scrollIntoView({ behavior: "instant", inline: "center" });
			if (detailsEl.scrollLeft > offsetLeft) detailsEl.scrollTo({ behavior: "instant", left: offsetLeft });
		}
	}, [query]);

	useEffect(() => {
		const container = searchResultEl.current?.parentElement;
		if (!container) return;
		const item = container.querySelector(".search-result.selected") ?? container.querySelector(".search-result");
		item?.scrollIntoViewIfNeeded();
	}, [query, keyboardSelectIndex]);

	function onUpDown(direction: -1 | 1) {
		setKeyboardSelectIndex(index => floorMod(index + direction, searchResults.length + 1));
	}

	function onEnter() {
		const i = Math.max(0, keyboardSelectIndex - 1);
		if (searchResults[i]) {
			onSelect?.();
			goto(searchResults[i].meta.path);
		}
	}

	useImperativeHandle(handler, () => ({ onUpDown, onEnter }));

	return searchResults.map(({ prop, meta, keyword }, i) => {
		const title = meta.translatedTitle;
		return (
			<StyledSearchResult
				key={meta.path + "—" + prop}
				ref={searchResultEl}
				tabIndex={-1}
				className={{ selected: keyboardSelectIndex - 1 === i }}
				onClick={() => { onSelect?.(); goto(meta.path); setKeyboardSelectIndex(index => index === 0 ? 0 : i + 1); }}
			>
				<IconWrapper>
					{meta.icon && meta.icon !== "placeholder" ? <Icon name={meta.icon} /> : undefined}
				</IconWrapper>
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
