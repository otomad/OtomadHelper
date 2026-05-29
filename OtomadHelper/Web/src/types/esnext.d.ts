export { };

// #region Misc
declare global {
	interface Element {
		/**
		 * The `Element.scrollIntoViewIfNeeded()` method scrolls the current element into the visible area of the
		 * browser window if it's not already within the visible area of the browser window. If the element is
		 * already within the visible area of the browser window, then no scrolling takes place. This method is
		 * a proprietary variation of the standard `Element.scrollIntoView()` method.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Element/scrollIntoViewIfNeeded)
		 */
		scrollIntoViewIfNeeded(centerIfNeeded?: boolean): void;

		// Element-Scoped View Transition API
		startViewTransition: Document["startViewTransition"];
	}

	interface ScreenOrientation {
		/**
		 * The `lock()` property of the `ScreenOrientation` interface locks the orientation of the containing document to the specified orientation.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/ScreenOrientation/lock)
		 * @see [Microsoft unexpected remove it from TypeScript library.](https://github.com/microsoft/TypeScript-DOM-lib-generator/issues/1615)
		 *
		 * @param type - An orientation lock type.
		 * @returns A Promise that resolves after locking succeeds.
		 */
		lock(type: "any" | "natural" | "landscape" | "portrait" | OrientationType): Promise<void>;
	}

	interface Object {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Symbol/toPrimitive) */
		[Symbol.toPrimitive](hint: "number" | "string" | "default"): any;
	}

	/**
	 * > Type 'Buffer' is not assignable to type 'Uint8Array'.\
	 * >   The types of 'entries()' are incompatible between these types.\
	 * >     Type 'IterableIterator<[number, number]>' is missing the following properties from type 'ArrayIterator<[number, number]>': map, filter, take, drop, and 9 more.
	 *
	 * @template T - {@link IterableIterator}
	 * @template U - {@link IterableIterator}
	 */
	interface IterableIterator<T, U> extends ArrayIterator<T> { }

	interface FontFaceDescriptors {
		/**
		 * I don't know why MDN missing the JS FontFace version of `size-adjust`.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/CSS/@font-face/size-adjust)\
		 * [Tracking](https://github.com/mdn/content/issues/36587)
		 */
		sizeAdjust?: string;
	}

	/**
	 * Fix: `cancelAnimationFrame` will accept `undefined` value.
	 *
	 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/DedicatedWorkerGlobalScope/cancelAnimationFrame)
	 *
	 * @param handle - The ID value returned by a call to `requestAnimationFrame()`;
	 * the call must have been made in the same worker.
	 */
	declare function cancelAnimationFrame(handle: number | undefined): void;

	interface RegExpConstructor {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/RegExp/escape) */
		escape(string: string): string;
	}

	interface Uint8ArrayConstructor {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array/fromBase64) */
		fromBase64(string: string, options?: {
			alphabet?: "base64" | "base64url";
			lastChunkHandling?: "loose" | "strict" | "stop-before-partial";
		}): Uint8Array<ArrayBuffer>;

		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array/fromHex) */
		fromHex(string: string): Uint8Array<ArrayBuffer>;
	}

	interface Uint8Array {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array/toBase64) */
		toBase64(options?: {
			alphabet?: "base64" | "base64url";
			omitPadding?: boolean;
		}): string;

		/** [MDN Reference](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array/toHex) */
		toHex(): string;
	}

	interface ScrollIntoViewOptions {
		container?: "all" | "nearest";
	}

	declare interface Map<K, V> {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Map/getOrInsert) */
		getOrInsert(key: K, defaultValue: V): V;

		/** [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Map/getOrInsertComputed) */
		getOrInsertComputed(key: K, computeFn: (key: K) => V): V;
	}

	declare interface IteratorConstructor {
		/**
		 * The `Iterator.concat()` static method creates a new `Iterator` object from a list of iterable objects.
		 * The new iterator yields the values from each of the input iterables in sequence.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Iterator/concat)
		 */
		concat<TIterable = never, TIterator = never>(...iterators: (Iterable<TIterable> | Iterator<TIterator>)[]): IteratorObject<TIterable | TIterator>;
	}
}
// #endregion

// #region New CSS Properties
declare global {
	interface CSSStyleDeclaration {
		interpolateSize: string;
		[x: string]: string;
	}
}

declare module "csstype" {
	interface Properties {
		/**
		 * **Custom properties** (sometimes referred to as **CSS variables** or
		 * **cascading variables**) are entities defined by CSS authors that contain
		 * specific values to be reused throughout a document. They are set using
		 * custom property notation (e.g., `--main-color: black;`) and are accessed
		 * using the `var()` function (e.g., `color: var(--main-color);`).
		 *
		 * @see https://developer.mozilla.org/docs/Web/CSS/Using_CSS_custom_properties
		 */
		[customProperty: `--${string}`]: string | number | undefined;

		[any: string & {}]: string | number | undefined;
	}
}
// #endregion

// #region Compatible with Internet Explorer
declare global {
	interface Window {
		/**
		 * The ActiveX object that is only available in Internet Explorer, higher versions of browsers will return undefined.
		 *
		 * [Microsoft Reference](https://learn.microsoft.com/openspecs/ie_standards/ms-es5ex/64528856-d0ab-4639-a8a0-625040a88c93)
		 */
		ActiveXObject: undefined;
	}

	interface Document {
		/**
		 * Text selection objects available in Internet Explorer.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Selection)
		 */
		selection: Selection | null;
	}
}
// #endregion

// #region Element-Scoped View Transition API
declare global {
	interface Element {
		startViewTransition: Document["startViewTransition"];
		activeViewTransition: Document["activeViewTransition"];
	}
}
// #endregion

// #region Local Font Access API
declare global {
	interface FontData {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/FontData/family) */
		readonly family: string;
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/FontData/fullName) */
		readonly fullName: string;
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/FontData/postscriptName) */
		readonly postscriptName: string;
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/FontData/style) */
		readonly style: string;

		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/FontData/blob) */
		blob(): Promise<Blob>;
	}

	declare interface Window {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/Window/queryLocalFonts) */
		queryLocalFonts(): Promise<FontData[]>;
	}
}
// #endregion

// #region User-Agent Client Hints API
declare global {
	declare interface NavigatorID {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/Navigator/userAgentData) */
		readonly userAgentData: UserAgentData;
	}

	interface UserAgentDataBrand {
		brand: string;
		version: string;
	}

	interface UserAgentData {
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/NavigatorUAData/brands) */
		readonly brands: UserAgentDataBrand[];
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/NavigatorUAData/mobile) */
		readonly mobile: boolean;
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/NavigatorUAData/platform) */
		readonly platform: string;

		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/NavigatorUAData/toJSON) */
		toJSON(): UserAgentData;
		/** [MDN Reference](https://developer.mozilla.org/docs/Web/API/NavigatorUAData/getHighEntropyValues) */
		getHighEntropyValues<T extends keyof UserAgentDataHighEntropyValues>(hints: T[]): Promise<Readonly<{
			brands: UserAgentDataBrand[];
			mobile: boolean;
			platform: string;
		} & OmitNevers<{
			[hint in keyof UserAgentDataHighEntropyValues]: T extends hint ? UserAgentDataHighEntropyValues[hint] : never;
		}>>>;
	}
}

interface UserAgentDataHighEntropyValues {
	architecture: string;
	bitness: string;
	formFactors: string[];
	fullVersionList: UserAgentDataBrand[];
	model: string;
	platformVersion: string;
	/** @deprecated */
	uaFullVersion: string;
	wow64: boolean;
}
// #endregion

// #region File System Access
declare global {
	type FileSystemPermissionMode = "read" | "readwrite";

	interface FileSystemPermissionDescriptor extends PermissionDescriptor {
		handle: FileSystemHandle;
		mode?: FileSystemPermissionMode;
	}

	interface FileSystemHandlePermissionDescriptor {
		mode?: FileSystemPermissionMode;
	}

	interface FileSystemHandle {
		queryPermission(descriptor?: FileSystemHandlePermissionDescriptor): Promise<PermissionState>;
		requestPermission(descriptor?: FileSystemHandlePermissionDescriptor): Promise<PermissionState>;
	}

	type WellKnownDirectory = "desktop" | "documents" | "downloads" | "music" | "pictures" | "videos";

	type StartInDirectory = WellKnownDirectory | FileSystemHandle;

	interface FilePickerAcceptType {
		/**
		 * An optional description of the category of files types allowed. Defaults to an empty string.
		 */
		description?: string;
		/**
		 * An `Object` with the keys set to the MIME type and the values an `Array` of file extensions.
		 */
		accept?: Record<string, string | string[]>;
	}

	interface FilePickerOptions {
		/**
		 * An `Array` of allowed file types to pick. Each item is an object.
		 */
		types?: FilePickerAcceptType[];
		/**
		 * A boolean value that defaults to `false`. By default the picker should include an option to not apply any file type filters
		 * (instigated with the type option below). Setting this option to `true` means that option is *not* available.
		 */
		excludeAcceptAllOption?: boolean;
		/**
		 * By specifying an ID, the browser can remember different directories for different IDs.
		 * If the same ID is used for another picker, the picker opens in the same directory.
		 */
		id?: string;
		/**
		 * A `FileSystemHandle` or a well known directory (`"desktop"`, `"documents"`, `"downloads"`, `"music"`, `"pictures"`,
		 * or `"videos"`) to open the dialog in.
		 */
		startIn?: StartInDirectory;
	}

	interface OpenFilePickerOptions extends FilePickerOptions {
		/**
		 * A boolean value that defaults to `false`. When set to `true` multiple files may be selected.
		 */
		multiple?: boolean;
	}

	interface SaveFilePickerOptions extends FilePickerOptions {
		/**
		 * A `String`. The suggested file name.
		 */
		suggestedName?: string;
	}

	interface DirectoryPickerOptions {
		/**
		 * By specifying an ID, the browser can remember different directories for different IDs.
		 * If the same ID is used for another picker, the picker opens in the same directory.
		 */
		id?: string;
		/**
		 * A `FileSystemHandle` or a well known directory (`"desktop"`, `"documents"`, `"downloads"`, `"music"`, `"pictures"`,
		 * or `"videos"`) to open the dialog in.
		 */
		startIn?: StartInDirectory;
		/**
		 * A string that defaults to `"read"` for read-only access or `"readwrite"` for read and write access to the directory.
		 */
		mode?: FileSystemPermissionMode;
	}

	interface DataTransferItem {
		getAsFileSystemHandle(): Promise<FileSystemHandle | null>;
	}

	interface Window {
		/**
		 * The `showOpenFilePicker()` method of the `Window` interface shows a file picker that allows a user
		 * to select a file or multiple files and returns a handle for the file(s).
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Window/showOpenFilePicker)
		 */
		showOpenFilePicker(options?: OpenFilePickerOptions): Promise<FileSystemFileHandle[]>;

		/**
		 * The `showSaveFilePicker()` method of the `Window` interface shows a file picker that allows a user
		 * to save a file. Either by selecting an existing file, or entering a name for a new file.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Window/showSaveFilePicker)
		 */
		showSaveFilePicker(options?: SaveFilePickerOptions): Promise<FileSystemFileHandle>;

		/**
		 * The `showDirectoryPicker()` method of the `Window` interface displays a directory picker
		 * which allows the user to select a directory.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Window/showDirectoryPicker)
		 */
		showDirectoryPicker(options?: DirectoryPickerOptions): Promise<FileSystemDirectoryHandle>;
	}
}
// #endregion
