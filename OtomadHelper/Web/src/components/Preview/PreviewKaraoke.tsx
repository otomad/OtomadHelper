const StyledPreviewKaraoke = styled.div`
	${styles.mixins.gridCenter()};
	position: absolute;
	inset: 0;
	transition: ${fallbackTransitions}, clip-path linear 1s;
	forced-color-adjust: none;

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
		clip-path: inset(0);

		> p {
			color: white;
			-webkit-text-stroke-color: black;
		}
	}

	&.past {
		clip-path: inset(0 100% 0 0);

		&:dir(rtl) {
			clip-path: inset(0 0 0 100%);
		}

		> p {
			color: lch(from ${c("colorization")} 50 c h);
			-webkit-text-stroke-color: white;
		}
	}

	&.custom > p {
		color: var(--color);
		-webkit-text-stroke-color: --contrast-color(var(--color));
	}

	main.page.enter-done &:not(.reset) {
		&.future,
		&.past:dir(rtl) {
			clip-path: inset(0 0 0 50%);
		}

		&.past,
		&.future:dir(rtl) {
			clip-path: inset(0 50% 0 0);
		}
	}

	&.demo-mode {
		&.future,
		&.past:dir(rtl) {
			clip-path: none !important;
		}

		&.past,
		&.future:dir(rtl) {
			animation: ${keyframes`
				0% {
					clip-path: inset(0 100% 0 0);
					opacity: 1;
				}

				97% {
					clip-path: inset(0);
					opacity: 1;
				}

				100% {
					clip-path: inset(0);
					opacity: 0;
					animation-timing-function: ${eases.easeInOutMax};
				}
			`} 2s linear infinite;
		}
	}
`;

export default function PreviewKaraoke({ reset, futureFill, pastFill, demoMode }: FCP<{
	/** Reset the karaoke lyrics progress? */
	reset?: boolean;
	/** Future text fill color. */
	futureFill?: string;
	/** Past text fill color. */
	pastFill?: string;
	/** Demo mode, run the animation infinitely? */
	demoMode?: boolean;
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
		<StyledPreviewKaraoke
			key={tense}
			className={[tense, {
				reset,
				custom: tense === "future" && futureFill || tense === "past" && pastFill,
				demoMode,
			}]}
		>
			<p
				ref={setTextEl(index)}
				style={{ "--color": tense === "future" ? futureFill : pastFill }}
			>
				{t.lyrics.sampleLyrics}
			</p>
		</StyledPreviewKaraoke>
	));
}
