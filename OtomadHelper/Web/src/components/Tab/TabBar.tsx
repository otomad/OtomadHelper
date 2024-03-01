import TabItem from "./TabItem";

const THICKNESS = 3;
const LENGTH = 20;
const DELAY = 200;

const Indicator = styled.div.attrs((({ $vertical }: { $vertical: boolean }) => ({
	className: $vertical ? "vertical" : "horizontal",
})) as Any)<{
	/** 是否禁用过渡动画？ */
	$noTransition?: boolean;
	/** 位置（上方向和下方向距离容器的距离元组）。 */
	$position?: TwoD;
	/** 纵向的选项卡？ */
	$vertical?: boolean;
}>`
	${styles.mixins.oval()}
	${({ $vertical }) => $vertical ? "width" : "height"}: ${THICKNESS}px;
	position: absolute;
	background-color: ${c("accent-color")};
	${({ $vertical }) => $vertical ? css`left: 5px` : css`bottom: 0`};
	${({ $noTransition }) => $noTransition && css`transition: none`};
	${({ $position, $vertical }) => $position && css`
		${$vertical ? "top" : "left"}: ${$position[0]}px;
		${$vertical ? "bottom" : "right"}: ${$position[1]}px;
	`};
`;

const StyledTabBar = styled.div`
	> .scroll {
		position: relative;
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
		width: 100%;
		margin: -4px;
		padding: 4px;
		overflow-x: auto;

		&::-webkit-scrollbar {
			height: 0;
		}

		> .scroll {
			width: min-content;
		}

		.items {
			display: flex;
		}
	}
`;

export default function TabBar<T extends string = string>({ current: [current, setCurrent], collapsed, children, vertical }: FCP<{
	/** 当前选中项标识符。 */
	current: StateProperty<T>;
	/** 是否隐藏文本标签，仅显示图标？ */
	collapsed?: boolean;
	/** 是否使用纵向的 NavigationView 样式？ */
	vertical?: boolean;
}>) {
	const indicator = useDomRef<HTMLDivElement>();
	const [position, _setPosition] = useState<TwoD>([NaN, NaN]);
	const [noIndicatorTransition, setNoIndicatorTransition] = useState(false);

	/**
	 * 更新选项卡指示器。
	 */
	const update = useCallback(async () => {
		const ind = indicator.current;
		if (!ind) return;
		type TabBarMovement = "previous" | "next" | "appear" | "disappear" | "none";
		let movement: TabBarMovement = "none";
		const entireRect = ind.parentElement!.getBoundingClientRect();
		const entire1 = entireRect[vertical ? "top" : "left"],
			entire2 = entireRect[vertical ? "bottom" : "right"],
			entireLength = entire2 - entire1;
		const setPosition = setStateInterceptor(_setPosition, ([pos1, pos2]: TwoD) => [pos1, entireLength - pos2] as TwoD);
		const [entry1, entry2] = position;
		if (entry1 + entry2 >= entireLength || !Number.isFinite(entry1) || !Number.isFinite(entry2))
			movement = "appear";
		const selectedTabItem = ind.previousElementSibling!.querySelector(".selected");
		if (!selectedTabItem) {
			if (movement === "appear") return;
			movement = "disappear";
			const center = (entry1 + entireLength - entry2) / 2;
			setPosition([center, center - 1]);
			return;
		}
		const targetRect = selectedTabItem.getBoundingClientRect();
		let target1 = targetRect[vertical ? "top" : "left"] - entire1,
			target2 = targetRect[vertical ? "bottom" : "right"] - entire1;
		const targetOffset = (target2 - target1 - LENGTH) / 2;
		if (targetOffset > 0) target1 += targetOffset, target2 -= targetOffset;
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
		if (movement === "previous") {
			setPosition1();
			await delayTime();
			setPosition2();
		} else if (movement === "next") {
			setPosition2();
			await delayTime();
			setPosition1();
		}
	}, [position]);

	useEffect(() => {
		update();
	}, [current, children]);

	return (
		<StyledTabBar className={[vertical ? "vertical" : "horizontal"]}>
			<div className="scroll">
				<div className="items">
					{React.Children.map(children, child => {
						if (!isReactInstance(child, TabItem)) return child;
						const id = child.props.id as T;
						return (
							React.cloneElement(child, {
								collapsed,
								vertical,
								selected: current === id,
								onClick: () => setCurrent?.(id),
							})
						);
					})}
				</div>
				<Indicator ref={indicator} $position={position} $noTransition={noIndicatorTransition} $vertical={vertical} />
			</div>
		</StyledTabBar>
	);
}

TabBar.Item = TabItem;
