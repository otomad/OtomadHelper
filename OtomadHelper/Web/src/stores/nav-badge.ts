import { watch } from "valtio/utils";

const DEFAULT = [false, "accent"] as BadgeArgs;

export const navBadgeStore = createStore({
	score: DEFAULT,
	audio: DEFAULT,
	visual: DEFAULT,
	sonar: DEFAULT,
	lyrics: DEFAULT,
	shupelunker: DEFAULT,
	ytp: DEFAULT,
	track: DEFAULT,
});

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
	const layoutEnabledCount = [c.track.grid.enabled, c.track.box3d.enabled, c.track.gradient.enabled].filter(enabled => enabled).length;
	b.track = [layoutEnabledCount, "accent", layoutEnabledCount === 0];
});
