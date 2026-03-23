const triStates = [false, null, true] as const;
const labels = { false: "off", true: "on" } as const;
const icons = { false: "dismiss", true: "checkmark" } as const;
const triStateMap = { false: false, null: null, true: true } as const;
type TriStateKey = keyof typeof triStateMap;

const toTriStateKey = (triState: TriState) => String(triState) as TriStateKey;

export default function TriStateSwitch({ current, indetText, indetIcon, className }: FCP<{
	/** The identifier of the selected segmented item. */
	current: StateProperty<TriState>;
	/** Text for indeterminate option. */
	indetText: string;
	/** Icon for indeterminate option. */
	indetIcon: DeclaredIcons;
	className?: ClassValue;
}, "div">) {
	const segmentedCurrent = useStateSelector(current, toTriStateKey, key => triStateMap[key]);

	return (
		<Segmented current={segmentedCurrent} className={[nameof.kebab(TriStateSwitch), className]}>
			{triStates.map(option => {
				const key = toTriStateKey(option);
				return (
					<Segmented.Item
						id={key}
						key={key}
						icon={key === "null" ? indetIcon : icons[key]}
					>
						{key === "null" ? indetText : t[labels[key]]}
					</Segmented.Item>
				);
			})}
		</Segmented>
	);
}

// Business component
export function OffAndAutoSwitch({ current }: {
	/** Current selected state. True stands for auto, and false stands for off. */
	current: StateProperty<boolean>;
}) {
	const segmentedCurrent = useStateSelector(current, toTriStateKey, key => triStateMap[key]);

	return (
		<Segmented current={segmentedCurrent}>
			<Segmented.Item id="false" key="false" icon={icons.false}>{t.off}</Segmented.Item>
			<Segmented.Item id="true" key="true" icon="auto">{t.auto}</Segmented.Item>
		</Segmented>
	);
}

export function DualStateSwitch({ current, falseText, trueText, falseIcon, trueIcon, className }: {
	/** Current selected state. True stands for auto, and false stands for off. */
	current: StateProperty<boolean>;
	falseText: string;
	trueText: string;
	falseIcon?: DeclaredIcons;
	trueIcon?: DeclaredIcons;
	className?: ClassValue;
}) {
	const segmentedCurrent = useStateSelector(current, toTriStateKey, key => triStateMap[key]);

	return (
		<Segmented current={segmentedCurrent} className={[nameof.kebab(DualStateSwitch), className]}>
			<Segmented.Item id="false" key="false" icon={falseIcon}>{falseText}</Segmented.Item>
			<Segmented.Item id="true" key="true" icon={trueIcon}>{trueText}</Segmented.Item>
		</Segmented>
	);
}
