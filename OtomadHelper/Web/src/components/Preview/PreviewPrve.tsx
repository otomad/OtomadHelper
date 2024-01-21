/* eslint-disable indent */
const getDuration = (frames: number) => frames * 375 + "ms";

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
	
	.grid-view-item:not(:hover) & img {
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
				img:nth-child(1) {
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { scale: -1 1; }
						50% { scale: 1; }
					`} ${getDuration(2)} step-start infinite;
				}
				img:nth-child(2) {
					clip-path: inset(0 50% 0 0);
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 50% 0 0); }
						50% { clip-path: inset(0 0 0 50%); scale: -1 1; }
					`} ${getDuration(2)} step-start infinite both;
				}
			`,
			vMirror: css`
				img:nth-child(1) {
					scale: 1 -1;
					animation: ${keyframes`
						0%, 100% { scale: 1 -1; }
						50% { scale: 1; }
					`} ${getDuration(2)} step-start infinite;
				}
				img:nth-child(2) {
					clip-path: inset(0 0 50% 0);
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 0 50% 0); }
						50% { clip-path: inset(50% 0 0 0); scale: 1 -1; }
					`} ${getDuration(2)} step-start infinite both;
				}
			`,
			ccwMirror: css`
				img:nth-child(2) {
					clip-path: inset(0 50% 0 0);
					scale: -1 1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 50% 0 0); }
						25% { clip-path: inset(0 0 0 50%); }
						50% { clip-path: inset(0 0 0 50%); }
						75% { clip-path: inset(0 50% 0 0); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(3) {
					clip-path: inset(50% 0 0 0);
					scale: 1 -1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(50% 0 0 0); }
						25% { clip-path: inset(50% 0 0 0); }
						50% { clip-path: inset(0 0 50% 0); }
						75% { clip-path: inset(0 0 50% 0); }
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
						0%, 100% { clip-path: inset(0 0 0 50%); }
						25% { clip-path: inset(0 0 0 50%); }
						50% { clip-path: inset(0 50% 0 0); }
						75% { clip-path: inset(0 50% 0 0); }
					`} ${getDuration(4)} step-start infinite;
				}
				img:nth-child(3) {
					clip-path: inset(0 0 50% 0);
					scale: 1 -1;
					animation: ${keyframes`
						0%, 100% { clip-path: inset(0 0 50% 0); }
						25% { clip-path: inset(50% 0 0 0); }
						50% { clip-path: inset(50% 0 0 0); }
						75% { clip-path: inset(0 0 50% 0); }
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
			monochrome: css`
				img {
					filter: grayscale(1);
					animation: ${keyframes`
						0%, 100% { filter: grayscale(1); }
						50% { filter: none; }
					`} ${getDuration(2)} step-start infinite;
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

	return (
		<StyledPreviewPrve $name={name}>
			{forMap(imageCount, i =>
				<img key={i} src={thumbnail} draggable={false} />)}
		</StyledPreviewPrve>
	);
}
