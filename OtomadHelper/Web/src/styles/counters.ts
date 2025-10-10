export default css`
	@counter-style tuning-classic-mode-list-item {
		system: extends decimal-leading-zero;
		prefix: "A";
		pad: 2 "0";
	}

	@counter-style lower-latin-right-paren {
		system: extends lower-latin;
		suffix: ") ";
	}
`;
