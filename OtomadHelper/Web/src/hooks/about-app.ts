import { displayName as appName, version } from "web/package.json";

export function useAboutApp() {
	return { appName, version };
}
