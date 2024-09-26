using System.Globalization;
using System.Resources;

using OtomadHelper.Properties;

namespace OtomadHelper.Helpers;

public class I18n : DynamicObject {
	public static readonly CultureInfo SystemCulture = Thread.CurrentThread.CurrentCulture;
	public static CultureInfo Culture { get; private set; } = SystemCulture;
	private static readonly CultureInfo DefaultCulture = new("en-US");
	private static ResXResourceSet CurrentCultureRes { get; set; } = GetSuitableCultureRes(SystemCulture);
	private static ResXResourceSet DefaultCultureRes { get; set; } = GetSuitableCultureRes(DefaultCulture);

	public static string SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			if (culture.Equals(Culture)) return;
			Culture = culture;
			CurrentCultureRes?.Dispose();
			CurrentCultureRes = GetSuitableCultureRes(culture);
			CultureChanged?.Invoke(culture);
		}
	}

	public static void RefreshCulture() {
		Thread.CurrentThread.CurrentCulture = Culture;
		Thread.CurrentThread.CurrentUICulture = Culture;
	}

	private static Dictionary<CultureInfo, CultureInfo>? cultureFallbackMap;
	private static ResXResourceSet GetSuitableCultureRes(CultureInfo culture) {
		const string RESW_EXT = "resw";
		List<CultureInfo> cultures = ResourceHelper.GetEmbeddedResourceNamesInFolder("Strings")
			.Select(path => new CultureInfo(path.Match(new Regex($@"([^\.]+)\.{RESW_EXT}$", RegexOptions.IgnoreCase)).Groups[1].Value))
			.ToList();
		CultureInfo suitableCulture;
		if (cultures.Contains(culture)) suitableCulture = culture;
		else if (!cultures.Select(culture => culture.TwoLetterISOLanguageName).ToList().Contains(culture.TwoLetterISOLanguageName)) suitableCulture = DefaultCulture;
		else {
			if (cultureFallbackMap is null) {
				cultureFallbackMap = [];
				foreach (CultureInfo targetCulture in cultures) {
					CultureInfo cultureItem = targetCulture;
					while (!cultureItem.IsReadOnly) {
						cultureItem = cultureItem.Parent;
						if (!cultureFallbackMap.ContainsKey(cultureItem))
							cultureFallbackMap[cultureItem] = targetCulture;
						else
							continue;
					}
				}
			}
			{
				CultureInfo cultureItem = culture;
				while (!cultureItem.IsReadOnly) {
					cultureItem = cultureItem.Parent;
					if (cultureFallbackMap.TryGetValue(cultureItem, out CultureInfo targetCulture)) {
						suitableCulture = targetCulture;
						goto Found;
					}
				}
				// Plan B: Unreachable or bug
				suitableCulture = DefaultCulture;
			Found:
				Unused();
			}
		}
		Stream stream = ResourceHelper.GetEmbeddedResource($"Strings.{suitableCulture}.{RESW_EXT}");
		return new(stream);
	}

	public delegate void CultureChangedEventHandler(CultureInfo culture);
	public static event CultureChangedEventHandler? CultureChanged;

	public object Translate(string key) {
		if (string.IsNullOrWhiteSpace(key)) return "";
		List<string> newParents = Parents.ToList();
		newParents.Add(key);
		string chainedKey = newParents.Join('.');
		/*ResourceManager ResourceManager = Resources.ResourceManager;
		return ResourceManager.GetString(chainedKey, Culture) ??
			ResourceManager.GetString(chainedKey) ?? // The specified culture missing the string.
			$"<{chainedKey}>"; // No such string key.*/
		string? result = CurrentCultureRes.GetString(chainedKey) ??
			DefaultCultureRes.GetString(chainedKey);
		if (result is null) {
			foreach (DictionaryEntry entry in DefaultCultureRes)
				if (entry.Key.ToString().StartsWith(chainedKey + '.'))
					return new I18n(newParents);
			return $"<{chainedKey}>";
		}
		return result;

	}

	public override bool TryGetMember(GetMemberBinder binder, out object result) {
		result = Translate(binder.Name);
		//return result is not null;
		return true;
	}

	public static readonly dynamic t = new I18n();

	private IEnumerable<string> Parents { get; set; } = [];

	private I18n() { }
	private I18n(IEnumerable<string> parents) => Parents = parents;
}
