useListen("host:consoleLog", ({ severity, message }) => {
	console[severity]?.(message);
});

useListenAndReturnAck("host:elementFromPoint", ({ x, y }) => {
	const elements = document.elementsFromPoint(x, y) as HTMLElement[];
	const attrs = elements.map(element => ({
		tag: element.tagName.toLowerCase(),
		type: "type" in element ? element.type as string : null,
		class: [...element.classList],
		id: element.id,
		data: element.dataset,
	}));
	return attrs;
});
