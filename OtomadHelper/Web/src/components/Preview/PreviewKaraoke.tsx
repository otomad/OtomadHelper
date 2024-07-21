const StyledPreviewKaraoke = styled.div`
	${styles.mixins.gridCenter()};
	position: absolute;
	inset: 0;
	transition: ${fallbackTransitions}, clip-path linear 1s;

	&.reset {
		transition: ${fallbackTransitions}, clip-path linear 250ms;
	}

	> p {
		font-family: "Open Huninn", ui-rounded;
		font-weight: bold;
		transition: ${fallbackTransitions}, scale 0s;
		paint-order: stroke fill;
		-webkit-text-stroke-width: 1px;
	}

	&.future {
		clip-path: inset(0 0 0 0);

		> p {
			color: white;
			-webkit-text-stroke-color: black;
		}
	}

	&.past {
		clip-path: inset(0 100% 0 0);

		> p {
			color: lch(from ${c("accent-color")} 50 100 h);
			-webkit-text-stroke-color: white;
		}
	}

	main.page.enter-done &:not(.reset) {
		&.future {
			clip-path: inset(0 0 0 50%);
		}

		&.past {
			clip-path: inset(0 50% 0 0);
		}
	}
`;

export default function PreviewKaraoke({ reset }: FCP<{
	/** Reset the karaoke lyrics progress? */
	reset?: boolean;
}>) {
	const [textEls, setTextEl] = useDomRefs<"p">();

	useEffect(() => {
		const CONTAINER_PADDING = 20;
		for (const text of textEls.current)
			if (text) {
				text.style.scale = String(1);
				text.style.scale = String((text.parentElement!.clientWidth - CONTAINER_PADDING * 2) / text.clientWidth);
			}
	});

	return (["future", "past"] as const).map((tense, index) => (
		<StyledPreviewKaraoke key={tense} className={[tense, { reset }]}>
			<p ref={setTextEl(index)}>{t.lyrics.sampleLyrics}</p>
		</StyledPreviewKaraoke>
	));
}
