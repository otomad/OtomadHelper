/**
 * Check if the browser supports some certain features, if any of them are not supported, then treat the browser is too old.
 * @returns Does the browser support certain features?
 */
export function doesBrowserSupportACertainFeature() {
	return [
		"interpolate-size: allow-keywords",
		"animation-delay: calc(sibling-index() * 250ms)",
		"top: anchor(end)",
	].every(conditionText => CSS.supports(conditionText));
}

type BrowserNameAndUpdateLink = [browserName?: string, updateLink?: string];

/**
 * Get the browser name and the latest version update link (if given). Or get undefined if it doesn't know which is the browser.
 * @param language - Display language.
 * @returns The browser name and the latest version update link (if given).
 */
export function getBrowserName(language: Intl.Locale | Intl.UnicodeBCP47LocaleIdentifier = "en"): BrowserNameAndUpdateLink {
	language = getValidLocale(language) ?? new Intl.Locale("en");
	const userAgent = navigator.userAgent.toLowerCase();
	const matches = (...keywords: string[]) => keywords.some(keyword => userAgent.includes(keyword.toLowerCase()));
	const l9e /* localizable */ = (en: string, zh?: string) => language.language === "zh" && zh ? zh : en;
	const result: string | undefined | BrowserNameAndUpdateLink =
		// Domestic browsers
		matches("Quark") ? l9e("Quark", "夸克") :
		matches("UCBrowser", "UCWeb", "UCMini") ? "UC" :
		matches("MicroMessenger", "WeChat", "Weixin") ? l9e("WeChat", "微信") :
		matches("Weiyun") ? l9e("Weiyun", "微云") :
		matches("QQBrowser") ? "QQ" :
		matches("TencentTraveler") ? "TT" :
		matches("SearchCraft") ? l9e("SearchCraft", "简单搜索") :
		matches("Baidu") ? l9e("Baidu", "百度") :
		matches("Weibo") ? l9e("Weibo", "微博") :
		matches("SE ", "Sogou") ? l9e("Sogou", "搜狗") :
		matches("360EE") ? l9e("360 Speed", "360极速") :
		matches("QIHU", "360SE", "360Browser") ? l9e("360 Secure", "360安全") :
		matches("2345Explorer", "2345Browser", "2345Chrome") ? "2345" :
		matches("LBBrowser", "LieBaoFast") ? l9e("LieBao", "猎豹") :
		matches("ALiSearchApp", "AliApp") ? l9e("Taobao", "淘宝") :
		matches("Via") ? "Via" :
		matches("XiaoMi", "MIUI") ? l9e("Xiaomi", "小米") :
		matches("Huawei", "OpenHarmony", "HarmonyOS") ? l9e("Huawei", "华为") :
		matches("Samsung") ? l9e("Samsung", "三星") :
		matches("VivoBrowser") ? "VIVO" :
		matches("OnePlusBrowser") ? l9e("OnePlus", "一加") :
		matches("OppoBrowser", "HeyTapBrowser") ? "OPPO" :
		matches("MZBrowser") ? l9e("Meizu", "魅族") :
		matches("Cent") ? l9e("Cent", "百分") :
		matches("Vivaldi") ? "Vivaldi" :
		matches("KaiOS Browser") ? "KaiOS" :
		matches("Maxthon", "MxNitro") ? l9e("Maxthon", "傲游") :
		matches("NokiaBrowser", "Symbian") ? l9e("Symbian", "塞班") :
		matches("BingWeb") ? l9e("Bing", "必应") :
		matches("The World") ? l9e("The World", "世界之窗") :
		matches("Avant") ? "Avant" :
		matches("BlackBerry") ? "BlackBerry" :
		matches("Kiwi") ? "Kiwi" :
		matches("Yandex") ? "Yandex" :
		// Major browsers
		matches("Firefox") ? ["Mozilla Firefox", "https://www.mozilla.org/firefox/new"] :
		matches("Opera") ? "Opera" :
		matches("Edge") ? ["Microsoft Edge Legacy", "https://www.microsoft.com/edge/download"] :
		matches("Edg") ? ["Microsoft Edge", "https://www.microsoft.com/edge/download"] :
		matches("Chrome") ? ["Google Chrome", `https://www.google.${language.region === "CN" ? "cn" : "com"}/chrome/index.html`] :
		matches("Safari") ? ["Apple Safari", "https://www.apple.com/safari/"] :
		matches("Trident", "MSIE") ? ["Internet Explorer", "https://www.microsoft.com/edge/download"] :
		undefined;
	return !Array.isArray(result) ? [result, undefined] : result;
}

/**
 * A hook to get the browser name and the latest version update link (if given).
 * @returns The browser name and the latest version update link (if given).
 */
export function useBrowserName() {
	const [language] = useLanguage();
	const browserName = useMemo(() => getBrowserName(language), [language]);
	return browserName;
}
