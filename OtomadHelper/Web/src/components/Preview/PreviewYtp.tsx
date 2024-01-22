import ytpChangePitchImage from "assets/images/effects/ytp_change_pitch.jpg";
import ytpChangeSpeedImage from "assets/images/effects/ytp_change_speed.png";
import ytpChorusImage from "assets/images/effects/ytp_chorus.jpg";
import ytpDelayImage from "assets/images/effects/ytp_delay.png";
import ytpReverseImage from "assets/images/effects/ytp_reverse.png";
import { getDuration } from "./PreviewPrve";

const StyledPreviewYtp = styled.div<{
	/** 效果名称。 */
	$name: string;
}>`
	${styles.mixins.square("100%")};

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
		position: absolute;
	}

	${({ $name }) => {
		return {
			changeHue: css`
				img {
					filter: hue-rotate(180deg);
				}
			`,
			rotateHue: css`
				img {
					filter: hue-rotate(0deg);
					animation: ${keyframes`
						from { filter: hue-rotate(0deg); }
						to { filter: hue-rotate(360deg); }
					`} ${getDuration(4)} linear infinite;
				}
			`,
			monochrome: css`
				img {
					filter: grayscale(1);
				}
			`,
			negative: css`
				img {
					filter: invert(1);
				}
			`,
			repeatRapidly: css`
				img {
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { scale: -1 1; }
						50% { scale: 1; }
					`} 50ms step-start infinite;
				}
			`,
			randomTuning: css`
				img {
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { scale: -1 1; }
						50% { scale: 1; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			upsize: css`
				img {
					animation: ${keyframes`
						from { scale: 1; }
						to { scale: 10; }
					`} ${getDuration(3)} ${eases.easeInExpo} infinite;
				}
			`,
			mirror: css`
				img:nth-child(2) {
					clip-path: inset(0 50% 0 0);
					scale: -1 1;
				}
			`,
			highContrast: css`
				img {
					filter: contrast(5);
				}
			`,
			oversaturation: css`
				img {
					filter: saturate(5);
				}
			`,
			emphasizeThrice: css`
				img {
					animation: ${keyframes`
						0% { scale: 1; }
						14.285714% { scale: 1.5; filter: grayscale(1); }
						42.857143% { scale: 2.25; filter: grayscale(1); }
					`} ${getDuration(3)} step-end infinite;
				}
			`,
		}[$name];
	}}
`;

export default function PreviewYtp({ thumbnail, name }: FCP<{
	/** 缩略图。 */
	thumbnail: string;
	/** 效果名称。 */
	name: string;
}>) {
	const imageCount = {
		mirror: 2,
	}[name] ?? 1;

	const forceUpdate = useForceUpdate();
	const canvasFilters = useCanvasFilter(thumbnail);

	const alterImages: Record<string, string> = {
		chorus: ytpChorusImage,
		changePitch: ytpChangePitchImage,
		reverse: ytpReverseImage,
		delay: ytpDelayImage,
		changeSpeed: ytpChangeSpeedImage,

		vibrato: canvasFilters?.vibrato,
		spherize: canvasFilters?.spherize,
	};
	const alterImage = alterImages[name];

	const repeatRef = useRef<Timer>();
	useEffect(() => {
		repeatRef.current = setInterval(() => {
			console.log(1);
			if (document.querySelector<HTMLImageElement>("[data-name=vibrato]")?.src.startsWith("blob") === false ||
				document.querySelector<HTMLImageElement>("[data-name=spherize]")?.src.startsWith("blob") === false)
				forceUpdate();
			else clearInterval(repeatRef.current);
		}, 100);
	}); // BUG: 刷新bug。

	return (
		<StyledPreviewYtp $name={name}>
			{forMap(imageCount, i =>
				<img key={i} data-name={name} src={alterImage || thumbnail} draggable={false} />)}
		</StyledPreviewYtp>
	);
}
