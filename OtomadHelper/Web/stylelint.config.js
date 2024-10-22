import propertiesOrder from "./stylelint-properties-order.js";

/** @type {import("stylelint").Config} */
export default {
	defaultSeverity: "error",
	extends: [
		"stylelint-config-standard",
	],
	customSyntax: "postcss-styled-syntax",
	plugins: [
		"stylelint-order",
		"@stylistic/stylelint-plugin",
	],
	rules: {
		// "media-feature-name-no-vendor-prefix": true, // 不要使用已被 autoprefixer 支持的浏览器前缀。
		"at-rule-no-vendor-prefix": true,
		"selector-no-vendor-prefix": true,
		"property-no-vendor-prefix": null,
		"value-no-vendor-prefix": null,
		"@stylistic/color-hex-case": "lower", // 颜色值要小写。
		"color-hex-length": "short", // 颜色值能短则短。
		"color-named": "always-where-possible", // 颜色若有名字则必须写为其名字。
		// "function-disallowed-list": [/^rgb/, /^hsl/, /^hwb/], // 只允许用 16 进制表示颜色，不允许使用 rgb、rgba 等表示颜色。
		// "@stylistic/indentation": ["tab", { baseIndentLevel: "auto" }],
		"length-zero-no-unit": true,
		"value-keyword-case": null, // 与 v-bind 冲突了。
		"@stylistic/value-list-comma-newline-after": null,
		"@stylistic/max-line-length": null,
		"@stylistic/no-eol-whitespace": [true, { "ignore": ["empty-lines"] }],
		"no-descending-specificity": null,
		"function-url-quotes": "always",
		"@stylistic/string-quotes": "double",
		"@stylistic/block-opening-brace-space-before": "always",
		// "@stylistic/block-closing-brace-empty-line-before": "never",
		"@stylistic/number-leading-zero": "always",
		"@stylistic/unit-case": "lower",
		"import-notation": null,
		"at-rule-no-unknown": null,
		"function-no-unknown": null,
		"property-no-unknown": [true, {
			"ignoreProperties": [
				"interpolate-size", // WARN: 等 StyleLint 支持该新属性后移除。
			],
		}],
		"declaration-empty-line-before": null,
		"custom-property-empty-line-before": null,
		"selector-pseudo-class-no-unknown": [true, {
			"ignorePseudoClasses": ["deep", "slotted", "global", "export", "vertical", "horizontal", "decrement", "increment", "component", "comp", "any-hover", "lang-latin"],
		}],
		"declaration-block-no-duplicate-properties": true,
		"declaration-block-no-duplicate-custom-properties": true,
		"font-family-no-duplicate-names": true,
		"keyframe-block-no-duplicate-selectors": true,
		"custom-property-no-missing-var-function": true,
		"keyframe-declaration-no-important": true,
		"font-family-no-missing-generic-family-keyword": true,
		"font-family-name-quotes": "always-where-recommended",
		"comment-empty-line-before": null,
		"function-calc-no-unspaced-operator": null, // 暂时解决一打 calc() 还没打完内容右下角就开始疯狂报错的问题。
		"at-rule-empty-line-before": ["always", {
			"except": ["first-nested"],
			"ignore": ["after-comment"],
			"ignoreAtRules": ["import", "include", "else", "return", "forward", "use", "debug", "extend"],
		}],
		"unit-disallowed-list": [
			"vw", "vh", "vmin", "vmax", // 请使用 dvw、dvh、dvmin、dvmax 代替之。
			"cm", "mm", "Q", "in", "pc", "pt", "mozmm", // 你觉得这种单位可能合理吗？
		],
		"rule-empty-line-before": null,
		"no-invalid-double-slash-comments": null,
		"number-max-precision": null,
		// "property-disallowed-list": ["direction"],
		"@stylistic/declaration-block-trailing-semicolon": "always",
		"@stylistic/declaration-block-semicolon-space-after": "always-single-line",
		"@stylistic/declaration-block-semicolon-space-before": "never",
		"@stylistic/declaration-colon-space-after": "always-single-line",
		"@stylistic/declaration-colon-space-before": "never",
		"@stylistic/function-comma-space-after": "always-single-line",
		"@stylistic/function-comma-space-before": "never",
		"@stylistic/function-parentheses-space-inside": "never-single-line",
		"@stylistic/media-feature-colon-space-after": "always",
		"@stylistic/media-feature-colon-space-before": "never",
		"@stylistic/media-feature-parentheses-space-inside": "never",
		"@stylistic/media-feature-range-operator-space-after": "always",
		"@stylistic/media-feature-range-operator-space-before": "always",
		"@stylistic/media-query-list-comma-space-after": "always-single-line",
		"@stylistic/media-query-list-comma-space-before": "never",
		"@stylistic/selector-attribute-brackets-space-inside": "never",
		"@stylistic/selector-attribute-operator-space-after": "never",
		"@stylistic/selector-attribute-operator-space-before": "never",
		"@stylistic/selector-combinator-space-after": "always",
		"@stylistic/selector-combinator-space-before": "always",
		"@stylistic/selector-list-comma-newline-after": "always",
		"@stylistic/selector-list-comma-newline-before": "never-multi-line",
		"@stylistic/selector-list-comma-space-after": "always-single-line",
		"@stylistic/selector-list-comma-space-before": "never",
		"@stylistic/selector-pseudo-class-parentheses-space-inside": "never",
		"@stylistic/value-list-comma-space-after": "always-single-line",
		"@stylistic/value-list-comma-space-before": "never",
		"order/order": [
			{
				"type": "at-rule",
				"name": "include",
				"hasBlock": false,
			},
			// "dollar-variables",
			"custom-properties",
			"declarations",
			// "at-rules", // <-- important, `@media` should go before `&:pseudo`
			"rules",
		],
		"order/properties-order": [propertiesOrder, { "unspecified": "bottom", "severity": "warning" }],
	},
};
