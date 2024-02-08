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
	const [hideFeatureTips] = selectConfig(c => c.settings.hideFeatureTips);
	if (hideFeatureTips) return;

	const LearnMore = learnMoreLink ? OpenLink : "a";

	return (
		<StyledSettingsPageControl>
			<SettingsPageControlPreviewImage image={image} />
			<p>
				<Preserves>{children}</Preserves>
				{learnMoreLink !== undefined && (
					<>
						<br /><br />
						<LearnMore>{t.learnMore}</LearnMore>
					</>
				)}
			</p>
		</StyledSettingsPageControl>
	);
}
