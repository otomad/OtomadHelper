export { };

declare module "./webview2" {
	interface HostObjects {
		bridge: {
			showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): Promise<string>;
		};
	}
}

interface ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
