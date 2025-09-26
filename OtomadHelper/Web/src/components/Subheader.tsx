import { $t } from "helpers/settings-metas";

const StyledSubheader = styled.h4`
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
	meta?: PropsOf<typeof Setting>["meta"];
}, "h4">) {
	children ??= $t(meta?.meta.title);
	return <StyledSubheader data-anchor={CSS_escape(meta?.meta.path)} {...htmlAttrs}>{children}</StyledSubheader>;
}
