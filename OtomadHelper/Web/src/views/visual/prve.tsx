import exampleThumbnail from "assets/images/ヨハネの氷.png";

const controlModes = ["general", "samePitch", "differentSyllables"] as const;
const getControlModeIcon = (mode: string) => `prve_control_${new VariableName(mode).snake}`;
const prveEffect = (fx: string, initial: number = 0) => ({ fx, initial });
const DEFAULT_EFFECT = "normal";
const STEP_CHANGE_HUE = "stepChangeHue";
const getWhirlInfo = () => withObject(t.prve.effects, fx => `${fx.whirl} = ${fx.pingpong} + ${fx.hFlip}`);

/** With frames step. */
const $s = (frames: number, ...effectIds: string[]) => effectIds.map(effect => ({ effect, frames }));
type PrveClassEffect = {
	effect: string;
	frames: number;
};
class PrveClass {
	public static readonly all = [
		new PrveClass("flip", "flip", [...$s(2, "hFlip", "vFlip"), ...$s(4, "ccwFlip", "cwFlip")]),
		new PrveClass("rotation", "rotate", [...$s(4, "ccwRotate", "cwRotate"), ...$s(2, "turned")]),
		new PrveClass("scale", "resize_image", $s(1, "zoomOutIn")),
		new PrveClass("mirror", "dual_screen_mirror", [...$s(2, "hMirror", "vMirror"), ...$s(4, "ccwMirror", "cwMirror")]),
		new PrveClass("invert", "invert_color", [...$s(2, "negative", "luminInvert", "negativeBlur", "negativeThreshold")]),
		new PrveClass("hue", "hue", [...$s(2, "hueInvert"), ...forMapFromTo(3, 8, frames => ({ effect: STEP_CHANGE_HUE + frames, frames }))]),
		new PrveClass("chromatic", "black_and_white", $s(2, "chromatic", "chromaticBlur")),
		new PrveClass("time", "timer", $s(2, "pingpong", "whirl")),
		new PrveClass("time2", "timer_2", $s(1, "sharpRewind", "wobblePeriod")),
		new PrveClass("ec", "arrow_autofit_height_in", [...$s(1, "vExpansion", "vExpansionBounce", "vCompression", "vCompressionBounce", "vBounce"), ...$s(2, "slantDown", "slantUp", "puyo")]),
		new PrveClass("swing", "arrow_rotate", $s(2, "pendulum")),
		new PrveClass("blur", "blur", $s(1, "gaussianBlur", "radialBlur")),
		new PrveClass("wipe", "double_tap_swipe", [...$s(2, "wipeRight"), ...$s(1, "wipeRight1", "splitVOut")]),
	];

	public readonly class: string;
	private constructor(
		klass: string,
		public readonly icon: DeclaredIcons,
		public readonly effects: PrveClassEffect[],
	) {
		this.class = klass;
		this.findEffectFrames = this.findEffectFrames.bind(this);
	}

	public static findClass(klass: string) { return PrveClass.all.find(_class => _class.class === klass); }
	public get effectIds() { return this.effects.map(effect => effect.effect) ?? []; }
	public static findClassEffects(klass: string) { return PrveClass.findClass(klass)?.effectIds ?? []; }
	public findEffectFrames(effect: string) { return this.effects.find(_effect => _effect.effect === effect)?.frames ?? 1; }
}

export default function Prve() {
	const [controlMode, setControlMode] = useState<typeof controlModes[number]>("general");
	const isGeneralCurrent = useMemo(() => controlMode === "general", [controlMode]);
	const { control, isMultiple, effects } = selectConfig(c => c.visual.prve[controlMode]);
	const selectionMode = useSelectionMode(isMultiple);
	const selectPrve = (klass: string) => {
		const classEffects = PrveClass.findClassEffects(klass);
		const flipEffects = PrveClass.findClassEffects("flip");
		return useStateSelector(
			effects,
			effects => {
				// if (!isMultiple) effects = effects.slice(0, 1);
				// const effect = effects.find(effect => classEffects.includes(effect));
				if (classEffects === undefined) return undefined;
				const fx = effects.map(effect => effect.fx).intersection(classEffects)[0];
				return fx ?? DEFAULT_EFFECT;
			},
			(effect, prevEffects) => {
				if (effect === undefined) return prevEffects;
				const addInitialValueToEffects = (effects: Iterable<string>) =>
					Array.from(effects, fx => prevEffects.find(effect => effect.fx === fx) ?? prveEffect(fx, 0));
				const isWhirl = effect === "whirl";
				const selectEffects = isWhirl ? ["hFlip", "pingpong"] : effect === DEFAULT_EFFECT && isMultiple[0] ? [] : [effect];
				if (!isMultiple[0]) return addInitialValueToEffects(selectEffects);
				const effects = new Set(prevEffects.map(effect => effect.fx));
				effects.delete(DEFAULT_EFFECT);
				effects.deletes(...classEffects);
				if (isWhirl) effects.deletes(...flipEffects);
				effects.adds(...selectEffects);
				if (effects.size === 0) effects.add(DEFAULT_EFFECT);
				return addInitialValueToEffects(effects);
			},
		);
	};
	const useInitialValue = (currentEffect: string) => useStateSelector(
		effects,
		effects => effects.find(effect => effect.fx === currentEffect)?.initial ?? 0,
		(initial, prevEffects) => {
			const draft = [...prevEffects];
			const effect = draft.find(effect => effect.fx === currentEffect);
			if (effect !== undefined) effect.initial = initial;
			else draft.push(prveEffect(currentEffect, initial));
			return draft;
		},
	);

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

			{PrveClass.all.map(({ class: klass, icon, effectIds, findEffectFrames }) => {
				const currentEffectState = selectPrve(klass), currentEffect = currentEffectState[0]!;
				return (
					<ExpanderRadio
						key={klass}
						title={t.prve.classes[klass]}
						disabled={!control[0]}
						icon={icon}
						items={[DEFAULT_EFFECT, ...effectIds]}
						value={currentEffectState}
						view="grid"
						idField
						nameField={getEffectName}
						imageField={effect => <PreviewPrve key={effect} thumbnail={exampleThumbnail} effect={effect} frames={findEffectFrames(effect)} />}
						checkInfoCondition={effect => !effect || effect === DEFAULT_EFFECT ? "" : getEffectName(effect)}
						alwaysShowCheckInfo
					>
						{klass === "time" ? <InfoBar status="info" title={getWhirlInfo()} /> : undefined}
						<InitialValue klass={klass} effect={currentEffect} initialValue={useInitialValue(currentEffect)} />
					</ExpanderRadio>
				);
			})}
		</div>
	);
}

const StyledInitialValue = styled(Expander.Item)`
	padding-inline: 15px;

	.text {
		flex: 0 0 auto;
		width: unset;
	}

	.trailing,
	.initial-value-items {
		flex: 1 1 0%;
		justify-content: flex-start;
		width: 100%;
	}

	.image-wrapper {
		height: 100px;

		* {
			transition: none !important;
		}
	}
`;

function InitialValue({ klass, effect, initialValue }: FCP<{
	/** Current PRVE class. */
	klass: string;
	/** Current PRVE effect in this class. */
	effect: string;
	/** The initial value of the PRVE effect. */
	initialValue: StateProperty<number>;
	children?: undefined;
}>) {
	const frames = PrveClass.findClass(klass)?.findEffectFrames(effect) ?? 1;

	return (
		<StyledInitialValue title={t.prve.initialValue} icon="replay">
			<ItemsView className="initial-value-items" view="grid" current={initialValue} $itemWidth={100}>
				{forMap(frames, j => {
					const i = (j + frames - 1) % frames; // Change the order from `0 1 2 3` to `3 0 1 2`.
					return (
						<ItemsView.Item
							image={(
								<PreviewPrve thumbnail={exampleThumbnail} effect={effect} frames={frames} style={{ "--i": i }} />
							)}
							key={j} id={j} className="initial-value-item"
						/>
					);
				})}
			</ItemsView>
		</StyledInitialValue>
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
	const effects = useSnapshot(configStore.visual.prve.general).effects.map(effect => effect.fx);
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
	const timeEffects = PrveClass.findClassEffects("time");
	const prve = useSnapshot(configStore.visual.prve);
	const effects = Object.values(prve).flatMap(control => control.effects);
	return !(effects.map(effect => effect.fx).intersection(timeEffects).length === 0);
}
