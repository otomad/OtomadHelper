const graphs = (name: string) => import.meta.glob<FC<{}, "svg">>("/src/assets/svg/graphs/*.svg", { import: "default", eager: true, query: "?react" })[`/src/assets/svg/graphs/${name}.svg`];

const CONTAINER_PADDING = 8;

const StyledPreviewGraph = styled.i.attrs({
	className: "icon",
})`
	margin: ${-CONTAINER_PADDING}px;
	margin-inline: -10px -2px;
	padding: ${CONTAINER_PADDING}px;
	background-color: ${getClearColorFromBackgroundColor("color", 0.5)};
	border-radius: 2px;

	svg {
		${styles.mixins.square("54px")};
		display: block;
		stroke: ${c("color")};
	}
`;

export default function PreviewGraph({ name, color }: FCP<{
	/** Graph name. */
	name: string;
	/** Graph stroke color. */
	color?: string;
}>) {
	const Graph = graphs(name);
	return (
		<StyledPreviewGraph style={{ "--color": color }}>
			<Graph />
		</StyledPreviewGraph>
	);
}
