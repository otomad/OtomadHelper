const StyledPreviewPrve = styled.div<{
	/** 效果名称。 */
	$name: string;
}>`
	${styles.mixins.square("100%")};

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
	}

	${({ $name }) => ({
		
	}[$name])}
`;

export default function PreviewPrve({ thumbnail, name }: FCP<{
	/** 缩略图。 */
	thumbnail: string;
	/** 效果名称。 */
	name: string;
}>) {
	return (
		<StyledPreviewPrve $name={name}>
			<img src={thumbnail} />
		</StyledPreviewPrve>
	);
}
