export default function OpenLink({ children, ...htmlAttrs }: FCP<{}, "a">) {
	return <a target="_blank" rel="noopener noreferrer nofollow" {...htmlAttrs}>{children}</a>;
}
