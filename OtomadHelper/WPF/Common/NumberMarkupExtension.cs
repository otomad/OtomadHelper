using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

[MarkupExtensionReturnType(typeof(int))]
public class IntExtension : MarkupExtension {
	public IntExtension(int value) => Value = value;

	[ConstructorArgument("value")]
	public int Value { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}

[MarkupExtensionReturnType(typeof(double))]
public class DoubleExtension : MarkupExtension {
	public DoubleExtension(int value) => Value = value;

	[ConstructorArgument("value")]
	public double Value { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
