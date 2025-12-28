/**
 * Add attributes or properties to all children.
 *
 * @example
 * ```jsx
 * <Attrs disabled>
 *     <button />
 *     <input />
 * </Attrs>
 *
 * // Equivalent to
 *
 * <button disabled />
 * <input disabled />
 * ```
 *
 * @returns Attrs.
 */
export default function Attrs({ compactUndefined = true, children, ...attrs }: FCP<{
	/** If true, omit properties with undefined values; If false, properties with undefined values will also be passed to child components. @default true */
	compactUndefined?: boolean;
}, "section"> & Record<string, Any>) {
	if (compactUndefined) Object.compactUndefined(attrs);
	if ("disabled" in attrs && !("aria-disabled" in attrs)) // Auto add `aria-disabled` when `disabled`.
		attrs["aria-disabled"] = attrs.disabled;
	return (
		<>
			{React.Children.map(children, child => {
				if (!isValidElement(child)) return child;
				return React.cloneElement(child, attrs);
			})}
		</>
	);
}
