import { watch } from "valtio/utils";

const DEFAULT = [false, "accent"] as BadgeArgs;
const tabs = ["score", "audio", "visual", "sonar", "lyrics", "shupelunker", "ytp", "track"] as const;

export const navBadgeStore = createStore(mapObjectConst(tabs, () => DEFAULT));

watch(get => {
	const c = configStore, b = navBadgeStore;
	get(c);
	const ytpWarn = c.ytp.enabled ? "warning" : "accent";
	b.score = c.ytp.enabled ? [true, ytpWarn] : DEFAULT;
	b.audio = [c.audio.enabled];
	b.visual = [c.visual.enabled];
	b.sonar = [c.sonar.enabled, ytpWarn];
	b.lyrics = [c.lyrics.enabled, ytpWarn];
	b.shupelunker = [c.shupelunker.enabled, ytpWarn];
	b.ytp = [c.ytp.enabled];
	b.track = [c.track.grid.enabled || c.track.concentric.enabled || c.track.box3d.enabled || c.track.gradient.enabled];
});
