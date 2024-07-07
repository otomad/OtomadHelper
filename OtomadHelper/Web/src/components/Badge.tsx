import type { ColorNames } from "styles/colors";
const backgroundColors: Record<Status, ColorNames> = {
	neutual: "fill-color-system-solid-neutral-background",
	accent: "accent-color",
	info: "fill-color-system-solid-neutral",
	asterisk: "fill-color-system-attention",
	warning: "fill-color-system-caution",
	success: "fill-color-system-success",
	error: "fill-color-system-critical",
};

const StyledBadge = styled.div<{
	/** The state of the badge, that is, the color. */
	$status: Status;
	/** Hidden? */
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
	transition: ${fallbackTransitions}, scale ${eases.easeOutBackSmooth} 250ms;

	&.icon-only {
		width: 16px;
	}

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

export default forwardRef(function Badge({ children, status = "info", hidden, transitionOnAppear = true, ...htmlAttrs }: FCP<{
	/** The state of the badge, that is, the color. */
	status?: Status;
	/** Hidden? */
	hidden?: boolean;
	/** Play transition when the badge is appeared? */
	transitionOnAppear?: boolean;
}, "div">, ref: ForwardedRef<"div">) {
	const iconName = `badge/${["neutual", "accent"].includes(status) ? "info" : status}`;
	return (
		<CssTransition in={!hidden} unmountOnExit appear={transitionOnAppear}>
			<StyledBadge $status={status} ref={ref} className={{ iconOnly: !children }} {...htmlAttrs}>
				{children != null ? <span>{children}</span> : <Icon name={iconName} />}
			</StyledBadge>
		</CssTransition>
	);
});
