export default function Portal({ container = "#popovers", children }: FCP<{
	/**
	 * 某个已经存在的 DOM 节点，例如由 `document.getElementById()` 返回的节点。在更新过程中传递不同的 DOM 节点将导致 portal 内容被重建。
	 *
	 * 此外也支持直接传递字符串，将会自动调用 `document.querySelector()`。
	 */
	container?: Element | DocumentFragment | string;
}>) {
	if (typeof container === "string")
		container = document.querySelector(container)!;

	return createPortal(children, container);
}
