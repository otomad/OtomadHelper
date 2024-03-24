export /* internal */ const StyledCard = styled.div`
	width: 100cqw; // BUG: 100% 会造成横向 TAB 那里超出宽度；100cqw 会造成 sticky 后的 backdrop-filter blur 渲染异常。
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
