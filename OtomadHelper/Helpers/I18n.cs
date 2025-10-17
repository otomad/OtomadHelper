using System.Globalization;
using System.Resources;

using CultureInfoMatcher;

namespace OtomadHelper.Helpers;

public class I18n : DynamicObject {
	public static readonly CultureInfo SystemCulture = Thread.CurrentThread.CurrentCulture;
	public static CultureInfo Culture { get; private set; } = SystemCulture;
	private static readonly CultureInfo DefaultCulture = new("en-US");
	private static ResXResourceSet CurrentCultureRes { get; set; } = GetCultureRes(SystemCulture);
	private static ResXResourceSet DefaultCultureRes { get; set; } = GetCultureRes(DefaultCulture);

	public static string SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			if (culture.Equals(Culture)) return;
			Culture = culture;
			CurrentCultureRes?.Dispose();
			CurrentCultureRes = GetCultureRes(culture);
			CultureChanged?.Invoke(culture);
		}
	}

	public static void RefreshCulture() {
		Thread.CurrentThread.CurrentCulture = Culture;
		Thread.CurrentThread.CurrentUICulture = Culture;
	}

	private static ResXResourceSet GetCultureRes(CultureInfo culture) {
		const string RESW_EXT = "resw";
		List<CultureInfo> cultures = ResourceHelper.GetEmbeddedResourceNamesInFolder("Strings")
			.Select(path => new CultureInfo(path.Match(new($@"([^\.]+)\.{RESW_EXT}$", RegexOptions.IgnoreCase)).Groups[1].Value))
			.ToList();
		CultureInfo matchedCulture = CultureMatcher.Match(culture, cultures, DefaultCulture);
		Stream stream = ResourceHelper.GetEmbeddedResource($"Strings.{matchedCulture}.{RESW_EXT}");
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
				return new I18n(newParents) { EnablePangu = EnablePangu };
			return $"<{chainedKey}>";
		}
		return EnablePangu ? Pangu.Spacing(result) : result;
	}

	public override bool TryGetMember(GetMemberBinder binder, out object result) {
		result = Translate(binder.Name);
		//return result is not null;
		return true;
	}

	public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
		string? name = indexes.FirstOrDefault() switch {
			string name_string => name_string,
			int name_int => name_int.ToString(),
			_ => null,
		};
		if (name is null) {
			result = null!;
			return false;
		}
		result = Translate(name);
		return true;
	}

	public static readonly dynamic t = new I18n();
	public static readonly dynamic t_disablePangu = new I18n() { EnablePangu = false };

	private IEnumerable<string> Parents { get; set; } = [];

	private I18n() { }
	private I18n(IEnumerable<string> parents) => Parents = parents;

	public bool EnablePangu { get; set; } = true;
}
