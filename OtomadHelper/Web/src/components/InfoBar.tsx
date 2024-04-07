import type { ColorNames } from "styles/colors";
const backgroundColors: Record<Status, ColorNames> = {
	neutual: "fill-color-system-neutral-background",
	accent: "background-fill-color-card-background-secondary",
	info: "background-fill-color-card-background-secondary",
	asterisk: "fill-color-system-attention-background",
	warning: "fill-color-system-caution-background",
	success: "fill-color-system-success-background",
	error: "fill-color-system-critical-background",
};

const StyledInfoBar = styled.div<{
	/** 角标的状态，即颜色。 */
	$status: Status;
}>`
	display: flex;
	column-gap: 13px;
	align-items: flex-start;
	padding: 13px 15px;
	overflow-x: clip;
	background-color: ${c("background-fill-color-card-background-secondary")};
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-radius: 3px;

	.title {
		${styles.effects.text.bodyStrong};
		${styles.mixins.hideIfEmpty()};
	}

	.text:is(:lang(zh), :lang(ja), :lang(ko)) {
		text-align: justify;
	}

	.text-part {
		display: flex;
		column-gap: inherit;
		width: 100%;

		.title,
		.text {
			display: inline-block;
			line-height: 20px;
		}

		.text {
			width: 100%;
			white-space: nowrap;
		}

		.buttons {
			${styles.mixins.hideIfEmpty()};
			display: flex;
			gap: 4px;
			margin-block: -6px;
			margin-inline-end: -4px;
			transition: ${fallbackTransitions}, margin 0s;
		}

		> :not(.text) {
			flex-shrink: 0;
		}
	}

	&.multiline .text-part {
		flex-direction: column;

		.text {
			white-space: normal;
		}

		.buttons {
			margin-block: 13px 0;
			margin-inline-end: 0;
		}
	}

	.badge {
		${styles.mixins.square("16px")};
		margin-top: ${(20 - 16) / 2}px;
	}

	${({ $status }) => css`
		background-color: ${c(backgroundColors[$status])};
	`}
`;

export default function InfoBar({ status, title, children, button, className, ...htmlAttrs }: FCP<{
	/** 角标的状态，颜色和图标。 */
	status?: Status;
	/** 标题。 */
	title?: string;
	/** 尾随按钮。 */
	button?: ReactNode;
}, "div">) {
	const [multiline, setMultiline] = useState(false);
	const infoBarEl = useDomRef<"div">();

	useMountEffect(() => {
		if (!infoBarEl.current) return;
		const observer = new ResizeObserver(([{ target }]) => {
			setMultiline(multiline => {
				const getMultiline = () => target.scrollWidth > target.clientWidth;
				if (!multiline) return getMultiline();

				target.classList.remove("multiline");
				const result = getMultiline();
				target.classList.add("multiline");
				return result;
			});
		});
		observer.observe(infoBarEl.current);
		return () => observer.disconnect();
	});

	return (
		<StyledInfoBar ref={infoBarEl} $status={status ?? "info"} className={[className, { multiline }]} {...htmlAttrs}>
			{status && <Badge status={status} />}
			<div className="text-part">
				<div className="title">{title}</div>
				<div className="text">{children}</div>
				<div className="buttons">{button}</div>
			</div>
		</StyledInfoBar>
	);
}
