const StyledSettingsPageControl = styled.div`
	display: flex;
	gap: 16px;
	margin-bottom: 16px;
	padding: 0 1px;
`;

export default function SettingsPageControl({ image, learnMoreLink, children }: FCP<{
	/** 图片。 */
	image: string;
	/** “了解更多”链接地址。 */
	learnMoreLink?: string;
}>) {
	return (
		<StyledSettingsPageControl>
			<SettingsPageControlPreviewImage image={image} />
			<p>
				<Preserves>{children}</Preserves>
				{learnMoreLink !== undefined && (
					<>
						<br /><br />
						{learnMoreLink ?
							<a href={learnMoreLink} target="_blank" rel="noreferrer">{t.learnMore}</a> :
							<a href="javascript:void(0);">{t.learnMore}</a>
						}
					</>
				)}
			</p>
		</StyledSettingsPageControl>
	);
}
