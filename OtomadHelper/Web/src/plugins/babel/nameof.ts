/*
 * NOTE: Wait for https://github.com/typescript-nameof/nameof/issues/16 to replace this.
 */

import type { NodePath, PluginObj, default as babelCore } from "@babel/core";
import type { CallExpression, Identifier, MemberExpression, Node, OptionalMemberExpression, ThisExpression } from "@babel/types";
import VariableName from "variable-name-conversion";

export default function (babel: typeof babelCore): PluginObj {
	const { types: t } = babel;

	// Check if the node is a member expression (`foo.bar`) or an optional member expression (`foo?.bar`).
	const isMemberOrOptionalMember = (node: Node) => t.isMemberExpression(node) || t.isOptionalMemberExpression(node);
	const isStringNumberBigInt = (node: Node) => t.isStringLiteral(node) || t.isNumericLiteral(node) || t.isBigIntLiteral(node);

	function getMemberExpressionNames(node: MemberExpression | OptionalMemberExpression) {
		const { object, property } = node;
		const children: string[] = [];
		if (t.isIdentifier(property)) children.unshift(property.name);
		else if (isStringNumberBigInt(property)) children.unshift(String(property.value));
		if (t.isIdentifier(object)) children.unshift(object.name);
		else if (isMemberOrOptionalMember(object)) children.unshift(...getMemberExpressionNames(object));
		return children;
	}

	type CallExpressionNodePath = NodePath<babelCore.types.CallExpression>;
	// Deconstruct Arrow Functions.
	function resolveTargetExpression(arg: CallExpression["arguments"][number], path: CallExpressionNodePath) {
		// () => x.y.z
		if (t.isArrowFunctionExpression(arg)) {
			// Arrow Function cannot contain function body (braces).
			if (!t.isExpression(arg.body)) throw path.buildCodeFrameError("`nameof` only supports arrow function without braces `() => expr`");
			return arg.body;
		}

		// x.y.z
		return arg;
	}

	// Recursive extract the final property name of Member Expression
	function extractFinalName(node: Node) {
		// x.y.z / x.y?.z → z
		if (t.isMemberExpression(node) || t.isOptionalMemberExpression(node))
			return extractFinalName(node.property);

		// .foo
		if (t.isIdentifier(node))
			return node.name;

		// ["bar"] / [1] / [1n]
		if (isStringNumberBigInt(node))
			return String(node.value);

		return null;
	}

	type ValidExpression = Identifier | MemberExpression | ThisExpression;
	// Check if the expression is valid.
	function validateExpression(node: Node, path: CallExpressionNodePath): asserts node is ValidExpression {
		const validTypes = [
			"Identifier",
			"MemberExpression",
			"OptionalMemberExpression",
			"ThisExpression",
		] satisfies Node["type"][];

		if (!validTypes.includes(node.type))
			throw path.buildCodeFrameError("`nameof` only supports identifier, member expression, directly or returned by arrow function");
	}

	const cases = ["kebab", "snake", "camel", "pascal"] as const;
	interface TransformOptions {
		case?: typeof cases[number];
	}
	function transform(path: CallExpressionNodePath, { case: caseType }: TransformOptions = {}) {
		const { arguments: args } = path.node;
		if (args.length !== 1) throw path.buildCodeFrameError("`nameof` function only accept one argument");
		const targetExpr = resolveTargetExpression(args[0], path);
		validateExpression(targetExpr, path);
		let finalName = extractFinalName(targetExpr);
		if (!finalName) throw path.buildCodeFrameError("`nameof` cannot extract a valid name from expression");
		if (caseType) finalName = new VariableName(finalName)[caseType];
		path.replaceWith(t.stringLiteral(finalName));
	}

	return {
		name: "babel-plugin-nameof",
		visitor: {
			// Prioritize running this plugin by visiting the top-level Program node.
			Program(programPath) {
				programPath.traverse({
					CallExpression(path) {
						const { callee } = path.node;
						if (t.isIdentifier(callee) && callee.name === "nameof")
							transform(path);
						else if (isMemberOrOptionalMember(callee)) {
							const members = getMemberExpressionNames(callee);
							if (members[0] === "nameof")
								transform(path, { case: members.find(property => cases.includes(property)) });
						}
					},
				});
			},
		},
	};
}

type NameOf = (obj: Any) => string;
declare global {
	const nameof: NameOf & {
		kebab: NameOf;
		snake: NameOf;
		camel: NameOf;
		pascal: NameOf;
	};
}
