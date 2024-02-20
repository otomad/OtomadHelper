const StyledIconOff = styled.div`
	display: inline-flex;

	> svg {
		${styles.mixins.square("1em")};
		fill: currentColor;
	}
`;

export default function IconOff({ name }: FCP<{
	/** 图标。 */
	name: string;
}>) {
	const [svgPath, setSvgPath] = useState("");
	const symbolId = getIconSymbolId(name);
	const maskId = useId();

	useEffect(() => {
		const path = document.querySelector(symbolId)?.innerHTML;
		if (!path) return;
		setSvgPath(path);
	}, [name]);

	return (
		<StyledIconOff>
			<svg width={16} height={16} viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg">
				<mask id={maskId}>
					<g dangerouslySetInnerHTML={{ __html: svgPath }} />
					<g
						fill="none" strokeDasharray={26} strokeDashoffset={26} strokeLinecap="round" strokeLinejoin="round"
						strokeWidth={1}
					>
						<path stroke="black" d="M1.21 -0.21L16.21 14.79" strokeWidth={2} />
						<path stroke="white" d="M0.5 0.5L15.5 15.5" />
						<animate
							fill="freeze" attributeName="stroke-dashoffset" dur="250ms" values="26;0" calcMode="spline"
							keySplines="1 0 0 1"
						/>
					</g>
				</mask>
				<rect width={16} height={16} fill="currentColor" mask={`url(#${maskId})`} />
			</svg>
		</StyledIconOff>
	);
}
