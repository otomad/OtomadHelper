using System.Windows.Markup;

using Icon = OtomadHelper.WPF.Controls.Icon;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Define <see cref="int"/> number more simply.
/// </summary>
/// <remarks>
/// <example>
/// <para>Before:</para>
/// <code>
/// <![CDATA[
/// <Element>
///     <Element.Property>
///         <s:Int32>1</s:Int32>
///     </Element.Property>
/// </Element>
/// ]]>
/// </code>
/// <para>After:</para>
/// <code>
/// <![CDATA[
/// <Element Property="{m:Int 1}" />
/// ]]>
/// </code>
/// </example>
/// </remarks>
[MarkupExtensionReturnType(typeof(int))]
public class IntExtension(int value) : MarkupExtension {
	[ConstructorArgument("value")]
	public int Value { get; set; } = value;

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}

/// <summary>
/// Define <see cref="double"/> number more simply.
/// </summary>
/// <remarks>
/// <example>
/// <para>Before:</para>
/// <code>
/// <![CDATA[
/// <Element>
///     <Element.Property>
///         <s:Double>3.14</s:Double>
///     </Element.Property>
/// </Element>
/// ]]>
/// </code>
/// <para>After:</para>
/// <code>
/// <![CDATA[
/// <Element Property="{m:Double 3.14}" />
/// ]]>
/// </code>
/// </example>
/// </remarks>
[MarkupExtensionReturnType(typeof(double))]
public class DoubleExtension(int value) : MarkupExtension {
	[ConstructorArgument("value")]
	public double Value { get; set; } = value;

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}

[MarkupExtensionReturnType(typeof(Icon))]
public class IconExtension(string name) : MarkupExtension {
	[ConstructorArgument("name")]
	public string Name { get; set; } = name;

	public override object ProvideValue(IServiceProvider serviceProvider) => new Icon { IconName = Name };
}

[MarkupExtensionReturnType(typeof(bool))]
public class BoolExtension(bool value) : MarkupExtension {
	[ConstructorArgument("value")]
	public bool Value { get; set; } = value;

	public override object ProvideValue(IServiceProvider serviceProvider) => Value;
}
