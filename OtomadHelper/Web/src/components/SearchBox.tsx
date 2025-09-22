export default function SearchBox({ value, collapsed, collapsedButtonTooltip, enableShortcutKey, onCollapsedButtonClick, className, ...htmlAttrs }: FCP<{
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
	/** Occurs when the collapsed button clicked. */
	onCollapsedButtonClick?(): void;
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
					value={value}
					fullWidth
					icon="search"
					showClearAll
					aria-label={t.aria.searchBox}
				/>
			) : (
				<Tooltip {...collapsedButtonTooltip!}>
					<Button subtle minWidthUnbounded icon="search" onClick={handleCollapsedButtonClick} aria-label={t.aria.searchBox} />
				</Tooltip>
			)}
		</search>
	);
}
