declare module "*.mid" {
	import { Midi } from "@tonejs/midi";
	const midi: Midi;
	export default midi;
}

declare module "*.midi" {
	const midi: typeof import("*.mid").default;
	export default midi;
}

declare module "*.mid?keyframes" {
	const midi: {
		tracks: Record<string, [number[], number[], number[], number[]]>;
		length: number;
	};
	export default midi;
}

declare module "*.midi?keyframes" {
	const midi: typeof import("*.mid?keyframes").default;
	export default midi;
}
