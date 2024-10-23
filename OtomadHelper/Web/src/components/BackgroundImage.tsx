const StyledBackgroundImage = styled.div`
	${styles.mixins.fullscreen()};
	inset: 0;
	z-index: -1;
	pointer-events: none;
	transition: ${fallbackTransitions}, scale ${eases.easeOutMax} 250ms !important;

	&,
	* {
		${styles.mixins.square("100%")};
		inset: 0;
		// transition: none; // Enable for low configuration devices.
	}

	img {
		object-fit: cover;
	}

	.overlay {
		position: absolute;
		background-color: ${c("colorization")};
		mix-blend-mode: screen;
	}
`;

export default function BackgroundImage() {
	const { currentImage } = useBackgroundImages();
	const { backgroundImageOpacity: [opacity], backgroundImageTint: [tint], backgroundImageBlur: [blur] } = selectConfig(c => c.settings);

	if (!currentImage) return;

	return (
		<StyledBackgroundImage style={{ opacity }}>
			<img key={currentImage} src={currentImage} style={{ filter: `blur(${blur}px)` }} />
			<div className="overlay" style={{ opacity: tint }} />
		</StyledBackgroundImage>
	);
}
