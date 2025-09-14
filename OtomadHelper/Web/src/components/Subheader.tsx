const Subheader = styled.h4`
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

export default Subheader;
