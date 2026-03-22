import { styledPreviewFilterBase } from "./PreviewPrve";

const StyledPreviewGraduallyGradient = styled.div<{
	/** Effect name. */
	$effect: string;
}>`
	${styledPreviewFilterBase};

	${({ $effect }) => {
		return {
			saturation: css`
				.overlay {
					background-image: linear-gradient(to right, gray, red);
					mix-blend-mode: saturation;
				}
			`,
			brightness: css`
				.overlay {
					background-image: linear-gradient(to right, white, black);
					mix-blend-mode: hard-light;
				}
			`,
			opacity: css`
				img {
					mask: linear-gradient(to right, black, transparent);
				}
			`,
		}[$effect];
	}}
`;

const showOverlayEffects = ["saturation", "brightness"] as const;

export default function PreviewGraduallyGradient({ thumbnail, effect }: {
	/** Thumbnail. */
	thumbnail: string;
	/** Effect name. */
	effect: string;
}) {
	const webglFilters = useWebglFilters(thumbnail);

	const image = {
		hue: webglFilters?.gradientHue,
		contrast: webglFilters?.gradientContrast,
		threshold: webglFilters?.gradientThreshold,
	}[effect];

	return (
		<StyledPreviewGraduallyGradient $effect={effect}>
			<img alt="" data-name={effect} src={image || thumbnail} />
			{showOverlayEffects.includes(effect) && <div className="overlay" />}
		</StyledPreviewGraduallyGradient>
	);
}

const StyledPreviewTwistEffect = styled.div`
	${styledPreviewFilterBase};
`;

export function PreviewTwistEffect({ thumbnail, direction, className }: {
	/** Thumbnail. */
	thumbnail: string;
	/** Twist direction: Clockwise or Counterclockwise. */
	direction: "cw" | "ccw";
	className?: ClassValue;
}) {
	const webglFilters = useWebglFilters(thumbnail);
	const image = direction === "ccw" ? webglFilters?.twist_ccw : webglFilters.twist;
	return (
		<StyledPreviewTwistEffect className={className}>
			<img alt="" src={image || thumbnail} />
		</StyledPreviewTwistEffect>
	);
}
