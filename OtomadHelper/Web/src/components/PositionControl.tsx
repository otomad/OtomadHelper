const positions = [
	"top left", "top", "top right",
	"left", "center", "right",
	"bottom left", "bottom", "bottom right",
] satisfies Position[] as Position[];

const boxDrawings = [
	"┌", "─", "┐",
	"│", "", "│",
	"└", "─", "┘",
];

const StyledPositionControl = styled.div`
	display: grid;
	grid-template-columns: repeat(3, auto);
	gap: 4px;

	button {
		min-block-size: unset;
		min-inline-size: unset;
		${styles.mixins.square("32px")};
	}
`;

export default function PositionControl({ value: [value, setValue], disabled }: FCP<{
	/** Position. */
	value: StateProperty<Position>;
	/** Disabled? */
	disabled?: boolean;
	children?: never;
}>) {
	return (
		<StyledPositionControl>
			{positions.map((position, i) => (
				<ToggleButton
					key={position}
					appearance="obvious"
					checked={[value === position]}
					disabled={disabled}
					onClick={() => setValue?.(position)}
				>
					{boxDrawings[i]}
				</ToggleButton>
			))}
		</StyledPositionControl>
	);
}
