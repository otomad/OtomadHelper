// C# doesn't support union type (mix boolean and string), so we can only use all strings.
const trueFalseAuto = ["false", "auto", "true"] as const;
const onOffAuto = { false: "off", auto: "auto", true: "on" } as const;
const icons = { false: "dismiss", auto: "auto", true: "checkmark" } as const;

export default function ThreeStageSwitch({ current, indeterminateText }: FCP<{
	/** The identifier of the selected segmented item. */
	current: StateProperty<TrueFalseAuto>;
	/** Alternate text for "auto" caption. */
	indeterminateText?: string;
}, "div">) {
	return (
		<Segmented current={current}>
			{trueFalseAuto.map(option => (
				<Segmented.Item id={option} key={option} icon={icons[option]}>
					{indeterminateText && option === "auto" ? indeterminateText : t[onOffAuto[option]]}
				</Segmented.Item>
			))}
		</Segmented>
	);
}
