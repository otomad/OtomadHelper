const StyledBackgroundImage = styled.div`
	${styles.mixins.fullscreen()};
	inset: 0;
	z-index: -1;
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
	const { currentImage } = useBackgroundImages();
	const {
		backgroundImageOpacity: opacity, backgroundImageTint: tint, backgroundImageBlur: blur,
		backgroundImageFit: fit, backgroundImagePosition: position,
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

export /* @internal */ const FitType = Enum({
	cover: { value: "cover", icon: "aspect_ratio" },
	contain: { value: "contain", icon: "letterbox" },
	original: { value: "none" },
	stretch: { value: "fill" },
	scaleDown: { value: "scale-down" },
	tile: { value: "tile" },
	tileContain: { value: "tile contain" },
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

export function BackgroundImageImg({ src, autoAlt = false, fit = "cover", position = "center", children: _children, style, ...htmlAttrs }: FCP<{
	/** Image source href. */
	src: string;
	/** Allow screen reader to auto fetch image a11y description online? @default false */
	autoAlt?: boolean;
	/** Fit type. */
	fit?: Config.ImageFitType;
	/** Image position. */
	position?: Position;
	children?: never;
}, "img">) {
	const isTile = fit.includes("tile");
	const cssFit = FitType[fit] as CSSProperties["objectFit"];
	return (
		<StyledBackgroundImageImg
			alt={autoAlt ? undefined : ""}
			src={src}
			className={cssFit}
			style={{
				...style as AnyObject,
				backgroundImage: isTile ? `url("${src}")` : undefined,
				objectFit: !isTile ? cssFit : undefined,
				"--position": position,
			}}
			{...htmlAttrs}
		/>
	);
}
