const StyledKbd = styled.div`
	${styles.text.caption};
	display: inline-block;
	margin-inline-start: 0.5em;
	color: ${c("fill-color-text-tertiary")};
	translate: 0 -1px;

	kbd {
		padding-block: 1px;
		padding-inline: 2px;
		border: 1px solid ${c("fill-color-text-tertiary", 50)};
		border-radius: 4px;
	}

	.add {
		margin-inline: 0.25em;
	}
`;

export default function Kbd({ children, ...htmlAttrs }: FCP<{
	/** Shortcut key or access key. */
	children: string | string[];
}, "div">) {
	children = wrapIfNotArray(children);
	return (
		<StyledKbd translate="no" lang="en" {...htmlAttrs}>
			{children
				.map((key, i) => <kbd key={i}>{key}</kbd>)
				.interpose(i => <span key={`add-${i}`} translate="no" className="monospace add">+</span>)}
		</StyledKbd>
	);
}
