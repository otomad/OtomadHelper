import type { ColorScheme } from "helpers/color-mode";
import colors from "styles/colors";

const actualColorSchemes = ["light", "dark"] as const;

const StyledPreviewColorScheme = styled.div.attrs({
	inert: "" as never as boolean, // WARN: In React 19, change it to true, because React 18 is not standard.
})`
	position: relative;

	&,
	.container {
		${styles.mixins.gridCenter()};
		${styles.mixins.square("100%")};
	}

	.container:nth-child(2) {
		position: absolute;
		clip-path: polygon(30% 100%, 100% 100%, 100% 0%, 70% 0%);
	}

	${actualColorSchemes.map((scheme, i) => css`
		.container[data-scheme="${scheme}"] {
			background-color: ${colors["background-color"][i]};

			button {
				${styles.mixins.square("60%")};
				min-width: unset;
				background-color: ${colors["fill-color-control-default"][i]};
				border-color: ${colors["stroke-color-control-stroke-default"][i]};

				${scheme === "dark" ? css`
					border-top-color: ${colors["stroke-color-control-stroke-secondary"][i]};
				` : css`
					border-bottom-color: ${colors["stroke-color-control-stroke-secondary"][i]};
				`}
			}

			.radio-button-label:has(input:not(:checked)) {
				.bullet {
					background-color: ${colors["fill-color-text-on-accent-primary"][i]};
					outline: 1px solid ${colors["stroke-color-control-stroke-secondary"][i]};
				}

				.base {
					background-color: ${colors["fill-color-control-alt-secondary"][i]};
					outline: 1px solid ${colors["stroke-color-control-strong-stroke-default"][i]};
				}
			}
		}
	`)}
`;

export default function PreviewColorScheme({ colorScheme, currentColorScheme }: FCP<{
	/** Color scheme. */
	colorScheme: ColorScheme;
	/** Current color scheme. */
	currentColorScheme: ColorScheme;
	children?: never;
}>) {
	return (
		<StyledPreviewColorScheme>
			{actualColorSchemes.map(scheme => (colorScheme === scheme || colorScheme === "auto") && (
				<div className="container" data-scheme={scheme} key={scheme}>
					<Button>
						<RadioButton id={colorScheme} value={[currentColorScheme]} plain />
					</Button>
				</div>
			))}
		</StyledPreviewColorScheme>
	);
}
