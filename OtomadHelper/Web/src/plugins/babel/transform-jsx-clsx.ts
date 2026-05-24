/*
 * Similar to `babel-plugin-transform-jsx-classnames`, but use `clsx` rather than `classnames`, and optimize more.
 */

import type { PluginObj, default as babelCore } from "@babel/core";
import type { Expression, ObjectProperty, SpreadElement } from "@babel/types";

// Util function: convert the camelCase to kebab-case. (e.g., isSelected -> is-selected)
function toKebabCase(str: string) {
	return str.replaceAll(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase();
}

export default function (babel: typeof babelCore): PluginObj {
	const { types: t } = babel;

	// Process the single object properties, transform it to the best performance expression in `clsx`.
	function transformObjectProperty(prop: ObjectProperty) {
		let keyStr = "";

		// Process the object key: computed key `{ [computed]: true }`, literal `{ "foo-bar": true }` or identifier `{ fooBar: true }`.
		if (prop.computed)
			if (t.isStringLiteral(prop.key))
				keyStr = toKebabCase(prop.key.value);
			else
				// Keep computed key unchanged.
				return t.logicalExpression("&&", prop.value as Expression, prop.key as Expression);

		else if (t.isIdentifier(prop.key))
			keyStr = toKebabCase(prop.key.name);
		else if (t.isStringLiteral(prop.key))
			keyStr = toKebabCase(prop.key.value);

		const keyLiteral = t.stringLiteral(keyStr);

		// If boolean literal `true`, return the key string directly.
		if (t.isBooleanLiteral(prop.value, { value: true }))
			return keyLiteral;

		// If boolean literal `false`, return `null` (will be filtered out later).
		if (t.isBooleanLiteral(prop.value, { value: false }))
			return null;

		// Core optimization: Transform to `value && "key-str"`.
		return t.logicalExpression("&&", prop.value as Expression, keyLiteral);
	}

	// Recursively flatten arrays and objects.
	function flattenExpression(node: SpreadElement | Expression): (SpreadElement | Expression)[] {
		// 1. Process array literal `[...]`.
		if (t.isArrayExpression(node)) {
			const elements: (SpreadElement | Expression)[] = [];
			for (const el of node.elements) {
				if (!el) continue;
				const result = flattenExpression(el);
				if (Array.isArray(result))
					elements.push(...result);
				else if (result)
					elements.push(result);
			}
			return elements;
		}

		// 2. Process object literal `{...}`.
		if (t.isObjectExpression(node)) {
			const elements = [];
			for (const prop of node.properties)
				if (t.isObjectProperty(prop)) {
					const transformed = transformObjectProperty(prop);
					if (transformed) elements.push(transformed);
				} else if (t.isSpreadElement(prop))
					// If there is object deconstruct `{ ...props }`, pass it originally to `clsx` without any processing.
					elements.push(prop.argument);

			return elements;
		}

		// 3. Return basic expression (e.g. string, logic expression, identifier) directly.
		return node as never;
	}

	return {
		name: "transform-jsx-clsx",
		visitor: {
			Program: {
				enter(path, state) {
					// Initialize state flags.
					state.hasClsxImport = false;
					state.needsClsxImport = false;

					path.traverse({
						// 1. Prioritize scanning and processing the JSX className props in the entire file.
						// React Compiler haven't been introduced at this moment, the JSX still maintain the original object or array literal form.
						JSXAttribute(path) {
							// Process className prop only.
							if (path.node.name.name !== "className") return;

							// Ensure the property values are enclosed in a `{}`.
							const valuePath = path.get("value");
							if (!valuePath.isJSXExpressionContainer()) return;

							const expressionPath = valuePath.get("expression");
							const node = expressionPath.node;

							// Process array and object literal only.
							if (!t.isArrayExpression(node) && !t.isObjectExpression(node)) return;

							// Execute the flatten and transform operation.
							const flatElements = flattenExpression(node);
							const args = Array.isArray(flatElements) ? flatElements : [flatElements];

							// Core optimization 1: Filter out the useless nodes (i.e. the excluded bool false value.)
							const validArgs = args.filter(arg => arg !== null);

							// If there are no any valid args after transformation, then remove them or set to empty string (transform to `className=""`).
							if (validArgs.length === 0) {
								path.set("value", t.stringLiteral(""));
								return;
							}

							// Core optimization 2: Check if every args are string literals.
							const isAllStrings = validArgs.every(arg => t.isStringLiteral(arg));

							if (isAllStrings) {
								// Take all string literals out, deduplication, filter the empty strings out, then joined with spaces.
								const combinedString = Array.from(
									new Set(validArgs.map(arg => arg.value).filter(Boolean)),
								).join(" ");

								// Remove the `{}` expression container thoroughly, downgrade to the normal `className="xxx"`.
								path.set("value", t.stringLiteral(combinedString));
								return;
							}

							// Core change: Once `clsx(...)` function applying required to generate, mark it need to import.
							state.needsClsxImport = true;

							// Core optimization 3: If there are static strings and computed expressions,
							// keep static strings would reduce the internal string concatenating in `clsx`.
							// (Note: Here keeps directly pass in `clsx` is also standard method, which can be processed in clsx runtime.)
							// Construct `clsx(...)` function applying.
							const clsxCall = t.callExpression(t.identifier("clsx"), validArgs);
							// Substitute the original JSX expression.
							expressionPath.replaceWith(clsxCall);
						},

						// 2. Traverse the top nodes, check if the user has already import the modules of clsx manually.
						ImportDeclaration(importPath) {
							if (state.hasClsxImport) return;
							const source = importPath.node.source.value;
							// Compatible with the situation when the user manually importing the "clsx" or "clsx/lite".
							if (source === "clsx" || source === "clsx/lite")
								state.hasClsxImport = true;
						},
					});
				},

				exit(path, state) {
					// If the code triggers clsx transformation, and user doesn't import manually, then insert the import statement at the top.
					if (state.needsClsxImport && !state.hasClsxImport) {
						const importDeclaration = t.importDeclaration(
							[t.importDefaultSpecifier(t.identifier("clsx"))],
							t.stringLiteral("clsx/lite"), // Suggest to use the lite version which has a better performance.
						);
						path.unshiftContainer("body", importDeclaration);
					}
				},
			},
		},
	};
}
