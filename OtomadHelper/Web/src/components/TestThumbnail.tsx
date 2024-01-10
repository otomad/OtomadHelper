const thumbnail = "https://app/thumbnail/D:/Downloads/test.mp4";

const flip = keyframes`
	0% {
		scale: 1;
	}

	50% {
		scale: -1 1;
	}
`;

const StyledTestThumbnail = styled.img`
	width: 100%;
	max-width: 500px;
	animation: ${flip} 1s step-end infinite !important;

	&:active {
		filter: invert(1);
	}
`;

export default function TestThumbnail() {
	return <StyledTestThumbnail src={thumbnail} />;
}
