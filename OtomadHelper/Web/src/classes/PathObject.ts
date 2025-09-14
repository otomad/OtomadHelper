const isPathObject = Symbol("path-object.is_path_object");

function pathObjectTarget(path: string) {
	const noop = () => { };
	noop.path = path;
	return noop;
}

function stringifyIfRequired(value: string | number | undefined) {
	// eslint-disable-next-line no-restricted-globals
	return value === undefined ? "" : isFinite(value as number) && value !== "" ? String(value) : JSON.stringify(value);
}

function pathObjectProxy(path: string) {
	return new Proxy(pathObjectTarget(path), {
		get(target, property) {
			let { path } = target;
			if ([Symbol.toPrimitive, "toString", "valueOf"].includes(property))
				return () => path;
			if (property === isPathObject) return true;
			if (typeof property === "string")
				if (property.match(/^[a-z_$][\w$]*$/))
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
			return path;
		},
	});
}

const PathObject = (class PathObject {
	constructor() {
		return new Proxy(this, {
			get: (_, property) => {
				if (typeof property !== "string") return;
				return pathObjectProxy(property);
			},
		});
	}

	static [Symbol.hasInstance](instance: Any) {
		return !!instance?.[isPathObject];
	}
}) as new<T = Any> () => Record<string, T>;

export default PathObject;
