export interface Bridge {
	setIsDevMode(isDevMode: bool): void;
	showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): string;
	showComboBox(rect: [x: number, y: number, width: number, height: number], selected: string, options: string[]): Promise<string>;
}

export interface ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
