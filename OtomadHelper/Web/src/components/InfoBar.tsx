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
	padding: 14px 13px;
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
		flex-wrap: wrap;
		column-gap: inherit;
	}

	.title,
	.text {
		display: inline-block;
		line-height: 20px;
	}

	.badge {
		${styles.mixins.square("16px")};
		margin-top: ${(20 - 16) / 2}px;
	}

	${({ $status }) => css`
		background-color: ${c(backgroundColors[$status])};
	`}
`;

export default function InfoBar({ status, title, children, ...htmlAttrs }: FCP<{
	/** 角标的状态，颜色和图标。 */
	status?: Status;
	/** 标题。 */
	title?: string;
}, "div">) {
	return (
		<StyledInfoBar $status={status ?? "info"} {...htmlAttrs}>
			{status && <Badge status={status} />}
			<div className="text-part">
				<div className="title">{title}</div>
				<div className="text">{children}</div>
			</div>
		</StyledInfoBar>
	);
}
