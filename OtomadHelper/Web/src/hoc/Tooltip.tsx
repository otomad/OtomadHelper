/** CAUTION: Only used for debugging during development, please ensure to be `false` in production. */
const DEBUG_MODE = false;

const StyledTooltip = styled.div`
	--offset: 10px;
	position: fixed;
	position-anchor: var(--anchor);
	z-index: 80;
	display: flex;
	justify-self: anchor-center;
	overflow: clip;
	border-radius: 4px;
	outline: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	box-shadow: 0 4px 8px ${c("shadows-flyout")};
	transition: opacity ${eases.easeOutMax} 250ms, margin ${eases.easeOutMax} 250ms;
	${!DEBUG_MODE && css`pointer-events: none;`};

	.base {
		flex-shrink: 0;
		block-size: max-content;
		max-width: if(
			media(width < 576px): 100dvw;
			media(width < 768px): 75dvw;
			else: 50dvw;
		);
		padding: 6px 8px;
		white-space-collapse: preserve-breaks;
		text-wrap: wrap; // Disable pretty text-wrap, because if line breaks, there are too many blank at the right.
		// stylelint-disable-next-line property-no-deprecated
		word-wrap: break-word;
		overflow-wrap: break-word;
		background-color: ${c("background-fill-color-acrylic-background-default")};
		backdrop-filter: blur(60px);

		&:has(.tooltip-content) {
			padding: 0;
		}
	}

	&:has(.tooltip-content) {
		border-radius: 7px;
	}

	&.top,
	&.bottom,
	&.y {
		position-area: top;
		position-try: flip-block;
		margin-bottom: var(--offset);

		&.bottom {
			position-area: bottom;
			margin-top: var(--offset);
			margin-bottom: 0;
		}

		&.y {
			position-try-order: most-height;
		}
	}

	&.left,
	&.right,
	&.x {
		position-area: left;
		position-try: flip-inline;
		margin-right: var(--offset);

		&.right {
			position-area: right;
			margin-right: 0;
			margin-left: var(--offset);
		}

		&.x {
			position-try-order: most-width;
		}
	}

	&.block-start,
	&.block-end,
	&.block {
		position-area: block-start;
		position-try: flip-block;
		margin-block-end: var(--offset);

		&.block-end {
			position-area: block-end;
			margin-block: var(--offset) 0;
		}

		&.block {
			position-try-order: most-block-size;
		}
	}

	&.inline-start,
	&.inline-end,
	&.inline {
		position-area: inline-start;
		position-try: flip-inline;
		margin-inline-end: var(--offset);

		&.inline-end {
			position-area: inline-end;
			margin-inline: var(--offset) 0;
		}

		&.inline {
			position-try-order: most-inline-size;
		}
	}

	${tgs()} {
		margin: 0 !important;
		opacity: 0;
	}
`;

const DEFAULT_TOOLTIP_ANCHOR_PREFIX = "--tooltip-anchor";

export default function Tooltip({ title: _title, placement, offset, timeout = 500, disabled = false, applyAriaLabel = true, children, ref }: FCP<{
	/** Tooltip content. */
	title: ReactNode | (() => ReactNode);
	/** Tooltip placement. */
	placement: Placement;
	/** Tooltip offset. Unit: px. @default 10 */
	offset?: number;
	/** Delayed display time. Unit: ms. @default 500 */
	timeout?: number;
	/** Do not show the tooltip? */
	disabled?: boolean;
	/** Automatically apply the tooltip title to the target element's aria label attribute unless it already has the attribute or it is aria hidden? @default true */
	applyAriaLabel?: boolean;
	ref?: ForwardedRef<"section">;
}>) {
	const getUpdatedTitle = useCallback(() => isI18nItem(_title) ? _title.toString() : typeof _title === "function" ? _title() : _title, [_title]);
	const [title, setTitle] = useState(getUpdatedTitle());
	const updateTitle = useCallback(() => setTitle(getUpdatedTitle()), [getUpdatedTitle]);
	const [shown, setShown] = useState(false);
	const [childEl, setChildEl] = useDomRefState<"div">(); // Use state instead of ref to make sure change it to rerender.
	const shownTimeout = useRef<Timeout>(undefined);
	const newAnchorName = DEFAULT_TOOLTIP_ANCHOR_PREFIX + useId();
	const anchorName = useRef(newAnchorName);
	const { isInPage } = useContext(MainPageContext);

	useImperativeHandle(ref, () => childEl!);

	useEffect(() => {
		if (!childEl) return;
		if (childEl.style.anchorName) anchorName.current = childEl.style.anchorName;
		else childEl.style.anchorName = newAnchorName;
		return () => {
			if (childEl.style.anchorName?.startsWith(DEFAULT_TOOLTIP_ANCHOR_PREFIX)) childEl.style.anchorName = null!;
		};
	}, [childEl]);

	const dom = useMemo(() => {
		let dom: Element | null = childEl;
		while (dom && (getComputedStyle(dom).display === "contents" || dom.classList.contains("expander")))
			dom = dom.firstElementChild;
		return dom as HTMLElement | null;
	}, [childEl]);

	useEffect(() => {
		updateTitle();
		if (dom && title && applyAriaLabel) {
			if (canToString(title)) dom.ariaLabel ||= title.toString();
			if (isReactInstance(title, TooltipContent)) {
				if (canToString(title.props.title)) dom.ariaLabel ||= title.props.title.toString();
				if (canToString(title.props.children)) dom.ariaDescription ||= title.props.children.toString();
			}
		}
	}, [dom, title, applyAriaLabel, updateTitle]);

	const handleHover = (e: MouseEvent) => {
		updateTitle();
		clearTimeout(shownTimeout.current);
		if (!dom || disabled || !isInPath(e, dom)) return;
		shownTimeout.current = setTimeout(() => {
			if (!dom) return;
			setShown(true);
		}, timeout);
	};

	const handleUnhover = () => {
		clearTimeout(shownTimeout.current);
		if (DEBUG_MODE) return;
		setShown(false);
	};

	useEventListener(dom, "mouseenter", handleHover, undefined, [childEl, title, placement, offset, timeout, disabled, children]);
	useEventListener(dom, "mouseleave", handleUnhover, undefined, [childEl]);
	useEventListener(dom, "mousedown", handleUnhover, undefined, [childEl]);
	useEventListener(window, "keydown", handleUnhover, { capture: true }, [childEl]);

	return (
		<>
			{cloneRef(children, setChildEl)}
			{!disabled && title && (
				<Portal>
					<CssTransition in={shown || DEBUG_MODE} unmountOnExit>
						<StyledTooltip
							role="tooltip"
							className={placement ?? "unknown"}
							style={{
								"--anchor": anchorName.current,
								"--offset": offset !== undefined ? offset + "px" : undefined,
								positionVisibility: isInPage ? "anchors-visible" : undefined,
							}}
						>
							<div className="base">
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

	img {
		max-width: 250px;
	}

	figcaption {
		${styles.effects.text.body}
		padding-block: 8px;
		padding-inline: 12px;

		h6 {
			${styles.effects.text.bodyStrong}
		}
	}

	:only-child > img {
		margin: 0 -2px;
	}
`;

function TooltipContent({ image, title, children, ...htmlAttrs }: FCP<{
	/** Image. */
	image?: string;
	/** Title. */
	title?: ReactNode;
}, "figure">) {
	return (
		<StyledTooltipContent {...htmlAttrs}>
			{image && <Img src={image} />}
			{(title || children) && (
				<figcaption>
					{title && <h6>{title}</h6>}
					{children}
				</figcaption>
			)}
		</StyledTooltipContent>
	);
}

export type TooltipProps = PropsOf<typeof Tooltip>;

function TooltipWith<TKey extends keyof TooltipProps>(withProps: Partial<Record<TKey, TooltipProps[TKey]>>) {
	const CurriedTooltip = Curry(Tooltip, withProps);
	return CurriedTooltip;
}

function TooltipWrap(component: ReactElement | React.ExoticComponent, tooltipProps: Partial<TooltipProps>) {
	const Component = component as unknown as FC;
	return function TooltipWrappedComponent({ title, ...props }: AnyObject) {
		const newTooltipProps = Object.assign({}, tooltipProps, title && { title }) as TooltipProps;
		return (
			<Tooltip {...newTooltipProps}>
				<Component {...props} />
			</Tooltip>
		);
	};
}

Tooltip.Content = TooltipContent;
Tooltip.with = TooltipWith;
Tooltip.wrap = TooltipWrap;
