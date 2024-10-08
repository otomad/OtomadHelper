using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

public class TranslateExtension(string key) : MarkupExtension {
	public override object ProvideValue(IServiceProvider serviceProvider) =>
		((I18n)t).Translate(key);
}
