const scaleIn = keyframes`
	from {
		scale: 0.9;
		opacity: 0;
	}
`;

const StyledEmptyMessage = styled.div`
	${styles.mixins.flexCenter()}
	flex-direction: column;
	gap: 12px;
	height: 100%;
	animation-name: ${scaleIn} !important;
	text-align: center;

	> .icon,
	> .icon-off {
		font-size: 48px;
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

export default function EmptyMessage({ icon, title, details, iconOff = false, children }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	title?: ReactNode;
	/** 详细描述。 */
	details?: ReactNode;
	/** 在图标上画斜线？ */
	iconOff?: boolean;
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
			{icon && <IconEl name={icon} />}
			<header>
				<h2>{title}</h2>
				<p>{details}</p>
			</header>
			{children}
		</StyledEmptyMessage>
	);
}
