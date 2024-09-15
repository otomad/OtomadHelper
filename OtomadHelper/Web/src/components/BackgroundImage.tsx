const StyledBackgroundImage = styled.div`
	${styles.mixins.fullscreen()};
	inset: 0;
	pointer-events: none;

	&,
	* {
		transition: none;
	}

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
	}
`;

export default function BackgroundImage() {
	const { currentImage } = useBackgroundImages();

	if (!currentImage) return;

	return (
		<StyledBackgroundImage>
			<img key={currentImage} src={currentImage} />
		</StyledBackgroundImage>
	);
}
