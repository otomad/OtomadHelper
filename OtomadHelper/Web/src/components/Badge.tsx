import type { ColorNames } from "styles/colors";
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
	${styles.mixins.flexCenter()};
	${styles.effects.text.caption};
	display: inline-flex;
	flex-shrink: 0;
	min-width: 16px;
	height: 16px;
	padding: 0 3px;
	text-align: center;
	background-color: ${c("fill-color-system-solid-neutral-background")};
	scale: 1;

	${tgs()} {
		scale: 0;
	}

	${({ $status }) => css`
		color: ${c($status === "neutual" ? "foreground-color" : "fill-color-text-on-accent-primary")};
		background-color: ${c(backgroundColors[$status])};
	`}

	span {
		position: relative;
		top: -0.5px;
	}

	.icon {
		font-size: 12px;
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
				{children ? <span>{children}</span> : <Icon name={"badge/" + (status === "neutual" ? "info" : status)} />}
			</StyledBadge>
		</CssTransition>
	);
});
