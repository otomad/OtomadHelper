const StyledSearchBox = styled.search`
	margin-block: 4px 8px;
	margin-inline: 9px;

	.text-box .leading-icon {
		margin-inline-end: 4px;
	}
`;

export default function SearchBox({ value, collapsed, onCollapsedButtonClick, ...htmlAttrs }: FCP<{
	/** The value of the input box. */
	value: StateProperty<string>;
	/** Collapse the search box?  */
	collapsed?: boolean;
	/** Occurs when the collapsed button clicked. */
	onCollapsedButtonClick?(): void;
	children?: never;
}, "search">) {
	const textBoxEl = useDomRef<"input">();

	return (
		<StyledSearchBox {...htmlAttrs}>
			{!collapsed ?
				<TextBox inputRef={textBoxEl} value={value} fullWidth icon="search" showClearAll /> :
				<Button subtle minWidthUnbounded icon="search" onClick={() => { onCollapsedButtonClick?.(); textBoxEl.current?.focus(); }} />}
		</StyledSearchBox>
	);
}
