namespace Receives {
	export type DragOver = {
		extension: string;
		contentType: string;
		isDirectory: boolean;
		isDragging: true;
	} | {
		isDragging: false;
	};
}

export default Receives;
