const fakeAnimations = [
	"Normal",
	"Hover",
	"Pressed",
	"Selected",
	"HoverSelected",
	"PressedSelected",
];

export default fakeAnimations.map(identifier => css`@keyframes ${identifier} {}`);
