using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

public class TransExtension(string key) : MarkupExtension {
	public override object ProvideValue(IServiceProvider serviceProvider) => t[key];
}
