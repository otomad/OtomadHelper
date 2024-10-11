const StyledContentDialog = styled.div`
	max-width: 100dvw;
	max-height: 100dvh;
	overflow: clip;
	background-color: ${c("background-fill-color-layer-alt-solid")};
	border-radius: 8px;
	outline: 1px solid ${c("stroke-color-surface-stroke-default")};
	box-shadow:
		0 32px 64px ${c("black", 19)},
		0 2px 21px ${c("black", 15)};

	> .content {
		display: flex;
		flex-direction: column;
		gap: 12px;
		padding: 24px;

		> * {
			${styles.mixins.hideIfEmpty()};
		}

		.title {
			${styles.effects.text.subtitle};
		}

		.body {
			${styles.effects.text.body};
		}
	}

	.button-grid {
		${styles.mixins.hideIfEmpty()};
		display: flex;
		gap: 8px;
		align-items: center;
		width: 100%;
		padding: 24px;
		background-color: ${c("background-fill-color-solid-background-base")};
		border-block-start: 1px solid ${c("stroke-color-card-stroke-default")};

		> button {
			width: 100%;
		}
	}
`;

const Mask = styled.div`
	${styles.mixins.gridCenter()};
	position: absolute;
	inset: 0;
	z-index: 30;
	background-color: ${c("background-fill-color-smoke-default")};
	backdrop-filter: grayscale(0.75);
	transition-timing-function: ${eases.easeOutMaterialStandard};

	${tgs()} {
		opacity: 0;

		${StyledContentDialog} {
			scale: 1.1;
		}
	}
`;

export default function ContentDialog({ shown: [shown, setShown], title, static: isStatic = false, children, buttons, ...htmlAttrs }: FCP<{
	/** Show the content dialog? */
	shown: StateProperty<boolean>;
	/** Dialog title. */
	title?: string;
	/** Focus content dialog. Don't close it when click outside? */
	static?: boolean;
	/** Action buttons. */
	buttons?: ReactNode | ((close: () => void) => ReactNode);
}, "div">) {
	const close = (): void => void setShown?.(false);

	const maskEl = useDomRef<"div">();

	const handleMaskPointerDown: PointerEventHandler<HTMLDivElement> = e => {
		if (!isStatic && e.currentTarget === e.target)
			maskEl.current = e.currentTarget;
	};

	useEventListener(document, "pointerup", e => {
		if (!isStatic && maskEl.current && maskEl.current === e.target)
			close();
		maskEl.current = null;
	});

	useEventListener(window, "keydown", e => {
		if (!isStatic && e.code === "Escape")
			close();
	});

	const setRootInert = (inert: boolean) => {
		const root = document.getElementById("root");
		if (root) root.inert = inert;
	};

	useEffect(() => {
		if (shown)
			setRootInert(true);
		else if (document.querySelectorAll("#popovers .content-dialog").length <= 1)
			setRootInert(false);
	}, [shown]);

	return (
		<Portal>
			<CssTransition in={shown} unmountOnExit appear>
				<Mask onPointerDown={handleMaskPointerDown}>
					<StyledContentDialog {...htmlAttrs}>
						<div className="content">
							<div className="title">{title}</div>
							<div className="body">{children}</div>
						</div>
						<div className="button-grid">
							{typeof buttons === "function" ? buttons(close) : buttons}
						</div>
					</StyledContentDialog>
				</Mask>
			</CssTransition>
		</Portal>
	);
}
