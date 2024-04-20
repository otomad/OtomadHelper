/**
 * `Portal` lets you render some children into a different part of the DOM.
 */
export default function Portal({ container = "#popovers", children }: FCP<{
	/**
	 * Some DOM node, such as those returned by document.getElementById(). The node must already exist.
	 * Passing a different DOM node during an update will cause the portal content to be recreated.
	 *
	 * In addition, passing strings directly is also supported, and `document.querySelector()` will be called automatically.
	 */
	container?: Element | DocumentFragment | string;
}>): React.ReactPortal {
	if (typeof container === "string")
		container = document.querySelector(container)!;

	return createPortal(children, container);
}
