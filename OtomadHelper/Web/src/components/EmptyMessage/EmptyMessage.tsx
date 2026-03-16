import EmptyMessageMini from "./EmptyMessageMini";
import EmptyMessageTypical from "./EmptyMessageTypical";
import EmptyMessageYtpDisabled from "./EmptyMessageYtpDisabled";

const ICON_SIZE = 48;

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
	padding-block: 30px;
	text-align: center;
	animation-name: ${scaleIn} !important;

	> .icon,
	> .icon-off {
		font-size: ${ICON_SIZE}px;

		&.spin {
			animation: ${spin} 750ms ${eases.easeOutSmooth};
		}
	}

	> header {
		> * {
			text-wrap: balance;
		}

		> h2 {
			${styles.text.title};

			+ p {
				margin-top: 5px;
			}
		}

		> p {
			${styles.text.custom(14, 20, "regular", "small")};
			color: ${c("fill-color-text-tertiary")};
		}
	}
`;

export default function EmptyMessage({ icon, title, details, iconOff = false, spinAtBegin = false, noSideEffect = false, children, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons | ReactElement;
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
}, "div">) {
	const el = useDomRef<"div">();
	const IconEl = iconOff ? IconOff : Icon;
	useEffect(() => {
		if (noSideEffect) return;
		const container = el.current?.parentElement;
		if (container)
			for (const child of container.children)
				if (child instanceof HTMLElement && child !== el.current)
					child.style.animation = "none";
	}, [el, noSideEffect]);
	if (isObject(icon) && "props" in icon)
		icon = React.cloneElement(icon as never, { width: ICON_SIZE, height: ICON_SIZE });

	return (
		<StyledEmptyMessage ref={el} {...htmlAttrs}>
			{icon && (typeof icon === "string" ? <IconEl className={{ spin: spinAtBegin }} name={icon} /> : icon)}
			<header>
				<h2>{title}</h2>
				<p>{details}</p>
			</header>
			{children}
		</StyledEmptyMessage>
	);
}

EmptyMessage.Typical = EmptyMessageTypical;
EmptyMessage.YtpDisabled = EmptyMessageYtpDisabled;
EmptyMessage.Mini = EmptyMessageMini;
