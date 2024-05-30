export /* internal */ const styledExpanderItemBase = css`
	display: flex;
	gap: 16px;
	align-items: center;
	min-height: 48px;
	overflow-x: clip;

	:where(&) {
		padding: 7px 51px;
	}

	> :not(.text) {
		flex-shrink: 0;
	}
`;

export /* internal */ const styledExpanderItemText = css`
	.text {
		> * {
			${styles.mixins.hideIfEmpty()};
		}

		.title {
			line-height: 20px;
		}

		.details {
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
		gap: 1rem;
		align-items: center;

		.trailing-icon {
			${styles.mixins.square("30px")};
			${styles.mixins.flexCenter()};
			flex-shrink: 0;
			margin-right: -7px;
			border-radius: 3px;

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

export /* internal */ default function ExpanderItem({ icon, title, details, children }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
}>) {
	return (
		<StyledExpanderItem>
			{icon ? <Icon name={icon} /> : <div className="icon-placeholder" />}
			<div className="text">
				<p className="title"><Preserves>{title}</Preserves></p>
				<p className="details"><Preserves>{details}</Preserves></p>
			</div>
			<div className="trailing">
				{children}
			</div>
		</StyledExpanderItem>
	);
}
