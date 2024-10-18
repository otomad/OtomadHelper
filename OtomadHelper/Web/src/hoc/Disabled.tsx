export default function Disabled({ children, disabled = true as boolean | undefined, as = Fragment, container, ...htmlAttrs }: FCP<{
	/** Disable all children controls? */
	disabled?: boolean;
	/** Modify the container type. Defaults to `React.Fragment` (a.k.a. nothing). */
	as?: AsTarget;
	/** Same as `as`, but compatible with Styled Components. */
	container?: AsTarget;
}>) {
	disabled ||= undefined;
	const Container = container ?? as;
	if (Container === Fragment) htmlAttrs = {};
	return (
		<Container {...htmlAttrs}>
			{React.Children.map(children, child =>
				React.cloneElement(child as ReactElement, {
					disabled,
					"aria-disabled": disabled,
				}),
			)}
		</Container>
	);
}
