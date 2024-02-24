const StyledGlitchyText = styled.div`
	position: relative;
	display: inline-block;
	width: fit-content;

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
	return (
		<StyledGlitchyText>
			<p className="normal">{normal}</p>
			<p className="shadow">{glitchy}</p>
		</StyledGlitchyText>
	);
}
