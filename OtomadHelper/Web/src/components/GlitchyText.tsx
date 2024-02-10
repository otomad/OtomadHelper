const StyledGlitchyText = styled.div`
	// position: relative;
	width: fit-content;
	display: inline-block;

	.shadow {
		visibility: hidden;
	}

	.visible {
		position: absolute;
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
			<p className="visible">{isInstructionHovered ? normal : glitchy}</p>
			<p className="shadow">{glitchy}</p>
		</StyledGlitchyText>
	);
}
