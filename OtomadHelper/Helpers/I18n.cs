using System.Globalization;

namespace OtomadHelper.Helpers;

public class I18n : DynamicObject {
	public string SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
		}
	}

	public string Translate(string key) =>
		Properties.Resources.ResourceManager.GetString(key);

	public override bool TryGetMember(GetMemberBinder binder, out object result) {
		result = Translate(binder.Name);
		return result is not null;
	}

	public static readonly dynamic t = new I18n();
}
