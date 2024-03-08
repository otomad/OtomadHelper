// 本机端代码向网页端通信事件
window.chrome ??= new VirtualObject();
window.chrome.webview ??= new VirtualObject();
window.chrome.webview.addEventListener("message", e => {
	const { type, ...data } = e.data;
	console.log(type, data);
	useEvent(`host:${type}`, data);
});
