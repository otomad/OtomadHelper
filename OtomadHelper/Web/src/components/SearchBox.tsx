const StyledDataList = styled.div`
	${styles.effects.flyout};
	position: fixed;
	position-anchor: var(--text-box-anchor-name);
	position-area: block-end;
	z-index: 9;
	max-block-size: 100%;
	inline-size: if(
		${ifColorScheme.contrast}: calc(anchor-size(inline) - 2px);
		else: anchor-size(inline);
	);
	padding: 3px;
	overflow-block: auto;
	scroll-behavior: auto;
	border-block-start-width: 0;
	border-start-start-radius: 0;
	border-start-end-radius: 0;
	transform-origin: top;
	transition: ${fallbackTransitions}, inline-size 0s;
	transition-behavior: allow-discrete;

	.text-box:focus-within:has(&) {
		border-end-start-radius: 0;
		border-end-end-radius: 0;
	}

	@starting-style {
		scale: 0.98;
		opacity: 0;
	}

	.text-box:not(:focus-within) & {
		display: none;
		scale: 0.98;
		opacity: 0;
	}
`;

declare global {
	namespace SearchBox { // unplugin-auto-import doesn't support namespace import now.
		interface SearchResultProps {
			/** Search keyword. */
			query?: string;
			/** Occurs when user click the search result item. */
			onSelect?(): void;
			/** A ref to handle keydown events from search box. */
			handler?: RefObject<{
				/** Occurs when user press up or down key. -1: Up; 1: Down. */
				onUpDown(direction: -1 | 1): void;
				/** Occurs when user press enter key. */
				onEnter(): void;
			} | undefined>;
		}
	}
}

export default function SearchBox({ value: [value, setValue], collapsed, collapsedButtonTooltip, enableShortcutKey, placeholder, onCollapsedButtonClick, onSearch, onSearchResultSelect, className, ...htmlAttrs }: FCP<{
	/** The value of the input box. */
	value: StateProperty<string>;
	/** Collapse the search box? */
	collapsed?: boolean;
	/** Custom collapsed button tooltip. */
	collapsedButtonTooltip?: TooltipProps;
	/**
	 * Should listen global search shortcut key (Ctrl + F) to search?
	 * Note that there can only be one search box in a page that can listen to the shortcut key, or they will conflict.
	 */
	enableShortcutKey?: boolean;
	/** Content placeholder shown in the input box. */
	placeholder?: string;
	/** Occurs when the collapsed button clicked. */
	onCollapsedButtonClick?(): void;
	/** Get search results. */
	onSearch?(props: SearchBox.SearchResultProps): ReactNode;
	/** Occurs when user click the search result item. */
	onSearchResultSelect?(): void;
	children?: never;
}, "search">) {
	const [language] = useLanguage();
	const focusSearchBox = () =>
		[...document.querySelectorAll<HTMLInputElement>(`.${nameof.kebab({ SearchBox })} input`)]
			.find(searchBox => searchBox.checkVisibility())?.focus();
	const handleCollapsedButtonClick = async () => {
		if (!onCollapsedButtonClick) return;
		onCollapsedButtonClick();
		await delay(0);
		focusSearchBox();
	};
	const datalistEl = useDomRef<"div">();
	const keyDownHandler = useRef<Unref<SearchBox.SearchResultProps["handler"]>>(undefined);

	const searchResults = useMemo(() => onSearch?.({
		query: value ?? "",
		onSelect: onSearchResultSelect,
		handler: keyDownHandler,
	}), [value, onSearch, onSearchResultSelect]);

	useEffect(() => { setValue?.(""); }, [language]);

	useEventListener(window, "keydown", e => {
		if (!enableShortcutKey || !(e.ctrlKey && e.code === "KeyF")) return;
		e.preventDefault();
		collapsed ? handleCollapsedButtonClick() : focusSearchBox();
	}, undefined, [enableShortcutKey, onCollapsedButtonClick, collapsed]);

	// const scrollDatalistToTop = () => datalistEl.current?.scrollTo({ top: 0, behavior: "instant" });
	// useEffect(() => { scrollDatalistToTop(); delay(0).then(scrollDatalistToTop); }, [value]);

	const handleKeyDown: KeyboardEventHandler<HTMLInputElement> = e => {
		if (e.code.in("ArrowUp", "ArrowDown"))
			keyDownHandler.current?.onUpDown(e.code === "ArrowUp" ? -1 : 1);
		else if (e.code.in("Enter", "NumpadEnter"))
			keyDownHandler.current?.onEnter();
		else
			return;
		e.preventDefault();
	};

	return (
		<search className={["search-box", { collapsed }, className]} aria-label={t.aria.searchBox} {...htmlAttrs}>
			{!collapsed ? (
				<TextBox
					type="search"
					value={[value, setValue]}
					fullWidth
					icon="search"
					showClearAll
					placeholder={placeholder}
					aria-label={t.aria.searchBox}
					customFlyout={value?.trim() && (
						<StyledDataList ref={datalistEl} tabIndex={-1} onMouseDown={mod.prevent()}>
							{searchResults}
							<EmptyMessage.Mini icon="search">{t.noMatchingResults}</EmptyMessage.Mini>
						</StyledDataList>
					)}
					onKeyDown={handleKeyDown}
				/>
			) : (
				<Tooltip {...collapsedButtonTooltip!}>
					<Button subtle minWidthUnbounded icon="search" onClick={handleCollapsedButtonClick} aria-label={t.aria.searchBox} />
				</Tooltip>
			)}
		</search>
	);
}
