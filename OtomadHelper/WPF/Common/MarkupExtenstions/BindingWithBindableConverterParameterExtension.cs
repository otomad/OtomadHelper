using System.Globalization;
using System.Windows;
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
	public BindingMode Mode { get; set; } = BindingMode.TwoWay;
	public dynamic? Converter { get; set; } // IValueConverter or IMultiValueConverter
	public BindingBase? ConverterParameter { get; set; }
	private int BindingCount { get; set; }
	public UpdateSourceTrigger UpdateSourceTrigger { get; set; }
	public bool NotifyOnSourceUpdated { get; set; }

	public BindingExtension() => Binding = new Binding();

	public BindingExtension(string path) => Binding = new Binding(path);

	public BindingExtension(BindingBase binding) => Binding = binding;

	public override object ProvideValue(IServiceProvider serviceProvider) { // PriorityBinding
		MultiBinding multiBinding = new() {
			Mode = Mode,
			UpdateSourceTrigger = UpdateSourceTrigger,
			NotifyOnSourceUpdated = NotifyOnSourceUpdated
		};
		Binding.SetMode(Mode);
		BindingCount = multiBinding.AddBinding(Binding);
		if (ConverterParameter != null) {
			ConverterParameter.SetMode(BindingMode.OneWay);
			multiBinding.AddBinding(ConverterParameter);
		}
		if (Converter != null) {
			MultiValueConverterAdapter adapter = new() { BindingCount = BindingCount };
			if (Converter is IValueConverter) adapter.Converter = Converter;
			else if (Converter is IMultiValueConverter) adapter.MultiConverter = Converter;
			else throw new NotSupportedException($"The converter type `{Converter}` is not supported");
			multiBinding.Converter = adapter;
		}
		return multiBinding.ProvideValue(serviceProvider);
	}

	[ContentProperty(nameof(Converter))]
	private class MultiValueConverterAdapter : IMultiValueConverter {
		internal IValueConverter? Converter { get; set; }
		internal IMultiValueConverter? MultiConverter { get; set; }

		public int BindingCount { private get; init; }

		private object? lastParameter;

		private static object ArrayOrOnly(object[] objects) => objects.Length == 1 ? objects[0] : objects;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			object[] bindingValues = values[0..BindingCount];
			object bindingValue = MultiConverter is null ? ArrayOrOnly(bindingValues) : bindingValues;
			if (values.Length > BindingCount)
				lastParameter = ArrayOrOnly(values[BindingCount..]);
			if (MultiConverter is not null)
				return MultiConverter.Convert(bindingValues, targetType, lastParameter, culture);
			else if (Converter is not null)
				return Converter.Convert(bindingValue, targetType, lastParameter, culture);
			else
				return bindingValue; // Required for VS design-time
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			if (MultiConverter is not null)
				return MultiConverter.ConvertBack(value, targetTypes[0..BindingCount], lastParameter, culture);
			else if (Converter is not null)
				return [Converter.ConvertBack(value, targetTypes[0], lastParameter, culture)];
			else
				return [value]; // Required for VS design-time
		}
	}
}

public class StaticBinding : MarkupExtension {
	public object? Value { get; set; }

	public StaticBinding() { }
	public StaticBinding(object? value) => Value = value;

	public override object ProvideValue(IServiceProvider serviceProvider) {
		Binding binding = new() {
			ConverterParameter = Value,
			Converter = new StaticConverter(),
			Mode = BindingMode.OneTime,
		};
		return binding;
	}

	private class StaticConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => parameter;
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => parameter;
	}
}
