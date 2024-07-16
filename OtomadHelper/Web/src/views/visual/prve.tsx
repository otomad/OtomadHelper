import exampleThumbnail from "assets/images/ヨハネの氷.png";

const prves = [
	{ class: "flip", icon: "placeholder", effects: ["hFlip", "vFlip", "ccwFlip", "cwFlip"] },
	{ class: "rotation", icon: "placeholder", effects: ["ccwRotate", "cwRotate", "turned"] },
	{ class: "scale", icon: "placeholder", effects: ["zoomOutIn"] },
	{ class: "mirror", icon: "placeholder", effects: ["hMirror", "vMirror", "ccwMirror", "cwMirror"] },
	{ class: "invert", icon: "placeholder", effects: ["negative", "luminInvert"] },
	{ class: "hue", icon: "placeholder", effects: ["hueInvert", ...forMapFromTo(3, 8, i => "stepChangeHue" + i)] },
	{ class: "chromatic", icon: "placeholder", effects: ["chromatic"] },
	{ class: "time", icon: "placeholder", effects: ["pingpong", "whirl"] },
	{ class: "time2", icon: "placeholder", effects: ["sharpRewind", "wobblePeriod"] },
	{ class: "ec", icon: "placeholder", effects: ["vExpansion", "vExpansionBounce", "vCompression", "vCompressionBounce", "vBounce", "slantDown", "slantUp", "puyo"] },
	{ class: "swing", icon: "placeholder", effects: ["pendulum"] },
	{ class: "blur", icon: "placeholder", effects: ["gaussianBlur", "radialBlur"] },
	{ class: "wipe", icon: "placeholder", effects: ["wipeRight", "splitVOut"] },
];

const controlModes = ["general", "samePitch", "differentSyllables"] as const;
const getControlModeIcon = (mode: string) => `prve_control_${new VariableName(mode).snake}`;
const defaultEffect = "normal";

export default function Prve() {
	const [controlMode, setControlMode] = useState<typeof controlModes[number]>("general");
	const isGeneralCurrent = useMemo(() => controlMode === "general", [controlMode]);
	const { control, isMultiple, effects } = selectConfig(c => c.visual.prve[controlMode]);
	const selectionMode = useSelectionMode(isMultiple);
	const selectPrve = (klass: string) => {
		const classEffects = prves.find(prve => prve.class === klass)?.effects ?? [];
		const flipEffects = prves.find(prve => prve.class === "flip")!.effects;
		return useStateSelector(
			effects,
			effects => {
				// if (!isMultiple) effects = effects.slice(0, 1);
				const effect = effects.find(effect => classEffects.includes(effect));
				return effect ?? defaultEffect;
			},
			(effect, _effects) => {
				const isWhirl = effect === "whirl";
				const selectEffects = isWhirl ? ["hFlip", "pingpong"] : effect === defaultEffect && isMultiple ? [] : [effect];
				if (!isMultiple[0]) return selectEffects;
				const effects = new Set(_effects);
				effects.delete(defaultEffect);
				effects.deletes(...classEffects);
				if (isWhirl) effects.deletes(...flipEffects);
				effects.adds(...selectEffects);
				if (effects.size === 0) effects.add(defaultEffect);
				return [...effects];
			},
		);
	};

	return (
		<div className="container">
			<StackPanel $align="space-between" $endAlignWhenWrap>
				<Segmented current={[controlMode, setControlMode]}>
					{controlModes.map(mode =>
						<Segmented.Item id={mode} key={mode} icon={getControlModeIcon(mode)}>{t.prve.control[mode]}</Segmented.Item>)}
				</Segmented>
				<Segmented current={selectionMode}>
					<Segmented.Item id="single" icon="single_select">{t.selectionMode.single}</Segmented.Item>
					<Segmented.Item id="multiple" icon="multiselect">{t.selectionMode.multiple}</Segmented.Item>
				</Segmented>
			</StackPanel>
			<SettingsCardToggleSwitch
				on={isGeneralCurrent ? [true] : control}
				disabled={isGeneralCurrent}
				icon={getControlModeIcon(controlMode)}
				title={t.prve.control({ mode: t.prve.control[controlMode] })}
				details={t.descriptions.prve.control[controlMode]}
			/>
			<Subheader>{t.prve.classes}</Subheader>

			{prves.map(({ class: klass, icon, effects }) => (
				<ExpanderRadio
					key={klass}
					title={t.prve.classes[klass]}
					disabled={!control[0]}
					icon={icon}
					items={[defaultEffect, ...effects]}
					value={selectPrve(klass)}
					view="grid"
					idField
					nameField
					imageField={name => <PreviewPrve key={name} thumbnail={exampleThumbnail} name={name} />}
					checkInfoCondition={effect => effect === defaultEffect ? "" : effect}
					alwaysShowCheckInfo
				>
					{klass === "time" ? <InfoBar status="info" title={t.descriptions.prve.whirl} /> : undefined}
				</ExpanderRadio>
			))}
		</div>
	);
}

export function usePrveCheckInfo() {
	let { effects: [effects] } = selectConfig(c => c.visual.prve.general);
	effects = effects.slice();
	const { samePitch, differentSyllables } = selectConfig(c => c.visual.prve);
	const enableOtherSeparateControls = samePitch[0].control || differentSyllables[0].control;
	if (effects.includes("hFlip") && effects.includes("pingpong")) {
		const index = Math.min(effects.indexOf("hFlip"), effects.indexOf("pingpong"));
		effects[index] = "whirl";
		effects.removeAllItem("hFlip", "pingpong");
	}
	const effect = effects[0] ?? defaultEffect;
	if (effects.length > 1 || enableOtherSeparateControls) return t.etc({ examples: effect });
	else return effect;
}
