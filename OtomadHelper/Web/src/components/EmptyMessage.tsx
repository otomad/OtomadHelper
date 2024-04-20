const scaleIn = keyframes`
	from {
		scale: 0.9;
		opacity: 0;
	}
`;

const spin = keyframes`
	from {
		rotate: -1turn;
	}
`;

const StyledEmptyMessage = styled.div`
	${styles.mixins.flexCenter()}
	flex-direction: column;
	gap: 12px;
	height: 100%;
	text-align: center;
	animation-name: ${scaleIn} !important;

	> .icon,
	> .icon-off {
		font-size: 48px;

		&.spin {
			animation: ${spin} 750ms ${eases.easeOutSmooth};
		}
	}

	> header {
		> h2 {
			${styles.effects.text.title};
			text-wrap: balance;

			+ p {
				margin-top: 5px;
			}
		}

		> p {
			${styles.effects.text.custom(14, 20, "regular", "small")};
			color: ${c("fill-color-text-tertiary")}
		}
	}
`;

export default function EmptyMessage({ icon, title, details, iconOff = false, spinAtBegin = false, noSideEffect = false, children }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Draw a slash on the icon? */
	iconOff?: boolean;
	/** Spinning the icon at the beginning. */
	spinAtBegin?: boolean;
	/** No side effects? No additional changes are made to the styles in the view. */
	noSideEffect?: boolean;
}>) {
	const el = useDomRef<"div">();
	const IconEl = iconOff ? IconOff : Icon;
	useEffect(() => {
		if (noSideEffect) return;
		const container = el.current?.parentElement;
		if (container)
			for (const child of container.children)
				if (child instanceof HTMLElement && child !== el.current)
					child.style.animation = "none";
	}, []);

	return (
		<StyledEmptyMessage ref={el}>
			{icon && <IconEl className={{ spin: spinAtBegin }} name={icon} />}
			<header>
				<h2>{title}</h2>
				<p>{details}</p>
			</header>
			{children}
		</StyledEmptyMessage>
	);
}
