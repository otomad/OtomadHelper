const StyledSubheader = styled.h4<{
	/** Vertical the text? */
	$vertical?: boolean;
}>`
	--focus-border-radius: 4px;
	inline-size: fit-content;
	margin-block: 10px 4px;
	margin-inline: 2px;
	font-weight: 600;

	&:first-child,
	.settings-page-control + & {
		margin-block-start: 0;
	}

	:is(.command-bar-group, .command-bar) + &,
	:is(.command-bar-group, .command-bar) + .items-view.grid > &:first-child {
		margin-block-start: -25px;
	}

	.items-view.grid > & {
		grid-column: 1 / -1;

		&:first-child {
			margin-inline: 0;
		}
	}

	${ifProp("$vertical", css`
		writing-mode: vertical-lr;
		text-orientation: sideways;

		&:not(:lang(zh), :lang(ja), :lang(ko)) > span {
			display: inline-block;
			rotate: 0.5turn;
		}
	`)}
`;

export default function Subheader({ meta, vertical, children, ...htmlAttrs }: FCP<{
	/** Auto fill props from a setting meta. */
	meta?: PropsOf<typeof Setting>["meta"];
	/** Vertical the text? */
	vertical?: boolean;
	anchor?: string;
}, "h4">) {
	// eslint-disable-next-line no-var
	var { children, anchor } = Setting.useMeta(meta, arguments);
	return (
		<StyledSubheader data-anchor={anchor} $vertical={vertical} {...htmlAttrs}>
			{vertical ? <span>{children}</span> : children}
		</StyledSubheader>
	);
}
