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

	return (
		<Expander
			type="button"
			disabled={disabled}
			trailingIcon={trailingIcon}
			className={[className, "settings-card-toggle-switch"]}
			onClick={onClick}
			hideExpandIfNoChildren
			actions={(
				<>
					<ToggleSwitch
						as="label"
						$color={$color}
						on={[on, setOn]}
						isPressing={[isToggleSwitchPressing, setIsToggleSwitchPressing]}
						tabIndex={-1}
						disabled={disabled}
						resetTransitionOnChanging={resetTransitionOnChanging}
					/>
					{actions}
				</>
			)}
			{...settingsCardProps}
		>
			{children}
		</Expander>
	);
}
