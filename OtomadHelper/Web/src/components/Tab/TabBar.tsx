const THICKNESS = 3;
const LENGTH = 16;
const DELAY = 100;

const Indicator = styled.div<{
	/** 是否禁用过渡动画？ */
	$noTransition?: boolean;
	/** 位置（上方向和下方向距离容器的距离元组）。 */
	$position?: TwoD;
}>`
	${styles.mixins.oval()}
	width: ${THICKNESS}px;
	background-color: ${c("accent-color")};
	position: absolute;
	left: 5px;
	${({ $noTransition }) => $noTransition && css`transition: none`};
	${({ $position }) => $position && css`
		top: ${$position[0]}px;
		bottom: ${$position[1]}px;
	`};
`;

const StyledTabBar = styled.div`
	position: relative;

	&:has(.active:active) ${Indicator} {
		scale: 1 0.5;
	}

	hr {
		margin: 4px 0;
		border: none;
		border-bottom: 1px solid ${c("white", 8)};
	}

	.items {
		.enter {
			opacity: 0;
		}
		.enter-active {
			opacity: 1;
			transition: opacity 500ms ease-in;
		}
		.exit {
			opacity: 1;
		}
		.exit-active {
			opacity: 0;
			transition: opacity 500ms ease-in;
		}
	}
`;

interface Prop<T> {
	current: StateProperty<T>;
	children?: ReactNode;
}

const TabBar: <T extends string = string>(p: Prop<T>) => ReactElement<Prop<T>> =
	({ current: [current, setCurrent], children }) => {
		type T = typeof current;
		const indicator = useRef<HTMLDivElement>(null);
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
			const entire1 = entireRect.top, entire2 = entireRect.bottom, entireLength = entire2 - entire1;
			const setPosition = setStateInterceptor(_setPosition, ([pos1, pos2]: TwoD) => [pos1, entireLength - pos2] as TwoD);
			const [entry1, entry2] = position;
			if (entry1 + entry2 >= entireLength || !Number.isFinite(entry1) || !Number.isFinite(entry2))
				movement = "appear";
			const activeTabItem = ind.previousElementSibling!.querySelector(".active");
			if (!activeTabItem) {
				if (movement === "appear") return;
				movement = "disappear";
				const center = (entry1 + entireLength - entry2) / 2;
				setPosition([center, center - 1]);
				return;
			}
			const targetRect = activeTabItem.getBoundingClientRect();
			let target1 = targetRect.top - entire1, target2 = targetRect.bottom - entire1;
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
			const movementSign = (entry1 + entireLength - entry2) - (target1 + target2);
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
			<StyledTabBar>
				<div className="items">
					{React.Children.map(children, child => {
						if (!isReactInstance(child, TabItem)) return child;
						const id = child.props.id as T;
						return (
							React.cloneElement(child, {
								active: current === id,
								onClick: () => setCurrent(id),
							})
						);
					})}
				</div>
				<Indicator ref={indicator} $position={position} $noTransition={noIndicatorTransition} />
			</StyledTabBar>
		);
	};

export default TabBar;
