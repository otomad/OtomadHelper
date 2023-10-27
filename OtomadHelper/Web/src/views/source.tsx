const flip = keyframes`
	0%,
	100% {
		scale: 1;
	}

	50% {
		scale: -1 1;
	}
`;

const StyledImage = styled.img`
	max-width: 100%;
	width: 400px;
	animation: ${flip} 500ms step-start infinite;

	&:active {
		filter: invert(1);
	}
`;

const thumbnail = "https://app/thumbnail/D:/Downloads/test.mp4";

export default function Source() {
	return (
		<StyledImage src={thumbnail} />
	);
}
