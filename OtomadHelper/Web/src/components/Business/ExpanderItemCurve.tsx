const curves = ["linear", "fast", "slow", "smooth", "sharp", "hold"] as const;

export /* @internal */ default function ExpanderItemCurve({ curve }: FCP<{
	curve: StateProperty<CurveType>;
	children?: never;
}, "div">) {
	return (
		<Expander.Item title={t.curve} details={t.descriptions.curve} icon="curve">
			<ComboBox ids={curves} options={curves.map(curve => t.curve[curve])} icons={curves.map(curve => `curves/${curve}` as const)} current={curve} />
		</Expander.Item>
	);
}
