scalar RectTuple;

interface Bridge {
	setIsDevMode(isDevMode: boolean): void;
	setCulture(culture: string): void;
	showMessageBox(title: string, body: string, buttons: ContentDialogButtonItem[]): string;
	showComboBox(rect: RectTuple, selected: string, options: string[]): string;
	showPitchPicker(rect: RectTuple, pitch: string): string;
}

model ContentDialogButtonItem {
	text: string;
	dialogResult: string;
	isDefault?: boolean;
}
