import type babelCore from "@babel/core";

export const matrixAttrs = {
	feColorMatrix: ["values"],
	feConvolveMatrix: ["kernelMatrix"],
	feFuncR: ["tableValues"],
	feFuncG: ["tableValues"],
	feFuncB: ["tableValues"],
	feFuncA: ["tableValues"],
};

export function minifyMatrixValue(value: string) {
	value = value.trim().replaceAll(/\s+/g, " ");
	value = value.split(" ").map(n => n.replace(/^0+/, "").replace(/\.0*$/, "") || "0").join(" ");
	return value;
}

export default function (_babel: typeof babelCore): babelCore.PluginObj | undefined {
	const dev = process.env.NODE_ENV === "development";

	return {
		name: "babel-plugin-minify-svg-matrix",
		visitor: {
			JSXElement(path) {
				// Skip in development mode.
				if (dev) return;
				// Get the opening element from jsxElement node.
				const openingElement = path.node.openingElement;
				// TagName is name of tag like div, p etc.
				const tagName = (openingElement.name as babelCore.types.JSXIdentifier).name;
				if (!(tagName in matrixAttrs)) return;
				const expectedAttributes = matrixAttrs[tagName as keyof typeof matrixAttrs];
				for (const attribute of openingElement.attributes)
					if ("name" in attribute && expectedAttributes.includes(attribute.name.name as string))
						if (attribute.value?.type === "StringLiteral")
							attribute.value.value = minifyMatrixValue(attribute.value.value);
						else if (attribute.value?.type === "JSXExpressionContainer" && attribute.value.expression.type === "StringLiteral")
							attribute.value.expression.value = minifyMatrixValue(attribute.value.expression.value);
			},
		},
	};
}
