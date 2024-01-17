import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

const THUMB_SIZE = 18;
const THUMB_PRESSED_WIDTH = 22;

const StyledToggleSwitchLabel = styled.label`
	display: flex;
	align-items: center;
	justify-content: space-between;

	.right {
		display: flex;
		align-items: center;
		gap: 12px;

		.text {
			width: unset !important;
		}
	}

	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};
	}

	.stroke {
		${styles.mixins.oval()};
		width: 40px;
		height: 20px;
		border: 1px solid ${c("stroke-color-control-strong-stroke-default")};
		overflow: hidden;
	}

	.base {
		${styles.mixins.square("100%")};
		background-color: ${c("fill-color-control-alt-secondary")};
		position: relative;
	}

	.thumb {
		${styles.mixins.square(`${THUMB_SIZE}px`)};
		${styles.mixins.oval()};
		background-color: ${c("fill-color-text-secondary")};
		position: absolute;
		left: 0;
		scale: calc(12 / ${THUMB_SIZE});
	}

	&:hover .base {
		background-color: ${c("fill-color-control-alt-tertiary")};

		.thumb {
			scale: calc(14 / ${THUMB_SIZE});
		}
	}

	&:active .base {
		background-color: ${c("fill-color-control-alt-quarternary")};

		.thumb {
			scale: calc(14 / ${THUMB_SIZE});
			width: ${THUMB_PRESSED_WIDTH}px;
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

	&.active {
		.stroke {
			border-color: ${c("accent-color")};
		}

		.base {
			background-color: ${c("accent-color")};
		}

		.thumb {
			background-color: ${c("fill-color-text-on-accent-primary")};
			outline: 1px solid ${c("stroke-color-control-stroke-secondary")};
			left: calc(100% - ${THUMB_SIZE}px);
		}

		&:hover {
			opacity: 0.9;
		}

		&:active {
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

export default function ToggleSwitch({ on: [on, setOn], disabled, children }: FCP<{
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
}>) {
	const textLabel = on ? t.on : t.off;
	const [isDraging, setIsDraging] = useState(false);
	const [thumbLeft, setThumbLeft] = useState<number>();
	// 注意：直接使用 styled-components 的参数改变会影响性能。
	const thumbStyle = useMemo(() => thumbLeft === undefined ? undefined : {
		left: thumbLeft + "px",
		transition: "none",
	} as CSSProperties, [thumbLeft]);

	const handleCheck = (on: boolean, e?: MouseEvent) => {
		stopEvent(e);
		if (!isDraging) setOn?.(on);
		setIsDraging(false);
	};

	const onThumbDown = useCallback<PointerEventHandler<HTMLDivElement>>(e => {
		stopEvent(e);
		const thumb = e.currentTarget;
		const control = thumb.parentElement!;
		const controlRect = control.getBoundingClientRect();
		const left = controlRect.left, max = controlRect.width - THUMB_PRESSED_WIDTH;
		const x = e.pageX - left - thumb.offsetLeft;
		const firstTime = new Date().getTime();
		const pointerMove = (e: PointerEvent) => {
			setThumbLeft(clamp(e.pageX - left - x, 0, max));
		};
		const pointerUp = (e: PointerEvent) => {
			document.removeEventListener("pointermove", pointerMove);
			document.removeEventListener("pointerup", pointerUp);
			const isOn = e.pageX - x > left + max / 2;
			handleCheck(isOn);
			setThumbLeft(undefined);
			const lastTime = new Date().getTime();
			setIsDraging(lastTime - firstTime > 200); // 定义识别为拖动而不是点击的时间差。
		};
		document.addEventListener("pointermove", pointerMove);
		document.addEventListener("pointerup", pointerUp);
	}, []);

	return (
		<StyledToggleSwitchLabel
			className={{ active: on }}
			disabled={disabled}
			onClick={e => handleCheck(!on, e)}
		>
			<div className="text">{children}</div>
			<div className="right">
				<span className="text">{textLabel}</span>
				<div className="stroke">
					<div className="base">
						<div className="thumb" style={thumbStyle} onPointerDown={onThumbDown} />
					</div>
				</div>
			</div>
		</StyledToggleSwitchLabel>
	);
}
