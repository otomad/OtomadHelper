const StyledIconTile = styled.div`
	${styles.mixins.square("100%")};
	${styles.mixins.gridCenter()};
	--accent: transparent;
	--gradient-rotate: 172.87deg;
	background:
		radial-gradient(79.17% 79.17% at 31.94% 26.04%, ${c("background-color", 20)} 17.71%, ${c("fill-color-control-icon-tile-secondary")} 100%),
		linear-gradient(var(--gradient-rotate), transparent 3.09%, ${c("accent-color", 16.806723)} 94.44%); // 235
	border: 1px solid ${c("stroke-color-surface-stroke-default")};
	border-radius: inherit;

	.items-view-item.selected & {
		--gradient-rotate: -11.5deg;
	}
`;

type DeclaredColoredIcons = {
	[Name in DeclaredIcons]: Name extends `colored/${infer SubName}` ? SubName : never;
}[DeclaredIcons];

// Use it inside the image field of ItemsView.Item in "grid" view only.
export function IconTile({ name, size }: FCP<{
	/** Icon. Must be a colored icon. */
	name: DeclaredColoredIcons;
	/** The icon size in px. */
	size?: number;
	children?: never;
}, "div">) {
	const iconName = `colored/${name}` as const;
	// const accent = useMemo(() => datasets(name).accent, [name]);

	return (
		<StyledIconTile role="img" className="no-border">
			<Icon name={iconName} size={size} filled />
		</StyledIconTile>
	);
}
