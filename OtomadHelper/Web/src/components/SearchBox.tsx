const StyledSearchBox = styled.search`
	color: red;
`;

export default function SearchBox({ children, ...htmlAttrs }: FCP<{

}, "div">) {
	return (
		<StyledSearchBox className="abc">
			123
		</StyledSearchBox>
	);
}
