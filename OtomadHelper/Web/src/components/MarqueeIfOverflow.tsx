const StyledMarqueeIfOverflow = styled.div`
	${styles.mixins.noScrollbar()};
	--h-space: 1em;
	container: marquee-if-overflow / inline-size scroll-state;
	position: relative;
	overflow-inline: scroll;
	overflow-anchor: none;

	&,
	.info-bar > .text-part > :is(.title, .text):has(&) {
		flex-shrink: 1;
		inline-size: stretch;
	}

	p,
	.marquee {
		display: inline-block;
		white-space: nowrap;
	}

	.marquee {
		position: absolute;
	}

	.marquee p:nth-child(1) {
		margin-inline-end: var(--h-space);
	}

	@container marquee-if-overflow not scroll-state(scrollable: none) {
		> p {
			visibility: hidden;
		}

		.marquee {
			animation: ${keyframes`
				from {
					translate: 0;
				}

				to {
					translate: calc(-50% - var(--h-space) / 2);
				}
			`} 5s linear infinite;
			animation-duration: calc(var(--width) * 10ms);
		}
	}

	@container marquee-if-overflow scroll-state(scrollable: none) {
		.marquee {
			display: none;
		}
	}
`;

/**
 * Make marquee text only when it's overflowing.
 * @returns React JSX element.
 */
export default function MarqueeIfOverflow({ children, ...htmlAttrs }: FCP<{}, "div">) {
	const plainEl = useDomRef<"p">();
	const width = useElementSize(plainEl, "borderBoxInlineSize");

	return (
		<StyledMarqueeIfOverflow inert {...htmlAttrs}>
			<div className="marquee" style={{ "--width": width }}>
				<p>{children}</p>
				<p>{children}</p>
			</div>
			<p ref={plainEl}>{children}</p>
		</StyledMarqueeIfOverflow>
	);
}
