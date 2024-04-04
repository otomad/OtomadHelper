import { styledExpanderItemBase, styledExpanderItemContent, styledExpanderItemText } from "components/Expander/ExpanderItem";

const THUMB_SIZE = 18;
const THUMB_PRESSED_WIDTH = 22;

// const isHoverPseudo = ":is(&:hover, .settings-card:hover .trailing &)";
// const isPressedPseudo = ":is(&:active, .settings-card:active .trailing &)";
// WARN: styled components bug: https://github.com/styled-components/styled-components/issues/4279
const isHoverPseudo = "&:hover, .settings-card:hover .trailing &:only-child";
const isPressedPseudo = "&:active, &.pressed, .settings-card:active .trailing &:only-child";

const StyledToggleSwitchLabel = styled.button`
	display: flex;
	gap: 12px;
	justify-content: space-between;
	align-items: center;
	text-align: left;

	:where(&) {
		width: 100%;
	}

	.right {
		display: flex;
		gap: inherit;
		align-items: center;

		.text {
			width: unset !important;
		}
	}

	${styledExpanderItemText};

	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};
	}

	.stroke {
		${styles.mixins.oval()};
		width: 40px;
		height: 20px;
		overflow: clip;
		border: 1px solid ${c("stroke-color-control-strong-stroke-default")};
	}

	.base {
		${styles.mixins.square("100%")};
		position: relative;
		background-color: ${c("fill-color-control-alt-secondary")};
	}

	.thumb {
		${styles.mixins.square(`${THUMB_SIZE}px`)};
		${styles.mixins.oval()};
		position: absolute;
		left: 0;
		background-color: ${c("fill-color-text-secondary")};
		scale: calc(12 / ${THUMB_SIZE});
		touch-action: pinch-zoom;
	}

	${isHoverPseudo} {
		.base {
			background-color: ${c("fill-color-control-alt-tertiary")};

			.thumb {
				scale: calc(14 / ${THUMB_SIZE});
			}
		}
	}

	${isPressedPseudo} {
		.base {
			background-color: ${c("fill-color-control-alt-quarternary")};

			.thumb {
				width: ${THUMB_PRESSED_WIDTH}px;
				scale: calc(14 / ${THUMB_SIZE});
			}
		}
	}

	&[disabled] {
		.stroke {
			border-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
		}

		.thumb {
			background-color: ${c("fill-color-text-disabled")};
		}

		.text {
			color: ${c("fill-color-text-disabled")};
		}
	}

	${styles.mixins.forwardFocusRing("toggle-switch-base")};

	&.selected {
		.stroke {
			border-color: ${c("accent-color")};
		}

		.base {
			background-color: ${c("accent-color")} !important;
		}

		.thumb {
			left: calc(100% - ${THUMB_SIZE}px);
			background-color: ${c("fill-color-text-on-accent-primary")};
			outline: 1px solid ${c("stroke-color-control-stroke-secondary")};
		}

		${isHoverPseudo} {
			opacity: 0.9;
		}

		${isPressedPseudo} {
			opacity: 0.8;

			.thumb {
				left: calc(100% - ${THUMB_PRESSED_WIDTH}px);
			}
		}

		&[disabled] {
			.stroke {
				border-color: ${c("stroke-color-control-strong-stroke-disabled")};
			}

			.base {
				background-color: ${c("stroke-color-control-strong-stroke-disabled")};
			}

			.thumb {
				background-color: ${c("fill-color-text-on-accent-disabled")};
			}
		}
	}
`;

export default function ToggleSwitch({ on: [on, setOn], disabled, isPressing: [isPressing, setIsPressing] = [], hideLabel, as, details, resetTransitionOnChanging = false, children, ...htmlAttrs }: FCP<{
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
	/** 向父组件通信当前切换开关是否已经按下。 */
	isPressing?: StateProperty<boolean>;
	/** 隐藏“开/关”文本标签？ */
	hideLabel?: boolean;
	/** 改变标签名称。 */
	as?: WebTarget;
	/** 详细描述。 */
	details?: ReactNode;
	/**
	 * 在切换开关时重设页面的过渡效果。
	 * @note 这是一个业务逻辑，但是出现在了基础组件中。
	 */
	resetTransitionOnChanging?: boolean;
}, "button">) {
	const textLabel = on ? t.on : t.off;
	const [isDragging, setIsDragging] = useState(false);
	const [thumbLeft, setThumbLeft] = useState<number>();
	const [pressed, setPressed] = useState(false);
	// 注意：直接使用 styled-components 的参数改变会影响性能。
	const thumbStyle = useMemo(() => thumbLeft === undefined ? undefined : {
		left: thumbLeft + "px",
		transition: "none",
	} as CSSProperties, [thumbLeft]);

	const { resetTransition } = usePageStore();
	useUpdateEffect(() => {
		if (resetTransitionOnChanging)
			resetTransition();
	}, [resetTransitionOnChanging, resetTransition, on, disabled]);

	const handleCheck = (on: boolean, e?: MouseEvent) => {
		stopEvent(e);
		if (!isDragging) setOn?.(on);
		setIsDragging(false);
	};

	const onThumbDown = useCallback<PointerEventHandler<HTMLDivElement>>(e => {
		stopEvent(e);
		setPressed(true);
		setIsPressing?.(true);
		const thumb = e.currentTarget;
		const control = thumb.parentElement!;
		const controlRect = control.getBoundingClientRect();
		const left = controlRect.left, max = controlRect.width - THUMB_PRESSED_WIDTH;
		const x = e.pageX - left - thumb.offsetLeft;
		let isMoved = false, prevE: PointerEvent | undefined;
		const pointerMove = (e: PointerEvent) => {
			isMoved = true;
			setThumbLeft(clamp(e.pageX - left - x, 0, max));
			prevE = e;
		};
		const pointerUp = (e: PointerEvent) => {
			document.removeEventListener("pointermove", pointerMove);
			document.removeEventListener("pointerup", pointerUp);
			if (!(e instanceof MouseEvent) && prevE) e = prevE;
			const isOn = e.pageX - x > left + max / 2;
			handleCheck(isOn);
			setThumbLeft(undefined);
			setIsDragging(isMoved); // 定义识别为拖动而不是点击。
			nextAnimationTick().then(() => {
				setPressed(false);
				setIsPressing?.(false);
			});
		};
		document.addEventListener("pointermove", pointerMove);
		document.addEventListener("pointerup", pointerUp);
	}, []);

	return (
		<StyledToggleSwitchLabel
			as={as as "button"}
			className={{ selected: on, pressed }}
			disabled={disabled}
			onClick={e => handleCheck(!on, e)}
			tabIndex={0}
			{...htmlAttrs}
		>
			<div className="text">
				<p className="title">{children}</p>
				<p className="details">{details}</p>
			</div>
			<div className="right">
				{!hideLabel && <span className="text">{textLabel}</span>}
				<div className={["stroke", "toggle-switch-base", { pressing: isPressing }]}>
					<div className="base">
						<div className="thumb" style={thumbStyle} onPointerDown={onThumbDown} />
					</div>
				</div>
			</div>
		</StyledToggleSwitchLabel>
	);
}
