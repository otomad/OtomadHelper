export default function Disabled({ children, disabled = true, as = Fragment, wrapper, ...htmlAttrs }: FCP<{
	/** Disable all children controls? */
	disabled?: boolean;
	/** Modify the wrapper type. Defaults to `React.Fragment` (a.k.a. nothing). */
	as?: AsTarget;
	/** Same as `as`, but compatible with Styled Components. */
	wrapper?: AsTarget;
}>) {
	const d = disabled || undefined;
	const Wrapper = wrapper ?? as;
	if (Wrapper === Fragment) htmlAttrs = {};
	return (
		<Wrapper {...htmlAttrs}>
			{React.Children.map(children, child =>
				React.cloneElement(child as ReactElement, {
					disabled: d,
					"aria-disabled": d,
				}),
			)}
		</Wrapper>
	);
}
