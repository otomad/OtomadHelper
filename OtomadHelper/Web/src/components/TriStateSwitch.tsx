const triStates = [false, null, true] as const;
const labels = { false: "off", true: "on" } as const;
const icons = { false: "dismiss", true: "checkmark" } as const;
const triStateMap = { false: false, null: null, true: true } as const;
type TriStateKey = keyof typeof triStateMap;

const toTriStateKey = (triState: TriState) => String(triState) as TriStateKey;

export default function TriStateSwitch({ current, indetText, indetIcon }: FCP<{
	/** The identifier of the selected segmented item. */
	current: StateProperty<TriState>;
	/** Text for indeterminate option. */
	indetText: string;
	/** Icon for indeterminate option. */
	indetIcon: DeclaredIcons;
}, "div">) {
	const segmentedCurrent = useStateSelector(current, toTriStateKey, key => triStateMap[key]);
	return (
		<Segmented current={segmentedCurrent}>
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
