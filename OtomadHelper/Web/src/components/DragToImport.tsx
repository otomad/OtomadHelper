const StyledDragToImport = styled.div`
	${styles.mixins.fullscreen()};
	z-index: 80;
	padding: 1rem;
	background-color: ${c("background-fill-color-smoke-default")};
	scale: 1.05;
	opacity: 0;

	* {
		pointer-events: none;
	}

	.box {
		${styles.mixins.flexCenter()};
		${styles.mixins.square("100%")};
		overflow: hidden;
		border: 2px dashed ${c("fill-color-text-secondary")};
		border-radius: 5px;

		.empty-message {
			scale: 1.1;

			h2 {
				${styles.effects.text.titleLarge};
			}

			.icon {
				font-size: 72px;
			}
		}
	}

	&.enter-active,
	&.enter-done {
		padding: 1rem;
		scale: 1;
		opacity: 1;
		backdrop-filter: blur(4px);

		.empty-message {
			scale: 1;
		}
	}
`;

export default function DragToImport({ children }: FCP<{
	children: string;
}>) {
	const [shown, setShown] = useState(false);

	function onDragOver(e: DragEvent) {
		if (!e.dataTransfer) return;
		stopEvent(e);
		if (e.dataTransfer.items[0]?.kind !== "file") {
			e.dataTransfer.dropEffect = "none";
			return;
		}
		e.dataTransfer.dropEffect = "copy";
		console.log(e.dataTransfer.items[0]); // WARN: 发行时移除。
		setShown(true);
	}

	function onDragEnd(e: DragEvent) {
		stopEvent(e);
		setShown(false);
	}

	useEventListener(document, "dragover", onDragOver);
	useEventListener(document, "dragstart", onDragOver);
	useEventListener(document, "dragleave", onDragEnd);
	useEventListener(document, "dragend", onDragEnd);
	useEventListener(document, "drop", onDragEnd);

	const title = t.dragToImport({ item: children });

	return (
		<Portal>
			<CssTransition in={shown} unmountOnExit>
				<StyledDragToImport>
					<div className="box">
						<EmptyMessage icon="touch_pointer" heading={title} />
					</div>
				</StyledDragToImport>
			</CssTransition>
		</Portal>
	);
}
