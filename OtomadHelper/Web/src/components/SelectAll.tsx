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

	.expander-child-items & .checkbox-label {
		padding-inline: 52px 40px;
	}
`;

export default function SelectAll<T>({ value, all, icon, title, meta, details }: FCP<{
	value: StateProperty<T[]>;
	all: T[];
	/** Icon. Use an empty string or Boolean type to indicate disabling. */
	icon?: DeclaredIcons;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Inherit from a setting meta. */
	meta?: SettingMetaInside;
}, "div">) {
	const selectAll = useSelectAll(value, all);
	// eslint-disable-next-line no-var
	var { title, details, icon, anchor } = Setting.useMeta(meta, arguments);
	const props = { title, details, icon, anchor };

	return (
		<StyledSelectAll>
			{title && <Expander.Item {...props} asSubtitle />}
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
