const StyledCommandBarGroup = styled(StackPanel)`
	justify-content: space-between;

	&:has(> .left):not(:has(> .right), :has(> .center)) {
		justify-content: start;
	}

	&:has(> .right):not(:has(> .left), :has(> .center)) {
		justify-content: end;
	}

	&:has(> .center):not(:has(> .left), :has(> .right)) {
		justify-content: center;
	}
`;

export /* @internal */ default function CommandBarGroup({ children }: FCP) {
	return (
		<StyledCommandBarGroup $sticky>
			{children}
		</StyledCommandBarGroup>
	);
}
