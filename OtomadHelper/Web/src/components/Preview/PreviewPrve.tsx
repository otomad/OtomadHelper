import prvePingpongImage from "assets/images/effects/prve_pingpong.gif";
import prveWhirlImage from "assets/images/effects/prve_whirl.webp";
import { freezeframes } from "helpers/freezeframe";
import { type WebGLFilter, initWebgl2 } from "hooks/webgl/render";
import { getStepChangeHueStep } from "views/visual/prve";
const prvePingpongStaticImage = freezeframes["effects/prve_pingpong.gif"];
const prveWhirlStaticImage = freezeframes["effects/prve_whirl.webp"];

export /* @internal */ const MILLISECONDS_PER_FRAME = 375;

const StyledPreviewPrve = styled.div<{
	/** Effect identifier. */
	$effect: string;
	/** Frame count. */
	$frames?: number;
	/** Static normal visual? */
	$static?: boolean;
}>`
	${styles.mixins.square("100%")};
	${styles.mixins.gridCenter()};
	container: preview-prve / size;

	img {
		position: absolute;
		object-fit: cover;
		animation-duration: calc(var(--frames) * ${MILLISECONDS_PER_FRAME}ms);
		animation-timing-function: step-start;
		animation-iteration-count: infinite;
	}

	canvas {
		${styles.mixins.square("100%")};
		object-fit: cover;
	}

	@layer base {
		--frames: 1;
		--i: 0;
		--adjust-order: 0;
		--adjust-rate: 1;

		img {
			${styles.mixins.square("100%")};
		}
	}

	.items-view-item:not(:hover, :focus-visible, .initial-step-item) & img {
		animation: none;
	}

	:is(.initial-step, .step-sequence-item) & img {
		animation-play-state: paused;
		animation-delay: calc((var(--i) + var(--adjust-order)) * -${MILLISECONDS_PER_FRAME}ms / var(--adjust-rate) + 1ms);
	}

	.icon {
		font-size: 48px;
	}

	@layer components {
		${({ $frames }) => $frames !== undefined && css`--frames: ${$frames};`};

		${({ $effect, $static }) => {
			if ($static) return;
			const stepChangeHueStep = getStepChangeHueStep($effect);
			if (stepChangeHueStep !== null)
				return css`
					img {
						filter: hue-rotate(${360 / stepChangeHueStep}deg);
						animation: ${keyframes`
							${forMapFromTo(1, stepChangeHueStep, 1, i => {
								const offset = 100 / stepChangeHueStep * (i - 1);
								return `${offset === 0 ? "0%, 100%" : offset + "%"} { filter: hue-rotate(${360 / stepChangeHueStep * i}deg); }`;
							})}
						`};
					}
				`;
			return {
				hFlip: css`
					img {
						scale: -1 1;
						animation: ${keyframes`
							0%, 100% { scale: -1 1; }
							50% { scale: 1; }
						`};
					}
				`,
				vFlip: css`
					img {
						scale: 1 -1;
						animation: ${keyframes`
							0%, 100% { scale: 1 -1; }
							50% { scale: 1; }
						`};
					}
				`,
				ccwFlip: css`
					img {
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
						width: 100cqh;
						height: 100cqw;
						rotate: -90deg;
						animation: ${keyframes`
							0%, 100% { rotate: -90deg; width: 100cqh; height: 100cqw; }
							25% { rotate: -180deg; width: 100%; height: 100%; }
							50% { rotate: -270deg; width: 100cqh; height: 100cqw; }
							75% { rotate: 0deg; width: 100%; height: 100%; }
						`};
					}
				`,
				cwRotate: css`
					img {
						width: 100cqh;
						height: 100cqw;
						rotate: 90deg;
						animation: ${keyframes`
							0%, 100% { rotate: 90deg; width: 100cqh; height: 100cqw; }
							25% { rotate: 180deg; width: 100%; height: 100%; }
							50% { rotate: 270deg; width: 100cqh; height: 100cqw; }
							75% { rotate: 0deg; width: 100%; height: 100%; }
						`};
					}
				`,
				turned: css`
					img {
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
						animation-timing-function: ${eases.easeOutSmooth} !important;
					}
				`,
				hMirror: css`
					--adjust-order: 1;
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
					--adjust-order: 1;
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
						filter: invert(1);
						animation: ${keyframes`
							0%, 100% { filter: invert(1); }
							50% { filter: none; }
						`};
					}
				`,
				luminInvert: css`
					img {
						filter: invert(1) hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: invert(1) hue-rotate(180deg); }
							50% { filter: none; }
						`};
					}
				`,
				negativeFade: css`
					--frames: 1;
					--adjust-order: 2;
					img:nth-child(1) {
						opacity: 0.5;
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
				rotInvertHue: css`
					img {
						filter: hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: hue-rotate(180deg); }
							25% { filter: invert(1); }
							50% { filter: invert(1) hue-rotate(180deg); }
							75% { filter: none; }
						`};
					}
				`,
				rotInvertLumin: css`
					img {
						filter: invert(1) hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: invert(1) hue-rotate(180deg); }
							25% { filter: invert(1); }
							50% { filter: hue-rotate(180deg); }
							75% { filter: none; }
						`};
					}
				`,
				altInvertHue: css`
					img {
						filter: invert(1);
						animation: ${keyframes`
							0%, 100% { filter: invert(1); }
							25% { filter: hue-rotate(180deg); }
							50% { filter: invert(1) hue-rotate(180deg); }
							75% { filter: none; }
						`};
					}
				`,
				altInvertLumin: css`
					img {
						filter: invert(1);
						animation: ${keyframes`
							0%, 100% { filter: invert(1); }
							25% { filter: invert(1) hue-rotate(180deg); }
							50% { filter: hue-rotate(180deg); }
							75% { filter: none; }
						`};
					}
				`,
				hueInvert: css`
					img {
						filter: hue-rotate(180deg);
						animation: ${keyframes`
							0%, 100% { filter: hue-rotate(180deg); }
							50% { filter: none; }
						`};
					}
				`,
				chromatic: css`
					img {
						filter: grayscale(1);
						animation: ${keyframes`
							0%, 100% { filter: grayscale(1); }
							50% { filter: none; }
						`};
					}
				`,
				chromaticFade: css`
					--adjust-order: 2;
					img {
						filter: grayscale(0.5);
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
					--adjust-order: 1;
					--adjust-rate: 0.5;
					img {
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
					--adjust-order: 1;
					--adjust-rate: 0.5;
					img {
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
					--adjust-order: 1;
					img {
						scale: 1 0.75;
						animation: ${keyframes`
							0%, 100% { scale: 1 0.75; }
							50% { scale: 0.75 1; }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				pendulum: css`
					--adjust-order: 1;
					img {
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
						animation-timing-function: ${eases.easeOutQuad} !important;
					}
				`,
				wipeRight: css`
					--adjust-order: 1;
					img {
						clip-path: inset(0 75% 0 0);
						animation: ${keyframes`
							0% { clip-path: inset(0 100% 0 0); }
							50% { clip-path: inset(0 0 0 0); }
							100% { clip-path: inset(0 0 0 100%); }
						`};
						animation-timing-function: ${eases.easeOutMax} !important;
					}
				`,
				wipeRight1: css`
					--adjust-order: 1;
					img {
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
			}[$effect];
		}}
	}
`;

export default function PreviewPrve({ thumbnail, effect, frames, step, isDegree, ...htmlAttrs }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
	/** Effect identifier. */
	effect: string;
	/** Frame count. */
	frames?: number;
	/** The step for displaying initial step. */
	step?: number;
	/** Use degree angle instead of step. */
	isDegree?: boolean;
}, "div">) {
	const imageCount = {
		hMirror: 2,
		vMirror: 2,
		ccwMirror: 4,
		cwMirror: 4,
		radialBlur: 2,
		negativeFade: 2,
	}[effect] ?? 1;

	// const canvasFilters = useCanvasFilters(thumbnail);
	// const webglFilters = useWebglFilters(thumbnail);

	/* const alterImage = {
		negativeLuma: webglFilters?.negativeLuma,
		radialBlur: webglFilters?.radialBlur,
	}[effect]; */

	const webglFilters = ["negativeLuma", "radialBlur"];

	const animatedImage = {
		pingpong: Tuple(prvePingpongImage, prvePingpongStaticImage),
		whirl: Tuple(prveWhirlImage, prveWhirlStaticImage),
	}[effect];

	const isStatic = step !== undefined && (step <= 0 || frames !== undefined && step > frames || isDegree);

	return (
		<StyledPreviewPrve
			$effect={effect}
			$frames={frames}
			$static={isStatic}
			{...htmlAttrs}
		>
			{step === 0 ? <img src={thumbnail} alt="" /> :
			isDegree ? <img src={thumbnail} alt="" style={{ rotate: (step ?? 0) + "deg" }} /> :
			isStatic ? <Icon name="colored/question_circle" /> :
			webglFilters.includes(effect) ? /* isStepSequence ? undefined : */ <WebglFilter src={thumbnail} effect={effect} step={step} /> :
			forMap(imageCount, i => animatedImage !== undefined ?
				<HoverToChangeImg key={i} animatedSrc={animatedImage[0]} staticSrc={animatedImage[1]} /> :
				<img key={i} src={thumbnail} alt="" />)}
		</StyledPreviewPrve>
	);
}

/**
 * Custom hook that checks if the Items View Item is hovered and if the user does not prefer reduced motion.
 *
 * This hook uses the `ItemsView.Item.StateContext` to get the hover state and checks the user's
 * preference for reduced motion using the `prefers-reduced-motion` media query.
 *
 * @returns Is the Items View Item hovered and does the user prefer motion?
 */
function useHoverWhenPreferringMotion() {
	const { hover } = useContext(ItemsView.Item.StateContext);
	const prefersMotion = !useMediaQuery.reduceMotion();
	return prefersMotion ? hover : false;
}

function HoverToChangeImg({ staticSrc, animatedSrc }: FCP<{
	/** Static image source path. */
	staticSrc: string;
	/** Animated picture source path. */
	animatedSrc: string;
}>) {
	const hover = useHoverWhenPreferringMotion();

	return <img src={hover ? animatedSrc : staticSrc} />;
}

function WebglFilter({ src, effect, step }: {
	/** Image source path. */
	src: string;
	/** Effect identifier. */
	effect: string;
	/** The step for displaying initial step. */
	step?: number;
}) {
	const canvas = useDomRef<"canvas">();
	const filter = useRef<WebGLFilter>(undefined);
	const hover = useHoverWhenPreferringMotion();
	const [isMounted, setIsMounted] = useState(false);
	const [uniformName, setUniformName] = useState<string>();
	const [staticUniformValue, setStaticUniformValue] = useState(0);
	const [uniformValue, setUniformValue] = useState(0);
	const [uniformKeyframes, setUniformKeyframes] = useState<[number, number][]>([]);
	const currentKeyframe = useRef(0);
	const lastTime = useRef(0);
	const animationId = useRef<number>(undefined);
	const usingStaticUniformValue = !hover && step === undefined;
	const displayUniformValue = usingStaticUniformValue ? staticUniformValue : uniformValue;

	useAsyncMountEffect(async () => {
		if (!canvas.current) return;
		filter.current = initWebgl2(canvas.current);
		const image = await createImageFromUrl(src);
		filter.current.changeImage(image);
		filter.current.changeFilter(effect);
		setIsMounted(true);
	});

	useEffect(() => {
		if (!isMounted) return;
		switch (effect) {
			case "radialBlur":
				setUniformName("strength");
				setStaticUniformValue(0.25);
				setUniformKeyframes([[0.25, 0]]);
				break;
			case "negativeLuma":
				setUniformName("progress");
				setStaticUniformValue(-0.5);
				setUniformKeyframes([[0, 1], [-1, 0]]);
				break;
			default:
				break;
		}
	}, [isMounted, effect]);

	useEffect(() => {
		const animation = () => {
			if (usingStaticUniformValue) {
				lastTime.current = 0;
				currentKeyframe.current = 0;
				return;
			}
			const keyframeLength = uniformKeyframes.length ?? 0;
			if (keyframeLength === 0) return;
			const keyframe = uniformKeyframes[step !== undefined ? step - 1 : currentKeyframe.current];
			if (step !== undefined) {
				setUniformValue(keyframe[0]);
				return;
			}
			const nowTime = performance.now();
			if (lastTime.current === 0 || nowTime - lastTime.current >= MILLISECONDS_PER_FRAME) {
				lastTime.current = nowTime;
				currentKeyframe.current = (currentKeyframe.current + 1) % keyframeLength;
			}
			let progress = (nowTime - lastTime.current) / MILLISECONDS_PER_FRAME;
			// progress = 3 * progress ** (2 / 3) - 2 * progress; // eases.easeOutMax
			progress = Math.sqrt(progress); // eases.easeOutQuad
			const value = map(progress, 0, 1, keyframe[0], keyframe[1]);
			setUniformValue(value);
			animationId.current = requestAnimationFrame(animation);
		};
		animation();
		return () => cancelAnimationFrame(animationId.current);
	}, [usingStaticUniformValue, uniformKeyframes, step]);

	useEffect(() => {
		if (!isMounted || !filter.current || !uniformName) return;
		filter.current.uniform("1f", `${effect}_${uniformName}`, displayUniformValue);
		filter.current.apply();
		filter.current.gl.finish();
	}, [isMounted, effect, uniformName, displayUniformValue, canvas, step, src]);

	return <canvas ref={canvas} />;
}
