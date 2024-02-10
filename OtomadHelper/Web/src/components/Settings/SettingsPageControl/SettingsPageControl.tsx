const StyledSettingsPageControl = styled.div`
	margin-bottom: 16px;
	padding: 0 1px;

	&:is(:lang(zh), :lang(ja), :lang(ko)) {
		text-align: justify;
	}

	.settings-page-control-preview-image {
		float: left;
		margin-right: 16px;
		margin-bottom: 5px;
	}
`;

export default forwardRef(function SettingsPageControl({ image, learnMoreLink, children, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image: string;
	/** “了解更多”链接地址。 */
	learnMoreLink?: string;
}, "div">, ref: ForwardedRef<HTMLDivElement>) {
	const [hideFeatureTips] = selectConfig(c => c.settings.hideFeatureTips);
	if (hideFeatureTips) return;

	const LearnMore = learnMoreLink ? OpenLink : "a";

	return (
		<StyledSettingsPageControl ref={ref} {...htmlAttrs}>
			<SettingsPageControlPreviewImage image={image} />
			<Preserves>{children}</Preserves>
			{learnMoreLink !== undefined && (
				<>
					<br /><br />
					<LearnMore>{t.learnMore}</LearnMore>
				</>
			)}
		</StyledSettingsPageControl>
	);
});
