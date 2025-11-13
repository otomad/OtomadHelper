/* eslint-disable no-var */
// Show warning when user trying to use Internet Explorer 10 and 11 to visit the webpage.
if (window.ActiveXObject || "ActiveXObject" in window) {
	var noscript = document.getElementsByTagName("noscript")[0];
	if (noscript) {
		var conditionalComment = noscript.nextSibling;
		// Apply only for Internet Explorer 10 and 11, because they don't support condition comments which supported by older versions.
		if (typeof Comment !== "undefined" && conditionalComment instanceof Comment) {
			var html = conditionalComment.textContent.replace("[if IE]>", "").replace("<![endif]", "");
			noscript.insertAdjacentHTML("afterend", html);
		}
	}
}
