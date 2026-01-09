const StyledPreviewGraduallyGradient = styled.div<{
	/** Effect name. */
	$effect: string;
}>`
	${styles.mixins.square("100%")};

	img,
	.overlay {
		${styles.mixins.square("100%")};
		position: absolute;
		object-fit: cover;
	}

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

export default function PreviewGraduallyGradient({ thumbnail, effect }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
	/** Effect name. */
	effect: string;
}>) {
	const webglFilters = useWebglFilters(thumbnail);

	const alterImage = {
		hue: webglFilters?.gradientHue,
		contrast: webglFilters?.gradientContrast,
		threshold: webglFilters?.gradientThreshold,
	}[effect];

	return (
		<StyledPreviewGraduallyGradient $effect={effect}>
			<img alt="" data-name={effect} src={alterImage || thumbnail} />
			{showOverlayEffects.includes(effect) && <div className="overlay" />}
		</StyledPreviewGraduallyGradient>
	);
}
