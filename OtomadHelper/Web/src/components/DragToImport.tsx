const StyledDragToImport = styled.div`
	${styles.mixins.fullscreen()};
	z-index: 80;
	padding: 1rem;
	background-color: ${c("background-fill-color-smoke-default")};
	backdrop-filter: blur(4px);

	* {
		pointer-events: none;
	}

	.box {
		${styles.mixins.flexCenter()};
		${styles.mixins.square("100%")};
		overflow: clip;
		border: 2px dashed ${c("fill-color-text-secondary")};
		border-radius: 5px;

		.empty-message {
			h2 {
				${styles.effects.text.titleLarge};
			}

			.icon {
				font-size: 72px;
			}
		}
	}

	${tgs()} {
		scale: 1.05;
		opacity: 0;
		backdrop-filter: none;

		.empty-message {
			scale: 1.1;
		}
	}

	.empty-message {
		.icon,
		.letter-by-letter span {
			animation: ${keyframes`
				from {
					opacity: 0;
					translate: 25px;
					scale: 1.15;
					filter: blur(2px);
				}
			`} 500ms ${eases.easeOutBackSmooth} calc(var(--i, 0) * (50ms / var(--length, 7) * 7)) backwards;
		}
	}

	&.exit .empty-message {
		--character-unit: 1ex;

		&:is(:lang(zh), :lang(ko)) {
			--character-unit: 1em;
		}

		.icon,
		.letter-by-letter span {
			transform-origin: 50% 25%;
			animation: ${keyframes`
				to {
					scale: 0.25;
					translate: calc(var(--i, 0) * -1 * var(--character-unit));
					filter: blur(5px);
				}
			`} 250ms ${eases.easeOutMax} calc(var(--i, 0) * (15ms / var(--length, 7) * 7)) forwards;
		}

		.icon {
			transform-origin: left center;
			animation-duration: 750ms;
		}
	}

	body:has(&:not(.exit)) #root {
		> .navigation-view {
			scale: 0.94;
		}

		> .background-image {
			scale: 1.08;
		}
	}
`;

export default function DragToImport({ children }: FCP<{
	children: string;
}>) {
	const [shown, setShown] = useState(true);

	useListen("host:dragOver", e => {
		setShown(e.isDragging);
	});

	/* useInterval(() => {
		setShown(shown => !shown);
	}, 1000); */

	const title = t.dragToImport({ item: children });

	return (
		<Portal>
			<CssTransition in={shown} unmountOnExit>
				<StyledDragToImport onMouseDown={() => setShown(false)} onMouseUp={() => setShown(false)}>
					<div className="box">
						<EmptyMessage icon="touch_pointer" title={<LetterByLetter>{title}</LetterByLetter>} noSideEffect />
					</div>
				</StyledDragToImport>
			</CssTransition>
		</Portal>
	);
}
