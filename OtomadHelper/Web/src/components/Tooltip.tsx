const StyledTooltip = styled.div<{
	/** 工具提示偏移（仅用作动画）。 */
	$offset: number;
}>`
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

	${tgs()} {
		.base {
			opacity: 0;
		}

		${({ $offset }) => css`
			&.top {
				translate: 0 ${$offset}px;
			}

			&.bottom {
				translate: 0 ${-$offset}px;
			}

			&.right {
				translate: ${-$offset}px;
			}

			&.left {
				translate: ${$offset}px;
			}
		`}
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

		&:dir(rtl) {
			justify-content: flex-end;
		}
	}

	&.left {
		justify-content: flex-end;
		align-items: center;

		&:dir(rtl) {
			justify-content: flex-start;
		}
	}
`;

export default function Tooltip({ title, placement, offset = 10, timeout = 500, disabled = false, children }: FCP<{
	/** 工具提示内容。 */
	title: ReactNode;
	/** 工具提示方向。 */
	placement?: Placement;
	/** 工具提示偏移。 */
	offset?: number;
	/** 延时显示。 */
	timeout?: number;
	/** 不显示工具提示？ */
	disabled?: boolean;
}>) {
	const [shown, setShown] = useState(false);
	const [contentsEl, setContentsEl] = useDomRefState<"div">(); // Use state instead of ref to make sure change it to rerender.
	const tooltipEl = useDomRef<"div">();
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
			const tooltip = tooltipEl.current;
			if (!tooltip) return;
			setPosition(moveIntoPage(tooltip, tooltip.parentElement!));
		}, timeout);
	};

	const handleUnhover = () => {
		clearTimeout(shownTimeout.current);
		setShown(false);
	};

	useEventListener(dom, "mouseenter", handleHover, undefined, [contentsEl, title, placement, offset, timeout, disabled, children]);
	useEventListener(dom, "mouseleave", handleUnhover, undefined, [contentsEl]);
	useEventListener(dom, "click", handleUnhover, undefined, [contentsEl]);

	return (
		<>
			<Contents className="tooltip-child-wrapper" ref={setContentsEl}>
				{children}
			</Contents>
			{!disabled && (
				<Portal>
					<CssTransition in={shown} unmountOnExit>
						<StyledTooltip $offset={offset} className={actualPlacement} style={position}>
							<div className="base" ref={tooltipEl}>
								{title}
							</div>
						</StyledTooltip>
					</CssTransition>
				</Portal>
			)}
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
	}

	:only-child > img {
		margin: 0 -2px;
	}
`;

function TooltipContent({ image, children, ...htmlAttrs }: FCP<{
	/** 图片。 */
	image?: string;
}, "figure">) {
	return (
		<StyledTooltipContent {...htmlAttrs}>
			{image && <Img src={image} />}
			{children && <figcaption>{children}</figcaption>}
		</StyledTooltipContent>
	);
}

type TooltipProps = Parameters<typeof Tooltip>[0];

function TooltipWith(withProps: Partial<TooltipProps>) {
	// eslint-disable-next-line react/display-name
	return (props: TooltipProps) => <Tooltip {...withProps} {...props} />;
}

Tooltip.Content = TooltipContent;
Tooltip.with = TooltipWith;
