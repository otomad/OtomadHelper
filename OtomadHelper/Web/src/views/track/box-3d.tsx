import grab from "assets/cursors/grab.svg?cursor";
import grabbing from "assets/cursors/grabbing.svg?cursor";

const faces = ["front", "back", "left", "right", "top", "bottom"] as const;
// const SIDE_LENGTH = 200;
const DEFAULT_ROTATION = Object.freeze(new Point(-27, -36));

const StyledCube = styled.div`
	${styles.mixins.flexCenter()};
	cursor: ${grab};

	&:active {
		cursor: ${grabbing};
	}

	.container-outer {
		--side-length: 20dvh;
		align-content: center;
		align-self: center;
		block-size: calc(var(--side-length) * 1.75);
		perspective: 750px;
		/* pointer-events: none; */

		.container {
			${styles.mixins.square("var(--side-length)")};
			position: relative;
			transform: rotateX(var(--rotate-x, ${DEFAULT_ROTATION.x})) rotateY(var(--rotate-y, ${DEFAULT_ROTATION.y}));
			transform-style: preserve-3d;
			user-select: none;
			transition: ${fallbackTransitions}, width 0s, height 0s;

			@starting-style {
				transform: none;
			}

			main.page.exit-done &,
			main.page.enter:not(.enter-active) & {
				transform: none;
			}

			// CAUTION: Do not add overflow: clip or hidden.
			// Or when drag the box to the top, bottom, left, and right face, it cannot be clicked.
			// See: https://stackoverflow.com/q/79460167/19553213
			* {
				${styles.mixins.square("var(--side-length)")};
				position: absolute;
				align-content: center;
				text-align: center;
				background-color: ${c("fill-color-control-default")};
				border: 2px solid ${c("stroke-color-control-stroke-default")};
				border-radius: calc(8 / 200 * var(--side-length));
				opacity: 0.875;
				backface-visibility: hidden;
				pointer-events: auto;
				transition: ${fallbackTransitions}, width 0s, height 0s, transform 0s;

				&:hover {
					background-color: ${c("fill-color-control-secondary")};
				}

				&:active {
					background-color: ${c("fill-color-control-tertiary")};
				}

				&.selected {
					color: ${c("fill-color-text-on-accent-primary")};
					background-color: ${c("accent-color")};

					&:hover {
						background-color: ${c("accent-color", 75)};
					}

					&:active {
						background-color: ${c("accent-color", 65)};
					}
				}

				&:not(.selected) {
					&:hover,
					&:active {
						color: ${c("accent-color")};
					}

					&:active {
						opacity: 0.5;
					}
				}

				&.front {
					transform: translateZ(calc(var(--side-length) / 2));
				}

				&.back {
					transform: translateZ(calc(var(--side-length) / -2)) rotateY(180deg);
				}

				&.left {
					transform: translateX(calc(var(--side-length) / -2)) rotateY(-90deg);
				}

				&.right {
					transform: translateX(calc(var(--side-length) / 2)) rotateY(90deg);
				}

				&.top {
					transform: translateY(calc(var(--side-length) / -2)) rotateX(90deg);
				}

				&.bottom {
					transform: translateY(calc(var(--side-length) / 2)) rotateX(-90deg);
				}

				&.backface {
					opacity: 0.1;
					backface-visibility: visible;
					pointer-events: none;
				}
			}
		}
	}
`;

export default function Box3d() {
	useSetLayoutEnabledOnSave("box3d", true);
	const { deleteTracks, useLongerSide } = useSelectConfig(c => c.track.box3d);
	const [selectedFace, setSelectedFace] = useState<typeof faces[number]>();

	// #region Drag Behavior
	const [position, setPosition] = useState(Point.zero);
	const [rotation, setRotation] = useState<Point>(DEFAULT_ROTATION);
	const rotationCss = useMemo(() => ({ "--rotate-x": rotation.x + "deg", "--rotate-y": rotation.y + "deg" }), [rotation]);
	const [isMoved, setIsMoved] = useState(false);

	const reset = () => {
		setPosition(Point.zero);
		setRotation(DEFAULT_ROTATION);
	};

	const onDragStart: PointerEventHandler<HTMLDivElement> = e => {
		setPosition(new Point(Math.round(e.clientX), Math.round(e.clientY)));
		e.currentTarget.setPointerCapture(e.pointerId);
	};

	const onDrag: PointerEventHandler<HTMLDivElement> = e => {
		if (!e.currentTarget.hasPointerCapture(e.pointerId)) return;
		const rx = floorMod(rotation.x, 360),
			ryDir = rx > 90 && rx < 270 ? 1 : -1;
		setRotation(({ x, y }) => new Point(
			x - (Math.round(e.clientY) - position.y),
			y - ryDir * (Math.round(e.clientX) - position.x) % 360,
		));
		const newPosition = new Point(Math.round(e.clientX), Math.round(e.clientY));
		setPosition(newPosition);
		if (!isMoved && newPosition.distance(position) >= 1) setIsMoved(true);
	};

	const onDragEnd: PointerEventHandler<HTMLDivElement> = e => {
		setPosition(Point.zero);
		e.currentTarget.releasePointerCapture(e.pointerId);
		const target = document.elementFromPoint(e.pageX, e.pageY); // The cube capture the mouse, so cannot get e.target directly.
		const face = target?.closest(".face");
		// eslint-disable-next-line @stylistic/brace-style
		if (e.button === 1 || e.button === 2) { if (!isMoved) reset(); }
		else if (!isMoved && face instanceof HTMLElement) face.click();
		setIsMoved(false);
	};
	// #endregion

	return (
		<div className="container">
			<CommandBar.Group>
				<CommandBar position="left" addGaps>
					<CommandBar.Item icon="delete" caption={t.track.box3d.deleteTracks} details={t.descriptions.track.box3d.deleteTracks} on={deleteTracks} />
					<CommandBar.Item icon="codepen" caption={t.track.box3d.useLongerSide} details={t.descriptions.track.box3d.useLongerSide} on={useLongerSide} />
				</CommandBar>
			</CommandBar.Group>

			<StyledCube onPointerDown={onDragStart} onPointerMove={onDrag} onPointerUp={onDragEnd}>
				<div className="container-outer">
					<div className="container" style={rotationCss}>
						{faces.map(face => {
							const className = [face, "face", { selected: selectedFace === face }];
							return (
								<Fragment key={face}>
									<div className={className} onClick={() => setSelectedFace(face)}>{t.track.box3d.faces[face]}</div>
									<div className={[className, "backface"]}>{t.track.box3d.faces[face]}</div>
								</Fragment>
							);
						})}
					</div>
				</div>
			</StyledCube>
		</div>
	);
}
