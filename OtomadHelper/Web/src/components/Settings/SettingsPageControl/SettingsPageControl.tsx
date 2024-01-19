const StyledSettingsPageControl = styled.div`
	display: flex;
	gap: 16px;
	margin-bottom: 16px;
`;

export default function SettingsPageControl({ image, children }: FCP<{
	/** 图片。 */
	image: string;
}>) {
	return (
		<StyledSettingsPageControl>
			<SettingsPageControlPreviewImage image={image} />
			{children}
		</StyledSettingsPageControl>
	);
}
