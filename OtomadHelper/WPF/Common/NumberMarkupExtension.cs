using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

[MarkupExtensionReturnType(typeof(int))]
public class IntExtension(int value) : MarkupExtension {
	[ConstructorArgument("value")]
	public int Value { get; set; } = value;

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}

[MarkupExtensionReturnType(typeof(double))]
public class DoubleExtension(int value) : MarkupExtension {
	[ConstructorArgument("value")]
	public double Value { get; set; } = value;

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
