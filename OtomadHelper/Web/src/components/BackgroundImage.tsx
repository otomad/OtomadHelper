const StyledBackgroundImage = styled.div`
	position: fixed;
	inset: 0;
	z-index: ${styles.z.backgroundImage};
	pointer-events: none;
	transition: ${fallbackTransitions}, scale ${eases.easeOutMax} 750ms !important;

	&,
	* {
		${styles.mixins.square("100%")};
		inset: 0;
		// transition: none; // Enable for low configuration devices.
	}

	.overlay {
		position: absolute;
		background-color: ${c("colorization")};
		mix-blend-mode: screen;
		forced-color-adjust: none;
	}
`;

export default function BackgroundImage() {
	const { currentImage, fit: [fit], position: [position] } = useBackgroundImages();
	const {
		backgroundImageOpacity: opacity, backgroundImageTint: tint, backgroundImageBlur: blur,
	} = useSnapshot(configStore.settings);

	if (!currentImage) return;

	return (
		<StyledBackgroundImage style={{ opacity }}>
			<BackgroundImageImg
				key={currentImage}
				src={currentImage}
				autoAlt={false}
				fit={fit}
				position={position}
				style={{ filter: `blur(${blur}px)` }}
			/>
			<div className="overlay" style={{ opacity: tint }} />
		</StyledBackgroundImage>
	);
}

export /* @internal */ const ImageFitTypes = Enum({
	cover: { value: "cover", icon: "aspect_ratio" },
	contain: { value: "contain", icon: "letterbox" },
	original: { value: "none", icon: "ratio_one_to_one" },
	stretch: { value: "fill", icon: "fit_fill" },
	scaleDown: { value: "scale-down", icon: "fit_scale_down" },
	tile: { value: "tile", icon: "fit_tile" },
	tileContain: { value: "tile contain", icon: "fit_tile_contain" },
}, { labelPrefix: t.fit });

const StyledBackgroundImageImg = styled.img`
	${styles.mixins.square("100%")};
	object-position: var(--position);
	background-repeat: repeat;
	background-position: var(--position);

	&.tile {
		content-visibility: hidden;

		&.contain {
			background-size: contain;
		}
	}
`;

export function BackgroundImageImg({ src, autoAlt = false, fit = "cover", position: [x, y] = [50, 50], children: _children, style, alt, ...htmlAttrs }: FCP<{
	/** Image source href. */
	src: string;
	/** Allow screen reader to auto fetch image a11y description online? @default false */
	autoAlt?: boolean;
	/** Fit type. */
	fit?: Config.ImageFitType;
	/** Image position (percent of x and y). */
	position?: TwoD;
	children?: never;
}, "img">) {
	const isTile = fit.includes("tile");
	const cssFit = ImageFitTypes[fit] as CSSProperties["objectFit"];
	return (
		<StyledBackgroundImageImg
			alt={alt || (autoAlt ? undefined : "")}
			src={src}
			className={cssFit}
			style={{
				...style as AnyObject,
				backgroundImage: isTile ? `url("${src}")` : undefined,
				objectFit: !isTile ? cssFit : undefined,
				"--position": `${x}% ${y}%`,
			}}
			{...htmlAttrs}
		/>
	);
}
