const StyledSettingsPageControlMedia = styled(Card)<{
	/** 启用？ */
	$enabled: boolean;
}>`
	position: relative;
	overflow: hidden;
	flex-shrink: 0;

	.base {
		display: flex;
		gap: 25px;


		> .right {
			display: flex;
			flex-direction: column;
			width: 100%;

			> p {
				height: 100%;
			}

			> .bottom {
				display: flex;
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
		filter: blur(30px);
		opacity: 30%;
		object-fit: cover;
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
	/** 选择是音频还是视频？ */
	stream: "audio" | "visual";
	/** 文件名。 */
	fileName: string;
	/** 启用？ */
	enabled: StateProperty<boolean>;
	/** 缩略图。 */
	thumbnail: string;
}>) {
	return (
		<StyledSettingsPageControlMedia $enabled={enabled[0] ?? true}>
			<img className="background" src={thumbnail} />
			<SettingsPageControlPreviewImage image={thumbnail}>
				<Icon name={"colored/" + stream} filled />
			</SettingsPageControlPreviewImage>
			<div className="right">
				<p>{fileName}</p>
				<div className="bottom">
					<ToggleSwitch on={enabled} hideLabel>
						<StackPanel>
							<Icon name="enabled" />
							{t.enabled}
						</StackPanel>
					</ToggleSwitch>
					<Button icon="play">{t.audioVisual.preview}</Button>
				</div>
			</div>
		</StyledSettingsPageControlMedia>
	);
}
