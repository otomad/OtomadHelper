import type { ColorNames } from "styles/colors";
type InfoBarStatus = Exclude<Status, "neutual">;
const backgroundColors: Record<InfoBarStatus, ColorNames> = {
	info: "background-fill-color-card-background-secondary",
	asterisk: "fill-color-system-attention-background",
	warning: "fill-color-system-caution-background",
	success: "fill-color-system-success-background",
	error: "fill-color-system-critical-background",
};

const StyledInfoBar = styled.div<{
	/** 角标的状态，即颜色。 */
	$status: InfoBarStatus;
}>`
	display: flex;
	gap: 13px;
	align-items: flex-start;
	padding: 14px 13px;
	background-color: ${c("background-fill-color-card-background-secondary")};
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-radius: 3px;

	.title {
		${styles.effects.text.bodyStrong};
	}

	.text:is(:lang(zh), :lang(ja), :lang(ko)) {
		text-align: justify;
	}

	.title,
	.text {
		line-height: 18px;
	}

	.badge {
		${styles.mixins.square("16px")};
		margin-top: ${(20 - 18) / 2}px;
	}

	${({ $status }) => css`
		background-color: ${c(backgroundColors[$status])};
	`}
`;

export default function InfoBar({ status = "info", title, children, ...htmlAttrs }: FCP<{
	/** 角标的状态，即颜色。 */
	status?: InfoBarStatus;
	/** 标题。 */
	title?: string;
}, "div">) {
	return (
		<StyledInfoBar $status={status} {...htmlAttrs}>
			{status && <Badge status={status} />}
			<div className="title">{title}</div>
			<div className="text">{children}</div>
		</StyledInfoBar>
	);
}
