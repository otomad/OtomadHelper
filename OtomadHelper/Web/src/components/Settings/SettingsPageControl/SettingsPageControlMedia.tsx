const StyledSettingsPageControlMedia = styled(Card)<{
	/** Enabled? */
	$enabled: boolean;
}>`
	position: relative;
	flex-shrink: 0;
	overflow: clip;

	.base {
		display: flex;
		gap: 25px;


		> .right {
			z-index: 0; // If this is not set, it will be auto by default, and the child elements will have a strange phenomenon of abnormal rendering levels (may only exist in Chromium).
			display: flex;
			flex-direction: column;
			width: 100%;

			> p {
				height: 100%;
			}

			> .bottom {
				display: flex;
				flex-wrap: wrap;
				gap: 12px;
				justify-content: space-between;
			}
		}
	}

	.settings-page-control-preview-image .icon {
		position: absolute;
		bottom: 8px;
		left: 10px;
		font-size: 36px;
	}

	img.background {
		${styles.mixins.square("100%")};
		position: absolute;
		inset: 0;
		object-fit: cover;
		opacity: 0.3;
		filter: blur(30px);
		pointer-events: none;
	}

	.toggle-switch-label {
		width: auto;
	}

	${({ $enabled }) => !$enabled && css`
		.settings-page-control-preview-image img {
			filter: grayscale(1);
		}

		img.background {
			filter: blur(30px) grayscale(1);
		}
	`};
`;

export default function SettingsPageControlMedia({ stream, fileName, enabled, thumbnail }: FCP<{
	/** Audio or visual? */
	stream: "audio" | "visual";
	/** File name. */
	fileName: string;
	/** Enabled? */
	enabled: StateProperty<boolean>;
	/** Thumbnail. */
	thumbnail: string;
}>) {
	const icon = stream === "audio" ? "volume" : "image";
	return (
		<StyledSettingsPageControlMedia $enabled={enabled[0] ?? true}>
			<Img className="background" src={thumbnail} duplicate="background" />
			<SettingsPageControlPreviewImage image={thumbnail}>
				<Icon name={`colored/${icon}`} filled />
			</SettingsPageControlPreviewImage>
			<div className="right">
				<p>{fileName}</p>
				<div className="bottom">
					<ToggleSwitch on={enabled} hideLabel resetTransitionOnChanging>
						<StackPanel>
							<Icon name="enabled" />
							{t.enabled}
						</StackPanel>
					</ToggleSwitch>
					<Button icon="play">{t.stream.preview}</Button>
				</div>
			</div>
		</StyledSettingsPageControlMedia>
	);
}
