export /* internal */ default function SegmentedItem({ icon, children, className, onClick }: FCP<{
	/** Icon, optional. */
	icon?: DeclaredIcons;
	/** Identifier. */
	id: string;
}, "div">) {
	return (
		<div className={["item", className]} onClick={onClick}>
			<div className="base">
				{icon && <Icon name={icon} />}
				{children && <p>{children}</p>}
			</div>
		</div>
	);
}
