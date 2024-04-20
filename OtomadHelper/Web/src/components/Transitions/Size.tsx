import type { TransitionProps } from "react-transition-group";
import type { AnimateSizeOptions, SameOrDifferent } from "utils/animation";

export /* internal */ default function Size({ in: shown, specified, duration, easing, enterOptions, exitOptions, unmountOnExit = true, children, ...transitionAttrs }: FCP<{
	/** Show the children? */
	in?: boolean;
	/** Explicitly specify which direction needs to be animated. Defaults to height animation. */
	specified?: "width" | "height";
	/** Specify the animation duration. */
	duration?: SameOrDifferent<number>;
	/** Specify the animation easing curve. */
	easing?: SameOrDifferent<string>;
	/** Specify other parameters when entering animation. */
	enterOptions?: AnimateSizeOptions;
	/** Specify other parameters when exiting animation. */
	exitOptions?: AnimateSizeOptions;
}> & TransitionProps) {
	const [onEnter, onExit, endListener] = simpleAnimateSize(specified, duration, easing, enterOptions, exitOptions);

	return (
		<Transition
			{...transitionAttrs}
			in={shown}
			addEndListener={endListener}
			onEnter={onEnter}
			onExit={onExit}
			unmountOnExit={unmountOnExit}
		>
			{children}
		</Transition>
	);
}
