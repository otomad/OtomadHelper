import { MILLISECONDS_PER_FRAME } from "./PreviewPrve";

function floatIn(from: "up" | "down" | "left" | "right", previewIdeality: boolean) {
	const fromOrigin = from === "up" ? "top" : from === "down" ? "bottom" : from;
	const toOrigin = from === "up" ? "bottom" : from === "down" ? "top" : from === "left" ? "right" : "left";
	const ZOOM = 1 / 0.75, translate = (ZOOM - 1) * 50;
	const getTranslate = (placement: typeof fromOrigin) => {
		const value = translate;
		return placement === "left" ? `${value}%` : placement === "right" ? `${-value}%` : placement === "top" ? `0 ${value}%` : `0 ${-value}%`;
	};
	return css`
		img {
			translate: ${getTranslate(fromOrigin)};
			scale: ${ZOOM};
			animation: ${keyframes`
				from { translate: ${getTranslate(fromOrigin)}; }
				to { translate: ${getTranslate(toOrigin)}; }
			`};
			animation-timing-function: ${eases.easeInOutSmooth};
			${!previewIdeality ? css`
				animation: ${keyframes`
					from { translate: ${getTranslate(fromOrigin)}; }
					to { translate: ${getTranslate(toOrigin)}; }
				`};
			` : css`
				--frames: 2;
				${from.in("left", "right") ? css`
					animation:
						${keyframes`
							0%, 100% { translate: ${getTranslate(fromOrigin)}; }
							50% { translate: ${getTranslate(toOrigin)}; }
						`},
						${keyframes`
							0%, 50% { scale: ${ZOOM}; }
							50.001%, 100% { scale: ${-ZOOM} ${ZOOM}; }
						`};
				` : css`
					animation:
						${keyframes`
							0%, 50.001% { translate: ${getTranslate(fromOrigin)}; }
							50%, 100% { translate: ${getTranslate(toOrigin)}; }
						`},
						${keyframes`
							0%, 50% { scale: ${ZOOM}; }
							50.001%, 100% { scale: ${-ZOOM} ${ZOOM}; }
						`};
				`}
			`}
		}
	`;
}

function rotate(direction: "cw" | "ccw", previewIdeality: boolean) {
	const OFFSET = 25;
	const FRAMES = 4;
	const sign = direction === "ccw" ? 1 : -1;
	return css`
		img {
			rotate: ${OFFSET * sign}deg;
			animation-timing-function: ${eases.easeOutMaterialEmphasized};
			${!previewIdeality ? css`
				animation: ${keyframes`
					from { rotate: ${OFFSET * sign}deg; }
					to { rotate: 0deg; }
				`};
			` : css`
				--frames: ${FRAMES};
				animation: ${keyframes`
					from { rotate: ${OFFSET * sign}deg; }
					to { rotate: 0deg; }
					${forMap(FRAMES, i => css`
						${withObject(100 / FRAMES * i, j => j === 0 ? j : j + 0.001)}% {
							rotate: ${OFFSET * sign + 360 / FRAMES * i * -sign}deg;
						}
						${100 / FRAMES * (i + 1)}% {
							rotate: ${360 / FRAMES * i * -sign}deg;
						}
					`)}
				`};
			`}
		}
	`;
}

const StyledPreviewParameterPreset = styled.div<{
	/** Effect name. */
	$name: string;
	/** Preview ideality instead of reality? */
	$previewIdeality?: boolean;
}>`
	${styles.mixins.square("100%")};

	img {
		position: absolute;
		object-fit: cover;
		animation-duration: calc(var(--frames) * ${MILLISECONDS_PER_FRAME}ms);
		animation-iteration-count: infinite;
	}

	@layer base {
		--frames: 1;

		img {
			${styles.mixins.square("100%")};
			animation-timing-function: ${eases.easeOutMax};
		}
	}

	.items-view-item:not(:hover, :focus-visible) & img {
		animation: none;
	}

	@layer components {
		${({ $name, $previewIdeality = false }) => {
			return {
				enter: css`
					img {
						scale: 1.1;
						animation: ${keyframes`
							from { scale: 1.1; }
							to { scale: 1; }
						`};
					}
				`,
				enterStaff: css`
					img {
						scale: 0.9;
						animation: ${keyframes`
							from { scale: 0.9; }
							to { scale: 1; }
						`};
					}
				`,
				exit: css`
					img {
						scale: 1;
						animation: ${keyframes`
							from { scale: 1; }
							to { scale: 1.1; }
						`};
					}
				`,
				fadeOut: css`
					img {
						opacity: 0.5;
						animation: ${keyframes`
							from { opacity: 1; }
							to { opacity: 0; }
						`};
						animation-timing-function: ${eases.easeInMax};
					}
				`,
				flashlight: css`
					img {
						filter: brightness(2);
						animation: ${keyframes`
							from { filter: brightness(2); }
							to { filter: none; }
						`};
					}
				`,
				floatLeft: floatIn("left", $previewIdeality),
				floatRight: floatIn("right", $previewIdeality),
				floatUp: floatIn("up", $previewIdeality),
				floatDown: floatIn("down", $previewIdeality),
				ccwRotate: rotate("ccw", $previewIdeality),
				cwRotate: rotate("cw", $previewIdeality),
				colorful: css`
					img {
						--frames: 2;
						filter: hue-rotate(-0.5turn);
						animation: ${keyframes`
							from { filter: hue-rotate(-0.5turn); }
							to { filter: hue-rotate(0.5turn); }
						`};
						animation-timing-function: linear;
					}
				`,
				oversaturation: css`
					img {
						filter: saturate(5);
						animation: ${keyframes`
							from { filter: saturate(1); }
							to { filter: saturate(5); }
						`};
					}
				`,
				highContrast: css`
					img {
						filter: contrast(5);
						animation: ${keyframes`
							from { filter: contrast(1); }
							to { filter: contrast(5); }
						`};
					}
				`,
				lumaFade: css`
					img {
						--threshold-change-brightness: 2;
						filter: brightness(var(--threshold-change-brightness)) url("#posterize");
						animation: ${keyframes`
							from { --threshold-change-brightness: 1.5; }
							to { --threshold-change-brightness: 0.75; }
						`};
					}
				`,
			}[$name];
		}}
	}
`;

export default function PreviewParameterPreset({ thumbnail, name, previewIdeality = false }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
	/** Effect name. */
	name: string;
	/** Preview ideality instead of reality? */
	previewIdeality?: boolean;
}>) {
	return (
		<StyledPreviewParameterPreset $name={name} $previewIdeality={previewIdeality}>
			<img src={thumbnail} alt="" />
			<SvgFilters />
		</StyledPreviewParameterPreset>
	);
}

function SvgFilters() {
	return (
		<DefineSvgFilter id="posterize">
			<feComponentTransfer>
				<feFuncR type="discrete" tableValues="0 0.25 0.5 0.75 1" />
				<feFuncG type="discrete" tableValues="0 0.25 0.5 0.75 1" />
				<feFuncB type="discrete" tableValues="0 0.25 0.5 0.75 1" />
			</feComponentTransfer>
		</DefineSvgFilter>
	);
}
