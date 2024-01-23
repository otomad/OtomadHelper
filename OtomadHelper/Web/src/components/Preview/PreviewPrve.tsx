import prvePingpongImage from "assets/images/effects/prve_pingpong.gif";
import prveWhirlImage from "assets/images/effects/prve_whirl.gif";

/* eslint-disable indent */
export /* internal */ const getDuration = (frames: number) => frames * 375 + "ms";

const StyledPreviewPrve = styled.div<{
	/** 效果名称。 */
	$name: string;
}>`
	${styles.mixins.square("100%")};

	img {
		${styles.mixins.square("100%")};
		object-fit: cover;
		position: absolute;
	}

	.grid-view-item:not(:hover, :focus-visible) & img {
		animation: none;
	}

	${({ $name }) => {
		if ($name?.startsWith("stepChangeHue")) {
			const step = +$name.match(/\d+$/)![0];
			return css`
				img {
					filter: hue-rotate(${360 / step}deg);
					animation: ${keyframes`
						${forMapFromTo(1, step, i => {
							const offset = 100 / step * (i - 1);
							return `${!offset ? "0%, 100%" : offset + "%"} { filter: hue-rotate(${360 / step * i}deg); }`;
						})}
					`} ${getDuration(step)} step-start infinite;
				}
			`;
		}
		return {
			hFlip: css`
				img {
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { scale: -1 1; }
						50% { scale: 1; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			vFlip: css`
				img {
					scale: 1 -1;
					animation: ${keyframes`
						0%, 100% { scale: 1 -1; }
						50% { scale: 1; }
					`} ${getDuration(2)} step-start infinite;
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
					`} ${getDuration(4)} step-start infinite;
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
					`} ${getDuration(4)} step-start infinite;
				}
			`,
			ccwRotate: css`
				img {
					rotate: -90deg;
					animation: ${keyframes`
						0%, 100% { rotate: -90deg; }
						25% { rotate: -180deg; }
						50% { rotate: -270deg; }
						75% { rotate: 0deg; }
					`} ${getDuration(4)} step-start infinite;
				}
			`,
			cwRotate: css`
				img {
					rotate: 90deg;
					animation: ${keyframes`
						0%, 100% { rotate: 90deg; }
						25% { rotate: 180deg; }
						50% { rotate: 270deg; }
						75% { rotate: 0deg; }
					`} ${getDuration(4)} step-start infinite;
				}
			`,
			turned: css`
				img {
					rotate: 180deg;
					animation: ${keyframes`
						0%, 100% { rotate: 180deg; }
						50% { rotate: 0deg; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			zoomOutIn: css`
				img {
					scale: 10;
					animation: ${keyframes`
						from { scale: 10; }
						to { scale: 1; }
					`} ${getDuration(1)} ${eases.easeOutMax} alternate infinite;
				}
			`,
			hMirror: css`
				img:nth-child(2) {
					clip-path: inset(0 50% 0 0);
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 50% 0 0); scale: -1 1; }
						50% { clip-path: inset(0 0 0 50%); }
					`} ${getDuration(2)} step-start infinite both;
				}
			`,
			vMirror: css`
				img:nth-child(2) {
					clip-path: inset(0 0 50% 0);
					scale: 1 -1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 0 50% 0); scale: 1 -1; }
						50% { clip-path: inset(50% 0 0 0); }
					`} ${getDuration(2)} step-start infinite both;
				}
			`,
			ccwMirror: css`
				img:nth-child(2) {
					clip-path: inset(0 50% 0 0);
					scale: -1 1;
					animation: ${keyframes`
						0%, 75%, 100% { clip-path: inset(0 50% 0 0); }
						25%, 50% { clip-path: inset(0 0 0 50%); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(3) {
					clip-path: inset(50% 0 0 0);
					scale: 1 -1;
					animation: ${keyframes`
						0%, 25%, 100% { clip-path: inset(50% 0 0 0); }
						50%, 75% { clip-path: inset(0 0 50% 0); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(4) {
					clip-path: inset(50% 50% 0 0);
					scale: -1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(50% 50% 0 0); }
						25% { clip-path: inset(50% 0 0 50%); }
						50% { clip-path: inset(0 0 50% 50%); }
						75% { clip-path: inset(0 50% 50% 0); }
					`} ${getDuration(4)} step-start infinite;
				}
			`,
			cwMirror: css`
				img:nth-child(2) {
					clip-path: inset(0 0 0 50%);
					scale: -1 1;
					animation: ${keyframes`
						0%, 25%, 100% { clip-path: inset(0 0 0 50%); }
						50%, 75% { clip-path: inset(0 50% 0 0); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(3) {
					clip-path: inset(0 0 50% 0);
					scale: 1 -1;
					animation: ${keyframes`
						0%, 75%, 100% { clip-path: inset(0 0 50% 0); }
						25%, 50% { clip-path: inset(50% 0 0 0); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(4) {
					clip-path: inset(0 0 50% 50%);
					scale: -1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 0 50% 50%); }
						25% { clip-path: inset(50% 0 0 50%); }
						50% { clip-path: inset(50% 50% 0 0); }
						75% { clip-path: inset(0 50% 50% 0); }
					`} ${getDuration(4)} step-start infinite;
				}
			`,
			negative: css`
				img {
					filter: invert(1);
					animation: ${keyframes`
						0%, 100% { filter: invert(1); }
						50% { filter: none; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			luminInvert: css`
				img {
					filter: invert(1) hue-rotate(180deg);
					animation: ${keyframes`
						0%, 100% { filter: invert(1) hue-rotate(180deg); }
						50% { filter: none; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			hueInvert: css`
				img {
					filter: hue-rotate(180deg);
					animation: ${keyframes`
						0%, 100% { filter: hue-rotate(180deg); }
						50% { filter: none; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			chromatic: css`
				img {
					filter: grayscale(1);
					animation: ${keyframes`
						0%, 100% { filter: grayscale(1); }
						50% { filter: none; }
					`} ${getDuration(2)} step-start infinite;
				}
			`,
			vExpansion: css`
				img {
					transform-origin: center bottom;
					scale: 1 0.75;
					animation: ${keyframes`
						from { scale: 1 0.75; }
						to { scale: 1; }
					`} ${getDuration(1)} ${eases.easeOutMax} infinite;
				}
			`,
			vExpansionBounce: css`
				img {
					transform-origin: center bottom;
					scale: 1 0.75;
					animation: ${keyframes`
						from { scale: 1 0.75; }
						to { scale: 1; }
					`} ${getDuration(1)} ${eases.easeOutMax} alternate infinite;
				}
			`,
			vCompression: css`
				img {
					transform-origin: center bottom;
					animation: ${keyframes`
						from { scale: 1; }
						to { scale: 1 0.75; }
					`} ${getDuration(1)} ${eases.easeOutMax} infinite;
				}
			`,
			vCompressionBounce: css`
				img {
					transform-origin: center bottom;
					animation: ${keyframes`
						from { scale: 1; }
						to { scale: 1 0.75; }
					`} ${getDuration(1)} ${eases.easeOutMax} alternate infinite;
				}
			`,
			vBounce: css`
				img {
					scale: 1 0.5;
					animation: ${keyframes`
						from { scale: 1 0.5; }
						to { scale: 1; }
					`} ${getDuration(1)} ${eases.easeOutMax} infinite;
				}
			`,
			slantDown: css`
				img {
					transform: skewX(25deg) scaleX(0.5);
					transform-origin: center bottom;
					animation: ${keyframes`
						0% { transform: skewX(25deg) scaleX(0.5); }
						50% { transform: scale(0.5); animation-timing-function: ${eases.easeOutMax}; }
						100% { transform: skewX(-25deg) scaleX(0.5); }
					`} ${getDuration(2)} ${eases.easeInMax} alternate infinite;
				}
			`,
			slantUp: css`
				img {
					transform: skewX(25deg) scale(0.5);
					transform-origin: center bottom;
					animation: ${keyframes`
						0%, { transform: skewX(25deg) scale(0.5); }
						50% { transform: scaleX(0.5); animation-timing-function: ${eases.easeInMax}; }
						100% { transform: skewX(-25deg) scale(0.5); }
					`} ${getDuration(2)} ${eases.easeOutMax} alternate infinite;
				}
			`,
			puyo: css`
				img {
					scale: 1 0.75;
					animation: ${keyframes`
						0%, 100% { scale: 1 0.75; }
						50% { scale: 0.75 1; }
					`} ${getDuration(2)} ${eases.easeOutMax} infinite;
				}
			`,
			pendulum: css`
				img {
					scale: 0.5;
					rotate: -25deg;
					animation: ${keyframes`
						0%, 100% { rotate: -25deg; }
						50% { rotate: 25deg; }
					`} ${getDuration(2)} ${eases.easeOutMax} infinite;
				}
			`,
			gaussianBlur: css`
				img {
					filter: blur(5px);
					animation: ${keyframes`
						from { filter: blur(5px); }
						to { filter: blur(0); }
					`} ${getDuration(1)} ${eases.easeOutMax} infinite;
				}
			`,
			wipeRight: css`
				img {
					clip-path: inset(0 75% 0 0);
					animation: ${keyframes`
						0% { clip-path: inset(0 100% 0 0); }
						50% { clip-path: inset(0 0 0 0); }
						100% { clip-path: inset(0 0 0 100%); }
					`} ${getDuration(2)} ${eases.easeOutMax} infinite;
				}
			`,
			splitVOut: css`
				img {
					clip-path: inset(25% 0 25% 0);
					animation: ${keyframes`
						from { clip-path: inset(25% 0 25% 0); }
						to { clip-path: inset(0 0 0 0); }
					`} ${getDuration(1)} ${eases.easeOutMax} infinite;
				}
			`,
		}[$name];
	}}
`;

export default function PreviewPrve({ thumbnail, name }: FCP<{
	/** 缩略图。 */
	thumbnail: string;
	/** 效果名称。 */
	name: string;
}>) {
	const imageCount = {
		hMirror: 2,
		vMirror: 2,
		ccwMirror: 4,
		cwMirror: 4,
	}[name] ?? 1;

	const animatedImage = {
		pingpong: prvePingpongImage,
		whirl: prveWhirlImage,
	}[name];

	return (
		<StyledPreviewPrve $name={name}>
			{forMap(imageCount, i => animatedImage ?
				<ReactFreezeframe key={i} src={animatedImage} /> :
				<img key={i} src={thumbnail} draggable={false} />)}
		</StyledPreviewPrve>
	);
}
