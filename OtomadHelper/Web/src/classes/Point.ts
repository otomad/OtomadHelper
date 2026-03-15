type PointLike = { x: number; y: number };
const isReadonlyArray = (arg: Any): arg is readonly Any[] => Array.isArray(arg);

/**
 * Point class.
 */
export default class Point {
	x: number;
	y: number;

	/**
	 * Construct a new 2D Point from the number values of x-axis and y-axis.
	 */
	constructor(x: number, y: number);
	/**
	 * Construct a new 2D Point from a 2D number tuple.
	 * ```typescript
	 * [x: number, y: number]
	 * ```
	 */
	constructor(twoD: Readonly<TwoD>);
	/**
	 * Construct a new 2D Point from a literal object of a 2D point.
	 * ```typescript
	 * { x: number; y: number }
	 * ```
	 */
	constructor(pointLike: Readonly<PointLike>);
	constructor(arg1: Readonly<TwoD> | Readonly<PointLike> | number, arg2?: number) {
		if (typeof arg1 === "number" && typeof arg2 === "number")
			[this.x, this.y] = [arg1, arg2];
		else if (isReadonlyArray(arg1))
			[this.x, this.y] = arg1;
		else if (isObject(arg1))
			({ x: this.x, y: this.y } = arg1);
		else
			throw new TypeError(`No overload matches this call. The argument ${
				arg2 === undefined ? `type "${type(arg1)}" is` : `types "${type(arg1)}" and "${type(arg2)}" are`
			} not assignable to the constructor of "Point"`);
	}

	/**
	 * Find the distance between two points.
	 * @param point - Another point.
	 * @returns Distance.
	 */
	distance(point: Point): number {
		return Math.hypot(point.x - this.x, point.y - this.y);
	}

	/**
	 * Find the X-axis distance between two points.
	 * @param point - Another point.
	 * @returns X-axis distance.
	 */
	distanceX(point: Point): number {
		return point.x - this.x;
	}

	/**
	 * Find the Y-axis distance between two points.
	 * @param point - Another point.
	 * @returns Y-axis distance.
	 */
	distanceY(point: Point): number {
		return point.y - this.y;
	}

	/**
	 * Returns a string representation of an object.
	 * @override
	 * @returns A string representing the object.
	 */
	toString() {
		return `(${this.x}, ${this.y})`;
	}

	/**
	 * Get a new Point from original point `(0, 0)`.
	 */
	static get zero() {
		return new Point(0, 0);
	}
}
