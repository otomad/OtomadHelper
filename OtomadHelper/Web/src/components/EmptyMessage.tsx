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
		font-weight: 600;
		font-size: 28px;
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
	const debounceTimeout = useRef<Timeout>();
	const debouncing = useRef(false);
	const { resetTransition } = usePageStore();
	useEffect(() => {
		debouncing.current = true;
		debounceTimeout.current = setTimeout(() => debouncing.current = false, 100);
		return () => {
			if (!debouncing.current) {
				console.log(456);
				resetTransition();
			}
			clearTimeout(debounceTimeout.current);
			debouncing.current = false;
		};
	}, []);

	return (
		<StyledEmptyMessage>
			{icon && <Icon name={icon} />}
			<h2>{heading}</h2>
			<p>{caption}</p>
			{children}
		</StyledEmptyMessage>
	);
}
