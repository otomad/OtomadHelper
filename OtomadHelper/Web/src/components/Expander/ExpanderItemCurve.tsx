const curves = ["linear", "fast", "slow", "smooth", "sharp", "none"] as const; // TODO: i18n

export /* @internal */ default function ExpanderItemCurve({ curve }: FCP<{
	curve: StateProperty<CurveType>;
	children?: never;
}, "div">) {
	return (
		<Expander.Item title={t.curve} details={t.descriptions.curve} icon="curve">
			<ComboBox ids={curves} options={curves} current={curve} />
		</Expander.Item>
	);
}
