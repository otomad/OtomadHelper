import lottie from "lottie-web";

const LavContainer = styled.div.attrs({
	className: "lottie",
})`
	&,
	* {
		transition: color ${eases.easeOutMax} 100ms;
	}
`;

export default function Lottie({ loop = false, autoplay = false, animationData, onAnimCreated, ...htmlAttrs }: FCP<{
	/** 循环播放？ */
	loop?: boolean;
	/** 自动播放？ */
	autoplay?: boolean;
	/** 动画资源。 */
	animationData: object;
	/** 动画创建完成事件。 */
	onAnimCreated?: (anim: AnimationItem) => void;
}, "div">) {
	const [anim, setAnim] = useState<AnimationItem>();
	const lavContainerEl = useDomRef<HTMLDivElement>();

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
	});

	useUnmountEffect(async () => {
		await delay(1000); // 等待界面过渡动画时间。
		anim?.destroy();
	});

	return (
		<LavContainer ref={lavContainerEl} {...htmlAttrs} />
	);
}
