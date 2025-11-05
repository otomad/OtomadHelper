import type babelCore from "@babel/core";
type Node = babelCore.types.Node;

const tRoots = ["t", "tf"] as const;

interface Options {
	excludePaths: string[];
}

export default function (babel: typeof babelCore): babelCore.PluginObj {
	const { types: t } = babel;

	let options: Options = undefined!;

	/**
	 * Recursively check if the root of the member expression is `t` related.
	 * @param node - The node that to be checked.
	 * @returns It is matched the expected root.
	 */
	function isTRelated(node: Node) {
		if (t.isIdentifier(node))
			return tRoots.includes(node.name) ? node.name as typeof tRoots[number] : false;
		if (t.isMemberExpression(node)) return isTRelated(node.object);
		if (t.isCallExpression(node) && t.isIdentifier(node.callee)) return isTRelated(node.callee);
		return false;
	}

	/**
	 * Check if the node is a call expression (`foo()`) or an optional call expression (`foo?.()`).
	 * @param node - The node that to be checked.
	 * @returns The node is a call expression or an optional call expression.
	 */
	function isCallOrOptionalCall(node: Node) {
		return t.isCallExpression(node) || t.isOptionalCallExpression(node);
	}

	/**
	 * Check if the node is a member expression (`foo.bar`) or an optional member expression (`foo?.bar`).
	 * @param node - The node that to be checked.
	 * @returns The node is a member expression or an optional member expression.
	 */
	function isMemberOrOptionalMember(node: Node) {
		return t.isMemberExpression(node) || t.isOptionalMemberExpression(node);
	}

	return {
		name: "babel-plugin-t-auto-tostring",
		visitor: {
			// Handle `MemberExpression` (e.g. `t.foo` / `t[bar]` / `t.foo.bar`).
			MemberExpression(path, state) {
				const { node, parent, parentPath } = path, { filename } = state;
				if (!options) {
					options = state.opts as Options;
					options.excludePaths ??= [];
				}
				if (filename && options.excludePaths.some(path => filename.includes(path))) return;

				// Ensure that it is the final node of the member expression chain
				// (the parent node is not MemberExpression).
				if (t.isMemberExpression(parent) && parent.property !== node) return;

				// Exclude cases where function calls already exist
				// (the parent node is `CallExpression` and the current node is `callee`).
				if (isCallOrOptionalCall(parent) && parent.callee === node) return;

				// Exclude left-hand side of assignment statement
				// (should not add `()` on `t.foo = 123`).
				if (t.isAssignmentExpression(parent) && parent.left === node) return;

				// #region Special
				// Exclude: <ExpanderRadio xxxField={t.foo} />, <TransInterpolation i18nKey={t.foo} />
				if (t.isJSXExpressionContainer(parent) && t.isJSXAttribute(parentPath.parent) && t.isJSXIdentifier(parentPath.parent.name)) {
					const propName = parentPath.parent.name.name;
					if (propName.endsWith("Field") || propName === "i18nKey") return;
				}
				// withObject(t.foo, t => t.bar) --> withObject(t.foo, t => t.bar).toString()
				if (isCallOrOptionalCall(parent) && t.isIdentifier(parent.callee) && parent.callee.name === "withObject") {
					if (isMemberOrOptionalMember(parentPath.parent)) return;
					const callExpr = t.callExpression(t.memberExpression(parent, t.identifier("toString")), []);
					parentPath.replaceWith(callExpr);
				}
				// #endregion

				// Recursively check if the root of the member expression is `t` related.
				const tCallInfo = isTRelated(node);
				if (!tCallInfo) return;

				// All conditions met: wrap member expressions into parameterless function calls.
				const callExpr = t.optionalCallExpression(node, [], tCallInfo !== "t");
				path.replaceWith(callExpr);
			},
		},
	};
}
