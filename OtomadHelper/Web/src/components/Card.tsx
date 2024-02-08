export /* internal */ const StyledCard = styled.div`
	border-radius: 3px;
	width: -webkit-fill-available;
	text-align: initial;
	border: 1px solid ${c("stroke-color-card-stroke-default")};

	> .base {
		background-color: ${c("background-fill-color-card-background-default")};
		padding: 13px 15px;
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
