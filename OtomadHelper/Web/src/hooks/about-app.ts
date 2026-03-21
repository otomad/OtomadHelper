import { displayName as appName, version } from "web/package.json" with { type: "json" };

export function useAboutApp() {
	return { appName, version };
}

export const APP_NAME = appName;
