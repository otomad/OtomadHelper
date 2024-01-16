export default function SettingsCardToggleSwitch({ on, disabled, children, trailingIcon, onClick, ...settingsCardProps }: FCP<PropsOf<typeof SettingsCard> & {
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
}>) {
	trailingIcon ||= "";
	onClick ??= () => (on[1] as SetStateNarrow<boolean>)?.(on => !on);

	return (
		<SettingsCard type="button" {...settingsCardProps} trailingIcon={trailingIcon} onClick={onClick}>
			<ToggleSwitch on={on} disabled={disabled} />
			{children}
		</SettingsCard>
	);
}
