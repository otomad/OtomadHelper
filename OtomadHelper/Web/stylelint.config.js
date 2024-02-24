// 标记为 /// 的配置在 StyleLint 16 中被移除，等待以后修正。

/** @type {import("stylelint").Config} */
export default {
	defaultSeverity: "error",
	extends: [
		"stylelint-config-standard",
	],
	customSyntax: "postcss-styled-syntax",
	plugins: [
		"stylelint-order",
	],
	rules: {
		// "media-feature-name-no-vendor-prefix": true, // 不要使用已被 autoprefixer 支持的浏览器前缀。
		"at-rule-no-vendor-prefix": true,
		"selector-no-vendor-prefix": true,
		"property-no-vendor-prefix": null,
		"value-no-vendor-prefix": null,
		/// "color-hex-case": "lower", // 颜色值要小写。
		"color-hex-length": "short", // 颜色值能短则短。
		"color-named": "always-where-possible", // 颜色若有名字则必须写为其名字。
		// "function-disallowed-list": [/^rgb/, /^hsl/, /^hwb/], // 只允许用 16 进制表示颜色，不允许使用 rgb、rgba 等表示颜色。
		/// "indentation": "tab",
		"length-zero-no-unit": true,
		"value-keyword-case": null, // 与 v-bind 冲突了。
		/// "value-list-comma-newline-after": null,
		/// "max-line-length": null,
		/// "no-eol-whitespace": [true, { "ignore": ["empty-lines"] }],
		"no-descending-specificity": null,
		"function-url-quotes": "always",
		/// "string-quotes": "double",
		/// "block-opening-brace-space-before": "always",
		/// "block-closing-brace-empty-line-before": "never",
		"import-notation": null,
		"at-rule-no-unknown": null,
		"function-no-unknown": null,
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
		"order/order": [
			{
				"type": "at-rule",
				"name": "include",
				"hasBlock": false,
			},
			"dollar-variables",
			"custom-properties",
			"declarations",
			// "at-rules", // <-- important, `@media` should go before `&:pseudo`
			"rules",
		],
		"order/properties-order": [[], { "severity": "warning" }],
	},
};
