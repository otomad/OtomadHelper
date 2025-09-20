/* eslint-disable import/order */
import type { ColorScheme } from "helpers/color-mode";
import imgLight from "assets/images/loop/light.avif";
import imgDark from "assets/images/loop/dark.avif";
import imgBlack from "assets/images/loop/black.avif";
import imgLightContrast from "assets/images/loop/light_contrast.avif";
import imgDarkContrast from "assets/images/loop/dark_contrast.avif";
import imgBlackContrast from "assets/images/loop/black_contrast.avif";

const StyledPreviewColorScheme = styled.div.attrs({
	inert: true,
})`
	${styles.mixins.square("100%")};
	position: relative;

	.img {
		background-image: url("${imgDark}");

		&,
		&::before {
			position: absolute;
			inset: 0;
			object-fit: cover;
			background-size: cover;
		}

		&.light,
		&.auto {
			background-image: url("${imgLight}");
		}

		&.black {
			background-image: url("${imgBlack}");
		}

		&.contrast {
			background-image: url("${imgDarkContrast}");

			${ifColorScheme.light} & {
				background-image: url("${imgLightContrast}");
			}

			${ifColorScheme.black} & {
				background-image: url("${imgBlackContrast}");
			}
		}

		&.auto {
			${ifColorScheme.contrast} & {
				background-image: url("${imgLightContrast}");
			}

			&::before {
				content: "";
				background-image: url("${imgDark}");
				mask: linear-gradient(in oklch 120deg, black 35%, white 60%);
				mask-mode: luminance; // Use oklch and luminance make the gradient smoother.

				${ifColorScheme.contrast} & {
					background-image: url("${imgDarkContrast}");
				}
			}

			&.black-enabled::before {
				background-image: url("${imgBlack}");

				${ifColorScheme.contrast} & {
					background-image: url("${imgBlackContrast}");
				}
			}
		}
	}
`;

type ColorSchemeEx = ColorScheme | "black" | "contrast";

export default function PreviewColorScheme({ colorScheme: value }: FCP<{
	/** Color scheme. */
	colorScheme: ColorSchemeEx;
	children?: never;
}>) {
	const { amoledDark, scheme, contrast } = useSnapshot(colorModeStore);

	return (
		<StyledPreviewColorScheme>
			<div className={["img", value, { blackEnabled: value === "auto" && amoledDark }]} />
		</StyledPreviewColorScheme>
	);
}
