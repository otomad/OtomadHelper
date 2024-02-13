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

class InterruptLottieError extends Error {
	constructor() {
		super("Lottie animation playing has been interrupted");
		this.name = this.constructor.name;
	}
}

export default forwardRef(function AnimatedIcon({ loop = false, autoplay = false, name, hidden = false, speed = 1, filled = false, onInit, onClick, ...htmlAttrs }: FCP<{
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
	// state?: AnimatedIconState;
	/** 是否保持图标本身的颜色。 */
	filled?: boolean;
	/** 初始化事件。 */
	onInit?: (anim?: AnimationItem) => void;
	/** 点击事件。 */
	onClick?: (anim?: AnimationItem) => void;
}, "div">, ref: ForwardedRef<{
	play: () => void;
	pause: () => void;
	stop: () => void;
}>) {
	const animationItem = useRef<AnimationItem>();
	const iconBox = useDomRef<HTMLDivElement>();
	const markerSequenceRejects = useRef<(() => void)[]>([]);

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
	const handleClick = () => {
		if (!animationItem.current) return;
		onClick?.(animationItem.current);
	};

	const stop = () => animationItem.current?.stop();
	const play = () => animationItem.current?.play();
	const pause = () => animationItem.current?.pause();
	const handleSpeedChange = () => animationItem.current?.setSpeed(speed);
	const findMarker = (callback: string | ((name: string) => boolean)) =>
		animationItem.current?.markers.find(m =>
			typeof callback === "function" ? callback(m.payload.name) : m.payload.name === callback);

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
			let markerItem = findMarker(marker);
			if (!markerItem)
				if (Object.is(speed, -0)) markerItem = findMarker(m => m.endsWith("To" + marker));
				else if (Object.is(speed, 0)) markerItem = findMarker(m => m.startsWith(marker + "To"));
			if (markerItem) {
				markerSequenceRejects.current.forEach(reject => reject());
				markerSequenceRejects.current = [];
				const marker = markerItem.payload.name;
				if (Object.is(speed, 0)) anim.goToAndStop(marker, true);
				else if (Object.is(speed, -0)) anim.goToAndStop(markerItem.time + markerItem.duration, true);
				else anim.goToAndPlay(marker, true);
			}
			if (!markerItem)
				if (marker === "NormalToSelected")
					playMarkerSequence("NormalToPressed", "PressedToSelected");
				else if (marker === "PressedSelectedToNormal")
					playMarkerSequence("PressedSelectedToSelected", "SelectedToNormal");
				else if (marker === "PressedToPressedSelected" && findMarker("PressedSelectedToSelected"))
					anim.goToAndStop("PressedSelectedToSelected", true);
				else if (marker === "PressedSelectedToPressed" && findMarker("PressedToNormal"))
					anim.goToAndStop("PressedToNormal", true);
		}
	}

	async function playMarkerSequence(...markers: string[]) {
		const anim = animationItem.current;
		if (!anim) return;
		markers = markers.filter(m => findMarker(m));
		for (const marker of markers)
			try {
				await new Promise<void>((resolve, reject) => {
					const interrupt = () => reject(new InterruptLottieError());
					const onComplete = () => {
						anim.removeEventListener("complete", onComplete);
						arrayRemoveItem(markerSequenceRejects.current, interrupt);
						resolve();
					};
					anim.addEventListener("complete", onComplete);
					anim.goToAndPlay(marker, true);
					markerSequenceRejects.current.push(interrupt);
				});
			} catch (error) {
				if (error instanceof InterruptLottieError) break;
				else throw error;
			}
	}

	useEffect(() => handleSpeedChange(), [speed]);

	const previousAnimationName = useRef("Normal");
	useEventListener(iconBox, "animationstart", e => {
		const [previous, current] = [previousAnimationName.current, e.animationName];
		if (!current || previous === current) return;
		handleStateChange({ marker: `${previous}To${current}`, speed: 1 });

		previousAnimationName.current = e.animationName;
	});

	/**
	 * 当 Lottie 动画完成加载后调用，用于获取 anim 对象。
	 * @param anim - anim 对象。
	 */
	function onAnimationCreated(anim: AnimationItem) {
		animationItem.current = anim;
		handleStateChange({ marker: previousAnimationName.current, speed: -0 });
		handleSpeedChange();
		onInit?.(anim);
	}

	useImperativeHandle(ref, () => ({
		play, pause, stop,
	}), []);

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
});
