import type autoImport from "unplugin-auto-import/vite";
type AutoImportOptionsImports = NonNullable<Parameters<typeof autoImport>[0]>["imports"];

export default [
	"react",
	{
		"react": [
			["default", "React"],
			["createElement", "h"],
			"createContext",
			"Fragment",
		],
		"react-dom/client": [
			["*", "ReactDOM"],
		],
		"react-dom": [
			"createPortal",
		],
		"styled-components": [
			"styled",
			"keyframes",
			"css",
			"createGlobalStyle",
			"isStyledComponent",
		],
		"classnames": [
			["default", "classNames"],
		],
		"react-transition-group": [
			"SwitchTransition",
			"TransitionGroup",
			"Transition",
			"CssTransition",
		],
		"react-i18next": [
			"useTranslation",
		],
		"valtio": [
			["proxy", "createStore"],
			["subscribe", "subscribeStore"],
			["ref", "valtioRef"],
			"useSnapshot",
		],
		"valtio/utils": [
			["subscribeKey", "subscribeStoreKey"],
			"proxyMap",
			"proxySet",
		],
		"immer": [
			"produce",
		],
		"use-immer": [
			"useImmer",
		],
		"react-flip-move": [
			["default", "FlipMove"],
		],
		"lodash": [ // unplugin-auto-import cannot tree-shaking for lodash-es, causing nearly half of the build time to pack lodash-es.
			["*", "_"],
			["*", "lodash"],
		],
		"styled-tools": [
			["prop", "styledProp"],
			"ifProp",
			"ifNotProp",
			"withProp",
			"switchProp",
		],
	},
	/* {
		from: "react",
		imports: ["CSSProperties", "ChangeEvent", "ChangeEventHandler", "DependencyList", "EventHandler", "MouseEventHandler", "ReactElement", "ReactNode", "RefObject"],
		type: true,
	},
	{
		from: "react-transition-group",
		imports: ["SwitchTransition", "TransitionGroup"], // CSSTransition has the same name as a native class.
		type: true,
	}, */
] satisfies AutoImportOptionsImports;
