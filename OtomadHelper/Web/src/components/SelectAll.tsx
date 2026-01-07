export /* @internal */ const SELECT_ALL_PADDING_INLINE = [52, 40] as const;

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
		padding-inline: ${SELECT_ALL_PADDING_INLINE[0]}px ${SELECT_ALL_PADDING_INLINE[1]}px;
	}
`;

export default function SelectAll<T>({ value, all, icon, title, meta, details, ...htmlAttrs }: FCP<{
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
	const { 0: selectAll, 1: setSelectAll, 2: invertSelection } = useSelectAll(value, all);
	// eslint-disable-next-line no-var
	var { title, details, icon, anchor } = Setting.useMeta(meta, arguments);
	const props = { title, details, icon, anchor };

	return (
		<StyledSelectAll {...htmlAttrs}>
			{title && <Expander.Item {...props} asSubtitle="closerAfter" />}
			<Checkbox
				value={[selectAll, setSelectAll]}
				dynamicFontWeight={[value[0]?.length ?? 0, all.length]}
				actions={
					<Button subtle icon="invert_selection" onClick={e => { stopEvent(e); invertSelection(); }}>{t.invertSelection}</Button>
				}
			>
				{t.selectAll}
			</Checkbox>
		</StyledSelectAll>
	);
}
