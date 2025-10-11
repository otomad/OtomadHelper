import lottie from "lottie-web";

const LavContainer = styled.div.attrs({
	className: "lottie",
})`
	&,
	* {
		transition: if(
			${ifColorScheme.contrast}: none;
			else: color ${eases.easeOutMax} 100ms;
		);
	}
`;

export default function Lottie({ loop = false, autoplay = false, animationData, onAnimCreated, ...htmlAttrs }: FCP<{
	/** Loop? */
	loop?: boolean;
	/** Autoplay? */
	autoplay?: boolean;
	/** Animation resource data. */
	animationData: object;
	/** Animation creation completion event. */
	onAnimCreated?(anim: AnimationItem): void;
}, "div">) {
	const [, setAnim] = useState<AnimationItem>();
	const lavContainerEl = useDomRef<"div">();

	useMountEffect(() => {
		if (!lavContainerEl.current) return;

		const anim = lottie.loadAnimation({
			container: lavContainerEl.current,
			renderer: "svg",
			loop,
			autoplay,
			animationData,
		});

		const svgEl = lavContainerEl.current.firstElementChild;
		if (svgEl) {
			const luminanceMasks = svgEl.querySelectorAll<SVGUseElement>("[mask-type=luminance] use");
			for (const use of luminanceMasks) {
				const lottieElementId = use.href.animVal;
				svgEl.querySelector(lottieElementId)?.classList.add("luminance-mask");
			}
		}

		setAnim(anim);
		onAnimCreated?.(anim);

		return () => {
			// await delay(1000); // Wait for the UI transition duration.
			anim?.destroy();
			svgEl?.remove();
		};
	});

	return <LavContainer ref={lavContainerEl} {...htmlAttrs} />;
}
