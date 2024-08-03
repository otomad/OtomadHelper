import { PREVIEW_IMAGE_HEIGHT } from "./SettingsPageControlPreviewImage";
const IMAGE_MARGIN = 16;

const StyledSettingsPageControl = styled.div<{
	/** Clear float. */
	$clearFloat?: boolean;
	/** The file path of the Easter egg mouse cursor. */
	$cursor?: string;
}>`
	display: flex;
	gap: ${IMAGE_MARGIN}px;
	${({ $cursor }) => $cursor && css`
		cursor: ${$cursor.startsWith("url(") ? $cursor : `url("${$cursor}"), auto`};
	`}

	${ifNotProp("$clearFloat", css`
		display: block;

		&.no-image {
			margin-block-end: 10px;
		}

		.settings-page-control-preview-image {
			float: inline-start;
			margin-block-end: 5px;
			margin-inline-end: ${IMAGE_MARGIN}px;
		}
	`)}

	.settings-page-control-preview-image {
		transition: ${fallbackTransitions}, margin 0s;

		:where(img) {
			width: ${PREVIEW_IMAGE_HEIGHT / 9 * 16}px;
			height: ${PREVIEW_IMAGE_HEIGHT}px;
		}
	}

	> p {
		${styles.effects.text.body};
		margin-block-start: -4px;
	}
`;

export default forwardRef(function SettingsPageControl({ image, imageOverlay, learnMoreLink, clearFloat, cursor, children, ...htmlAttrs }: FCP<{
	/** Image. */
	image?: string;
	/** Something overlay on the image. */
	imageOverlay?: ReactNode;
	/** "Learn More" link href. */
	learnMoreLink?: string;
	/** Clear float? */
	clearFloat?: boolean;
	/** The file path of the Easter egg mouse cursor. */
	cursor?: string;
}, "div">, ref: ForwardedRef<"div">) {
	const { hideUseTips } = useSnapshot(configStore.settings);
	if (hideUseTips) return;

	const LearnMore = learnMoreLink ? OpenLink : "a";
	const paragraphEl = useDomRef<"p">();
	const [isWidow, setIsWidow] = useState(true);

	useLayoutEffect(() => {
		if (!paragraphEl.current || !image || clearFloat || learnMoreLink === undefined) return;
		const observer = new ResizeObserver(lodash.debounce(([{ target }]) => {
			const lineHeight = parseFloat(getComputedStyle(target).lineHeight);
			const imageEl = target.previousElementSibling as HTMLElement | null;
			if (!imageEl || !Number.isFinite(lineHeight)) return;
			const overflowedLines = Math.ceil((target.clientHeight - imageEl.clientHeight) / lineHeight);
			setIsWidow(overflowedLines < 4);
		}));
		observer.observe(paragraphEl.current);
		return () => observer.disconnect();
	}, []);

	return (
		<StyledSettingsPageControl
			ref={ref}
			$clearFloat={clearFloat || isWidow}
			className={{ noImage: !image }}
			$cursor={cursor}
			{...htmlAttrs}
		>
			{image && <SettingsPageControlPreviewImage image={image}>{imageOverlay}</SettingsPageControlPreviewImage>}
			<p ref={paragraphEl}>
				<Preserves>{children}</Preserves>
				{learnMoreLink !== undefined && (
					<>
						<Br repeat={image ? 2 : 0} />
						<LearnMore>{t.learnMore}</LearnMore>
					</>
				)}
			</p>
		</StyledSettingsPageControl>
	);
});
