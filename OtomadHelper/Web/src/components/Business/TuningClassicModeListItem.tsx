const StyledTuningClassicModeListItem = styled.li`
	margin-inline-start: 5.25ex;
	font-variant-numeric: tabular-nums;
	list-style-type: tuning-classic-mode-list-item;
`;

export default function TuningClassicModeListItem({ id }: {
	/** The ID of the classic stretch attributes. */
	id: string;
}) {
	return <StyledTuningClassicModeListItem value={+id.slice(1)}>{t.stream.tuning.stretchAttributes.classic[id]}</StyledTuningClassicModeListItem>;
}
