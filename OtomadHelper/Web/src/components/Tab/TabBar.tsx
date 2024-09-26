import TabItem from "./TabItem";

const THICKNESS = 3;
const LENGTH = 20;
const DELAY = 200;

const Indicator = styled.div.attrs(({ $vertical }) => ({
	className: $vertical ? "vertical" : "horizontal",
}))<{
	/** Disable transition animation? */
	$noTransition?: boolean;
	/** Position (a tuple of distances from the container in the upper and lower directions). */
	$position?: TwoD;
	/** Vertical tabs? */
	$vertical?: boolean;
}>`
	${styles.mixins.oval()}
	${({ $vertical }) => $vertical ? "inline" : "block"}-size: ${THICKNESS}px;
	position: absolute;
	background-color: ${c("accent-color")};
	${({ $vertical }) => $vertical ? css`inset-inline-start: 5px;` : css`inset-block-end: 0;`}
	${({ $noTransition }) => $noTransition && css`transition: none;`}
	${({ $position, $vertical }) => $position && css`
		${$vertical ? "inset-block-start" : "left"}: ${$position[0] ?? 0}px;
		${$vertical ? "inset-block-end" : "right"}: ${isUndefinedNullNaN($position[1]) ? "100%" : $position[1] + "px"};
	`};
`;

const StyledTabBar = styled.div`
	> .scroll {
		position: relative;
		overscroll-behavior: contain;
	}

	&:has(.selected:active) ${Indicator} {
		scale: 1 0.5;

		&.horizontal {
			scale: 0.5 1;
		}
	}

	hr {
		margin: 4px 0;
		border: none;
		border-bottom: 1px solid ${c("stroke-color-divider-stroke-default")};
	}

	.items {
		> * {
			flex-shrink: 0;
		}
	}

	&.horizontal {
		${styles.mixins.noScrollbar()};
		width: 100%;
		margin: -4px;
		padding: 4px;
		overflow-x: auto;

		> .scroll {
			width: min-content;
		}

		.items {
			display: flex;
		}
	}
`;

export default function TabBar<T extends string = string>({ current: [current, setCurrent], collapsed, children, vertical }: FCP<{
	/** The identifier of the currently selected item. */
	current: StateProperty<T>;
	/** Hide the text label and only show the icon? */
	collapsed?: boolean;
	/** Use the vertical NavigationView style? */
	vertical?: boolean;
}>) {
	const indicatorEl = useDomRef<"div">();
	const [position, _setPosition] = useState<TwoD>([NaN, NaN]);
	const [noIndicatorTransition, setNoIndicatorTransition] = useState(false);
	const updateIndicatorThread = useRef<symbol>();
	const { uiScale1 } = useSnapshot(configStore.settings);

	/**
	 * Update the tab indicator.
	 */
	const update = useCallback(async () => { // FIXME: 先点击设置，再点击其它导航项，会意外发生从设置之前的一个导航项过渡到新导航项的动画，仅在生产环境触发，开发环境却没得问题。
		const indicator = indicatorEl.current;
		if (!indicator) return;
		type TabBarMovement = "previous" | "next" | "appear" | "disappear" | "none";
		let movement: TabBarMovement = "none";
		if (!indicator.offsetParent) { // If the indicator (and its ancestors) are hidden
			_setPosition([NaN, NaN]);
			const hiddenElement = getPath(indicator).find(element => getComputedStyle(element).display === "none");
			if (!hiddenElement) return;
			const MAX_WAITING_TIME = 10_000, observeTime = Date.now();
			const observer = new MutationObserver(() => {
				if (indicator.offsetParent) {
					observer.disconnect();
					update();
				}
				if (Date.now() - observeTime > MAX_WAITING_TIME)
					observer.disconnect();
			});
			observer.observe(hiddenElement, { attributeFilter: ["class"] });
			return;
		}
		const entireRect = indicator.parentElement!.getBoundingClientRect();
		const entire1 = entireRect[vertical ? "top" : "left"],
			entire2 = entireRect[vertical ? "bottom" : "right"],
			entireLength = (entire2 - entire1) / uiScale1;
		const setPosition = setStateInterceptor(_setPosition, ([pos1, pos2]: TwoD) => [pos1, entireLength - pos2] as TwoD);
		const [entry1, entry2] = position;
		if (entry1 + entry2 >= entireLength || !Number.isFinite(entry1) || !Number.isFinite(entry2))
			movement = "appear";
		const selectedTabItem = indicator.previousElementSibling!.querySelector(".selected");
		if (!selectedTabItem) {
			if (movement === "appear") return;
			movement = "disappear";
			const center = (entry1 + entireLength - entry2) / 2;
			setPosition([center, center - 1]);
			return;
		}
		const targetRect = selectedTabItem.getBoundingClientRect();
		let target1 = (targetRect[vertical ? "top" : "left"] - entire1) / uiScale1,
			target2 = (targetRect[vertical ? "bottom" : "right"] - entire1) / uiScale1;
		const targetOffset = (target2 - target1 - LENGTH) / 2;
		if (targetOffset > 0) { target1 += targetOffset; target2 -= targetOffset; }
		if (movement === "appear") {
			setNoIndicatorTransition(true);
			const center = (target1 + target2) / 2;
			setPosition([center, center]);
			await nextAnimationTick();
			setNoIndicatorTransition(false);
			setPosition([target1, target2]);
			return;
		}
		const movementSign = entry1 + entireLength - entry2 - (target1 + target2);
		movement = movementSign > 0 ? "previous" : movementSign < 0 ? "next" : "none";
		if (movement === "none") return;
		const setPosition1 = () => _setPosition(([_, pos2]) => [target1, pos2]);
		const setPosition2 = () => setPosition(([pos1]) => [pos1, target2]);
		const delayTime = () => delay(DELAY);
		const thisThread = Symbol("update");
		updateIndicatorThread.current = thisThread;
		if (movement === "previous") {
			setPosition1();
			await delayTime();
			if (updateIndicatorThread.current !== thisThread) return;
			setPosition2();
		} else if (movement === "next") {
			setPosition2();
			await delayTime();
			if (updateIndicatorThread.current !== thisThread) return;
			setPosition1();
		}
	}, [position, uiScale1]);

	useEffect(() => {
		update();
	}, [current, children]);

	return (
		<HorizontalScroll enabled={!vertical}>
			{/* Vertical tab bar (navigation view) do not care about horizontal scrolling. */}
			<StyledTabBar className={[vertical ? "vertical" : "horizontal"]}>
				<div className="scroll">
					<div className="items">
						{React.Children.map(children, child => {
							if (!isReactInstance(child, TabItem)) return child;
							const id = child.props.id as T;
							return React.cloneElement(child, {
								collapsed,
								_vertical: vertical,
								selected: current === id,
								onClick: () => setCurrent?.(id),
							});
						})}
					</div>
					<Indicator ref={indicatorEl} $position={position} $noTransition={noIndicatorTransition} $vertical={vertical} />
				</div>
			</StyledTabBar>
		</HorizontalScroll>
	);
}

TabBar.Item = TabItem;
