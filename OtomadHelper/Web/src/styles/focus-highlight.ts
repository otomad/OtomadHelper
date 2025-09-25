// Focus testing

const focusHighlight = keyframes`
	from {
		box-shadow: 0 0 8px 6px ${c("accent-color", 60)};
	}

	to {
		box-shadow: none;
	}
`;

const animationStyle = css`animation: ${focusHighlight} 1s linear 0s 5 alternate none running;`;

export default animationStyle;
