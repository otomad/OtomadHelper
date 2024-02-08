import type { ColorNames } from "styles/colors";
type Status = "neutual" | "info" | "asterisk" | "warning" | "success" | "error";
const backgroundColors: Record<Status, ColorNames> = {
	neutual: "fill-color-system-solid-neutral-background",
	info: "fill-color-system-solid-neutral",
	asterisk: "fill-color-system-attention",
	warning: "fill-color-system-caution",
	success: "fill-color-system-success",
	error: "fill-color-system-critical",
};

const StyledBadge = styled.div<{
	/** 角标的状态，即颜色。 */
	$status: Status;
	/** 隐藏？ */
	$hidden?: boolean;
}>`
	${styles.mixins.oval()};
	height: 16px;
	min-width: 16px;
	background-color: ${c("fill-color-system-solid-neutral-background")};
	font-size: 12px;
	padding: 0 3px;
	flex-shrink: 0;
	text-align: center;
	scale: 0;

	&.appear,
	&.enter,
	&.enter-done {
		scale: 1;
	}

	${({ $status }) => css`
		background-color: ${c(backgroundColors[$status])};
		color: ${c($status === "neutual" ? "foreground-color" : "fill-color-text-on-accent-primary")};
	`}

	span {
		position: relative;
		top: -0.5px;
	}
`;

export default forwardRef(function Badge({ children, status = "info", hidden, ...htmlAttrs }: FCP<{
	/** 角标的状态，即颜色。 */
	status?: Status;
	/** 隐藏？ */
	hidden?: boolean;
}, "div">, ref: ForwardedRef<HTMLDivElement>) {
	return (
		<CssTransition in={!hidden} unmountOnExit appear>
			<StyledBadge $status={status} ref={ref} {...htmlAttrs}>
				<span>{children}</span>
			</StyledBadge>
		</CssTransition>
	);
});
