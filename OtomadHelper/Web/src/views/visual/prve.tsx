import exampleThumbnail from "assets/images/ヨハネの氷.png";

const controlModes = ["general", "samePitch", "differentSyllables"] as const;
const getControlModeIcon = (mode: string) => `prve_control_${new VariableName(mode).snake}`;
const DEFAULT_EFFECT = "normal";
const STEP_CHANGE_HUE = "stepChangeHue";
const getWhirlInfo = () => withObject(t.prve.effects, fx => `${fx.whirl} = ${fx.pingpong} + ${fx.hFlip}`);

/** With step. */
const $s = (step: number, ...effectIds: string[]) => effectIds.map(effect => ({ effect, step }));
const prves = [
	{ class: "flip", icon: "flip", effects: [...$s(2, "hFlip", "vFlip"), ...$s(4, "ccwFlip", "cwFlip")] },
	{ class: "rotation", icon: "rotate", effects: [...$s(4, "ccwRotate", "cwRotate"), ...$s(2, "turned")] },
	{ class: "scale", icon: "resize_image", effects: $s(1, "zoomOutIn") },
	{ class: "mirror", icon: "dual_screen_mirror", effects: [...$s(2, "hMirror", "vMirror"), ...$s(4, "ccwMirror", "cwMirror")] },
	{ class: "invert", icon: "invert_color", effects: [...$s(2, "negative", "luminInvert", "negativeBlur", "negativeThreshold")] },
	{ class: "hue", icon: "hue", effects: [...$s(2, "hueInvert"), ...forMapFromTo(3, 8, step => ({ effect: STEP_CHANGE_HUE + step, step }))] },
	{ class: "chromatic", icon: "black_and_white", effects: $s(2, "chromatic", "chromaticBlur") },
	{ class: "time", icon: "timer", effects: $s(2, "pingpong", "whirl") },
	{ class: "time2", icon: "timer_2", effects: $s(1, "sharpRewind", "wobblePeriod") },
	{ class: "ec", icon: "arrow_autofit_height_in", effects: [...$s(1, "vExpansion", "vExpansionBounce", "vCompression", "vCompressionBounce", "vBounce"), ...$s(2, "slantDown", "slantUp", "puyo")] },
	{ class: "swing", icon: "arrow_rotate", effects: $s(2, "pendulum") },
	{ class: "blur", icon: "blur", effects: $s(1, "gaussianBlur", "radialBlur") },
	{ class: "wipe", icon: "double_tap_swipe", effects: $s(1, "wipeRight", "splitVOut") },
];
const getEffectIds = (effects?: typeof prves[number]["effects"]) => effects?.map(effect => effect.effect) ?? [];
const findPrveClassEffects = (klass: string) => getEffectIds(prves.find(prve => prve.class === klass)?.effects);

export default function Prve() {
	const [controlMode, setControlMode] = useState<typeof controlModes[number]>("general");
	const isGeneralCurrent = useMemo(() => controlMode === "general", [controlMode]);
	const { control, isMultiple, effects } = selectConfig(c => c.visual.prve[controlMode]);
	const selectionMode = useSelectionMode(isMultiple);
	const selectPrve = (klass: string) => {
		const classEffects = findPrveClassEffects(klass);
		const flipEffects = findPrveClassEffects("flip");
		return useStateSelector(
			effects,
			effects => {
				// if (!isMultiple) effects = effects.slice(0, 1);
				// const effect = effects.find(effect => classEffects.includes(effect));
				const effect = effects.intersection(classEffects)[0];
				return effect ?? DEFAULT_EFFECT;
			},
			(effect, _effects) => {
				const isWhirl = effect === "whirl";
				const selectEffects = isWhirl ? ["hFlip", "pingpong"] : effect === DEFAULT_EFFECT && isMultiple ? [] : [effect];
				if (!isMultiple[0]) return selectEffects;
				const effects = new Set(_effects);
				effects.delete(DEFAULT_EFFECT);
				effects.deletes(...classEffects);
				if (isWhirl) effects.deletes(...flipEffects);
				effects.adds(...selectEffects);
				if (effects.size === 0) effects.add(DEFAULT_EFFECT);
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
				title={t({ context: "full" }).prve.control[controlMode]}
				details={t.descriptions.prve.control[controlMode]}
			/>
			<Subheader>{t.prve.classes}</Subheader>

			{prves.map(({ class: klass, icon, effects }) => (
				<ExpanderRadio<string>
					key={klass}
					title={t.prve.classes[klass]}
					disabled={!control[0]}
					icon={icon}
					items={[DEFAULT_EFFECT, ...getEffectIds(effects)]}
					value={selectPrve(klass)}
					view="grid"
					idField
					nameField={getEffectName}
					imageField={id => <PreviewPrve key={id} thumbnail={exampleThumbnail} id={id} />}
					checkInfoCondition={effect => effect === DEFAULT_EFFECT ? "" : effect}
					alwaysShowCheckInfo
				>
					{klass === "time" ? <InfoBar status="info" title={getWhirlInfo()} /> : undefined}
				</ExpanderRadio>
			))}
		</div>
	);
}

export function getStepChangeHueStep(effectId: string) {
	if (effectId?.startsWith(STEP_CHANGE_HUE))
		return +effectId.match(/\d+$/)![0];
	return null;
}

function getEffectName(effectId: string) {
	const { effects } = t.prve;
	const stepChangeHueStep = getStepChangeHueStep(effectId);
	if (stepChangeHueStep !== null)
		return effects[STEP_CHANGE_HUE]({ count: stepChangeHueStep });
	return effects[effectId];
}

export function usePrveCheckInfo() {
	let { effects } = useSnapshot(configStore.visual.prve.general);
	effects = effects.slice();
	const { samePitch, differentSyllables } = useSnapshot(configStore.visual.prve);
	const enableOtherSeparateControls = samePitch.control || differentSyllables.control;
	if (effects.includes("hFlip") && effects.includes("pingpong")) {
		const index = Math.min(effects.indexOf("hFlip"), effects.indexOf("pingpong"));
		effects[index] = "whirl";
		effects.removeAllItem("hFlip", "pingpong");
	}
	const effectId = effects[0] ?? DEFAULT_EFFECT;
	const effect = getEffectName(effectId);
	if (effects.length > 1 || enableOtherSeparateControls) return t.etc({ examples: effect });
	else return effect;
}

export function useIsForceStretch() {
	const timeEffects = findPrveClassEffects("time");
	const prve = useSnapshot(configStore.visual.prve);
	const effects = Object.values(prve).flatMap(control => control.effects);
	return !!effects.intersection(timeEffects).length;
}
