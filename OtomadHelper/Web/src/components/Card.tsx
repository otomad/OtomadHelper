export /* internal */ const StyledCard = styled.div`
	width: 100cqw; // WARN: 设置页浅色模式下由于 SVG LOGO 有 filter 会导致展开器的 backdrop-filter 显示异常，是 Chromium 的问题。
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
