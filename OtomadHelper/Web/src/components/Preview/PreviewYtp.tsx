import ytpChangePitchImage from "assets/images/effects/ytp_change_pitch.avif";
import ytpChangeSpeedImage from "assets/images/effects/ytp_change_speed.avif";
import ytpChorusImage from "assets/images/effects/ytp_chorus.avif";
import ytpDelayImage from "assets/images/effects/ytp_delay.avif";
import ytpReverseImage from "assets/images/effects/ytp_reverse.avif";
import { MILLISECONDS_PER_FRAME, styledMirror } from "./PreviewPrve";

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
					filter: hue-rotate(0.5turn);
				}
			`,
			rotateHue: css`
				img {
					filter: hue-rotate(0turn);
					animation: ${keyframes`
						from { filter: hue-rotate(0turn); }
						to { filter: hue-rotate(1turn); }
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
				img {
					--mirror-on: 0;
					${styledMirror.h};
					animation: ${keyframes`
						0%, 100% { --mirror-on: 0; }
						50% { --mirror-on: 1; }
					`};
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
						42.857143% { scale: 2; filter: url("#red-channel"); }
					`} ${getDuration(3)} step-end infinite;
				}
			`,
			spectrum: css`
				img {
					filter: url("#spectrum");
				}
			`,
			thermal: css`
				img {
					filter: url("#thermal");
				}
			`,
			emboss: css`
				img {
					filter: url("#emboss");
				}
			`,
			bump: css`
				img {
					filter: url("#bump");
				}
			`,
			edge: css`
				img {
					filter: url("#edge");
				}
			`,
		}[$name];
	}}
`;

function SvgFilters() {
	return (
		<>
			{/* <DefineSvgFilter id="pixelate">
				<feGaussianBlur stdDeviation="4" in="SourceGraphic" result="smoothed" />
				<feImage width="8" height="8" xlinkHref="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAIAAAACDbGyAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAWSURBVAgdY1ywgOEDAwKxgJhIgFQ+AP/vCNK2s+8LAAAAAElFTkSuQmCC" result="displacement-map" />
				<feTile in="displacement-map" result="pixelate-map" />
				<feDisplacementMap
					in="smoothed"
					in2="pixelate-map"
					xChannelSelector="R"
					yChannelSelector="G"
					scale="50"
					result="pre-final"
				/>
				<feComposite operator="in" in2="SourceGraphic" />
			</DefineSvgFilter> */}
			<DefineSvgFilter id="spectrum" colorInterpolationFilters="sRGB">
				<feColorMatrix
					type="matrix"
					values="
						0.33 0.33 0.33 0 0
						0.33 0.33 0.33 0 0
						0.33 0.33 0.33 0 0
						0 0 0 1 0
					"
					in="SourceGraphic"
					result="colorMatrix"
				/>
				<feComponentTransfer in="colorMatrix" result="componentTransfer">
					<feFuncR type="table" tableValues="1 1 0 0 0 1 1" />
					<feFuncG type="table" tableValues="0 1 1 1 0 0 0" />
					<feFuncB type="table" tableValues="0 0 0 1 1 1 0" />
					<feFuncA type="table" tableValues="0 1" />
				</feComponentTransfer>
				<feBlend mode="normal" in="componentTransfer" in2="SourceGraphic" result="blend" />
			</DefineSvgFilter>
			<DefineSvgFilter id="thermal" colorInterpolationFilters="sRGB">
				<feComponentTransfer>
					<feFuncR type="table" tableValues="0 0.125 0.8   1     1" />
					<feFuncG type="table" tableValues="0 0     0     0.843 1" />
					<feFuncB type="table" tableValues="0 0.549 0.466 0     1" />
				</feComponentTransfer>
			</DefineSvgFilter>
			<DefineSvgFilter id="red-channel">
				<feColorMatrix
					type="matrix"
					values="
						1 0 0 0 0
						1 0 0 0 0
						1 0 0 0 0
						0 0 0 1 0
					"
				/>
			</DefineSvgFilter>
			<DefineSvgFilter id="emboss">
				<feConvolveMatrix
					order="3 3"
					preserveAlpha="true"
					kernelMatrix="
						1 0 0
						0 0 0
						0 0 -1
					"
					divisor="1"
					bias="0.2"
				/>
			</DefineSvgFilter>
			<DefineSvgFilter id="bump">
				<feConvolveMatrix
					order="5 5"
					preserveAlpha="true"
					kernelMatrix="
						0.5 0 0 0 0
						0 1 0 0 0
						0 0 -1.5 0 0
						0.3 0 0 0 0
						0 0 0 0 0
					"
				/>
			</DefineSvgFilter>
			<DefineSvgFilter id="edge">
				<feConvolveMatrix
					order="3 3"
					preserveAlpha="true"
					kernelMatrix="
						1 1 1
						1 -8 1
						1 1 1
					"
				/>
			</DefineSvgFilter>
		</>
	);
}

export default function PreviewYtp({ thumbnail, name }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
	/** Effect name. */
	name: string;
}>) {
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
		pixelate: webglFilters?.pixelate,
		// spectrum: webglFilters?.spectrum,
		// emboss: webglFilters?.emboss,
		// bump: webglFilters?.bump,
		// edge: webglFilters?.edge,
	}[name];

	return (
		<StyledPreviewYtp $name={name}>
			<img alt="" data-name={name} src={alterImage || thumbnail} />
			<SvgFilters />
		</StyledPreviewYtp>
	);
}
