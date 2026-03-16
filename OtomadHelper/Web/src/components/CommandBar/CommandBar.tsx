import type { TransitionGroupChildFactory } from "react-transition-group-fc";
import CommandBarGroup from "./CommandBarGroup";
import { CommandBarItem } from "./CommandBarItem";

const StyledCommandBar = styled.div`
	@layer props {
		--icon-only: false;
	}

	${styles.text.body};
	position: relative;
	display: flex;
	align-items: center;
	block-size: 100%;
	padding: 4px;
	background-color: ${c("background-fill-color-acrylic-background-command-bar")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 6px;
	backdrop-filter: blur(10px);

	${styles.effects.refreshedBackdropIfHasBackgroundImage};

	* {
		white-space: nowrap;
	}

	.command-bar-item {
		transition: all ${eases.easeInOutFluent} 500ms;

		.button {
			transition: ${fallbackTransitions}, min-inline-size 0s;

			@container style(--icon-only: true) or style(--too-narrow: true) {
				min-inline-size: unset;

				span {
					display: none;
				}
			}
		}

		&.enter,
		&.exit {
			overflow: hidden;

			.badge {
				scale: 0;
			}
		}

		${tgs()} {
			inline-size: 0;
			scale: 0.5;
			opacity: 0;
		}
	}

	&.gaps .command-bar-item:not(:last-child, :has(+ hr)) {
		margin-inline-end: 1px;
	}

	@container page-scroll scroll-state(scrollable: top) {
		background-color: ${c("background-fill-color-acrylic-background-default")};
		box-shadow: 0 8px 16px ${c("shadows-flyout")};
	}

	hr {
		display: inline-block;
		align-self: stretch;
		block-size: auto;
		inline-size: 1px;
		margin-block: 4px;
		margin-inline: 1px;
		border: none;
		border-inline-start: 1px solid ${c("stroke-color-divider-stroke-default")};
	}

	&.shadow {
		position: absolute;
		visibility: hidden;
	}
`;

export /* @internal */ const CommandBarAnchorContext = createContext<{
	anchorName: string;
	position?: Position;
	tooNarrow?: boolean;
}>(null!);

type Position = "left" | "center" | "right";

export default function CommandBar({ position, autoCollapse, addGaps, children, className, style, disabled, ...htmlAttrs }: FCP<{
	/** Position the command bar to somewhere. */
	position?: Position;
	/** Auto collapse command bar if too narrow. */
	autoCollapse?: boolean;
	/** Add gaps between each command bar items? */
	addGaps?: boolean;
}, "div">) {
	const [commandBarEl, setCommandBarEl] = useDomRefs<"div">();
	const anchorName = "--command-bar" + useId();
	const childFactory: TransitionGroupChildFactory = child => child.type === "hr" ? <hr key={child.key} /> : child;
	const overflowed = useIsCommandBarOverflowed(commandBarEl.current[1]);

	return forMap(autoCollapse ? 2 : 1, i => (
		<StyledCommandBar
			key={i}
			ref={setCommandBarEl(i)}
			role="toolbar"
			aria-label={t.aria.commandBar}
			className={[className, position, { shadow: i !== 0, gaps: addGaps }]}
			style={{ ...style, anchorName }}
			disabled={disabled}
			aria-disabled={disabled}
			{...htmlAttrs}
		>
			<InteractionStateContext value={{ disabled }}>
				<CommandBarAnchorContext value={i === 0 ? { anchorName, position, tooNarrow: autoCollapse && overflowed } : {} as never}>
					<TransitionGroup childFactory={childFactory} component={null}>
						{children}
					</TransitionGroup>
				</CommandBarAnchorContext>
			</InteractionStateContext>
		</StyledCommandBar>
	));
}

CommandBar.Item = CommandBarItem;
CommandBar.Group = CommandBarGroup;

function useIsCommandBarOverflowed(element: MaybeRef<HTMLElement | null>) {
	const [overflowed, setOverflowed] = useState(false);
	const forceUpdate = useForceUpdate();
	const elementNotFoundTimeoutId = useRef<Timeout>(undefined);

	useEffect(() => {
		clearTimeout(elementNotFoundTimeoutId.current);
		const el = toValue(element);
		if (!el) {
			elementNotFoundTimeoutId.current = setTimeout(() => forceUpdate(), 100);
			return;
		}

		const page = el.closest("main.page");
		if (page) { // In special circumstances of laziness, determine whether it is in the main page.
			const determine = () => {
				const { left, right } = el.getBoundingClientRect();
				const { left: pageLeft, right: pageRight } = page.getBoundingClientRect();
				setOverflowed(left < pageLeft || right > pageRight);
			};
			const observer = new MutationObserver(determine);
			observer.observe(page, { attributeFilter: ["class"] });
			window.addEventListener("resize", determine);
			determine();
			return () => {
				observer.disconnect();
				window.removeEventListener("resize", determine);
			};
		} else {
			const observer = new IntersectionObserver(
				([e]) => setOverflowed(e.intersectionRatio < 1),
				{
					// rootMargin: "-1px 0px 0px 0px",
					threshold: [1],
				},
			);
			observer.observe(el);
			return () => observer.disconnect();
		}
	}, [element]);

	return overflowed;
}
