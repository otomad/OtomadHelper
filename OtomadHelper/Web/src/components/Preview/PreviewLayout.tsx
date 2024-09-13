const FLIPPING_SCALE = 1.3;

const StyledPreviewLayout = styled.div<{
	/** Image source. */
	$img: string;
}>`
	${styles.mixins.square("100%")};
	container: preview-layout / size;
	position: relative;

	> * {
		inset-block-start: 0;
		${styles.mixins.square("100%")};

		&:not(:first-child) {
			position: absolute;
		}
	}

	section {
		${styles.mixins.square("100%", true)};
		position: relative;
		overflow: hidden;

		&::before {
			${styles.mixins.square("100%")};
			content: "";
			display: block;
			background-image: url(${styledProp("$img")});
			background-position: center;
			background-size: cover;
			border-radius: inherit;
		}

		&:not(.bass) {
			border-radius: 4px;
			animation: ${keyframes`
				from {
					translate: 0 30%;
					opacity: 0;
				}
			`} ${eases.easeOutMaterialEmphasized} 1s calc(var(--i) * 100ms) backwards;
		}

		&::after {
			${styles.mixins.square("100%")};
			--focus-ring-length-inner: 3px;
			content: "";
			position: absolute;
			inset: 0;
			display: block;
			border-radius: inherit;
			box-shadow: 0 0 0 var(--focus-ring-length-inner) white inset;
			mix-blend-mode: difference;
			pointer-events: none;
			animation: ${keyframes`
				from {
					--focus-ring-length-inner: 20px;
				}
			`} ${eases.easeOutMaterialEmphasized} 1s calc(var(--i) * 100ms) backwards;
		}

		&.lead {
			--size: 60%;
		}

		&.bass {
			filter: hue-rotate(180deg);

			&::after {
				--focus-ring-length-inner: 2px;
			}
		}

		// TODO: Replace it with real midi score.
		/* stylelint-disable-next-line no-duplicate-selectors */
		&::before {
			animation: ${keyframes`
				0% {
					scale: ${FLIPPING_SCALE};
				}

				50% {
					scale: 1;
				}

				50.0001% {
					scale: -${FLIPPING_SCALE} ${FLIPPING_SCALE};
				}

				100% {
					scale: -1 1;
				}
			`} ${eases.easeOutMax} 1s infinite;
		}
	}

	.lead-wrapper {
		${styles.mixins.gridCenter()};
	}

	.tracks {
		${styles.mixins.gridCenter()};
		grid-template-columns: repeat(2, 1fr);

		section {
			--size: 75%;
		}
	}
`;

export default function PreviewLayout({ thumbnail, ...htmlAttrs }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
}, "div">) {
	return (
		<StyledPreviewLayout $img={thumbnail}>
			<section className="bass" />
			<div className="tracks">
				{forMap(4, i => <section key={i} style={{ "--i": i }} />, 1)}
			</div>
			<div className="lead-wrapper">
				<section className="lead" style={{ "--i": 5 }} />
			</div>
		</StyledPreviewLayout>
	);
}
