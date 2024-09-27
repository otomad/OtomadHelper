using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Allows Binding to Converter Parameter.
/// </summary>
/// <remarks>
/// Original name: <b><c>BindingWithBindableConverterParameterExtension</c></b><br/>
/// <see href="https://stackoverflow.com/a/60822278/19553213"/>
/// </remarks>
[ContentProperty(nameof(Binding))]
public class BindingExtension : MarkupExtension {
	public BindingBase Binding { get; set; }
	public BindingMode Mode { get; set; }
	public IValueConverter? Converter { get; set; }
	public BindingBase? ConverterParameter { get; set; }
	private int BindingCount { get; set; }

	public BindingExtension() => Binding = new Binding();

	public BindingExtension(string path) => Binding = new Binding(path);

	public BindingExtension(Binding binding) => Binding = binding;

	public override object ProvideValue(IServiceProvider serviceProvider) { // PriorityBinding
		MultiBinding multiBinding = new();
		Binding.SetMode(Mode);
		BindingCount = multiBinding.AddBinding(Binding);
		if (ConverterParameter != null) {
			ConverterParameter.SetMode(BindingMode.OneWay);
			multiBinding.AddBinding(ConverterParameter);
		}
		if (Converter != null) {
			MultiValueConverterAdapter adapter = new() { Converter = Converter, BindingCount = BindingCount };
			multiBinding.Converter = adapter;
		}
		return multiBinding.ProvideValue(serviceProvider);
	}

	[ContentProperty(nameof(Converter))]
	private class MultiValueConverterAdapter : IMultiValueConverter {
		public IValueConverter? Converter { get; set; }

		public int BindingCount { private get; init; }

		private object? lastParameter;

		private static object ArrayOrOnly(object[] objects) => objects.Length == 1 ? objects[0] : objects;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			object bindingValue = ArrayOrOnly(values[0..BindingCount]);
			if (Converter == null)
				return bindingValue; // Required for VS design-time
			if (values.Length > BindingCount)
				lastParameter = ArrayOrOnly(values[BindingCount..]);
			return Converter.Convert(bindingValue, targetType, lastParameter, culture);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			if (Converter == null) return [value]; // Required for VS design-time

			return [Converter.ConvertBack(value, targetTypes[0], lastParameter, culture)];
		}
	}
}
