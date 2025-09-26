import { PREVIEW_IMAGE_HEIGHT } from "./SettingsPageControlPreviewImage";
const IMAGE_MARGIN = 16;
type CursorType = typeof import("*.svg?cursor").default | string;

const StyledSettingsPageControl = styled.div<{
	/** The file path of the Easter egg mouse cursor. */
	$cursor?: CursorType;
}>`
	display: flex;
	gap: 0.5em ${IMAGE_MARGIN}px;
	${({ $cursor }) => $cursor && css`
		cursor: ${typeof $cursor === "string" ? `url("${$cursor}"), auto` : $cursor};
	`}

	.settings-page-control-preview-image {
		transition: ${fallbackTransitions}, margin 0s;

		:where(img) {
			width: ${PREVIEW_IMAGE_HEIGHT / 9 * 16}px;
			height: ${PREVIEW_IMAGE_HEIGHT}px;
		}
	}

	> .paragraph-wrapper {
		${styles.effects.text.caption};

		> .scroll-wrapper {
			${styles.mixins.overflowGradient("y", "1.25lh")};
			max-block-size: ${PREVIEW_IMAGE_HEIGHT + 1}px;

			> p {
				margin-block-start: -3px;
			}

			* {
				user-select: text;
			}
		}


		> .learn-more-wrapper {
			display: block;
			margin-block-start: 0.375lh;
		}
	}

`;

export default function SettingsPageControl({ image, imageOverlay, learnMoreLink, cursor, children, ref, ...htmlAttrs }: FCP<{
	/** Image. */
	image?: string | ReactNode;
	/** Something overlay on the image. */
	imageOverlay?: ReactNode;
	/** "Learn More" link href. */
	learnMoreLink?: string;
	/** The file path of the Easter egg mouse cursor. */
	cursor?: CursorType;
}, "div">) {
	const { hideUseTips } = useSnapshot(configStore.settings);
	if (hideUseTips) return;

	const LearnMore = learnMoreLink ? Link : "a";

	return (
		<StyledSettingsPageControl
			ref={ref}
			$cursor={cursor}
			{...htmlAttrs}
		>
			{image && <SettingsPageControlPreviewImage image={image}>{imageOverlay}</SettingsPageControlPreviewImage>}
			<div className="paragraph-wrapper">
				<div className={["scroll-wrapper", ifColorScheme.forceMotion]}>
					<p><Preserves spacing>{children}</Preserves></p>
				</div>
				{learnMoreLink !== undefined && <div className="learn-more-wrapper"><LearnMore>{t.learnMore}</LearnMore></div>}
			</div>
		</StyledSettingsPageControl>
	);
}
