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

export default function EmptyMessage({ icon, title, details, iconOff = false, spinAtBegin = false, children }: FCP<{
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
}>) {
	const { resetTransition } = usePageStore();
	const el = useDomRef<HTMLDivElement>();
	const IconEl = iconOff ? IconOff : Icon;
	useEffect(() => {
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
