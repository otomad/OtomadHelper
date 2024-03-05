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
		border-radius: 3px;
		box-shadow: 0 4px 8px ${c("shadows-flyout")};
		backdrop-filter: blur(60px);

		&:has(img) {
			border-radius: 5px;
		}
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
	const [contentsEl, setContentsEl] = useState<HTMLDivElement | null>(null); // Use state instead of ref to make sure change it to rerender.
	const tooltipWrapperEl = useDomRef<HTMLDivElement>();
	const [actualPlacement, setActualPlacement] = useState(placement);
	const [position, setPosition] = useState<CSSProperties>();
	const shownTimeout = useRef<Timeout>();

	const dom = useMemo(() => {
		let dom = contentsEl?.firstElementChild;
		while (dom && (getComputedStyle(dom).display === "contents" || dom.classList.contains("expander")))
			dom = dom.firstElementChild;
		return dom as HTMLElement | null;
	}, [contentsEl]);

	const handleHover = (e: MouseEvent) => {
		clearTimeout(shownTimeout.current);
		if (!dom || !isInPath(e, dom)) return;
		shownTimeout.current = setTimeout(async () => {
			if (!dom) return;
			const options = getPosition(dom, placement, offset);
			setActualPlacement(options.placement);
			setPosition(options.style);
			setShown(true);
			await nextAnimationTick();
			const tooltip = tooltipWrapperEl.current?.firstElementChild as HTMLElement; // FIXME: tooltipWrapper 获取不到 ref。
			if (!tooltip) return;
			setPosition(moveIntoPage(tooltip, tooltipWrapperEl));
		}, timeout);
	};

	const handleUnhover = () => {
		clearTimeout(shownTimeout.current);
		setShown(false);
	};

	useEventListener(dom, "mouseenter", handleHover, undefined, [contentsEl]);
	useEventListener(dom, "mouseleave", handleUnhover, undefined, [contentsEl]);
	useEventListener(dom, "click", handleUnhover, undefined, [contentsEl]);

	return (
		<>
			<Contents ref={setContentsEl}>
				{children}
			</Contents>
			<Portal>
				<CssTransition in={shown} unmountOnExit>
					<StyledTooltip ref={tooltipWrapperEl} className={actualPlacement} style={position}>
						<div className="base">
							{title}
						</div>
					</StyledTooltip>
				</CssTransition>
			</Portal>
		</>
	);
}

const StyledTooltipContent = styled.figure`
	display: flex;
	flex-direction: column;
	gap: 8px;

	img {
		max-width: 250px;
		border-radius: 3px;

		&:has(~ figcaption:empty) {
			margin: 0 -2px;
		}
	}

	figcaption {
		${styles.mixins.hideIfEmpty()};
	}
`;

function TooltipContent({ image, children, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string;
}, "figure">) {
	return (
		<StyledTooltipContent {...htmlAttrs}>
			{image && <img src={image} />}
			<figcaption>{children}</figcaption>
		</StyledTooltipContent>
	);
}

Tooltip.Content = TooltipContent;
