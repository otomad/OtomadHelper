const StyledSelectAll = styled.div`
	&:not(.expander-child-items *) {
		padding: 7px 12px;

		.checkbox-label {
			display: flex;
			gap: 15px;
			align-items: center;

			.text {
				flex: 1;
				width: 100%;
			}
		}
	}
`;

export default function SelectAll<T>({ value, all, icon, title, meta, details }: FCP<{
	value: StateProperty<T[]>;
	all: T[];
	/** Icon. Use an empty string or Boolean type to indicate disabling. */
	icon?: DeclaredIcons | ReactElement;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Inherit from a setting meta. */
	meta?: SettingMetaInside;
}, "div">) {
	const selectAll = useSelectAll(value, all);
	const props = Setting.useMeta(meta, { title, details, icon });

	return (
		<StyledSelectAll>
			{props.title && <Expander.Item {...props} asSubtitle />}
			<Checkbox
				value={selectAll}
				actions={
					<Button subtle icon="invert_selection" onClick={e => { stopEvent(e); selectAll[2](); }}>{t.invertSelection}</Button>
				}
			>
				{t.selectAll}
			</Checkbox>
		</StyledSelectAll>
	);
}
