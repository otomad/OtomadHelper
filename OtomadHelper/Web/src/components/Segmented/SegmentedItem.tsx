export /* @internal */ default function SegmentedItem({ icon, selected, children, className, onClick }: FCP<{
	/** Icon, optional. */
	icon?: DeclaredIcons;
	/** Identifier. */
	id: string;
	/** Selected? */
	selected?: boolean;
}, "div">) {
	return (
		<div
			role="radio"
			aria-checked={selected}
			className={["item", { selected }, className]}
			onClick={onClick}
		>
			<div className="base">
				{icon && <Icon name={icon} />}
				{children && <p>{children}</p>}
			</div>
		</div>
	);
}
