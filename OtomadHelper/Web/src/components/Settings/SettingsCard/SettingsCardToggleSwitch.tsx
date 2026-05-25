export default function SettingsCardToggleSwitch({ on: _on, disabled: _disabled, children, actionIcon, resetTransitionOnChanging, className, color, actions, lock, actuallyOn, title, expanded, onClick, onChange, ...settingsCardProps }: FCP<Override<PropsOf<typeof SettingsCard>, {
	/** Is on? */
	on: VariousState<boolean>;
	/** Disabled? */
	disabled?: VariousStateWithSelf<boolean>;
	/**
	 * Reset the page's transition effect when toggling the switch.
	 * @remarks This is business logic, but present in the base component.
	 */
	resetTransitionOnChanging?: boolean;
	/** Use special accent color for the toggle switch. */
	color?: string;
	/** The other action control area on the right side of the component. */
	actions?: ReactNode;
	/**
	 * Sets the displayed value of the toggle switch and disables it.
	 *
	 * This only changes its appearance, not its internal data.
	 *
	 * Useful when you need to disable user input without affecting configuration saving.
	 */
	lock?: boolean | null;
	/**
	 * If when the toggle switch is on but useless, please pass this prop with `false`, and the label will show "On (Actually Off)".
	 *
	 * If you do not need this feature, please pass `undefined` for better performance.
	 *
	 * Note that this prop either toggles between `true` and `false` or is always `undefined`, never toggles between `true` and `undefined`.
	 *
	 * @default undefined
	 */
	actuallyOn?: boolean;
	/** Expanded initially if it has children and it is on? */
	expanded?: boolean;
	/** Occurs while toggling. */
	onChange?(on: boolean): void;
}>>) {
	actionIcon ||= "";
	const [on, setOn] = useVariousState(_on);
	onClick ??= () => setOn(on => { onChange?.(!on); return !on; });
	const isExpander = shouldBeExpander(children);
	const disabled = useReadonlyVariousState(_disabled);

	return (
		<SettingsCardOrExpander
			type="button"
			disabled={disabled || lock != null}
			actionIcon={actionIcon}
			className={[className, nameof.kebab(SettingsCardToggleSwitch), { toggleSwitchHoverable: !isExpander || !on }]}
			actions={(
				<>
					<ToggleSwitch
						as={isExpander ? undefined : "label"}
						color={color}
						on={[on, setOn]}
						lock={lock}
						actuallyOn={actuallyOn}
						tabIndex={isExpander ? undefined : -1}
						disabled={disabled}
						resetTransitionOnChanging={resetTransitionOnChanging}
						aria-label={applyAriaLabel(title)}
						aria-hidden={!(isExpander && on) || undefined}
						onChange={onChange}
					/>
					{actions}
				</>
			)}
			childrenDisabled={isExpander ? !on : undefined}
			title={title}
			aria-label={applyAriaLabel(title)}
			role={isExpander && on ? undefined : "switch"}
			aria-checked={on}
			wrapActionsWhenNarrow={!actions ? false : undefined} // For better performance if there is no other custom actions.
			expanded={expanded}
			onClick={onClick}
			onClickWhenChildrenDisabled={isExpander && setOn ? () => { setOn(true); onChange?.(true); } : undefined}
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
