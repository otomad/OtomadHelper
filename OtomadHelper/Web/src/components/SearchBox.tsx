const StyledDataList = styled.div`
	${styles.effects.flyout};
	position: fixed;
	position-anchor: var(--text-box-anchor-name);
	position-area: block-end;
	z-index: 9;
	max-block-size: 100%;
	inline-size: anchor-size(inline);
	padding: 4px;
	overflow-block: auto;
	border-block-start-width: 0;
	border-start-start-radius: 0;
	border-start-end-radius: 0;
	transform-origin: top;
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

	&:empty::after {
		${styles.effects.text.body};
		content: attr(data-empty);
		display: block;
		margin-block: 0.5lh;
		color: ${c("fill-color-text-tertiary")};
		font-style: italic;
		text-align: center;
	}
`;

export default function SearchBox({ value: [value, setValue], collapsed, collapsedButtonTooltip, enableShortcutKey, placeholder, onCollapsedButtonClick, onSearch, className, ...htmlAttrs }: FCP<{
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
	onSearch?(keyword: string): ReactNode;
	children?: never;
}, "search">) {
	const focusSearchBox = () =>
		[...document.querySelectorAll<HTMLInputElement>(`.${nameof.kebab({ SearchBox })} input`)]
			.find(searchBox => searchBox.checkVisibility())?.focus();
	const handleCollapsedButtonClick = async () => {
		if (!onCollapsedButtonClick) return;
		onCollapsedButtonClick();
		await delay(0);
		focusSearchBox();
	};

	const searchResults = useMemo(() => onSearch?.(value ?? ""), [value, onSearch]);

	useEventListener(window, "keydown", e => {
		if (!enableShortcutKey || !(e.ctrlKey && e.code === "KeyF")) return;
		e.preventDefault();
		collapsed ? handleCollapsedButtonClick() : focusSearchBox();
	}, undefined, [enableShortcutKey, onCollapsedButtonClick, collapsed]);

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
						<StyledDataList data-empty={t.noMatchingResults} onMouseDown={e => e.preventDefault()}>
							{searchResults}
						</StyledDataList>
					)}
				/>
			) : (
				<Tooltip {...collapsedButtonTooltip!}>
					<Button subtle minWidthUnbounded icon="search" onClick={handleCollapsedButtonClick} aria-label={t.aria.searchBox} />
				</Tooltip>
			)}
		</search>
	);
}
