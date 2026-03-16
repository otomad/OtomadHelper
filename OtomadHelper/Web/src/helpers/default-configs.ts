/**
 * Do not let TypeScript make object as const.
 * @inheritdoc
 */
const freeze = <T>(object: T) => Object.freeze(object);

export const defaultPrveAmounts = freeze({
	compression: 0.8,
	slant: 0.7,
	puyo: 0.625,
	pendulum: 15,
	gaussianBlur: 0.1,
	radialBlur: 0.8,
	rotation: -90,
	initialAngle: 0,
	rotateCustomSequence: false,
	randomClassAlwaysInitialAtNormal: true,
});

export const defaultDurationFilter = freeze({
	min: NaN,
	max: NaN,
	minEqual: true,
	maxEqual: true,
	unit: "beat" satisfies Config.DurationFilterUnit as Config.DurationFilterUnit,
});
