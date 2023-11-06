const StyledButton = styled.button`
	display: inline-flex;
	border-radius: 4px;
	border: 1px solid ${c("stroke-color-control-stroke-default")};

	${ifColorScheme.dark} &:not(:active, [disabled]) {
		border-top-color: ${c("stroke-color-control-stroke-secondary")};
	}

	${ifColorScheme.light} &:not(:active, [disabled]) {
		border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
	}

	> .base {
		${styles.mixins.square("100%")};
		${styles.mixins.flexCenter()};
		padding: 4px 11px 6px;
		background-color: ${c("fill-color-control-default")};
		border-radius: 3px;
	}

	@layer components {
		min-width: 96px;
	}

	&:hover > .base {
		background-color: ${c("fill-color-control-secondary")};
	}

	&:active > .base {
		background-color: ${c("fill-color-control-tertiary")};

		> .content {
			opacity: ${c("pressed-text-opacity")};
		}
	}

	&[disabled] > .base {
		background-color: ${c("fill-color-control-disabled")};

		> .content {
			opacity: ${c("disabled-text-opacity")};
		}
	}

	> .base > .content {
		${styles.mixins.flexCenter()};
		gap: 8px;

		> span:empty {
			display: none;
		}
	}

	&.subtle {
		border: none;
		padding: 0;

		.base {
			background-color: ${c("fill-color-subtle-transparent")};
		}

		&::before {
			display: none;
		}

		&:hover > .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active > .base {
			background-color: ${c("fill-color-subtle-tertiary")};
		}
	}
`;

export default function Button({ children, icon, subtle, className, ...htmlAttrs }: FCP<{
	/** 按钮图标。 */
	icon?: string;
	/** 是否使用无背景按钮？ */
	subtle?: boolean;
}, HTMLButtonElement>) {
	return (
		<StyledButton type="button" className={[className, { subtle }]} {...htmlAttrs}>
			<div className="base">
				<div className="content">
					{icon && <Icon name={icon} />}
					<span>{children}</span>
				</div>
			</div>
		</StyledButton>
	);
}
