const StyledEmptyMessageMini = styled.div`
	${styles.text.body};
	${styles.mixins.flexCenter()};
	gap: 1ex;
	margin-block: 0.5lh;
	color: ${c("fill-color-text-tertiary")};

	> .icon {
		${styles.text.iconSmall};
	}

	> p {
		margin-block-start: -1px;
	}

	&:not(:only-child) {
		display: none;
	}
`;

export /* @internal */ default function EmptyMessageMini({ children, icon = "placeholder", ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	children: ReactNode;
}, "div">) {
	return (
		<StyledEmptyMessageMini {...htmlAttrs}>
			<Icon name={icon} />
			<p>{children}</p>
		</StyledEmptyMessageMini>
	);
}
