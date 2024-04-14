import { STATUS_PREFIX } from "styles/fake-animations";

const StyledAnimatedIcon = styled.div<{
	/** 是否 `overflow: clip`？ */
	$clipped?: boolean;
}>`
	@layer props {
		--state: normal;
		--selected: false;
	}

	display: contents;

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
			${useLottieStatus.name("Normal")};
		}

		@container style(--state: pressed) {
			${useLottieStatus.name("Pressed")};
		}

		@container style(--state: normal) and style(--selected: true) {
			${useLottieStatus.name("Selected")};
		}

		@container style(--state: pressed) and style(--selected: true) {
			${useLottieStatus.name("PressedSelected")};
		}

		.lottie {
			${styles.effects.text.icon};
			${styles.mixins.square("1em")};

			&:not(.filled) {
				:not(.luminance-mask *) {
					fill: currentColor;
				}

				[stroke] {
					stroke: currentColor;
				}
			}


			svg > g {
				clip-path: none;
			}

			${({ $clipped }) => $clipped && css`
				svg,
				svg * {
					overflow: clip;
				}
			`}
		}
	}
`;

type LottieStateMarker = `${string}To${string}`;
type MarkerFromTo = Partial<{ from: string; to: string }> | undefined;

function useLottieSequence(animationItem: MutableRefObject<AnimationItem | undefined>) {
	const [sequence, setSequence] = useImmer<LottieStateMarker[]>([]);

	function findMarker(callback: string | ((name: string) => boolean)) {
		return animationItem.current?.markers.find(m =>
			typeof callback === "function" ? callback(m.payload.name) : m.payload.name === callback);
	}

	function getMarkerFromTo(marker: LottieStateMarker) {
		return marker.match(/(?<from>.*)To(?<to>[^a-z].*)/)?.groups as MarkerFromTo;
	}

	function push(...state: LottieStateMarker[]) {
		setSequence(sequence => {
			const isPaused = !sequence.length;
			sequence.push(...state);
			for (let index = sequence.length - 1; index >= 1; index--) {
				const state = sequence[index];
				const previouses = sequence.slice(0, index);
				const duplicateIndex = previouses.indexOf(state);
				if (duplicateIndex !== -1) {
					const deleteCount = index - duplicateIndex;
					sequence.splice(duplicateIndex, deleteCount);
					index = duplicateIndex;
				}
			}
			spliced: while (true) {
				let { length } = sequence;
				while (--length)
					for (let i = 0, j = length; j < sequence.length; i++, j++) {
						const first = getMarkerFromTo(sequence[i]), last = getMarkerFromTo(sequence[j]);
						if (!first?.from || !last?.to) continue;
						const marker: LottieStateMarker = `${first.from}To${last.to}`;
						if (findMarker(marker) && (i !== 0 || i === 0 && marker === sequence[i])) {
							sequence.splice(i, length, marker);
							break spliced;
						}
					}
				break;
			}

			const anim = animationItem.current;
			if (isPaused && anim && state[0])
				anim.goToAndPlay(state[0], true);
		});
	}

	function clearAll() {
		setSequence(sequence => void sequence.clearAll());
	}

	function shift() {
		setSequence(sequence => void sequence.shift());
	}

	function goToAndStop(state: LottieStateMarker | number, reversed: boolean = false) {
		clearAll();
		const anim = animationItem.current;
		if (!anim) return;
		if (typeof state === "string")
			if (!reversed) anim.goToAndStop(state);
			else {
				const markerItem = findMarker(state);
				if (markerItem)
					anim.goToAndStop(markerItem.time + markerItem.duration - 1);
			}
		else anim.goToAndStop(state);
	}

	function onAnimationComplete() {
		setSequence(sequence => {
			sequence.shift();
			const nextState = sequence[0];
			const anim = animationItem.current;
			if (nextState && anim)
				anim.goToAndPlay(nextState, true);
		});
	}

	return { sequence, findMarker, push, clearAll, shift, goToAndStop, onAnimationComplete };
}

export default forwardRef(function AnimatedIcon({ loop = false, autoplay = false, name, hidden = false, speed = 1, filled = false, onInit, onClick, ...htmlAttrs }: FCP<{
	/** 循环播放？ */
	loop?: boolean;
	/** 自动播放？ */
	autoplay?: boolean;
	/** 动画数据 JSON 或其文件名。 */
	name: object | DeclaredLotties;
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
	const iconBoxEl = useDomRef<"div">();
	const { findMarker, onAnimationComplete, ...sequence } = useLottieSequence(animationItem);

	/**
	 * 获取以文件名形式的图标。
	 */
	const animationData = useMemo<AnyObject>(() => {
		if (typeof name !== "string")
			return name;
		try {
			const iconsImport = import.meta.glob<string>("/src/assets/lotties/**/*.json", { query: "?raw", import: "default", eager: true });
			const rawIcon = iconsImport[`/src/assets/lotties/${name}.json`];
			return JSON.parse(rawIcon);
		} catch (e) {
			console.error(`Lottie file "${name}" doesn't exist in "assets/lotties"`, e);
		}
	}, [name]);

	const clipped = useMemo(() => !!animationData.metadata?.customProps?.clipped, [name]);

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
				const marker = markerItem.payload.name as LottieStateMarker;
				if (speed === 0) sequence.goToAndStop(marker, Object.is(speed, -0));
				else sequence.push(marker);
			}
			if (!markerItem)
				if (marker === "NormalToSelected")
					sequence.push("NormalToPressed", "PressedToSelected");
				else if (marker === "PressedSelectedToNormal")
					sequence.push("PressedSelectedToSelected", "SelectedToNormal");
				else if (marker === "PressedToPressedSelected" && findMarker("PressedSelectedToSelected"))
					sequence.goToAndStop("PressedSelectedToSelected");
				else if (marker === "PressedSelectedToPressed" && findMarker("PressedToNormal"))
					sequence.goToAndStop("PressedToNormal");
				else {
					markerItem = findMarker(m => m.startsWith(marker + "To"));
					if (markerItem)
						sequence.goToAndStop(markerItem.payload.name as LottieStateMarker);
				}
		}
	}

	useEffect(() => handleSpeedChange(), [speed]);

	const previousAnimationName = useRef("Normal");
	useEventListener(iconBoxEl, "animationstart", e => {
		let [previous, current] = [previousAnimationName.current, e.animationName];
		if (!current.startsWith(STATUS_PREFIX)) return;
		current = current.replace(STATUS_PREFIX, "");
		if (!current || previous === current) return;
		handleStateChange({ marker: `${previous}To${current}`, speed: 1 });

		previousAnimationName.current = current;
	});

	/**
	 * 当 Lottie 动画完成加载后调用，用于获取 anim 对象。
	 * @param anim - anim 对象。
	 */
	function onAnimationCreated(anim: AnimationItem) {
		animationItem.current = anim;
		anim.addEventListener("complete", onAnimationComplete);
		handleStateChange({ marker: previousAnimationName.current, speed: 0 });
		handleSpeedChange();
		onInit?.(anim);
	}

	useImperativeHandle(ref, () => ({
		play, pause, stop,
	}), []);

	return (
		<StyledAnimatedIcon $clipped={clipped} {...htmlAttrs}>
			<div ref={iconBoxEl} className="icon-box" onClick={handleClick}>
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
