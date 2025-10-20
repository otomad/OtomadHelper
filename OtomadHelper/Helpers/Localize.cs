using System.Globalization;
using System.Resources;

using CultureInfoMatcher;

namespace OtomadHelper.Helpers;

#pragma warning disable IDE1006 // 命名样式

public class Localize : DynamicObject {
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
				return new Localize(newParents) { EnablePangu = EnablePangu };
			return $"<{chainedKey}>";
		}
		return EnablePangu ? Pangu.Spacing(result) : result;
	}

	public string TranslateToString(string key) {
		object translated = Translate(key);
		return (translated as string)!;
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

	private static readonly Localize _t = new();
	private static readonly Localize _t_disablePangu = new() { EnablePangu = false };
	//public static dynamic t => _t;
	//public static dynamic t_disablePangu => _t_disablePangu;
	public static readonly LocalizeGen.Root t = new(_t);
	public static readonly LocalizeGen.Root t_disablePangu = new(_t_disablePangu);

	private IEnumerable<string> Parents { get; set; } = [];

	private Localize() { }
	private Localize(IEnumerable<string> parents) => Parents = parents;

	public bool EnablePangu { get; set; } = true;
}

public abstract class LocalizeNested(Localize localize, string ancestor = "") {
	protected string GetString(string key) => localize.TranslateToString(key);

	public virtual string this[string key] => localize.TranslateToString(ancestor + (string.IsNullOrEmpty(ancestor) ? string.Empty : ".") + key);

	public virtual string this[int index] => this[index.ToString()];
}
