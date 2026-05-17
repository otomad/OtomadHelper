const StyledCurvesWrapper = styled.i`
	${styles.mixins.square("1em")};
	display: grid;

	svg {
		${styles.mixins.square("inherit")};
		grid-area: 1 / 1;
		fill: none;
		stroke: currentColor;

		path {
			stroke-width: --light-dark(1, 1.5);
		}

		&:nth-of-type(2) {
			scale: 1 -1;
		}
	}
`;

const paths = {
	linear: "M1.5 14.5H1.5C1.5 14.5 14.5 1.5 14.5 1.5V1.5",
	fast: "M1.5 14.5H1.5C1.5 7.3203 7.3203 1.5 14.5 1.5V1.5",
	slow: "M1.5 14.5H1.5C8.6797 14.5 14.5 8.6797 14.5 1.5V1.5",
	smooth: "M1.5 14.5H1.5C9.5 14.5 6.5 1.5 14.5 1.5V1.5",
	sharp: "M1.5 14.5H1.5C1.5 6.5 14.5 9.5 14.5 1.5V1.5",
	hold: "M1.5 14.5H13.75C14.1642 14.5 14.5 14.1642 14.5 13.75V1.5",
};

type CurveType = keyof typeof paths;

function SingleCurve({ curve, className, "aria-label": ariaLabel }: {
	curve: CurveType;
	className?: ClassValue;
	"aria-label"?: string;
}) {
	const path = paths[curve];
	return (
		<svg role="img" aria-label={ariaLabel} className={className} viewBox="0 0 16 16" xmlns="http://www.w3.org/2000/svg">
			<path d={path} strokeLinecap="round" />
		</svg>
	);
}

export default function PreviewCurve({ multiplicandCurve, reciprocalCurve, className, ...htmlAttrs }: FCP<{
	multiplicandCurve: CurveType;
	reciprocalCurve?: CurveType;
}, "i">) {
	return (
		<StyledCurvesWrapper className={[className, "preview-curve", "synthetic-icon"]} {...htmlAttrs}>
			<SingleCurve curve={multiplicandCurve} />
			{reciprocalCurve && <SingleCurve curve={reciprocalCurve} />}
		</StyledCurvesWrapper>
	);
}
