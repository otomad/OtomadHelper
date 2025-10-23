const ICON_INITIAL_SIZE = 16;
const STROKE_DASHARRAY = 15 * Math.SQRT2;

const StyledIconOff = styled.div`
	display: inline-flex;

	> svg {
		--shadow-stroke-width: --light-dark(1, 1.5);
		${styles.mixins.square("1em")};
		fill: currentColor;
	}
`;

export default function IconOff({ name: _name }: FCP<{
	/** Icon. */
	name: DeclaredIcons;
}>) {
	const name = redirectIcon(_name);
	const comp = useDomRef<"div">();
	const [svgPath, setSvgPath] = useState("");
	const symbolId = getIconSymbolId(name);
	const ariaLabel = getIconAriaLabel(name) + " off";
	const maskId = useId();
	const [shouldDelayToShow, setShouldDelayToShow] = useState(false);

	useEffect(() => {
		const path = document.querySelector(symbolId)?.innerHTML;
		if (path) setSvgPath(path);
	}, [name, symbolId]);

	useEffect(() => {
		// Delay the slash animation if SwitchTransition hasn't translate to this page yet.
		const page = comp.current?.closest("main.page");
		if (!page) return;
		const isEntered = () => page.classList.containsAny("enter", "enter-active", "enter-done");
		if (isEntered()) return;
		// This will only trigger if the page transition is "forward" or "backward".
		setShouldDelayToShow(true);
		const observer = new MutationObserver(() => {
			if (isEntered()) {
				setShouldDelayToShow(false);
				observer.disconnect();
			}
		});
		observer.observe(page, { attributeFilter: ["class"] });
		return () => observer.disconnect();
	}, [comp]);

	if (shouldDelayToShow) return;

	return (
		<StyledIconOff ref={comp} role="img" aria-description={ariaLabel} aria-hidden>
			<svg width={ICON_INITIAL_SIZE} height={ICON_INITIAL_SIZE} viewBox={`0 0 ${ICON_INITIAL_SIZE} ${ICON_INITIAL_SIZE}`} xmlns="http://www.w3.org/2000/svg">
				<mask id={maskId}>
					<g fill="white" dangerouslySetInnerHTML={{ __html: svgPath }} />
					<g
						fill="none"
						strokeDasharray={STROKE_DASHARRAY}
						strokeDashoffset={STROKE_DASHARRAY}
						strokeLinecap="round"
						strokeLinejoin="round"
						strokeWidth={1}
					>
						<path stroke="black" d="M1.21 -0.21L16.21 14.79" strokeWidth="var(--shadow-stroke-width)" />
						<path stroke="white" d="M0.5 0.5L15.5 15.5" />
						<animate
							fill="freeze"
							attributeName="stroke-dashoffset"
							dur="250ms"
							values={`${STROKE_DASHARRAY}; 0`}
							calcMode="spline"
							keySplines="0.5 0 0 1"
							begin="100ms"
						/>
					</g>
				</mask>
				<rect width={ICON_INITIAL_SIZE} height={ICON_INITIAL_SIZE} fill="currentColor" mask={`url("#${maskId}")`} />
			</svg>
		</StyledIconOff>
	);
}

const corrections = Object.keys(import.meta.glob("./**/*", { base: "/src/assets/icons/off_slash_alt", eager: true }))
	.map(path => path.match(new RegExp("\\./(.*).svg"))?.[1]).toCompacted();

/**
 * Some icons will become strange while the off slash shown,
 * so correcting these special icons.
 * @param name - The icon name that to be redirected.
 * @returns Off slash alt icon name.
 */
function redirectIcon(name: string): DeclaredIcons {
	return (corrections.includes(name) ? `off_slash_alt/${name}` : name) as DeclaredIcons;
}
