import exampleThumbnail from "assets/images/ヨハネの氷.avif";

const AlternatelyEffects = Enum({
	hFlip: { label: t.prve.effects.hFlip },
	vFlip: { label: t.prve.effects.vFlip },
	hMirror: { label: t.prve.effects.hMirror },
	vMirror: { label: t.prve.effects.vMirror },
	monochrome: { label: t.ytp.effects.monochrome },
	hueInvert: { label: t.prve.effects.hueInvert },
	luminInvert: { label: t.prve.effects.luminInvert },
});

const GraduallyEffects = Enum({
	hue: { label: t.stream.parameters.hue },
	saturation: { label: t.stream.parameters.saturation },
	contrast: { label: t.stream.parameters.contrast },
	threshold: { label: t.stream.parameters.threshold },
	brightness: { label: t.stream.parameters.brightness },
	opacity: { label: t.settings.appearance.backgroundImage.opacity },
});

const parityTypes = ["unflipped", "all_flipped", "even_columns", "odd_columns", "even_rows", "odd_rows", "even_checker", "odd_checker", "even_dots", "odd_dots", "even_gridlines", "odd_gridlines", "random"] as const;
type GridParityType = typeof parityTypes[number];
const getParityText = (parity: GridParityType) => t.track.grid.parity[new VariableName(parity).camel];
const getParityIcon = (parity: GridParityType): DeclaredIcons =>
	parity === "unflipped" ? "dismiss_square" : parity === "all_flipped" ? "checkmark_square" : parity === "random" ? "question_square" : `parity/${parity}`;

function matchParity(parity: GridParityType, column: number, row: number, randomSeed?: string): boolean {
	if (parity === "random") return (randomSeed === undefined ? Math.random() : seedRandom(`${randomSeed},${column},${row}`)()) >= 0.5;
	return {
		unflipped: false,
		all_flipped: true,
		even_columns: !(column % 2),
		odd_columns: !!(column % 2),
		even_rows: !(row % 2),
		odd_rows: !!(row % 2),
		even_checker: !!((column + row) % 2),
		odd_checker: !((column + row) % 2),
		even_dots: !(column % 2) && !(row % 2),
		odd_dots: !!(column % 2) && !!(row % 2),
		even_gridlines: !!(column % 2) || !!(row % 2),
		odd_gridlines: !(column % 2) || !(row % 2),
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
		overflow-inline: auto;
	}

	.items-view[data-page="effect"] {
		${styles.mixins.overflowGradient("x", "1.25em")};
		display: flex;
		justify-content: start;

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

	.items-view[data-page="target"] {
		display: flex;
		flex-flow: column wrap;
	}
`;

export default function MirrorGradientTrackFlyoutEditor() {
	const [currentPage, setCurrentPage] = useState<"effect" | "target">("effect");
	const titles = useMemo<PropsOf<typeof Breadcrumb>["titles"]>(() =>
		currentPage === "effect" ? [{ name: "Effect" }] :
		currentPage === "target" ? [{ name: "Effect", onClick() { setCurrentPage("effect"); } }, { name: "Target" }] :
		[{ name: "Effect", onClick() { setCurrentPage("effect"); } }], [currentPage]);
	const prevTitles = usePrevious(titles);
	const transitionName = useMemo(() => titles.length < (prevTitles?.length ?? NaN) ? "forward" : "backward", [titles, prevTitles]);

	return (
		<Contents className={transitionName}>
			<Breadcrumb large={false} titles={titles} />
			<SwitchTransition>
				<CssTransition key={currentPage} moreCoherentWhenCombo timeout={125}>
					<StyledMirrorGradientTrackFlyoutEditor>
						<HorizontalScroll as={Fragment}>
							{currentPage === "effect" ? (
								<ItemsView data-page="effect" view="grid" current={null}>
									<Subheader vertical>{t.track.gradient.groups.alternately}</Subheader>
									{AlternatelyEffects.map(({ key, label }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={<PreviewPrve thumbnail={exampleThumbnail} effect={key} />}
											onClick={() => setCurrentPage("target")}
										>
											<MarqueeIfOverflow>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
									<Subheader vertical>{t.track.gradient.groups.gradually}</Subheader>
									{GraduallyEffects.map(({ key, label }) => (
										<ItemsView.Item
											id={key}
											key={key}
											image={<PreviewGraduallyGradient thumbnail={exampleThumbnail} effect={key} />}
										>
											<MarqueeIfOverflow>{label}</MarqueeIfOverflow>
										</ItemsView.Item>
									))}
								</ItemsView>
							) : currentPage === "target" ? (
								<ItemsView data-page="target" view="tile" current={null}>
									{parityTypes.map(option => (
										<ItemsView.Item
											id={option}
											key={option}
											icon={getParityIcon(option)}
											// onClick={option === "random" ? () => (isH ? setFlipHRandomTimestamp : setFlipVRandomTimestamp)(Date.now()) : undefined}
										>
											{getParityText(option)}
										</ItemsView.Item>
									))}
								</ItemsView>
							) : undefined}
						</HorizontalScroll>
					</StyledMirrorGradientTrackFlyoutEditor>
				</CssTransition>
			</SwitchTransition>
		</Contents>
	);
}
