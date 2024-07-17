export /* @internal */ const PREVIEW_IMAGE_HEIGHT = 120;

const StyledSettingsPageControlPreviewImage = styled.div`
	position: relative;
	display: inline-block;
	flex-shrink: 0;
	max-width: 280px;
	height: ${PREVIEW_IMAGE_HEIGHT}px;
	overflow: clip;
	border-radius: 3px;

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
	}

	.stroke {
		position: absolute;
		inset: 0;
		border-radius: inherit;
		box-shadow: 0 0 0 1px #00000019 inset;
		pointer-events: none;
	}
`;

export default function SettingsPageControlPreviewImage({ image, children }: FCP<{
	/** Image. */
	image: string;
}>) {
	return (
		<StyledSettingsPageControlPreviewImage>
			<Img src={image} />
			<div className="stroke" />
			{children}
		</StyledSettingsPageControlPreviewImage>
	);
}
