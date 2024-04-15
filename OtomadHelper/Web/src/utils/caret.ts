/**
 * Text cursor assisted operation object.
 */
export const Caret = {
	/**
	 * Get the index of the text cursor.
	 * @param input - Input elements (if has).
	 * @returns The index of the text cursor.
	 */
	get(input?: MaybeRef<HTMLInputElement>) {
		if (input) {
			input = toValue(input);
			return input.selectionStart;
		}
		const selection = window.getSelection();
		if (selection === null) return null;
		return selection.anchorOffset;
	},

	/**
	 * Set the index of the text cursor.
	 * @param element HTML DOM element.
	 * @param offset The index of the text cursor.
	 */
	set(element: MaybeRef<Element>, offset: number | undefined | null) {
		if (offset == null) return;
		element = toValue(element);

		if (element instanceof HTMLInputElement) {
			if (offset < 0) offset = element.value.length + 1 + offset;
			if (offset < 0) offset = 0;
			element.setSelectionRange(offset, offset);
			return;
		}

		const range = document.createRange();
		const selection = window.getSelection();
		if (selection === null) return;

		if (!element.textContent) return;
		if (offset < 0) offset = element.textContent.length + 1 + offset;
		offset = Math.min(offset, element.textContent.length) - 1;
		if (offset < 0) offset = 0;
		range.setStart(element, offset);
		range.collapse(true);

		selection.removeAllRanges();
		selection.addRange(range);
	},

	/**
	 * Clear text selection.
	 *
	 * Note that the text should be deselected instead of being deleted.
	 */
	clear() {
		if (window.getSelection) {
			if (window.getSelection()?.empty) // Chrome
				window.getSelection()?.empty();
			else if (window.getSelection()?.removeAllRanges) // Firefox
				window.getSelection()?.removeAllRanges();
		} else if (document.selection) // IE?
			document.selection.empty();
	},
};

/**
 * Insert the text into the specified text box at the cursor position, and if text is selected, replace it.
 * @param input - Input box.
 * @param text - Insert text.
 */
export function insertTextToTextBox(input: MaybeRef<HTMLInputElement | HTMLTextAreaElement>, text: string = "") {
	input = toValue(input);
	const { selectionStart: start, selectionEnd: end, value } = input;
	if (start === null || end === null) return;
	const newValue = value.slice(0, start) + text + value.slice(end);
	const newCaret = start + text.length;
	input.value = newValue;
	input.setSelectionRange(newCaret, newCaret);
}
