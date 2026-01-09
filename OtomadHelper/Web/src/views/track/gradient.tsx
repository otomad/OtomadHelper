import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import { directionTypes, getParityIcon, parityTypes as _parityTypes } from "./grid";

const gradients = [
	{ group: "gradually", items: ["rainbow", "graSaturated", "graContrasted", "threshold"] },
	{ group: "alternately", items: ["altChromatic", "altNegative", "altLuminInvert", "altHueInvert", "rotInvert"] },
];
const parityTypes = _parityTypes.toMoved(-2, undefined, 1);
const DEFAULT_ITEM_WIDTH = 325;

export default function Gradient() {
	return (
		<div className="container">
			<MirrorGradientTrackFlyoutEditor />
		</div>
	);
}

function Gradient_legacy() {
	const {
		effect, descending: [descending, setDescending],
		viewOverlay, viewSquare, viewMirrorEdges, viewSize,
	} = useSelectConfig(c => c.track.gradient);
	const {
		enabled: enableGridIntegration, columns, autoColumns, direction, parity, parity2,
	} = useSelectConfig(c => c.track.gradient.gridIntegration);
	useSetLayoutEnabledOnSave("gradient", true);
	const order = useMemo(() => descending ? "descending" : "ascending", [descending]);
	const [showGridIntegration, setShowGridIntegration] = useState(false);
	const verticalDirection = direction[0].startsWith("tb");
	const [flip1RandomTimestamp, setFlip1RandomTimestamp] = useState(0), [flip2RandomTimestamp, setFlip2RandomTimestamp] = useState(0);

	return (
		<div className="container" style={{ marginBlockStart: 0, blockSize: "max-content" }}>
			<CommandBar.Group>
				<CommandBar position="right">
					<CommandBar.Item icon={order} caption={t[order]} details={t.descriptions.track.descending} onClick={() => setDescending(desc => !desc)} />
					<CommandBar.Item icon="grid" altCaption={t.titles.grid} caption={t.track.gradient.gridIntegration} details={t.descriptions.track.gradient.gridIntegration} onClick={() => setShowGridIntegration(true)} />
					<hr />
					<CommandBar.Item iconOnly icon="extra_large_icons" caption={t.view} details={t.descriptions.track.view}>
						<ToggleSwitch on={viewOverlay} icon="photo_filter">{t.fit.overlay}</ToggleSwitch>
						<ToggleSwitch on={viewSquare} icon="grid">{t.track.grid.square}</ToggleSwitch>
						<ToggleSwitch on={viewMirrorEdges} icon="image_reflection" lock={viewOverlay[0] ? false : null}>{t.track.grid.mirrorEdges}</ToggleSwitch>
						<Flyout.Item icon="resize" title={t.size} />
						<Flyout.Item style={{ paddingBlockStart: "4px" }}>
							<Slider
								value={viewSize}
								min={200}
								max={487}
								step={1}
								defaultValue={DEFAULT_ITEM_WIDTH}
								displayValue
							/>
						</Flyout.Item>
					</CommandBar.Item>
				</CommandBar>
			</CommandBar.Group>

			<ContentDialog
				title={t.track.gradient.gridIntegration}
				shown={[showGridIntegration, setShowGridIntegration]}
				width={1000}
				peek
			>
				<div className="container">
					<SettingsCardToggleSwitch on={enableGridIntegration} icon="lightbulb" title={t.enabled} details={t.descriptions.track.gradient.gridIntegration.enabled} onChange={viewSquare[1]} />
					<EmptyMessage.Typical icon="grid" name={t.track.gradient.gridIntegration} enabled={enableGridIntegration}>
						<SettingsCard icon={verticalDirection ? "rows" : "columns"} title={t({ context: "full" }).track.grid[verticalDirection ? "row" : "column"]}>
							<TextBox.Number value={columns} min={1} max={100} />
						</SettingsCard>
						<SettingsCardToggleSwitch on={autoColumns} icon="checkmark" title={t.track.gradient.gridIntegration.autoLineLength} details={t.descriptions.track.gradient.gridIntegration.autoLineLength} />
						<Subheader>{t.titles.parameters}</Subheader>
						<ExpanderRadio
							title={t.track.grid.direction}
							items={directionTypes}
							value={direction}
							view="tile"
							idField
							nameField={dir => t.track.grid.direction[new VariableName(dir).camel]}
							checkInfoCondition={dir => t.track.grid.direction[new VariableName(dir!).camel]}
							icon="lr_tb"
							iconField={() => "lr_tb"}
							dirBasedIcon={direction[0]}
							itemsViewItemAttrs={dir => ({ dirBasedIcon: dir })}
						/>
						<Subheader>{t.track.gradient.groups.alternately}</Subheader>
						{...[1, 0].map(i => (
							<ExpanderRadio
								key={`parity${i ? 1 : 2}`}
								title={i ? t.track.grid.parity : t.track.grid.paritySpare({ number: 2 })}
								items={parityTypes}
								value={i ? parity : parity2}
								icon={getParityIcon((i ? parity : parity2)[0])}
								iconField={parity => getParityIcon(parity)}
								view="tile"
								idField
								nameField={parity => t.track.grid.parity[new VariableName(parity).camel]}
								checkInfoCondition={parity => t.track.grid.parity[new VariableName(parity!).camel]}
								onItemClick={option => option === "random" && (i ? setFlip1RandomTimestamp : setFlip2RandomTimestamp)(Date.now())}
							/>
						))}
						{/* <Subheader>{t.track.gradient.groups.gradually}</Subheader> */}
						{/* TODO: 逐渐组设置，包括形状（如菱形、方形、圆形）、中心点。 */}
					</EmptyMessage.Typical>
				</div>
			</ContentDialog>

			{gradients.map(({ group, items }) => (
				<Fragment key={group}>
					<ItemsView view="grid" current={effect} itemWidth={viewSize[0]}>
						<Subheader>{t.track.gradient.groups[group]}</Subheader>
						{items.map(id => (
							<ItemsView.Item
								key={id}
								id={id}
								image={(
									<PreviewGradient
										thumbnail={exampleThumbnail}
										square={viewSquare[0]}
										mirrorEdges={viewMirrorEdges[0]}
										overlay={viewOverlay[0]}
										effect={id}
										descending={descending}
										direction={direction[0]}
										parity={enableGridIntegration && group === "alternately" && [parity[0], parity2[0]]}
										randomTimestamp={[flip1RandomTimestamp, flip2RandomTimestamp]}
									/>
								)}
							>
								{t.track.gradient.effects[id]}
							</ItemsView.Item>
						))}
					</ItemsView>
				</Fragment>
			))}
		</div>
	);
}
