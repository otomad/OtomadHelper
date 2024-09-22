using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Allows Binding to Converter Parameter.
/// </summary>
/// <remarks>
/// Original name: <b><c>BindingWithBindableConverterParameter Extension</c></b><br/>
/// <see href="https://stackoverflow.com/a/60822278/19553213"/>
/// </remarks>
[ContentProperty(nameof(Binding))]
public class BindingExtension : MarkupExtension {
	public Binding Binding { get; set; }
	public BindingMode Mode { get; set; }
	public IValueConverter? Converter { get; set; }
	public Binding? ConverterParameter { get; set; }

	public BindingExtension() => Binding = new Binding();

	public BindingExtension(string path) => Binding = new Binding(path);

	public BindingExtension(Binding binding) => Binding = binding;

	public override object ProvideValue(IServiceProvider serviceProvider) {
		MultiBinding multiBinding = new();
		Binding.Mode = Mode;
		multiBinding.Bindings.Add(Binding);
		if (ConverterParameter != null) {
			ConverterParameter.Mode = BindingMode.OneWay;
			multiBinding.Bindings.Add(ConverterParameter);
		}
		if (Converter != null) {
			MultiValueConverterAdapter adapter = new() { Converter = Converter };
			multiBinding.Converter = adapter;
		}
		return multiBinding.ProvideValue(serviceProvider);
	}

	[ContentProperty(nameof(Converter))]
	private class MultiValueConverterAdapter : IMultiValueConverter {
		public IValueConverter? Converter { get; set; }

		private object? lastParameter;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			if (Converter == null)
				return values[0]; // Required for VS design-time
			if (values.Length > 1)
				lastParameter = values[1];
			return Converter.Convert(values[0], targetType, lastParameter, culture);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			if (Converter == null) return [value]; // Required for VS design-time

			return [Converter.ConvertBack(value, targetTypes[0], lastParameter, culture)];
		}
	}
}
