const StyledVerticalIfFlexWrap = styled.div`
	display: flex;
	flex-flow: row wrap;

	&.has-child-wrapped {
		flex-direction: column;
	}
`;

export default function VerticalIfFlexWrap({ children, ...htmlAttrs }: FCP<object, "div">) {
	const el = useDomRef<"div">();
	useDetectWrappedElements(el);

	return (
		<StyledVerticalIfFlexWrap ref={el} {...htmlAttrs}>
			{children}
		</StyledVerticalIfFlexWrap>
	);
}
