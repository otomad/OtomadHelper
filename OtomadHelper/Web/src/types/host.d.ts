export interface Bridge {
	setIsDevMode(isDevMode: bool): void;
	showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): string;
}

export interface ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
