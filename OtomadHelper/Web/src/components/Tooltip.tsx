const StyledTooltip = styled.div`
	${styles.mixins.square("0")};
	position: fixed;
	z-index: 80;
	display: flex;
	pointer-events: none;

	.base {
		flex-shrink: 0;
		max-width: 50dvw;
		height: max-content;
		padding: 6px 8px;
		word-wrap: break-word;
		overflow-wrap: break-word;
		background-color: ${c("background-fill-color-acrylic-background-default")};
		border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
		border-radius: 4px;
		box-shadow: 0 4px 8px ${c("shadows-flyout")};
		backdrop-filter: blur(60px);
	}

	${tgs()} .base {
		opacity: 0;
	}

	&.top {
		justify-content: center;
		align-items: flex-end;
	}

	&.bottom {
		justify-content: center;
		align-items: flex-start;
	}

	&.right {
		justify-content: flex-start;
		align-items: center;
	}

	&.left {
		justify-content: flex-end;
		align-items: center;
	}
`;

export default function Tooltip({ title, placement, offset = 10, timeout = 500, children }: FCP<{
	/** 工具提示内容。 */
	title: ReactNode;
	/** 工具提示方向。 */
	placement?: Placement;
	/** 工具提示偏移。 */
	offset?: number;
	/** 延时显示。 */
	timeout?: number;
}>) {
	const [shown, setShown] = useState(false);
	const contentsDom = useDomRef<HTMLDivElement>();
	const tooltipWrapper = useDomRef<HTMLDivElement>();
	const [actualPlacement, setActualPlacement] = useState(placement);
	const [position, setPosition] = useState<CSSProperties>();
	const shownTimeout = useRef<Timeout>();

	const handleHover = () => {
		clearTimeout(shownTimeout.current);
		shownTimeout.current = setTimeout(async () => {
			const dom = contentsDom.current?.firstElementChild;
			if (!dom) return;
			const options = getPosition(dom, placement, offset);
			setActualPlacement(options.placement);
			setPosition(options.style);
			setShown(true);
			await nextAnimationTick();
			const tooltip = tooltipWrapper.current?.firstElementChild as HTMLElement;
			if (!tooltip) return;
			setPosition(moveIntoPage(tooltip, tooltipWrapper));
		}, timeout);
	};

	const handleUnhover = () => {
		clearTimeout(shownTimeout.current);
		setShown(false);
	};

	return (
		<>
			<Contents ref={contentsDom} onMouseEnter={handleHover} onMouseLeave={handleUnhover}>
				{children}
			</Contents>
			<Portal>
				<CssTransition in={shown} unmountOnExit>
					<StyledTooltip ref={tooltipWrapper} className={actualPlacement} style={position}>
						<div className="base">
							{title}
						</div>
					</StyledTooltip>
				</CssTransition>
			</Portal>
		</>
	);
}
