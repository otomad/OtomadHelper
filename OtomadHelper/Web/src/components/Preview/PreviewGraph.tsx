const graphs = (name: string) => import.meta.glob<GetReactElementFromTag<"svg">>("./*.svg", { base: "/src/assets/svg/graphs", import: "default", eager: true, query: "?react" })[`./${name}.svg`];

const CONTAINER_PADDING = 8;

const StyledPreviewGraph = styled.i.attrs({
	className: "icon",
})`
	margin: ${-CONTAINER_PADDING}px;
	margin-inline: -10px -2px;
	padding: ${CONTAINER_PADDING}px;
	background-color: rgb(from contrast-color(var(--color)) r g b / 50%);
	border-radius: 2px;
	forced-color-adjust: none;

	svg {
		${styles.mixins.square("54px")};
		display: block;
		stroke: var(--color);
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
