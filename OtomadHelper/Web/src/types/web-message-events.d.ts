declare global {
	namespace WebMessageEvents {
		export type DragOver = {
			extension: string;
			contentType: string;
			isDirectory: boolean;
			isDragging: true;
		} | {
			isDragging: false;
		};
	}
}

export default WebMessageEvents;
