import { jotaiStore } from "utils/atom";
import { IN_CONTEXT_LANGUAGE_CODE } from "./jipt-activator_constants";

const _jipt = Object.entries({
	project: "otomadhelper",
	escape() {
		location.search = "";
	},
});

export function activateJipt(enabled: boolean) {
	if (enabled && !globalThis.jipt) {
		globalThis._jipt = _jipt;
		const script = document.createElement("script");
		script.src = "https://cdn.crowdin.com/jipt/jipt.js";
		document.head.append(script);
	} else if (globalThis.jipt)
		globalThis.jipt[enabled ? "start" : "stop"]();
}

const _enableJipt = atom(new URLSearchParams(location.search).has("jipt", "on"));
const enableJipt = atom(
	get => get(_enableJipt),
	(_get, set, value: boolean) => {
		const params = new URLSearchParams(location.search);
		if (value) params.set("jipt", "on");
		else params.delete("jipt");
		history.pushState(null, "", "?" + params.toString());
		set(_enableJipt, value);
	},
);

export function useInContextLocalization() {
	const [enabled, _setEnabled] = useAtom(enableJipt);

	function setEnabled(value: boolean) {
		if (value && !confirm(t.confirm.enableImproveTranslation.toString())) return;
		const languageBackup = i18n.language;
		startColorViewTransition(async () => {
			_setEnabled(value);
			if (value) temporarilyHideRoot();
			activateJipt(value);
			if (value) {
				await i18n.changeLanguage(IN_CONTEXT_LANGUAGE_CODE);
				localStorage.setItem("i18nextLng", languageBackup);
			} else {
				const targetLanguage = globalThis.jipt ? globalThis.jipt.target_language : localStorage.getItem("i18nextLng") ?? "en";
				await i18n.changeLanguage(targetLanguage);
			}
		}, [[{
			opacity: [0, 1],
		}]], {
			duration: 500,
			cursor: "progress",
		});
	}

	return [enabled, setEnabled] as StatePropertyNonNull<boolean>;
}

function temporarilyHideRoot() {
	const root = document.getElementById("root");
	if (!root) return;
	root.hidden = true;
	const observer = new MutationObserver(mutations => {
		if (mutations.flatMap(mutation => [...mutation.addedNodes]).find(node => node instanceof Element && node.id === "crowdin-jipt-mask")) {
			root.hidden = false;
			observer.disconnect();
		}
	});
	observer.observe(document.body, { childList: true });
}

if (jotaiStore.get(enableJipt)) {
	temporarilyHideRoot();
	const languageBackup = i18n.language;
	i18n.changeLanguage(IN_CONTEXT_LANGUAGE_CODE);
	localStorage.setItem("i18nextLng", languageBackup);
	activateJipt(true);
}

// export { IN_CONTEXT_LANGUAGE_CODE }; // DO NOT export or cause a crazy circular dependencies issue.
