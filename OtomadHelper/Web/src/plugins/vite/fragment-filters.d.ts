declare module "virtual:fragment-filters" {
	import type { FragmentDefaults } from "./fragment-filters";

	const fragmentShaderSource: string;
	export default fragmentShaderSource;

	export const fragNames: string[];
	export const defaults: FragmentDefaults;
}
