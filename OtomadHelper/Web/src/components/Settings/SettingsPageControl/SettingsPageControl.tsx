const IMAGE_MARGIN = 16;

const StyledSettingsPageControl = styled.div<{
	/** 清除浮动。 */
	$clearFloat?: boolean;
}>`
	&:is(:lang(zh), :lang(ja), :lang(ko)) {
		text-align: justify;
	}

	${({ $clearFloat }) => $clearFloat ? css`
		display: flex;
		gap: ${IMAGE_MARGIN}px;
	` : css`
		margin-bottom: ${IMAGE_MARGIN}px;
		padding: 0 1px;

		&.no-image {
			margin-bottom: 10px;
		}

		.settings-page-control-preview-image {
			float: left;
			margin-right: ${IMAGE_MARGIN}px;
			margin-bottom: 5px;
		}
	`}
`;

export default forwardRef(function SettingsPageControl({ image, learnMoreLink, clearFloat, children, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string;
	/** “了解更多”链接地址。 */
	learnMoreLink?: string;
	/** 清除浮动。 */
	clearFloat?: boolean;
}, "div">, ref: ForwardedRef<"div">) {
	const [hideUseTips] = selectConfig(c => c.settings.hideUseTips);
	if (hideUseTips) return;

	const LearnMore = learnMoreLink ? OpenLink : "a";
	const TextWrapper = clearFloat ? "p" : Fragment;

	return (
		<StyledSettingsPageControl ref={ref} $clearFloat={clearFloat} className={{ noImage: !image }} {...htmlAttrs}>
			{image && <SettingsPageControlPreviewImage image={image} />}
			<TextWrapper>
				<Preserves>{children}</Preserves>
				{learnMoreLink !== undefined && (
					<>
						{forMap(image ? 2 : 0, () => <br />)}
						<LearnMore>{t.learnMore}</LearnMore>
					</>
				)}
			</TextWrapper>
		</StyledSettingsPageControl>
	);
});
