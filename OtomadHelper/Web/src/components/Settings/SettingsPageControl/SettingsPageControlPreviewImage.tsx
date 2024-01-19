const StyledSettingsPageControlPreviewImage = styled.div`
	display: inline-block;
	position: relative;
	max-width: 280px;
	height: 120px;
	border-radius: 3px;
	overflow: hidden;
	flex-shrink: 0;

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
	}

	.stroke {
		position: absolute;
		inset: 0;
		border-radius: inherit;
		box-shadow: 0 0 0 1px #00000019 inset;
	}
`;

export default function SettingsPageControlPreviewImage({ image, children }: FCP<{
	/** 图片。 */
	image: string;
}>) {
	return (
		<StyledSettingsPageControlPreviewImage>
			<img src={image} alt="preview" />
			<div className="stroke" />
			{children}
		</StyledSettingsPageControlPreviewImage>
	);
}
