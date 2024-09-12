import ytpChangePitchImage from "assets/images/effects/ytp_change_pitch.jpg";
import ytpChangeSpeedImage from "assets/images/effects/ytp_change_speed.png";
import ytpChorusImage from "assets/images/effects/ytp_chorus.jpg";
import ytpDelayImage from "assets/images/effects/ytp_delay.png";
import ytpReverseImage from "assets/images/effects/ytp_reverse.png";
import { MILLISECONDS_PER_FRAME } from "./PreviewPrve";

const getDuration = (frames: number) => frames * MILLISECONDS_PER_FRAME + "ms";

const StyledPreviewYtp = styled.div<{
	/** Effect name. */
	$name: string;
}>`
	${styles.mixins.square("100%")};

	img {
		${styles.mixins.square("100%")};
		position: absolute;
		object-fit: cover;
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
					scale: -1 1;
					clip-path: inset(0 50% 0 0);
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
	/** Thumbnail. */
	thumbnail: string;
	/** Effect name. */
	name: string;
}>) {
	const imageCount = {
		mirror: 2,
	}[name] ?? 1;

	// const canvasFilters = useCanvasFilters(thumbnail);
	const webglFilters = useWebglFilters(thumbnail);

	const alterImage = {
		chorus: ytpChorusImage,
		changePitch: ytpChangePitchImage,
		reverse: ytpReverseImage,
		delay: ytpDelayImage,
		changeSpeed: ytpChangeSpeedImage,

		vibrato: webglFilters?.wave,
		spherize: webglFilters?.spherize,
		twist: webglFilters?.twist,
		mosaic: webglFilters?.mosaic,
		thermal: webglFilters?.thermal,
		emboss: webglFilters?.emboss,
		bump: webglFilters?.bump,
		edge: webglFilters?.edge,
	}[name];

	return (
		<StyledPreviewYtp $name={name}>
			{forMap(imageCount, i =>
				<img key={i} data-name={name} src={alterImage || thumbnail} />)}
		</StyledPreviewYtp>
	);
}
