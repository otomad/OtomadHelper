import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import ConicGradientIcon from "assets/svg/icons/conic_gradient.svg?react";
import DiamondGradientIcon from "assets/svg/icons/diamond_gradient.svg?react";

const ParityStyles = Enum({
	hFlip: { label: t.prve.effects.hFlip },
	vFlip: { label: t.prve.effects.vFlip },
	monochrome: { label: t.ytp.effects.monochrome, effect: "chromatic" },
	hueInvert: { label: t.prve.effects.hueInvert },
	luminInvert: { label: t.prve.effects.luminInvert },
	hMirrorLeft: { label: t.prve.effects.hMirror_left, effect: "hMirror", step: 1 },
	hMirrorRight: { label: t.prve.effects.hMirror_right, effect: "hMirror", step: 2 },
	vMirrorTop: { label: t.prve.effects.vMirror_top, effect: "vMirror", step: 1 },
	vMirrorBottom: { label: t.prve.effects.vMirror_bottom, effect: "vMirror", step: 2 },
	twistCw: { label: t.ytp.effects.twist_cw, effect: "twist" },
	twistCcw: { label: t.ytp.effects.twist_ccw, effect: "twist_ccw" },
});

const GradientStyles = Enum({
	hue: { label: t.stream.parameters.hue },
	saturation: { label: t.stream.parameters.saturation },
	contrast: { label: t.stream.parameters.contrast },
	threshold: { label: t.stream.parameters.threshold },
	brightness: { label: t.stream.parameters.brightness },
	opacity: { label: t.settings.appearance.backgroundImage.opacity },
});

type StyleType = typeof ParityStyles.keyType | typeof GradientStyles.keyType;

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
	conic: { icon: ConicGradientIcon as never },
	diamond: { icon: DiamondGradientIcon as never },
	reflected: { icon: "reflected_gradient" },
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

const TRIPPY_COLORING_ID = "trippy-coloring";

const StyledGradientFlyoutEditor = styled.div`
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
		${styles.mixins.overflowGradient("x", "1.25em")};
		inline-size: 100% !important;
		overflow-inline: auto;
	}

	.items-view[data-page="style"] {
		display: flex;
		justify-content: start;

		.items-view-item {
			flex-shrink: 0;
			inline-size: 100px;

			> .base {
				${styles.mixins.square("100px")};
			}

			.items-view-item-text-part {
				${styles.mixins.square("100%")};
			}
		}
	}

	.items-view[data-page="pattern"] {
		display: grid;
		grid-auto-flow: column;
		grid-template-rows: repeat(2, 1fr);
		justify-content: start;
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

	.pattern {
		display: flex;
		justify-content: space-evenly;

		hr {
			all: unset;
			margin-inline: 8px;
			border-inline-start: 1px solid ${c("stroke-color-divider-stroke-default")};
		}

		.parameters {
			display: grid;
			grid-template-columns: auto 1fr auto 1fr auto;
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

			.sign {
				speak-as: literal-punctuation;
			}

			.exchange-rowspan-2 {
				grid-row: span 2;
				block-size: 100%;

				.icon,
				.icon svg {
					width: 16px;
					height: 58px;
				}
			}
		}
	}

	.asterisk.badge {
		position: absolute;
		inset-block-start: 3px;
		inset-inline-end: 3px;
		cursor: help;
	}

	.custom-parity-btn {
		inline-size: max-content;

		.content {
			flex-shrink: 0;
			gap: 16px;
			padding-block: 12px;
			padding-inline: 8px;
		}
	}

	.${TRIPPY_COLORING_ID} img {
		filter: invert(1) hue-rotate(45deg) saturate(2);
	}
`;

const MARQUEE_SPEED = 40;
export default function GradientFlyoutEditor() {
	const ariaId = useId();
	const tc = tAlias.track.gradient;
	const [currentPage, setCurrentPage] = useState<"style" | "pattern">("style");
	const [currentPattern, setCurrentPattern] = useState<"parity" | "gradient">("parity");
	const [currentStyle, setCurrentStyle] = useState<StyleType>("hFlip");
	const titles = useMemo<PropsOf<typeof Breadcrumb>["titles"]>(() => [
		{ name: t.titles.gradient({ context: "short" }), onClick: () => setCurrentPage("style") },
		currentPage === "pattern" && { name: ParityStyles.has(currentStyle) ? ParityStyles.allKeys[currentStyle].label : GradientStyles.has(currentStyle) ? GradientStyles.allKeys[currentStyle].label : "" },
	], [currentPage]);
	const prevTitles = usePrevious(titles);
	const transitionName = useMemo(() => titles.toCompacted().length < (prevTitles?.toCompacted().length ?? NaN) ? "forward" : "backward", [titles, prevTitles]);
	const stylesEl = useDomRef<"div">(), currentStylesScrollLeft = useRef(0);

	function clickOnStyle(pattern: typeof currentPattern, style: StyleType) {
		if (stylesEl.current) currentStylesScrollLeft.current = stylesEl.current.scrollLeft;
		setCurrentPattern(pattern);
		setCurrentStyle(style);
		setCurrentPage("pattern");
	}

	function onBackToStyles() {
		if (stylesEl.current) stylesEl.current.scrollTo({ left: currentStylesScrollLeft.current, behavior: "instant" });
	}

	return (
		<Contents className={transitionName}>
			<Breadcrumb large={false} titles={titles} />
			<SwitchTransition>
				<CssTransition key={`${currentPage}/${currentPattern}`} moreCoherentWhenCombo timeout={125} onEnter={onBackToStyles}>
					<StyledGradientFlyoutEditor>
						<HorizontalScroll as={Fragment}>
							{currentPage === "style" ? (
								<ItemsView data-page="style" ref={stylesEl} className={nameof.kebab({ GradientFlyoutEditor })} view="grid" current={null}>
									<ItemsView.Item
										id={TRIPPY_COLORING_ID}
										key={TRIPPY_COLORING_ID}
										image={<PreviewTwistEffect thumbnail={exampleThumbnail} direction="cw" className={TRIPPY_COLORING_ID} />}
										imageOverlay={<AsteriskHelp>{t.descriptions.track.gradient.trippyColoring}</AsteriskHelp>}
										role="button"
										_multiple
										aria-label={t.track.gradient.trippyColoring}
										checkmarkPosition="top left"
										// onClick={() => clickAStyle("parity", key)}
									>
										<MarqueeIfOverflow speed={MARQUEE_SPEED}><Preserves>{t.track.gradient.trippyColoring}</Preserves></MarqueeIfOverflow>
									</ItemsView.Item>
									<Subheader vertical>{tc.groups.parity}</Subheader>
									{ParityStyles.map(({ key, label, ...raw }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={(key.startsWith("twist") ?
												<PreviewTwistEffect thumbnail={exampleThumbnail} direction={key === "twistCcw" ? "ccw" : "cw"} /> :
												(
													<PreviewPrve
														thumbnail={exampleThumbnail}
														effect={"effect" in raw ? raw.effect : key}
														step={"step" in raw ? raw.step : 1}
														frames={"step" in raw ? 2 : undefined}
													/>
												))}
											imageOverlay={(() => {
												const tooltip = key.includes("Mirror") || key.includes("twist") ? t.descriptions.track.gradient.mirrorPriorityInfo :
													key.includes("Invert") ? t.descriptions.track.gradient.colorInvertInfo : undefined;
												return <AsteriskHelp>{tooltip}</AsteriskHelp>;
											})()}
											role="button"
											aria-label={label}
											onClick={() => clickOnStyle("parity", key)}
										>
											<MarqueeIfOverflow speed={MARQUEE_SPEED}>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
									<Subheader vertical>{tc.groups.gradient}</Subheader>
									{GradientStyles.map(({ key, label }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={<PreviewGraduallyGradient thumbnail={exampleThumbnail} effect={key} />}
											role="button"
											aria-label={label}
											onClick={() => clickOnStyle("gradient", key)}
										>
											<MarqueeIfOverflow speed={MARQUEE_SPEED}>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
								</ItemsView>
							) : currentPage === "pattern" ? (
								<div className="pattern">
									{currentPattern === "parity" ? (
										<>
											<ItemsView
												data-page="pattern"
												data-pattern="parity"
												view="tile"
												current={null}
												aria-label={tc.groups.parity}
											>
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
												<Button subtle icon="edit" className="custom-parity-btn">{t.custom}</Button>
											</ItemsView>
										</>
									) : currentPattern === "gradient" ? (
										<>
											<ItemsView
												data-page="pattern"
												data-pattern="gradient"
												view="tile"
												current={null}
												aria-label={tc.groups.gradient}
											>
												{GradientPatterns.map(({ key, label, icon }) => (
													<ItemsView.Item
														id={key}
														key={key}
														icon={typeof icon === "function" ? <Icon svgr={icon} /> : icon}
														// onClick={option === "random" ? () => (isH ? setFlipHRandomTimestamp : setFlipVRandomTimestamp)(Date.now()) : undefined}
													>
														{label}
													</ItemsView.Item>
												))}
											</ItemsView>
											<hr />
											<div role="group" className="parameters" aria-label={t.titles.parameters}>
												<label htmlFor={`${ariaId}-input-start`}>
													<Icon name="stream_input" />
													{tc.parameters.input}
												</label>
												<TextBox.Number id={`${ariaId}-input-start`} value={[0]} min={-100} max={200} prefix={tc.parameters.start} />
												<label className="sign" htmlFor={`${ariaId}-input-end`}>{t.rangeDash}</label>
												<TextBox.Number id={`${ariaId}-input-end`} value={[1]} min={-100} max={200} prefix={tc.parameters.end} />
												<Tooltip title={t.descriptions.track.gradient.exchange} placement="block">
													<Button icon="arrow_bidirectional_left_right" minWidthUnbounded />
												</Tooltip>

												<label htmlFor={`${ariaId}-start-col`}>
													<IconWithHighlightPoint name="linear_gradient" location="left" />
													{tc.parameters.start}
												</label>
												<TextBox.Number id={`${ariaId}-start-col`} value={[0]} min={-100} max={200} prefix={t.track.grid.column} />
												<label className="sign" htmlFor={`${ariaId}-start-row`}>,</label>
												<TextBox.Number id={`${ariaId}-start-row`} value={[0]} min={-100} max={200} prefix={t.track.grid.row} />
												<Tooltip title={t.descriptions.track.gradient.exchange} placement="block">
													<Button className="exchange-rowspan-2" icon="arrow_bidirectional_left_right_rowspan_2" minWidthUnbounded />
												</Tooltip>

												<label htmlFor={`${ariaId}-end-col`}>
													<IconWithHighlightPoint name="linear_gradient" location="right" />
													{tc.parameters.end}
												</label>
												<TextBox.Number id={`${ariaId}-end-col`} value={[0]} min={-100} max={200} prefix={t.track.grid.column} />
												<label className="sign" htmlFor={`${ariaId}-end-role`}>,</label>
												<TextBox.Number id={`${ariaId}-end-role`} value={[0]} min={-100} max={200} prefix={t.track.grid.row} />
											</div>
										</>
									) : undefined}
								</div>
							) : undefined}
						</HorizontalScroll>
					</StyledGradientFlyoutEditor>
				</CssTransition>
			</SwitchTransition>
		</Contents>
	);
}

const AsteriskHelp = ({ children: tooltip }: { children?: string }) => tooltip ?
	<Tooltip placement="block" title={tooltip}><Badge className="asterisk" status="asterisk" /></Tooltip> : undefined;

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
