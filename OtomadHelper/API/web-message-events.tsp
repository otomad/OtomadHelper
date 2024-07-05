namespace WebMessageEvents;

model DragOver {
	isDragging: boolean;
}

model DragOverIsDragging extends DragOver {
	extension: string;
	contentType: string;
	isDirectory: boolean;
	isDragging: true;
}

model ConsoleLog {
	severity: "log" | "error" | "warn";
	message: string;
}
