/**
 * Screen reader-only.
 *
 * Use `sr-only` (`SrOnly`) to hide an element visually without hiding it from screen readers.
 */
const SrOnly = styled.p`
	position: fixed;
	width: 1px;
	height: 1px;
	margin: -1px;
	padding: 0;
	overflow: hidden;
	white-space: nowrap;
	border: 0;
	clip-path: inset(50%);
`;

export default SrOnly;
