/**
 * Higher-order component that wraps `React.Activity` with visibility control.
 * @remarks I don't know why React official use a `mode` prop and users should pass a string to control it.
 * @param props - The component props.
 * @param props.visible - Determines whether the activity indicator is visible. Defaults to `true`.
 * @param activityProps - Additional props to pass through to the `React.Activity` component.
 * @returns A `React.Activity` component with the specified visibility mode.
 */
export default function Activity({ visible = true, ...activityProps }: React.ActivityProps & {
	/** Determines whether the activity indicator is visible. @default true */
	visible?: boolean;
}) {
	return <React.Activity mode={visible ? "visible" : "hidden"} {...activityProps} />;
}
