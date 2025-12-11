export /* @internal */ default function SegmentedEnum<T extends AnyEnum>({ items, current, disabled }: Override<PropsOf<typeof Segmented<T["keyType"]>>, {
	/** An enum-plus collection. */
	items: T;
	children?: never;
}>) {
	return (
		<Segmented current={current} disabled={disabled}>
			{items.map(({ key, label, icon }) =>
				<Segmented.Item key={key} id={key} icon={icon}>{label}</Segmented.Item>)}
		</Segmented>
	);
}
