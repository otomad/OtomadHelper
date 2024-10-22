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
});

watch(get => {
	get(configStore);
	const ytpWarn = configStore.ytp.enabled ? "warning" : "accent";
	navBadgeStore.score = configStore.ytp.enabled ? [true, ytpWarn] : DEFAULT;
	navBadgeStore.audio = [configStore.audio.enabled];
	navBadgeStore.visual = [configStore.visual.enabled];
	navBadgeStore.sonar = [configStore.sonar.enabled, ytpWarn];
	navBadgeStore.lyrics = [configStore.lyrics.enabled, ytpWarn];
	navBadgeStore.shupelunker = [configStore.shupelunker.enabled, ytpWarn];
	navBadgeStore.ytp = [configStore.ytp.enabled];
});
