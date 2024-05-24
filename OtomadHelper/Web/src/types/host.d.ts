export interface Bridge {
	setIsDevMode(isDevMode: bool): void;
	setCulture(culture: string): void;
	showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): string;
	showComboBox(rect: RectTuple, selected: string, options: string[]): string;
	showPitchPicker(rect: RectTuple, pitch: string): string;
}

export interface ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
