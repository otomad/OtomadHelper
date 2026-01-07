// env
import globals from "globals";
// extends
import eslint from "@eslint/js";
import tseslint from "typescript-eslint";
import stylistic from "@stylistic/eslint-plugin";
import importPlugin from "eslint-plugin-import";
import reactRecommended from "eslint-plugin-react/configs/recommended.js";
import jsdoc from "eslint-plugin-jsdoc";
// plugins
import reactRefresh from "eslint-plugin-react-refresh";
import unicorn from "eslint-plugin-unicorn";
import reactHooks from "eslint-plugin-react-hooks";

/** @type {import("eslint").Linter.Config[]} */
export default [
	eslint.configs.recommended,
	...tseslint.configs.recommended,
	reactRecommended,
	importPlugin.flatConfigs.warnings,
	jsdoc.configs["flat/recommended-typescript"],
	// Stylistic 简单规则
	stylistic.configs.customize({
		indent: "tab",
		quotes: "double",
		semi: true,
	}),
	// 主规则
	{
		languageOptions: {
			parserOptions: {
				ecmaFeatures: {
					jsx: true,
				},
				project: true,
				tsconfigRootDir: import.meta.dirname,
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
			unicorn,
			"react-hooks": reactHooks,
		},
		linterOptions: {
			reportUnusedInlineConfigs: "error",
			reportUnusedDisableDirectives: "error",
		},
		settings: {
			react: {
				version: "detect",
			},
			jsdoc: {
				tagNamePreference: {
					arg: "param",
					return: "returns",
					typeParam: "template",
					params: "param",
					remark: "remarks",
					notes: "note",
					warning: "warn",
					throw: "throws",
					yield: "yields",
					defaults: "default",
				},
			},
		},
		rules: {
			"@stylistic/indent": ["error", "tab", {
				"SwitchCase": 1,
				"flatTernaryExpressions": true,
				"ignoredNodes": [
					"TaggedTemplateExpression *", // 修复 styled-components 的字符串模板缩进问题。
					"TSFunctionType *", // stylistic typescript indent bug
					"TSMappedType *", // stylistic typescript indent bug
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
			"no-eq-null": "off",
			"@stylistic/no-floating-decimal": "error",
			"@stylistic/no-mixed-spaces-and-tabs": ["error", false],
			"no-var": "error",
			"no-unused-vars": "off",
			"@stylistic/no-tabs": "off",
			"no-empty": ["error", { "allowEmptyCatch": true }],
			"no-constant-condition": ["error", { "checkLoops": false }],
			"eqeqeq": ["error", "always", { "null": "ignore" }],
			"prefer-const": ["error", { "destructuring": "all" }],
			"for-direction": "error",
			"getter-return": "error",
			"no-compare-neg-zero": "error",
			"no-cond-assign": ["error", "except-parens"],
			"@stylistic/no-extra-semi": "error",
			"no-irregular-whitespace": "error",
			"no-unreachable": "warn",
			"use-isnan": ["error", { "enforceForSwitchCase": true, "enforceForIndexOf": true }],
			"valid-typeof": "error",
			"curly": ["error", "multi"],
			"no-lonely-if": "off",
			"dot-notation": ["error"],
			"guard-for-in": "error",
			"no-extra-label": "off",
			"require-await": "error",
			"yoda": "error",
			"@stylistic/block-spacing": "error",
			"@stylistic/function-call-spacing": ["error", "never"],
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
			"@stylistic/operator-linebreak": ["error", "after"],
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
			"@stylistic/no-extra-parens": ["error", "all", { "ignoreJSX": "multi-line", "conditionalAssign": false }],
			"no-void": ["off", { "allowAsStatement": true }], // 我就是要使用 void。
			"no-labels": "off",
			"no-label-var": "error",
			"default-case-last": "off",
			"no-useless-constructor": "off", // private constructor() { } 你跟我说无用？
			"@stylistic/no-multiple-empty-lines": ["error", { "max": 1, "maxEOF": 0, "maxBOF": 0 }],
			"@stylistic/max-statements-per-line": "off",
			"no-useless-assignment": "error",
			"no-control-regex": "off",
			"prefer-numeric-literals": "error",
			"@stylistic/generator-star-spacing": ["error", {
				"before": false,
				"after": true,
				"method": { "before": true, "after": false },
			}],
			"prefer-rest-params": "off",
			"no-empty-pattern": "off",
			"no-misleading-character-class": ["error", { "allowEscape": true }],
			"import/order": ["warn", {
				"alphabetize": { "order": "asc", "orderImportKind": "asc", "caseInsensitive": false },
				"named": true,
			}],
			"import/first": "off", // 与 Vue 特性冲突。
			"import/named": "off", // 与 TypeScript 特性冲突。
			"import/no-named-as-default": "off", // 似乎与文件命名方式有点出入。
			"import/no-named-as-default-member": "off", // 某些库在导出成员时用 TS 命名空间欺诈。
			"n/no-callback-literal": "off", // 这是啥？
			"unicorn/escape-case": ["error", "lowercase"],
			"unicorn/number-literal-case": ["error", { "hexadecimalValue": "lowercase" }],
			"unicorn/prefer-code-point": "error",
			"unicorn/better-regex": "off",
			"unicorn/consistent-empty-array-spread": "error",
			"unicorn/consistent-existence-index-check": "error",
			"unicorn/explicit-length-check": "error",
			"unicorn/no-array-push-push": "error",
			"unicorn/no-console-spaces": "warn",
			"unicorn/no-document-cookie": "error",
			"unicorn/prefer-string-replace-all": "error",
			"unicorn/no-useless-length-check": "error",
			"jsdoc/require-jsdoc": ["off", {
				contexts: ["TSDeclareFunction"],
				exemptOverloadedImplementations: true,
				skipInterveningOverloadedDeclarations: true,
			}],
			"jsdoc/tag-lines": "off",
			"jsdoc/require-param": ["error", {
				"enableFixer": false,
				"checkDestructuredRoots": false,
				"exemptedBy": ["inheritdoc", "deprecated", "see"],
			}],
			"jsdoc/check-param-names": ["warn", {
				"checkDestructured": false,
				"allowExtraTrailingParamDocs": true,
				"disableExtraPropertyReporting": true,
			}],
			"jsdoc/check-tag-names": ["error", {
				"definedTags": ["note", "remarks", "memberOf", "category", "warn"],
			}],
			"jsdoc/require-hyphen-before-param-description": ["error", "always", { "tags": { "template": "always" } }],
			"jsdoc/require-returns": ["warn", {
				"checkGetters": false,
				"exemptedBy": ["inheritdoc", "deprecated", "see"],
			}],
			"jsdoc/require-asterisk-prefix": "error",
			"jsdoc/no-multi-asterisks": ["error", { "allowWhitespace": true }],
			// "jsdoc/check-examples": ["error", {
			// 	"exampleCodeRegex": "```",
			// }],
			"jsdoc/require-returns-check": "off",
			"jsdoc/empty-tags": "off",
			"jsdoc/require-template": ["error", {
				"requireSeparateTemplates": true,
				"exemptedBy": ["inheritdoc", "deprecated", "see"],
			}],
			"jsdoc/require-throws": ["error", {
				"exemptedBy": ["inheritdoc", "deprecated", "see"],
			}],
			"jsdoc/require-yields": ["error", {
				"exemptedBy": ["inheritdoc", "deprecated", "see"],
			}],
			"jsdoc/require-throws-type": "warn",
			"jsdoc/require-yields-type": "warn",
			"jsdoc/require-next-type": "warn",
			"jsdoc/require-throws-description": "warn",
			"jsdoc/require-yields-description": "warn",
			"jsdoc/require-next-description": "warn",
			"jsdoc/escape-inline-tags": "off",
			"jsdoc/no-undefined-types": "warn",
			"@typescript-eslint/no-unused-vars": ["warn", { // 非要使用未使用变量，前面加下划线。
				"argsIgnorePattern": "^_",
				"varsIgnorePattern": "^_",
				"caughtErrorsIgnorePattern": "^_",
			}],
			"no-unused-private-class-members": "warn",
			"no-object-constructor": "error",
			"no-array-constructor": "error",
			"@typescript-eslint/no-inferrable-types": ["error", { "ignoreParameters": true, "ignoreProperties": true }],
			"@typescript-eslint/no-non-null-assertion": "off",
			"@typescript-eslint/triple-slash-reference": "off",
			"@typescript-eslint/ban-ts-comment": "off",
			"@typescript-eslint/ban-types": "off",
			"@typescript-eslint/no-namespace": "off",
			"@typescript-eslint/consistent-type-assertions": ["error", { "assertionStyle": "as" }],
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
			"@typescript-eslint/no-use-before-define": ["off", {
				"functions": false,
			}],
			"@typescript-eslint/no-empty-function": "off",
			"@typescript-eslint/no-redeclare": "off", // TypeScript 编译器已支持。
			"@typescript-eslint/no-useless-constructor": "error",
			"@typescript-eslint/no-this-alias": "off",
			"@stylistic/indent-binary-ops": "error",
			"@stylistic/type-generic-spacing": "error",
			"@stylistic/type-named-tuple-spacing": "error",
			"@typescript-eslint/no-confusing-void-expression": "off", // 不是很好用。
			"@typescript-eslint/no-floating-promises": "off", // 不是很好用。
			"@typescript-eslint/no-unnecessary-boolean-literal-compare": "error",
			"@typescript-eslint/strict-boolean-expressions": ["off", { // 如需检查错误时可再临时手动开启。
				"allowString": false,
				"allowNumber": false,
				"allowNullableObject": false,
				"allowNullableBoolean": true,
				"allowNullableString": false,
				"allowNullableNumber": false,
				"allowNullableEnum": false,
				"allowAny": false,
			}],
			"@typescript-eslint/no-unnecessary-type-assertion": "error",
			"@typescript-eslint/no-unnecessary-type-constraint": "error",
			"@typescript-eslint/no-unnecessary-type-parameters": "error",
			"@typescript-eslint/no-unnecessary-type-arguments": "error",
			"@typescript-eslint/no-unnecessary-template-expression": "error",
			"@typescript-eslint/no-unnecessary-qualifier": "off", // 开启后，某些包含复杂类型的特殊文件会把 eslint 弄崩。
			"@typescript-eslint/no-unnecessary-parameter-property-assignment": "error",
			"@typescript-eslint/no-unnecessary-condition": ["off", { "allowConstantLoopConditions": true }],
			"@typescript-eslint/no-empty-object-type": "off",
			"react-refresh/only-export-components": [
				"off",
				{ "allowConstantExport": true },
			],
			"@typescript-eslint/no-unsafe-function-type": "off",
			"@typescript-eslint/no-unused-expressions": ["error", {
				"allowShortCircuit": true,
				"allowTernary": true,
				"enforceForJSX": true,
			}],
			// "@typescript-eslint/prefer-readonly-parameter-types": "error",
			"@typescript-eslint/prefer-reduce-type-parameter": "error",
			"react-hooks/exhaustive-deps": "off",
			"react-hooks/rules-of-hooks": "off",
			"react/jsx-uses-vars": "error",
			"react/react-in-jsx-scope": "off",
			"react/prop-types": "off",
			"react/jsx-no-undef": "off",
			"react/button-has-type": "error",
			"react/jsx-boolean-value": ["error", "never"],
			"react/jsx-key": "error",
			"react/jsx-fragments": ["error", "syntax"],
			"react/no-unknown-property": "off", // 正在使用 TypeScript，不需要你瞎操心。（指不支持 MathML 属性）
			"react/display-name": "warn",
			"@stylistic/jsx-closing-bracket-location": ["error", "tag-aligned"],
			"@stylistic/jsx-closing-tag-location": "error",
			"@stylistic/jsx-curly-spacing": ["error", { "when": "never", "children": true }],
			"@stylistic/jsx-equals-spacing": ["error", "never"],
			"@stylistic/jsx-first-prop-new-line": ["error", "multiline"],
			"@stylistic/jsx-indent-props": ["error", "tab"],
			"@stylistic/jsx-indent": ["error", "tab", { "checkAttributes": true, "indentLogicalExpressions": true }],
			"@stylistic/jsx-max-props-per-line": ["error", { "maximum": { "single": 5, "multi": 1 } }],
			"@stylistic/jsx-pascal-case": ["error", { "allowLeadingUnderscore": true, "allowNamespace": true }],
			"@stylistic/jsx-props-no-multi-spaces": "error",
			"@stylistic/jsx-tag-spacing": ["error", {
				"closingSlash": "never",
				"beforeSelfClosing": "always",
				"afterOpening": "never",
				"beforeClosing": "never",
			}],
			"@stylistic/jsx-one-expression-per-line": "off", // ["error", { allow: "non-jsx" }],
			"react/jsx-one-expression-per-line": "off", // ["error", { allow: "non-jsx" }],
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
				property: "__lookupGetter__",
				message: "Please use Object.getOwnPropertyDescriptor instead.",
			}, {
				property: "__lookupSetter__",
				message: "Please use Object.getOwnPropertyDescriptor instead.",
			}, {
				property: "__proto__",
				message: "Please use Object.getPrototypeOf instead.",
			}, {
				object: "Math",
				property: "pow",
				message: "Please use the exponentiation operator (**) instead.",
			}],
			"no-restricted-globals": ["error", {
				name: "arguments",
				message: "arguments is deprecated.",
			}, {
				name: "isFinite",
				message: "Please use Number.isFinite instead.",
			}, {
				name: "isNaN",
				message: "Please use Number.isNaN instead.",
			}, {
				name: "addEventListener",
				message: "Please use window.addEventListener instead.",
			}, {
				name: "removeEventListener",
				message: "Please use window.removeEventListener instead.",
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
			}, {
				name: "open",
				message: "Please use window.open instead.",
			}, {
				name: "matchMedia",
				message: "Please use window.matchMedia instead.",
			}, {
				name: "print",
				message: "Please use window.print instead.",
			}, {
				name: "length",
				message: "Please use window.length instead.",
			}],
			"no-restricted-syntax": ["error", {
				selector: ":not(ForOfStatement, ForInStatement) > VariableDeclaration[kind = 'let'] > VariableDeclarator[init = null]:not([id.typeAnnotation])",
				message: "Type must be inferred at variable declaration",
			}],
		},
	},
	// ESLint 和 StyleLint 的规则属性名建议加引号以保持统一
	{
		files: ["*.config.{js,ts}"],
		rules: {
			"@stylistic/quote-props": "off",
			"import/order": "off",
		},
	},
	// 类型声明文件中可以用 any
	{
		files: ["**/*.d.ts"],
		rules: {
			"@typescript-eslint/no-explicit-any": "off",
			"@typescript-eslint/no-unused-vars": "off",
			"no-var": "off", // 在 globalThis 中声明成员时必须要用 var（不能使用 let 或 const）！参见：https://stackoverflow.com/a/69429093/19553213
		},
	},
	// JavaScript 源文件需要在 JSDoc 中写类型
	{
		files: ["**/*.{js,jsx}"],
		rules: {
			"jsdoc/check-tag-names": "off",
			"jsdoc/no-types": "off",
			"jsdoc/require-param-type": "error",
			"jsdoc/require-returns-type": "error",
			"jsdoc/require-property-type": "error",
		},
	},
	// 排除列表
	{
		ignores: [
			"dist/*",
			".output/*",
			"node_modules/*",
			"**/dist/*",
			"**/.auto-import/*",
			"**/supportedBrowsers.[jt]s",
		],
	},
];
