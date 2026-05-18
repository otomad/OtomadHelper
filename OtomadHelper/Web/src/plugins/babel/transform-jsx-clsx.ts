import type { PluginObj, default as babelCore } from "@babel/core";
import type { Expression, ObjectProperty, SpreadElement } from "@babel/types";

// 辅助函数：将驼峰式命名转换为连字符式 (e.g., isSelected -> is-selected)
function toKebabCase(str: string) {
	return str.replaceAll(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase();
}

export default function (babel: typeof babelCore): PluginObj {
	const { types: t } = babel;

	// 处理单个对象属性，将其转换为 clsx 的最佳性能表达式
	function transformObjectProperty(prop: ObjectProperty) {
		let keyStr = "";

		// 处理键名：计算属性 {[computed]: true}、字面量 {"foo-bar": true} 或标识符 {fooBar: true}
		if (prop.computed)
			if (t.isStringLiteral(prop.key))
				keyStr = toKebabCase(prop.key.value);
			else
				// 动态键名不转换，保持原样
				return t.logicalExpression("&&", prop.value as Expression, prop.key as Expression);

		else if (t.isIdentifier(prop.key))
			keyStr = toKebabCase(prop.key.name);
		else if (t.isStringLiteral(prop.key))
			keyStr = toKebabCase(prop.key.value);

		const keyLiteral = t.stringLiteral(keyStr);

		// 如果值是布尔字面量 true，直接返回键名字符串
		if (t.isBooleanLiteral(prop.value, { value: true }))
			return keyLiteral;

		// 如果值是布尔字面量 false，返回 null（后续会被过滤掉）
		if (t.isBooleanLiteral(prop.value, { value: false }))
			return null;

		// 核心优化：转换为 `value && "key-str"`
		return t.logicalExpression("&&", prop.value as Expression, keyLiteral);
	}

	// 递归拉平数组和对象
	function flattenExpression(node: SpreadElement | Expression): (SpreadElement | Expression)[] {
		// 1. 处理数组字面量 [...]
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

		// 2. 处理对象字面量 {...}
		if (t.isObjectExpression(node)) {
			const elements = [];
			for (const prop of node.properties)
				if (t.isObjectProperty(prop)) {
					const transformed = transformObjectProperty(prop);
					if (transformed) elements.push(transformed);
				} else if (t.isSpreadElement(prop))
					// 如果存在对象解构 {...props}，保留原样传给 clsx，不作处理
					elements.push(prop.argument);

			return elements;
		}

		// 3. 基础表达式 (如字符串、逻辑表达式、标识符)，直接返回
		return node as never;
	}

	return {
		name: "transform-jsx-clsx",
		visitor: {
			Program: {
				enter(path, state) {
					// 初始化状态标记
					state.hasClsxImport = false;
					state.needsClsxImport = false;

					path.traverse({
						// 1. 优先扫描并处理整个文件的 JSX className 属性
						// 此时 React Compiler 还未介入，JSX 仍然保持原始的对象/数组字面量形态
						JSXAttribute(path) {
							// 仅处理 className 属性
							if (path.node.name.name !== "className") return;

							// 确保属性值包裹在 {} 中
							const valuePath = path.get("value");
							if (!valuePath.isJSXExpressionContainer()) return;

							const expressionPath = valuePath.get("expression");
							const node = expressionPath.node;

							// 仅处理数组或对象字面量
							if (!t.isArrayExpression(node) && !t.isObjectExpression(node)) return;

							// 执行拉平与转换操作
							const flatElements = flattenExpression(node);
							const args = Array.isArray(flatElements) ? flatElements : [flatElements];

							// 核心优化 1：过滤掉可能存在的无用节点（如被排除的布尔假值）
							const validArgs = args.filter(arg => arg !== null);

							// 如果转换后没有任何有效参数，则移除或设为空串（转换为 className=""）
							if (validArgs.length === 0) {
								path.set("value", t.stringLiteral(""));
								return;
							}

							// 核心优化 2：检查是否全部都是字符串字面量
							const isAllStrings = validArgs.every(arg => t.isStringLiteral(arg));

							if (isAllStrings) {
								// 将所有字符串字面量的值取出，去重、过滤空串，并用空格拼接
								const combinedString = Array.from(
									new Set(validArgs.map(arg => arg.value).filter(Boolean)),
								).join(" ");

								// 彻底移除 {} 表达式容器，降级为普通的 className="xxx"
								path.set("value", t.stringLiteral(combinedString));
								return;
							}

							// 核心改动：一旦需要生成 clsx(...) 函数调用，标记为需要导入
							state.needsClsxImport = true;

							// 核心优化 3：如果有静态字符串和动态表达式，保留静态字符串可以减少 clsx 内部的字符串拼接
							// (注：这里保持原样传入 clsx 也是标准做法，可由 clsx 运行时处理)
							// 构建 clsx(...) 函数调用
							const clsxCall = t.callExpression(t.identifier("clsx"), validArgs);
							// 替换原有的 JSX 表达式
							expressionPath.replaceWith(clsxCall);
						},

						// 2. 遍历顶层节点，检查用户是否已经手动导入了 clsx 相关的库
						ImportDeclaration(importPath) {
							if (state.hasClsxImport) return;
							const source = importPath.node.source.value;
							// 兼容用户手动引入 'clsx' 或 'clsx/lite' 的情况
							if (source === "clsx" || source === "clsx/lite")
								state.hasClsxImport = true;
						},
					});
				},

				exit(path, state) {
					// 如果代码中触发了 clsx 转换，且用户没手动导入，则在顶部插入导入语句
					if (state.needsClsxImport && !state.hasClsxImport) {
						const importDeclaration = t.importDeclaration(
							[t.importDefaultSpecifier(t.identifier("clsx"))],
							t.stringLiteral("clsx/lite"), // 推荐使用性能更好的精简版
						);
						path.unshiftContainer("body", importDeclaration);
					}
				},
			},
		},
	};
}
