const DEFAULT_REL = "noopener noreferrer nofollow";

export default function Link({ children, href, blank = true, target, rel = DEFAULT_REL, onKeyDown, ...htmlAttrs }: FCP<{
	/** Open in new tab? */
	blank?: boolean;
}, "a">) {
	target = blank ? "_blank" : target;
	rel = rel.trim().replaceAll(/\s+/g, " ");

	const handleKeyDown: KeyboardEventHandler<HTMLAnchorElement> = e => {
		onKeyDown?.(e);
		if (e.code.in("Enter", "Space")) {
			e.preventDefault();
			e.currentTarget.click();
		}
	};

	return (
		<a
			href={href}
			target={target}
			rel={rel}
			tabIndex={0}
			onKeyDown={handleKeyDown}
			{...htmlAttrs}
		>
			{children}
		</a>
	);
}
