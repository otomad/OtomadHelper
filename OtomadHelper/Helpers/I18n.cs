using System.Globalization;
using System.Resources;

using Microsoft.CSharp.RuntimeBinder;

using OtomadHelper.Properties;

namespace OtomadHelper.Helpers;

public class I18n : DynamicObject {
	public static CultureInfo Culture { get; private set; } = SystemCulture;
	public static CultureInfo SystemCulture => Thread.CurrentThread.CurrentCulture;

	public static string SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			Culture = culture;
			CultureChanged?.Invoke(culture);
		}
	}

	public static void RefreshCulture() {
		Thread.CurrentThread.CurrentCulture = Culture;
		Thread.CurrentThread.CurrentUICulture = Culture;
	}

	public delegate void CultureChangedEventHandler(CultureInfo culture);
	public static event CultureChangedEventHandler? CultureChanged;

	public string Translate(string key) {
		ResourceManager ResourceManager = Resources.ResourceManager;
		return ResourceManager.GetString(key, Culture) ??
			ResourceManager.GetString(key) ?? // The specified culture missing the string.
			$"<{key}>"; // No such string key.
	}

	public override bool TryGetMember(GetMemberBinder binder, out object result) {
		result = Translate(binder.Name);
		//return result is not null;
		return true;
	}

	public static readonly dynamic t = new I18n();
}
