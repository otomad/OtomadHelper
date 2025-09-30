const StyledSubheader = styled.h4`
	--focus-border-radius: 4px;
	inline-size: fit-content;
	margin-block: 10px 4px;
	margin-inline: 2px;
	font-weight: 600;

	&:first-child:not(.items-view.grid > *),
	.items-view.grid:first-child > &,
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
`;

export default function Subheader({ meta, children, ...htmlAttrs }: FCP<{
	/** Auto fill props from a setting meta. */
	meta?: PropsOf<typeof Setting>["meta"];
	anchor?: string;
}, "h4">) {
	// eslint-disable-next-line no-var
	var { children, anchor } = Setting.useMeta(meta, arguments);
	return <StyledSubheader data-anchor={anchor} {...htmlAttrs}>{children}</StyledSubheader>;
}
