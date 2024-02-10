export /* internal */ const styledExpanderItemBase = css`
	display: flex;
	gap: 16px;
	align-items: center;
	min-height: 48px;

	:where(&) {
		padding: 7px 51px;
	}

	> :not(.text) {
		flex-shrink: 0;
	}
`;

export /* internal */ const styledExpanderItemText = css`
	.text {
		> :empty {
			display: none;
		}

		.heading {
			line-height: 20px;
		}

		.caption {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};
		}
	}
`;

export /* internal */ const styledExpanderItemContent = css`
	.icon-placeholder {
		${styles.mixins.square("20px")};
	}

	${styledExpanderItemText};

	.text {
		width: 100%;
	}

	.trailing {
		display: flex;
		align-items: center;
		gap: 1rem;

		.trailing-icon {
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			margin-right: -7px;
			border-radius: 3px;
			flex-shrink: 0;

			.icon {
				font-size: 16px;
			}
		}
	}
`;

const StyledExpanderItem = styled.div`
	${styledExpanderItemBase};
	padding-left: 15px;

	${styledExpanderItemContent};
`;

export /* internal */ default function ExpanderItem({ icon, heading, caption, children }: FCP<{
	/** 图标。 */
	icon?: string;
	/** 标题。 */
	heading?: ReactNode;
	/** 详细描述。 */
	caption?: ReactNode;
}>) {
	return (
		<StyledExpanderItem>
			{icon ? <Icon name={icon} /> : <div className="icon-placeholder" />}
			<div className="text">
				<p className="heading">{heading}</p>
				<p className="caption">{caption}</p>
			</div>
			<div className="trailing">
				{children}
			</div>
		</StyledExpanderItem>
	);
}
