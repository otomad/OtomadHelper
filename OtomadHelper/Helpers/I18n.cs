using System.Globalization;

namespace OtomadHelper.Helpers;

public static class I18n {
	public static string t_SetCulture {
		set {
			CultureInfo culture = new(value);
			Thread.CurrentThread.CurrentCulture = culture;
		}
	}

	public static string t(string key) {
		return Properties.Resources.ResourceManager.GetString(key);
	}
}
