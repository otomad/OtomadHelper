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
			`} 500ms ${eases.easeOutBackSmooth} calc(--sibling-index-0() * (50ms / var(--length, 7) * 7)) backwards;
			will-change: opacity, transform, filter;
		}
	}

	&.exit.drop .empty-message {
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
					translate: calc(--sibling-index-0() * -1 * var(--character-unit));
					filter: blur(5px);
				}
			`} 250ms ${eases.easeOutMax} calc(--sibling-index-0() * (15ms / var(--length, 7) * 7)) forwards;
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
			transition-duration: 500ms !important;
		}
	}
`;

export default function DragToImport({ children }: FCP<{
	children: string;
}>) {
	const [shown, setShown] = useState(false);
	const [drop, setDrop] = useState(false);

	useListen("host:dragOver", e => {
		setShown(e.isDragging ?? false);
		setDrop(e.isDragging === false);
	});

	useOnDrag_dev(setShown, setDrop);

	const title = t.dragToImport({ item: children });

	return (
		<Portal>
			<CssTransition in={shown} unmountOnExit>
				<StyledDragToImport
					role="alertdialog"
					aria-modal
					className={{ drop }}
					onMouseDown={() => setShown(false)}
					onMouseUp={() => setShown(false)}
				>
					<div className="box">
						<EmptyMessage icon="touch_pointer" title={<LetterByLetter>{title}</LetterByLetter>} noSideEffect />
					</div>
				</StyledDragToImport>
			</CssTransition>
		</Portal>
	);
}

// Only work for testing in development mode.
function useOnDrag_dev(setShown: SetState<boolean>, setDrop: SetState<boolean>) {
	if (window.isWebView) return;

	function onDragOver(e: DragEvent) {
		if (!e.dataTransfer) return;
		stopEvent(e);
		if (e.dataTransfer.items[0]?.kind !== "file") {
			e.dataTransfer.dropEffect = "none";
			return;
		}
		e.dataTransfer.dropEffect = "copy";
		console.log(e.dataTransfer.items[0]);
		setShown(true);
	}

	function onDragEnd(e: DragEvent) {
		stopEvent(e);
		setShown(false);
		setDrop(e.type === "drop");
	}

	useEventListener(document, "dragover", onDragOver);
	useEventListener(document, "dragstart", onDragOver);
	useEventListener(document, "dragleave", onDragEnd);
	useEventListener(document, "dragend", onDragEnd);
	useEventListener(document, "drop", onDragEnd);
}
