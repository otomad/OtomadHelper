export /* internal */ const StyledCard = styled.div`
	width: 100cqw;
	text-align: initial;
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-radius: 3px;
	transition: ${fallbackTransitions}, width 0s;

	> .base {
		padding: 13px 15px;
		background-color: ${c("background-fill-color-card-background-default")};
		border-radius: 2px;
	}
`;

export default function Card({ children, ...htmlAttrs }: FCP<{}, "div">) {
	return (
		<StyledCard {...htmlAttrs}>
			<div className="base">
				{children}
			</div>
		</StyledCard>
	);
}
