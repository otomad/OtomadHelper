import exampleThumbnail from "assets/images/ヨハネの氷.avif";

const ParityStyles = Enum({
	hFlip: { label: t.prve.effects.hFlip },
	vFlip: { label: t.prve.effects.vFlip },
	hMirror: { label: t.prve.effects.hMirror },
	vMirror: { label: t.prve.effects.vMirror },
	monochrome: { label: t.ytp.effects.monochrome },
	hueInvert: { label: t.prve.effects.hueInvert },
	luminInvert: { label: t.prve.effects.luminInvert },
});

const GradientStyles = Enum({
	hue: { label: t.stream.parameters.hue },
	saturation: { label: t.stream.parameters.saturation },
	contrast: { label: t.stream.parameters.contrast },
	threshold: { label: t.stream.parameters.threshold },
	brightness: { label: t.stream.parameters.brightness },
	opacity: { label: t.settings.appearance.backgroundImage.opacity },
});

const ParityPatterns = Enum({
	none: { label: t.none, icon: "dismiss_square" },
	all: { label: t.all, icon: "checkmark_square" },
	evenColumns: { icon: "parity/even_columns" },
	oddColumns: { icon: "parity/odd_columns" },
	evenRows: { icon: "parity/even_rows" },
	oddRows: { icon: "parity/odd_rows" },
	evenChecker: { icon: "parity/even_checker" },
	oddChecker: { icon: "parity/odd_checker" },
	evenDots: { icon: "parity/even_dots" },
	oddDots: { icon: "parity/odd_dots" },
	evenGridlines: { icon: "parity/even_gridlines" },
	oddGridlines: { icon: "parity/odd_gridlines" },
	random: { label: t.random, icon: "question_square" },
}, { labelPrefix: t.track.gradient.parities });
type GridParityType = typeof ParityPatterns.keyType;

const GradientPatterns = Enum({
	none: { label: t.none, icon: "dismiss_square" },
	flow: { icon: "flow_gradient" },
	linear: { icon: "linear_gradient" },
	radial: { icon: "radial_gradient" },
}, { labelPrefix: t.track.gradient.gradients });

function matchParity(parity: GridParityType, column: number, row: number, randomSeed?: string): boolean {
	if (parity === "random") return (randomSeed === undefined ? Math.random() : seedRandom(`${randomSeed},${column},${row}`)()) >= 0.5;
	return {
		none: false,
		all: true,
		evenColumns: !(column % 2),
		oddColumns: !!(column % 2),
		evenRows: !(row % 2),
		oddRows: !!(row % 2),
		evenChecker: !!((column + row) % 2),
		oddChecker: !((column + row) % 2),
		evenDots: !(column % 2) && !(row % 2),
		oddDots: !!(column % 2) && !!(row % 2),
		evenGridlines: !!(column % 2) || !!(row % 2),
		oddGridlines: !(column % 2) || !(row % 2),
	}[parity];
}

const StyledMirrorGradientTrackFlyoutEditor = styled.div`
	.forward > &.exit-active,
	.backward > &.exit-active {
		transition-timing-function: ${eases.easeInOutSmooth};
	}

	.forward > &${tgs(tgs.exit)},
	.backward > &${tgs(tgs.enter)} {
		translate: 0 20%;
		opacity: 0;
	}

	.forward > &${tgs(tgs.enter)},
	.backward > &${tgs(tgs.exit)} {
		translate: 0 -20%;
		opacity: 0;
	}

	&:is(.enter-active, .exit-active) {
		pointer-events: none;
	}

	.items-view {
		block-size: 140px;
	}

	> * {
		inline-size: 100% !important;
		overflow-inline: auto;
	}

	.items-view[data-page="style"] {
		${styles.mixins.overflowGradient("x", "1.25em")};
		display: flex;
		justify-content: start;
		overflow-inline: auto;

		.items-view-item {
			flex-shrink: 0;
			inline-size: 100px;

			.base {
				${styles.mixins.square("100px")};

				.preview-prve img {
					animation: none !important;
				}
			}

			.items-view-item-text-part {
				${styles.mixins.square("100%")};

				.marquee {
					animation-duration: 4s;
				}
			}
		}
	}

	.items-view[data-page="pattern"] {
		display: grid;
		grid-auto-flow: column;
		grid-template-rows: repeat(2, 1fr);
		inline-size: min-content;
		padding-block: 16px;

		.items-view-item {
			padding: 2px;

			.base {
				padding-inline-end: 16px;
			}

			.items-view-item-text-part .title {
				white-space: nowrap;
			}
		}
	}

	.gradient-pattern {
		display: flex;
		justify-content: space-evenly;

		hr {
			all: unset;
			margin-inline: 8px;
			border-inline-start: 1px solid ${c("stroke-color-divider-stroke-default")};
		}

		.parameters {
			display: grid;
			grid-template-columns: auto 1fr auto 1fr;
			gap: 8px;
			align-items: center;
			margin-inline: 16px;
			padding-block: 10px;

			label {
				display: flex;
				gap: inherit;
				align-items: center;
				white-space: nowrap;
			}

			.text-box {
				inline-size: 150px;
			}
		}
	}
`;

export default function MirrorGradientTrackFlyoutEditor() {
	const ariaId = useId();
	const tc = tAlias.track.gradient;
	const [currentPage, setCurrentPage] = useState<"style" | "pattern">("style");
	const [currentPattern, setCurrentPattern] = useState<"parity" | "gradient">("parity");
	const [currentStyle, setCurrentStyle] = useState("hFlip");
	const titles = useMemo<PropsOf<typeof Breadcrumb>["titles"]>(() =>
		currentPage === "style" ? [{ name: tc.style }] :
		currentPage === "pattern" ? [{ name: tc.style, onClick() { setCurrentPage("style"); } }, { name: tc.pattern }] :
		[{ name: tc.style, onClick() { setCurrentPage("style"); } }], [currentPage]);
	const prevTitles = usePrevious(titles);
	const transitionName = useMemo(() => titles.length < (prevTitles?.length ?? NaN) ? "forward" : "backward", [titles, prevTitles]);

	function clickAStyle(pattern: typeof currentPattern, style: string) {
		setCurrentPattern(pattern);
		setCurrentStyle(style);
		setCurrentPage("pattern");
	}

	return (
		<Contents className={transitionName}>
			<Breadcrumb large={false} titles={titles} />
			<SwitchTransition>
				<CssTransition key={`${currentPage}/${currentPattern}`} moreCoherentWhenCombo timeout={125}>
					<StyledMirrorGradientTrackFlyoutEditor>
						<HorizontalScroll as={Fragment}>
							{currentPage === "style" ? (
								<ItemsView data-page="style" view="grid" current={null}>
									<Subheader vertical>{t.track.gradient.groups.parity}</Subheader>
									{ParityStyles.map(({ key, label }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={<PreviewPrve thumbnail={exampleThumbnail} effect={key} />}
											onClick={() => clickAStyle("parity", key)}
										>
											<MarqueeIfOverflow>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
									<Subheader vertical>{t.track.gradient.groups.gradient}</Subheader>
									{GradientStyles.map(({ key, label }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={<PreviewGraduallyGradient thumbnail={exampleThumbnail} effect={key} />}
											onClick={() => clickAStyle("gradient", key)}
										>
											<MarqueeIfOverflow>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
								</ItemsView>
							) : currentPage === "pattern" ? currentPattern === "parity" ? (
								<ItemsView data-page="pattern" data-pattern="parity" view="tile" current={null}>
									{ParityPatterns.map(({ key, label, icon }) => (
										<ItemsView.Item
											id={key}
											key={key}
											icon={icon}
											// onClick={option === "random" ? () => (isH ? setFlipHRandomTimestamp : setFlipVRandomTimestamp)(Date.now()) : undefined}
										>
											{label}
										</ItemsView.Item>
									))}
								</ItemsView>
							) : currentPattern === "gradient" ? (
								<div className="gradient-pattern">
									<ItemsView data-page="pattern" data-pattern="gradient" view="tile" current={null}>
										{GradientPatterns.map(({ key, label, icon }) => (
											<ItemsView.Item
												id={key}
												key={key}
												icon={icon}
												// onClick={option === "random" ? () => (isH ? setFlipHRandomTimestamp : setFlipVRandomTimestamp)(Date.now()) : undefined}
											>
												{label}
											</ItemsView.Item>
										))}
									</ItemsView>
									<hr />
									<div className="parameters">
										<label htmlFor={`${ariaId}-input-start`}>
											<Icon name="stream_input" />
											{tc.parameters.input}
										</label>
										<TextBox.Number id={`${ariaId}-input-start`} value={[0]} min={-100} max={200} prefix={tc.parameters.startStop({ context: "short" })} />
										<label htmlFor={`${ariaId}-input-end`}>{t.rangeDash}</label>
										<TextBox.Number id={`${ariaId}-input-end`} value={[1]} min={-100} max={200} prefix={tc.parameters.endStop({ context: "short" })} />

										<label htmlFor={`${ariaId}-start-col`}>
											<IconWithHighlightPoint name="linear_gradient" location="left" />
											{tc.parameters.startStop}
										</label>
										<TextBox.Number id={`${ariaId}-start-col`} value={[0]} min={-100} max={200} prefix={t.track.grid.column} />
										<label htmlFor={`${ariaId}-start-row`}>,</label>
										<TextBox.Number id={`${ariaId}-start-row`} value={[0]} min={-100} max={200} prefix={t.track.grid.row} />

										<label htmlFor={`${ariaId}-end-col`}>
											<IconWithHighlightPoint name="linear_gradient" location="right" />
											{tc.parameters.endStop}
										</label>
										<TextBox.Number id={`${ariaId}-end-col`} value={[0]} min={-100} max={200} prefix={t.track.grid.column} />
										<label htmlFor={`${ariaId}-end-role`}>,</label>
										<TextBox.Number id={`${ariaId}-end-role`} value={[0]} min={-100} max={200} prefix={t.track.grid.row} />
									</div>
								</div>
							) : undefined : undefined}
						</HorizontalScroll>
					</StyledMirrorGradientTrackFlyoutEditor>
				</CssTransition>
			</SwitchTransition>
		</Contents>
	);
}

const StyledIconWithHighlightPoint = styled.div`
	${styles.mixins.square("1em")};
	--size: 6px;
	position: relative;

	@layer props {
		font-size: 20px;
	}

	.icon {
		display: flex;
	}

	.highlight-point {
		${styles.mixins.square("var(--size)")};
		position: absolute;
		top: calc(50% - var(--size) / 2);
		left: -1px;
		background-color: ${c("accent-color")};
		border-radius: 100%;

		&.right {
			right: -1px;
			left: unset;
		}

		&.center {
			left: calc(50% - var(--size) / 2);
		}
	}
`;

function IconWithHighlightPoint({ name, location }: {
	name: DeclaredIcons;
	location: "left" | "right" | "center";
}) {
	return (
		<StyledIconWithHighlightPoint>
			<Icon name={name} />
			<div className={["highlight-point", location]} />
		</StyledIconWithHighlightPoint>
	);
}
