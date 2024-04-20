export default function SettingsCardToggleSwitch({ on: [on, setOn], disabled, children, trailingIcon, resetTransitionOnChanging, onClick, ...settingsCardProps }: FCP<PropsOf<typeof SettingsCard> & {
	/** Is on? */
	on: StateProperty<boolean>;
	/** Disabled? */
	disabled?: boolean;
	/** Reset the page's transition effect when toggling the switch. */
	resetTransitionOnChanging?: boolean;
}>) {
	const [isToggleSwitchPressing, setIsToggleSwitchPressing] = useState(false);
	trailingIcon ||= "";
	onClick ??= () => !isToggleSwitchPressing && (setOn as SetStateNarrow<boolean>)?.(on => !on);

	return (
		<SettingsCard
			type="button"
			disabled={disabled}
			trailingIcon={trailingIcon}
			onClick={onClick}
			{...settingsCardProps}
		>
			<ToggleSwitch
				as="label"
				on={[on, setOn]}
				isPressing={[isToggleSwitchPressing, setIsToggleSwitchPressing]}
				tabIndex={-1}
				resetTransitionOnChanging={resetTransitionOnChanging}
			/>
			{children}
		</SettingsCard>
	);
}
