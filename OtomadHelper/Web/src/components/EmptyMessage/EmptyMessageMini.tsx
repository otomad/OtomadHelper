const StyledEmptyMessageMini = styled.div`
	${styles.effects.text.body};
	${styles.mixins.flexCenter()};
	gap: 1.5ex;
	margin-block: 0.5lh;
	color: ${c("fill-color-text-tertiary")};

	.icon {
		${styles.effects.text.iconSmall};
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
		<StyledEmptyMessageMini>
			<Icon name={icon} />
			<p>{children}</p>
		</StyledEmptyMessageMini>
	);
}
