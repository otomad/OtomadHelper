/**
 * Higher-order component that curries props into a component.
 *
 * @template TComponent - The component type to curry props into.
 * @template TProps - The partial props object type to curry.
 * @param component - The component to wrap with curried props.
 * @param withProps - The props to curry into the component.
 * @returns A new component that accepts remaining props and merges them with the curried props.
 *
 * @example
 * ```jsx
 * const ButtonWithBlueColor = Curry(Button, { color: "blue" });
 * // Usage
 * <ButtonWithBlueColor onClick={handleClick} />
 * ```
 */
export default function Curry<TComponent extends React.JSX.ElementType, TProps extends Partial<PropsOf<TComponent>>>(component: TComponent, withProps: TProps) {
	const Component = component as Any;
	const CurriedComponent = (props: PartialWith<PropsOf<TComponent>, keyof TProps>) => <Component {...withProps} {...props} />;
	return CurriedComponent;
}
