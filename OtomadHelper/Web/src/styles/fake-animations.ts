const fakeAnimations = [
	"Normal",
	"Hover",
	"Pressed",
	"Selected",
	"HoverSelected",
	"PressedSelected",
];

export default fakeAnimations.map(identifier => css`
	// stylelint-disable-next-line block-no-empty
	@keyframes ${identifier} {}
`);
