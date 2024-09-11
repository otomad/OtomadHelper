import prvePingpongImage from "assets/images/effects/prve_pingpong.gif";
import prveWhirlImage from "assets/images/effects/prve_whirl.gif";
import { freezeframes } from "helpers/freezeframe";
import { getStepChangeHueStep } from "views/visual/prve";
const prvePingpongStaticImage = freezeframes["effects/prve_pingpong.gif"];
const prveWhirlStaticImage = freezeframes["effects/prve_whirl.gif"];

const MILLISECONDS_PER_FRAME = 375;
export /* @internal */ const getDuration = (frames: number) => frames * 375 + "ms";

const StyledPreviewPrve = styled.div<{
	/** Effect identifier. */
	$id: string;
}>`
	${styles.mixins.square("100%")};
	container: preview-prve / size;

	img {
		position: absolute;
		object-fit: cover;
		animation-duration: calc(var(--frames) * ${MILLISECONDS_PER_FRAME}ms);
		animation-timing-function: step-start;
		animation-iteration-count: infinite;

		@layer base {
			--frames: 1;
			${styles.mixins.square("100%")};
		}
	}

	.animated-image {
		${useLottieStatus.animation("Hover")};
	}

	.items-view-item:not(:hover, :focus-visible) & img {
		animation: none;
	}

	.initial-value & img {
		animation-play-state: paused;
		animation-delay: calc(var(--i, 0) * -${MILLISECONDS_PER_FRAME}ms);
	}

	@layer components {
		${({ $id }) => {
			const stepChangeHueStep = getStepChangeHueStep($id);
			if (stepChangeHueStep !== null)
				return css`
					img {
						--frames: ${stepChangeHueStep};
						filter: hue-rotate(${360 / stepChangeHueStep}deg);
						animation: ${keyframes`
							${forMapFromTo(1, stepChangeHueStep, i => {
								const offset = 100 / stepChangeHueStep * (i - 1);
								return `${offset === 0 ? "0%, 100%" : offset + "%"} { filter: hue-rotate(${360 / stepChangeHueStep * i}deg); }`;
							})}
						`};
					}
				`;
			return {
				hFlip: css`
					img {
						--frames: 2;
						scale: -1 1;
						animation: ${keyframes`
							0%, 100% { scale: -1 1; }
							50% { scale: 1; }
						`};
					}
				`,
				vFlip: css`
					img {
						--frames: 2;
						scale: 1 -1;
						animation: ${keyframes`
							0%, 100% { scale: 1 -1; }
							50% { scale: 1; }
						`};
					}
				`,
				ccwFlip: css`
					img {
						--frames: 4;
						scale: 1 -1;
						animation: ${keyframes`
							0%, 100% { scale: 1 -1; }
							25% { scale: -1 -1; }
							50% { scale: -1 1; }
							75% { scale: 1; }
						`};
					}
				`,
				cwFlip: css`
					img {
						--frames: 4;
						scale: -1 1;
						animation: ${keyframes`
							0%, 100% { scale: -1 1; }
							25% { scale: -1 -1; }
							50% { scale: 1 -1; }
							75% { scale: 1; }
						`};
					}
				`,
				ccwRotate: css`
					img {
						--frames: 4;
						width: 100cqh;
						height: 100cqw;
						transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(-90deg);
						animation: ${keyframes`
							0%, 100% { transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(-90deg); width: 100cqh; height: 100cqw; }
							25% { transform: rotate(-180deg); width: 100%; height: 100%; }
							50% { transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(-270deg); width: 100cqh; height: 100cqw; }
							75% { transform: rotate(0deg); width: 100%; height: 100%; }
						`};
					}
				`,
				cwRotate: css`
					img {
						--frames: 4;
						width: 100cqh;
						height: 100cqw;
						transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(90deg);
						animation: ${keyframes`
							0%, 100% { transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(90deg); width: 100cqh; height: 100cqw; }
							25% { transform: rotate(180deg); width: 100%; height: 100%; }
							50% { transform: translate(calc((100cqw - 100cqh) / 2), calc((100cqw - 100cqh) / -2)) rotate(270deg); width: 100cqh; height: 100cqw; }
							75% { transform: rotate(0deg); width: 100%; height: 100%; }
						`};
					}
				`,
				turned: css`
					img {
						--frames: 2;
						rotate: 180deg;
						animation: ${keyframes`
							0%, 100% { rotate: 180deg; }
							50% { rotate: 0deg; }
						`};
					}
				`,
				zoomOutIn: css`
					img {
						scale: 10;
						animation: ${keyframes`
							from { scale: 10; }
							to { scale: 1; }
						`} alternate;
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				hMirror: css`
					img {
						--frames: 2;
					}
					img:nth-child(2) {
						scale: -1 1;
						clip-path: inset(0 50% 0 0);
						animation: ${keyframes`
							0%, 100% { clip-path: inset(0 50% 0 0); scale: -1 1; }
							50% { clip-path: inset(0 0 0 50%); }
						`};
					}
				`,
				vMirror: css`
					img {
						--frames: 2;
					}
					img:nth-child(2) {
						scale: 1 -1;
						clip-path: inset(0 0 50% 0);
						animation: ${keyframes`
							0%, 100% { clip-path: inset(0 0 50% 0); scale: 1 -1; }
							50% { clip-path: inset(50% 0 0 0); }
						`};
					}
				`,
				ccwMirror: css`
					img {
						--frames: 4;
					}
					img:nth-child(2) {
						scale: -1 1;
						clip-path: inset(0 50% 0 0);
						animation: ${keyframes`
							0%, 75%, 100% { clip-path: inset(0 50% 0 0); }
							25%, 50% { clip-path: inset(0 0 0 50%); }
						`};
					}
					img:nth-child(3) {
						scale: 1 -1;
						clip-path: inset(50% 0 0 0);
						animation: ${keyframes`
							0%, 25%, 100% { clip-path: inset(50% 0 0 0); }
							50%, 75% { clip-path: inset(0 0 50% 0); }
						`};
					}
					img:nth-child(4) {
						scale: -1;
						clip-path: inset(50% 50% 0 0);
						animation: ${keyframes`
							0%, 100% { clip-path: inset(50% 50% 0 0); }
							25% { clip-path: inset(50% 0 0 50%); }
							50% { clip-path: inset(0 0 50% 50%); }
							75% { clip-path: inset(0 50% 50% 0); }
						`};
					}
				`,
				cwMirror: css`
					img {
						--frames: 4;
					}
					img:nth-child(2) {
						scale: -1 1;
						clip-path: inset(0 0 0 50%);
						animation: ${keyframes`
							0%, 25%, 100% { clip-path: inset(0 0 0 50%); }
							50%, 75% { clip-path: inset(0 50% 0 0); }
						`};
					}
					img:nth-child(3) {
						scale: 1 -1;
						clip-path: inset(0 0 50% 0);
						animation: ${keyframes`
							0%, 75%, 100% { clip-path: inset(0 0 50% 0); }
							25%, 50% { clip-path: inset(50% 0 0 0); }
						`};
					}
					img:nth-child(4) {
						scale: -1;
						clip-path: inset(0 0 50% 50%);
						animation: ${keyframes`
							0%, 100% { clip-path: inset(0 0 50% 50%); }
							25% { clip-path: inset(50% 0 0 50%); }
							50% { clip-path: inset(50% 50% 0 0); }
							75% { clip-path: inset(0 50% 50% 0); }
						`};
					}
				`,
				negative: css`
					img {
						--frames: 2;
						filter: invert(1);
						animation: ${keyframes`
							0%, 100% { filter: invert(1); }
							50% { filter: none; }
						`};
					}
				`,
				luminInvert: css`
					img {
						--frames: 2;
						filter: invert(1) hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: invert(1) hue-rotate(180deg); }
							50% { filter: none; }
						`};
					}
				`,
				negativeBlur: css`
					img:nth-child(1) {
						opacity: 0;
						animation: ${keyframes`
							0% { opacity: 0; animation-timing-function: ${eases.easeInMax}; }
							50% { opacity: 1; animation-timing-function: ${eases.easeOutMax}; }
							100% { opacity: 1; }
						`} alternate;
					}
					img:nth-child(2) {
						filter: invert(1);
						mix-blend-mode: difference;
						animation: ${keyframes`
							0% { opacity: 1; animation-timing-function: ${eases.easeInMax}; }
							50% { opacity: 1; animation-timing-function: ${eases.easeOutMax}; }
							100% { opacity: 0; }
						`} alternate;
					}
				`,
				hueInvert: css`
					img {
						--frames: 2;
						filter: hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: hue-rotate(180deg); }
							50% { filter: none; }
						`};
					}
				`,
				chromatic: css`
					img {
						--frames: 2;
						filter: grayscale(1);
						animation: ${keyframes`
							0%, 100% { filter: grayscale(1); }
							50% { filter: none; }
						`};
					}
				`,
				chromaticBlur: css`
					img {
						--frames: 2;
						filter: grayscale(1);
						animation: ${keyframes`
							0%, 100% { filter: grayscale(1); }
							50% { filter: none; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				vExpansion: css`
					img {
						transform-origin: center bottom;
						scale: 1 0.75;
						animation: ${keyframes`
							from { scale: 1 0.75; }
							to { scale: 1; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				vExpansionBounce: css`
					img {
						transform-origin: center bottom;
						scale: 1 0.75;
						animation: ${keyframes`
							from { scale: 1 0.75; }
							to { scale: 1; }
						`} alternate;
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				vCompression: css`
					img {
						transform-origin: center bottom;
						animation: ${keyframes`
							from { scale: 1; }
							to { scale: 1 0.75; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				vCompressionBounce: css`
					img {
						transform-origin: center bottom;
						animation: ${keyframes`
							from { scale: 1; }
							to { scale: 1 0.75; }
						`} alternate;
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				vBounce: css`
					img {
						scale: 1 0.5;
						animation: ${keyframes`
							from { scale: 1 0.5; }
							to { scale: 1; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				slantDown: css`
					img {
						--frames: 2;
						transform: skewX(25deg) scaleX(0.5);
						transform-origin: center bottom;
						animation: ${keyframes`
							0% { transform: skewX(25deg) scaleX(0.5); }
							50% { transform: scale(0.5); animation-timing-function: ${eases.easeOutMax}; }
							100% { transform: skewX(-25deg) scaleX(0.5); }
						`} alternate;
						animation-timing-function: ${eases.easeInMax} !important;
					}
				`,
				slantUp: css`
					img {
						--frames: 2;
						transform: skewX(25deg) scale(0.5);
						transform-origin: center bottom;
						animation: ${keyframes`
							0%, { transform: skewX(25deg) scale(0.5); }
							50% { transform: scaleX(0.5); animation-timing-function: ${eases.easeInMax}; }
							100% { transform: skewX(-25deg) scale(0.5); }
						`} alternate;
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				puyo: css`
					img {
						--frames: 2;
						scale: 1 0.75;
						animation: ${keyframes`
							0%, 100% { scale: 1 0.75; }
							50% { scale: 0.75 1; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				pendulum: css`
					img {
						--frames: 2;
						scale: 0.5;
						rotate: -25deg;
						animation: ${keyframes`
							0%, 100% { rotate: -25deg; }
							50% { rotate: 25deg; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				gaussianBlur: css`
					img {
						scale: 1.05;
						filter: blur(5px);
						animation: ${keyframes`
							from { filter: blur(5px); scale: 1.05; }
							to { filter: blur(0); }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				radialBlur: css`
					img:nth-child(2) {
						animation: ${keyframes`
							from { opacity: 1; }
							to { opacity: 0; }
						`};
						animation-timing-function: linear !important;
					}
				`,
				wipeRight: css`
					img {
						--frames: 2;
						clip-path: inset(0 75% 0 0);
						animation: ${keyframes`
							0% { clip-path: inset(0 100% 0 0); }
							50% { clip-path: inset(0 0 0 0); }
							100% { clip-path: inset(0 0 0 100%); }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				splitVOut: css`
					img {
						clip-path: inset(25% 0 25% 0);
						animation: ${keyframes`
							from { clip-path: inset(25% 0 25% 0); }
							to { clip-path: inset(0 0 0 0); }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
			}[$id];
		}}
	}
`;

export default function PreviewPrve({ thumbnail, id, onFramesChange, ...htmlAttrs }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
	/** Effect identifier. */
	id: string;
	/** Tell parent (PRVE page) what is the frame count of the current effect. */
	onFramesChange?(frames: number): void;
}, "div">) {
	const imageCount = {
		hMirror: 2,
		vMirror: 2,
		ccwMirror: 4,
		cwMirror: 4,
		radialBlur: 2,
		negativeBlur: 2,
	}[id] ?? 1;

	// const canvasFilters = useCanvasFilters(thumbnail);
	const webglFilters = useWebglFilters(thumbnail);

	const alterImage = {
		radialBlur: webglFilters?.radialBlur,
	}[id];

	const animatedImage = {
		pingpong: Tuple(prvePingpongImage, prvePingpongStaticImage),
		whirl: Tuple(prveWhirlImage, prveWhirlStaticImage),
	}[id];

	const el = useDomRef<"div">();
	useEffect(() => {
		const img = el.current?.querySelector("img");
		let frames = !img ? NaN : +getComputedStyle(img).getPropertyValue("--frames");
		if (!Number.isFinite(frames)) frames = 1;
		onFramesChange?.(frames);
	}, [id]);

	return (
		<StyledPreviewPrve ref={el} $id={id} {...htmlAttrs}>
			{forMap(imageCount, i => animatedImage ?
				<HoverToChangeImg key={i} animatedSrc={animatedImage[0]} staticSrc={animatedImage[1]} /> :
				<img key={i} src={id === "radialBlur" && i ? alterImage! : thumbnail} />)}
		</StyledPreviewPrve>
	);
}

function HoverToChangeImg({ staticSrc, animatedSrc }: FCP<{
	/** Static image address. */
	staticSrc: string;
	/** Animated picture address. */
	animatedSrc: string;
}>) {
	const [isHovered, setIsHovered] = useState(false);

	return (
		<EventInjector onAnimationStart={() => setIsHovered(true)} onAnimationCancel={() => setIsHovered(false)}>
			<img src={isHovered ? animatedSrc : staticSrc} className="animated-image" />
		</EventInjector>
	);
}
