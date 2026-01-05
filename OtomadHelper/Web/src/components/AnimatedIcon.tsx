import { STATUS_PREFIX } from "styles/fake-animations";

const StyledAnimatedIcon = styled.div<{
	/** Should `overflow: clip`? */
	$clipped?: boolean;
}>`
	@layer props {
		--state: normal;
		--selected: false;
	}

	${styles.effects.text.icon};
	display: contents;

	&,
	.icon-box,
	.lottie {
		display: inline-block;
		line-height: 0;
	}

	.icon-box,
	.lottie {
		font-size: inherit;
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
			${styles.mixins.square("1em", false, true)};

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

function useLottieSequence(animationItem: RefObject<AnimationItem | undefined>, actions: LottieInternalActions) {
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
			const isPaused = sequence.length === 0;
			sequence.push(...state);
			for (let index = sequence.length - 1; index >= 1; index--) {
				const state = sequence[index];
				const previousStates = sequence.slice(0, index);
				const duplicateIndex = previousStates.indexOf(state);
				if (~duplicateIndex) {
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
				actions.goToAndPlay(state[0], true);
		});
	}

	function clearAll() {
		setSequence(sequence => { sequence.clearAll(); });
	}

	function shift() {
		setSequence(sequence => { sequence.shift(); });
	}

	function goToAndStop(state: LottieStateMarker | number, reversed: boolean = false) {
		clearAll();
		const anim = animationItem.current;
		if (!anim) return;
		if (typeof state === "string")
			if (!reversed) actions.goToAndStop(state);
			else {
				const markerItem = findMarker(state);
				if (markerItem)
					actions.goToAndStop(markerItem.time + markerItem.duration - 1);
			}
		else actions.goToAndStop(state);
	}

	function onAnimationComplete() {
		setSequence(sequence => {
			sequence.shift();
			const nextState = sequence[0];
			const anim = animationItem.current;
			if (nextState && anim)
				actions.goToAndPlay(nextState, true);
		});
		actions.setIsPlaying(false);
	}

	return { sequence, findMarker, push, clearAll, shift, goToAndStop, onAnimationComplete };
}

interface LottieActions {
	play(): void;
	pause(): void;
	stop(): void;
	goToAndPlay: AnimationItem["goToAndPlay"];
	goToAndStop: AnimationItem["goToAndStop"];
}

interface LottieInternalActions extends Pick<LottieActions, "goToAndPlay" | "goToAndStop"> {
	setIsPlaying: SetState<boolean>;
}

const iconsImport = import.meta.glob<AnyObject>("./**/*.json", { base: "/src/assets/lotties", import: "default", eager: true, query: "?lottie" });

export default function AnimatedIcon({
	loop = false,
	autoplay = false,
	name,
	hidden = false,
	speed = 1,
	filled = false,
	showFallbackIcon = false,
	onInit,
	onClick,
	onPlayStateChange,
	ref,
	...htmlAttrs
}: FCP<{
	/** Loop? */
	loop?: boolean;
	/** Autoplay? */
	autoplay?: boolean;
	/** Animation data JSON or its filename. */
	name: object | DeclaredLotties;
	/** Hidden? */
	hidden?: boolean;
	/** Play speed. */
	speed?: number;
	/** Status information. Parameters are: marker, loop, speed. */
	// state?: AnimatedIconState;
	/** Keep the color of the icon itself? */
	filled?: boolean;
	/**
	 * If the specified animated icon doesn't exist, then replace it with a static icon with the same icon name.\
	 * You can also manually specify the fallback icon name.
	 */
	showFallbackIcon?: boolean | DeclaredIcons;
	/** Initialization event. */
	onInit?(anim?: AnimationItem): void;
	/** Click event. */
	onClick?(anim?: AnimationItem): void;
	/** Occurs when the animation played, paused, or stopped. */
	onPlayStateChange?(isPlaying: boolean): void;
	children?: never;
	ref?: ForwardedRef<LottieActions>;
}, "div">) {
	const animationItem = useRef<AnimationItem>(undefined);
	const [isPlaying, setIsPlaying] = useState(false);

	const stop = () => { animationItem.current?.stop(); setIsPlaying(false); };
	const play = () => { animationItem.current?.play(); setIsPlaying(true); };
	const pause = () => { animationItem.current?.pause(); setIsPlaying(false); };
	const goToAndStop: AnimationItem["goToAndStop"] = (...args) => { animationItem.current?.goToAndStop(...args); setIsPlaying(false); };
	const goToAndPlay: AnimationItem["goToAndPlay"] = (...args) => { animationItem.current?.goToAndPlay(...args); setIsPlaying(true); };
	const handleSpeedChange = () => animationItem.current?.setSpeed(speed);

	useImperativeHandle(ref, () => ({ play, pause, stop, goToAndPlay, goToAndStop }), []);

	const { findMarker, onAnimationComplete, ...sequence } = useLottieSequence(animationItem, { goToAndPlay, goToAndStop, setIsPlaying });

	useEffect(() => { onPlayStateChange?.(isPlaying); }, [isPlaying, onPlayStateChange]);

	/**
	 * Gets the icon as a filename.
	 */
	const animationData = useMemo<AnyObject>(() => {
		if (typeof name !== "string")
			return name;
		try {
			const rawIcon = iconsImport[`./${name}.json`];
			return rawIcon;
		} catch (cause) {
			if (!showFallbackIcon)
				console.error(new Error(`Lottie file "${name}" doesn't exist in "assets/lotties"`, { cause }));
			return null!;
		}
	}, [name, showFallbackIcon]);

	if (!isObject(animationData) && showFallbackIcon) {
		const iconName =
			showFallbackIcon === true && typeof name === "string" ? name :
			typeof showFallbackIcon === "string" ? showFallbackIcon : null;
		if (iconName)
			return <Icon name={iconName} hidden={hidden} filled={filled} {...htmlAttrs} />;
		else
			return;
	}

	const clipped = useMemo(() => !!animationData?.metadata?.customProps?.clipped, [name]);

	/**
	 * Click icon interaction event.
	 */
	const handleClick = () => {
		if (!animationItem.current) return;
		onClick?.(animationItem.current);
	};

	/**
	 * Control status information.
	 * @param state - Animated icon state (tuple or object form).
	 */
	function handleStateChange(state: AnimatedIconState) {
		const anim = animationItem.current;
		if (!anim) return;
		let marker: string | undefined, loop: boolean | undefined, speed: number | undefined;
		if (Array.isArray(state))
			[marker, loop, speed] = state;
		else
			({ marker, loop, speed } = state);
		if (loop !== undefined) anim.loop = loop;
		if (speed) { // Valid when not 0.
			anim.playSpeed = Math.abs(speed);
			anim.playDirection = Math.sign(speed);
		}
		if (!marker)
			if (speed === 0) pause();
			else play();
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

	useEffect(() => handleSpeedChange(), [speed, handleSpeedChange]);

	const previousAnimationName = useRef("Normal");
	const onAnimationStart = useCallback<AnimationEventHandler>(e => {
		let [previous, current] = [previousAnimationName.current, e.animationName];
		if (!current.startsWith(STATUS_PREFIX)) return;
		current = current.replace(STATUS_PREFIX, "");
		if (!current || previous === current) return;
		handleStateChange({ marker: `${previous}To${current}`, speed: 1 });

		previousAnimationName.current = current;
	}, []);

	/**
	 * Called when the Lottie animation completes loading, used to get the `anim` object.
	 * @param anim - `anim` object.
	 */
	function onAnimationCreated(anim: AnimationItem) {
		animationItem.current = anim;
		anim.addEventListener("complete", onAnimationComplete);
		handleStateChange({ marker: previousAnimationName.current, speed: 0 });
		handleSpeedChange();
		onInit?.(anim);
	}

	return (
		<StyledAnimatedIcon $clipped={clipped} {...htmlAttrs}>
			<EventInjector onAnimationStart={onAnimationStart}>
				<div className="icon-box" aria-hidden onClick={handleClick}>
					<Lottie
						className={{ filled }}
						loop={loop}
						autoplay={autoplay}
						animationData={animationData}
						hidden={hidden}
						onAnimCreated={onAnimationCreated}
					/>
				</div>
			</EventInjector>
		</StyledAnimatedIcon>
	);
}
