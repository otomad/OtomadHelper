import noise from "assets/images/noise_asset.png";
import type { systemBackdrops } from "views/settings";
type SystemBackdrop = typeof systemBackdrops[number]["name"];
const TRANSLATE = 7;

const StyledPreviewBackdrop = styled.div`
	${styles.mixins.square("100%")};
	${styles.mixins.gridCenter()};
	container: image / size;
	position: relative;

	.card {
		${styles.mixins.square("calc(78cqmin - 20px)")}
		--box-shadow-base:
			0 0 0 1px oklch(from ${c("accent-color")} l calc(c / 2) h / 18.352941%),
			// c("stroke-color-surface-stroke-default")
			0 3px 6px ${c("black", 43.9)};
		--opacity: 100%;
		position: absolute;
		overflow: clip;
		background-image: linear-gradient(135deg, rgb(from ${c("accent-color")} r g b / var(--opacity)) 0%, rgb(from color-mix(in oklch, ${c("accent-color")} 100%, black 30%) r g b / var(--opacity)) 100%);
		border-radius: 6px;
		box-shadow: if(
			${ifColorScheme.contrast}: none;
			${ifColorScheme.light}: var(--box-shadow-base), inset 0 -2px 8px ${c("stroke-color-control-stroke-on-accent-secondary", 22)};
			else: var(--box-shadow-base), inset 0 2px 2px ${c("stroke-color-control-stroke-on-accent-secondary")};
		);

		${ifColorScheme.at.light} {
			&.solid {
				box-shadow: var(--box-shadow-base), inset 0 -2px 2px ${c("stroke-color-control-stroke-on-accent-secondary", 22)};
			}
		}

		${ifColorScheme.at.contrast} {
			border: 1px solid black;
		}

		&.back {
			translate: ${-TRANSLATE}cqmin ${TRANSLATE}cqmin;
		}

		&.fore {
			--blur: 0;
			translate: ${TRANSLATE}cqmin ${-TRANSLATE}cqmin;
			backdrop-filter: blur(var(--blur)) !important;

			&::before,
			&::after {
				${styles.mixins.square("100%")};
				position: absolute;
				display: block;
				pointer-events: none;
			}

			&::after {
				content: "";
				background-color: ${c("background-color", "var(--mix)")};
			}

			&.acrylic {
				--blur: 6px;
				--opacity: 49.411765%;
				--mix: 30%;

				&::before {
					content: "";
					background: url("${noise}") left top repeat;
					background-size: calc(256px / var(--dpi));
					opacity: 0.125;
					mix-blend-mode: multiply;
				}
			}

			&.mica {
				--blur: 24px;
				--opacity: 47.647059%;
				--mix: 75%;

				&.mica-alt {
					--opacity: 32.745098%;
					--mix: 50%;

					${ifColorScheme.at.dark} {
						backdrop-filter: blur(var(--blur)) brightness(0.05) !important;
					}
				}
			}

			&.solid {
				background: ${c("background-color")};

				&::after {
					content: none;
				}
			}
		}
	}

	.items-view-item.grid:has(&) {
		.text-part .title {
			padding-inline-start: 13.5px;
		}
	}
`;

export default function PreviewBackdrop({ type }: { type: SystemBackdrop }) {
	return (
		<StyledPreviewBackdrop>
			<div className="card back" />
			<div className={["card fore", type === "micaAlt" ? "mica mica-alt" : type]} />
		</StyledPreviewBackdrop>
	);
}
