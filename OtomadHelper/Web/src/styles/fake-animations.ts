export const STATUS_PREFIX = "-status-";
export type AvailableLottieStatus = typeof fakeAnimations[number];

const fakeAnimations = [
	"Normal",
	"Hover",
	"Pressed",
	"Selected",
	"HoverSelected",
	"PressedSelected",
] as const;

export default fakeAnimations.map(identifier => css`
	// stylelint-disable-next-line block-no-empty
	@keyframes ${STATUS_PREFIX}${identifier} {}
`);
