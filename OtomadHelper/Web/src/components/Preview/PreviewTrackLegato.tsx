// cspell:disable-next-line
import Waveform1 from "assets/svg/waveform_dong.svg?react";
// cspell:disable-next-line
import Waveform2 from "assets/svg/waveform_ssr.svg?react";

const StylePreviewTrackEvent = styled.div`
	--r: 2;
	position: absolute;
	overflow: clip;
	background-color: ${c("accent-color")};
	border: 2px solid color-mix(in srgb, ${c("accent-color")}, ${c("fill-color-text-on-accent-primary")});
	border-radius: 5px;
	fill: ${c("fill-color-text-on-accent-primary")};

	svg {
		position: absolute;
		top: 0;
		left: 0;
		width: calc(200% / var(--preview-waveform-scale, 1));
		height: 100%;
		transition: ${fallbackTransitions}, width 0s;

		&:nth-of-type(2) {
			left: 12.5%;
		}
	}
`;

function PreviewTrackEvent({ subsequent }: {
	/** Includes subsequent? */
	subsequent?: boolean;
}) {
	return (
		<StylePreviewTrackEvent>
			<Waveform1 />
			{subsequent && <Waveform2 />}
		</StylePreviewTrackEvent>
	);
}

const StyledPreviewTrackLegato = styled.div<{
	/** Track legato mode. */
	$mode: Config.LegatoMode;
}>`
	--rows: 3;
	--columns: 10;
	position: relative;
	overflow: clip;

	@layer components {
		${styles.mixins.square("100%")};
	}

	.track-line {
		position: absolute;
		top: calc(100% / var(--rows));
		left: -15px;
		width: calc(100% + 30px);
		height: calc(100% / var(--rows));
		background-image: linear-gradient(to right, ${c("stroke-color-surface-stroke-default")} 50%, transparent 0%);
		background-repeat: repeat-x;
		background-size: 20px 2px;
		forced-color-adjust: none;

		&:nth-of-type(2) {
			background-position: left bottom;
		}
	}

	${StylePreviewTrackEvent} {
		top: calc((var(--r) - 1) * 100% / var(--rows));
		left: calc(var(--c1, 1) * 100% / var(--columns));
		width: calc(100% / var(--columns) * var(--d1, 1));
		height: calc(100% / var(--rows));
		animation: ${keyframes`
			0%,
			25% {
				--preview-waveform-scale: var(--s1, 1);
				left: calc(var(--c1, 1) * 100% / var(--columns) - 0.5px);
				width: calc(100% / var(--columns) * var(--d1, 1) + 1px);
			}
			75%,
			100% {
				--preview-waveform-scale: var(--s2, 1);
				left: calc(var(--c2, var(--c1, 1)) * 100% / var(--columns) - 0.5px);
				width: calc(100% / var(--columns) * var(--d2, var(--d1, 1)) + 1px);
			}
		`} 1.5s ${eases.easeInOutMaterialEmphasized} infinite both;

		@layer base {
			&:nth-of-type(2) {
				--c1: 3;
			}
			&:nth-of-type(3) {
				--c1: 6;
			}
			&:nth-of-type(4) {
				--c1: 8;
			}
		}

		${({ $mode }) => {
			switch ($mode) {
				case "stacking": return css`
					&:nth-of-type(2) {
						--c2: 2;
					}
					&:nth-of-type(3) {
						--c2: 3;
					}
					&:nth-of-type(4) {
						--c2: 4;
					}
				`;
				case "stackingAllAfter": return css`
					&:nth-of-type(2) {
						--c2: 2;
					}
					&:nth-of-type(3) {
						--c2: 3;
					}
					&:nth-of-type(4) {
						--c2: 4;
					}
					&:nth-of-type(5) {
						--c1: 11;
						--c2: 6;
					}
					&:nth-of-type(6) {
						--c1: 15;
						--c2: 8;
					}
				`;
				case "stackingAllTracks": return css`
					&:nth-of-type(2) {
						--c2: 2;
					}
					&:nth-of-type(3) {
						--c2: 3;
					}
					&:nth-of-type(4) {
						--c2: 4;
					}
					&:nth-of-type(5) {
						--r: 1;
						--c1: 3;
						--c2: 2;
					}
					&:nth-of-type(6) {
						--r: 3;
						--c1: 6;
						--c2: 3;
					}
				`;
				case "limitStretch": return css`
					&:nth-of-type(1),
					&:nth-of-type(2),
					&:nth-of-type(3) {
						--d2: 2;
					}
				`;
				case "stretch": return css`
					&:nth-of-type(1),
					&:nth-of-type(3) {
						--d2: 2;
					}
					&:nth-of-type(2) {
						--d2: 3;
						--s2: 1.5;
					}
				`;
				case "lengthen": return css`
					&:nth-of-type(1),
					&:nth-of-type(3) {
						--d2: 2;
						--s2: 2;
					}
					&:nth-of-type(2) {
						--d2: 3;
						--s2: 3;
					}

					svg {
						width: 58px; // Hard code for debounce.
					}
				`;
				case "increaseSpacing": return css`
					&:nth-of-type(2) {
						--c1: 2;
						--c2: 3;
					}
					&:nth-of-type(3) {
						--c1: 3;
						--c2: 5;
					}
					&:nth-of-type(4) {
						--c1: 4;
						--c2: 7;
					}
				`;
				case "increaseSpacingAllTracks": return css`
					&:nth-of-type(2) {
						--c1: 2;
						--c2: 3;
					}
					&:nth-of-type(3) {
						--c1: 3;
						--c2: 5;
					}
					&:nth-of-type(4) {
						--c1: 4;
						--c2: 7;
					}
					&:nth-of-type(5) {
						--r: 1;
						--c1: 2;
						--c2: 3;
					}
					&:nth-of-type(6) {
						--r: 3;
						--c1: 3;
						--c2: 5;
					}
				`;
				default: break;
			}
		}}
	}
`;

export default function PreviewTrackLegato({ mode, ...htmlAttrs }: FCP<{
	/** Track legato mode. */
	mode: Config.LegatoMode;
}, "div">) {
	const eventCount = ({
		stackingAllAfter: 6,
		stackingAllTracks: 6,
		increaseSpacingAllTracks: 6,
	} as Record<Config.LegatoMode, number>)[mode] ?? 4;

	return (
		<StyledPreviewTrackLegato $mode={mode} {...htmlAttrs}>
			<hgroup className="track-line" />
			<hgroup className="track-line" />
			{forMap(eventCount, i => (
				<PreviewTrackEvent
					key={i}
					subsequent={mode === "stretch" && i === 2}
				/>
			), 1)}
		</StyledPreviewTrackLegato>
	);
}
