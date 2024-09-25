export interface Bridge {
	setIsDevMode(isDevMode: boolean): void;
	setCulture(culture: string): void;
	showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): string;
	showComboBox<T>(rect: RectTuple, selected: T, ids: T[], options: string[]): T;
	showPitchPicker(rect: RectTuple, pitch: string): string;
}

export interface ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
