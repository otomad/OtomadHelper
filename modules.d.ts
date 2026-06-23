declare module "*.module.css" {
	const classes: { [key: string]: string };
	export default classes;
}

declare module "*.css" {
	const stylesheet: CSSStyleSheet;
	export default stylesheet;
}

declare module "*.vue" {
	import type { DefineComponent } from "vue";
	const component: DefineComponent<{}, {}, any>;
	export default component;
}
