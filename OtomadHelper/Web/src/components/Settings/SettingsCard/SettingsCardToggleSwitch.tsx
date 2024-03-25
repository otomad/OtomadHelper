export default function SettingsCardToggleSwitch({ on: [on, setOn], disabled, children, trailingIcon, resetTransitionOnChanging, onClick, ...settingsCardProps }: FCP<PropsOf<typeof SettingsCard> & {
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
	/** 在切换开关时重设页面的过渡效果。 */
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
