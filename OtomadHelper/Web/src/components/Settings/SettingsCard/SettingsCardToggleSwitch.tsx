export default function SettingsCardToggleSwitch({ on: [on, setOn], disabled, children, trailingIcon, onClick, ...settingsCardProps }: FCP<PropsOf<typeof SettingsCard> & {
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
}>) {
	const [isToggleSwitchPressing, setIsToggleSwitchPressing] = useState(false);
	trailingIcon ||= "";
	onClick ??= () => !isToggleSwitchPressing && (setOn as SetStateNarrow<boolean>)?.(on => !on);

	return (
		<SettingsCard type="button" {...settingsCardProps} trailingIcon={trailingIcon} onClick={onClick}>
			<ToggleSwitch as="label" on={[on, setOn]} disabled={disabled} isPressing={[isToggleSwitchPressing, setIsToggleSwitchPressing]} tabIndex={-1} />
			{children}
		</SettingsCard>
	);
}
