export default function SettingsCardToggleSwitch({ on: [on, setOn], disabled, children, trailingIcon, resetTransitionOnChanging, className, $color, actions, onClick, ...settingsCardProps }: FCP<PropsOf<typeof SettingsCard> & {
	/** Is on? */
	on: StateProperty<boolean>;
	/** Disabled? */
	disabled?: boolean;
	/**
	 * Reset the page's transition effect when toggling the switch.
	 * @remarks This is business logic, but present in the base component.
	 */
	resetTransitionOnChanging?: boolean;
	/** Use special accent color for the toggle switch. */
	$color?: string;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
}>) {
	const [isToggleSwitchPressing, setIsToggleSwitchPressing] = useState(false);
	trailingIcon ||= "";
	onClick ??= () => !isToggleSwitchPressing && (setOn as SetStateNarrow<boolean>)?.(on => !on);
	const isExpander = shouldBeExpander(children);

	return (
		<SettingsCardOrExpander
			type="button"
			disabled={disabled}
			trailingIcon={trailingIcon}
			className={[className, "settings-card-toggle-switch"]}
			onClick={onClick}
			actions={(
				<>
					<ToggleSwitch
						as={isExpander ? undefined : "label"}
						$color={$color}
						on={[on, setOn]}
						isPressing={[isToggleSwitchPressing, setIsToggleSwitchPressing]}
						tabIndex={isExpander ? undefined : -1}
						disabled={disabled}
						resetTransitionOnChanging={resetTransitionOnChanging}
					/>
					{actions}
				</>
			)}
			childrenDisabled={isExpander ? !on : undefined}
			{...settingsCardProps}
		>
			{children}
		</SettingsCardOrExpander>
	);
}

function shouldBeExpander(children?: ReactNode) { return !([undefined, null, NaN, ""] as ReactNode[]).includes(children); }

function SettingsCardOrExpander({ children, actions, ...htmlAttrs }: {
	children?: ReactNode;
	actions?: ReactNode;
	[x: string]: Any;
}) {
	const isExpander = shouldBeExpander(children);
	const Container = isExpander ? Expander : SettingsCard;

	return (
		<Container {...htmlAttrs} actions={isExpander ? actions : undefined}>
			{isExpander ? children : actions}
		</Container>
	);
}
