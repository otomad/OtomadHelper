export default function OpenLink({ children, ...htmlAttrs }: FCP<{}, HTMLAnchorElement>) {
	return <a target="_blank" rel="noopener noreferrer nofollow" {...htmlAttrs}>{children}</a>;
}
