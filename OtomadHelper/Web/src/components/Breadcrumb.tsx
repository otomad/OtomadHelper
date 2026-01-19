const TITLE_LINE_HEIGHT = 40;

const StyledBreadcrumb = styled.nav`
	display: flex;
	align-items: center;
	transition: all ${eases.easeInOutMaterialEmphasized} 700ms;

	&[aria-level="1"] {
		${styles.effects.text.title};
		gap: 14px;
	}

	&[aria-level="4"] {
		${styles.effects.text.body};
		gap: 4px;
	}

	* {
		white-space: nowrap;
	}

	${tgs(tgs.exit)} {
		translate: 0 -${TITLE_LINE_HEIGHT}px;
	}

	${tgs(tgs.enter)} {
		translate: 0 ${TITLE_LINE_HEIGHT}px;
	}

	&.exit:has(+ .title.exit) {
		transition-duration: 1s;
	}

	> div {
		display: contents;

		.enter,
		.exit-active {
			translate: 20px;
			opacity: 0;
		}

		.enter-active {
			translate: 0;
			opacity: 1;
			transition-duration: 300ms;
			transition-delay: 200ms;

			&.crumb {
				transition-delay: 300ms;
			}
		}

		.exit-active {
			transition-timing-function: ${eases.easeInMax};

			&.breadcrumb-chevron-right {
				transition-delay: 50ms;
			}
		}

		> .parent {
			color: ${c("fill-color-text-secondary")};

			&:hover {
				color: ${c("foreground-color")};
			}

			&:active {
				color: ${c("fill-color-text-tertiary")};
			}
		}
	}
`;

export default function Breadcrumb({ titles, large = true, ...htmlAttrs }: FCP<{
	/** Array of breadcrumb navigation titles. */
	titles: ({ name: ReactNode; onClick?(): void } | undefined | null | false)[];
	/** Show as large size header (which used in navigation view)? @default true */
	large?: boolean;
	children?: never;
}, "h1">) {
	return (
		<StyledBreadcrumb
			className="title"
			aria-label={t.aria.breadcrumb}
			aria-level={large ? 1 : 4}
			{...htmlAttrs}
		>
			<TransitionGroup>
				{titles.toCompacted().flatMap((title, i, { length }) => {
					const last = i === length - 1;
					const crumb = (
						<button
							key={i}
							className={["crumb", { parent: !last }]}
							tabIndex={last ? -1 : 0}
							type="button"
							role="link"
							aria-current={last && "page"}
							onClick={last ? undefined : title.onClick}
						>
							{title.name}
						</button>
					);
					const result = [crumb];
					if (!last) result.push(<BreadcrumbChevronRight key={i + "-chevron"} />);
					return result.map((node, j) =>
						<CssTransition key={`${i}-${j}`}>{node}</CssTransition>);
				})}
			</TransitionGroup>
		</StyledBreadcrumb>
	);
}

const StyledBreadcrumbChevronRight = styled.div(() => css`
	${styles.mixins.flexCenter()};
	${styledDirBasedIcon(true)};
	margin-block-start: 4px;

	.icon {
		color: ${c("fill-color-text-secondary")};
		font-size: 16px;
	}

	h4${StyledBreadcrumb} & {
		margin-block-start: 1px;

		.icon {
			font-size: 12px;
		}
	}
`);

const BreadcrumbChevronRight = ({ ref }: FCP<{}, "div">) => (
	<StyledBreadcrumbChevronRight ref={ref}>
		<Icon name="chevron_right" />
	</StyledBreadcrumbChevronRight>
);
