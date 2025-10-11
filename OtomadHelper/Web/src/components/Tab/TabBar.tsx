import TabItem from "./TabItem";

const THICKNESS = 3;
const LENGTH = 20;
const DELAY = 200;

type TabBarMovement = "previous" | "next" | "appear" | "disappear" | "none";

const Indicator = styled.div.attrs(({ $vertical }) => ({
	className: $vertical ? "vertical" : "horizontal",
}))<{
	/** Vertical tabs? */
	$vertical?: boolean;
}>(({ $vertical }) => {
	const start = $vertical ? "inset-block-start" : "left", end = $vertical ? "inset-block-end" : "right";
	return css`
		/** Position (a tuple of distances from the container in the upper and lower directions). */
		--position-s: 0;
		--position-e: 100%;

		--delayed-property: content;

		${styles.mixins.oval()};
		${$vertical ? "inline" : "block"}-size: ${THICKNESS}px;
		position: absolute;
		background-color: ${c("accent-color")};
		${$vertical ? css`inset-inline-start: 5px;` : css`inset-block-end: 0;`}
		${start}: var(--position-s);
		${end}: var(--position-e);
		transition:
			all ${eases.easeOutCirc} 300ms,
			${fallbackTransitions.replace(/all.*?ms,\s*/, "")},
			scale ${eases.easeInOutMaterialEmphasized} 350ms,
			var(--delayed-property) ${eases.easeInOutMaterialEmphasized} 500ms 50ms;

		&.appearing {
			transition: none !important;
		}
		${$vertical && css`
			${ifColorScheme.at.contrast} {
				background-color: ${cc("HighlightText")};
			}
		`}
	`;
});

const StyledTabBar = styled(HorizontalScroll).attrs({
	container: "nav",
})`
	scroll-padding: 0;

	> .scroll-target {
		position: relative;
		overscroll-behavior: contain;
	}

	&:has(.selected:active):not(.disable-press-indicator-style) ${Indicator} {
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
		container: horizontal-tab / scroll-state size;
		position: relative;
		display: flex;
		block-size: 50px;
		inline-size: 100%;
		overflow-inline: auto;

		> .scroll-target {
			width: min-content;
		}

		.items {
			display: flex;
		}
	}
`;

export default function TabBar<T extends string = string>({ current: [current, setCurrent], collapsed, children, vertical, ...htmlAttrs }: FCP<{
	/** The identifier of the currently selected item. */
	current: StateProperty<T>;
	/** Hide the text label and only show the icon? */
	collapsed?: boolean;
	/** Use the vertical NavigationView style? */
	vertical?: boolean;
}, "nav">) {
	const indicatorEl = useDomRef<"div">();
	const [position, _setPosition] = useState<TwoD>([NaN, NaN]);
	const uiScale1 = useUiScale1();
	const [_movement, setMovement] = useState<TabBarMovement>("disappear");
	const [disablePressIndicatorStyle, setDisablePressIndicatorStyle] = useDelayState(false);
	const [appearingPosition, setAppearingPosition] = useDelayState<TwoD>();
	// const { status: mainPageTransitionStatus } = useContext(MainPageTransitionContext);

	/**
	 * Update the tab indicator.
	 */
	const update = useCallback(() => {
		const indicator = indicatorEl.current;
		if (!indicator?.checkVisibility()) return;
		let movement: TabBarMovement = "none";
		// setDisablePressIndicatorStyle(true, { keep: DELAY, allowInterrupt: true }).then(() => setDisablePressIndicatorStyle(false));
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
			movement = "disappear";
			const center = (entry1 + entireLength - entry2) / 2;
			setPosition([center, center - 1]);
			setMovement(movement);
			return;
		}
		const targetRect = selectedTabItem.getBoundingClientRect();
		let target1 = (targetRect[vertical ? "top" : "left"] - entire1) / uiScale1,
			target2 = (targetRect[vertical ? "bottom" : "right"] - entire1) / uiScale1;
		const targetOffset = (target2 - target1 - LENGTH) / 2;
		if (targetOffset > 0) { target1 += targetOffset; target2 -= targetOffset; }
		const setPositionBoth = () => setPosition([target1, target2]);
		if (movement === "appear") {
			const center = (target1 + target2) / 2;
			setAppearingPosition([center, entireLength - center], { keep: 1, allowInterrupt: true }).then(() => setAppearingPosition(undefined));
			setPositionBoth();
			setMovement(movement);
			return;
		}
		const movementSign = entry1 + entireLength - entry2 - (target1 + target2);
		movement = movementSign > 0 ? "previous" : movementSign < 0 ? "next" : "none";
		setPositionBoth();
		setMovement(movement);
	}, [position, uiScale1, _movement]);

	useEffect(() => {
		// if (mainPageTransitionStatus !== "entered") return;
		update();
	}, [current, children/* , mainPageTransitionStatus */]);

	return (
		<StyledTabBar enabled={!vertical} role="tablist" className={[vertical ? "vertical" : "horizontal", { disablePressIndicatorStyle }]} {...htmlAttrs}>
			{!vertical && <Flipper arrow="left" />}
			<div className="scroll-target">
				<div className="items">
					{React.Children.map(children, child => {
						if (!isReactInstance(child, TabItem)) return child;
						const id = child.props.id as T;
						const { onClick } = child.props;
						return React.cloneElement(child, {
							collapsed,
							_vertical: vertical,
							selected: current === id,
							onClick: e => { setCurrent?.(id); onClick?.(e); },
						});
					})}
				</div>
				<Indicator
					ref={indicatorEl}
					className={{ appearing: appearingPosition }}
					style={{
						...!appearingPosition ? {
							"--position-s": (position[0] ?? 0) + "px",
							"--position-e": isUndefinedNullNaN(position[1]) ? "100%" : position[1] + "px",
						} : {
							"--position-s": appearingPosition[0] + "px",
							"--position-e": appearingPosition[1] + "px",
						},
						"--delayed-property":
							!_movement.in("next", "previous") ? undefined :
							_movement === "next" ?
								vertical ? "inset-block-start" : "left" :
								vertical ? "inset-block-end" : "right",
					}}
					$vertical={vertical}
				/>
			</div>
			{!vertical && <Flipper arrow="right" />}
		</StyledTabBar>
	);
}

TabBar.Item = TabItem;
