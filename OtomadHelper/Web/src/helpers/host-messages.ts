useListen("host:consoleLog", ({ severity, message }) => {
	console[severity]?.(message);
});
