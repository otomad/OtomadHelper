using System.Globalization;

namespace OtomadHelper.Helpers;

public class I18n : DynamicObject {
	public static CultureInfo Culture => Thread.CurrentThread.CurrentCulture;

	public static string SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
			Thread.CurrentThread.CurrentUICulture = culture;
			CultureChanged?.Invoke(culture);
		}
	}

	public delegate void CultureChangedEventHandler(CultureInfo culture);
	public static event CultureChangedEventHandler? CultureChanged;

	public string Translate(string key) =>
		Properties.Resources.ResourceManager.GetString(key);

	public override bool TryGetMember(GetMemberBinder binder, out object result) {
		result = Translate(binder.Name);
		return result is not null;
	}

	public static readonly dynamic t = new I18n();
}
