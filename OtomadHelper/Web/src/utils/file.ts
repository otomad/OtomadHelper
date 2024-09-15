/**
 * Convert File or Blob to `blob:` URL.
 * @param file - File or Blob.
 * @returns `blob:` URL.
 */
export async function fileToBlob(file: File | Blob) {
	if (file instanceof FileSystemFileHandle) file = await file.getFile();
	const blob = URL.createObjectURL(file);
	return blob;
}

/**
 * Convert File or Blob to `data:` Base64 URL.
 * @param file - File or Blob.
 * @returns `data:` Base64 URL.
 */
export function fileToData(file: File | Blob) {
	return new Promise<string>(resolve => {
		const fileReader = new FileReader();
		fileReader.onload = function () { resolve(this.result as string); };
		fileReader.readAsDataURL(file);
	});
}

/**
 * Convert `data:` Base64 URL to Blob.
 * @param dataUrl - `data:` Base64 URL.
 * @returns Blob.
 */
export function dataToFile(dataUrl: string): Blob;
/**
 * Convert `data:` Base64 URL to File.
 * @param dataUrl - `data:` Base64 URL.
 * @param fileName - File name.
 * @returns File.
 */
export function dataToFile(dataUrl: string, fileName: string): File;
/**
 * Convert `data:` Base64 URL to File or Blob.
 * @param dataUrl - `data:` Base64 URL.
 * @param fileName - File name (optional).
 * @returns File or Blob.
 */
export function dataToFile(dataUrl: string, fileName?: string) {
	const mimeAndBytes = dataUrl.split(",") as [string, string];
	const mime = mimeAndBytes[0].match(/:(.*?);/)![1];
	const byteString = atob(mimeAndBytes[1]);
	let n = byteString.length;
	const bytes = new Uint8Array(n);
	while (n--)
		bytes[n] = byteString.charCodeAt(n);
	return fileName != null ?
		new File([bytes], fileName, { type: mime }) :
		new Blob([bytes], { type: mime }) as File;
}

/**
 * Opens a file selection dialog and returns the selected file(s) as a File[] object.
 *
 * @returns A single File object representing the selected file. If no file is selected, returns `null`.
 */
export async function openFile(options?: {
	/** A string specifying the types of files to accept, e.g., ".jpg,.png,image/*". */
	accept?: string;
	/** A boolean indicating whether to allow multiple file selection. */
	multiple?: false;
}): Promise<File | null>;
/**
 * Opens a file selection dialog and returns the selected file(s) as a File object.
 *
 * @returns An array of File objects representing the selected files.
 */
export async function openFile(options?: {
	/** A string specifying the types of files to accept, e.g., ".jpg,.png,image/*". */
	accept?: string;
	/** A boolean indicating whether to allow multiple file selection. */
	multiple: true;
}): Promise<File[]>;
export async function openFile({ accept = "", multiple = false } = {}): Promise<File | File[] | null> {
	// `showOpenFilePicker` is better, but it's not supported in all browsers and **TypeScript** yet.
	const input = document.createElement("input");
	input.type = "file";
	input.accept = accept;
	input.multiple = multiple;
	await new Promise<void>(resolve => {
		input.onchange = () => resolve();
		input.click();
	});
	const files = await Promise.all(Array.from(input.files ?? [], async file => file instanceof FileSystemFileHandle ? await file.getFile() : file));
	if (multiple) return files;
	else return files[0] ?? null;
}
