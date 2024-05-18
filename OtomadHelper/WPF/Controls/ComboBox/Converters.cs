using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

internal class ComboBoxIsCheckedProxy : ObservableObject {
	public string? Current { get; set; }
	public ComboBoxViewModel? ViewModel { get; set; }

	public bool IsChecked {
		get => Current == ViewModel?.Selected;
		set => SetProperty(ViewModel?.Selected, v => ViewModel!.Selected = v!, Current,
			ViewModel is not null && Current is not null && value); // BUG
	}
}

public class ComboBoxStringToIsCheckedConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		(string current, ComboBoxViewModel viewModel) = values.ToTuple<Tuple<string, ComboBoxViewModel>>();
		return new ComboBoxIsCheckedProxy { Current = current, ViewModel = viewModel };
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		Enumerable.Repeat(Binding.DoNothing, targetTypes.Length).ToArray();
}
