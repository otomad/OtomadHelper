const StyledAnimatedIcon = styled.div`
	@layer props {
		--state: normal;
		--selected: false;
	}

	&,
	.icon-box,
	.lottie {
		display: inline-block;
		line-height: 0;
	}

	.icon-box {
		position: relative;
		animation: 1s infinite;

		@container style(--state: normal) {
			animation-name: Normal;
		}

		@container style(--state: pressed) {
			animation-name: Pressed;
		}

		@container style(--state: normal) and style(--selected: true) {
			animation-name: Selected;
		}

		@container style(--state: pressed) and style(--selected: true) {
			animation-name: PressedSelected;
		}

		.lottie {
			${styles.effects.text.icon};
			${styles.mixins.square("1em")};

			&:not(.filled) * {
				fill: currentColor;
				stroke: currentColor;
			}
		}
	}
`;

export default function AnimatedIcon({ loop = false, autoplay = false, name, hidden = false, speed = 1, state: _state = [], filled = false, onInit, onClick, onPress, onLift, ...htmlAttrs }: FCP<{
	/** 循环播放？ */
	loop?: boolean;
	/** 自动播放？ */
	autoplay?: boolean;
	/** 动画数据 JSON 或其文件名。 */
	name: object | string;
	/** 隐藏？ */
	hidden?: boolean;
	/** 播放速度。 */
	speed?: number;
	/** 状态信息。参数依次为：标记、循环、速度。 */
	state?: AnimatedIconState;
	/** 是否保持图标本身的颜色。 */
	filled?: boolean;
	/** 初始化事件。 */
	onInit?: (anim?: AnimationItem) => void;
	/** 点击事件。 */
	onClick?: (anim?: AnimationItem) => void;
	/** 按下事件。 */
	onPress?: (anim?: AnimationItem) => void;
	/** 弹起事件。 */
	onLift?: (anim?: AnimationItem) => void;
}, "div">) {
	const animationItem = useRef<AnimationItem>();
	const iconBox = useDomRef<HTMLDivElement>();

	/**
	 * 获取以文件名形式的图标。
	 */
	const animationData = useMemo<object>(() => {
		if (typeof name !== "string")
			return name;
		try {
			const iconsImport = import.meta.glob<string>("/src/assets/lotties/**/**.json", {
				as: "raw",
				eager: true,
			});
			const rawIcon = iconsImport[`/src/assets/lotties/${name}.json`];
			return JSON.parse(rawIcon);
		} catch (e) {
			// eslint-disable-next-line no-console
			console.error(`Lottie file '${name}' doesn't exist in 'assets/lotties'`, e);
		}
	}, [name]);

	/**
	 * 点击图标交互事件。
	 */
	const handleClick = useCallback(() => {
		if (!animationItem.current) return;
		onClick?.(animationItem.current);
	}, [animationItem]);

	const stop = useCallback(() => animationItem.current?.stop(), [animationItem]);
	const play = useCallback(() => animationItem.current?.play(), [animationItem]);
	const pause = useCallback(() => animationItem.current?.pause(), [animationItem]);
	const handleSpeedChange = useCallback(() => animationItem.current?.setSpeed(speed), [animationItem, speed]);

	/**
	 * 控制状态信息。
	 */
	function handleStateChange(state: AnimatedIconState) {
		const anim = animationItem.current;
		if (!anim) return;
		let marker: string | undefined, loop: boolean | undefined, speed: number | undefined;
		if (state instanceof Array)
			[marker, loop, speed] = state;
		else
			({ marker, loop, speed } = state);
		if (loop !== undefined) anim.loop = loop;
		if (speed) { // 在不为 0 时有效。
			anim.playSpeed = Math.abs(speed);
			anim.playDirection = Math.sign(speed);
		}
		if (!marker)
			if (speed === 0) anim.pause();
			else anim.play();
		else {
			console.log(marker);
			let markerItem = anim.markers.find(m => m.payload.name === marker);
			if (!markerItem)
				if (Object.is(speed, -0)) markerItem = anim.markers.find(m => m.payload.name.endsWith("To" + marker));
				else if (Object.is(speed, 0)) markerItem = anim.markers.find(m => m.payload.name.startsWith(marker + "To"));
			if (markerItem) {
				const marker = markerItem.payload.name;
				if (Object.is(speed, 0)) anim.goToAndStop(marker, true);
				else if (Object.is(speed, -0)) anim.goToAndStop(markerItem.time + markerItem.duration - 1, true);
				else anim.goToAndPlay(marker, true);
			}
			// TODO: 指定 fallback 机制。
		}
	}

	useEffect(() => handleSpeedChange(), [speed]);
	// useEffect(() => handleStateChange(state), [state]);

	const previousAnimationName = useRef(""); // TODO: 修改 AEP 之后稍后改成 "normal"。
	useEventListener(iconBox, "animationstart", e => {
		const [previous, current] = [previousAnimationName.current, e.animationName];
		if (previous) {
			if (!current || previous === current) return;
			handleStateChange({ marker: `${previous}To${current}`, speed: 1 });
		} else if (current)
			handleStateChange({ marker: `${current}`, speed: -0 });

		previousAnimationName.current = e.animationName;
	});

	/**
	 * 当 Lottie 动画完成加载后调用，用于获取 anim 对象。
	 * @param anim - anim 对象。
	 */
	function onAnimationCreated(anim: AnimationItem) {
		animationItem.current = anim;
		handleSpeedChange();
		// handleStateChange(_state);
		onInit?.(anim);
	}

	return (
		<StyledAnimatedIcon {...htmlAttrs}>
			<div ref={iconBox} className="icon-box" onClick={handleClick}>
				<Lottie
					className={{ filled }}
					loop={loop}
					autoplay={autoplay}
					animationData={animationData!}
					hidden={hidden}
					onAnimCreated={onAnimationCreated}
				/>
			</div>
		</StyledAnimatedIcon>
	);
}
