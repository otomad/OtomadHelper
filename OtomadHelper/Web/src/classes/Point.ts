/**
 * Point class.
 */
export default class Point {
	constructor(public x: number, public y: number) { }

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
}
