import _DynamicAutoSize from "./Transitions/DynamicAutoSize";
import _Size from "./Transitions/Size";

/** Transitions HOC (Higher-Order Components) Namespace. */
namespace Transitions {
	export /* @internal */ const DynamicAutoSize = _DynamicAutoSize;
	/** @deprecated */
	export /* @internal */ const Size = _Size;
}

export { Transitions };
