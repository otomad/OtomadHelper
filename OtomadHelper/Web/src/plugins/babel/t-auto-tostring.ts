import type { PluginObj, default as babelCore } from "@babel/core";
import type { Identifier, Node } from "@babel/types";

const tRoots = ["t", "tf", "tAlias"] as const;

interface Options {
	excludePaths: string[];
}

export default function (babel: typeof babelCore): PluginObj {
	const { types: t } = babel;

	let options: Options = undefined!;
	const tAliases = new Map<string, Set<string>>();

	function AddToTAliases(filename: string, id: Identifier | string) {
		let aliases = tAliases.get(filename);
		if (!aliases) tAliases.set(filename, aliases = new Set());
		aliases.add(typeof id === "string" ? id : id.name);
	}

	/**
	 * Recursively check if the root of the member expression is `t` related.
	 * @param node - The node that to be checked.
	 * @param filename - Script code file name.
	 * @returns It is matched the expected root.
	 */
	function isTRelated(node: Node, filename?: string) {
		if (t.isIdentifier(node)) {
			if (tRoots.includes(node.name)) return node.name as typeof tRoots[number];
			if (filename && tAliases.get(filename)?.has(node.name)) return "t";
			return false;
		}
		if (t.isMemberExpression(node)) return isTRelated(node.object, filename);
		if (t.isCallExpression(node) && t.isIdentifier(node.callee)) return isTRelated(node.callee, filename);
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
			// Prioritize running this plugin by visiting the top-level Program node.
			Program(programPath, state) {
				const { filename } = state;
				if (!options) {
					options = state.opts as Options;
					options.excludePaths ??= [];
				}
				if (filename && options.excludePaths.some(path => filename.includes(path))) return;

				programPath.traverse({
					// Handle `MemberExpression` (e.g. `t.foo` / `t[bar]` / `t.foo.bar`).
					MemberExpression(path) {
						const { node, parent, parentPath } = path;

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
						// Exclude: { labelPrefix: t.foo }
						if (t.isObjectProperty(parent) && !parent.computed && t.isIdentifier(parent.key) && parent.key.name === "labelPrefix") return;
						// Exclude: Enum({ foo: { label: () => t.foo } }) // Note that do not use `Enum({ foo: { label: t.foo } })` !!!
						if (t.isObjectProperty(parent) && !parent.computed && t.isIdentifier(parent.key) && parent.key.name === "label")
							for (let ancestorPath = parentPath.parentPath; ancestorPath !== null; ancestorPath = ancestorPath.parentPath) {
								const ancestor = ancestorPath.node;
								if (t.isCallExpression(ancestor) && t.isIdentifier(ancestor.callee) && ancestor.callee.name === "Enum")
									return;
							}
						// #endregion

						// Recursively check if the root of the member expression is `t` related.
						const tCallInfo = isTRelated(node, filename);
						if (!tCallInfo) return;

						// Assign to an alias of a path of t (e.g. `const tEffects = tAlias.prve.effects;`).
						if (tCallInfo === "tAlias") {
							if (filename && t.isVariableDeclarator(parent) && t.isIdentifier(parent.id))
								AddToTAliases(filename, parent.id);
							return;
						}

						// All conditions met: wrap member expressions into parameterless function calls.
						const callExpr = t.optionalCallExpression(node, [], tCallInfo !== "t");
						path.replaceWith(callExpr);
					},

					CallExpression(path) {
						const { node, parent } = path;

						// Assign to an alias of a path of t (e.g. `const tFull = tAlias({ context: "full" });`).
						if (filename &&
							t.isIdentifier(node.callee) && node.callee.name === "tAlias" &&
							t.isVariableDeclarator(parent) && t.isIdentifier(parent.id))
							AddToTAliases(filename, parent.id);
					},
				});
			},
		},
	};
}
