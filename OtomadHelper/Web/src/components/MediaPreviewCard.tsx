import exampleThumbnail from "assets/images/ヨハネの氷.png";

const StyledMediaPreviewCard = styled(Card)<{
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

	.preview-img-wrapper {
		display: inline-block;
		position: relative;
		max-width: 280px;
		height: 120px;
		border-radius: 3px;
		overflow: hidden;
		flex-shrink: 0;

		img {
			${styles.mixins.square("100%")};
			object-fit: cover;
		}

		.stroke {
			position: absolute;
			inset: 0;
			border-radius: inherit;
			box-shadow: 0 0 0 1px #00000019 inset;
		}

		.icon {
			position: absolute;
			bottom: 10px;
			left: 10px;
			font-size: 36px;
		}
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

	${({ $enabled }) => !$enabled && css`
		.preview-img-wrapper img {
			filter: grayscale(1);
		}

		img.background {
			filter: blur(30px) grayscale(1);
		}
	`};
`;

export default function MediaPreviewCard({ stream, fileName, enabled }: FCP<{
	/** 选择是音频还是视频？ */
	stream: "audio" | "visual";
	/** 文件名。 */
	fileName: string;
	/** 启用？ */
	enabled: StateProperty<boolean>;
}>) {
	const icon = stream === "audio" ? "colored/media_player" : "colored/photo";

	return (
		<StyledMediaPreviewCard $enabled={enabled[0] ?? true}>
			<img className="background" src={exampleThumbnail} />
			<div className="preview-img-wrapper">
				<img src={exampleThumbnail} alt="thumbnail" />
				<div className="stroke" />
				<Icon name={icon} filled />
			</div>
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
		</StyledMediaPreviewCard>
	);
}
