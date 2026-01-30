using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

public class TransExtension(string key) : MarkupExtension {
	public bool Pangu { get; set; } = true;

	public override object ProvideValue(IServiceProvider serviceProvider) => Pangu ? t[key] : t_disablePangu[key];
}
