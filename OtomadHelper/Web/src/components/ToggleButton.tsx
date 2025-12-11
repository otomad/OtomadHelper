export default function ToggleButton({ checked: [checked, setChecked] = [false] as never, appearance = "intense", subtle, onToggled, onClick, ...htmlAttrs }: FCP<Override<PropsOf<typeof Button>, {
	/** Checked? */
	checked?: StateProperty<boolean>;
	/**
	 * Specify the intensity of the toggle switch style.
	 * - `"subtle"`: The button is background-less, no matter whether it is selected or not,
	 * only the content has color. Gray when unchecked and accent when checked.
	 * - `"intense"`: When unchecked, the button is background-less, the content is **foreground color**;
	 * When checked, the button has a background, the background is accent color, and the content is highlighted.
	 * - `"intense-hyperlink"`: When unchecked, the button is background-less, the content is **accent color**;
	 * When checked, the button has a background, the background is accent color, and the content is highlighted.
	 * - `"obvious"`: Regardless of whether it is checked or not, the button has a background.
	 * @default intense
	 */
	appearance?: "subtle" | "intense" | "intense-hyperlink" | "obvious";
	/** Occurs when check state changed. */
	onToggled?(checked?: boolean): void;
}>>) {
	subtle ||= appearance === "subtle" ? true : "small-icon";
	return (
		<Button
			style={{ paddingBlock: 0 }}
			role="checkbox"
			aria-checked={checked}
			{...htmlAttrs}
			onClick={e => {
				(setChecked as SetStateNarrow<boolean>)?.(checked => {
					onToggled?.(!checked);
					return !checked;
				});
				if (!setChecked) onToggled?.(!checked);
				onClick?.(e);
			}}
			{...appearance === "subtle" ? {
				subtle,
				accent: checked ? true : "neutral",
			} : appearance === "obvious" ? {
				accent: checked,
			} : {
				subtle: checked ? false : subtle,
				accent: appearance === "intense" ? checked : true,
			}}
		/>
	);
}
