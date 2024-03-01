import type autoImport from "unplugin-auto-import/vite";
type AutoImportOptionsImports = NonNullable<Parameters<typeof autoImport>[0]>["imports"];

export default [
	"react",
	{
		"react": [
			["default", "React"],
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
		],
		"react-i18next": [
			"useTranslation",
		],
		"zustand": [
			["create", "createStore"],
		],
		"zustand/middleware": [
			"persist",
			"subscribeWithSelector",
		],
		"zustand/middleware/immer": [
			["immer", "zustandImmer"],
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
		"lodash-es": [
			["*", "lodash"],
		],
		"react-freezeframe": [
			["default", "ReactFreezeframe"],
		],
	},
	/* {
		from: "react",
		imports: ["CSSProperties", "ChangeEvent", "ChangeEventHandler", "DependencyList", "EventHandler", "MouseEventHandler", "ReactElement", "ReactNode", "RefObject"],
		type: true,
	},
	{
		from: "react-transition-group",
		imports: ["SwitchTransition", "TransitionGroup"], // CSSTransition 与原生类重名。
		type: true,
	}, */
] satisfies AutoImportOptionsImports;
