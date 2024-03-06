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
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	title?: ReactNode;
	/** 详细描述。 */
	details?: ReactNode;
	/** 在图标上画斜线？ */
	iconOff?: boolean;
	/** 开始时图标陀螺旋。 */
	spinAtBegin?: boolean;
	/** 无副作用？不会额外对视图中的样式作修改。 */
	noSideEffect?: boolean;
}>) {
	const { resetTransition } = usePageStore();
	const el = useDomRef<"div">();
	const IconEl = iconOff ? IconOff : Icon;
	useEffect(() => {
		if (noSideEffect) return;
		const container = el.current?.parentElement;
		if (container)
			for (const child of container.children)
				if (child instanceof HTMLElement && child !== el.current)
					child.style.animation = "none";
		resetTransition();
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
