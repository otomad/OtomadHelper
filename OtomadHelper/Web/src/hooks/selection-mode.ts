/**
 * Convert is multiple selection mode to selection mode enum string.
 * @param isMultiple - Is multiple selection mode?
 * @returns Selection mode enum string.
 */
export function useSelectionMode(isMultiple: StateProperty<boolean>) {
	return useStateSelector(
		isMultiple,
		multiple => multiple ? "multiple" : "single",
		selectionMode => selectionMode === "multiple",
	);
}
