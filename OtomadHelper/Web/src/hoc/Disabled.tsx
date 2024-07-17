export default function Disabled({ children, disabled = true }: FCP<{
	/** Disabled all children controls? */
	disabled?: boolean;
}>) {
	const d = disabled || undefined;
	return React.Children.map(children, child =>
		React.cloneElement(child as ReactElement, {
			disabled: d,
			"aria-disabled": d,
		}),
	);
}
