// DELETE: Useless now.

const isPathObject = Symbol("path-object.is_path_object");
const variableNameRe = /^[a-z_$][\w$]*$/;

function pathObjectTarget(path: string) {
	const noop = function () { };
	noop.path = path;
	Object.freeze(noop);
	return noop;
}

function stringifyIfRequired(value: string | number | undefined) {
	return value === undefined ? "" : isValidNumber(value) ? String(value) : JSON.stringify(value);
}

function pathObjectProxy(path: string): ReturnType<typeof pathObjectTarget> {
	return new Proxy(pathObjectTarget(path), {
		get(target, property) {
			let { path } = target;
			if ([Symbol.toPrimitive, "toString", "valueOf"].includes(property))
				return () => path;
			if (property === isPathObject) return true;
			if (typeof property === "string")
				if (property.match(variableNameRe))
					path += `.${property}`;
				else
					path += `[${stringifyIfRequired(property)}]`;
			else if (typeof property === "symbol") {
				const { description, isGlobal } = property;
				path += `[${isGlobal ? "Symbol.for" : "Symbol"}(${stringifyIfRequired(description)})]`;
			}
			return pathObjectProxy(path);
		},
		apply(target, _thisArg, argArray) {
			let { path } = target;
			path += `(${JSON.stringify(argArray).slice(1, -1)})`;
			return pathObjectProxy(path);
		},
		construct(target, argArray, _newTarget) {
			let { path } = target;
			if (path.includes("(")) path = `(${path})`;
			path += `(${JSON.stringify(argArray).slice(1, -1)})`;
			path = "new " + path;
			return pathObjectProxy(path);
		},
		set() { return false; },
		has() { return true; },
		deleteProperty() { return false; },
		ownKeys() { return []; },
	});
}

/** @deprecated */
const PathObject = (class PathObject {
	constructor() {
		return new Proxy(this, {
			get: (_, property) => {
				if (typeof property !== "string" || !property.match(variableNameRe))
					throw new SyntaxError(`Property name ${typeof property === "symbol" ? property.toString() : JSON.stringify(property)} is not a valid JavaScript variable name`);
				return pathObjectProxy(property);
			},
		});
	}

	static [Symbol.hasInstance](instance: Any) {
		return !!instance?.[isPathObject];
	}
}) as new<T = Any> () => Record<string, T>;

globals.PathObject = PathObject;

export default PathObject;
