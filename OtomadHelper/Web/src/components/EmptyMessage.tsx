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

	> .icon {
		font-size: 48px;
	}

	> h2 {
		${styles.effects.text.title};
	}

	> p {
		color: ${c("fill-color-text-tertiary")}
	}
`;

export default function EmptyMessage({ icon, heading, caption, children }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	heading?: ReactNode;
	/** 详细描述。 */
	caption?: ReactNode;
}>) {
	const { resetTransition } = usePageStore();
	const el = useRef<HTMLDivElement | null>(null);
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
			{icon && <Icon name={icon} />}
			<h2>{heading}</h2>
			<p>{caption}</p>
			{children}
		</StyledEmptyMessage>
	);
}
