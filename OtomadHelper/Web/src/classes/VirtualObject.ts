export default (class VirtualObject {
	constructor() {
		const aFunctionToGetThis = { ""() { return new VirtualObject(); } }[""];
		Object.defineProperty(aFunctionToGetThis, "toString", { value: () => "" });
		return new Proxy(aFunctionToGetThis, {
			get: (_, property) => {
				if ([Symbol.toPrimitive, "toString", "valueOf"].includes(property))
					return () => "";
				return aFunctionToGetThis();
			},
			set: () => true,
			has: () => true,
			ownKeys: () => [],
		});
	}
}) as new () => Any;
