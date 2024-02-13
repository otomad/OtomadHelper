const StyledGlitchyText = styled.div`
	position: relative;
	width: fit-content;
	display: inline-block;

	* {
		transition: none;
	}

	.shadow {
		visibility: visible;
	}

	.normal {
		position: absolute;
		visibility: hidden;
	}

	&:hover {
		.shadow {
			visibility: hidden;
		}

		.normal {
			visibility: visible;
		}
	}
`;

export default function GlitchyText({ normal, glitchy }: FCP<{
	/** 正常文本。 */
	normal: string;
	/** 故障文本。 */
	glitchy: string;
}>) {
	const [isInstructionHovered, setIsInstructionHovered] = useState(false);

	return (
		<StyledGlitchyText
			onMouseEnter={() => setIsInstructionHovered(true)}
			onMouseLeave={() => setIsInstructionHovered(false)}
		>
			<p className="normal">{normal}</p>
			<p className="shadow">{glitchy}</p>
		</StyledGlitchyText>
	);
}
