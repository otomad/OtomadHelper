const ICON_INITIAL_SIZE = 16;
const STROKE_DASHARRAY = 15 * Math.SQRT2;

const StyledIconOff = styled.div`
	display: inline-flex;

	> svg {
		--shadow-stroke-width: 1.5;
		${styles.mixins.square("1em")};
		fill: currentColor;

		${ifColorScheme.light} & {
			--shadow-stroke-width: 1;
		}
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
			<svg width={ICON_INITIAL_SIZE} height={ICON_INITIAL_SIZE} viewBox={`0 0 ${ICON_INITIAL_SIZE} ${ICON_INITIAL_SIZE}`} xmlns="http://www.w3.org/2000/svg">
				<mask id={maskId}>
					<g fill="white" dangerouslySetInnerHTML={{ __html: svgPath }} />
					<g
						fill="none" strokeDasharray={STROKE_DASHARRAY} strokeDashoffset={STROKE_DASHARRAY} strokeLinecap="round" strokeLinejoin="round"
						strokeWidth={1}
					>
						<path stroke="black" d="M1.21 -0.21L16.21 14.79" strokeWidth="var(--shadow-stroke-width)" />
						<path stroke="white" d="M0.5 0.5L15.5 15.5" />
						<animate
							fill="freeze"
							attributeName="stroke-dashoffset"
							dur="250ms"
							values={`${STROKE_DASHARRAY}; 0`}
							calcMode="spline"
							keySplines="0.5 0 0 1"
						/>
					</g>
				</mask>
				<rect width={ICON_INITIAL_SIZE} height={ICON_INITIAL_SIZE} fill="currentColor" mask={`url(#${maskId})`} />
			</svg>
		</StyledIconOff>
	);
}
