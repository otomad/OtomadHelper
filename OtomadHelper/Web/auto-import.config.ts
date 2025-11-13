import type autoImport from "unplugin-auto-import/vite";
type AutoImportOptionsImports = NonNullable<Parameters<typeof autoImport>[0]>["imports"];

export default [
	"react",
	{
		"react": [
			["*", "React"],
			["createElement", "h"],
			"createContext",
			"Fragment",
			"useOptimistic",
			"useEffectEvent",
			"Activity",
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
		"clsx": [
			["default", "classNames"],
		],
		"react-transition-group-fc": [
			"SwitchTransition",
			"TransitionGroup",
			"Transition",
			["CSSTransition", "CssTransition"],
			"tgs",
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
		"jotai": [
			"atom",
			"useAtom",
			"useAtomValue",
			"useSetAtom",
			// ["createStore", "createJotaiStore"],
		],
		"jotai-valtio": [
			"atomWithProxy",
		],
		"jotai-immer": [
			"atomWithImmer",
		],
		"variable-name-conversion": [
			["default", "VariableName"],
		],
		"enum-plus": [
			"Enum",
		],
		"colorjs.io": [
			["default", "Color"],
		],
		/* "@number-flow/react": [
			["default", "NumberFlow"],
		], */
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
