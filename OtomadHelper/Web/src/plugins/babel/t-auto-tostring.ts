import type babelCore from "@babel/core";

interface Options {
	roots: string[];
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
	function isTRelated(node: babelCore.types.Node): boolean {
		if (t.isIdentifier(node)) return options.roots.includes(node.name);
		if (t.isMemberExpression(node)) return isTRelated(node.object);
		if (t.isCallExpression(node) && t.isIdentifier(node.callee)) return isTRelated(node.callee);
		return false;
	}

	return {
		name: "babel-plugin-t-auto-tostring",
		visitor: {
			// Handle `MemberExpression` (e.g. `t.foo` / `t[bar]` / `t.foo.bar`).
			MemberExpression(path, state) {
				const { node, parent, parentPath } = path, { filename } = state;
				if (!options) {
					options = state.opts as Options;
					options.roots ??= [];
					options.excludePaths ??= [];
				}
				if (filename && options.excludePaths.some(path => filename.includes(path))) return;

				// Ensure that it is the final node of the member expression chain
				// (the parent node is not MemberExpression).
				if (t.isMemberExpression(parent) && parent.property !== node) return;

				// Exclude cases where function calls already exist
				// (the parent node is `CallExpression` and the current node is `callee`).
				if (t.isCallExpression(parent) && parent.callee === node) return;

				// Exclude left-hand side of assignment statement
				// (should not add `()` on `t.foo = 123`).
				if (t.isAssignmentExpression(parent) && parent.left === node) return;

				// #region Special
				// Exclude: withObject(t.foo, t => t.bar)
				if (t.isCallExpression(parent) && t.isIdentifier(parent.callee) && parent.callee.name === "withObject") return;
				// Exclude: <ExpanderRadio xxxField={t.foo} />, <TransInterpolation i18nKey={t.foo} />
				if (t.isJSXExpressionContainer(parent) && t.isJSXAttribute(parentPath.parent) && t.isJSXIdentifier(parentPath.parent.name)) {
					const propName = parentPath.parent.name.name;
					if (propName.endsWith("Field") || propName === "i18nKey") return;
				}
				// #endregion

				// Recursively check if the root of the member expression is `t` related.
				if (!isTRelated(node)) return;

				// All conditions met: wrap member expressions into parameterless function calls.
				const callExpr = t.callExpression(node, []);
				path.replaceWith(callExpr);
			},
		},
	};
}
