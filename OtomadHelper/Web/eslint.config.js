// env
import globals from "globals";
// extends
import eslint from "@eslint/js";
import stylistic from "@stylistic/eslint-plugin";
import reactRefresh from "eslint-plugin-react-refresh";
import reactRecommended from "eslint-plugin-react/configs/recommended.js";
import tseslint from "typescript-eslint";

/** @type {import("eslint").Linter.FlatConfig[]} */
export default [
	eslint.configs.recommended,
	...tseslint.configs.recommended,
	reactRecommended,
	stylistic.configs.customize({
		indent: "tab",
		quotes: "double",
		semi: true,
	}),
	// plugin:react-hooks/recommended
	{
		languageOptions: {
			parserOptions: {
				ecmaFeatures: {
					jsx: true,
				},
			},
			ecmaVersion: "latest",
			sourceType: "module",
			globals: {
				...globals.browser,
				...globals.node,
			},
		},
		files: ["**/*.{js,jsx,ts,tsx}"],
		plugins: {
			"react-refresh": reactRefresh,
		},
		rules: {
			"@stylistic/indent": ["error", "tab", {
				"SwitchCase": 1,
				"flatTernaryExpressions": true,
				"ignoredNodes": [
					"TaggedTemplateExpression *", // 修复 styled-components 的字符串模板缩进问题。
				],
			}],
			"@stylistic/linebreak-style": ["error", "unix"],
			"@stylistic/quotes": ["error", "double", { "avoidEscape": true }],
			"@stylistic/semi": ["error", "always"],
			"@stylistic/array-bracket-spacing": ["error", "never"],
			"@stylistic/brace-style": ["error", "1tbs", { "allowSingleLine": true }],
			"@stylistic/comma-dangle": ["error", "always-multiline"],
			"@stylistic/comma-spacing": ["error", { "before": false, "after": true }],
			"@stylistic/comma-style": ["error", "last"],
			"@stylistic/eol-last": "error",
			"default-case": "error",
			"no-duplicate-case": "error",
			"no-eq-null": "error",
			"@stylistic/no-floating-decimal": "error",
			"@stylistic/no-mixed-spaces-and-tabs": ["error", false],
			"no-var": "error",
			"no-unused-vars": "off",
			"@stylistic/no-tabs": "off",
			"no-empty": ["error", { "allowEmptyCatch": true }],
			"no-constant-condition": ["error", { "checkLoops": false }],
			"eqeqeq": "error",
			"prefer-const": ["error", { "destructuring": "all" }],
			"for-direction": "error",
			"getter-return": "error",
			"no-compare-neg-zero": "error",
			"no-cond-assign": ["error", "except-parens"],
			"@stylistic/no-extra-semi": "error",
			"no-irregular-whitespace": "error",
			"no-unreachable": "warn",
			"use-isnan": "error",
			"valid-typeof": "error",
			"curly": ["error", "multi"],
			"no-lonely-if": "off",
			"dot-notation": ["error"],
			"guard-for-in": "error",
			"no-extra-label": "error",
			"require-await": "error",
			"yoda": "error",
			"@stylistic/block-spacing": "error",
			"@stylistic/func-call-spacing": ["error", "never"],
			"@stylistic/computed-property-spacing": ["error", "never"],
			"@stylistic/no-whitespace-before-property": "error",
			"@stylistic/object-curly-spacing": ["error", "always"],
			"@stylistic/padded-blocks": ["error", "never"],
			"@stylistic/quote-props": ["error", "as-needed"],
			"@stylistic/semi-spacing": "error",
			"@stylistic/semi-style": ["error", "last"],
			"@stylistic/space-before-function-paren": ["error", {
				"anonymous": "always",
				"named": "never",
				"asyncArrow": "always",
			}],
			"@stylistic/space-infix-ops": "error",
			"@stylistic/space-in-parens": ["error", "never"],
			"@stylistic/space-unary-ops": "error",
			"unicode-bom": ["error", "never"],
			"@stylistic/arrow-spacing": "error",
			"require-yield": "error",
			"@stylistic/yield-star-spacing": ["error", "after"],
			"symbol-description": "error",
			"@stylistic/template-tag-spacing": "error",
			"@stylistic/switch-colon-spacing": "error",
			"@stylistic/keyword-spacing": "error",
			"@stylistic/key-spacing": "error",
			"@stylistic/jsx-quotes": "error",
			"@stylistic/no-multi-spaces": "error",
			"@stylistic/dot-location": ["error", "property"],
			"no-loss-of-precision": "error",
			"no-useless-concat": "error",
			"object-shorthand": "error",
			"prefer-template": "off",
			"@stylistic/template-curly-spacing": "error",
			"no-undef": "off", // 这波 nuxt 的锅。
			"@stylistic/multiline-ternary": "off",
			"@stylistic/operator-linebreak": "off",
			"@stylistic/no-trailing-spaces": ["error", { "skipBlankLines": true }],
			"one-var": "off",
			"@stylistic/arrow-parens": ["error", "as-needed"],
			"camelcase": "off",
			"@stylistic/spaced-comment": ["error", "always", {
				"exceptions": ["+", "-", "*", "/"],
				"markers": ["/", "!", "@", "#", "#region", "#endregion"],
			}],
			"radix": "error", // parseInt 必须要指明是十进制。
			"no-self-assign": "off",
			"no-debugger": "warn",
			"no-use-before-define": "off",
			"accessor-pairs": "off",
			"no-empty-function": "off",
			"no-inner-declarations": "warn",
			"no-unmodified-loop-condition": "off",
			"no-return-assign": "off",
			"no-redeclare": "off",
			"@stylistic/no-mixed-operators": "off",
			"@stylistic/no-extra-parens": ["error", "all", { "ignoreJSX": "multi-line" }],
			"no-void": ["off", { "allowAsStatement": true }], // 我就是要使用 void。
			"no-labels": "off",
			"default-case-last": "off",
			"no-useless-constructor": "off", // private constructor() { } 你跟我说无用？
			"@stylistic/no-multiple-empty-lines": ["error", { max: 1, maxEOF: 0, maxBOF: 0 }],
			"@stylistic/max-statements-per-line": "off",
			// "capitalized-comments": ["error", "always", { "ignorePattern": "internal" }],
			"import/order": "off", // 与 VSCode 内置导入排序特性打架。
			"import/first": "off", // 与 Vue 特性冲突。
			"import/named": "off", // 与 TypeScript 特性冲突。
			"import/no-named-as-default": "off", // 似乎与文件命名方式有点出入。
			"import/no-named-as-default-member": "off", // 某些库在导出成员时用 TS 命名空间欺诈。
			"n/no-callback-literal": "off", // 这是啥？
			"unicorn/escape-case": "off", // 暂时禁用，待修复。
			"unicorn/number-literal-case": "off", // 同上，你真的觉得大写很好看吗？
			"@typescript-eslint/no-unused-vars": ["warn", { // 非要使用未使用变量，前面加下划线。
				"argsIgnorePattern": "^_",
				"varsIgnorePattern": "^_",
				"caughtErrorsIgnorePattern": "^_",
			}],
			"@typescript-eslint/no-inferrable-types": ["error", { ignoreParameters: true, ignoreProperties: true }],
			"@typescript-eslint/no-non-null-assertion": "off",
			"@typescript-eslint/triple-slash-reference": "off",
			"@typescript-eslint/ban-ts-comment": "off",
			"@typescript-eslint/ban-types": "off",
			"@typescript-eslint/no-namespace": "off",
			"@typescript-eslint/consistent-type-assertions": ["error", { assertionStyle: "as" }],
			"@typescript-eslint/no-confusing-non-null-assertion": "error",
			"@typescript-eslint/no-duplicate-enum-values": "error",
			"@typescript-eslint/no-empty-interface": "off",
			"@stylistic/member-delimiter-style": ["error", {
				"multiline": {
					"delimiter": "semi",
					"requireLast": true,
				},
				"singleline": {
					"delimiter": "semi",
					"requireLast": false,
				},
			}],
			"@typescript-eslint/no-explicit-any": "error",
			"@typescript-eslint/no-use-before-define": ["warn", {
				"functions": false,
			}],
			"@typescript-eslint/no-empty-function": "off",
			"@typescript-eslint/no-redeclare": "warn",
			"@typescript-eslint/no-useless-constructor": "error",
			"@typescript-eslint/no-this-alias": "off",
			"@stylistic/indent-binary-ops": "error",
			"@stylistic/type-generic-spacing": "error",
			"@stylistic/type-named-tuple-spacing": "error",
			"react-refresh/only-export-components": [
				"off",
				{ allowConstantExport: true },
			],
			"react-hooks/exhaustive-deps": "off",
			"react-hooks/rules-of-hooks": "off",
			"react/react-in-jsx-scope": "off",
			"react/prop-types": "off",
			"react/jsx-no-undef": "off",
			"react/button-has-type": "error",
			"react/jsx-boolean-value": ["error", "never"],
			"react/jsx-key": "error",
			"react/jsx-fragments": ["error", "syntax"],
			"@stylistic/jsx-closing-bracket-location": ["error", "tag-aligned"],
			"@stylistic/jsx-closing-tag-location": "error",
			"@stylistic/jsx-curly-spacing": ["error", { "when": "never", "children": true }],
			"@stylistic/jsx-equals-spacing": ["error", "never"],
			"@stylistic/jsx-first-prop-new-line": ["error", "multiline"],
			"@stylistic/jsx-indent-props": ["error", "tab"],
			"@stylistic/jsx-indent": ["error", "tab", { checkAttributes: true, indentLogicalExpressions: true }],
			"@stylistic/jsx-max-props-per-line": ["error", { "maximum": 5 }],
			"@stylistic/jsx-pascal-case": ["error", { allowLeadingUnderscore: true, allowNamespace: true }],
			"@stylistic/jsx-props-no-multi-spaces": "error",
			"@stylistic/jsx-tag-spacing": ["error", {
				"closingSlash": "never",
				"beforeSelfClosing": "always",
				"afterOpening": "never",
				"beforeClosing": "never",
			}],
			"@stylistic/jsx-one-expression-per-line": ["error", { "allow": "single-child" }],
			"react/self-closing-comp": ["error", { "component": true }],
			"no-restricted-properties": ["error", {
				object: "arguments",
				property: "callee",
				message: "arguments.callee is deprecated.",
			}, {
				object: "global",
				property: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				object: "self",
				property: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				object: "window",
				property: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				object: "globalThis",
				property: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				object: "global",
				property: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				object: "self",
				property: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				object: "window",
				property: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				object: "globalThis",
				property: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				property: "__defineGetter__",
				message: "Please use Object.defineProperty instead.",
			}, {
				property: "__defineSetter__",
				message: "Please use Object.defineProperty instead.",
			}, {
				object: "Math",
				property: "pow",
				message: "Use the exponentiation operator (**) instead.",
			}],
			"no-restricted-globals": ["error", {
				name: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				name: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				name: "addEventListener",
				message: "Please use window.addEventListener instead.",
			}, {
				name: "innerHeight",
				message: "Please use window.innerHeight instead.",
			}, {
				name: "outerHeight",
				message: "Please use window.outerHeight instead.",
			}, {
				name: "innerWidth",
				message: "Please use window.innerWidth instead.",
			}, {
				name: "outerWidth",
				message: "Please use window.outerWidth instead.",
			}],
		},
	},
	{
		files: ["*.config.{js,ts}"],
		rules: {
			"@stylistic/quote-props": "off",
		},
	},
	{
		files: ["**/*.d.ts"],
		rules: {
			"@typescript-eslint/no-explicit-any": "off",
		},
	},
	{
		ignores: [
			"dist/*",
			".output/*",
			"node_modules/*",
			"**/dist/*",
		],
	},
];
